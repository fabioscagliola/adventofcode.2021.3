using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.fabioscagliola.AdventOfCode202103
{
    class Program
    {
        /// <summary>
        /// Stores the count of 0 bits and 1 bits, and returns the most and the least common 
        /// </summary>
        class BitCounter
        {
            public int CountOf0Bits { get; set; }
            public int CountOf1Bits { get; set; }
            public char MostCommon => CountOf0Bits > CountOf1Bits ? '0' : '1';
            public char LeastCommon => CountOf0Bits > CountOf1Bits ? '1' : '0';

            public void ResetCounter()
            {
                CountOf0Bits = 0;
                CountOf1Bits = 0;
            }

        }

        /// <summary>
        /// Computes the product of two numbers represented as strings in base-2 
        /// </summary>
        class Multiplication
        {
            /// <param name="multiplier">The string representation of the multiplier in base-2</param>
            /// <param name="multiplicand">The string representation of the multiplicand in base-2</param>
            public Multiplication(string multiplier, string multiplicand)
            {
                Multiplier = Convert.ToInt32(multiplier, 2);
                Multiplicand = Convert.ToInt32(multiplicand, 2);
            }

            public int Multiplier { get; protected set; }
            public int Multiplicand { get; protected set; }
            public int Product => Multiplier * Multiplicand;
        }

        static void Main()
        {
            // Read all the lines from the input 
            string[] lineList = File.ReadAllLines("Input1.txt");
            int size = lineList[0].Length;  // I assume that the file contains at least one line 

            // PART 1 
            {
                // Create an array of counters, one for each bit position 
                BitCounter[] bitCount = new BitCounter[size];
                for (int i = 0; i < size; i++)
                    bitCount[i] = new BitCounter();

                // Populate the array of counters counting the bits in all the positions 
                foreach (string line in lineList)
                    for (int i = 0; i < size; i++)
                        if (line[i] == '0')
                            bitCount[i].CountOf0Bits++;
                        else if (line[i] == '1')
                            bitCount[i].CountOf1Bits++;

                // Retrieve the gamma rate and the epsilon rate 
                string gammaRate = null;
                string epsilonRate = null;
                for (int i = 0; i < bitCount.Length; i++)
                {
                    gammaRate += bitCount[i].MostCommon;
                    epsilonRate += bitCount[i].LeastCommon;
                }

                // Compute the power consumption 
                Multiplication powerConsumptionCalculator = new Multiplication(gammaRate, epsilonRate);
                Console.WriteLine($"The power consumption of the submarine is {powerConsumptionCalculator.Product}");
            }

            // PART 2 
            List<string> oxygenGeneratorRatings = new List<string>();
            List<string> carbonDioxideScrubberRatings = new List<string>();

            for (int i = 0; i < size; i++)
            {
                BitCounter bitCount = new BitCounter();

                if (i == 0)  // Populate the lists of oxygen generator ratings and carbon dioxide scrubber ratings based on the bit in the first position 
                {
                    foreach (string line in lineList)
                    {
                        if (line[0] == '0')
                            bitCount.CountOf0Bits++;
                        else if (line[0] == '1')
                            bitCount.CountOf1Bits++;
                    }

                    oxygenGeneratorRatings.AddRange(lineList.ToList().FindAll(x => x[0] == bitCount.MostCommon));
                    carbonDioxideScrubberRatings.AddRange(lineList.ToList().FindAll(x => x[0] == bitCount.LeastCommon));
                }
                else
                {
                    // Remove oxygen generator ratings based on the bit in the subsequent position 
                    foreach (string rating in oxygenGeneratorRatings)
                    {
                        if (rating[i] == '0')
                            bitCount.CountOf0Bits++;
                        else if (rating[i] == '1')
                            bitCount.CountOf1Bits++;
                    }

                    if (oxygenGeneratorRatings.Count != 1)
                        oxygenGeneratorRatings.RemoveAll(rating => lineList.ToList().FindAll(x => x[i] == bitCount.LeastCommon).Contains(rating));

                    // Reset the counter 
                    bitCount.ResetCounter();

                    // Remove carbon dioxide scrubber ratings based on the bit in the subsequent position 
                    foreach (string rating in carbonDioxideScrubberRatings)
                    {
                        if (rating[i] == '0')
                            bitCount.CountOf0Bits++;
                        else if (rating[i] == '1')
                            bitCount.CountOf1Bits++;
                    }

                    if (carbonDioxideScrubberRatings.Count != 1)
                        carbonDioxideScrubberRatings.RemoveAll(rating => lineList.ToList().FindAll(x => x[i] == bitCount.MostCommon).Contains(rating));
                }
            }

            // Compute the life support rating 
            Multiplication lifeSupportRatingCalculator = new Multiplication(oxygenGeneratorRatings[0], carbonDioxideScrubberRatings[0]);
            Console.WriteLine($"The life support rating of the submarine is {lifeSupportRatingCalculator.Product}");
        }

    }
}

