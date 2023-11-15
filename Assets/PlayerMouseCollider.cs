using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseCollider : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = GameController.Instance.player;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (player.curFishType)
        {
            case "Black":
                {
                    //≈ˆµΩ”„Õ∑ À¿Õˆ
                    if (other.GetComponent<PinkHead>())
                    {
                        player.SetFishDie();
                    }
                }
                break;
            case "Red":
                {

                }
                break;
            case "Pink":
                {

                }
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
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

                }
                break;
            default:
                break;
        }
    }
}
