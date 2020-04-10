using System;
using System.Collections.Generic;
using ObjectPooler.Domain;
using UnityEngine;

namespace ObjectPooler.Application
{
    /**
     * In this place we configure Object Pool Items (see more at ObjectPooler.Domain.ObjectPoolItem)
     * For convenience: items are splitted into groups. Later on items from groups are combined into one list (_poolDictionary))
     */
    public class ObjectPoolerManager : MonoBehaviour
    {
        public static ObjectPoolerManager SharedInstance = null;

        //this is divided by groups for more human-friendly management in the Inspector
        public List<ObjectPoolItem> trees;
        public List<ObjectPoolItem> rocks;
        public List<ObjectPoolItem> flags;

        //this is needed to run loop over grouped items
        private List<List<ObjectPoolItem>> _allObjects = new List<List<ObjectPoolItem>>();
        
        //here we store all GameObject for Pool Item with specific name.
        //For example: in this place all GameObjects for "building_Large" will stored.
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        
        //list of all unique pools
        private Dictionary<string, ObjectPoolItem> _objectsPoolItems = new Dictionary<string, ObjectPoolItem>();
        
        //fast check if Pool is resizable, for example:
        //"building_Large" - true
        //"arrows" - false
        //this prevents for looping all the pools everytime
        private Dictionary<string, bool> _autoResize = new Dictionary<string, bool>();

        public void Awake()
        {
            if (SharedInstance == null)
            {
                SharedInstance = this;
            }
            else if (SharedInstance != this)
            {
                Debug.LogWarning("Multible ObjectPoolerManager. One was destroy");
                Destroy(this);
            }
        }

        public void Start()
        {
            _allObjects.Add(trees);
            _allObjects.Add(rocks);
            _allObjects.Add(flags);

            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            //create that many GameObjects as there is in ObjectPooler.Domain.ObjectPoolItem.size
            foreach (List<ObjectPoolItem> objectPoolItems in _allObjects)
            {
                foreach (ObjectPoolItem poolItem in objectPoolItems)
                {
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int i = 0; i < poolItem.size; i++)
                    {
                        SpawnGO(poolItem.prefab, poolItem.parent, objectPool);
                    }

                    _poolDictionary.Add(poolItem.poolItemName, objectPool);
                    
                    _objectsPoolItems.Add(poolItem.poolItemName, poolItem);
                    _autoResize.Add(poolItem.poolItemName, poolItem.autoResize);
                }
            }
        }

        //public ObjectPoolItem GetObjectPoolItem(List<ObjectPoolItem> obstaclePool,int ID)
        //{

        //    if (poolWithGameObjects == null)
        //    {
        //        Debug.LogWarningFormat("Pool with name: {0} doesn't exist", poolItemName);
        //        return null;
        //    }
        //    try
        //    {
        //        //get free GameObject from Pool queue of GameObjects
        //        GameObject obj = poolWithGameObjects.Dequeue();

        //        return obj;
        //    }
        //}

        /**
         * Spawns GameObject from Pool.
         * If there are no free GameObjects and resize option is disabled: display warning
         */
        public GameObject SpawnFromPool(string poolItemName, Vector3 position, Quaternion rotation)
        {
            _poolDictionary.TryGetValue(poolItemName, out Queue<GameObject> poolWithGameObjects);
            if (poolWithGameObjects == null)
            {
                Debug.LogWarningFormat("Poll with name: {0} doesn't exist", poolItemName);
                return null;
            }

            try
            {
                //get free GameObject from Pool queue of GameObjects
                GameObject obj = poolWithGameObjects.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                
                return obj;
            }
            catch (InvalidOperationException)
            {
                //there are no more free GameObjects in the pool queue
                _autoResize.TryGetValue(poolItemName, out var autoresize);
                _objectsPoolItems.TryGetValue(poolItemName, out ObjectPoolItem objectPoolItem);
                
                if (objectPoolItem == null)
                {
                    Debug.LogWarningFormat("Poll with name: {0} doesn't exist", poolItemName);
                    return null;
                }
                
                if (autoresize)
                { 
                    SpawnGO(objectPoolItem.prefab, objectPoolItem.parent, poolWithGameObjects); 
                    return SpawnFromPool(poolItemName, position, rotation);
                }
                    
                Debug.LogWarningFormat("Poll with name: {0} is empty. Autoresize disabled", poolItemName);
                return null;
            }
        }

        //We don't need this GameObject anymore. Set it at 0,0,0 position and disable it.
        public void ReleaseBackToPool(string poolItemName, GameObject obj)
        {
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            _poolDictionary[poolItemName].Enqueue(obj);
        }

        //create GameObject and ands it into Pool queue
        private void SpawnGO(GameObject prefab, Transform parent, Queue<GameObject> objectPool)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            objectPool.Enqueue(obj);
            obj.SetActive(false);
        }
    }
}