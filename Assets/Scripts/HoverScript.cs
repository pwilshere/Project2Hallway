using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    public float bobOffset;
    private Vector3 currentPos;
    private Vector3 restPos;
    void Start()
    {
        restPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = restPos + new Vector3(0,Mathf.Sin(Time.time)*bobOffset,0);
    }
}
