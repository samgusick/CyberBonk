using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerScript : MonoBehaviour
{
    public GameObject npcPrefab;
    public int NumberOfNPCs;
    public float spawnRadius;


    private void Update()
    {
        transform.position = PlayerManager.player.transform.position;
    }

    private void Awake()
    {
        startTheSpawn();
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        while (NumberOfNPCs < 200)
        {
            Vector3 spawnPosition;
            spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
            for (int i = 0; i < 2; i++)
            {
                Instantiate(npcPrefab, spawnPosition, npcPrefab.transform.rotation);
                NumberOfNPCs++;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(Random.Range(0, 25));
        }
        yield break;
    }


public void startTheSpawn()
{
    StartCoroutine(Spawn());
}

}
