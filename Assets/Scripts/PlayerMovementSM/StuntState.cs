using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntState : ActionState
{
	//float speed = Vector3.Magnitude(_entity.transform.rigidBody.velocity)
	[Header("Needed Components.")]
	public CharacterController controller;
	public Transform cameraTransform;
	public Vector3 playerVelocity;
	[Range(2, 10)] public float playerSpeed = 6.5f;
	public float gravityValue = -50.0f;

	[Header("Stunt Variables.")]
	[SerializeField] public bool groundedPlayer;
	public float stunnedTime = 1.5f;
	public float timer = 0;
	
	public StuntState(PlayerMovementSM player) : base(player)
	{
	}

	public override void setEntity(PlayerMovementSM player)
	{
		base.setEntity(player);
	}

	public override void OnEnter()
	{
		controller = _player.charController;
		GameObject camera = GameObject.Find("Camera");
		cameraTransform = camera.transform;
		playerVelocity = Vector3.zero;
		stunnedTime = 1.5f;
		timer = 0;

	}

	public override void OnExit()
	{
		_player.canRun = true;
	}

	public override ActionState OnUpdate()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer)
		{
			_player.move *= 0.95f;
			timer += 1.0f * Time.deltaTime;
			if (timer >= stunnedTime)
			{
				return _player.movingState;
			}
		}

		if (_player.move != Vector3.zero)
		{
			_player.gameObject.transform.forward = _player.move;
			controller.Move(_player.move * Time.deltaTime * playerSpeed);
		}

		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}
		
		//_player.move = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * _player.move;
		//_player.move.y = 0;

		_player.stamina += 1.0f * Time.deltaTime;
		if (_player.stamina >= 9.9)
		{
			_player.stamina = 10;
			_player.canRun = true;
		}


		if (Input.GetKeyDown(KeyCode.L))
		{
			return _player.movingState;
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);

		return this;
	}
}