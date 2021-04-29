using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services.DGC.ValueSet;

namespace DGCValidator.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public async Task<DSC_TL> RefreshTrustListAsync()
        {
            DSC_TL trustList = new DSC_TL();
            Uri uri = new Uri(string.Format(Constants.RestUrl, "dsctl"));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    trustList = DSC_TL.FromJson(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return trustList;
        }

        public async Task<Dictionary<string, ValueSet>> RefreshValueSetAsync()
        {
            Dictionary<string, ValueSet> valueSets = new Dictionary<string, ValueSet>();
            Uri uri = new Uri(string.Format(Constants.RestUrl, "valusets"));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    valueSets = ValueSet.FromJson(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return valueSets;
        }

    }

    public class Constants
    {
        public static string RestUrl = "http://localhost:8080/dgc/";
        //public static string RestUrl = "https://digg.se/dgc/";
    }
}
