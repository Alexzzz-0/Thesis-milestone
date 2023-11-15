using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShowTime : MonoBehaviour
{
    public float showTime = 2;
    float showTimer = 0;
    private void OnEnable()
    {
        showTimer = 0;
    }
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showTimer += Time.deltaTime;
        if (showTimer>showTime)
        {
            showTimer = 0;
            GetComponentInParent<StateIcon>().SetState("");
        }
    }
}
