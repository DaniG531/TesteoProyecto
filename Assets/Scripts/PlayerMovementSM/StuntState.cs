using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntState : ActionState
{
	//float speed = Vector3.Magnitude(_entity.transform.rigidBody.velocity)

	public StuntState(PlayerMovementSM player) : base(player)
	{
	}

	public override void setEntity(PlayerMovementSM player)
	{
		base.setEntity(player);
	}

	public override void OnEnter()
	{
		
	}

	public override void OnExit()
	{

	}

	public override ActionState OnUpdate()
	{



		return this;
	}
}