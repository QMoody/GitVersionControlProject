using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public Material textureMat;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    [Header("Create Noise Plane Variables")]
    //public float planePosX;
    //public float planePosZ;
    public int planeX;
    public int planeZ;
    public float planeScale;
    [Range(0.0f, 10.0f)] public float perlinFreq;

    [Header("Change Noise Plane Variables")]
    public bool autoUpdate;

    //[Header("Output Variables")] // Do not edit in inspector

    [Header("Mesh Variables")]
    Mesh mesh;
    Vector3[,] verticesMatrix;
    Vector3[] verts;

    [Header("Hidden Variables")]
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
        //if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        if (autoUpdate == true && noiseFieldGenerated == true)
            UpdateField();
    }

    public void GenerateNoiseField()
    {
        if (noiseFieldGenerated == false)
        {
            FieldSetup();

            noiseFieldGenerated = true;
        }
        else if (noiseFieldGenerated == true && autoUpdate == false)
        {
            //UpdateField();
        }
    }

    void UpdateField()
    {
        int i = 0;
        for (int x = 0; x < planeX; x++)
            for (int z = 0; z < planeZ; z++)
            {
                SetVerticies(i, x, z);
                i++;
            }

        Debug.Log("Updated field");

        mesh.vertices = verts;
        m_meshCollider.sharedMesh = mesh;
    }

    private void SetVerticies(int i, int x, int z)
    {
        int xf = x + (int)transform.position.x * 2;
        int zf = z + (int)transform.position.z * 2;
        verticesMatrix[x, z] = new Vector3(x * planeScale, GetPerlinValue(xf, zf), z * planeScale);
        verts[i] = verticesMatrix[x, z];

        //if (noiseFieldGenerated == false)
        //    Debug.Log("Value: " + xf + " / " + zf);
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
                SetVerticies(i, x, z);
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

        m_meshCollider.sharedMesh = mesh;
        m_meshRenderer.material = textureMat;
    }
}