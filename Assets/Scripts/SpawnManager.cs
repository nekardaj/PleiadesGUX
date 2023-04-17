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

    private int indexOfLastSpawned = 0; // Index of the last tile spawned
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
        if ((int)player.transform.position.x / darknessSpawner.prefabWidth > indexOfLastSpawned)
        {
            bool spawnStar = false;
            bool spawnObstacle = false;
            //print((indexOfLastSpawned / spawnInterval > starsSpawned) + " " + (indexOfLastSpawned / spawnInterval) + " " + starsSpawned);
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
        darknessSpawner.Spawn(false, false);
        if (spawnStar)
        {
            if (lastStarLayer)
            {
                print("Spawning star underwater");
                landscapeSpawner.Spawn(false, false);
                underwaterSpawner.Spawn(true, spawnObstacle);
            }
            else
            {
                print("Spawning star landscape");
                landscapeSpawner.Spawn(true, spawnObstacle);
                underwaterSpawner.Spawn(false, false);
            }
            lastStarLayer = !lastStarLayer;
        }
        else
        {
            landscapeSpawner.Spawn(false, spawnObstacle);
            underwaterSpawner.Spawn(false, spawnObstacle);
        }
        if (spawnObstacle) print("  with obstacle");
    }
}
