using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfig playerConfig;
    private PlayerController playerController;

    private PlayerInputSystem controls;
    private bool canPunch = true;
    private float punchtimer = 2f;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        controls = new PlayerInputSystem();
    }

    public void InitializePlayer(PlayerConfig pc)
    {
        playerConfig = pc;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.Player.Move.name)
        {
            OnMove(obj);
        }
        else if (obj.action.name == controls.Player.Punch.name && canPunch)
        {
            OnPunch(obj);
        }
    }

    private void OnMove(CallbackContext context)
    {
        if (playerController != null)
        {
            playerController.SetInputVector(context.ReadValue<Vector2>());
        }
    }

    private void OnPunch(CallbackContext context)
    {
        if (playerController != null)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Debug.Log("OnPunchCalled");
            if(Physics.Raycast(ray, out RaycastHit hit, 1))
            {
                Debug.Log(hit.collider.gameObject.name + " was hit");
                if(hit.collider.gameObject.tag == "Player")
                {
                    playerConfig.PlayerScore += 1;
                    Destroy(hit.collider.gameObject);
                    canPunch = false;
                }
                else if (hit.collider.gameObject.tag == "Bot")
                {
                    playerConfig.PlayerScore -= 1;
                    Destroy(hit.collider.gameObject);
                    canPunch = false;
                }
            }
        }
    }

    private void Update()
    {
        if (!canPunch)
        {
            if(punchtimer >= 0)
            {
                punchtimer -= Time.deltaTime;
            }
            else
            {
                canPunch = true;
                punchtimer = 2f;
            }
        }
    }
}
