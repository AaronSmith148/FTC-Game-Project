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
    }

    private void OnMove(CallbackContext context)
    {
        if(playerController != null)
        {
            playerController.SetInputVector(context.ReadValue<Vector2>());
        }
    }
}
