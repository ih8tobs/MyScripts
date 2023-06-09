using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickup : MonoBehaviour
{
    [Header("Pickup variables")]
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform pickupTarget;
    [Space]
    [SerializeField] private float pickupRange;
    private Rigidbody CurrentObject;



    private void Update()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            if (CurrentObject)
            {
                CurrentObject.useGravity = true;
                CurrentObject = null;
            }

            Ray CameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit Hitinfo, pickupRange, pickupMask))
            {
                CurrentObject = Hitinfo.rigidbody;
                CurrentObject.useGravity = false;
            }
        }

    }

    void FixedUpdate()
    {
        if (CurrentObject && Input.GetKey(KeyCode.E))
        {
            Vector3 DirectionToPoint = pickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = 12f * DistanceToPoint * DirectionToPoint;

            if (CurrentObject.CompareTag("Enemy"))
            {
                
                Debug.Log("Grabbing enemy");
                

            }

        }
    }

}
