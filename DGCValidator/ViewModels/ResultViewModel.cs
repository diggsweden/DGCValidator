using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.DGC;
using DGCValidator.Services.DGC.V1;
using DGCValidator.Views;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        String _resultText;
        String _resultHeader;
        bool _resultOk = false;
        SignatureModel _signature;
        SubjectModel _subject;
        bool _hasVaccination = false;
        bool _hasTest = false;
        bool _hasRecovered = false;
        ObservableCollection<object> _certs;
        System.Timers.Timer timer;

        private ICommand scanCommand;
        private ICommand cancelCommand;

        public ResultViewModel()
        {
            _subject = new SubjectModel();
            _certs = new ObservableCollection<object>();
        }

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand ??
                (scanCommand = new Command(async () => await Scan()));
            }
        }

        public ICommand CancelCommand => cancelCommand ??
        (cancelCommand = new Command(async () =>
        {
            if( timer != null)
            {
                timer.Stop();
                timer = null;
            }
            MessagingCenter.Send(Application.Current, "Cancel");
        }));

        public async Task Scan()
        {
            Clear();
            try
            {
                var scanner = DependencyService.Get<IQRScanningService>();
                var result = await scanner.ScanAsync();

                if (result != null)
                {
                    UpdateFields(result);
                }
                else
                {
                    MessagingCenter.Send(Application.Current, "Cancel");
                }
            }
            catch (Exception ex)
            {
                ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                IsResultOK = false;
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(200);
                MessagingCenter.Send(Application.Current, "Cancel");
            });
            
        }

        public void UpdateFields(String scanResult)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
            try
            {
                Clear();
                if (scanResult != null)
                {
                    ResultHeader = AppResources.NotApprovedHeader;
                    var result = VerificationService.VerifyData(scanResult);
                    if (result != null)
                    {
                        SignedDGC proof = result;
                        if (proof != null)
                        {
                            Subject = new Models.SubjectModel
                            {
                                Firstname = proof.Dgc.Nam.Gn,
                                Familyname = proof.Dgc.Nam.Fn,
                                TranName = proof.Dgc.Nam.Fnt+"<<"+proof.Dgc.Nam.Gnt,
                                DateOfBirth = proof.Dgc.Dob,
                            };
                            Signature = new Models.SignatureModel
                            {
                                IssuedDate = proof.IssuedDate,
                                ExpirationDate = proof.ExpirationDate,
                                IssuerCountry = proof.IssuingCountry
                            };

                            bool fullyVaccinated = false;

                            if (proof.Dgc.V != null && proof.Dgc.V.Length > 0)
                            {
                                HasVaccinations = true;
                                foreach (VElement vac in proof.Dgc.V)
                                {
                                    if( vac.Dn >= vac.Sd)
                                    {
                                        fullyVaccinated = true;
                                    }
                                     AddCertificate(new Models.VaccineCertModel
                                    {
                                        Type = Models.CertType.VACCINE,
                                        Tg = CodeMapperUtil.GetDiseaseAgentTargeted(vac.Tg),
                                        Vp = CodeMapperUtil.GetVaccineOrProphylaxis(vac.Vp),
                                        Mp = CodeMapperUtil.GetVaccineMedicalProduct(vac.Mp),
                                        Ma = CodeMapperUtil.GetVaccineMarketingAuthHolder(vac.Ma),
                                        Dn = vac.Dn,
                                        Sd = vac.Sd,
                                        Dt = vac.Dt,
                                        Co = vac.Co,
                                        Is = vac.Is,
                                        Ci = vac.Ci
                                    });
                                }

                            }
                            if (proof.Dgc.T != null && proof.Dgc.T.Length > 0)
                            {
                                HasTest = true;
                                foreach (TElement tst in proof.Dgc.T)
                                {
                                    AddCertificate(new Models.TestCertModel
                                    {
                                        Type = Models.CertType.TEST,
                                        Tg = CodeMapperUtil.GetDiseaseAgentTargeted(tst.Tg),
                                        Tt = CodeMapperUtil.GetTestType(tst.Tt),
                                        Ma = CodeMapperUtil.GetTestMarketingAuthHolder(tst.Ma),
                                        Nm = tst.Nm,
                                        Sc = tst.Sc, /*Models.TestCertModel.ConvertFromSecondsEpoc(tst.Sc),*/
                                        //Dr = tst.Dr, /*Models.TestCertModel.ConvertFromSecondsEpoc(tst.Dr),*/
                                        Tr = CodeMapperUtil.GetTestResult(tst.Tr),
                                        Tc = tst.Tc,
                                        Co = tst.Co,
                                        Is = tst.Is,
                                        Ci = tst.Ci
                                    });
                                }
                            }
                            if (proof.Dgc.R != null && proof.Dgc.R.Length > 0)
                            {
                                HasRecovered = true;
                                foreach (RElement rec in proof.Dgc.R)
                                {
                                    AddCertificate(new Models.RecoveredCertModel
                                    {
                                        Type = Models.CertType.RECOVERED,
                                        Tg = CodeMapperUtil.GetDiseaseAgentTargeted(rec.Tg),
                                        Fr = rec.Fr,
                                        Co = rec.Co,
                                        Is = rec.Is,
                                        Df = rec.Df,
                                        Du = rec.Du,
                                        Ci = rec.Ci
                                    });
                                }
                            }

                            List<string> texts = new List<string>();
                            if(_hasVaccination)
                            {
                                if( fullyVaccinated)
                                {
                                    ResultHeader = AppResources.ApprovedHeader;
                                    texts.Add(AppResources.VaccinatedText);
                                    IsResultOK = true;
                                }
                                else
                                {
                                    texts.Add(AppResources.NotFullyVaccinatedText);
                                    IsResultOK = false;
                                }
                            }
                            if(_hasTest)
                            {
                                texts.Add(AppResources.TestedText);
                                IsResultOK = false;
                            }
                            if (_hasRecovered)
                            {
                                texts.Add(AppResources.RecoveredText);
                                IsResultOK = false;
                            }
                            if( !_hasVaccination && !_hasTest && !_hasRecovered)
                            {
                                texts.Add(AppResources.MissingDataText);
                                IsResultOK = false;
                            }
                            if( proof.Message != null)
                            {
                                texts.Add(" " + proof.Message);
                                IsResultOK = false;
                            }

                            ResultText = string.Join(", ", texts.ToArray());
                        }
                        else
                        {

                            ResultText = AppResources.ErrorReadingText;
                            IsResultOK = false;

                        }
                    }
                    else
                    {
                        ResultText = AppResources.ErrorReadingText + ", " + scanResult;
                        IsResultOK = false;

                    }
                }
            }
            catch (Exception ex)
            {
                ResultHeader = AppResources.NotApprovedHeader;
                ResultText = ex.Message;
                IsResultOK = false;
            }
            timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Interval = 60000;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        public bool IsVisible
        {
            get { return (_resultHeader!=null?true:false); }
        }

        public bool HasVaccinations
        {
            get { return _hasVaccination; }
            set
            {
                _hasVaccination = value;
                OnPropertyChanged();
            }
        }

        public bool HasTest
        {
            get { return _hasTest; }
            set
            {
                _hasTest = value;
                OnPropertyChanged();
            }
        }

        public bool HasRecovered
        {
            get { return _hasRecovered; }
            set
            {
                _hasRecovered = value;
                OnPropertyChanged();
            }
        }

        public String ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public String ResultHeader
        {
            get { return _resultHeader; }
            set
            {
                _resultHeader = value;
                OnPropertyChanged();
                OnPropertyChanged("IsVisible");
            }
        }
        public bool IsResultOK
        {
            get { return _resultOk; }
            set
            {
                _resultOk = value;
                if( value)
                {
                    _resultHeader = AppResources.ApprovedHeader;
                }
                else
                {
                    _resultHeader = AppResources.NotApprovedHeader;
                }
                OnPropertyChanged();
                OnPropertyChanged("IsResultNotOK");
                OnPropertyChanged("ResultHeader");
            }
        }
        public bool IsResultNotOK
        {
            get { return !_resultOk; }
        }
        public SignatureModel Signature
        {
            get { return _signature; }
            set
            {
                _signature = value;
                OnPropertyChanged();
            }
        }

        public SubjectModel Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
                OnPropertyChanged("IsVisible");
            }
        }

        public ObservableCollection<object> Certs
        {
            get { return _certs; }
            set
            {
                _certs = value;
                OnPropertyChanged();
            }
        }
        public void AddCertificate(object cert)
        {
            _certs.Add(cert);
            OnPropertyChanged("Certs");
        }
        public void Clear()
        {
            _resultHeader = null;
            _resultText = null;
            if (_signature != null)
            {
                _signature.Clear();
            }
            if (_subject != null)
            {
                _subject.Clear();
            }
            if (_certs != null)
            {
                _certs.Clear();
            }
            _resultOk = false;
            HasVaccinations = false;
            HasTest = false;
            HasRecovered = false;
            OnPropertyChanged("IsVisible");
            OnPropertyChanged("IsResultOK");
            OnPropertyChanged("IsResultNotOK");
            OnPropertyChanged("ResultHeader");
            OnPropertyChanged("ResultText");
            OnPropertyChanged("Signature");
            OnPropertyChanged("Subject");
        }

    }

    public class LabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return ((bool)value ? Color.DarkGreen : Color.DarkRed );
            }
            return "#695F59";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LabelVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ObservableCollection<VaccineCertModel>)
            {
                return ((ObservableCollection<VaccineCertModel>)value).Count > 0 ? true : false;
            }
            if (value is ObservableCollection<TestCertModel>)
            {
                return ((ObservableCollection<TestCertModel>)value).Count > 0 ? true : false;
            }
            if (value is ObservableCollection<RecoveredCertModel>)
            {
                return ((ObservableCollection<RecoveredCertModel>)value).Count > 0 ? true : false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}