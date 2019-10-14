using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.Comments;
using ProjectInsight.Models.ReferenceData;
using ProjectInsight.WebApi.Client.Comments;
using ProjectInsight.WebApi.Client.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Services
{

    public class CommentsService
    {
        public static CommentClient client;
        public static async Task<ApiSaveResponse> AddComment(Comment model)
        {
            return await client.SaveAsync(model);
        }

        public static async Task<Comment> Get(Guid itemId)
        {
            return await client.GetAsync(itemId);
        }

        public static async Task<List<Comment>> GetAll(Guid itemId)
        {
            return await client.ListByObjectUserCommentsAsync(itemId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,UserCreated;User:FirstName,LastName,Name,CreatedDateTimeUTC,PhotoUrl,AvatarHtml"));
        }

    }

}
