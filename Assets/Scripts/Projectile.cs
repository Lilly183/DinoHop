using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    //=================
    // Takes Prefab(s):
    //=================

    public GameObject prefabOnImpact;

    //====================
    // Member Variable(s):
    //====================

    public float speed = 12.5f;
    public int damage = 25;

    //================
    // Movement Logic:
    //================

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
    }

    /*
    ========================
    When Projectile Impacts:
    ========================

    Upon impact (no matter what we've collided with), destroy the projectile and 
    instantiate the referenced prefabOnImpact (an explosion). Apparently, we can 
    do the former here (even if the projectile must exist for the Player script 
    to be able to retrieve its damage) because, according to Unity's documentation, 
    Destroy() doesn't kill an object until immediately after the current Update() 
    loop.
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        GameObject explosion = Instantiate(prefabOnImpact, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
        explosion.transform.localScale *= Random.Range(0.65f, 1.35f);
    }
}