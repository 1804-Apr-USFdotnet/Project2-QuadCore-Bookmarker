
namespace Models
{
    public class Bookmark : ABaseEntity
    {
        public Bookmark(string name, Collection collection, string url)
        {
            Name = name;
            Collection = collection;
            URL = url;
            Rating = 0;
        }

        public string Name { get; set; }
        public virtual Collection Collection { get; set; }
        public string URL { get; set; }
        public int Rating { get; set; }
    }
}
