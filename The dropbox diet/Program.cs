using System;
using System.Collections.Generic;
using System.Linq;

namespace The_dropbox_diet
{
    class Program
    {
        static void Main()
        {
            int itemsNo = GetNumberOfItems();

            IEnumerable<DropBoxItem> dropboxItems = GetDropboxItems(itemsNo);

            // generate a solution
            IEnumerable<DropBoxItem> solutionItems = GetSolution(dropboxItems);

            // present the output
            PrintOutput(solutionItems);

            //stop. Hammer time.
            Console.Read();
        }

        #region Methods

        private static int GetNumberOfItems()
        {
            int itemsNo = 0;

            // keep asking until a good number is provided
            while (itemsNo < 1 || itemsNo > 50)
            {
                Console.Write("Enter a number smaller than 50: ");
                int.TryParse(Console.ReadLine(), out itemsNo);
            }

            return itemsNo;
        }

        private static IEnumerable<DropBoxItem> GetDropboxItems(int itemsNo)
        {
            //ask for the items
            Console.WriteLine(Environment.NewLine + "Enter " + itemsNo + " Dropbox items with their corresponding number of calories:");
            
            int itemsRead = 0;
            var dropboxItems = new List<DropBoxItem>();

            while (itemsRead < itemsNo)
            {
                // index
                Console.Write(itemsRead + 1 + ": ");

                string inputString = Console.ReadLine();
                if (!string.IsNullOrEmpty(inputString))
                {
                    inputString = inputString.Trim().ToLower(); //some adjustments
                    
                    string[] parts = inputString.Split(' ');
                    try
                    {
                        dropboxItems.Add(new DropBoxItem
                        {
                            Name = parts[0],
                            NumberOfCalories = int.Parse(parts[1])
                        });
                        itemsRead++;
                    }
                    catch
                    {
                        Console.WriteLine("This is an invalid entry, try again:");
                    }
                }
            }

            return dropboxItems;
        }

        private static IEnumerable<DropBoxItem> GetSolution(IEnumerable<DropBoxItem> dropboxItems)
        {
            // TODO: this can be surely improved

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

                        // we have a solution. Sum it up and go
                        firstSolution.AddRange(positiveCombination);
                        firstSolution.AddRange(negativeCombination);

                        return firstSolution.OrderBy(item => item.Name).ToList(); //order alphabetically
                    }
                }
            }

            return new List<DropBoxItem>();//nothing found
        }

        private static void PrintOutput(IEnumerable<DropBoxItem> solutionItems)
        {
            if (!solutionItems.Any())
            {
                Console.WriteLine(Environment.NewLine + "No solution... :(");
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