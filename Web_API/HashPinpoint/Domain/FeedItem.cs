using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class FeedItem
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Link { get; set; }
        public bool isPublished { get; set; }

        //public string Image { get; set; }



    }
}
