using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Base_course_exercise
{
    internal class Program
    {
        public static List<int> findCoordinates(string [] textLine,string searchWord,int currentLine)
        {
            var coordinates = new List<int>();

            for (int j = 0; j < textLine.Length; j++)
            {
                if (searchWord == textLine[j])
                {
                    coordinates.Add(currentLine);
                    coordinates.Add(j);
                    break;
                }
            }

            return coordinates;
        }
        static void Main(string[] args)
        {
            Console.Write("Enter the path to the text file : ");

            //accepts the path to a text file
            string textPath = Console.ReadLine();
            //store all text from text file
            string[] allTextArray = File.ReadAllLines(textPath);

            Hashtable dictionary = new Hashtable();
            for (int i = 0; i < allTextArray.Length; i++)
            {
                //array for store words from current line
                string[] currentLine = allTextArray[i].Split();

                foreach (var word in currentLine)
                {
                    //skip an empty word from array
                    if (word == "")
                        continue;
                    
                    //add new word and it's positions to the hashtable(our dictionary)
                    if (dictionary.ContainsKey(word))
                    {
                        var coordinates = findCoordinates(currentLine, word, i);

                        foreach (var coordinate in (List<int>)dictionary[word])
                        {
                            coordinates.Add(coordinate);
                        }
                        dictionary[word] = coordinates;
                    }
                    else
                    {
                        var coordinates = findCoordinates(currentLine, word, i);

                        dictionary.Add(word, coordinates);
                    }
                }
            }

            //creat <DictionaryEntry> in order to store sorted entry from dictionary
            var sortedDictionary = new List<DictionaryEntry>(dictionary.Count);
            foreach (DictionaryEntry entry in dictionary)
                sortedDictionary.Add(entry);
            //sort <DictionaryEntry>
            sortedDictionary.Sort(
                 (x, y) =>
                 {
                     List<int> list1 = (List<int>)x.Value;
                     List<int> list2 = (List<int>)y.Value;

                     IComparable comparable = list1.Count as IComparable;
                     if (comparable != null)
                     {
                         return comparable.CompareTo(list2.Count);
                     }
                     return 0;
                 });

            sortedDictionary.Reverse();
            //display statistic about every word from dictionary
            Console.WriteLine("Your Dictionary :");
            foreach (DictionaryEntry item in sortedDictionary)
            {
                int h = 1;
                var coordinatesList = new List<int>((List<int>)item.Value);

                Console.Write($"Word : '{item.Key}' count of repeated : {coordinatesList.Count/2} " +
                    $"the positions of occurrences in the text : ");

                for (int i = 0; i < coordinatesList.Count - 1; i+= 2)
                {
                    Console.Write("\n\tline-" + (coordinatesList[i] + h) + " position-" + (coordinatesList[i + 1] + h));
                }

                Console.WriteLine();
            }

            //give posibility for the user to find a word from dictionary
            Console.Write("\nEnter the word that you want to find : ");
            string findWord = Console.ReadLine();

            //check availability of the word in the dictionary and correct input from user
            if (findWord == null)
                Console.WriteLine("Invalid word format");
            else if(dictionary.ContainsKey(findWord) == false)
                Console.WriteLine("Sorry, dictionary don't have this word");
            else if (dictionary.ContainsKey(findWord))
            {
                var coordinatesList = new List<int>((List<int>)dictionary[findWord]);
                int one = 1;

                Console.Write($"Your word : '{findWord}' count of repeated : {coordinatesList.Count/2} " +
                    $"the positions of occurrences in the text : " );

                for (int i = 0; i < coordinatesList.Count - 1; i += 2)
                    Console.Write("\n\tline-" + (coordinatesList[i] + one) + " position-" + (coordinatesList[i + 1] + one));
            }

            
            Console.ReadLine();
        }
    }
}
