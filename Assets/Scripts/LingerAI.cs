using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LingerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float lingerTime = 5;
    public float range; //radius of sphere

    private Transform centerPoint; //center of the area agent walks around in

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        centerPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if its done with the path
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if(lingerTime > 0)
            {
                lingerTime-= Time.deltaTime;
            }
            else
            {
                Vector3 point;
                //give it the center and the radius
                if (RandomPoint(centerPoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //for testing purposes
                    agent.SetDestination(point);
                    lingerTime = 5;
                }
                
            }
            
        }
        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            //make the random point
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }
    }

    
}
