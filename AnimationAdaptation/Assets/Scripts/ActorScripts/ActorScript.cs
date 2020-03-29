using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorScript : MonoBehaviour {

    public string actorName;
    public bool gender; //true = female, false = male
    public GameObject actorMesh;
    public int actorIndex;
    public string[] actorSpeakingParts;




    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public string getName()
    {
        return actorName;
    }


    public void setPosition(float x, float y, float z)
    {
        this.transform.position = new Vector3(x, y, z);
    }


}
