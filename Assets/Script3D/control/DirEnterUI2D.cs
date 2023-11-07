using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirEnterUI2D : UIEnterBase
{
    public int power = 1;
    bool isEnter = false;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        isEnter = true;
        GameController2D.Instance.player.SetDirMove(power);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        isEnter = false;
        GameController2D.Instance.player.SetDirMove(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter)
        {
           
        }
    }
}
