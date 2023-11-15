using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFish : FishBase
{
    /*
    public enum BlackFishState
    {
        MoveToPinkFish,
        WaitFood,
        Down
    }
    public BlackFishState blackFishState = BlackFishState.MoveToPinkFish;
    */
    public Transform pinkCreateTF;
    public PinkFish createPinkPrefab;
    public void CreatePinkFish()
    {
        GameController.Instance.CreatePink(pinkCreateTF.position, pinkCreateTF.rotation,(PinkFish pink) => { pink.SetMinScale(); });
        /*
        var pink = Instantiate(createPinkPrefab);
        pink.transform.SetParent(GameController.Instance.fishParent);
        pink.transform.position = pinkCreateTF.position;
        pink.transform.rotation = pinkCreateTF.rotation;
        */
    }

    public Transform blackCreateTF;

    PinkFish jishengPinkFish;
    public override void DeleteThis()
    {
        base.DeleteThis();
        if (jishengPinkFish != null)
        {
            jishengPinkFish.RemoveBlackFish(this);
        }

    }
    public float trackSpeedPower = 5.0f;
    public float trackTime = 0.5f;
    public float trackSkillInterval = 0.5f;
    float trackSkillTimer = 0;
    bool isTrack = false;
    Transform jishengPos;
    public enum BlackFishState
    {
        MoveToHC,
        RotateByHaiCao,
    }
    BlackFishState blackFishState = BlackFishState.MoveToHC;
    public float rotateRadiu = 11;
    public void BackJiSheng()
    {
        if (jishengPinkFish != null)
        {
            jishengPinkFish.RemoveBlackFish(this);
        }
        blackFishState = BlackFishState.MoveToHC;

        jishengPinkFish = null;
        jishengPos = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.tag=="Ground")
        {
            DeleteThis();
        }
        */
        if (other.GetComponent<RedFish>())
        {
           var fish= other.GetComponent<RedFish>();
            fish.DeleteThis();
            CreatePinkFish();
        }
    }
    public float DownScaleTime_NoEatFish = 10;
    float downScaleTimer_NoEatFish = 0;
    public void RefeshScaleTimer_NoEatFish()
    {
        downScaleTimer_NoEatFish = 0;
    }
    public float downSpeed = 10;
    HaiCaoCircle targetHaiCaoCircle = null;
    void FindTargetHaicaoCircle()
    {
        List<HaiCaoCircle> haicaoCircleList = new List<HaiCaoCircle>();
        for (int i = 0; i < GameController.Instance.haicaoQuanParent.childCount; i++)
        {
            var haicao = GameController.Instance.haicaoQuanParent.GetChild(i).GetComponent<HaiCaoCircle>();
            haicaoCircleList.Add(haicao);
        }
        var array = haicaoCircleList.ToArray(); System.Array.Sort(array, (a, b) => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
        if (array.Length > 0)
        {
            targetHaiCaoCircle = array[0];
         
        }
    }
    float realRadiu = 0;
    private void Update()
    {
        float moveSpeedT = moveSpeed;
        switch (blackFishState)
        {
            case BlackFishState.MoveToHC:
                {
                    if (targetHaiCaoCircle==null)
                    {
                        FindTargetHaicaoCircle();
                        break;
                    }
                    //Debug.Log("移动");
                    Vector3 targetPos = new Vector3(targetHaiCaoCircle.transform.position.x, transform.position.y, targetHaiCaoCircle.transform.position.z);
                    float distance = Vector3.Distance(transform.position, targetPos);
                    float moveDistance = moveSpeedT * Time.deltaTime;
                    float angle = Vector3.SignedAngle(Vector3.forward, targetPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    if (moveDistance < distance)
                    {
                        //Debug.Log("移动中"+distance);
                        transform.position += (targetPos - transform.position).normalized * moveDistance;
                    }
                    else
                    {
                        //Debug.Log("瞬移？");
                        transform.position = targetPos;
                      
                    }
                    
                    if (Vector3.Distance(transform.position, targetPos)<rotateRadiu)
                    {

                        realRadiu = Vector3.Distance(transform.position, targetPos);
                        blackFishState = BlackFishState.RotateByHaiCao;
                    }
                }
                break;
            case BlackFishState.RotateByHaiCao:
                {
                    float moveDistance = moveSpeedT * Time.deltaTime;
                    float moveAngle = moveDistance / (realRadiu * 2 * Mathf.PI)*360.0f;
                    Vector3 haicaoPos = new Vector3(targetHaiCaoCircle.transform.position.x, transform.position.y, targetHaiCaoCircle.transform.position.z);
                    float curAngle = Vector3.SignedAngle(Vector3.forward, transform.position-haicaoPos, Vector3.up);
                    float targetAngle = curAngle + moveAngle;
                    Vector3 lastPos = transform.position;
                    Vector3 targetLocalPos = new Vector3(Mathf.Sin(targetAngle/180*Mathf.PI),0, Mathf.Cos(targetAngle / 180 * Mathf.PI)) *realRadiu;
                    transform.position = haicaoPos + targetLocalPos;


                    float bodyAngle = Vector3.SignedAngle(Vector3.forward, transform.position-lastPos, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, bodyAngle, 0);
                }
                break;
            default:
                break;
        }

        /*
        //使用冲刺
        if (isTrack)
        {
            trackSkillTimer += Time.deltaTime;
            if (trackSkillTimer> trackTime)
            {
                trackSkillTimer = 0;
                isTrack = false;
            }
            moveSpeedT *= trackSpeedPower;
        }
        else
        {
            trackSkillTimer+= Time.deltaTime;
            if (trackSkillTimer > trackSkillInterval)
            {
                trackSkillTimer = 0;
                isTrack = true;
            }
       }
        switch (blackFishState)
        {
            case BlackFishState.MoveToPinkFish:
                {
                    Transform moveTargetPosTF=null;
                    PinkFish targetPink=null;
                    //寻找可以寄生的粉鱼
                    foreach (var item in GameController.Instance.fishParent.GetComponentsInChildren<PinkFish>())
                    {
                        if (item.isCanJiSheng())
                        {
                           moveTargetPosTF=item.GetJiShengPos();
                            targetPink = item;
                            break;
                        }
                    }

                    if (moveTargetPosTF == null)
                    {
                        //SetDown();
                        break ;
                    }
                    Vector3 endPos = moveTargetPosTF.position;
                    float moveDistance = moveSpeedT * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, endPos);
                    float angle = Vector3.SignedAngle(Vector3.forward, endPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    if (distance < moveDistance)
                    {
                        transform.position = endPos;
                        jishengPos = moveTargetPosTF;
                        jishengPinkFish = targetPink;
                        jishengPinkFish.AddBlackFish(this);
                        blackFishState = BlackFishState.WaitFood;
                    }
                    else
                    {
                        transform.position += (endPos - transform.position).normalized * moveDistance;
                    }
                }
                break;
            case BlackFishState.WaitFood:
                {
                    
                    transform.position = jishengPos.transform.position;
                    
                }
                break;
            case BlackFishState.Down:
                {
                    transform.position += downSpeed * Time.deltaTime*Vector3.down;
                }
                break;

            default:
                break;
        }
        if (blackFishState == BlackFishState.WaitFood)
        {
            downScaleTimer_NoEatFish += Time.deltaTime;
            if (downScaleTimer_NoEatFish > DownScaleTime_NoEatFish)
            {
                downScaleTimer_NoEatFish = 0;
                SetDown();
            }
        }
      */
    }
    public void SetDown()
    {
     //  BackJiSheng();
       // blackFishState = BlackFishState.Down;
    }

}
