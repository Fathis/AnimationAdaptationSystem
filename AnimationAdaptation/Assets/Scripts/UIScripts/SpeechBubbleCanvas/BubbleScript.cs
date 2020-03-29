using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleScript : MonoBehaviour {

    public GameObject speechBubble;
    public TextMeshProUGUI speechBubbleText;
    public BoxCollider speechBubbleBoundingBox;
    public float speechBubbleHeight;

    // Use this for initialization
    void Start () {
        speechBubbleBoundingBox = speechBubble.GetComponent<BoxCollider>();
        


    }
	
	// Update is called once per frame
	void Update () {

        Vector2 size = speechBubbleText.GetComponent<RectTransform>().rect.size;
        speechBubbleBoundingBox.size = size;
 

    }

    public void setBubblePosition(GameObject pActor)
    {
        transform.position = pActor.transform.position;
        calculateDistance(pActor);
    }

    public void setBubbleSize()
    {      
        Vector2 size = speechBubbleText.GetComponent<RectTransform>().rect.size; //new Vector2(width, height);
        speechBubbleBoundingBox.size = size;

    }

    public void fillSpeechBubble(string textString)
    {
        speechBubbleText.text = textString;

    }

    public void calculateDistance(GameObject pActor)
    {
        
        float height = speechBubbleText.GetComponent<RectTransform>().rect.height / 2;
        BoxCollider actorCollider = pActor.GetComponent<BoxCollider>();
        Vector3 colliderPosition = transform.position;
        float actorColliderYSize = actorCollider.size.y;       
        float positiveDistance = actorColliderYSize + height;
        float y = transform.position.y + positiveDistance;
           colliderPosition.y = y;
           transform.position = colliderPosition;
        
        
    }
}
