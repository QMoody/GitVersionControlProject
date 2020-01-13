using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public GameObject noiseMarker;

    [Header("Noise Plane Variabels")]
    public bool randNoiseLoc;
    public bool customRandNoise; // Use if you want to set custom random value
    public int planeX;
    public int planeZ;
    public float heightScale;
    public float noisePlaneScale; // 1 - normal scale / <1 - larger scale / >1 smaller scale
    public float setRandomValue;

    [Header("Output Variabels")] // Do not edit in editor
    public float randNum;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function
    private void Start()
    {
        if (randNoiseLoc == true)
            randNum = Random.Range(-1.000f, 1.000f);
        else if (customRandNoise == true)
            randNum = setRandomValue;
        else
            randNum = 1;

        for (int x = 0; x < planeX; x++)
        {
            for (int z = 0; z < planeZ; z++)
            {
                Vector2 fracCord = new Vector2((x + 0.01f) / (planeX / noisePlaneScale) / randNum, (z + 0.01f) / (planeZ / noisePlaneScale) / randNum); // Whole numbers return same Y value // Same values will always return same noise heights
                float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale; // Height scale will change the noise intensity
                Instantiate(noiseMarker, new Vector3(x, noiseYValue, z), Quaternion.Euler(0, 0, 0));
                Debug.Log(new Vector3(x, noiseYValue, z));
            }
        }
    }

    private void GenerateNoiseField()
    {

    }
}