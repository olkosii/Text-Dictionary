using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Base_course_exercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path to the text file : ");

            //accepts the path to a text file
            string textPath = Console.ReadLine();
            //store all text from text file
            string[] allTextArray;
            try
            {
                allTextArray = File.ReadAllLines(textPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sorry, there was an error : " + ex.Message);
                Environment.Exit(0);
            }
            allTextArray = File.ReadAllLines(textPath);

            //create a hashtable that would store all words from text file
            Hashtable dictionary = new Hashtable();
            for (int i = 0; i < allTextArray.Length; i++)
            {
                //array for store words from current line
                string[] currentLine = allTextArray[i].Split('.', '?', ',',' ',';','/','(',')','+','-');

                for (int j = 0; j < currentLine.Length; j++)
                {
                    //skip an empty word from array
                    if (currentLine[j] == "")
                        continue;

                    //add new word and it's positions to the hashtable(our dictionary)
                    if (dictionary.ContainsKey(currentLine[j]))
                    {
                        var coordinates = new List<int>((List<int>)dictionary[currentLine[j]]);
                        coordinates.Add(i);
                        coordinates.Add(j);

                        dictionary[currentLine[j]] = coordinates;
                    }
                    else
                    {
                        var coordinates = new List<int>() { i, j };

                        dictionary.Add(currentLine[j], coordinates);
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
            Console.WriteLine("Word:    Count of repeated:     Positions of repeated (line position):");
            foreach (DictionaryEntry item in sortedDictionary)
            {
                int h = 1;
                var coordinatesList = new List<int>((List<int>)item.Value);

                Console.Write($"'{item.Key}'");
                Console.Write($"\t\t{coordinatesList.Count / 2}\t\t");

                for (int i = 0; i < coordinatesList.Count - 1; i += 2)
                {
                    Console.Write("  " + "(" + (coordinatesList[i] + h) + " " + (coordinatesList[i + 1] + h) + ")");
                }

                Console.WriteLine("\n");
            }

            //give posibility for the user to find a word from dictionary
            while (true)
            {
                Console.Write("\nPress 'Esc' to exit or 'Enter' to find the word");
                ConsoleKey consoleKey = Console.ReadKey().Key;

                if (consoleKey == ConsoleKey.Escape)
                {
                    break;
                }
                
                Console.Write("\nEnter the word that you want to find: ");
                string findWord = Console.ReadLine();

                //check availability of the word in the dictionary and correct input from user
                if (findWord == null)
                    Console.WriteLine("\nInvalid word format");
                else if (dictionary.ContainsKey(findWord) == false)
                    Console.WriteLine("\nSorry, dictionary don't have this word");
                else if (dictionary.ContainsKey(findWord))
                {
                    var coordinatesList = new List<int>((List<int>)dictionary[findWord]);
                    int one = 1;

                    Console.Write($"\nYour word : '{findWord}' count of repeated : {coordinatesList.Count / 2} " +
                        $"positions of repeated : ");

                    for (int i = 0; i < coordinatesList.Count - 1; i += 2)
                        Console.Write("\n\tline-" + (coordinatesList[i] + one) + " position-" + (coordinatesList[i + 1] + one));
                    Console.WriteLine();
                }
            }
            
        }
    }
}
