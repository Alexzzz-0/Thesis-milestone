using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiCaoSC : MonoBehaviour
{
    public Transform enterPos;
    public BlackFish blackFish = null;
    public GameObject model ;
    public bool isDie = false;
    Collider col;
    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    public void DeleteThis()
    {
        if (isDie)
        {
            return;
        }
        isDie = true;
        model.SetActive(false);
        reviveTimer = 0;
        col.enabled = false;
        blackFish = null;
        //Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float reviveTime = 10.0f;
    float reviveTimer = 0;
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            reviveTimer+=Time.deltaTime;
            if (reviveTimer> reviveTime)
            {
                isDie = false;
                model.SetActive(true);
                col.enabled = true;
            }
        }
    }
}
