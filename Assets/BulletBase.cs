using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public Vector2 sizeRange = new Vector2(0.5f, 3.0f);
    public Vector2 foreRange = new Vector2(150, 0);
    public float moveTime = 3f;
    float moveTimer = 0;
    public float AllDistance = 5;
    Vector3 startPos;
    Vector3 moveDir;
    float curDistance = 0;
    public float forcePowerToFish = 1000;
    public void SetMove(Vector3 dir)
    {
        startPos = transform.position;
        curDistance = 0;
        moveDir = dir;
        moveTimer = 0;
        transform.localScale = Vector3.one * Mathf.Lerp(sizeRange.x, sizeRange.y, (moveTimer / moveTime));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    float GetForce
    {
        get
        {
            return Mathf.Lerp(foreRange.x, foreRange.y, moveTimer / moveTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        float movePercent = moveTimer / moveTime;
        transform.position = startPos + moveDir * movePercent * AllDistance;
        transform.localScale = Vector3.one * Mathf.Lerp(sizeRange.x, sizeRange.y, (moveTimer / moveTime));
        if (moveTime < moveTimer)
        {
            Destroy(this.gameObject, 0.25f);
        }
    }
}
