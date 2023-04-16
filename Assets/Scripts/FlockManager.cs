using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject leaderBird;
    public List<GameObject> flock = new List<GameObject>();

    private void Start()
    {
        leaderBird = this.gameObject;
        flock.Add(leaderBird);
    }

    private void Update()
    {
        for (int i = 0; i < flock.Count; i++)
        {
            flock[i].transform.rotation = this.transform.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "powerup")
        {
            AddPlayerToTheFlock();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Hurdle")
        {
            int flockCount = flock.Count;
            if (flockCount > 1)
            {
                GameObject lastBird = flock[flockCount - 1];
                flock.RemoveAt(flockCount - 1);

                Destroy(lastBird);
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private const float GridCellSize = 2f;
    private Dictionary<Vector2Int, List<Vector3>> flockPositions = new Dictionary<Vector2Int, List<Vector3>>();

    private void AddPlayerToTheFlock()
    {
        float radius = 3f;
        float minDistance = 3f;
        float distanceBehindLeader = 4f; // distance behind the leader bird to generate new birds
        Vector3 offset = -leaderBird.transform.forward * distanceBehindLeader;
        Vector3 randomPosition = leaderBird.transform.position + Random.insideUnitSphere * radius + offset;

        bool positionIsSuitable = false;
        while (!positionIsSuitable)
        {
            positionIsSuitable = true;

            Vector2Int gridPosition = GetGridPosition(randomPosition);
            List<Vector3> nearbyPositions = GetNearbyPositions(gridPosition);

            foreach (Vector3 position in nearbyPositions)
            {
                if (Vector3.Distance(randomPosition, position) < minDistance)
                {
                    positionIsSuitable = false;
                    randomPosition = leaderBird.transform.position + Random.insideUnitSphere * radius + offset;
                    break;
                }
            }
        }

        GameObject newBird = Instantiate(playerPrefab, randomPosition, Quaternion.identity);
        newBird.transform.SetParent(this.transform);
        flock.Add(newBird);

        Vector2Int newGridPosition = GetGridPosition(randomPosition);
        if (!flockPositions.ContainsKey(newGridPosition))
        {
            flockPositions[newGridPosition] = new List<Vector3>();
        }
        flockPositions[newGridPosition].Add(randomPosition);
    }

    private Vector2Int GetGridPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / GridCellSize);
        int z = Mathf.FloorToInt(position.z / GridCellSize);
        return new Vector2Int(x, z);
    }

    private List<Vector3> GetNearbyPositions(Vector2Int gridPosition)
    {
        List<Vector3> nearbyPositions = new List<Vector3>();

        for (int x = gridPosition.x - 1; x <= gridPosition.x + 1; x++)
        {
            for (int z = gridPosition.y - 1; z <= gridPosition.y + 1; z++)
            {
                Vector2Int nearbyGridPosition = new Vector2Int(x, z);
                if (flockPositions.TryGetValue(nearbyGridPosition, out List<Vector3> positions))
                {
                    nearbyPositions.AddRange(positions);
                }
            }
        }

        return nearbyPositions;
    }

}

