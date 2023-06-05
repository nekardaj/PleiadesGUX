using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DarknessSpawner : Spawner
{

    void Start()
    {
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        prefabWidth = prefabs[0].GetComponent<SpriteRenderer>().size.x;
        prefabHeight = prefabs[0].GetComponent<SpriteRenderer>().size.y;
        SetSortingLayers();
    }

    public override void Spawn()
    {
        GameObject newDarkness = Instantiate(prefabs[Random.Range(0, prefabs.Count)], new Vector3(spawnManager.indexOfLastSpawned * prefabWidth, prefabHeight * 0.85f, 0), Quaternion.identity);
        StartCoroutine(RemoveEnvironmentPlaceholder(newDarkness));
    }
}
