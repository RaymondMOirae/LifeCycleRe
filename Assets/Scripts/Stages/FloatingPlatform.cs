using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public bool Activated = false;
    public float slowRatio = 5;

    private float pastTime = 0.0f;
    private Vector3 dist;

    private void Start()
    {
        dist = endPoint.transform.position - startPoint.transform.position;
        transform.position = startPoint.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Activated)
        {
            dist = endPoint.transform.position - startPoint.transform.position;

            pastTime += Time.deltaTime;
            Vector3 delta = (0.5f - Mathf.Cos(pastTime / slowRatio) / 2) * dist;
            transform.position = startPoint.transform.position + delta;
        }
    }
}
