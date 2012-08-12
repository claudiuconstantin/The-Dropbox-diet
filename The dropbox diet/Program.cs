using System;
using System.Collections.Generic;
using System.Linq;

namespace The_dropbox_diet
{
    class Program
    {
        static void Main()
        {
            //get the number of items
            int numberOfItems = GetNumberOfItems();

            //get the items
            List<DropBoxItem> dropboxItems = GetDropboxItems(numberOfItems);

            //generate a solution
            List<DropBoxItem> solutionItems = GetSolution(dropboxItems);

            //present the output
            PrintOutput(solutionItems);

            //stop. Hammer time.
            Console.Read();
        }

        #region Methods

        private static int GetNumberOfItems()
        {
            int numberOfItems = 0;

            //keep asking until a good number is provided
            while (numberOfItems < 1 || numberOfItems > 50)
            {
                Console.Write("Enter a number smaller than 50: ");

                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        numberOfItems = int.Parse(input);
                    }
                    catch
                    {
                        //a FormatException can occur
                    }
                }
            }

            return numberOfItems;
        }

        private static List<DropBoxItem> GetDropboxItems(int numberOfItems)
        {
            //ask for the items
            Console.WriteLine(Environment.NewLine + "Enter " + numberOfItems + " Dropbox items with their corresponding number of calories:");

            //read the items
            int itemsRead = 0;
            var dropboxItems = new List<DropBoxItem>();

            while (itemsRead < numberOfItems)
            {
                Console.Write(itemsRead + 1 + ": ");

                string inputString = Console.ReadLine();
                
                if (!string.IsNullOrEmpty(inputString))
                {
                    inputString = inputString.Trim().ToLower();//some adjustments

                    int separatorIndex = inputString.LastIndexOf(' ');

                    try
                    {
                        string name = inputString.Substring(0, separatorIndex).Trim();
                        string calories = inputString.Substring(separatorIndex, inputString.Length - separatorIndex);

                        var item = new DropBoxItem
                                       {
                                           Name = name,
                                           NumberOfCalories = int.Parse(calories)
                                       };

                        dropboxItems.Add(item);

                        itemsRead++;
                    }
                    catch
                    {
                        //a FormatException can occur
                    }
                }
            }
            return dropboxItems;
        }

        private static List<DropBoxItem> GetSolution(List<DropBoxItem> dropboxItems)
        {
            IEnumerable<DropBoxItem> positives = dropboxItems.Where(item => item.NumberOfCalories > 0);
            IEnumerable<DropBoxItem> negatives = dropboxItems.Where(item => item.NumberOfCalories < 0);

            //all positive combinations
            var positiveCombinations = new List<DropBoxItem[]>();
            for (int i = positives.Count(); i > 0; i--)
            {
                positiveCombinations.AddRange(positives.Combinations(i));
            }

            //all negative combinations
            var negativeCombinations = new List<DropBoxItem[]>();
            for (int i = negatives.Count(); i > 0; i--)
            {
                negativeCombinations.AddRange(negatives.Combinations(i));
            }

            //find a match
            foreach (DropBoxItem[] positiveCombination in positiveCombinations)
            {
                foreach (DropBoxItem[] negativeCombination in negativeCombinations)
                {
                    int positiveSum = positiveCombination.Sum(item => item.NumberOfCalories);
                    int negativeSum = negativeCombination.Sum(item => item.NumberOfCalories);

                    if (positiveSum == -negativeSum)
                    {
                        var firstSolution = new List<DropBoxItem>();

                        //we have a solution. Sum it up and go
                        firstSolution.AddRange(positiveCombination);
                        firstSolution.AddRange(negativeCombination);

                        return firstSolution.OrderBy(item => item.Name).ToList();//order alphabetically
                    }
                }
            }

            return new List<DropBoxItem>();//nothing found
        }

        private static void PrintOutput(List<DropBoxItem> solutionItems)
        {
            if (!solutionItems.Any())
            {
                Console.WriteLine(Environment.NewLine + "No solution...");
            }
            else
            {
                Console.WriteLine(Environment.NewLine + "A solution could be this one:");

                foreach (DropBoxItem solution in solutionItems)
                {
                    Console.WriteLine(solution.Name);
                }
            }
        }

        #endregion
    }
}