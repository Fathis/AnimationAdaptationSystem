using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class VRListener : MonoBehaviour {
    public GameObject canvas;

    private void Start()
    {
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
    }

    void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        canvas.GetComponent<DialogueHandler>().controllerDialogueTrigger();

    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
