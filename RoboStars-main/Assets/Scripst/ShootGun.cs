using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ShootGun : Gun
{
    [SerializeField] Camera myCam;
    [SerializeField] private TMP_Text ammoText; 
    private PhotonView myPv;
    private int currentAmmo;
    private bool isReload;


    private void Awake()
    {
        myPv = GetComponent<PhotonView>();
        gunAnimator = GetComponentInChildren<Animator>();
        if (!myPv.IsMine) Destroy(ammoText);
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }
    private void UpdateAmmoUI()
    {
        ammoText.text = $"AMMO: {currentAmmo}";
    }
    private void Reload()
    {
        currentAmmo = maxAmmo;
        isReload = false;
        UpdateAmmoUI();
    }
    private void Shoot()
    {
        Debug.Log("Shoooooooooot!!!!!!");
        if (currentAmmo > 0)
        {
            
            currentAmmo--;
            UpdateAmmoUI();

            gunAnimator.Play("PistolShoot");
            Ray ray = myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = myCam.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.
                    TakeDamage(((GunInfo)itemInfo).Damage);
                myPv.RPC(nameof(RPC_SHOOT), RpcTarget.All, hit.point, hit.normal);
            }

            if (currentAmmo <= 0 && !isReload)
            {
                isReload = true;
                Invoke(nameof(Reload), 3f);
            }
        }
    }

    [PunRPC]
    void RPC_SHOOT(Vector3 hitPoint, Vector3 hitNormal)
    {
        GameObject tempFX = Instantiate(shootFX, muzzle.position, Quaternion.identity);
        tempFX.transform.SetParent(muzzle);
        Collider[] colls = Physics.OverlapSphere(hitPoint, 0.1f);
        if (colls.Length != 0)
        {
            GameObject bulletImp = Instantiate(bulletPrefab, hitPoint, 
                Quaternion.LookRotation(hitNormal, Vector3.up) * 
                bulletPrefab.transform.rotation);

            bulletImp.transform.SetParent(colls[0].transform);
            Destroy(bulletImp, 15f);
        }
    }
    public override void Use()
    {
        Shoot();
    }

    
}
