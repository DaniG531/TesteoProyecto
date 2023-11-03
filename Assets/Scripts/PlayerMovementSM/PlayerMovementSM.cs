using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSM : MonoBehaviour
{
    ActionState _currentState;

    //Todos los estados
    public ActionState movingState;
    public ActionState stuntState;

    //Variables

    ActionState CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            //value es el stado asignado
            if (_currentState != value)
            {
                if (_currentState != null)
                {
                    _currentState.OnExit();
                }
                _currentState = value;
                _currentState.OnEnter();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //idleState = gameObject.AddComponent<IdleState>();
        //idleState.OnInitialize(this);
        movingState = new MovingState(this);
        stuntState = new StuntState(this);
        _currentState = movingState;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState = CurrentState.OnUpdate();
        Debug.Log(CurrentState);
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}