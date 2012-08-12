using System.Collections.Generic;
using System.Linq;

namespace The_dropbox_diet
{
    public static class Extensions
    {
        /// <summary>
        /// Generates a list of n-sized combinations based on a list of DropboxItems.
        /// Can be extended to any type of items by using generics
        /// </summary>
        /// <param name="dropboxItems">A list of Dropbox items</param>
        /// <param name="combinationSize">The length of a combination</param>
        /// <returns>A collection of possible combinations</returns>
        public static IEnumerable<DropBoxItem[]> Combinations(this IEnumerable<DropBoxItem> dropboxItems, int combinationSize)
        {
            var combinations = new List<DropBoxItem[]>();

            if (combinationSize == 0)
            {
                combinations.Add(new DropBoxItem[0]);
            }
            else
            {
                int current = 1;
                foreach (DropBoxItem item in dropboxItems)
                {
                    combinations.AddRange(dropboxItems.Skip(current++)
                                                      .Combinations(combinationSize - 1)
                                                      .Select(combination => (new[] { item }).Concat(combination).ToArray()));
                }
            }

            return combinations;
        }
    }
}