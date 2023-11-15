using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirEnterUI : UIEnterBase
{
    public int power = 1;
    bool isEnter = false;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isEnter = true;
        GameController.Instance.player.SetUpOrDown(power);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isEnter = false;
        GameController.Instance.player.SetUpOrDown(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter)
        {
           
        }
    }
}
