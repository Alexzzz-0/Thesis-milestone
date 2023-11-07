using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoveBangDingPlayer : MonoBehaviour
{
    public Vector2 scaleRange = new Vector2(0.5f, 2.0f);
    public bool isDie = false;
    //public GameObject dieHide;
    public GameObject dieEffect;

    public float maxDie = 0.7f;
    public float minDie = 0.3f;
    float curYali = 0.5f;
    public void AddYaLi(float add)
    {
        
        curYali += add;
        if (curYali < 0)
        {
            curYali = 0;
        }
        if (curYali > 1)
        {
            curYali = 1;
        }
        SetScale(curYali);
    }
    public void SetScale(float percent)
    {
        if (isDie)
        {
            return;
        }
        transform.localScale = Vector3.one * Mathf.Lerp(scaleRange.x, scaleRange.y, percent);
        if (percent > maxDie)
        {
            Destroy(gameObject);
            isDie = true;
            var effect = Instantiate(dieEffect);
            effect.transform.position = transform.position;
        }
        if (percent < minDie)
        {
            Destroy(gameObject);

            isDie = true;
           
        }
    }
    public bool isTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        moveTargetPos = GetMovePoses();
    }
    float minMoveDistance = 1;
    float maxMoveDisatance = 9999;
    public float changePosDistance = 0.1f;
    Vector3 GetMovePoses()
    {
        List<Vector3> posList = new List<Vector3>();
        foreach (var item in GameController.Instance.ballMoveParent.GetComponentsInChildren<Transform>())
        {
            Vector3 pos = item.position;
            if (Vector3.Distance(pos,transform.position) < maxMoveDisatance && Vector3.Distance(pos, transform.position ) > minMoveDistance)
            {
                posList.Add(pos);
            }
        }
        return posList[Random.Range(0,posList.Count)];
    }
    Vector3 moveTargetPos;
    public float moveSpeed = 1;
    public Transform rotatemiddle;
    public XingBianToucher xingbianTouch;
    public float xingbianForce_JiYa=10000;
    public Transform jiya_Parent;
    public float xingbianForce_kuoZhang = 10000;
    public Transform kuozhangParent;
    // Update is called once per frame
    void Update()
    {
        //获取当前的压力
        float yali = curYali;
        if (yali > 0.5f)
        {
            float curLi = Mathf.Lerp(0, xingbianForce_kuoZhang, yali - 0.5f);

            Debug.Log("挤压");
            foreach (var item in kuozhangParent.GetComponentsInChildren<Transform>())
            {
                xingbianTouch.AddForce(item.position,item.forward, curLi);
            }
            
        }
        else if(yali<0.5f)
        {
            float curLi = Mathf.Lerp(0, xingbianForce_JiYa, 0.5f-yali);
            Debug.Log("扩张");
            foreach (var item in jiya_Parent.GetComponentsInChildren<Transform>())
            {
           
                xingbianTouch.AddForce(item.position, item.forward, curLi);
            }

        }

        if (!isTouch&&GetComponent<Rigidbody>().velocity.magnitude<=0.1f)
        {
            //不被拖拽时 随机移动
            float distance = Vector3.Distance(transform.position,moveTargetPos);
            float moveDistance=moveSpeed*Time.deltaTime*GameController.Instance.thisMovePower;
            Vector3 dir_normal = (moveTargetPos - transform.position).normalized;
            //设置旋转值
            float angle = Vector3.SignedAngle(Vector3.forward, dir_normal, Vector3.up);

            transform.rotation = Quaternion.Euler(0,angle,0);

            if (distance > moveDistance)
            {
                transform.position += dir_normal * moveDistance;
            }
            else
            {
                moveTargetPos = GetMovePoses();
            }
        }
    }
}
