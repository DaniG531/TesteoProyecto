using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : ActionState
{
    [Header("Needed Components.")]
    
    //public Rigidbody playerRB;
    public Transform cameraTransform;
    public Vector3 playerVelocity;
    //public Vector3 move;

    [Header("Movement Variables.")]
    [SerializeField] public bool groundedPlayer;
    [Range(2, 10)] public float playerSpeed = 6.5f;
    [Range(1, 5)] public float playerRunSpeed = 8;
    [Range(1, 5)] public float jumpHeight = 2f;
    public float gravityValue = -50.0f;
    public CharacterController controller;

    [Header("Stamina.")]

    public bool isParrot = true;
    public bool canDJump = true;
    public bool Stunned = false;

    public MovingState(PlayerMovementSM player) : base(player)
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
        Stunned = false;
    }

	public override void OnExit()
	{
       // playerVelocity = _player.move;
	}

	public override ActionState OnUpdate()
	{

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        _player.move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _player.move = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * _player.move;
        _player.move.y = 0;
        Debug.Log(playerVelocity);
        if (_player.move != Vector3.zero)
        {
            _player.gameObject.transform.forward = _player.move;
            controller.Move(_player.move * Time.deltaTime * playerSpeed);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _player.move.magnitude > 0.75)
        {
            if (_player.canRun)
            {
                controller.Move(_player.move * Time.deltaTime * playerRunSpeed);
                _player.stamina -= 3.0f * Time.deltaTime;
                if (_player.stamina <= 0)
                {
                    _player.stamina = 0;
                    _player.canRun = false;
                }
            }
            else
            {
                _player.stamina += 1.0f * Time.deltaTime;
                if (_player.stamina >= 9.9)
                {
                    _player.stamina = 10;
                    _player.canRun = true;
                }
            }

        }
        else
        {
            _player.stamina += 1.0f * Time.deltaTime;
            if (_player.stamina >= 9.9)
            {
                _player.stamina = 10;
                _player.canRun = true;
            }
        }


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            
            if (_player.canRun && _player.stamina > 3)
            {
                _player.stamina -= 3.0f;
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                
                
            }
            

        }
        if (isParrot && canDJump)
        {
            if (Input.GetButtonDown("Jump") && !groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.5f * gravityValue);
                canDJump = false;
            }
        }
        if (groundedPlayer)
        {
            canDJump = true;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (Stunned)
        {

            return _player.stuntState;  
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            return _player.stuntState;
        }

        return this;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAttack"))
        {

            Stunned = true;
        }
    }
}
