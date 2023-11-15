using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBody : MonoBehaviour
{
    RedFish redFish;
    private void Awake()
    {
        redFish = GetComponentInParent<RedFish>();
    }
    public Transform createPos;
    public FishBase createFishPre;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BlackFish>())
        {
            var fish=Instantiate(createFishPre);
            fish.transform.SetParent(GameController.Instance.fishParent);
            fish.transform.position = createPos.position;
            fish.transform.rotation = createPos.rotation;
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
