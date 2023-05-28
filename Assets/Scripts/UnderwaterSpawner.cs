using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UnderwaterSpawner : Spawner
{

    protected ushort lastHeight;
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

    public override void Spawn(bool spawnStar, bool spawnObstacle, bool forceZero)
    {
        List<GameObject> suitablePrefabs = prefabs.Where(prefab => prefab.name.StartsWith(lastHeight.ToString()) && ((prefab.name.Length == 2 && !prefab.name.EndsWith('0')) || (prefab.name.Length == 3 && prefab.name[1] == 'O'))).ToList();
        if (forceZero)
        {
            suitablePrefabs = prefabs.Where(prefab => prefab.name.StartsWith(lastHeight.ToString()) && prefab.name.Length == 3 && prefab.name[1] == '0').ToList();
        }
        GameObject prefabToSpawn = suitablePrefabs[Random.Range(0, suitablePrefabs.Count)];
        lastHeight = ushort.Parse(prefabToSpawn.name.Substring(prefabToSpawn.name.Length - 1));
        GameObject newUnderwater = Instantiate(prefabToSpawn, new Vector3(indexOfLastSpawned * prefabWidth, -prefabHeight / 2, 0), Quaternion.identity);
        Instantiate(backgrounds[Random.Range(0, backgrounds.Count)], newUnderwater.transform);
        if (spawnStar)
        {
            Instantiate(starPrefab, newUnderwater.transform.GetChild(Random.Range(0, newUnderwater.transform.childCount - 1)));
        }
        if (spawnObstacle)
        {
            Instantiate(obstacles[Random.Range(0, obstacles.Count)], newUnderwater.transform.GetChild(Random.Range(0, newUnderwater.transform.childCount - 1)));
        }

        StartCoroutine(RemoveEnvironmentPlaceholder(newUnderwater));
        indexOfLastSpawned++;
    }
}
