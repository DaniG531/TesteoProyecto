using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : ActionState
{
	//float speed = Vector3.Magnitude(_entity.transform.rigidBody.velocity)

	public MovingState(PlayerMovementSM player) : base(player)
	{
		 
	}

	public override void setEntity(PlayerMovementSM player)
	{
		base.setEntity(player);
		
	}

	public override void OnEnter()
	{
        GameObject camera = GameObject.Find("Camera");
        cameraTransform = camera.transform;
		
	}

	public override void OnExit()
	{

	}

	public override ActionState OnUpdate()
	{
		moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDir = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
		moveDir = moveDir.normalized * m_currSpeed;

		moveDir.y = m_rb.velocity.y;
		m_rb.velocity = moveDir;
		//

		Vector3 LookDir = moveDir;
		LookDir.y = 0;
		Quaternion targetRotation = Quaternion.LookRotation(LookDir);
		_player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, targetRotation, Time.deltaTime * m_rotataionSpeed);

		return this;
	}
}