using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float Time = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
