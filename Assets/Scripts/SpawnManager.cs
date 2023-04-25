using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private FlockMovement player;

    public GameObject starPrefab;

    private DarknessSpawner darknessSpawner;
    private LandscapeSpawner landscapeSpawner;
    private UnderwaterSpawner underwaterSpawner;

    public int indexOfLastSpawned = 0; // Index of the last tile spawned
    private int starsSpawned = 0; // Number of stars spawned
    private int spawnInterval = 1; // Every *spawnInterval* tiles a star spawns
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
        if ((int)player.transform.position.x / darknessSpawner.prefabWidth > indexOfLastSpawned)
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
}
