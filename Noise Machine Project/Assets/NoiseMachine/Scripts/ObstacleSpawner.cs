using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject player;
    private List<GameObject> treePrefabs;
    public Transform leftMarker;
    public Transform rightMarker;
    private float left;
    private float right;
    private float treeWidth;
    public Vector3 offsetFromPlayer;
    public float spawnRate;
    public float spawnAcceleration;
    private float currentDistance;
    private float distanceAtLastObstacle;
    private float distanceSinceLastObstacle;
    private bool canSpawnObstacle;
    private float lenghtOfPlayArea;
    private float spacing;

    private void Start()
    {
        treePrefabs = new List<GameObject>(0);
        currentDistance = player.transform.position.z;
        distanceAtLastObstacle = player.transform.position.z;
        left = leftMarker.position.x;
        right = rightMarker.position.x;
        BuildLevel();
        StartCoroutine(ObstacleTimer(0.01f)); //start spawning obstacles after 3 seconds;
        treeWidth = treePrefab.GetComponent<Collider>().bounds.size.x;
        lenghtOfPlayArea = Mathf.Abs(right - left);
        spacing = lenghtOfPlayArea / treeWidth;
    }

    void SpawnObstacle()
    {
        float randomPoint = roundUp(Random.Range(left, right), treeWidth); //obstacles will have even spacing between them equal to their width, meaning a 2u wide tree can only spawn on a multibale of 2.
        transform.position = new Vector3(randomPoint, transform.position.y, transform.position.z); // The spawner moves to that location

        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
           treePrefabs.Add(Instantiate(treePrefab, transform.position+transform.TransformDirection(Vector3.down)*hit.distance, Quaternion.identity)); //Instantiate an obstacle right on the surrface of that collider and add them to the list
        }
        
    }
    float roundUp(float numToRound, float multiple)
    {
        if (multiple == 0)
            return numToRound;

        float remainder = Mathf.Abs(numToRound) % multiple;
        if (remainder == 0)
            return numToRound;

        if (numToRound < 0)
            return -(Mathf.Abs(numToRound) - remainder);
        else
            return numToRound + multiple - remainder;
    }

    private void BuildLevel()
    {
        int prePlacedObsticals = (int)(offsetFromPlayer.z / spawnRate);
        for (int i = 0; i < prePlacedObsticals; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnRate * i);
            SpawnObstacle();                
        }
    }

    private void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    SpawnObstacle();
        //}
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetFromPlayer.y, player.transform.position.z + offsetFromPlayer.z);//move with the player

        currentDistance = player.transform.position.z; //track the player's distance down the hill
        distanceSinceLastObstacle = currentDistance - distanceAtLastObstacle; // track the diffrence between the distance the player had last time an obstacle was spawned and now
    }

    void DespawnObstacles()
    {
        for (int t = 0; t < treePrefabs.Count; t++) // for every tree in the list
        {
            float distanceAway = (transform.position.z - treePrefabs[t].transform.position.z); //calculate the distance way
            if (distanceAway > offsetFromPlayer.z * 1.5) // if it's far behind the player
            {
                Destroy(treePrefabs[t]); //DESTROY IT
                treePrefabs.Remove(treePrefabs[t]); //and remove the null refrence.
            }
        }
    }

    IEnumerator ObstacleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        while (true)
        {
            SpawnObstacle(); 
            distanceAtLastObstacle = player.transform.position.z; //track where the player's distance now 
            yield return new WaitForSeconds(0.01f); // buffer to wait
            yield return new WaitUntil(()=>distanceSinceLastObstacle>=spawnRate); // wait untill the diffrence is greater than the spawn rate or when the player goes far enough 
            DespawnObstacles();
        }
    }
}
