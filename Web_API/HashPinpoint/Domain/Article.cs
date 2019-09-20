using System;

namespace Domain
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool isPublished  { get; set; }
    }
}
