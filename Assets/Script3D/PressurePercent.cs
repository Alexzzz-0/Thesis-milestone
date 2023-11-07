using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressurePercent : MonoBehaviour
{
    public Gradient colorGra;
    Image img;
    public Transform max;
    public Transform min;
    public Transform upline;
    public Transform downLine;
    public float upLinePercent=0.65f;
    public float downLinePercent=0.35f;
    public StateIcon stateIcon;
    private void Awake()
    {
        img = GetComponent<Image>();
        {
            Vector3 pos = upline.transform.position;
            pos.y = Mathf.Lerp(min.position.y, max.position.y, upLinePercent);
            upline.transform.position=pos;
        }
        {
            Vector3 pos = downLine.transform.position;
            pos.y = Mathf.Lerp(min.position.y, max.position.y, downLinePercent);
            downLine.transform.position = pos;
        }
    }
    public void SetValue(float value)
    {
        img.fillAmount = value;
        img.color = colorGra.Evaluate(value);
        //Debug.Log("xxx"+value);
        if (value < downLinePercent)
        {
            Debug.Log("นýะก");
            stateIcon.SetState("low");
        }
        else if (value > upLinePercent)
        {
            stateIcon.SetState("high");
        }
        else
        {
            stateIcon.SetState("none");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
