using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isDie = false;
    public void SetDie()
    {
        isDie = true;
        rig.velocity = Vector3.zero;
    }
    Rigidbody rig;//刚体组件
    public float moveSpeed = 3;//移动速度
    public float jumpPow = 10;//跳跃力
    public float upSpeed = 5;//上升速度
   
    RotateByMouse cameraRotate;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        cameraRotate = GetComponent<RotateByMouse>();
        touchMouse.SetState("默认");
        upOrDownTip.SetState("默认");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool isJump = false;
    private void FixedUpdate()
    {
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        isJump = false;//回归不跳跃状态
    }
    public Transform playerMoveRangeMiddle;
    public float moveRange_h = 20;
    public float moveRange_v = 10;
    public float downTriggerAngle = -20f;
    public float upTriggerAngle = 20f;
    public KeyCode up_yaqiang_Key = KeyCode.Q;
    public KeyCode down_yaqiang_Key = KeyCode.E;
    public KeyCode upOrDown_key = KeyCode.Space;
    //拖拽小球
    public TouchMoveBangDingPlayer touchBall = null;
    public LayerMask ballMask;
    public float thisDistance = 100;
    bool isTouchBall = false;
    public StateIcon touchMouse;
    [Header("touch Distance")]
    public float touchDistance = 2;
    //发射子弹
    public PressureMouse mouse;
    public Transform firePos;
    public WaveBullet waveBulletPre;
    public float forceMoveSpeed = 20;
    public float forceMoveTime = 0.05f;
    float forceTimer = 0;
    bool isForceMove = false;
    Vector3 moveDir_Force;
    void ForceMove(Vector3 moveDir_Force)
    {
        this. moveDir_Force=moveDir_Force;
        isForceMove = true;
        forceTimer = 0;
    }
    //上升与下降
    float vMoveSpeed = 0;
    public Vector2 thisVSpeedPowerRange = new Vector2(0.4f,1.2f);
    float GetVSpeedPower()
    {
        float percent = (transform.position.y - playerMoveRangeMiddle.position.y+moveRange_v) / (moveRange_v * 2);
        float r = Mathf.Lerp(thisVSpeedPowerRange.x, thisVSpeedPowerRange.y, percent);
        return r;
    }
    public StateIcon upOrDownTip;
    public void SetUpOrDown(int dir)
    {
        if (isDie)
        {
            return;
        }
        this.vMoveSpeed = dir;
    }
    //加压减压
    public float curPressure = 0.5f;
    public float addPressurePercentSpeed_Move = 0.2f;
    public float addPressurePercentSpeed_Touch = -0.2f;
    //外界增压减压
    public void AddZengYa(float power)
    {
        curPressure += power;
        if (curPressure > 1)
        {
            curPressure = 1;

        }
        if (curPressure < 0)
        {
            curPressure = 0;

        }
        GameController.Instance.pressurePercent.SetValue(curPressure);
    }

    string mouseState = "";
    int wudiLayer = 0;
    public float wudiTime =2.0f;
    IEnumerator wudiIE()
    {
        wudiLayer++;
        yield return new WaitForSeconds(wudiTime);
        wudiLayer--;
    }
   public  void FuHuo()
    {
        curPressure = 0.5f;
        GameController.Instance.pressurePercent.SetValue(curPressure);
        mouse.Refesh();
        mouse.SetScale(curPressure);
        StartCoroutine(wudiIE());
    }
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            return;
        }
        if (isForceMove)
        {
            rig.velocity =Vector3.zero;
            transform.position += moveDir_Force * Time.deltaTime*forceMoveSpeed;
            forceTimer += Time.deltaTime;
            if (forceTimer>forceMoveTime)
            {
                isForceMove = false;
            }
            //修正水平的两个轴
            {
                Vector3 pos = playerMoveRangeMiddle.position;
                pos.y = transform.position.y;
                if (Vector3.Distance(transform.position, pos) > moveRange_h)
                {
                    Vector3 dir = (transform.position - pos).normalized;
                    transform.position = playerMoveRangeMiddle.position + dir * moveRange_h;
                }
            }
            //修正高度

            {
                Vector3 pos = transform.position;
                if (pos.y - playerMoveRangeMiddle.position.y > moveRange_v)
                {
                    pos.y = playerMoveRangeMiddle.position.y + moveRange_v;
                    vMoveSpeed = 0;
                }
                else if (playerMoveRangeMiddle.position.y - pos.y > moveRange_v)
                {
                    pos.y = playerMoveRangeMiddle.position.y - moveRange_v;
                    vMoveSpeed = 0;
                }
                transform.position = pos;
            }

            return;
        }
        //操控小球
        {
            //鼠标点击到了球 就可以拖拽小球
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (touchBall == null)
                {
                    var ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                    //发射射线 点击小球
                    RaycastHit hit;
                    bool isRay = Physics.Raycast(ray, out hit, touchDistance, ballMask);
                    if (isRay)
                    {
                        thisDistance = hit.distance;
                    
                        isTouchBall = true;
                        touchBall = hit.transform.GetComponent<TouchMoveBangDingPlayer>();
                        touchBall.isTouch = true;
                       
                     
                    }
                }
                else
                {
                    touchBall.isTouch = false;
                    touchBall = null;
                    isTouchBall = false;
                    mouseState = "idle";
                }
             
            }
            if (touchBall == null)
            {
                touchBall = null;
                isTouchBall = false;
                mouseState = "idle";
            }
            if (isTouchBall)
            {
                var ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, thisDistance, ballMask);
                touchBall.transform.position = ray.origin + thisDistance * ray.direction;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1) && isTouchBall)
            {
            
            }
           
        }
        if (touchBall!=null)
        {
            mouseState = "drag";
        }


       // Debug.Log(cameraRotate.getCurAngle);
        //move
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        if (Mathf.Abs(y) >0)
        {
            if (touchBall==null)
            {
                mouseState = "move";
            }

        }
        else
        {
          //  touchMouse.SetState("");
        }
        rig.velocity = transform.forward*y*moveSpeed+transform.right*x*moveSpeed+new Vector3( 0,rig.velocity.y,0)*GameController.Instance.thisMovePower;

        SetUpOrDown(0);
        float angle = -cameraRotate.getCurAngle;
        if (angle > upTriggerAngle)
        {
            upOrDownTip.SetState("up");
        }
        else if (angle < downTriggerAngle)
        {

        upOrDownTip.SetState("down");

        }    
        else
        {
            upOrDownTip.SetState("none");
        }
        //当检测到空格时
        if (Input.GetKey(upOrDown_key))
        { 
            //上升检测
            if (angle > upTriggerAngle)
            {
                //向上升
                SetUpOrDown(1);
                Debug.Log("up");
            }
            //下降检测
            if (angle < downTriggerAngle)
            {
                //向下升
                SetUpOrDown(-1);
                Debug.Log("down");
            }
        }
        //上升下降
        float thisVSpeed =vMoveSpeed * GameController.Instance.thisMovePower * GetVSpeedPower();
        transform.position += Vector3.up * thisVSpeed * Time.deltaTime * upSpeed ;

        //玩家位置修正
        //修正水平的两个轴
        {
            Vector3 pos = playerMoveRangeMiddle.position;
            pos.y = transform.position.y;
            if (Vector3.Distance(transform.position, pos) > moveRange_h)
            {
                Vector3 dir = (transform.position - pos).normalized;
                transform.position = playerMoveRangeMiddle.position + dir * moveRange_h;
            }
        }
        //修正高度
        
        {
            Vector3 pos = transform.position;
            if (pos .y- playerMoveRangeMiddle.position.y>moveRange_v)
            {
                pos.y = playerMoveRangeMiddle.position.y + moveRange_v;
                vMoveSpeed = 0;
            }
            else if (playerMoveRangeMiddle.position.y - pos.y > moveRange_v)
            {
                pos.y = playerMoveRangeMiddle.position.y - moveRange_v;
                vMoveSpeed = 0;
            }
            transform.position = pos;
        }
        
        /*
    if (Vector3.Distance(transform.position, playerMoveRangeMiddle.position) > moveRange)
    {
        Vector3 dir = (transform.position - playerMoveRangeMiddle.position).normalized;
        transform.position = playerMoveRangeMiddle.position + dir * moveRange;
    }
    if (Vector3.Distance(transform.position, playerMoveRangeMiddle.position) == moveRange)
    {
        vMoveSpeed = 0;
    }
    */
        //压力值计算
        if (vMoveSpeed != 0)
        {
            //增加压力值
          
            var Add = addPressurePercentSpeed_Move *thisVSpeed * Time.deltaTime;
            if (wudiLayer == 0)
            {
                curPressure -= Add;
            }
         

            //给小球增加压力值
            if (touchBall != null)
            {
                touchBall.AddYaLi(Add);
            }


            if (curPressure > 1)
            {
                curPressure = 1;

            }
            if (curPressure < 0)
            {
                curPressure = 0;

            }

            //手动增压减压
            if (Input.GetKey(KeyCode.Q))
            {
             
                AddZengYa(addPressurePercentSpeed_Touch *Mathf.Abs( thisVSpeed )* Time.deltaTime);

            }
            if (Input.GetKey(KeyCode.E))
            {
             
                AddZengYa(-addPressurePercentSpeed_Touch * Mathf.Abs(thisVSpeed) * Time.deltaTime);
            }

        }
        else
        {

            // curPressure = 0.5f;
        }
       

        //发射波
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var bt = Instantiate(waveBulletPre);

                /*
                {
                    bt.transform.position = firePos.position;

                    bt.transform.rotation = firePos.rotation;
                    bt.SetMove(firePos.forward);
                }
                */
                {
                    
                    Ray ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                    bt.transform.position = firePos.position;
                    bt.transform.forward = ray.direction.normalized;
                    bt.SetMove(ray.direction.normalized);
                    ForceMove(-firePos.forward);
                }
            }
        }


        GameController.Instance.pressurePercent.SetValue(curPressure);
        mouse.SetScale(curPressure);
        touchMouse.SetState(mouseState);
    }
}
