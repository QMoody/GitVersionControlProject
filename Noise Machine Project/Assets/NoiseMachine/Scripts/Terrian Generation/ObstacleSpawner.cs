using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ObjectPooler.Application;

public class ObstacleSpawner : MonoBehaviour
{    
    public enum ObstacleType { Tree, Rock, Flag};
    private List<ObjectPooler.Domain.ObjectPoolItem> obstacleVariants;
    public ObstacleType obstacleType;
    public GameObject player;
    private List<GameObject> currentObstacles;
    public Transform leftMarker;
    public Transform rightMarker;
    private float left;
    private float right;
    private float obstacleWidth;
    public Vector3 offsetFromPlayer;
    private Vector3 startPosition;    
    public float spawnRate;
    public int objectsPerSpawn;
    //for ever 100 spawns, the spawn rate will increase by this much
    public float spawnAcceleration;
    private float currentDistance;
    private float distanceAtLastObstacle;
    private float distanceSinceLastObstacle;
    private bool canSpawnObstacle;
    private float lenghtOfPlayArea;
    private float spacing;
    public bool turnable;
    ObjectPoolerManager OP;

    private void Start()
    {
        OP = ObjectPoolerManager.SharedInstance;
        startPosition = player.transform.position;        
        currentDistance = player.transform.position.z;
        distanceAtLastObstacle = player.transform.position.z;
        objectsPerSpawnIncreaseRate = objectsPerSpawn;
        ChangePlayArea();
        currentObstacles = new List<GameObject>(0);

        switch (obstacleType)
        {
            case ObstacleType.Tree:
                obstacleVariants = OP.trees;
                obstacleWidth = 6.4f;
                break;
            case ObstacleType.Rock:
                obstacleVariants = OP.rocks;
                obstacleWidth = 3.2f;
                break;
            case ObstacleType.Flag:
                obstacleVariants = OP.flags;
                obstacleWidth = 3.2f;
                break;
        }

        obCount = obstacleVariants.Count;
        StartCoroutine(ObstacleTimer(0.01f)); //start spawning obstacles after 1 milli-second;
        
        distanceAheadOfPlayer = PublicFunction.DistanceInYnZ(startPosition, transform.position);
        //spacing = lenghtOfPlayArea / treeWidth;
        //StartCoroutine(LateFunction(0.1f));


    }

    void ChangePlayArea()
    {
        left = leftMarker.position.x;
        right = rightMarker.position.x;
        lenghtOfPlayArea = Mathf.Abs(right - left);
    }

    int obID;
    float randomPoint;
    int obCount;
    private float distanceAheadOfPlayer;

    void SpawnObstacle()
    {
        obID = Random.Range(0, obCount);
        randomPoint = PublicFunction.RoundUp(Random.Range(left, right), obstacleWidth); //obstacles will have even spacing between them equal to their width, meaning a 2u wide tree can only spawn on a multibale of 2.
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
        GameObject ob;
        if (!dontSpawn)
        {
            ob = OP.SpawnFromPool(obstacleType.ToString() + obID.ToString(), transform.position + transform.TransformDirection(Vector3.down) * rayHit.distance, Quaternion.identity); //Instantiate an obstacle right on the surrface of that collider and add them to the list
            currentObstacles.Add(ob);            
            ob.name = obstacleType.ToString() + obID.ToString();
            if (turnable)
            {
                ob.transform.eulerAngles = new Vector3(ob.transform.eulerAngles.x, Random.Range(0.0f, 360.0f), ob.transform.eulerAngles.z);
            }
        }
    }

    private void BuildLevel()
    {
        int prePlacedObsticals = (int)(offsetFromPlayer.z / spawnRate);
        for (int i = 0; i < prePlacedObsticals; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnRate * i);
            distanceAheadOfPlayer = Mathf.Sqrt(Mathf.Pow(startPosition.y - transform.position.y, 2) + Mathf.Pow(startPosition.z - transform.position.z, 2));
            for (int t = 0; t <= objectsPerSpawn - 1; t++)
            {
                SpawnObstacle();
            }              
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetFromPlayer.y, player.transform.position.z + offsetFromPlayer.z);//move with the player

        distanceSinceLastObstacle = Traker.inst.totalDis - distanceAtLastObstacle; // track the diffrence between the distance the player had last time an obstacle was spawned and now
    }



    void DespawnObstacles()
    {
        foreach (GameObject ob in currentObstacles.ToList()) // for every obstacle in the list
        {
            float distanceAway = (Traker.inst.totalDis - PublicFunction.DistanceInYnZ(startPosition,ob.transform.position)); //calculate the distance way
            if (distanceAway > distanceAheadOfPlayer/10) // if it's far behind the player
            {
                currentObstacles.Remove(ob);
                OP.ReleaseBackToPool(ob.name, ob);    
            }
            else
            {
                break;
            }
        }
    }
    bool keepSpawning;
    private float objectsPerSpawnIncreaseRate;

    IEnumerator ObstacleTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        BuildLevel();
        keepSpawning = true;
        while (keepSpawning)
        {
            if (!Traker.inst.endless)
            {
                if(Traker.inst.totalDis >= Traker.inst.goalDis)
                {
                    keepSpawning = false;
                }
            }

            for (int t = 0; t <= objectsPerSpawn-1; t++)
            {
                SpawnObstacle();
            }
            distanceAtLastObstacle = Traker.inst.totalDis; //track where the player's distance now 
            yield return new WaitForFixedUpdate(); // buffer to wait
            if (spawnAcceleration != 0 && spawnRate>=0)
            {
                spawnRate -= spawnAcceleration/100;
                objectsPerSpawnIncreaseRate += spawnAcceleration / 1000;
                objectsPerSpawn = Mathf.RoundToInt(objectsPerSpawnIncreaseRate);
            }
            yield return new WaitUntil(()=>distanceSinceLastObstacle>=spawnRate); // wait untill the diffrence is greater than the spawn rate or when the player goes far enough 
            DespawnObstacles();
            ChangePlayArea();
        }
        yield break;
    }
    //void CleanUp()
    //{
    //    foreach(GameObject t in obstaclePrefabs)
    //    if (!t.GetComponentInChildren<Obstacle>().touchingGround)
    //    {
    //        obstaclePrefabs.Remove(t);
    //        Destroy(t.gameObject);
    //    }
    //}

    //IEnumerator LateFunction(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    CleanUp();
    //}

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

    public static float DistanceInYnZ(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
    }
    
}