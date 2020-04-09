using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabVariants;
    public GameObject player;
    private List<GameObject> obstaclePrefabs;
    public Transform leftMarker;
    public Transform rightMarker;
    private float left;
    private float right;
    private float obstacleWidth;
    public Vector3 offsetFromPlayer;
    public float spawnRate;
    public int objectsPerSpawn;
    public float spawnAcceleration;
    private float currentDistance;
    private float distanceAtLastObstacle;
    private float distanceSinceLastObstacle;
    private bool canSpawnObstacle;
    private float lenghtOfPlayArea;
    private float spacing;
    public bool turnable;

    private void Start()
    {
        obstaclePrefabs = new List<GameObject>(0);
        currentDistance = player.transform.position.z;
        distanceAtLastObstacle = player.transform.position.z;
        left = leftMarker.position.x;
        right = rightMarker.position.x;
        BuildLevel();
        StartCoroutine(ObstacleTimer(0.01f)); //start spawning obstacles after 3 seconds;
        
        lenghtOfPlayArea = Mathf.Abs(right - left);
        //spacing = lenghtOfPlayArea / treeWidth;
        //StartCoroutine(LateFunction(0.1f));
    }

    IEnumerator LateFunction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CleanUp();
    }

    void SpawnObstacle()
    {
        int obID = Random.Range(0, obstaclePrefabVariants.Length);
        obstacleWidth = obstaclePrefabVariants[obID].GetComponent<Collider>().bounds.size.x;
        float randomPoint = PublicFunction.RoundUp(Random.Range(left, right), obstacleWidth); //obstacles will have even spacing between them equal to their width, meaning a 2u wide tree can only spawn on a multibale of 2.
        transform.position = new Vector3(randomPoint, transform.position.y, transform.position.z); // The spawner moves to that location
        
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = 1 << 8;
        RaycastHit rayHit;
        bool dontSpawn = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out rayHit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
            RaycastHit[] capsuleHitArray = Physics.CapsuleCastAll(transform.position, rayHit.point + transform.TransformDirection(Vector3.down) * 2, 2, transform.TransformDirection(Vector3.down), 10);

            foreach (RaycastHit hit in capsuleHitArray)
            {
                //Debug.Log(rayHit.collider.gameObject.name);

                //if (hit.collider.gameObject.layer == 8)
                //{
                //    Debug.Log("Hit Path at " + hit.collider.gameObject.name );
                //    dontSpawn = true;
                //}
                if (hit.collider.gameObject.tag == "Obstacle")
                {
                    //Debug.Log("Hit another Obstacle " + hit.collider.gameObject.name);
                    dontSpawn = true;
                }
            }         
        }
        else
        {
            dontSpawn = true;
        }

        if (!dontSpawn)
        {            
            GameObject ob = Instantiate(obstaclePrefabVariants[obID], transform.position + transform.TransformDirection(Vector3.down) * rayHit.distance, Quaternion.identity); //Instantiate an obstacle right on the surrface of that collider and add them to the list
            obstaclePrefabs.Add(ob);
            ob.name = obstaclePrefabVariants[obID].name.ToString() + " " + obstaclePrefabs.IndexOf(ob).ToString();
            if (turnable)
            {
                ob.transform.eulerAngles = new Vector3(ob.transform.eulerAngles.x, Random.Range(0.0f, 360.0f), ob.transform.eulerAngles.z);
            }
        }
    }
    
    void CleanUp()
    {
        foreach(GameObject t in obstaclePrefabs)
        if (!t.GetComponentInChildren<Obstacle>().touchingGround)
        {
            obstaclePrefabs.Remove(t);
            Destroy(t.gameObject);
        }
    }

    private void BuildLevel()
    {
        int prePlacedObsticals = (int)(offsetFromPlayer.z / spawnRate);
        for (int i = 0; i < prePlacedObsticals; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnRate * i);
            for(int t = 0; t <= objectsPerSpawn - 1; t++)
            {
                SpawnObstacle();
            }              
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetFromPlayer.y, player.transform.position.z + offsetFromPlayer.z);//move with the player

        currentDistance = player.transform.position.z; //track the player's distance down the hill
        distanceSinceLastObstacle = currentDistance - distanceAtLastObstacle; // track the diffrence between the distance the player had last time an obstacle was spawned and now
    }

    void DespawnObstacles()
    {
        foreach (GameObject ob in obstaclePrefabs.ToList()) // for every obstacle in the list
        {
            float distanceAway = (transform.position.z - ob.transform.position.z); //calculate the distance way
            if (distanceAway > offsetFromPlayer.z * 1.5) // if it's far behind the player
            {
                Destroy(ob); //DESTROY IT
                obstaclePrefabs.Remove(ob); //and remove the null refrence.
            }
        }
    }

    IEnumerator ObstacleTimer(float time)
    {
        yield return new WaitForSeconds(time);
        while (true)
        {
            for (int t = 0; t <= objectsPerSpawn-1; t++)
            {
                SpawnObstacle();
            }
            distanceAtLastObstacle = player.transform.position.z; //track where the player's distance now 
            yield return new WaitForSeconds(0.01f); // buffer to wait
            yield return new WaitUntil(()=>distanceSinceLastObstacle>=spawnRate); // wait untill the diffrence is greater than the spawn rate or when the player goes far enough 
            DespawnObstacles();
            //StartCoroutine(LateFunction(0.1f));
        }
    }
}

public class PublicFunction
{
    public static float RoundUp(float numToRound, float multiple)
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

    //public static Vector3 RoundUp(Vector3 numToRound, float multiple)
    //{
    //    if (multiple == 0)
    //        return numToRound;

    //    Vector3 remainder = new Vector3(Mathf.Abs(numToRound.x) % multiple, Mathf.Abs(numToRound.y) % multiple, Mathf.Abs(numToRound.z) % multiple);
    //    if (remainder == Vector3.zero)
    //        return numToRound;

    //    if (numToRound < Vector3.zero)
    //        return -(new Vector3(Mathf.Abs(numToRound.x) - remainder.x, Mathf.Abs(numToRound.y) - remainder.y, Mathf.Abs(numToRound.z) - remainder.z);
    //    else
    //        return numToRound + multiple - remainder;
    //}
}