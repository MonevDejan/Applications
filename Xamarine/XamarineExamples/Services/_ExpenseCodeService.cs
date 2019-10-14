using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.ReferenceData;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ExpenseCodeService
    {
        public static ExpenseCodeClient client;
        public static async Task<List<ProjectInsight.Models.ReferenceData.ExpenseCode>> GetActive()
        {
            return await client.ListActiveAsync(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default"));
        }
    }

}
