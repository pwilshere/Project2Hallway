using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    
    public SphereCollider sp; 
    public Material matAlpha;
    public float maxRadius;
    public float growRate;
    private float curRadius;
    Color color;


    void Start()
    {
        curRadius = 0.1f;
        color = matAlpha.color;
        color.a = 50;
        matAlpha.color = color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        curRadius += 0.5f * growRate;
        
        color.a = ((maxRadius - curRadius)/maxRadius) * 0.5f;

        if (color.a > 0)
            matAlpha.color = color;
        
        transform.localScale = new Vector3 (curRadius,curRadius,curRadius);
        if (curRadius > maxRadius)
            Destroy(this.gameObject);
    }
}
