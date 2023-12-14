using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState : MonoBehaviour
{

    public PlayerMovementSM _player;
    

    public ActionState(PlayerMovementSM player)
    {
        //_player = player;
    }

    public virtual void setEntity(PlayerMovementSM player)
    {
        _player = player;
    }
    public virtual void OnInitialize(PlayerMovementSM player)
    {
        _player = player;
    }

    public abstract void OnEnter();
    public abstract ActionState OnUpdate();
    public abstract void OnExit();

}
