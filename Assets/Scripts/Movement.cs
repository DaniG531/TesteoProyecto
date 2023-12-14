using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Needed Components.")]
    public CharacterController controller;
    public Transform cameraTransform;
    private Vector3 playerVelocity;

    [Header("Movement Variables.")]
    [SerializeField] private bool groundedPlayer;
    [Range(2, 10)] public float playerSpeed;
    [Range(1, 3)] public float playerRunSpeed;
    [Range(1, 5)] public float jumpHeight = 2f;
    private float gravityValue = -50.0f;

    [Header("Stamina.")]
    [Range(0, 10)] [SerializeField] float stamina;
    [SerializeField] bool canRun;

    private void Start()
    {
        stamina = 10;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * move;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            controller.Move(move * Time.deltaTime * playerRunSpeed);
            stamina -= 3.0f * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                canRun = false;
            }
        }
        else
        {
            stamina += 1.0f*Time.deltaTime;
            if (stamina >= 9.9)
            {
                stamina = 10;
                canRun = true;
            }
            
        }


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            if (stamina < 3)
            {
                return;
            }
            else
            {
                if (canRun)
                {
                    stamina -= 3.0f;
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                }
            }
            
        }

        
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}