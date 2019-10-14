using ProjectInsight.WebApi.Client.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class ItemUtilityService
    {
        public static ItemUtilityClient client;
        public static async Task<List<ProjectInsight.Models.Items.RelatedItem>> GetItem(Guid itemId)
        {
            try
            {
                return await client.GetRelatedItemsAsync(itemId, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
