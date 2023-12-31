using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSM : MonoBehaviour
{
    ActionState _currentState;

    [Header("Needed Components.")]
    public ActionState movingState;
    public ActionState stuntState;

    [Header("Needed Components.")]
    public CharacterController charController;


    [Header("Needed Variants.")]
    public Vector3 playerVelocity;
    [Range(0, 10)] [SerializeField] public float stamina;
    [SerializeField] public bool canRun;
    public Vector3 move;


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

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //idleState = gameObject.AddComponent<IdleState>();
        //idleState.OnInitialize(this);
        //movingState = new MovingState(this);
        //stuntState = new StuntState(this);
        CurrentState = movingState;

        

    }

    // Update is called once per frame
    void Update()
    {

        CurrentState = CurrentState.OnUpdate();
        //Debug.Log(CurrentState);
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}