using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlockManager : MonoBehaviour
{
    public GameObject animalPrefab;
    public GameObject leaderBird;
    public List<GameObject> flock = new List<GameObject>();

    private FlockMovement movement;
    private bool isInEnvironment;

    private void Start()
    {
        movement = GetComponent< FlockMovement>();
        leaderBird = Instantiate(animalPrefab, transform);
        leaderBird.GetComponent<AnimalManager>().flock = this;
        movement.leadingBird = leaderBird.transform;
        flock.Add(leaderBird);
        isInEnvironment = false;
    }

    private void Update()
    {
        for (int i = 1; i < flock.Count; i++)
        {
            flock[i].transform.rotation = leaderBird.transform.rotation;
        }
    }

    public void SolveCollisionEnter(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            AddAnimalToTheFlock();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            if (movement.movementSpeed >= 30)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Collision(collision);
            }
        }
        else if (collision.CompareTag("Environment"))
        {
            if (isInEnvironment) return;

            isInEnvironment = true;
            Collision(collision);
            foreach (GameObject animal in flock)
            {
                animal.GetComponent<SpriteRenderer>().DOColor(new Color(0.5f, 0.5f, 0.5f), 0.25f);
            }
        }
    }

    public void SolveCollisionExit(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            isInEnvironment = false;
            foreach (GameObject animal in flock)
            {
                animal.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.5f);
            }
        }
    }

    private void Collision(Collider2D collision)
    {
        int flockCount = flock.Count;
        if (flockCount > 1)
        {
            GameObject lastBird = flock[flockCount - 1];
            flock.RemoveAt(flockCount - 1);

            Destroy(lastBird);
            if (collision.CompareTag("Obstacle")) Destroy(collision.gameObject);
        }
        else
        {
            print("Game Over");
            Destroy(this.gameObject);
        }
    }

    private const float GridCellSize = 2f;
    private Dictionary<Vector2Int, List<Vector3>> flockPositions = new Dictionary<Vector2Int, List<Vector3>>();

    private void AddAnimalToTheFlock()
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

        GameObject newBird = Instantiate(animalPrefab, randomPosition, Quaternion.identity, transform);
        newBird.GetComponent<AnimalManager>().flock = this;
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

