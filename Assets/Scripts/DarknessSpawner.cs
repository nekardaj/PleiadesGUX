using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DarknessSpawner : Spawner
{

    void Start()
    {
        indexOfLastSpawned = 0;
        prefabWidth = prefabs[0].GetComponent<SpriteRenderer>().size.x;
        prefabHeight = prefabs[0].GetComponent<SpriteRenderer>().size.y;
        SetSortingLayers();
    }

    public override void Spawn()
    {
        GameObject newDarkness = Instantiate(prefabs[Random.Range(0, prefabs.Count)], new Vector3(indexOfLastSpawned * prefabWidth, prefabHeight * 0.75f, 0), Quaternion.identity);
        StartCoroutine(RemoveEnvironmentPlaceholder(newDarkness));
        indexOfLastSpawned++;
    }
}
