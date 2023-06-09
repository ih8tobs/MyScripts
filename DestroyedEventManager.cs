using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedEventManager : MonoBehaviour
{
    public delegate void ObjectDestroyedHandler(GameObject destroyedObject);
    public static event ObjectDestroyedHandler OnObjectDestroyed;

    private void OnDestroy()
    {
        // Trigger the OnObjectDestroyed event when this GameObject is destroyed
        OnObjectDestroyed?.Invoke(gameObject);
    }
}
