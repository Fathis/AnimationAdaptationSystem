using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainScript : MonoBehaviour {

    public Animator anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void openCurtain()
    {
        GetComponent<BoxCollider>().enabled = false;
        anim.Play("Armature|Open", -1, 0f);
    }
}
