using System;
namespace DGCValidator.Models
{
    public interface ICertModel
    {
        public CertEnum Type { get; set; }
        public String Header { get; set; }
        public String Info { get; set; }
        public void CreateHeaderAndInfo();
    }
}
