using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerScript : MonoBehaviour
{
    public GameObject npcPrefab;
    public int NumberOfNPCs;
    public float spawnRadius;

    private void Update() {
        transform.position = PlayerManager.player.transform.position;
    }

    private void Start() {
        startTheSpawn();
    }

    public IEnumerator Spawn()
    {
        Vector3 spawnPosition;
        
        yield return new WaitForSeconds(3);
        for (int i = -1; i < 1; i++)
        {
            for (int l = -1; l < 1; l++)
            {
                spawnPosition = new Vector3(transform.position.x + (i * 10), 0, transform.position.z + (l * 10));
                Instantiate(npcPrefab, spawnPosition, npcPrefab.transform.rotation);
                yield return new WaitForEndOfFrame();
            }
        }
        yield break;
    }

    public void startTheSpawn(){
        StartCoroutine(Spawn());
    }

}
