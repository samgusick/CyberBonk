using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerScript : MonoBehaviour
{
    public GameObject npcPrefab;
    public int NumberOfNPCs;
    public float spawnRadius;
    
    private void Start() {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        while(true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(2);
                for (int x = 0; x < NumberOfNPCs; x++)
                {
                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
                GameObject NPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
                }

            }
            yield break;
        }
    }
}
