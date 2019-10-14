using ProjectInsight.Models.Base.Responses;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ExpenseEntryService
    {
        public static ExpenseEntryClient client;



        public static async Task<ProjectInsight.Models.TimeAndExpense.ExpenseEntry> Get(Guid guid)
        {
            try
            {
                return await client.GetAsync(guid, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,User,UserCreated,Project,Company,Department,Task,ExpenseCode,Qty,UnitPrice,ActualCost"));

            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }

}
