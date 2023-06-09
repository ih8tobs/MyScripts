using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShotToClick : MonoBehaviour
{
    public GameObject Bullet;
    public float Power;
    public Camera playerCamera;
    public GameObject Tip;

    public int maxAmmo = 1;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    [SerializeField] private AudioClip[] clips1;
    private int clipIndex1;
    public AudioSource audioSource2;

    private bool canShoot = true;
    public GameObject Shooting;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            clipIndex1 = Random.Range(0, clips1.Length);
            audioSource2.clip = clips1[clipIndex1];
            audioSource2.Play();
            
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        canShoot = true;
    }

    void Shoot()
    {

        if (canShoot == true)
        {
            Animator anim = Shooting.GetComponent<Animator>();
            anim.SetTrigger("ShootGun");
            Debug.Log("AnimPlaying");
            canShoot = false;
        }

        ParticleSystem ps = GameObject.Find("SmokeGun").GetComponent<ParticleSystem>();
        ps.Play();

        currentAmmo--;
        Rigidbody rb = Instantiate(Bullet, Tip.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Power, ForceMode.Impulse);
    }
}
