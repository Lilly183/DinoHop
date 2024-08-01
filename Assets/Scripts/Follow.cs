using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    //=======================================
    // Makes Reference(s) to Other Object(s):
    //=======================================

    public Transform targetTransform;

    //====================
    // Member Variable(s):
    //====================

    public bool lockXAxis = false;
    public bool lockYAxis = false;
    public float lerpPerFrame = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            Vector3 targetPosition = targetTransform.position;

            targetPosition.z = transform.position.z;

            if (lockXAxis)
            {
                targetPosition.x = transform.position.x;
            }

            if (lockYAxis)
            {
                targetPosition.y = transform.position.y;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpPerFrame);
        }
    }
}
