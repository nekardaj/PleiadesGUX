using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlockManager : MonoBehaviour
{
    public GameObject animalPrefab;
    public GameObject leadingAnimal;
    public List<GameObject> flock = new List<GameObject>();
    public List<GameObject> spawnPositions= new List<GameObject>();
    public List<GameObject> alignPositions = new List<GameObject>();

    private FlockMovement movement;
    private bool isInEnvironment;
    public bool isInWater = false;
    public bool colliding = true;

    public Vector3 flockCentre;

    //private bool aligningFlock = false;

    //private Coroutine alignCoroutine;
    //private Coroutine DisperseFlockCoroutine;

    private void Start()
    {
        movement = GetComponent<FlockMovement>();
        //leadingAnimal = Instantiate(animalPrefab, transform);
        //leadingAnimal.GetComponent<AnimalManager>().manager = this;
        //leadingAnimal.GetComponent<AnimalManager>().movement = GetComponent<FlockMovement>();
        leadingAnimal = transform.GetChild(0).gameObject;
        movement.leadingAnimal = leadingAnimal.transform;
        flock.Add(leadingAnimal);
        isInEnvironment = false;
    }

    private void Update()
    {
        /*
        for (int i = 1; i < flock.Count; i++)
        {
            flock[i].transform.rotation = leadingAnimal.transform.rotation;
        }
        */

        //flockCentre = Vector3.zero;
        //foreach (GameObject animal in flock)
        //{
        //    flockCentre += animal.transform.position / flock.Count;
        //}

        if (Input.GetKeyDown(KeyCode.D))
        {
            AddAnimalToTheFlock();
        }

        /*
        if (Input.GetKey(KeyCode.X))
        {
            if (!aligningFlock)
            {
                StartCoroutine(AlignFlock());
                aligningFlock = true;
            }
            // Stop the DisperseFlock coroutine if it is still running
            if (DisperseFlockCoroutine != null)
            {
                StopCoroutine(DisperseFlockCoroutine);
            }
        }
        else
        {
            if (aligningFlock)
            {
                DisperseFlockCoroutine = StartCoroutine(DisperseFlock());
                aligningFlock = false;
            }
        }
        */
    }

    public void SolveCollisionEnter(Collider2D collision)
    {
        if (!colliding) return;
        if (collision.CompareTag("Star"))
        {
            AddAnimalToTheFlock();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            if (movement.movementSpeed >= 25)
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
        GameObject newBird = Instantiate(animalPrefab, spawnPositions[flock.Count].transform.position, Quaternion.identity, transform);
        newBird.GetComponent<AnimalManager>().manager = this;
        newBird.GetComponent<AnimalManager>().movement = GetComponent<FlockMovement>();
        flock.Add(newBird);

        /*
        float radius = 3f;
        float minDistance = 3f;
        float distanceBehindLeader = 4f; // distance behind the leader bird to generate new birds
        Vector3 offset = -leadingAnimal.transform.forward * distanceBehindLeader;
        Vector3 randomPosition = leadingAnimal.transform.position + Random.insideUnitSphere * radius + offset;

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
                    randomPosition = leadingAnimal.transform.position + Random.insideUnitSphere * radius + offset;
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
        */
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

    private IEnumerator AlignFlock()
    {
        float minDistance = 2f;
        float distanceBehindLeader = 2f; // distance behind the leader bird to generate new birds
        float offsetDistance = 2f; // distance to offset each bird's target position
        Vector3 offset = -leadingAnimal.transform.forward * distanceBehindLeader;

        // Store the original offset for each bird from the leader bird
        List<Vector3> offsets = new List<Vector3>();
        for (int i = 0; i < flock.Count - 1; i++)
        {
            Vector3 offsetFromLeader = flock[i + 1].transform.position - leadingAnimal.transform.position;
            offsets.Add(offsetFromLeader);
        }

        Vector3 targetOffset = offsetDistance * leadingAnimal.transform.right;

        float duration = 3f; // duration of alignment movement

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            for (int i = 1; i < flock.Count; i++)
            {
                Vector3 targetPosition = leadingAnimal.transform.position + leadingAnimal.transform.forward * (i * minDistance) + offset + offsets[i - 1] + targetOffset * (i - 1);
                targetPosition.y = leadingAnimal.transform.position.y; // set y-axis position to that of the leader bird
                Vector3 currentPosition = flock[i].transform.position;
                Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, t / duration);
                flock[i].transform.position = newPosition;
                flock[i].transform.rotation = leadingAnimal.transform.rotation;
            }

            yield return null; // wait for the next frame
        }
    }


    private IEnumerator DisperseFlock()
    {
        float minDistance = 2f;
        float distanceBehindLeader = 2f; // distance behind the leader bird to generate new birds
        float offsetDistance = 2f; // distance to offset each bird's target position
        Vector3 offset = -leadingAnimal.transform.forward * distanceBehindLeader;

        float duration = 3f; // duration of alignment movement
        float t = 0f;

        while (t < duration)
        {
            for (int i = 1; i < flock.Count; i++)
            {
                Vector3 randomPosition = leadingAnimal.transform.position + Random.insideUnitSphere * minDistance;
                randomPosition.y += Random.Range(-0.5f, 0.5f); // randomize the y-axis position by adding a random value
                Vector3 targetOffset = (i - 1) * offsetDistance * leadingAnimal.transform.right;
                Vector3 targetPosition = randomPosition + offset + targetOffset;
                Vector3 currentPosition = flock[i].transform.position;
                Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, t / duration);
                flock[i].transform.position = newPosition;
                flock[i].transform.rotation = Quaternion.Lerp(flock[i].transform.rotation, leadingAnimal.transform.rotation, t / duration);
            }

            t += Time.deltaTime;
            yield return null; // wait for the next frame
        }
    }


}

