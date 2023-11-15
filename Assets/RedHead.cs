using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHead : MonoBehaviour
{
    public int curCount = 0;
   public  int useCount=2;
    RedFish redFish;
    private void Awake()
    {
        redFish = GetComponentInParent<RedFish>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BlackFish>())
        {
            var fish=collision.GetComponent<BlackFish>();
            Debug.Log("Åöµ½ºÚÓã»Ùµô×Ô¼º");
            fish.DeleteThis();
            curCount++;
            if (curCount>=useCount)
            {
                curCount = 0;
                redFish.AddScale();
                redFish.RefeshScaleTimer_NoEatFish();
            }
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
