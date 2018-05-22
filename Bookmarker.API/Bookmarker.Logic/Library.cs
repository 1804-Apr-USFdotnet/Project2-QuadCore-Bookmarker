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
        
        public static List<Bookmark> Sort(ref List<Bookmark> list, string sort)
        {
            // Sort terms:
            // AlphaTitle, AlphaURL, Rating, Created, Edited, AlphaCollection
            throw new NotImplementedException();
        }

        public static List<Collection> Sort(ref List<Collection> list, string sort)
        {
            // Sort terms:
            // AlphaName, Rating, Created, Edited, Owner, NumBookmarks, Private, OwnerName
            throw new NotImplementedException();
        }

        public static List<User> Sort(ref List<User> list, string sort)
        {
            // Sort terms:
            // UserName, Created, Edited
            throw new NotImplementedException();
        }

        private static List<object> Sort(ref List<object>, string sort)
        {
            throw new NotImplementedException();
        }

        private static string[][] ParseSortTerms(string sort)
        {
            string[] keys = sort.Split(',');
            string[][] output = new string[keys.Length][];

            for (int i = 0; i < keys.Length; i++)
            {
                output[i] = keys[i].Split(':');
            }

            return output;
        }

    }
}
