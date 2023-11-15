using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBullet : BulletBase
{
    bool isDie = false;
    public FishBase createFish_Prefab;
    public BlackFish createFishBlack_Prefab;
    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
        }
        //≈ˆµΩhong ’ŸªΩ∑€”„
        if (other.GetComponent<RedFish>())
        {
            isDie = true;
            Destroy(this.gameObject);
            Vector3 pos = GameController.Instance.player.createPinkFishPos.position;
            var fish = Instantiate(createFish_Prefab);
            fish.transform.SetParent(GameController.Instance.fishParent);
            fish.transform.position = pos;

        }
        else if(other.GetComponent<BlackFish>())
        {
            isDie = true;
            Destroy(this.gameObject);
            var fish = Instantiate(createFishBlack_Prefab);
            var thisFish = other.GetComponent<BlackFish>();
            fish.transform.SetParent(GameController.Instance.fishParent);
            fish.transform.position = thisFish.blackCreateTF.position;
            fish.transform.rotation = thisFish.blackCreateTF.rotation;
        }
    }
}
