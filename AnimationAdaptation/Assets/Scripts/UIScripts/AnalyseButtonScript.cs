using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class AnalyseButtonScript : MonoBehaviour
{

    public Button textScriptAnalysisButton;
    public Text InstructionText; //the text object that asks the player to choose the actors 
    public string path; //path for the text file
    public ArrayList speakingPartsList;
    ArrayList actorNames;
    public int[] speakingPartsActorIndexArray; //use this to see which actor is currently speaking
    public GameObject instantiateActor;
    public SpeakingPartClass[] speakingPartsJSON;


    // Use this for initialization
    void Start()
    {
        speakingPartsList = new ArrayList();
        textScriptAnalysisButton.onClick.AddListener(TaskOnClick);
        path = Application.dataPath + "/Text/Script.txt";

    }


    void TaskOnClick()
    {
        
        partitionSpeakingParts();
        textScriptAnalysisButton.gameObject.SetActive(false);
        actorNames = getActorNames();
        instantiateActors();
        fillSpeakingPartsJSONList(Application.dataPath + "/Text/JSONFiles/");
        InstructionText.gameObject.SetActive(true); //activates the text that asks the player to choose the actor's appearance
        GameObject.Find("InstructionText").GetComponent<UITextScript>().currentActor = GameObject.Find("Actor0");
        GameObject.Find("Actor0").GetComponent<ActorScript>().setPosition(-1.68f, -2.577f, -12.52f);
        GameObject.Find("Actor0").GetComponent<Transform>().Rotate(0.0f, 90.0f, 0.0f);
        GameObject.Find("Actor1").GetComponent<ActorScript>().setPosition(1.28f, -2.577f, -12.52f);
        GameObject.Find("Actor1").GetComponent<Transform>().Rotate(0.0f, -90.0f, 0.0f);








    }

    /// <summary>
    /// Splits the script up into the individual speaking parts of the actors.
    /// </summary>
    void partitionSpeakingParts()
    {
        //Read the text from the test.txt file
        StreamReader reader = new StreamReader(path);
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

        Debug.Log("Amount of speaking parts: " + speakingPartsList.Count);
        //set size for the sentiment arrays
        speakingPartsActorIndexArray = new int[speakingPartsList.Count];
        reader.Close();
    }


    void printSpeakingParts()
    {
        int size = speakingPartsList.Count;
        for (int i = 0; i < size; i++)
        {
            Debug.Log(speakingPartsList[i]);
        }
    }



    /// <summary>
    /// Sets the name of a given actor.
    /// </summary>
    /// <param name="pActorToBeNamed">The actor that is supposed to be named</param>
    /// <param name="actorIndex">Right now this only means the index in the speaking parts array in which the name should be looked for</param>
    public void setActorNames(GameObject pActorToBeNamed, int actorIndex)
    {
        string analyseSpeakingPart;
        string actorName = null;
        if (pActorToBeNamed != null)
        {
            analyseSpeakingPart = (string)speakingPartsList[actorIndex];
            foreach (char charactersInName in analyseSpeakingPart)
            {
                if (charactersInName != ':') //Everything before a : is considered part of the name
                {
                    actorName += charactersInName;
                }
                else
                {
                    break;
                }
            }
        }
        pActorToBeNamed.GetComponent<ActorScript>().actorName = actorName;
    }

    /// <summary>
    /// Goes through the speakingPartsList and checks how many actor names can be found.
    /// </summary>
    /// <returns>An array list with the names of the actors in the scene</returns>
    ArrayList getActorNames()
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

    void instantiateActors()
    {

        int arrayIndex = 0;
        foreach (string actorName in actorNames)
        {
            instantiateActor = Instantiate(instantiateActor, new Vector3(0, 0, 0), Quaternion.identity);
            instantiateActor.GetComponent<ActorScript>().actorName = actorName;
            instantiateActor.GetComponent<ActorScript>().actorIndex = arrayIndex;
            instantiateActor.name = "Actor" + arrayIndex;
            instantiateActor.GetComponent<ActorScript>().actorSpeakingParts = new string[speakingPartsList.Count];
            arrayIndex++;
            cleanUp(instantiateActor); //removes the names from the speaking parts and puts them in the designated array

        }

    }

    /// <summary>
    /// This method removes the names of the actors from the speaking parts strings so that they can later be used in the dialog.
    /// It also fills the speaking parts arrays of each actor with their respective speaking parts.
    /// Furthermore it fills the speakingPartsActorIndexArray so that it can be used to determine when a certain actor is speaking.
    /// </summary>
    public void cleanUp(GameObject pActor)
    {
        string speakingPart = null;
        string actorName = pActor.GetComponent<ActorScript>().getName();
        for (int i = 0; i < speakingPartsList.Count; i++)
        {
            speakingPart = (string)speakingPartsList[i];
            if (speakingPart.Contains(actorName + ":")) //to avoid that it mistakenly uses a name said by another character.
            {
                speakingPart = speakingPart.Replace(actorName + ":", "");
                pActor.GetComponent<ActorScript>().actorSpeakingParts[i] = speakingPart;
                speakingPartsActorIndexArray[i] = pActor.GetComponent<ActorScript>().actorIndex; //Fills the array in the order in which the actors speak
            }

            speakingPartsList[i] = speakingPart;

        }

    }

    void fillSpeakingPartsJSONList(string workingDirectory)
    {
        int i = 0;
        speakingPartsJSON = new SpeakingPartClass[speakingPartsList.Count];
        foreach (string filePath in Directory.GetFiles(workingDirectory, "*.json"))
        {
            string contents = File.ReadAllText(filePath);
            speakingPartsJSON[i]=(JsonUtility.FromJson<SpeakingPartClass>(contents));
            i++;
        }
    }



    /**
     * Here start the JSON classes for the emotions
     **/

    [System.Serializable]
    public class Probabilities
    {
        public double Bored;
        public double Sad;
        public double Fear;
        public double Angry;
        public double Excited;
        public double Happy;
    }

    [System.Serializable]
    public class Emotion
    {
        public string emotion;
        public Probabilities probabilities;
    }

    [System.Serializable]
    public class SpeakingPartClass
    {
        public Emotion emotion;
        public int code;
    }






    // Update is called once per frame
    void Update()
    {

    }
}
