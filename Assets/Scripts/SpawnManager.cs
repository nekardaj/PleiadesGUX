using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private FlockMovement player;

    public GameObject starPrefab;

    private DarknessSpawner darknessSpawner;
    private LandscapeSpawner landscapeSpawner;
    private UnderwaterSpawner underwaterSpawner;
    public List<GameObject> staticLevels;
    public List<GameObject> endPrefabs;

    public int indexOfLastSpawned = 0; // Index of the last tile spawned
    private int starsSpawned = 0; // Number of stars spawned
    private int spawnInterval = 7; // Every *spawnInterval* tiles a star spawns
    private bool lastStarLayer = false; // True - last star was spawned in landscape, False - last star was spawned underwater
    public float obstacleSpawnChance = 0.1f;

    void Start()
    {
        darknessSpawner = GetComponent<DarknessSpawner>();
        landscapeSpawner = GetComponent<LandscapeSpawner>();
        underwaterSpawner = GetComponent<UnderwaterSpawner>();
    }

    void Update()
    {
        if (player == null) return;
        if (((int)player.leadingAnimal.transform.position.x + darknessSpawner.prefabWidth * 2) / darknessSpawner.prefabWidth > indexOfLastSpawned)
        {
            bool spawnStar = false;
            bool spawnObstacle = false;
            if (indexOfLastSpawned / spawnInterval > starsSpawned)
            {
                spawnStar = true;
                starsSpawned++;
            }
            if (Random.Range(0f, 1f) <= obstacleSpawnChance)
            {
                spawnObstacle = true;
            }
            SpawnEnvironment(spawnStar, spawnObstacle);
            indexOfLastSpawned++;
        }
    }

    void SpawnEnvironment(bool spawnStar, bool spawnObstacle)
    {
        if (indexOfLastSpawned % 10 == 9) //0
        {
            SpawnStaticLevel();
            return;
        }
        darknessSpawner.Spawn();
        if (spawnStar)
        {
            if (lastStarLayer)
            {
                landscapeSpawner.Spawn(false, false);
                if (landscapeSpawner.lastHadZero)
                {
                    underwaterSpawner.Spawn(true, spawnObstacle, true);
                }
                else
                {
                    underwaterSpawner.Spawn(true, spawnObstacle, false);
                }
            }
            else
            {
                landscapeSpawner.Spawn(true, spawnObstacle);
                if (landscapeSpawner.lastHadZero)
                {
                    underwaterSpawner.Spawn(false, false, true);
                }
                else
                {
                    underwaterSpawner.Spawn(false, false, false);
                }
            }
            lastStarLayer = !lastStarLayer;
        }
        else
        {
            landscapeSpawner.Spawn(false, spawnObstacle);
            if (landscapeSpawner.lastHadZero)
            {
                underwaterSpawner.Spawn(false, spawnObstacle, true);
            }
            else
            {
                underwaterSpawner.Spawn(false, spawnObstacle, false);
            }
        }
    }

    private void SpawnStaticLevel()
    {
        // Az budou staticky levely hotovy. Do te doby nechat zakomentovany
        string toStartWith = landscapeSpawner.lastHeight.ToString() + underwaterSpawner.lastHeight.ToString();
        List<GameObject> possibleLevels = staticLevels.Where(level => level.name.StartsWith(toStartWith)).ToList();
        GameObject newLevel = possibleLevels.ElementAt(Random.Range(0, possibleLevels.Count()));
        //GameObject newLevel = staticLevels[Random.Range(0, staticLevels.Count)];
        Instantiate(newLevel, new Vector3(indexOfLastSpawned * landscapeSpawner.prefabWidth - landscapeSpawner.prefabWidth / 2, 0, 0), Quaternion.identity);
        landscapeSpawner.lastHeight = ushort.Parse(newLevel.name[2].ToString());
        underwaterSpawner.lastHeight = ushort.Parse(newLevel.name[3].ToString());
        indexOfLastSpawned += 15; // +1 v Update funkci
    }

    public void SpawnEnd()
    {
        string toStartWith = landscapeSpawner.lastHeight.ToString() + underwaterSpawner.lastHeight.ToString();
        List<GameObject> possibleLevels = endPrefabs.Where(level => level.name.StartsWith(toStartWith)).ToList();
        Instantiate(possibleLevels[0], new Vector3(indexOfLastSpawned * landscapeSpawner.prefabWidth, landscapeSpawner.prefabHeight / 2, 0), Quaternion.identity);
        indexOfLastSpawned += 10;
    }
}
