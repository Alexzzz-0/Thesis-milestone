using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMove : MonoBehaviour
{

    Rigidbody2D rig;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    //移动
    public float moveSpeed = 10;
    int thisMoveDir;
    public void SetDirMove(int dir)
    {
        thisMoveDir = dir;
        rig.velocity = transform.forward * dir * moveSpeed * actionPower;
    }
    public float actionPower = 1;
    //旋转
    public float rotateSpeed = 10;
    float rotatePower = 0;
    public void SetRotate(int power)
    {
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
        GameController2D.Instance.pressurePercent.SetValue(curPressure);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController2D.Instance.pressurePercent.SetValue(curPressure);
    }
    public TouchMoveBangDingPlayer touchBall = null;
    public LayerMask ballMask;
    public float thisDistance = 100;
    bool isTouchBall = false;
    // Update is called once per frame
    void Update()
    {


        rig.velocity = transform.forward * thisMoveDir * moveSpeed * actionPower;

        //旋转
        transform.Rotate(Vector3.back * rotatePower * Time.deltaTime * rotateSpeed * actionPower);
        //上升下降
        float v = Input.GetAxis("Vertical");
        transform.position += Vector3.up * upSpeed * Time.deltaTime * v * actionPower;
        //压力值计算
        if (v != 0)
        {
            //增加压力值
            curPressure += addPressurePercentSpeed_Move * v * Time.deltaTime;

            if (curPressure > 1)
            {
                curPressure = 1;

            }
            if (curPressure < 0)
            {
                curPressure = 0;

            }
            //手动增压减压
            if (Input.GetKey(KeyCode.Mouse0))
            {
                AddZengYa(addPressurePercentSpeed_Touch * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                AddZengYa(-addPressurePercentSpeed_Touch * Time.deltaTime);
            }
        }
        else
        {
            //自动回复平均值
            // curPressure = 0.5f;
        }



        GameController2D.Instance.pressurePercent.SetValue(curPressure);

        {
            //鼠标点击到了球 就可以拖拽小球
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = GameController2D.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                //发射射线 点击小球
                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, 2000, ballMask);
                if (isRay)
                {

                    thisDistance = hit.distance;
                    Debug.Log("射线检测到了" + hit.transform.name + hit.distance);
                    isTouchBall = true;
                    touchBall = hit.transform.GetComponent<TouchMoveBangDingPlayer>();
                }
            }
            if (isTouchBall)
            {
                var ray = GameController2D.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, thisDistance, ballMask);
                touchBall.transform.position = ray.origin + thisDistance * ray.direction;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                touchBall = null;
                isTouchBall = false;
            }

        }

    }
}
