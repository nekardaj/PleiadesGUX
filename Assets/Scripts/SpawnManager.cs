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

    void Start()
    {
        darknessSpawner = GetComponent<DarknessSpawner>();
        landscapeSpawner = GetComponent<LandscapeSpawner>();
        underwaterSpawner = GetComponent<UnderwaterSpawner>();
    }

    void Update()
    {
        if ((int)player.transform.position.x / darknessSpawner.prefabWidth > indexOfLastSpawned)
        {
            bool spawnStar = false;
            //print((indexOfLastSpawned / spawnInterval > starsSpawned) + " " + (indexOfLastSpawned / spawnInterval) + " " + starsSpawned);
            if (indexOfLastSpawned / spawnInterval > starsSpawned)
            {
                spawnStar = true;
                starsSpawned++;
            }
            SpawnEnvironment(spawnStar);
            indexOfLastSpawned++;
        }
    }

    void SpawnEnvironment(bool spawnStar)
    {
        darknessSpawner.Spawn(false);
        if (spawnStar)
        {
            if (lastStarLayer)
            {
                print("Spawning star underwater");
                landscapeSpawner.Spawn(false);
                underwaterSpawner.Spawn(true);
            }
            else
            {
                print("Spawning star landscape");
                landscapeSpawner.Spawn(true);
                underwaterSpawner.Spawn(false);
            }
        }
        else
        {
            landscapeSpawner.Spawn(false);
            underwaterSpawner.Spawn(false);
        }
    }
}
