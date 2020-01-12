using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    public GameObject noiseMarker;
    public int planeX;
    public int planeZ;
    public float heightScale;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting program
    private void Start()
    {
        for (int x = 0; x < planeX; x++)
        {
            for (int z = 0; z < planeZ; z++)
            {
                Vector2 fracCord = new Vector2((x + 0.01f) / planeX, (z + 0.01f) / planeZ); // Whole numbers return same value // Same values will always return same heights
                float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale; // Height scale will change the noise intensity
                Instantiate(noiseMarker, new Vector3(x, noiseYValue, z), Quaternion.Euler(0, 0, 0));
                Debug.Log(new Vector3(x, noiseYValue, z));
            }
        }
    }
}