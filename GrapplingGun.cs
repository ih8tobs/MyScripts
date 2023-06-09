using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, Camera, Player;
    public float maxDistance = 25f;
    private SpringJoint joint;

    public GameObject Grappling;
    private bool canShoot = true;


    [Header("Grappling Hook")]
    public float Spring = 5f;
    public float Damper = 5f;
    public float MassScale = 5f;


    [SerializeField] private AudioClip[] clips1;
    private int clipIndex1;
    public AudioSource audioSource2;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
            if (canShoot == true)
            {
                Animator anim = Grappling.GetComponent<Animator>();
                anim.SetTrigger("ShootGrapple");
                Debug.Log("AnimPlaying");
                canShoot = false;
            }

        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
            Animator anim = Grappling.GetComponent<Animator>();
            anim.ResetTrigger("ShootGrapple");
            anim.SetTrigger("IdleGrapple");
            canShoot = true;
        }
    }

    
    void LateUpdate()
    {
        DrawRope();
    }


    void StartGrapple()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = Player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;



            float distanceFromPoint = Vector3.Distance(Player.position, grapplePoint);

            //Distance for grapple
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            
            joint.spring = Spring;
            joint.damper = Damper;
            joint.massScale = MassScale;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            clipIndex1 = Random.Range(0, clips1.Length);
            audioSource2.clip = clips1[clipIndex1];
            audioSource2.Play();
            ParticleSystem ps = GameObject.Find("SmokeGrapple").GetComponent<ParticleSystem>();
            ps.Play();


        }
    }



    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}