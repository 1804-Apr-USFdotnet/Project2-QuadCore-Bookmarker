using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bookmarker.Models;

namespace Bookmarker.Logic
{
    public static class Library
    {
        // Sort string format: "key1:asc,key2:desc,key3,key4"

        // Sort terms:
        // Date created, Date last modified
        private static readonly Dictionary<string, Comparison<ABaseEntity>> entityComparisons =
            new Dictionary<string, Comparison<ABaseEntity>>
            {
                { "created",        (x,y) => x.Created.CompareTo(y.Created) },
                { "created:asc",    (x,y) => x.Created.CompareTo(y.Created) },
                { "created:desc",   (x,y) => y.Created.CompareTo(x.Created) },

                { "modified",       (x,y) => (x.Modified ?? new DateTime()).CompareTo(y.Modified ?? new DateTime()) },
                { "modified:asc",   (x,y) => (x.Modified ?? new DateTime()).CompareTo(y.Modified ?? new DateTime()) },
                { "modified:desc",  (x,y) => (y.Modified ?? new DateTime()).CompareTo(x.Modified ?? new DateTime()) }
            };

        // Sort terms:
        // User name
        private static readonly Dictionary<string, Comparison<User>> userComparisons =
            new Dictionary<string, Comparison<User>>
            {
                { "name",       (x,y) => x.Username.CompareTo(y.Username) },
                { "name:asc",   (x,y) => x.Username.CompareTo(y.Username) },
                { "name:desc",  (x,y) => y.Username.CompareTo(x.Username) },
            };

        // Sort terms:
        // Name, Rating, Owner's Name, Number of Bookmarks, Private
        private static readonly Dictionary<string, Comparison<Collection>> collectionComparisons =
            new Dictionary<string, Comparison<Collection>>
            {
                { "name",       (x,y) => x.Name.CompareTo(y.Name) },
                { "name:asc",   (x,y) => x.Name.CompareTo(y.Name) },
                { "name:desc",  (x,y) => y.Name.CompareTo(x.Name) },

                { "rating",     (x,y) => y.Rating.CompareTo(x.Rating) },
                { "rating:asc", (x,y) => x.Rating.CompareTo(y.Rating) },
                { "rating:desc",(x,y) => y.Rating.CompareTo(x.Rating) },

                { "owner",      (x,y) => x.Owner.Username.CompareTo(y.Owner.Username) },
                { "owner:asc",  (x,y) => x.Owner.Username.CompareTo(y.Owner.Username) },
                { "owner:desc", (x,y) => y.Owner.Username.CompareTo(x.Owner.Username) },

                { "bookmarks",      (x,y) => y.Bookmarks.Count().CompareTo(x.Bookmarks.Count()) },
                { "bookmarks:asc",  (x,y) => x.Bookmarks.Count().CompareTo(y.Bookmarks.Count()) },
                { "bookmarks:desc", (x,y) => y.Bookmarks.Count().CompareTo(x.Bookmarks.Count()) },

                { "private",        (x,y) => x.Private.CompareTo(y.Private) },
                { "private:asc",    (x,y) => x.Private.CompareTo(y.Private) },
                { "private:desc",   (x,y) => x.Private.CompareTo(y.Private) }
            };

        // Sort terms:
        // Name, URL, Collection, Rating
        private static readonly Dictionary<string, Comparison<Bookmark>> bookmarkComparisons =
            new Dictionary<string, Comparison<Bookmark>>
            {
                { "name",       (x,y) => x.Name.CompareTo(y.Name) },
                { "name:asc",   (x,y) => x.Name.CompareTo(y.Name) },
                { "name:desc",  (x,y) => y.Name.CompareTo(x.Name) },

                { "url",        (x,y) => x.URL.CompareTo(y.URL) },
                { "url:asc",    (x,y) => x.URL.CompareTo(y.URL) },
                { "url:desc",   (x,y) => y.URL.CompareTo(x.URL) },

                { "collection",         (x,y) => x.Collection.Name.CompareTo(y.Collection.Name) },
                { "collection:asc",     (x,y) => x.Collection.Name.CompareTo(y.Collection.Name) },
                { "collection:desc",    (x,y) => y.Collection.Name.CompareTo(x.Collection.Name) },

                { "rating",         (x,y) => y.Rating.CompareTo(x.Rating) },
                { "rating:asc",     (x,y) => x.Rating.CompareTo(y.Rating) },
                { "rating:desc",    (x,y) => y.Rating.CompareTo(x.Rating) }
            };

        public static void Sort(ref List<User> list, string sort)
        {
            var comparitors = ParseComparisonListUsers(sort);
            var aggregateComparitor = CombineComparisons(comparitors);
            list.Sort(aggregateComparitor);
        }
        public static void Sort(ref List<Collection> list, string sort)
        {
            var comparitors = ParseComparisonListCollections(sort);
            var aggregateComparitor = CombineComparisons(comparitors);
            list.Sort(aggregateComparitor);
        }
        public static void Sort(ref List<Bookmark> list, string sort)
        {
            var comparitors = ParseComparisonListBookmarks(sort);
            var aggregateComparitor = CombineComparisons(comparitors);
            list.Sort(aggregateComparitor);
        }
        
        private static Comparison<User>[] ParseComparisonListUsers(string sort)
        {
            List<Comparison<User>> list = new List<Comparison<User>>();

            string[] terms = sort.Split(',');
            
            foreach(string term in terms)
            {
                try
                {
                    list.Add(userComparisons[term]);
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        list.Add(entityComparisons[term]);
                    }
                    catch (KeyNotFoundException) { }
                }
            }

            return list.ToArray();
        }
        private static Comparison<Collection>[] ParseComparisonListCollections(string sort)
        {
            List<Comparison<Collection>> list = new List<Comparison<Collection>>();

            string[] terms = sort.Split(',');

            foreach (string term in terms)
            {
                try
                {
                    list.Add(collectionComparisons[term]);
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        list.Add(entityComparisons[term]);
                    }
                    catch (KeyNotFoundException) { }
                }
            }

            return list.ToArray();
        }
        private static Comparison<Bookmark>[] ParseComparisonListBookmarks(string sort)
        {
            List<Comparison<Bookmark>> list = new List<Comparison<Bookmark>>();

            string[] terms = sort.ToLower().Split(',');

            foreach (string term in terms)
            {
                try
                {
                    list.Add(bookmarkComparisons[term]);
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        list.Add(entityComparisons[term]);
                    }
                    catch (KeyNotFoundException) { }
                }
            }

            return list.ToArray();
        }

        private static Comparison<T> CombineComparisons<T>(Comparison<T>[] comparisons)
        {
            return delegate(T a, T b)
            {
                int output = 0;
                foreach (Comparison<T> comp in comparisons)
                {
                    output = comp(a, b);
                    if (output != 0)
                    {
                        return output;
                    }
                }
                return output;
            };
        }
    }
}
