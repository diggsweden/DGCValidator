using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services.DGC.ValueSet;
using DGCValidator.Services.Vaccinregler.ValueSet;

namespace DGCValidator.Services
{
    public interface IRestService
    {
        Task<DSC_TL> RefreshTrustListAsync();
        Task<Dictionary<string, string>> RefreshValueSetAsync();
        Task<VaccinRules> RefreshVaccinRulesAsync();
    }
}
