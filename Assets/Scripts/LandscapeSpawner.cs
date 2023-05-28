using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandscapeSpawner : Spawner
{

    public ushort lastHeight;
    public bool lastHadZero;
    public List<GameObject> backgrounds;

    void Start()
    {
        indexOfLastSpawned = 0;
        prefabWidth = prefabs[0].GetComponent<SpriteRenderer>().size.x * 0.95f;
        prefabHeight = prefabs[0].GetComponent<SpriteRenderer>().size.y;
        SetSortingLayers();
        starPrefab = GetComponent<SpawnManager>().starPrefab;
        lastHeight = 0;
    }

    public override void Spawn(bool spawnStar, bool spawnObstacle)
    {
        List<GameObject> suitablePrefabs = prefabs.Where(prefab => prefab.name.StartsWith(lastHeight.ToString())).ToList();
        GameObject prefabToSpawn = suitablePrefabs[Random.Range(0, suitablePrefabs.Count)];
        lastHeight = ushort.Parse(prefabToSpawn.name.Substring(prefabToSpawn.name.Length - 1));
        if (prefabToSpawn.name[1] == '0')
        {
            lastHadZero = true;
        }
        else
        {
            lastHadZero = false;
        }
        GameObject newLandscape = Instantiate(prefabToSpawn, new Vector3(indexOfLastSpawned * prefabWidth, prefabHeight / 2, 0), Quaternion.identity);
        Instantiate(backgrounds[Random.Range(0, backgrounds.Count)], newLandscape.transform);
        if (spawnStar)
        {
            Instantiate(starPrefab, newLandscape.transform.GetChild(Random.Range(0, newLandscape.transform.childCount - 1)));
        }
        if (spawnObstacle)
        {
            Instantiate(obstacles[Random.Range(0, obstacles.Count)], newLandscape.transform.GetChild(Random.Range(0, newLandscape.transform.childCount - 1)));
        }

        StartCoroutine(RemoveEnvironmentPlaceholder(newLandscape));
        indexOfLastSpawned++;
    }
}
