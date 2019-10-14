using ProjectInsight.Models.Items;
using ProjectInsight.Models.Users;
using ProjectInsight.WebApi.Client.Searches;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsight.WebApi.Client.Users;
using ProjectInsightMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class SearchesService
    {
        public static SearchClient client;
       

        public SearchesService()
        {
            
        }

        public static async Task<List<ItemSearchResult>> GetItemsByNumberFullOrName(string numberFullOrName, int pageNumber = 1, int pageSize = 10, bool includeExtraResult = false, int? itemType = null)
        {


            try
            {
                List<ItemSearchResult> searchResults = await client.GetItemsByNumberFullOrNameAsync(numberFullOrName,pageNumber, pageSize, includeExtraResult, itemType);
                return searchResults;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return null;

            }

        }
   

    }

}
