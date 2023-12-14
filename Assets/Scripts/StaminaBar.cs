using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider StaminaBarObject;
    public float maxStamina = 10.0f;
    public float curStamina;
    PlayerMovementSM Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PlayerTest").GetComponent<PlayerMovementSM>();
    }

    // Update is called once per frame
    void Update()
    {
        curStamina = Player.stamina;
        StaminaBarObject.value = curStamina / maxStamina;
        
        
    }
}
