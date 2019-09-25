using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Domain;

namespace API.Controllers
{
    [Route("api/{controller}/[action]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feedUri = "http://www.dailyherald.com/rss/feed/?feed=news_top10";
            var rssNewsItems = new List<FeedItem>();

            using (var xmlReader = XmlReader.Create(feedUri, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {
                        ISyndicationItem item = await feedReader.ReadItem();
                        rssNewsItems.Add(item.ConvertToNewsItem());
                    }
                }
            }

            var articlesByDate = rssNewsItems.OrderByDescending(p => p.PublishDate).ToArray();
            return Ok(articlesByDate);
        }
    }

    // Extention method
    public static class SyndicationExtensions
    {
        public static FeedItem ConvertToNewsItem(this ISyndicationItem item)
        {
            var article = new FeedItem()
            {
                Title = item.Title,
                Description = item.Description,
                PublishDate = item.Published.UtcDateTime,
                Link = item.Links.First().Uri.AbsoluteUri
            };
            return article;
        }
    }
}








