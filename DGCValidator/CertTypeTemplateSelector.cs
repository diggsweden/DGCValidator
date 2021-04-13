using System;
using DGCValidator.Models;
using DGCValidator.Views;
using Xamarin.Forms;

namespace DGCValidator
{
    public class CertTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VaccineTemplate { get; set; }
        public DataTemplate TestTemplate { get; set; }
        public DataTemplate RecoveredTemplate { get; set; }

        public CertTypeTemplateSelector()
        {
//            VaccineTemplate = new DataTemplate(typeof(VaccineView));
//            TestTemplate = new DataTemplate(typeof(TestView));
//            RecoveredTemplate = new DataTemplate(typeof(RecoveredView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((ICertModel)item).Type)
            {
                case CertEnum.RECOVERED: return RecoveredTemplate;
                case CertEnum.TEST: return TestTemplate;
                default: return VaccineTemplate;
            }
        }
    }
}
