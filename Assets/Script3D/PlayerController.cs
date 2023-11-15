using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rig;
    bool isDie = false;
    public void SetDie()
    {
        isDie = true;
        Debug.Log("刚体"+rig);
        rig.velocity = Vector3.zero;
    }
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        touchMouse.SetState("默认");
    }
    //移动
    public float moveSpeed = 10;
    int thisMoveDir;
    public StateIcon touchMouse;
    public void SetDirMove(int dir)
    {
        if (isDie)
        {
            return;
        }
        thisMoveDir = dir;
        rig.velocity =transform.forward* dir*moveSpeed*GameController.Instance.thisMovePower;
    }
    public float vMoveSpeed = 0;
    public void SetUpOrDown(int dir)
    {
        if (isDie)
        {
            return;
        }
        this.vMoveSpeed = dir;
    }
   
    //旋转
    public float rotateSpeed = 10;
    float rotatePower = 0;
    public void SetRotate(int power)
    {
        if (isDie)
        {
            return;
        }
        rotatePower = power;
    }
    //上升和下降
    public float upSpeed = 10;



    //加压减压
    public float curPressure = 0.5f;
    public float addPressurePercentSpeed_Move = 0.5f;
    public float addPressurePercentSpeed_Touch = 0.7f;
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
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.pressurePercent.SetValue(curPressure);
    }
    public TouchMoveBangDingPlayer touchBall = null;
    public LayerMask ballMask;
    public float thisDistance = 100;
    bool isTouchBall = false;
 
    public PressureMouse mouse;
    public Transform firePos;
    public WaveBullet waveBulletPre;
    public Transform playerRangeMiddle;
    public float moveRange = 10;
    [Header("可以点击距离")]
    public float touchDistance = 2000;
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            return;
        }
        float v = Input.GetAxis("Vertical");
        if (v == 0)
        {
            SetDirMove(0);
        }
        else
        {
            SetDirMove((int)Mathf.Sign(v));
        }

        rig.velocity = transform.forward * thisMoveDir * moveSpeed * GameController.Instance.thisMovePower;

        float h = Input.GetAxis("Horizontal");
        if (h == 0)
        {
            SetRotate(0);
        }
        else
        {
            SetRotate((int)Mathf.Sign(h));
        }
        //旋转
        transform.Rotate(Vector3.up*rotatePower*Time.deltaTime*rotateSpeed* GameController.Instance.thisMovePower);
        //上升下降
       
        transform.position += Vector3.up * upSpeed * Time.deltaTime* vMoveSpeed * GameController.Instance.thisMovePower;
        //玩家位置修正
        if (Vector3.Distance(transform.position,playerRangeMiddle.position)>moveRange)
        {
            Vector3 dir = (transform.position - playerRangeMiddle.position ).normalized;
            transform.position = playerRangeMiddle.position + dir * moveRange;
        }
        if (Vector3.Distance(transform.position, playerRangeMiddle.position)== moveRange)
        {
            vMoveSpeed = 0;
        }
        //压力值计算
        if (vMoveSpeed != 0)
        {
            //增加压力值
            var Add = addPressurePercentSpeed_Move * vMoveSpeed * Time.deltaTime;
            curPressure -= Add;
           
            //给小球增加压力值
            if (touchBall!=null)
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
                AddZengYa(addPressurePercentSpeed_Touch * Time.deltaTime);
                Debug.Log("按下q");
            }
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("按下e");
                AddZengYa(-addPressurePercentSpeed_Touch * Time.deltaTime);
            }
           
        }
        else
        {
            //自动回复平均值
           // curPressure = 0.5f;
        }
       


        GameController.Instance.pressurePercent.SetValue(curPressure);
        mouse.SetScale(curPressure);

        //操控小球
        {
            //鼠标点击到了球 就可以拖拽小球
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                //发射射线 点击小球
                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, touchDistance, ballMask);
                if (isRay)
                {
                    thisDistance = hit.distance;
                    Debug.Log("射线检测到了" + hit.transform.name + hit.distance);
                    isTouchBall = true;
                    touchBall = hit.transform.GetComponent<TouchMoveBangDingPlayer>();
                    touchBall.isTouch = true;
                    touchMouse.SetState("拖拽");
                }
            }
            if (touchBall==null)
            {
                touchBall = null;
                isTouchBall = false;
                touchMouse.SetState("默认");
            }
            if (isTouchBall)
            {
                var ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, thisDistance, ballMask);
                touchBall.transform.position = ray.origin + thisDistance * ray.direction;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1)&&isTouchBall)
            {
                touchBall.isTouch = false;
                touchBall = null;
                isTouchBall = false;
                touchMouse.SetState("默认");
            }

        }
        //发射波
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var bt = Instantiate(waveBulletPre);
                //固定方向
                /*
                {
                    bt.transform.position = firePos.position;

                    bt.transform.rotation = firePos.rotation;
                    bt.SetMove(firePos.forward);
                }
                */
                {
                    //获取鼠标指针
                    Ray ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                    bt.transform.position = firePos.position;
                    bt.transform.forward = ray.direction.normalized;
                    bt.SetMove(ray.direction.normalized);
                }
            }
        }
    }
}
