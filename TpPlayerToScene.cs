using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpPlayerToScene : MonoBehaviour
{
    // Start is called before the first frame update
    public int NextScene;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Application.LoadLevel(NextScene);
        }
    }
}
