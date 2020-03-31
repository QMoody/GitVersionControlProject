using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Noise : MonoBehaviour
{
    #region Variables
    [Header("Objects & Scripts")]
    public Material textureMat;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    [Header("Create Noise Plane Variables")]
    public int planeSize;
    public float planeScale;
    public Vector2 planeWorldPos;
    public float perlinFreq;

    [Header("Change Noise Plane Variables")]
    public bool autoUpdate;

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
        Vector2 fracCord = new Vector2(((float)x / (planeSize - 1)) * perlinFreq + planeWorldPos.x * perlinFreq, ((float)z / (planeSize - 1)) * perlinFreq + planeWorldPos.y * perlinFreq);

        //Debug.Log("[" + x + " / " + (planeSize - 1) + " -/- " + fracCord.x + " -/- " + fracCord.y);
        //Debug.Log(planeWorldPos.x);

        return Mathf.PerlinNoise(fracCord.x, fracCord.y);
    }

    private void Update()
    {
        //if (Input.GetKeyDown("r"))
            GenerateNoiseField();

        //if (autoUpdate == true && noiseFieldGenerated == true)
        //    UpdateField();
    }

    public void GenerateNoiseField()
    {
        //Check for first time noise field generation
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

    public void UpdateField()
    {
        int i = 0;
        for (int x = 0; x < planeSize; x++)
            for (int z = 0; z < planeSize; z++)
            {
                SetVerticies(i, x, z);
                i++;
            }

        mesh.vertices = verts;
        m_meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    private void SetVerticies(int i, int x, int z)
    {
        //Set the mesh field vert points
        verticesMatrix[x, z] = new Vector3(x * planeScale, GetPerlinValue(x, z) + z / (2.5f / planeScale), z * planeScale);
        verts[i] = verticesMatrix[x, z];
    }

    //First time field setup
    private void FieldSetup()
    {
        MeshFilter mFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mFilter.mesh = mesh;

        //Set Verts
        verts = new Vector3[planeSize * planeSize];
        verticesMatrix = new Vector3[planeSize, planeSize];
        Vector2[] uv = new Vector2[verts.Length];

        int i = 0;
        for (int x = 0; x < planeSize; x++)
            for (int z = 0; z < planeSize; z++)
            {
                SetVerticies(i, x, z);
                uv[i] = new Vector2((float)x / planeSize, (float)z / planeSize);
                i++;
            }

        mesh.vertices = verts;
        mesh.uv = uv;

        //Set tris
        int[] tris = new int[(planeSize - 1) * (planeSize - 1) * 6];
        int g = 0;
        for (int x = 0; x < planeSize - 1; x++)
            for (int z = 0; z < planeSize - 1; z++)
            {

                tris[g + 0] = planeSize * x + z;
                tris[g + 1] = planeSize * x + z + 1;
                tris[g + 2] = planeSize * (x + 1) + z;

                tris[g + 3] = planeSize * x + z + 1;
                tris[g + 4] = planeSize * (x + 1) + z + 1;
                tris[g + 5] = planeSize * (x + 1) + z;

                g += 6;
            }

        mesh.triangles = tris;

        //Set normals
        Vector3[] normals = new Vector3[planeSize * planeSize];
        for (int k = 0; k < normals.Length; k++)
            normals[k] = Vector3.forward;

        mesh.normals = normals;

        m_meshCollider.sharedMesh = mesh;
        m_meshRenderer.material = textureMat;
        mesh.RecalculateNormals();
    }
}