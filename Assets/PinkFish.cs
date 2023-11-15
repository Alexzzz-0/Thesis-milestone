using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkFish : FishBase
{
   RedFish targetRedFish;
    public bool isBig = false;

    public override void LoseScale()
    {
        base.LoseScale();
        for (int i = scaleSize; i < blackFishList.Count; i++)
        {
            blackFishList[i].SetDown();
            i--;
        }
    }
    bool isDie = false;
    public override void DeleteThis()
    {
        if (isDie)
        {
            return;
        }
        isDie = true;
        //粉鱼死亡一起死
        for (int i = 0; i < blackFishList.Count; i++)
        {
            
            blackFishList[i].DeleteThis();
            i--;
        }
 
        base.DeleteThis();
        if (isBig)
        {
            GameController.Instance.CreateBigFish();
        }
    }
    PinkFish middleFish;
    public List<BlackFish> blackFishList = new List<BlackFish>();
    public void AddBlackFish(BlackFish blackFish)
    {
        blackFishList.Add(blackFish);
    }
    public Transform GetJiShengPos()
    {
        return blackFishPoses[blackFishList.Count];
    }
    public bool isCanJiSheng()
    {
        return blackFishList.Count<scaleSize;
    }
    public void RemoveBlackFish(BlackFish blackFish)
    {
        blackFishList.Remove(blackFish);
    }
    public enum PinkFishState
    {
        MoveToBigFish,
        MoveToRedFish
    }
    public PinkFishState pinkFishState=PinkFishState.MoveToBigFish;
    bool IsMaxPinkFish
    {
        get
        {
            int maxSize = 0;
            int minSize = 100;
            for (int i = 0; i < GameController.Instance.fishParent.childCount; i++)
            {
                if (GameController.Instance.fishParent.GetChild(i).GetComponent<PinkFish>())
                {
                    var fish = GameController.Instance.fishParent.GetChild(i).GetComponent<PinkFish>();
                    if (fish.scaleSize < minSize)
                    {
                        minSize = fish.scaleSize;
                    }
                    if(fish.scaleSize>maxSize)
                    {
                        maxSize = fish.scaleSize;
                    }
                }

            }
            return maxSize==scaleSize;
        }
    }
    void FindRedFish()
    {
        List<RedFish> haicaoList = new List<RedFish>();
        for (int i = 0; i < GameController.Instance.fishParent.childCount; i++)
        {
            if (GameController.Instance.fishParent.GetChild(i).GetComponent<RedFish>())
            {
              var fish=  GameController.Instance.fishParent.GetChild(i).GetComponent<RedFish>();
                if (middleFish != null)
                {
                    if (Vector3.Distance(middleFish.transform.position,fish.transform.position)< redFishRange.y)
                    {
                        haicaoList.Add(fish);
                    }
                }
                else
                {
                    haicaoList.Add(fish);
                }
            }
           
        }
        var array = haicaoList.ToArray(); System.Array.Sort(array, (a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
        if (array.Length > 0)
        {
            targetRedFish = array[0];
        }
    }
    public Transform[] createRedPoses;
    public FishBase createFishPre;
    public int addScaleCount = 1;
    int addScaleCounter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<HaiCaoSC>())
        {

            //生成三个体型最小的红鱼
            foreach (var item in createRedPoses)
            {
                GameController.Instance.CreateRedFish(item.transform.position, item.transform.rotation,(RedFish redfish)=> { });
                /*
               var fish= Instantiate(createFishPre);
                fish.transform.SetParent(GameController.Instance.fishParent);
                fish.transform.position = item.transform.position;
                fish.transform.rotation = item.transform.rotation;
                */

            }

            DeleteThis();
         
        }
        if (other.transform.GetComponent<RedFish>())
        {
            other.transform.GetComponent<RedFish>().DeleteThis();
            addScaleCounter++;
            if (addScaleCounter>addScaleCount)
            {
                addScaleCounter = 0;
                AddScale();
            }
       
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.transform.GetComponent<RedFish>())
        {
            collision.transform.GetComponent<RedFish>().DeleteThis();
            AddScale();
        }
        if (collision.transform.GetComponent<HaiCaoSC>())
        {
            DeleteThis();
        }
       */
    }
    
    public float DownScaleTime_NoEatFish = 20;
    float downScaleTimer_NoEatFish = 0;
    public void RefeshScaleTimer_NoEatFish()
    {
        downScaleTimer_NoEatFish = 0;
    }
    float tankai_power = 150;
    public Vector2 redFishRange = new Vector2(20,50);
    public void FindMiddleFish()
    {
        List<PinkFish> haicaoList = new List<PinkFish>();
        for (int i = 0; i < GameController.Instance.fishParent.childCount; i++)
        {
            if (GameController.Instance.fishParent.GetChild(i).GetComponent<PinkFish>())
            {
                var fish = GameController.Instance.fishParent.GetChild(i).GetComponent<PinkFish>();
                if (fish.scaleSize>scaleSize)
                {
                    haicaoList.Add(fish);
                }
               
            }

        }
        var array = haicaoList.ToArray();
        System.Array.Sort(array, (a, b) => (-a.scaleSize).CompareTo((-b.scaleSize)));
        if (array.Length > 0)
        {
            middleFish = array[0];
        }
    }
    public Transform[] blackFishPoses;
    // Update is called once per frame
    void Update()
    {
        //判断是最大的鱼
        if (IsMaxPinkFish)
        {
            pinkFishState = PinkFishState.MoveToRedFish;
          
        }
        else
        {
            //不是最大的鱼 首先找最近的大鱼寄生
            if (middleFish == null)
            {
                pinkFishState = PinkFishState.MoveToBigFish;
            }
            else
            {
                //离远了切换
                if (Vector3.Distance(transform.position, middleFish.transform.position) > redFishRange.y)
                {
                    pinkFishState = PinkFishState.MoveToBigFish;
                }
                else
                {
                    //Debug.Log("附近有鱼？");
                    FindRedFish();
                    if (targetRedFish == null)
                    {
                        pinkFishState = PinkFishState.MoveToBigFish;
                    }
                    else
                    {
                        pinkFishState = PinkFishState.MoveToRedFish;

                    }
                 
                }
            }
        }
        switch (pinkFishState)
        {
            case PinkFishState.MoveToBigFish:
                {
                    if (middleFish == null)
                    {
                        FindMiddleFish();
                        break;
                    }
                    //移动向粉鱼
                    if (Vector3.Distance(transform.position, middleFish.transform.position) < redFishRange.x)
                    {
                        break;
                    }
             
                    Vector3 endPos = middleFish.transform.position;
                    float moveDistance = moveSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, endPos);
                    float angle = Vector3.SignedAngle(Vector3.forward, endPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    if (distance < moveDistance)
                    {
                        transform.position = endPos;

                    }
                    else
                    {
                        transform.position += (endPos - transform.position).normalized * moveDistance;
                    }
                }
                break;
            case PinkFishState.MoveToRedFish:
                {
                    if (targetRedFish == null)
                    {
                        FindRedFish();
                      //  break;
                    }
                    if (middleFish!=null&& targetRedFish!=null)
                    {
                        if (Vector3.Distance(middleFish.transform.position, targetRedFish.transform.position) < redFishRange.y)
                        {
                            targetRedFish = null;
                            FindRedFish();
                          //  break;
                        }
                    }
                    if (targetRedFish==null)
                    {
                        if (IsMaxPinkFish)
                        {
                            //pinkFishState = PinkFishState.MoveToBigFish;
                        }
                        else
                        {
                            pinkFishState = PinkFishState.MoveToBigFish;
                            Debug.Log("追踪粉鱼");
                        }
                        break;
                    }
                   
                    //吃鱼
                    Vector3 endPos = targetRedFish.transform.position;
                    float moveDistance = moveSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, endPos);
                    float angle = Vector3.SignedAngle(Vector3.forward, endPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    if (distance < moveDistance)
                    {
                        transform.position = endPos;

                    }
                    else
                    {
                        transform.position += (endPos - transform.position).normalized * moveDistance;
                    }
                }
                break;
          
            default:
                break;
        }
        if (!isBig)
        {
            downScaleTimer_NoEatFish += Time.deltaTime;
            if (downScaleTimer_NoEatFish > DownScaleTime_NoEatFish)
            {
                downScaleTimer_NoEatFish = 0;
                LoseScale();
            }
        }
       
    }
}
