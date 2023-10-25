using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;

    public Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        if(moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();

            transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);

            transform.forward = moveDirection;
        }
       
    }
}
