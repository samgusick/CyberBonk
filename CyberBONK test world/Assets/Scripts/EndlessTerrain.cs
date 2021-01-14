using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class EndlessTerrain : MonoBehaviour
{
    public float buildingSpacing;
    public GameObject billboardPrefab;
    public const float maxViewDst = 100;
    public Transform viewer;
    public GameObject[] buildingBlocks;
    public GameObject[] buildingTops;
    public Material[] buildingMaterials;
    public GameObject[] windowBlocks;
    public GameObject chunkPrefab;
    static ChunkGenAndManage mapGenerator;
    public static Vector2 viewerPosition;
    public int chunkSize;
    int chunksVisibleInViewDst;
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mapGenerator = FindObjectOfType<ChunkGenAndManage>();
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        player.transform.position = new Vector3((mapGenerator.mapWidth / 4 * chunkSize) - chunkSize / 2, 5, (mapGenerator.mapHeight / 4 * chunkSize) - chunkSize / 2);
        StartCoroutine(clearBuildingBlocks());
    }

    IEnumerator clearBuildingBlocks()
    {
        while (true)
        {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("buildingBlock");
        yield return new WaitForEndOfFrame();  
        foreach (var item in blocks)
        {
            Destroy(item);
            yield return new WaitForEndOfFrame();   
        }

        }
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, buildingSpacing, transform, buildingMaterials, windowBlocks ,buildingBlocks, buildingTops, chunkPrefab, billboardPrefab));
                }

            }
        }
    }

    public class TerrainChunk
    {
        float[,] noiseMap;
        Vector2 position;
        Vector2 coordinates;
        Vector3 positionV3;
        Bounds bounds;
        GameObject Chunk;
        GameObject[] buildingBlocks;
        GameObject[] buildingTops;
        Material[] buildingMats;
        GameObject[] windowBlocks;

        GameObject chunkPrefab;
        GameObject billboardPrefab;

        float buildingSpacing;
        public TerrainChunk(Vector2 coord, int size, float buildingSpacing, Transform parent, Material[] buildingMats, GameObject[] windowBlocks,GameObject[] buildingBlocks, GameObject[] buildingTops, GameObject chunkPrefab, GameObject billboardPrefab)
        {

            noiseMap = Noise.GenerateNoiseMap(mapGenerator.mapWidth, mapGenerator.mapHeight,mapGenerator.seed, mapGenerator.noiseScale, mapGenerator.octaves, mapGenerator.persistance, mapGenerator.lacunarity, 10 * coord * Vector2.one);
            this.billboardPrefab = billboardPrefab;
            this.buildingSpacing = buildingSpacing;
            this.chunkPrefab = chunkPrefab;
            this.buildingBlocks = buildingBlocks;
            this.buildingTops = buildingTops;
            this.buildingMats = buildingMats;
            this.windowBlocks = windowBlocks;
            position = coord * size;
            coordinates = coord;
            bounds = new Bounds(position, Vector2.one * size);
            positionV3 = new Vector3(position.x, 0, position.y);
            //Chunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Chunk = Instantiate(chunkPrefab, positionV3, chunkPrefab.transform.rotation);
            Chunk.name = "Chunk " + position.x + ", " + position.y;
            //Chunk.transform.position = positionV3;
            //Chunk.transform.localScale = Vector3.one * 10;

            mapGenerator.RequestMapData(OnMapDataRecieved);

            Chunk.transform.parent = parent;
            
            try 
            {
                Chunk.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
            }

            catch
            {
                
            }
            //Chunk.GetComponent<MeshRenderer>().enabled = false;
            // NPCSpawnerScript[] spawners = Chunk.GetComponentsInChildren<NPCSpawnerScript>();

            // foreach (var item in spawners)
            // {
            //     item.enabled = true;
            // }
            SetVisible(false);
        }

        void OnMapDataRecieved(MapData map)
        {
            GenerateChunk();
            
        }

        public Vector3[] billboardLocation(int x, int z, Vector3 buildingPosition, int heightValue)
        {
            
            Vector3 billboardPosition = new Vector3();
            Vector3 billboardRotation = new Vector3();

            Vector3 RightSide = new Vector3(0, 90, 0);
            Vector3 LeftSide = new Vector3(0, -90, 0);
            Vector3 UpSide = new Vector3(0, 0, 0);
            Vector3 NearSide = new Vector3(0, -180, 0);

            int heightOfbillboard = Random.Range(2, heightValue);
           
            billboardPosition = new Vector3(buildingPosition.x + 5, heightOfbillboard * 10,buildingPosition.z + 5);
            
            if (x == 2)
            {
                billboardRotation = NearSide;
            }

            else if (x == 7)
            {
                billboardRotation = UpSide;
            }

            else if (z == 2)
            {
                billboardRotation = RightSide;
            }

            else if (z == 7)
            {
                billboardRotation = LeftSide;
            }

            Vector3[] PosAndRot = new Vector3[] {  billboardPosition, billboardRotation };

            return PosAndRot;

        }
        public void GenerateChunk()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {

                    Vector3 buildingPosition = new Vector3(positionV3.x - (buildingSpacing * 5) + (x * buildingSpacing), 0, positionV3.z - (buildingSpacing * 5) + (z * buildingSpacing));
                    if (x == 0 || z == 0 || (x % 9 == 0) || (z % 9 == 0) || x == 1 || z == 1 ||  ((x + 1) % 9 == 0) || ((z + 1) % 9 == 0))
                    {
                        // GameObject road;
                        // if (x == z)
                        // {
                        //     road = Instantiate(buildingBlocks[2], Vector3.zero, buildingBlocks[2].transform.rotation);
                        // }
                        // else
                        // {
                        //     road = GameObject.Instantiate(buildingBlocks[1], Vector3.zero, buildingBlocks[1].transform.rotation);
                        // }
                        // road.transform.position = new Vector3(buildingPosition.x, -.2f, buildingPosition.z + 10);
                        // road.transform.parent = Chunk.transform;

                    }
                    else
                    {

                        GameObject building = new GameObject("Building");
                        GameObject window = new GameObject("Window");
                        
                        building.transform.position = buildingPosition;
                        window.transform.position = buildingPosition;
                        
                        int heightData = (int)(10 * noiseMap[x, z]);
                        Color windowColor = mapGenerator.windowGradient.Evaluate(Random.Range(0f, 1f));

                        if (x == 2 || x == 7 || z == 2 || z == 7 && heightData > 3)
                        {
                            if (z % 3 == 0 || x % 3 == 0)
                            {
                                Vector3[] billboardData = billboardLocation(x, z, buildingPosition, heightData);
                                GameObject billboard = GameObject.Instantiate(billboardPrefab, billboardData[0], Quaternion.Euler(billboardData[1].x, billboardData[1].y, billboardData[1].z));
                                billboard.transform.parent = Chunk.transform;
                            }
                            //int chanceOfBillboard = Random.Range(0, 10);
                        }

                        for (int y = 0; y < heightData + 1; y++)
                        {
                            GameObject buildingBlock = Instantiate(buildingBlocks[0], Vector3.zero, buildingBlocks[0].transform.rotation);
                            GameObject windowBlock = Instantiate(windowBlocks[0], Vector3.zero, windowBlocks[0].transform.rotation);

                            buildingBlock.tag = "buildingBlock";
                            windowBlock.tag = "buildingBlock";
                            
                            buildingBlock.AddComponent<MeshCollider>().convex = true;

                            buildingBlock.transform.position = new Vector3(buildingPosition.x, (10 * y), buildingPosition.z);
                            windowBlock.transform.position = new Vector3(buildingPosition.x, (10 * y), buildingPosition.z);
                            
                            buildingBlock.transform.parent = building.transform;
                            windowBlock.transform.parent = window.transform;

                            
                            //windowBlock.GetComponent<MeshRenderer>().material.SetVector("_EmissionColor", 2 * windowColor);

                        }
                        
                        MeshFilter[] BuildingMeshFilters = building.GetComponentsInChildren<MeshFilter>();
                        MeshFilter[] WindowMeshFilters = window.GetComponentsInChildren<MeshFilter>();
                        
                        CombineInstance[] combineBuildings = new CombineInstance[BuildingMeshFilters.Length];
                        CombineInstance[] combineWindows = new CombineInstance[WindowMeshFilters.Length];
                        
                        int i = 0;

                        while (i < BuildingMeshFilters.Length)
                        {
                            combineBuildings[i].mesh = BuildingMeshFilters[i].sharedMesh;
                            combineWindows[i].mesh = WindowMeshFilters[i].sharedMesh;
                            
                            combineBuildings[i].transform = BuildingMeshFilters[i].transform.localToWorldMatrix;
                            combineWindows[i].transform = WindowMeshFilters[i].transform.localToWorldMatrix;
                            
                            
                            BuildingMeshFilters[i].gameObject.SetActive(false);
            
                           WindowMeshFilters[i].gameObject.SetActive(false);

                            i++;
                        }

                        building.AddComponent<MeshFilter>();
                        building.AddComponent<MeshRenderer>();
                        building.GetComponent<MeshFilter>().mesh = new Mesh();
                        building.GetComponent<MeshFilter>().mesh.CombineMeshes(combineBuildings);
                        building.GetComponent<Renderer>().material = buildingMats[0];

                        
                        window.AddComponent<MeshFilter>();
                        window.AddComponent<MeshRenderer>();
                        window.GetComponent<MeshFilter>().mesh = new Mesh();
                        window.GetComponent<MeshFilter>().mesh.CombineMeshes(combineWindows);
                        window.GetComponent<Renderer>().material = buildingMats[1];
                        window.GetComponent<Renderer>().material.SetVector("_EmissionColor", 2 * windowColor);;


                        building.transform.position = Vector3.zero;
                        building.transform.parent = Chunk.transform;
                        building.AddComponent<MeshCollider>();
                        
                        window.transform.position = Vector3.zero;
                         window.transform.parent = Chunk.transform;
                        window.AddComponent<MeshCollider>().convex = true;

                    }

                }
            }
        }
        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            Chunk.SetActive(visible);
        }



        public bool IsVisible()
        {
            return Chunk.activeSelf;
        }

    }


}