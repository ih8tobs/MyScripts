using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject uiObject;
    void Start() //Make the text invisible at first
    {
        uiObject.SetActive(false);
    }

    void OnTriggerEnter(Collider Player) // appear text on trigger enter
    {
        if (Player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
            
            Debug.Log("working");
        }
    }

}
