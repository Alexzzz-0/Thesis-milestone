using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiCaoCircle : MonoBehaviour
{
    public Transform haiCaoParent;
    public float Radiu = 10;
   public  LayerMask groundLayer;
    public Transform redFishRandomMoveParent;
    public Vector3[] GetRedFishPos()
    {
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < redFishRandomMoveParent.childCount; i++)
        {
            posList.Add( redFishRandomMoveParent.GetChild(i).position);
        }
        return posList.ToArray();
    }
    void ResetHaiCao()
    {
        int parentCount = haiCaoParent.childCount;
        float oneAngle = 360.0f / (parentCount);

        for (int i = 0; i < haiCaoParent.childCount; i++)
        {
            int j = i;
            float angle = oneAngle * j;
            float pi = (angle) / 180.0f;
            Vector3 localPos = new Vector3(Mathf.Sin(pi * Mathf.PI), 0, Mathf.Cos(pi * Mathf.PI)) * Radiu;
            // Debug.Log(j+" "+pi + "  "+localPos);
            Vector3 pos = transform.position + localPos + Vector3.up * 100;
            RaycastHit hit;
            bool isRay = Physics.Raycast(pos, Vector3.down, out hit, 100000, groundLayer);
            if (isRay)
            {
                haiCaoParent.GetChild(j).transform.position = hit.point;
            }
            else
            {
                haiCaoParent.GetChild(j).transform.position =pos-Vector3.up*100;
            }

        }
    }
    private void OnValidate()
    {
        ResetHaiCao();
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetHaiCao();
    }

    // Update is called once per frame
    void Update()
    {
      //  ResetHaiCao();
    }
}
