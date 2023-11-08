using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum HunZhuo
    {
        None,
        Middle,
        Max
    }
    public float thisMovePower = 1;
    public void SetHunZhuo(HunZhuo hunzhuo)
    {
        switch (hunzhuo)
        {
            case HunZhuo.None:
                {
                    thisMovePower = 1;
                }
                break;
            case HunZhuo.Middle:
                {
                    thisMovePower= 0.5f;
                }
                break;
            case HunZhuo.Max:
                {
                    thisMovePower = 0;
                }
                break;
            default:
                break;
        }
    }
    static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }
    // public PlayerController player;
    public Player player;
    public Camera gameCamera;
    public Camera uiCamera;
    public Transform mouseMove;
    public Transform ballMoveParent;
    //Ñ¹Á¦±í
    public PressurePercent pressurePercent;
    bool isOver = false;
    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;
        //player.SetDie();
        StartCoroutine(OverIE());
    }
    public GameObject overShow;
    IEnumerator OverIE()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        isOver = false;
        // overShow.SetActive(true);
        // GameController.Instance.ShowMouseLayer++;
        player.FuHuo();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ä¬ÈÏ²»ÏÔÊ¾Êó±ê Êó±êÓÃÆäËûÌåÏÖ
        ShowMouseLayer = 0;
     //   MouseRefeshPos();
    }
    int showMouseLayer = 0;
    public int ShowMouseLayer
    {
        get
        {
            return showMouseLayer;
        }
        set
        {
            this.showMouseLayer = value;
            if (showMouseLayer > 0)
            {
                ShowMouse();
            }
            else
            {
                HideMouse();
            }
        }
    }
    void ShowMouse()
    {
      //  Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void HideMouse()
    {
        //Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void MouseRefeshPos()
    {
        //ÊÖÖ¸ÔÚÊó±êÎ»ÖÃ
        Vector3 touchPos = Input.mousePosition;
        //Vector3 worldPos= gameCamera.ScreenToWorldPoint(touchPos);
        Vector3 uiPos = uiCamera.ScreenToWorldPoint(touchPos);
        uiPos.z = mouseMove.transform.position.z;
        mouseMove.transform.position = uiPos;
    }
    // Update is called once per frame
    void Update()
    {
        //µ±Íæ¼Ò°´ÏÂ

        //  MouseRefeshPos();
        if (isOver)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetHunZhuo(HunZhuo.None);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetHunZhuo(HunZhuo.Middle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetHunZhuo(HunZhuo.Max);
        }
    }
}
