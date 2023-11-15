using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform haicaoParent;
    public Transform fishParent;
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
    public Transform haicaoQuanParent;
    // public PlayerController player;
    public Player player;
    public Camera gameCamera;
    public Camera uiCamera;
    public Transform mouseMove;
    public Transform ballMoveParent;
    //压力表
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
        //默认不显示鼠标 鼠标用其他体现
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
        //手指在鼠标位置
        Vector3 touchPos = Input.mousePosition;
        //Vector3 worldPos= gameCamera.ScreenToWorldPoint(touchPos);
        Vector3 uiPos = uiCamera.ScreenToWorldPoint(touchPos);
        uiPos.z = mouseMove.transform.position.z;
        mouseMove.transform.position = uiPos;
    }
    public Transform pinkFishPosParent;
    public PinkFish bigPinkFish;
    public StateIcon tipState;
    public void CreateBigFish()
    {
        Vector3 pos = pinkFishPosParent.GetChild(Random.Range(0, pinkFishPosParent.childCount)).position;
        var blackFish = Instantiate(bigPinkFish);
        blackFish.transform.position = pos;
        blackFish.transform.SetParent(fishParent);
    }
    public float createRedFishInterval=10f;
    float createRedFishTimer=0;
    public RedFish redFish_Pre;
    public GameObject wanchengShow;
    public int usePinkCount = 10;
    public Text pinkCountText;
    int pinkCounter = 0;
    public int maxRedCount = 30;
    public void CreateRedFish(Vector3 pos, Quaternion rotation, System.Action<RedFish> createEnd)
    {
        int curCount = 0;
        for (int i = 0; i < fishParent.childCount; i++)
        {
            if (fishParent.GetChild(i).GetComponent<RedFish>())
            {
                curCount++;
            }
        }
        if (curCount < maxRedCount)
        {
            var redfish = Instantiate(redFish_Pre);
            redfish.transform.SetParent(fishParent);
            redfish.transform.position = pos;
            redfish.transform.rotation = rotation;
            createEnd?.Invoke(redfish);
        }
    }
    public int maxPinkCount = 10;
    public PinkFish pinkFish_Pre;
    public void CreatePink(Vector3 pos,Quaternion rotation,System.Action<PinkFish> createEnd)
    {
        int curCount = 0;
        for (int i = 0; i < fishParent.childCount; i++)
        {
            if (fishParent.GetChild(i).GetComponent<PinkFish>())
            {
                curCount++;
            }
        }
        if (curCount<maxPinkCount)
        {
            var pinkFish=Instantiate(pinkFish_Pre);
            pinkFish.transform.SetParent(fishParent);
            pinkFish.transform.position = pos;
            pinkFish.transform.rotation = rotation;
            createEnd?.Invoke(pinkFish);
        }
    }
    public void AddPinkCount()
    {
        pinkCounter++;
        if (pinkCounter>=usePinkCount)
        {
            wanchengShow.SetActive(true);
        }
        pinkCountText.text = pinkCounter + "/"+ usePinkCount;
    }


    // Update is called once per frame
    void Update()
    {
        if (fishParent.GetComponentsInChildren<PinkFish>().Length>=10)
        {
         
        }

        //每过10s创建红鱼
        /*
        createRedFishTimer += Time.deltaTime;
        if (createRedFishTimer> createRedFishInterval)
        {
            createRedFishTimer = 0;
            Vector3 pos = redFishPosParent.GetChild(Random.Range(0, redFishPosParent.childCount)).position;
            var blackFish=Instantiate(redFish_Pre);
            blackFish.transform.position = pos;
            blackFish.transform.SetParent(fishParent);
        }
        */
        //当玩家按下

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


    //鱼群的控制
    //黑鱼速度
    public float BlackSpeedPower_OnceInPink = 0.9f;

    public float BlackMoveSpeedPower
    {
        get
        {
            float speedPower = 1;
            int count=fishParent.GetComponentsInChildren<PinkFish>().Length;
            for (int i = 0; i < count; i++)
            {
                speedPower *= BlackSpeedPower_OnceInPink;
            }
           // Debug.Log("黑鱼速度倍率"+speedPower);
            return speedPower;
        }

    }
    //红鱼速度
    public float RedSpeedPower_OnceInRed= 0.9f;
    public float RedMoveSpeedPower
    {
        get
        {
            float speedPower = 1;
            int count = fishParent.GetComponentsInChildren<RedFish>().Length;
            for (int i = 0; i < count; i++)
            {
                speedPower *= BlackSpeedPower_OnceInPink;
            }
            //Debug.Log("红鱼速度倍率" + speedPower);
            return speedPower;
        }

    }

}
