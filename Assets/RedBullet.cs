using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullet : BulletBase
{
    bool isDie = false;
   
    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
        }


        //碰到红鱼 相互弹开
        if (other.GetComponent<RedFish>())
        {
            isDie = true;
            Destroy(this.gameObject);
            float power = RedFish.tankai_power;
            Vector3 targetForceDir = (other.transform.position - GameController.Instance.player.transform.position).normalized;
            if (other.GetComponent<RedFish>().scaleSize<=GameController.Instance.player.mouseScale.scaleSize)
            {
               
                other.transform.GetComponent<Rigidbody>().AddForce(targetForceDir * power);
            }
            if (other.GetComponent<RedFish>().scaleSize >= GameController.Instance.player.mouseScale.scaleSize)
            {
                Vector3 thisForceDir = -targetForceDir;
                GameController.Instance.player.ForceMove(thisForceDir * power * 0.5f);
            }
               
            
        }
        else if (other.GetComponent<BlackFish>())
        {
            isDie = true;
            Destroy(this.gameObject);
          //  other.GetComponent<BlackFish>().CreatePink();
        }
        else if (other.GetComponent<HaiCaoSC>())
        {
            isDie = true;
            Destroy(this.gameObject);
            Debug.Log("碰到海草");
            {
                other.GetComponent<HaiCaoSC>().DeleteThis();
                //指针放大
                GameController.Instance.player.AddScale();
            }
        }
    }
}
