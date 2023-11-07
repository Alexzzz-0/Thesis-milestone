using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCatch : MonoBehaviour
{
    public float minCatch = 2.5f;//吃东西最短距离
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
  //  public PeopleUI people;
   // public Text tipText;
   // public GameObject tipGo;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log(Camera.main.transform.name);
            //发射射线，
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward); Camera.main.ScreenPointToRay(Input.mousePosition);
            if (GameController.Instance.ShowMouseLayer > 0)
            {
                return;
            }
            RaycastHit[] hit = Physics.RaycastAll(ray, minCatch);
            Debug.DrawRay(ray.origin, ray.direction* minCatch,Color.red,minCatch);

         
        }
    }
}
