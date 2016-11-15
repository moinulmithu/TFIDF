using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermFreqInverse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read a file into a string (this file must live in the same directory as the executable)
            //string filename = "TFIDF.txt";
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = filePath+ @"\TFIDF.txt";
            string filename_2 = filePath + @"\TFIDF_2.txt";
            string inputString = File.ReadAllText(filename);

            // Convert our input to lowercase
            inputString = inputString.ToLower();

            // Define characters to strip from the input and do it
            string[] stripChars = { ";", ",", ".", "-", "_", "^", "(", ")", "[", "]",
                        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "\n", "\t", "\r" };
            foreach (string character in stripChars)
            {
                inputString = inputString.Replace(character, "");
            }

            // Split on spaces into a List of strings
            List<string> wordList = inputString.Split(' ').ToList();

            // Define and remove stopwords
            string[] stopwords = new string[] { "and", "the", "she", "for", "this", "you", "but" };
            foreach (string word in stopwords)
            {              
                while (wordList.Contains(word))
                {
                    wordList.Remove(word);
                }
            }

            // Create a new Dictionary object
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // Loop over all over the words in our wordList...
            foreach (string word in wordList)
            {
                // If the length of the word is at least three letters...
                if (word.Length >= 3)
                {
                    // ...check if the dictionary already has the word.
                    if (dictionary.ContainsKey(word))
                    {
                        // If we already have the word in the dictionary, increment the count of how many times it appears
                        dictionary[word]++;
                    }
                    else
                    {
                        // Otherwise, if it's a new word then add it to the dictionary with an initial count of 1
                        dictionary[word] = 1;
                    }

                } // End of word length check

            } // End of loop over each word in our input

            // Create a dictionary sorted by value (i.e. how many times a word occurs)
            var sortedDict = (from entry in dictionary orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            // Loop through the sorted dictionary and output the top 10 most frequently occurring words
            int count = 1;
            Console.WriteLine("---- Most Frequent Terms in the File: " + filename + " ----");
            Console.WriteLine();
            foreach (KeyValuePair<string, int> pair in sortedDict)
            {
                // Output the most frequently occurring words and the associated word counts
                Console.WriteLine(count + "\t" + pair.Key + "\t" + pair.Value);
                count++;

                // Only display the top 10 words then break out of the loop!
                if (count > 10)
                {
                    break;
                }
            }

            // This part is for cosine similarity
            Console.WriteLine("========Cosine Similarity=============");
            int[] vecA = { 1, 2, 3, 4, 5 };
            int[] vecB = { 6, 7, 7, 9, 10 };

            var cosSimilarity = CalculateCosineSimilarity(vecA, vecB);

            Console.WriteLine(cosSimilarity);            
            Console.ReadKey();

        }
        private static double CalculateCosineSimilarity(int[] vecA, int[] vecB)
        {
            var dotProduct = DotProduct(vecA, vecB);
            var magnitudeOfA = Magnitude(vecA);
            var magnitudeOfB = Magnitude(vecB);

            return dotProduct / (magnitudeOfA * magnitudeOfB);
        }

        private static double DotProduct(int[] vecA, int[] vecB)
        {
            // I'm not validating inputs here for simplicity.            
            double dotProduct = 0;
            for (var i = 0; i < vecA.Length; i++)
            {
                dotProduct += (vecA[i] * vecB[i]);
            }

            return dotProduct;
        }

        // Magnitude of the vector is the square root of the dot product of the vector with itself.
        private static double Magnitude(int[] vector)
        {
            return Math.Sqrt(DotProduct(vector, vector));
        }


    } 

}

