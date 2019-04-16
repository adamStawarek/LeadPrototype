using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadPrototype.Libs.Helpers
{
    public static class ListExtensions
    {
        public static IEnumerable<int> GetNIndexesOfBiggestValues(this List<float> list, int n)
        {
            var indexes = new Dictionary<int,float>();
            for (int i = 0; i < list.Count; i++)
            {               
                if(indexes.Count<n)
                {
                    indexes.Add(i,list[i]);
                    indexes = indexes.OrderBy(idx => idx.Value).ToDictionary(idx => idx.Key, idx => idx.Value);
                    continue;                    
                }
                if (list[i] > indexes.Values.First())
                {
                    indexes.Remove(indexes.Keys.First());
                    indexes.Add(i,list[i]);
                }
                indexes = indexes.OrderBy(idx => idx.Value).ToDictionary(idx => idx.Key, idx => idx.Value);
            }

            return indexes.Keys.Take(n);
        }

        public static bool AreEquivalent(this List<float> list, List<float> list2)//not good for large collections
        {
            if (list.Count != list2.Count) return false;
            list.Sort();
            list2.Sort();
            return !list.Where((t, i) => Math.Abs(t - list2[i]) > 0.01).Any();
        }
    }
}
