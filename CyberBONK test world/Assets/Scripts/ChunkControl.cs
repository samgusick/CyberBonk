using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkControl : MonoBehaviour
{
    public List<GameObject> NPCs = new List<GameObject>();

    public void enableNPCs()
    {
        foreach (var item in NPCs)
        {
            item.SetActive(true);
        }
    }
}
