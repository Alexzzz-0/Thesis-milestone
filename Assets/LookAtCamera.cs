using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Look()
    {
        transform.forward = GameController.Instance.gameCamera.transform.forward;
    }
    private void OnValidate()
    {
        Look();
    }
    // Update is called once per frame
    void Update()
    {
        Look();
    }
}
