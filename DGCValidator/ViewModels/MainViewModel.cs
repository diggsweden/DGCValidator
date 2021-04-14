using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        String _resultText;
        bool _resultOk;
        SignatureModel _signature;
        SubjectModel _subject;
        ObservableCollection<ICertModel> _certificates;

        private ICommand scanCommand;

        public MainViewModel()
        {
            _subject = new SubjectModel();
            _certificates = new ObservableCollection<ICertModel>();
        }

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand ??
                (scanCommand = new Command(async () => await Scan()));


            }
        }

        private async Task Scan() {
            try
            {
                Clear();

                var scanner = DependencyService.Get<IQRScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    UpdateFields(result);
                }
            }
            catch (Exception ex)
            {
                ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                IsResultOK = false;
            }
        }

        private void UpdateFields(String scanResult)
        {
            try
            {
                if (scanResult != null)
                {
                    var result = VerificationService.VerifyData(scanResult);
                    if (result != null)
                    {
                        DGC proof = result;
                        if (proof != null)
                        {
                            ResultText = AppResources.VaccinatedText;
                            IsResultOK = true;
                            Subject = new Models.SubjectModel
                            {
                                Name = proof.vProof.Sub.N,
                                DateOfBirth = proof.vProof.Sub.Dob,
                                Identifier = proof.vProof.Sub.Id[0].I
                            };
                            Signature = new Models.SignatureModel
                            {
                                IssuedDate = proof.issuedDate,
                                ExpriationDate = proof.expirationDate,
                                IssuerCountry = proof.issuingCountry
                            };

                            if (proof.vProof.Vac != null)
                            {
                                foreach (Vac vac in proof.vProof.Vac)
                                {
                                    AddCertificate(new Models.VaccineCertModel
                                    {
                                        Type = Models.CertEnum.VACCINE,
                                        Adm = vac.Adm,
                                        Aut = vac.Aut,
                                        Dat = vac.Dat,
                                        Des = vac.Des,
                                        Dis = vac.Tar,
                                        Lot = vac.Lot,
                                        Nam = vac.Nam,
                                        Tot = vac.Tot,
                                        Seq = vac.Seq
                                    });
                                }

                            }
                            if (proof.vProof.Tst != null)
                            {
                                foreach (Tst tst in proof.vProof.Tst)
                                {
                                    AddCertificate(new Models.TestCertModel
                                    {
                                        Type = Models.CertEnum.TEST,
                                        Cou = tst.Cou,
                                        Dat = tst.Dat,
                                        Dis = tst.Dis,
                                        Fac = tst.Fac,
                                        Ori = tst.Ori,
                                        Res = tst.Res,
                                        Tma = tst.Tma,
                                        Tna = tst.Tna,
                                        Typ = tst.Typ
                                    });
                                }
                            }
                            if (proof.vProof.Rec != null)
                            {
                                foreach (Rec rec in proof.vProof.Rec)
                                {
                                    AddCertificate(new Models.RecoveredCertModel
                                    {
                                        Type = Models.CertEnum.RECOVERED,
                                        Cou = rec.Cou,
                                        Dat = rec.Dat,
                                        Dis = rec.Dis
                                    });
                                }
                            }
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
                ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                IsResultOK = false;
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

        public bool IsResultOK
        {
            get { return _resultOk; }
            set
            {
                _resultOk = value;
                OnPropertyChanged();
            }
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
            }
        }

        public ObservableCollection<ICertModel> Certs
        {
            get { return _certificates; }
            set
            {
                _certificates = value;
                OnPropertyChanged();
            }
        }
        public void AddCertificate(ICertModel cert)
        {
            _certificates.Add(cert);
            cert.CreateHeaderAndInfo();
            OnPropertyChanged("Certs");
        }
        public void Clear()
        {
            _resultText = null;
            if (_signature != null)
            {
                _signature.Clear();
            }
            if (_subject != null)
            {
                _subject.Clear();
            }
            if (_certificates != null)
            {
                _certificates.Clear();
            }
            OnPropertyChanged("ResultText");
            OnPropertyChanged("Signature");
            OnPropertyChanged("Subject");
            OnPropertyChanged("Certs");
        }

    }

    public class LabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return ((bool)value?Color.Green:Color.Red);
            }
            return Color.Red;
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
}