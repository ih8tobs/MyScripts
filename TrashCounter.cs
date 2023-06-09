using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashCounter : MonoBehaviour
{

    public int Amount;
    public int TrashDestroyed;
    public int TrashThreshold = 10;
    public TMP_Text TrashText;
    

    HealthManager HealingScript;

    private void Start()
    {
        // Subscribe to the event when a GameObject is destroyed
        DestroyedEventManager.OnObjectDestroyed += HandleObjectDestroyed;

        HealingScript = GameObject.Find("Player").GetComponent<HealthManager>();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this script is destroyed
        DestroyedEventManager.OnObjectDestroyed -= HandleObjectDestroyed;
    }

    private void HandleObjectDestroyed(GameObject destroyedObject)
    {
        if (destroyedObject.CompareTag("Trash"))
        {
            TrashDestroyed++;
            TrashText.SetText("Trash Collected: " + TrashDestroyed);
            HealingScript.Heal(3f);


            if (TrashDestroyed > TrashThreshold)
            {
                // Call the method or execute the code you want when the trash threshold is reached
                CheckForTrashDestroyed();

                
                
            }
        }
    }

    private void CheckForTrashDestroyed()
    {

        HealingScript.Heal(5f);
        Debug.Log("Healing");

        // Your code for handling the action when the trash threshold is reached
    }
}