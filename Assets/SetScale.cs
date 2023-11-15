using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScale : MonoBehaviour
{
    public float startScale = 1;
    float curScale = 0;
    //public float addScale = 0.3f;
    Player player;
    private void Awake()
    {
        player = GameController.Instance.player;
    }
   
    public int scaleSize = 6;
    public Text scaleCountText;
    public Vector2Int scaleSizeRange = new Vector2Int(2, 6);
    public Vector2 scaleNumRange = new Vector2(1.0f, 2.0f);
    public float GetScale
    {
        get
        {
            float size = scaleNumRange.x + (scaleSize - scaleSizeRange.x) / (float)(scaleSizeRange.y - scaleSizeRange.x) * (scaleNumRange.y - scaleNumRange.x);
            return size;
        }
    }
    public void AddThisScale()
    {
        if (scaleSize==scaleSizeRange.y)
        {
            return;
        }
        scaleSize++;
        transform.localScale = Vector3.one * GetScale;
        Debug.Log("ÅÐ¶Ï·ÛÉ«" + scaleSize + player.curFishType + scaleSizeRange.y);
        switch (player.curFishType)
        {
            case "Black":
                {

                }
                break;
            case "Red":
                {

                }
                break;
            case "Pink":
                {
                    Debug.Log("ÅÐ¶Ï·ÛÉ«" + scaleSize);
                    if (scaleSize == scaleSizeRange.y)
                    {

                        var pink = Instantiate(createPinkFishPre);
                        pink.SetMinScale();
                        pink.transform.SetParent(GameController.Instance.fishParent);
                        pink.transform.position = pinkCreatePinkPos.position;
                        pink.transform.rotation = pinkCreatePinkPos.rotation;

                        GameController.Instance.AddPinkCount();
                    }
                }
                break;
        }
        scaleCountText.text = "" + scaleSize;
    }
    public Transform pinkCreatePinkPos;
    public PinkFish createPinkFishPre;
    public void RemoveThisScale()
    {
        if (scaleSize==scaleSizeRange.x)
        {
            return;
        }
        scaleSize--;
        transform.localScale = Vector3.one * GetScale;
        scaleCountText.text = ""+ scaleSize;
    }
    public Vector2Int scaleSizeRange_Red = new Vector2Int(2, 6);
    public Vector2 scaleNumRange_Red = new Vector2(1.0f, 2.0f);
    public Vector2Int scaleSizeRange_Pink = new Vector2Int(2, 6);
    public Vector2 scaleNumRange_Pink = new Vector2(1.0f, 2.0f);
    public Vector2Int scaleSizeRange_Black = new Vector2Int(2, 6);
    public Vector2 scaleNumRange_Black = new Vector2(1.0f, 2.0f);
    public void ResetScale()
    {
        switch (player.curFishType)
        {
            case "Black":
                {
                    scaleSizeRange = scaleSizeRange_Black;
                }
                break;
            case "Red":
                {
                    scaleSizeRange = scaleSizeRange_Red;
                }
                break;
            case "Pink":
                {
                    scaleSizeRange = scaleSizeRange_Pink;
                }
                break;
        }
        scaleSize=scaleSizeRange.x;
        transform.localScale = Vector3.one * GetScale;
        scaleCountText.text = "" + scaleSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
