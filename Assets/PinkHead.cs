using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkHead : MonoBehaviour
{
    public int curCount = 0;
    public int useCount = 2;
    PinkFish pinkFish;
    private void Awake()
    {
        pinkFish = GetComponentInParent<PinkFish>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<RedFish>())
        {
            var fish = collision.GetComponent<RedFish>();
            GameController.Instance.player.mouseScale.AddThisScale();
            fish.DeleteThis();
            curCount++;
            if (curCount >= useCount)
            {
                curCount = 0;
                pinkFish.AddScale();
                pinkFish.RefeshScaleTimer_NoEatFish();
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
