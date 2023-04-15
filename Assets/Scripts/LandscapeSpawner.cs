using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandscapeSpawner : Spawner
{

    protected ushort lastHeight;

    void Start()
    {
        indexOfLastSpawned = 0;
        prefabWidth = prefabs[0].GetComponent<SpriteRenderer>().size.x;
        prefabHeight = prefabs[0].GetComponent<SpriteRenderer>().size.y;
        SetSortingLayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Spawn()
    {
        List<GameObject> suitablePrefabs = prefabs.Where(prefab => prefab.name.StartsWith(lastHeight.ToString())).ToList();
        GameObject prefabToSpawn = suitablePrefabs[Random.Range(0, suitablePrefabs.Count)];
        lastHeight = ushort.Parse(prefabToSpawn.name.Substring(prefabToSpawn.name.Length - 1));
        GameObject newLandscape = Instantiate(prefabToSpawn, new Vector3(indexOfLastSpawned * prefabWidth + 50, prefabHeight / 2, 0), Quaternion.identity);
        StartCoroutine(RemoveEnvironmentPlaceholder(newLandscape));
        indexOfLastSpawned++;
    }
}