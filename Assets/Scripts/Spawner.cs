using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> prefabs;

    [SerializeField]
    protected List<GameObject> obstacles;

    [SerializeField]
    protected GameObject playerReference;

    protected GameObject starPrefab;


    protected uint indexOfLastSpawned;

    public float prefabWidth;
    protected float prefabHeight;

    protected void SetSortingLayers()
    {
        foreach (GameObject prefab in prefabs)
        {
            prefab.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public abstract void Spawn(bool spawnStar, bool spawnObstacle);

    protected IEnumerator RemoveEnvironmentPlaceholder(GameObject prefab)
    {
        yield return new WaitForSeconds(10f);
        Destroy(prefab);
    }
}
