using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public GameObject noiseMarker;
    public Material textureMat;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    [Header("Create Noise Plane Variables")]
    public int planeX;
    public int planeZ;
    public float planeScale;
    [Range(0.0f, 10.0f)] public float perlinFreq;
    public Vector2 setRandomValue;

    [Header("Change Noise Plane Variables")]
    public bool autoUpdate;

    //[Header("Output Variables")] // Do not edit in inspector

    [Header("Mesh Variables")]
    Mesh mesh;

    Vector3[,] verticesMatrix;
    Vector3[] verts;

    [Header("Hidden Variables")]
    private GameObject[,] markerObject;
    public float[,] noiseMap; // noiseMap will be an array of floats that matches the transform of markerobjects to produce a value
    private bool noiseFieldGenerated;
    #endregion

    //--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    public float GetPerlinValue(int x, int z)
    {
        Vector2 fracCord = new Vector2(perlinFreq * x / planeX, perlinFreq * z / planeZ);
        return Mathf.PerlinNoise(fracCord.x, fracCord.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        if (autoUpdate == true && noiseFieldGenerated == true)
            UpdateField();
    }

    public void GenerateNoiseField()
    {
        if (noiseFieldGenerated == false)
        {
            //markerObject = new GameObject[planeX, planeZ];

            //for (int x = 0; x < planeX; x++)
            //    for (int z = 0; z < planeZ; z++)
            //    {
            //        GameObject markerTmp = Instantiate(noiseMarker, new Vector3(x * planeScale, GetPerlinValue(x, z), z * planeScale), Quaternion.Euler(0, 0, 0));
            //        markerObject[x, z] = markerTmp;
            //    }

            FieldSetup();

            noiseFieldGenerated = true;
        }
        else if (noiseFieldGenerated == true && autoUpdate == false)
        {
            UpdateField();
        }
    }

    void UpdateField()
    {
        int i = 0;
        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                verticesMatrix[x, z] = new Vector3(x * planeScale, GetPerlinValue(x, z), z * planeScale);
                verts[i] = verticesMatrix[x, z];
                i++;
            }

        mesh.vertices = verts;

        /*
        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                markerObject[x, z].transform.position = new Vector3(markerObject[x, z].transform.position.x, GetPerlinValue(x, z), markerObject[x, z].transform.position.z);
            }
            */
    }

    private void FieldSetup()
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mFilter.mesh = mesh;

        verts = new Vector3[planeX * planeZ];
        verticesMatrix = new Vector3[planeX, planeZ];
        Vector2[] uv = new Vector2[verts.Length];

        int i = 0;
        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                verticesMatrix[x, z] = new Vector3(x * planeScale, GetPerlinValue(x, z), z * planeScale);
                verts[i] = verticesMatrix[x, z];
                uv[i] = new Vector2((float)x / planeX, (float)z / planeZ);
                i++;
            }

        mesh.vertices = verts;
        mesh.uv = uv;

        int[] tris = new int[(planeX - 1) * (planeZ - 1) * 6];
        int g = 0;
        for (int x = 0; x < planeX - 1; x++)
            for (int z = 0; z < planeZ - 1; z++)
            {

                tris[g + 0] = planeZ * x + z;
                tris[g + 1] = planeZ * x + z + 1;
                tris[g + 2] = planeZ * (x + 1) + z;

                tris[g + 3] = planeZ * x + z + 1;
                tris[g + 4] = planeZ * (x + 1) + z + 1;
                tris[g + 5] = planeZ * (x + 1) + z;

                g += 6;
            }

        mesh.triangles = tris;

        Vector3[] normals = new Vector3[planeX * planeZ];
        for (int k = 0; k < normals.Length; k++)
            normals[k] = Vector3.forward;

        mesh.normals = normals;

        /*
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        */
        m_meshRenderer.material = textureMat;
    }

    /*
    private void FieldSetup()
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1)
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
    */
}