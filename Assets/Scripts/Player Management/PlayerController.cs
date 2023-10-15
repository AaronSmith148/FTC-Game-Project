using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= movementSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
