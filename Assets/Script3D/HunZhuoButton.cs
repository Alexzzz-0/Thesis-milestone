using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HunZhuoButton : MonoBehaviour,IPointerClickHandler
{
    public GameController.HunZhuo hunzhuo;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameController.Instance.SetHunZhuo(hunzhuo);
    }

    private void Awake()
    {
        

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
