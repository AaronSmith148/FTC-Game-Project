using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIMovement : PoolableObject
{
    public NavMeshAgent agent;

    public float movementSpeed = 15f;
    public float rotationSpeed = 100f;
    public float isStuckSpeed = 50f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool isStuck = false; //check if it is stuck
    private bool gettingUnstuck = false;

    Rigidbody rb;

    /* For Animations
     * Animator animator;
     */


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();


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


        //Will check if the ai is stuck and starts a coroutine

        if (isStuck == true && gettingUnstuck == false)
        {
            StartCoroutine(Unstuck());
        }

        //Clamp velocity
        if (rb.velocity.magnitude > movementSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, movementSpeed);
        }

        /* For Animations
        * if (isWalking == false) {
        *   animator.SetBool("isRunning", false);
        *  }
        */

    }

    //check to see if it is hitting a wall and stop it
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //Debug.Log("This is an obstacle!");
            // Disable the collider
            this.GetComponent<CapsuleCollider>().enabled = false;
            isStuck = true; //set is stuck to true
        }
    }

    IEnumerator Wander()
    {
        Debug.Log("Starting Wander....");

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

        //if the ai is stuck, break the coroutine
        if (isStuck == true)
        {
            yield break;
        }

        //Wait to turn
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

    //get the AI unstuck
    IEnumerator Unstuck()
    {
        Debug.Log("Starting Unstuck...");
        gettingUnstuck = true;

        int stuckWait = 2;
        float walkWait = 0.5f;
        float rayMax = 4.0f;
        //string targetTag = "Obstacle";

        yield return new WaitForSeconds(stuckWait);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward, transform.forward + rayMax);

        while (isStuck)
        {
            // Perform the raycast
            if (Physics.Raycast(ray, out hit, rayMax) && hit.collider.CompareTag("Obstacle"))
            {
                // Rotate the AI to try and get unstuck
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                // If not stuck anymore, exit the loop
                yield return new WaitForSeconds(stuckWait);
                isWalking = true;
                yield return new WaitForSeconds(walkWait);
                isWalking = false;
                this.GetComponent<CapsuleCollider>().enabled = true;
                gettingUnstuck = false;
                isStuck = false;
                isWandering = false;
            }

            // Update the ray's origin in each iteration to check for obstacles in front
            ray = new Ray(transform.position, transform.forward);
            yield return null; // This allows the coroutine to yield control back to the Update loop
        }
    }
}
