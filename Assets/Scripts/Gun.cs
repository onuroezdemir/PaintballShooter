﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem splashFire;
    public GameObject impactEffect;
    public GameObject decalPrefab;

    private float nextTimeToFire = 0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time>= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }


    }

    void Shoot()
    {
        splashFire.Play();
        RaycastHit hit;
       if( Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*impactForce);
            }

           GameObject impactGameObject =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            SpawnDecal(hit);
            Destroy(impactGameObject, 1f);
        }
    }

    private void SpawnDecal(RaycastHit hitInfo)
    {
        var decal = Instantiate(decalPrefab);
        decal.transform.position = hitInfo.point;
        decal.transform.forward = hitInfo.normal * -1f;
        Destroy(decal, 5f);
    }

}
