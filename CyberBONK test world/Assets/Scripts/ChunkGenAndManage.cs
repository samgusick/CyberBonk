using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class ChunkGenAndManage : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    static float[,] noiseMap;

    public float windowIntensity;
    public mapDisplay display;
    public Vector2 testCoordinates = new Vector2();
    public int ChunksPerBlock = 5;
    public GameObject ChunkPrefab;
    public GameObject buildingblock;
    public GameObject topBlock;
    public GameObject window;
    public Material buildingMat;
    public Material windowMat;

    public Gradient windowGradient;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();

    
    public void DrawMapInEditor(){
        //MapData mapData = GenerateMapData();
        //display.DrawNoiseMap(mapData.heightMap);
    }
    
    public void RequestMapData(Action<MapData> callback) {
        ThreadStart threadStart = delegate {
            MapDataThread(callback);
        };

        new Thread(threadStart).Start();
    }


    MapData GenerateMapData()
    {
        //noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        return new MapData(noiseMap);
    }

    void MapDataThread(Action<MapData> callback) {
        MapData MapData = GenerateMapData();
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, MapData));
        }
    }

    void Update() {
        if (mapDataThreadInfoQueue.Count > 0)

        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    struct MapThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo (Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }


}

[System.Serializable]
public struct TerrainType {
     public string name;
    }

public struct MapData {
    public float[,] heightMap;

    public MapData(float [,] heightMap)
    {
        this.heightMap = heightMap;
    }

}
