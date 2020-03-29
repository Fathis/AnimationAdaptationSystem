using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParallelDots;
using System.Diagnostics;

namespace EmotionAnalysis
{

    class Program
    {
        public static string filePath; //path for the text file
        public static ArrayList speakingPartsList;
        static paralleldots emotionAI = new paralleldots(""); //Insert API key here.



        static void Main(string[] args)
        {
            speakingPartsList = new ArrayList();
            filePath = @"D:\Studium\Bachelorarbeit\AnimationAdaptation\Assets\Text\Script.txt";
            partitionSpeakingParts();
            cleanUp();
            createJSONFiles();
            
    
               
            }


        public static void createJSONFiles()
        {
            int i = 0;
            string fileName = "000" + ".json"; //= i + ".json";
            foreach (string speakingPart in speakingPartsList)
            {
                if(Math.Floor(Math.Log10(i) + 1) == 1)
                {
                    fileName = "00"+ i + ".json";
                }
                else if (Math.Floor(Math.Log10(i) + 1) == 2)
                {
                    fileName = "0" + i + ".json";
                }
                else if (Math.Floor(Math.Log10(i) + 1) == 3)
                {
                    fileName = i + ".json";
                }
                //fileName = i + ".json";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                System.IO.File.WriteAllText(@"D:\Studium\Bachelorarbeit\AnimationAdaptation\Assets\Text\JSONFiles\" + fileName, emotionAI.emotion(speakingPart));
                i++;

            }
        }


        /// <summary>
        /// Partitions the speaking parts into separate strings and puts them into an array.
        /// </summary>
        static void partitionSpeakingParts()
        {
            //Read the text from the test.txt file
            StreamReader reader = new StreamReader(filePath);
            string currentSpeakingPart = null;
            string currentLine = null;

            while (reader.Peek() >= 0) //solange im Text noch ein gültiges Satzzeichen folgt
            {
                currentLine = reader.ReadLine();
                currentSpeakingPart += currentLine;
                //falls eine Leerzeile entdeckt wird, wird der bisherige Stringinhalt ins Arrays eingefügt und der string für einen neuen Durchlauf geleert
                if (currentLine.Length == 0 && currentSpeakingPart.Length != 0) //prevents the inclusion of empty lines as speaking parts
                {
                    speakingPartsList.Add(currentSpeakingPart);
                    currentSpeakingPart = null;
                }

            }

            if (currentSpeakingPart != null && currentSpeakingPart.Length != 0) // &&-check because it currenSpeakingPart.Length throws NullPointerException if it is null
            {
                speakingPartsList.Add(currentSpeakingPart); //damit auch der letzte Sprachanteil ins Array geschrieben wird
            }

            reader.Close();
        }

        public static ArrayList getActorNames()
        {
            ArrayList actorNamesArrayList = new ArrayList();
            string actorName;
            foreach (string speakingPart in speakingPartsList)
            {
                actorName = null;
                foreach (char charactersInName in speakingPart)
                {
                    if (charactersInName != ':') //Everything before a : is considered part of the name
                    {
                        actorName += charactersInName;
                    }
                    else
                    {
                        if (!actorNamesArrayList.Contains(actorName))
                        {
                            actorNamesArrayList.Add(actorName);
                            break;
                        }
                    }
                }

            }
            return actorNamesArrayList;
        }


        public static void cleanUp()
        {
            ArrayList actorNames = getActorNames();
            foreach(string actorName in actorNames)
            {
                string speakingPart = null;
                for (int i = 0; i < speakingPartsList.Count; i++)
                {
                    speakingPart = (string)speakingPartsList[i];
                    if (speakingPart.Contains(actorName + ":")) //to avoid that it mistakenly uses a name said by another character.
                    {
                        speakingPart = speakingPart.Replace(actorName + ":", "");
                        speakingPartsList[i] = speakingPart;
                    }

                    speakingPartsList[i] = speakingPart;                 

                }
            }

        }


    }
}
