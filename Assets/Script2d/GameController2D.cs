using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2D : MonoBehaviour
{
    public enum HunZhuo
    {
        无,
        轻,
        重
    }
    public void SetHunZhuo(HunZhuo hunzhuo)
    {
        switch (hunzhuo)
        {
            case HunZhuo.无:
                {
                    player.actionPower = 1;
                }
                break;
            case HunZhuo.轻:
                {
                    player.actionPower = 0.5f;
                }
                break;
            case HunZhuo.重:
                {
                    player.actionPower = 0;
                }
                break;
            default:
                break;
        }
    }
    static GameController2D _instance;
    public static GameController2D Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = FindObjectOfType<GameController2D>();
            }
            return _instance;
        }
    }
    public Player2DMove player;
    public Camera gameCamera;
    public Camera uiCamera;
    public Transform mouseMove;
    //压力表
    public PressurePercent pressurePercent;

    // Start is called before the first frame update
    void Start()
    {
        //默认不显示鼠标 鼠标用其他体现
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        MouseRefeshPos();
    }
    void MouseRefeshPos()
    {
        //手指在鼠标位置
        Vector3 touchPos = Input.mousePosition;
        //Vector3 worldPos= gameCamera.ScreenToWorldPoint(touchPos);
        Vector3 uiPos = uiCamera.ScreenToWorldPoint(touchPos);
        uiPos.z = mouseMove.transform.position.z;
        mouseMove.transform.position = uiPos;
    }
    // Update is called once per frame
    void Update()
    {
        //当玩家按下

        MouseRefeshPos();


    }
}
