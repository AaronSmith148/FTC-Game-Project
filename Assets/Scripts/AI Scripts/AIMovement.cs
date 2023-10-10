using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    Rigidbody rb;

    /* For Animations
     * Animator animator;
     */


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        /* For Animations
        * animator = GetComponent<Animator>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        if(isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        if (isWalking == true)
        {
            //Calls Rigidbody instead of just moving
            rb.AddForce(transform.forward * movementSpeed);

            /* For Animations
            * animator.SetBool("isRunning", true);
            */
        }
        /* For Animations
        * if (isWalking == false) {
        *   animator.SetBool("isRunning", false);
        *  }
        */

    }

    IEnumerator Wander()
    {
        //Amount of time rotating
        int rotTime = Random.Range(1, 3);

        //Time between rotating
        int rotateWait = Random.Range(1, 3);

        //bool for left or right
        int rotateDirection = Random.Range(1, 2);

        //Amount of time between walking
        int walkWait = Random.Range(1, 3);

        //Amount of time walking
        int walkTime = Random.Range(1, 3);

        isWandering = true;

        //Let it walk
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;

        //Let it turn
        yield return new WaitForSeconds(rotateWait);

        //Which way to turn, right or left
        if (rotateDirection == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateDirection == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }

}
