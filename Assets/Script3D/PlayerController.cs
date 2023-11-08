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
        Debug.Log("����"+rig);
        rig.velocity = Vector3.zero;
    }
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        touchMouse.SetState("Ĭ��");
    }
    //�ƶ�
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
        //rig.velocity =transform.forward* dir*moveSpeed*GameController.Instance.thisMovePower;
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
   
    //��ת
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
    //�������½�
    public float upSpeed = 10;



    //��ѹ��ѹ
    public float curPressure = 0.5f;
    public float addPressurePercentSpeed_Move = 0.5f;
    public float addPressurePercentSpeed_Touch = 0.7f;
    //�����ѹ��ѹ
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
    [Header("touchDistance")]
    public float touchDistance = 100;
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            return;
        }

        #region Forward&Backward Movement

        // //Jayde
        // float v = Input.GetAxis("Vertical");
        // if (v == 0)
        // {
        //     SetDirMove(0);
        // }
        // else
        // {
        //     SetDirMove((int)Mathf.Sign(v));
        // }
        //
        // rig.velocity = transform.forward * thisMoveDir * moveSpeed * GameController.Instance.thisMovePower;
        
        //Alex
        rig.velocity = transform.forward * moveSpeed;


        #endregion
       
        float h = Input.GetAxis("Horizontal");
        if (h == 0)
        {
            SetRotate(0);
        }
        else
        {
            SetRotate((int)Mathf.Sign(h));
        }
        //��ת
        transform.Rotate(Vector3.up*rotatePower*Time.deltaTime*rotateSpeed* GameController.Instance.thisMovePower);
        //�����½�
       
        transform.position += Vector3.up * upSpeed * Time.deltaTime* vMoveSpeed * GameController.Instance.thisMovePower;
        
        //���λ������
        if (Vector3.Distance(transform.position,playerRangeMiddle.position)>moveRange)
        {
            Vector3 dir = (transform.position - playerRangeMiddle.position ).normalized;
            transform.position = playerRangeMiddle.position + dir * moveRange;
        }
        if (Vector3.Distance(transform.position, playerRangeMiddle.position)== moveRange)
        {
            vMoveSpeed = 0;
        }
        //ѹ��ֵ����
        if (vMoveSpeed != 0)
        {
            //����ѹ��ֵ
            var Add = addPressurePercentSpeed_Move * vMoveSpeed * Time.deltaTime;
            curPressure -= Add;
           
            //��С������ѹ��ֵ
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
          
            //�ֶ���ѹ��ѹ
            if (Input.GetKey(KeyCode.Q))
            {
                AddZengYa(addPressurePercentSpeed_Touch * Time.deltaTime);
                Debug.Log("����q");
            }
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("����e");
                AddZengYa(-addPressurePercentSpeed_Touch * Time.deltaTime);
            }
           
        }
        else
        {
            //�Զ��ظ�ƽ��ֵ
           // curPressure = 0.5f;
        }
       


        GameController.Instance.pressurePercent.SetValue(curPressure);
        mouse.SetScale(curPressure);

        //�ٿ�С��
        {
            //����������� �Ϳ�����קС��
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                //�������� ���С��
                RaycastHit hit;
                bool isRay = Physics.Raycast(ray, out hit, touchDistance, ballMask);
                if (isRay)
                {
                    thisDistance = hit.distance;
                    Debug.Log("���߼�⵽��" + hit.transform.name + hit.distance);
                    isTouchBall = true;
                    touchBall = hit.transform.GetComponent<TouchMoveBangDingPlayer>();
                    touchBall.isTouch = true;
                    touchMouse.SetState("��ק");
                }
            }
            if (touchBall==null)
            {
                touchBall = null;
                isTouchBall = false;
                touchMouse.SetState("Ĭ��");
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
                touchMouse.SetState("Ĭ��");
            }

        }
        //���䲨
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var bt = Instantiate(waveBulletPre);
                //�̶�����
                /*
                {
                    bt.transform.position = firePos.position;

                    bt.transform.rotation = firePos.rotation;
                    bt.SetMove(firePos.forward);
                }
                */
                {
                    //��ȡ���ָ��
                    Ray ray = GameController.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
                    bt.transform.position = firePos.position;
                    bt.transform.forward = ray.direction.normalized;
                    bt.SetMove(ray.direction.normalized);
                }
            }
        }
    }
}
