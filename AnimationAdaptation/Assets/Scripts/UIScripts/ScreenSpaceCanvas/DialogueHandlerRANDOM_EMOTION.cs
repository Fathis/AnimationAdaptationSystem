using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandlerRANDOM_EMOTION : MonoBehaviour {
    private int currentArrayIndex;
    public Text actorName;
    public Text dialogue;
    public int[] speakAnimations; //enthält die Zahlen, die den Sprechanimationen zugeordnet sind
    public int[] idleAnimations; //enthält die Zahlen, die den idle-Animationen zugeordnet sind
    public GameObject speechBubbleContainer;
    public GameObject currentActor;


    // Use this for initialization
    void Start()
    {
        Debug.Log("WARNING: All ANIMATIONS ARE NOW RANDOM!");
        currentArrayIndex = 0;
        speakAnimations = new int[] { 1, 3, 4, 6, 7, 9, 10, 12, 13, 15, 16, 18, 19, 21 };
        idleAnimations = new int[] { 2, 5, 8, 11, 14, 17, 20 };
        actorName = transform.Find("Actor").GetComponent<Text>();
        dialogue = transform.Find("Dialogue").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("return"))
        {
            nextDialoguePiece(currentArrayIndex);
            currentArrayIndex++;
            Debug.Log("CurrentArrayIndex: " + currentArrayIndex);
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
            int i = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsActorIndexArray[pArrayIndex]; // i ist der Index vom aktuellen Actor. Bsp: Actor0 hat den Index 0.
            GameObject actor = GameObject.Find("Actor" + i); //der Actor der gerade am Sprechen ist.
            int j = pArrayIndex - 1; //j gibt nun die vorherige Stelle im Array an, wo die Actorindizes eingetragen sind, zu dem aktuellen Sprachteil. Bsp: sind bei Sprachteil 3, also wird im speakingPartsActorIndexArray geschaut welcher ActorIndex an dieser Stelle steht, bspw. 0 für Actor0.
            //Debug.Log(j);
            GameObject previousActor = null;
            if (j >= 0) //ein vorheriger Actor wird nur gesetzt, wenn es sich nicht um den allerersten (den 0-ten im array) Sprachteil handelt. Da gibt es nämlich keinen vorherigen actor.
            {
                int x = GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsActorIndexArray[j];
                previousActor = GameObject.Find("Actor" + x); //das ist der Actor, der vor dem aktuellen gesprochen hat.
            }
            speechBubbleContainer.GetComponent<BubbleScript>().fillSpeechBubble(actor.GetComponent<ActorScript>().actorSpeakingParts[pArrayIndex]);
            speechBubbleContainer.GetComponent<BubbleScript>().setBubblePosition(actor);
            Debug.Log(GameObject.Find("WorldSpaceCanvas").GetComponent<AnalyseButtonScript>().speakingPartsJSON[pArrayIndex].emotion.emotion);
            controlAnimation(actor, previousActor, pArrayIndex); //setzt die korrekte Animation




        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("The end of the dialogue has been reached");
        }
    }


    /// <summary>
    /// Assigns the correct animation to the current actor and assigns the idle animation to the one that isn't talking.
    /// </summary>
    public void controlAnimation(GameObject pActor, GameObject previousActor, int pIndex)
    {
        System.Random random = new System.Random();
        Animator anim = null; //der Animator der für den aktuellen und auch vorherigen Actor genutzt wird
        if (previousActor != null) //Abfrage soll nur durchgeführt werden, wenn es einen vorherigen Schauspieler gab. Wenn nicht ist davon auszugehen, dass gerade der allererste Sprechteil gesprochen wird.
        {

            anim = previousActor.GetComponent<Animator>();
            anim.SetInteger("Emotion", idleAnimations[random.Next(0, idleAnimations.Length)]);




        }
        anim = pActor.GetComponent<Animator>(); //greift auf den ANimator des aktuellen actors zu

        anim.SetInteger("Emotion", speakAnimations[random.Next(0, speakAnimations.Length)]);

        //findet raus welche Emotion gerade empfunden wird. Muss durch komplexeres System ersetzt werden.



    }
}