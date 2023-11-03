using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType
{
    kNone,
    kLookAtMoveDir,
    kLookAtMouse,
}

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float m_currSpeed = 1.0f;
    public float m_walkSpeed = 7.0f;
    public float m_runSpeed = 13.0f;
    public float m_rotataionSpeed = 6.0f;
    public float m_pushForce = 1.0f;

    [Header("Groundcheck & Jump")]
    public LayerMask m_floorLayer;

    public float jumpSpeed = 8.0f;
    public float gravity = 9.8f;
    bool m_grounded = false;
    [SerializeField]
    private float jumpTimeCounter;
    [SerializeField]
    public float jumpTime;
    private bool isJumping;

    [Header("Dash")]
    public bool m_isDashing = false;
    public bool m_canDash = true;
    public float m_dashDuration = 0.15f;
    public float m_dashCooldown = 0.4f;
    public float m_dashCooldownCounter;
    public float m_dashTimeCounter = 0.0f;
    public float m_dashSpeed = 60.0f;

    [Header("SFX & VFX")]
    public AudioClip m_jumpSound;
    public GameObject m_dustParticle;
    public GameObject m_sprintParticle;
    private ParticleSystem m_sprintParticleSpawned;

    [Header("Misc.")]
    public bool IgnoreLookDown;
    public RotationType m_rotType;
    Rigidbody m_rb;
    [SerializeField]
    private Transform cameraTransform;
    private Vector3 moveDir = Vector3.zero;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        m_currSpeed = m_walkSpeed;
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
        {
            m_rb = gameObject.AddComponent<Rigidbody>();
        }
        

        
    }


    void Update()
    {
        //if (m_health.m_currentHealth <=0)
        //{
        //   return;
        //}
        //Running
        //////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_currSpeed = m_runSpeed;
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_currSpeed = m_walkSpeed;
            
        }

        //GroundCheck
        //////////////////////////////////////////
        

        else
        {
            m_grounded = false;
        }

        //Jump
        //////////////////////////////////////////
        if (m_grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                //m_rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            m_grounded = false;
            if (jumpTimeCounter > 0.0)
            {
                m_rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            }

            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            }
        }

        //Dash
        //////////////////////////////////////////
        if (Input.GetKey(KeyCode.Mouse0) && m_canDash)
        {
            Dash();
            m_canDash = false;
            

        }
        m_dashCooldownCounter += Time.deltaTime;
        if (m_dashCooldownCounter >= m_dashCooldown)
        {
            m_canDash = true;
        }
        else
        {
            m_canDash = false;
        }

        if (m_isDashing)
        {
            m_dashTimeCounter += Time.deltaTime;
            if (m_dashTimeCounter >= m_dashDuration)
            {
                m_currSpeed = m_walkSpeed;
                m_isDashing = false;
                m_pushForce = 1.0f;
                m_dashCooldownCounter = 0.0f;
            }
        }

        //Movement
        //////////////////////////////////////////
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDir = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
        moveDir = moveDir.normalized * m_currSpeed;

        moveDir.y = m_rb.velocity.y;
        m_rb.velocity = moveDir;


        //CharacterRotation
        //////////////////////////////////////////
        switch (m_rotType)
        {
            case RotationType.kNone:
                break;
            case RotationType.kLookAtMoveDir:
                if (moveDir != Vector3.zero)
                {
                    Vector3 LookDir = moveDir;
                    if (IgnoreLookDown)
                    {
                        LookDir.y = 0;
                    }
                    // calcular rotacion en base a la direccion que nos estamos moviendo
                    Quaternion targetRotation = Quaternion.LookRotation(LookDir);
                    //Interpolar la rotacion actual con la rotacion deseada para dar efecto de suavizado
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_rotataionSpeed);
                }
                break;
            case RotationType.kLookAtMouse:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // obtener rayo
                Plane plane = new Plane(Vector3.up, Vector3.zero); // crear plano
                float collisionDistance;
                Vector3 point = Vector3.zero;
                if (plane.Raycast(ray, out collisionDistance)) // si el rayo choca con el plano, obtener distancia de choque
                {
                    point = ray.GetPoint(collisionDistance); //obtener punto a la distancia en que choco el rayo con el plano
                }
                Vector3 pointDirection = point - transform.position; // calcular direccion del personaje hacia el punto de choque
                pointDirection.y = 0.0f;
                Quaternion targetPointRotation = Quaternion.LookRotation(pointDirection); // crear direccion
                transform.rotation = Quaternion.Lerp(transform.rotation, targetPointRotation, Time.deltaTime * m_rotataionSpeed); //aplicar y suavizar rotacion
                break;
            default:
                break;


            
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRb = hit.gameObject.GetComponent<Rigidbody>();
        if (hitRb != null)
        {
            hitRb.AddForce(-hit.normal * m_pushForce, ForceMode.Impulse);
        }
     
    }
    
    public void Dash()
    {
        if (m_isDashing == false)
        {
            m_isDashing = true;
            m_pushForce = 6.0f;
            m_dashTimeCounter = 0.0f;
            m_currSpeed = m_dashSpeed;
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            m_grounded = true;
        }
    }
}
