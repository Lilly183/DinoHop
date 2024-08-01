using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MoveRotateShoot : MonoBehaviour
{
    //=======================
    // Component(s) Required:
    //=======================

    AudioSource fireSFX = null;

    //=======================================
    // Makes Reference(s) to Other Object(s):
    //=======================================
    
    public Transform targetTransform;
    Transform turretFirePoint = null;

    //=================
    // Takes Prefab(s):
    //=================

    public GameObject prefabToFire;

    //====================
    // Member Variable(s):
    //====================

    public bool targetMouse = false;
    public bool useActivationRange = false;
    public bool shouldMove = false;
    public bool shouldRotate = false;

    public float activationRange = 5f;
    public float speedPerSecond = 3f;
    public float spriteAngleOffset = 0f;
    public float fireDelay = 1.0f;

    Vector3 targetPosition;
    bool canFire = false;

    // Start is called before the first frame update
    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindWithTag("Player").transform;
        }

        if (turretFirePoint == null)
        {
            turretFirePoint = transform.GetChild(0).GetChild(0);
        }

        fireSFX = GetComponent<AudioSource>();

        Invoke(nameof(Fire), Random.Range(0.0f, fireDelay));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        ===================
        Set TargetPosition:
        ===================
       
        For objects attached with this script, we need a target to move towards and/or 
        face. If targetMouse is checked, targetPosition is the current position of the 
        mouse cursor (in world units, not pixels on-screen) with a z-coordinate of 0. 
        Otherwise, targetPosition is the position of a referenced Transform component 
        (targetTransform), assuming it isn't null.
        */

        if (targetMouse)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            targetPosition = mouseWorldPosition;
        }

        else if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }

        /*
        =============================================
        Calculate SeekDirection and DistanceToTarget:
        =============================================
        
        Subtract the position of our gameObject from targetPosition to get the vector 
        which exists between the two. Let this be seekDirection. Store the magnitude of 
        this new vector as distanceToTarget; this will be relevant for activationRange. 
        Normalize seekDirection so that its magnitude doesn't affect movement logic (We 
        don't want our GameObject to move faster/slower depending on the distance between 
        it and targetPosition).
        */

        Vector3 seekDirection = targetPosition - transform.position;
        float distanceToTarget = seekDirection.magnitude;
        seekDirection.Normalize();

        //==================
        // Activation Range:
        //==================

        // If useActivationRange is checked and distanceToTarget is beyond activationRange,
        // return. Because we don't proceed any further, the gameObject will not move, rotate,
        // or fire.

        if (useActivationRange && distanceToTarget > activationRange)
        {
            return;
        }

        //================
        // Movement Logic:
        //================

        if (shouldMove)
        {
            transform.position += speedPerSecond * Time.deltaTime * seekDirection;
        }

        //================
        // Rotation Logic:
        //================

        if (shouldRotate)
        {
            transform.right = seekDirection;
            transform.Rotate(new Vector3(0, 0, spriteAngleOffset));
        }

        //=============
        // Shoot Logic:
        //=============

        if (canFire && targetTransform != null)
        {
            Instantiate(prefabToFire, turretFirePoint.position, transform.rotation);
            
            fireSFX.pitch = Random.Range(0.8f, 1.2f);
            fireSFX.volume = Random.Range(0.8f, 1.2f);
            fireSFX.Play();

            canFire = false;
            
            Invoke(nameof(Fire), fireDelay);
        }
    }

    void Fire()
    {
        canFire = true;
    }
}
