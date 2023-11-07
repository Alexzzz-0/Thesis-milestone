using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIcon : MonoBehaviour
{
    [System.Serializable]
    public class StateConfig
    {
        public string stateName = "״̬";
        public GameObject stateShow;
    }
    [SerializeField]
    StateConfig[] states;
    string lastState;
    public void SetState(string stateName)
    {
        //Debug.Log(stateName+"");
        if (stateName==lastState)
        {
            return;
        }
        foreach (var state in states)
        {
            state.stateShow.SetActive(false);
        }
        foreach (var state in states)
        {
            if (state.stateName== stateName)
            {
                state.stateShow.SetActive(true);
            }
            
        }
        lastState = stateName;
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
