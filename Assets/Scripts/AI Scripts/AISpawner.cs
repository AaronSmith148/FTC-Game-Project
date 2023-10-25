using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public int NumberOfAIToSpawn;
    public List<PatrolAI> AIPrefabs = new List<PatrolAI>();

    private UnityEngine.AI.NavMeshTriangulation Triangulation;
    private Dictionary<int, ObjectPool> AIObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        NumberOfAIToSpawn = Random.Range(5, 9);
        for (int i = 0; i < AIPrefabs.Count; i++)
        {
            AIObjectPools.Add(i, ObjectPool.CreateInstance(AIPrefabs[i], NumberOfAIToSpawn));
        }
    }

    private void Start()
    {
        //Probably doesn't need to be a couroutine but it was safer to do it this way
        Triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
        StartCoroutine(SpawnAI());
    }

    private IEnumerator SpawnAI()
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);
        int SpawnedAI = 0;

        while (SpawnedAI < NumberOfAIToSpawn)
        {
            SpawnRandomAI();
            SpawnedAI++;
            yield return Wait;
        }
    }

    private void SpawnRandomAI()
    {
        DoSpawnAI(Random.Range(0, AIPrefabs.Count));
    }

    private void DoSpawnAI(int SpawnIndex)
    {
        PoolableObject poolableObject = AIObjectPools[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            PatrolAI patrolAI = poolableObject.GetComponent<PatrolAI>();

            int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

            UnityEngine.AI.NavMeshHit Hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, 1))
            {
                patrolAI.agent.Warp(Hit.position);
                //Set the centerpoint and range here?
                patrolAI.agent.enabled = true;
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch AI of type {SpawnIndex} from object pool");
        }
    }
}
