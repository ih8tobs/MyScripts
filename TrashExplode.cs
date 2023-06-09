using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TrashExplode : MonoBehaviour
{
    public GameObject SparklePart;
    public AudioSource audioSource;


    public TMP_Text TrashText; // The TextMeshPro object to display

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trash")
        {
            Destroy(other.gameObject);
            Destroy(other);
            ParticleSystem ps = SparklePart.GetComponent<ParticleSystem>();
            ps.Play();
            audioSource.Play();
        }
    }


}
