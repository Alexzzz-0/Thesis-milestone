using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        transform.parent = GameController.Instance.fishParent;
    }
    public float moveSpeed = 10;
    //public float thisScale = 1;
    //public float addScale = 0.5f;
    protected Rigidbody rig;
    // Start is called before the first frame update
    protected virtual void Start()
    {
       
    }
    protected virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        scaleCountText.text = "" + scaleSize;

        transform.localScale = Vector3.one * GetScale;
    }
    public int scaleSize = 4;
    public Text scaleCountText;
    public Vector2Int scaleSizeRange = new Vector2Int(2,4);
    public Vector2 scaleNumRange = new Vector2(1.0f,2.0f);
    public float GetScale
    {
        get
        {
            float size = scaleNumRange.x + (scaleSize - scaleSizeRange.x) / (float)(scaleSizeRange.y - scaleSizeRange.x) * (scaleNumRange.y - scaleNumRange.x);
            return size;
        }
    }
    public void SetMinScale()
    {
        scaleSize = scaleSizeRange.x;
        transform.localScale = Vector3.one * GetScale;
        scaleCountText.text = "" + scaleSize;
    }
    public void AddScale()
    {
        if (scaleSize== scaleSizeRange.y)
        {
            return;
        }
        scaleSize++;
        transform.localScale = Vector3.one * GetScale;
        scaleCountText.text = "" + scaleSize;
    }
    public virtual void LoseScale()
    {
        if (scaleSize == scaleSizeRange.x)
        {
            return;
        }
        scaleSize--;
        transform.localScale = Vector3.one * GetScale;
        scaleCountText.text = "" + scaleSize;
        if (scaleSize==0)
        {
            DeleteThis();
        }
    }
    public virtual void DeleteThis()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
