using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionState
{

    public PlayerMovementSM _player;
    public float m_currSpeed = 0.0f;
    public float m_rotataionSpeed = 6.0f;
    public Rigidbody m_rb;
    public Transform cameraTransform;
    public Vector3 moveDir = Vector3.zero;

    public ActionState(PlayerMovementSM player)
    {
        _player = player;
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
