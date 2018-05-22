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
        private static Dictionary<string, Comparison<ABaseEntity>> entityComparisons =
            new Dictionary<string, Comparison<ABaseEntity>>
            {
                { "created", (x,y) => x.Created.CompareTo(y.Created) },
                { "created:asc", (x,y) => x.Created.CompareTo(y.Created) },
                { "created:desc", (x,y) => y.Created.CompareTo(x.Created) },
                { "modified", (x,y) => (x.Modified ?? new DateTime()).CompareTo(y.Modified ?? new DateTime()) },
                { "modified:asc", (x,y) => (x.Modified ?? new DateTime()).CompareTo(y.Modified ?? new DateTime()) },
                { "modified:desc", (x,y) => (y.Modified ?? new DateTime()).CompareTo(x.Modified ?? new DateTime()) }
            };

        // Sort terms:
        // Name, Created, Edited
        private static Dictionary<string, Comparison<User>> userComparisons =
            new Dictionary<string, Comparison<User>>
            {
                { "name", (x,y) => x.Username.CompareTo(y.Username) },
                { "name:asc", (x,y) => x.Username.CompareTo(y.Username) },
                { "name:desc", (x,y) => y.Username.CompareTo(x.Username) },
            };

        // Sort terms:
        // Name, Rating, Owner, NumBookmarks, Private, OwnerName, Created, Edited
        private static Dictionary<string, Comparison<Collection>> collectionComparisons =
            new Dictionary<string, Comparison<Collection>>
            {
                //
            };

        // Sort terms:
        // Title, URL, Collection, Rating, Created, Edited
        private static Dictionary<string, Comparison<Bookmark>> bookmarkComparisons =
            new Dictionary<string, Comparison<Bookmark>>
            {
                //
            };

        public static List<Bookmark> Sort(ref List<Bookmark> list, string sort)
        {
            throw new NotImplementedException();
        }

        public static List<Collection> Sort(ref List<Collection> list, string sort)
        {
            throw new NotImplementedException();
        }

        public static List<User> Sort(ref List<User> list, string sort)
        {
            throw new NotImplementedException();
        }

        private static List<object> Sort(ref List<object> list, string sort)
        {
            throw new NotImplementedException();
        }
        
        private static Comparison<User>[] ParseComparisonListUsers(string sort)
        {
            List<Comparison<User>> list = new List<Comparison<User>();

            string[] terms = sort.Split(',');
            
            foreach(string term in terms)
            {
                if (userComparisons[term] != null)
                {
                    list.Add(userComparisons[term]);
                }
                else if (entityComparisons[term] != null)
                {
                    list.Add(entityComparisons[term]);
                }
            }

            return list.ToArray();
        }

        private static Comparison<Collection>[] ParseComparisonListCollections(string sort)
        {
            List<Comparison<Collection>> list = new List<Comparison<Collection>();

            string[] terms = sort.Split(',');

            foreach (string term in terms)
            {
                if (collectionComparisons[term] != null)
                {
                    list.Add(collectionComparisons[term]);
                }
                else if (entityComparisons[term] != null)
                {
                    list.Add(entityComparisons[term]);
                }
            }

            return list.ToArray();
        }

        private static Comparison<Bookmark>[] ParseComparisonListBookmarks(string sort)
        {
            List<Comparison<Bookmark>> list = new List<Comparison<Bookmark>();

            string[] terms = sort.Split(',');

            foreach (string term in terms)
            {
                if (bookmarkComparisons[term] != null)
                {
                    list.Add(bookmarkComparisons[term]);
                }
                else if (entityComparisons[term] != null)
                {
                    list.Add(entityComparisons[term]);
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
