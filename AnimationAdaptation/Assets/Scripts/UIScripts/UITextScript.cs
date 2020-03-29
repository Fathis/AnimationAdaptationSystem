using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextScript : MonoBehaviour {

    public Button male01Button;
    public Button male02Button;
    public Button female01Button;
    public Button female02Button;
    public GameObject currentActor;
    public int actorNumber;
    public GameObject speechBubbleCanvas;
    public bool characterAssignment;
    public GameObject male01;
    public GameObject male02;
    public GameObject female01;
    public GameObject female02;

    // Use this for initialization
    void Start () {
        characterAssignment = false;
        string actorName = currentActor.GetComponent<ActorScript>().getName();
        AddTextToCanvas("How is " + actorName + " supposed to look like?");
        male01Button.gameObject.SetActive(true);
        male01Button.GetComponent<Button>().onClick.AddListener(chooseMale01);
        male02Button.gameObject.SetActive(true);
        male02Button.GetComponent<Button>().onClick.AddListener(chooseMale02);
        female01Button.gameObject.SetActive(true);
        female01Button.GetComponent<Button>().onClick.AddListener(chooseFemale01);
        female02Button.gameObject.SetActive(true);
        female02Button.GetComponent<Button>().onClick.AddListener(chooseFemale02);
        actorNumber = currentActor.GetComponent<ActorScript>().actorIndex;
    }

    /// <summary>
    /// Changes the displayed text on the UI
    /// </summary>
    /// <param name="textString"></param>
    public void AddTextToCanvas(string textString)
    {
        this.gameObject.GetComponent<Text>().text = textString;
        

        return;
    }
    /// <summary>
    /// This gives the actor the appearance of male character #01.
    /// </summary>
    public void chooseMale01()
    {
        currentActor.GetComponent<ActorScript>().gender = false;
        male01 = Instantiate(male01, new Vector3(0, 0, 0), Quaternion.identity); //Actor mit male01-Mesh wird instaziiert
        male01.GetComponent<ActorScript>().actorName = currentActor.GetComponent<ActorScript>().actorName; //actor erhält den Namen unseres Austauschactors
        male01.GetComponent<ActorScript>().actorIndex = currentActor.GetComponent<ActorScript>().actorIndex; //actor erhält den Index vom Austauschactor
        string male01Name = currentActor.name; //Namen zwischenspeichern, damit er nach dem Löschen des Austauschactors noch vorhanden ist.
        male01.GetComponent<ActorScript>().actorSpeakingParts = currentActor.GetComponent<ActorScript>().actorSpeakingParts;
        male01.transform.position = currentActor.transform.position;
        male01.transform.rotation = currentActor.transform.rotation;
        Destroy(currentActor);
        male01.name = male01Name;
        nextActor();
    }


    public void chooseMale02()
    {
        currentActor.GetComponent<ActorScript>().gender = false;
        male02 = Instantiate(male02, new Vector3(0, 0, 0), Quaternion.identity); //Actor mit male01-Mesh wird instaziiert
        male02.GetComponent<ActorScript>().actorName = currentActor.GetComponent<ActorScript>().actorName; //actor erhält den Namen unseres Austauschactors
        male02.GetComponent<ActorScript>().actorIndex = currentActor.GetComponent<ActorScript>().actorIndex; //actor erhält den Index vom Austauschactor
        string male02Name = currentActor.name; //Namen zwischenspeichern, damit er nach dem Löschen des Austauschactors noch vorhanden ist.
        male02.GetComponent<ActorScript>().actorSpeakingParts = currentActor.GetComponent<ActorScript>().actorSpeakingParts;
        male02.transform.position = currentActor.transform.position;
        male02.transform.rotation = currentActor.transform.rotation;
        Destroy(currentActor);
        male02.name = male02Name;
        nextActor();
    }

    /// <summary>
    /// This gives the actor the appearance of female character #01.
    /// </summary>
    public void chooseFemale01()
    {
        currentActor.GetComponent<ActorScript>().gender = true;
        female01 = Instantiate(female01, new Vector3(0, 0, 0), Quaternion.identity); //Actor mit male01-Mesh wird instaziiert
        female01.GetComponent<ActorScript>().actorName = currentActor.GetComponent<ActorScript>().actorName; //actor erhält den Namen unseres Austauschactors
        female01.GetComponent<ActorScript>().actorIndex = currentActor.GetComponent<ActorScript>().actorIndex; //actor erhält den Index vom Austauschactor
        string female01Name = currentActor.name; //Namen zwischenspeichern, damit er nach dem Löschen des Austauschactors noch vorhanden ist.
        female01.GetComponent<ActorScript>().actorSpeakingParts = currentActor.GetComponent<ActorScript>().actorSpeakingParts;
        female01.transform.position = currentActor.transform.position;
        female01.transform.rotation = currentActor.transform.rotation;
        Destroy(currentActor);
        female01.name = female01Name;
        nextActor();
    }

    public void chooseFemale02()
    {
        currentActor.GetComponent<ActorScript>().gender = true;
        female02 = Instantiate(female02, new Vector3(0, 0, 0), Quaternion.identity); //Actor mit female02-Mesh wird instaziiert
        female02.GetComponent<ActorScript>().actorName = currentActor.GetComponent<ActorScript>().actorName; //actor erhält den Namen unseres Austauschactors
        female02.GetComponent<ActorScript>().actorIndex = currentActor.GetComponent<ActorScript>().actorIndex; //actor erhält den Index vom Austauschactor
        string female02Name = currentActor.name; //Namen zwischenspeichern, damit er nach dem Löschen des Austauschactors noch vorhanden ist.
        female02.GetComponent<ActorScript>().actorSpeakingParts = currentActor.GetComponent<ActorScript>().actorSpeakingParts;
        female02.transform.position = currentActor.transform.position;
        female02.transform.rotation = currentActor.transform.rotation;
        Destroy(currentActor);
        female02.name = female02Name;
        nextActor();
    }

    /// <summary>
    /// Switches the current actor with the next one so that the appearance the next actor can be chosen.
    /// </summary>
    public void nextActor()
    {       
        actorNumber++;
        if (GameObject.Find("Actor" + actorNumber) != null) //does the actor even exist?
        {         
            string actorName = GameObject.Find("Actor" + actorNumber).GetComponent<ActorScript>().getName();
            AddTextToCanvas("How is " + actorName + " supposed to look like?");
            currentActor = GameObject.Find("Actor" + actorNumber);
        } else
        {
            male01Button.gameObject.SetActive(false);
            male02Button.gameObject.SetActive(false);
            female01Button.gameObject.SetActive(false);
            female02Button.gameObject.SetActive(false);
            AddTextToCanvas("The stage has been set. Press ENTER to start the scene. \n You can proceed the dialogue by pressing the LEFT GRIP BUTTON.");
            characterAssignment = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp("return") && characterAssignment)
        {
            speechBubbleCanvas.SetActive(true);
            this.gameObject.SetActive(false);
            GameObject.Find("CurtainsLeft").GetComponent<CurtainScript>().openCurtain();
            GameObject.Find("CurtainsRight").GetComponent<CurtainScript>().openCurtain();
        }
    }
}
