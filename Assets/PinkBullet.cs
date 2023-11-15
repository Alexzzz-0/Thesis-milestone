using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBullet : BulletBase
{
    bool isDie = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
        }
        if (true)
        {

        }
      //  isDie = true;
      /*
        //碰到红鱼 杀死红鱼 消失 
        if (other.GetComponent<RedFish>())
        {
            Debug.Log("目标是红鱼");
            isDie = true;
            Destroy(this.gameObject);
            other.GetComponent<RedFish>().DeleteThis();
            // Update is called once per frame

            //鼠标放大
            GameController.Instance.player.AddScale();
        }
        
        else if (other.GetComponent<HaiCaoSC>())
        {
            Debug.Log("目标是水草");
            
            isDie = true;
            Destroy(this.gameObject);
            GameController.Instance.player.SetFishDie();
            
        }
    
        */
    }
}
