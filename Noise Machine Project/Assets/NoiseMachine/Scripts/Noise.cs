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
    public bool randStartPoint;
    public int planeX;
    public int planeZ;
    public float heightScale;
    public float noisePlaneScale; // 1 - normal scale / <1 - larger scale / >1 smaller scale
    public float setRandomValue;

    [Header("Mesh Plane Variabels")]
    int tmpVM;

    [Header("Output Variabels")] // Do not edit in inspector
    public float randNum;

    [Header("Hidden Variabels")]
    private GameObject[,] markerObject;
    private bool noiseFieldGenerated;
    private float startPoint;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    // Make this into a function that can be repeated without starting function

    public bool autoUpdate;

    private void Start()
    {
        //FieldSetup();
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
            GenerateNoiseField();

    }

    public void GenerateNoiseField()
    {
        if (markerObject != null)
            for (int x = 0; x < planeX; x++)
                for (int z = 0; z < planeZ; z++)
                    Destroy(markerObject[x, z]);

        markerObject = new GameObject[planeX, planeZ];

        if (randNoiseLoc == true)
            randNum = Random.Range(-1.000f, 1.000f);
        else if (customRandNoise == true)
            randNum = setRandomValue;
        else
            randNum = 1;

        if (randStartPoint == true)
            startPoint = Random.Range(-1.000f, 1.000f);
        else
            startPoint = 0;

        Vector3[] tmpPoints = new Vector3[4];

        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                Vector2 fracCord = new Vector2((x + startPoint) / (planeX / noisePlaneScale) / randNum, (z + startPoint) / (planeZ / noisePlaneScale) / randNum); // Whole numbers return same Y value // Same values will always return same noise heights
                float noiseYValue = Mathf.PerlinNoise(fracCord.x, fracCord.y) * heightScale; // Height scale will change the noise intensity
                GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x, noiseYValue, z), Quaternion.Euler(0, 0, 0));
                markerObject[x,z] = markerTmp;

                if (x < 2 && z < 2)
                {
                    tmpPoints[tmpVM] = new Vector3(x, noiseYValue, z);
                    tmpVM += 1;
                }
            }

        tmpVM = 0;
        FieldSetup(tmpPoints[0], tmpPoints[1], tmpPoints[2], tmpPoints[3]);
    }

    private void FieldSetup(Vector3 tmp1, Vector3 tmp2, Vector3 tmp3, Vector3 tmp4)
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[4]
        {
            tmp3,
            tmp4,
            tmp1,
            tmp2
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
    }

    private void CreateFieldMesh()
    {

    }
}