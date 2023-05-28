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


    public int indexOfLastSpawned;

    public float prefabWidth;
    protected float prefabHeight;

    protected void SetSortingLayers()
    {
        foreach (GameObject prefab in prefabs)
        {
            prefab.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public virtual void Spawn(bool spawnStar, bool spawnObstacle, bool forceZero)
    { }
    public virtual void Spawn(bool spawnStar, bool spawnObstacle)
    { }
    public virtual void Spawn(bool spawnStar)
    { }
    public virtual void Spawn()
    { }    

    protected IEnumerator RemoveEnvironmentPlaceholder(GameObject prefab)
    {
        yield return new WaitForSeconds(30f);
        Destroy(prefab);
    }
}
