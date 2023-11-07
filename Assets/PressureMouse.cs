using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureMouse : MonoBehaviour
{
    public Vector2 scaleRange = new Vector2(0.5f,2.0f);
    public bool isDie = false;
    public GameObject dieHide;
    public GameObject maxShow;
    public float maxDie=0.7f;
    public float minDie=0.3f;
    public void Refesh()
    {
        isDie = false;
        dieHide.SetActive(true);
        maxShow.SetActive(false);
    }
    public void SetScale(float percent)
    {
        if (isDie)
        {
            return;
        }
       
        transform.localScale = Vector3.one*Mathf.Lerp(scaleRange.x, scaleRange.y,percent);
        if (percent>maxDie)
        {
           dieHide.SetActive(false);
            isDie = true;
            Debug.Log("сно╥й╖╟э");
            GameController.Instance.GameOver();
        }
        if (percent<minDie)
        {
            dieHide.SetActive(false);
            isDie = true;
            maxShow.SetActive(true);
            Debug.Log("сно╥й╖╟э");
            GameController.Instance.GameOver();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
