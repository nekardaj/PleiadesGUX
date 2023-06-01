using System.Collections;
using System.Collections.Generic;
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
    public GameObject[] staticLevels;

    public int indexOfLastSpawned = -1; // Index of the last tile spawned
    private int starsSpawned = 0; // Number of stars spawned
    private int spawnInterval = 7; // Every *spawnInterval* tiles a star spawns
    private bool lastStarLayer = false; // True - last star was spawned in landscape, False - last star was spawned underwater
    public float obstacleSpawnChance = 0.1f;
    public float distanceToNextSpawn = 0f;

    void Start()
    {
        darknessSpawner = GetComponent<DarknessSpawner>();
        landscapeSpawner = GetComponent<LandscapeSpawner>();
        underwaterSpawner = GetComponent<UnderwaterSpawner>();
    }

    void Update()
    {
        if (player == null) return;
        distanceToNextSpawn = ((int)player.leadingAnimal.transform.position.x + darknessSpawner.prefabWidth) / darknessSpawner.prefabWidth;
        if (((int)player.leadingAnimal.transform.position.x + darknessSpawner.prefabWidth) / darknessSpawner.prefabWidth > indexOfLastSpawned)
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
        if (indexOfLastSpawned % 10 == 0)
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
        GameObject newLevel = Instantiate(staticLevels[Random.Range(0, staticLevels.Length)], new Vector3(indexOfLastSpawned * landscapeSpawner.prefabWidth - landscapeSpawner.prefabWidth / 2, 0, 0), Quaternion.identity);
        indexOfLastSpawned += 15; // +1 v Update funkci
    }
}
