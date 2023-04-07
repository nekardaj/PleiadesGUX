using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private FlockMovement player;

    private DarknessSpawner darknessSpawner;
    private LandscapeSpawner landscapeSpawner;
    private UnderwaterSpawner underwaterSpawner;

    private int indexOfLastSpawned = 0;

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
            SpawnEverything();
            indexOfLastSpawned++;
        }
    }

    void SpawnEverything()
    {
        darknessSpawner.Spawn();
        landscapeSpawner.Spawn();
        underwaterSpawner.Spawn();
    }
}
