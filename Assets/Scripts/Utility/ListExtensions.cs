using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list) =>
            list.Count == 0 ? default : list[Random.Range(0, list.Count)];

        public static List<T> Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = Random.Range(0, n--);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        public static List<T> GetRandomListElements<T>(this List<T> list, int elementsToTake)
        {
            if (list.Count < elementsToTake)
                return default;

            List<T> shuffledList = list.Shuffle();
            return shuffledList.Take(elementsToTake).ToList();
        }
    }
}