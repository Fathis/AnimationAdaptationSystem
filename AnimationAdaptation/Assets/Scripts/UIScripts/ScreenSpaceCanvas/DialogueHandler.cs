using System;
using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {
    private int currentArrayIndex;
    public Text actorName;
    public Text dialogue;
    public GameObject speechBubbleContainer;
    public GameObject currentActor;



    // Use this for initialization
    void Start () {
        currentArrayIndex = 0;
        actorName = transform.Find("Actor").GetComponent<Text>();
        dialogue = transform.Find("Dialogue").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp("return"))
        {
            nextDialoguePiece(currentArrayIndex);
            currentArrayIndex++;
            Debug.Log("CurrentArrayIndex: "+currentArrayIndex);
        }


        if (currentActor != null)
        {
            speechBubbleContainer.GetComponent<BubbleScript>().setBubblePosition(currentActor);
        }
	}

    public void controllerDialogueTrigger()
    {
        nextDialoguePiece(currentArrayIndex);
        currentArrayIndex++;
        Debug.Log("CurrentArrayIndex: " + currentArrayIndex);
    }



    public void nextDialoguePiece(int pArrayIndex)
    {
        try
        {
            int i = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsActorIndexArray[pArrayIndex]; // i is the index of the current Actor. E.g.: Actor0 has the index 0, therefor i is 0.
            GameObject actor = GameObject.Find("Actor"+i); //the currently speaking actor
            currentActor = actor;
            int j = pArrayIndex-1; //j gibt nun die vorherige Stelle im Array an, wo die Actorindizes eingetragen sind, zu dem aktuellen Sprachteil. Bsp: sind bei Sprachteil 3, also wird im speakingPartsActorIndexArray geschaut welcher ActorIndex an dieser Stelle steht, bspw. 0 für Actor0.
            GameObject previousActor = null;
            if(j >= 0) //ein vorheriger Actor wird nur gesetzt, wenn es sich nicht um den allerersten (den 0-ten im array) Sprachteil handelt. Da gibt es nämlich keinen vorherigen actor.
            {
                int x = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsActorIndexArray[j];
            previousActor = GameObject.Find("Actor" + x); //das ist der Actor, der vor dem aktuellen gesprochen hat.
            }
            speechBubbleContainer.GetComponent<BubbleScript>().fillSpeechBubble(actor.GetComponent<ActorScript>().actorSpeakingParts[pArrayIndex]);
            speechBubbleContainer.GetComponent<BubbleScript>().setBubblePosition(actor);
            Debug.Log(GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pArrayIndex].emotion.emotion);
            controlAnimation(actor, previousActor, pArrayIndex); //provides the correct animation




        }
        catch(IndexOutOfRangeException)
        {
            Debug.Log("The end of the dialogue has been reached");
        }
    }



    /// <summary>
    /// Assigns the correct animation to the current actor and assigns the idle animation to the one that isn't talking.
    /// </summary>
    public void controlAnimation(GameObject pActor, GameObject previousActor, int pIndex)
    {
        string previousEmotion = null;
        Animator anim = null; //der Animator der für den aktuellen und auch vorherigen Actor genutzt wird
        double probability = 0.0;
        if (previousActor != null) //Abfrage soll nur durchgeführt werden, wenn es einen vorherigen Schauspieler gab. Wenn nicht ist davon auszugehen, dass gerade der allererste Sprechteil gesprochen wird.
        {
            previousEmotion = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.emotion;
            switch (previousEmotion) //dieser switch-Block prüft welche Emotion guckt welche Emotion am wahrscheinlichsten ist und holt sich den Wahrscheinlichkeitswert in die probability-Variable
            {
                case "Excited":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Excited;
                    break;
                case "Bored":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Bored;
                    break;
                case "Fear":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Fear;
                    break;
                case "Happy":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Happy;
                    break;
                case "Sad":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Sad;
                    break;
                case "Angry":
                    probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex - 1].emotion.probabilities.Angry;
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }
            anim = previousActor.GetComponent<Animator>();
            //Debug.Log(previousActor.name);
            if (probability < 0.25) //wenn die vorherige Emotion unter 25% Wahrscheinlichkeit liegt wird die neutral/calm idle-Animation abgespielt.
            {
                anim.SetInteger("Emotion", 20);

            } else
            {
                switch (previousEmotion) //dieser switch-Block prüft welche Emotion guckt welche Emotion am wahrscheinlichsten ist und holt sich den Wahrscheinlichkeitswert in die probability-Variable
                {
                    case "Excited":
                        anim.SetInteger("Emotion", 11);
                        break;
                    case "Bored":
                        anim.SetInteger("Emotion", 17);
                        break;
                    case "Fear":
                        anim.SetInteger("Emotion", 14);
                        break;
                    case "Happy":
                        anim.SetInteger("Emotion", 2);
                        break;
                    case "Sad":
                        anim.SetInteger("Emotion", 8);
                        break;
                    case "Angry":
                        anim.SetInteger("Emotion", 5);
                        break;
                    default:
                        Debug.Log("Default case");
                        break;
                }
            }



        }
        string emotion = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.emotion; //holt sich die aktuelle Emotion zum aktuellen actor
        probability = 0.0; 
        switch (emotion) //dieser switch-Block prüft welche Emotion guckt welche Emotion am wahrscheinlichsten ist und holt sich den Wahrscheinlichkeitswert in die probability-Variable
        {
            case "Excited":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Excited;
                break;
            case "Bored":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Bored;
                break;
            case "Fear":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Fear;
                break;
            case "Happy":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Happy;
                break;
            case "Sad":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Sad;
                break;
            case "Angry":
                probability = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pIndex].emotion.probabilities.Angry;
                break;
            default:
                Debug.Log("Default case");
                break;
        }
        anim = pActor.GetComponent<Animator>(); //greift auf den ANimator des aktuellen actors zu

        //findet raus welche Emotion gerade empfunden wird. Muss durch komplexeres System ersetzt werden.
        if(probability < 0.25)
        {
            switch (emotion)
            {
                case "Excited":
                    anim.SetInteger("Emotion", 21);
                    break;
                case "Bored":
                    anim.SetInteger("Emotion", 21);
                    break;
                case "Fear":
                    anim.SetInteger("Emotion", 19);
                    break;
                case "Happy":
                    anim.SetInteger("Emotion", 21);
                    break;
                case "Sad":
                    anim.SetInteger("Emotion", 19);
                    break;
                case "Angry":
                    anim.SetInteger("Emotion", 19);
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }
        }
        else
        {
            switch (emotion)
            {
                case "Excited":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 10);
                    }
                    else { anim.SetInteger("Emotion", 10); }
                    break;
                case "Bored":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 16);
                    }
                    else { anim.SetInteger("Emotion", 18); }
                    break;
                case "Fear":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 13);
                    }
                    else { anim.SetInteger("Emotion", 15); }
                    break;
                case "Happy":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 1);
                    }
                    else { anim.SetInteger("Emotion", 3); }
                    break;
                case "Sad":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 7);
                    }
                    else { anim.SetInteger("Emotion", 9); }
                    break;
                case "Angry":
                    if (probability < 0.5)
                    {
                        anim.SetInteger("Emotion", 4);
                    }
                    else { anim.SetInteger("Emotion", 6); }
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }
        }

    }
}
