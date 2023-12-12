using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnerHerring : MonoBehaviour
{
    //I KNOW THIS SCRIPT IS REDUNDANT. Combining them gives many crazy errors. If it ain't broke don't fix it
    public int NumberOfAIToSpawn;
    public List<AIMovement> AIPrefabs = new List<AIMovement>();

    private UnityEngine.AI.NavMeshTriangulation Triangulation;
    private Dictionary<int, ObjectPool> AIObjectPools2 = new Dictionary<int, ObjectPool>();

    public int minAI;
    public int maxAI;

    public int fixY = 1;

    private void Awake()
    {
        NumberOfAIToSpawn = Random.Range(minAI, maxAI);
        for (int i = 0; i < AIPrefabs.Count; i++)
        {
            AIObjectPools2.Add(i, ObjectPool.CreateInstance(AIPrefabs[i], NumberOfAIToSpawn));
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
        PoolableObject poolableObject = AIObjectPools2[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            AIMovement AIMovement = poolableObject.GetComponent<AIMovement>();

            int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

            UnityEngine.AI.NavMeshHit Hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, 1))
            {
                AIMovement.agent.Warp(Hit.position);
                // Set the Y value of the instance
                AIMovement.transform.position = new Vector3(AIMovement.transform.position.x, fixY, AIMovement.transform.position.z);
                AIMovement.agent.enabled = false;
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch AI of type {SpawnIndex} from object pool");
        }
    }
}
