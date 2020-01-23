using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MeshPlane : MonoBehaviour
{

    //Modified Scripted by Matej Vanco https://youtu.be/c-pqEHR1jnw
    //Add https://catlikecoding.com/unity/tutorials/procedural-grid/
    public bool Editable = false;

    //public List<Transform> points;

    public Material m_textureMaterial;

    public float width = 2;
    public float length = 2;
    [Range(1, 5)]
    public int n_Subdivides;

    Vector3[] verts;

    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    void Start()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();

        if (m_textureMaterial == null)
        {
            m_textureMaterial = new Material(Shader.Find("Diffuse"));
        }

        CreateShape();
        UpdateMesh();
    }   

    private void CreateShape()
    {
        Mesh m_mesh = new Mesh();
        m_mesh.name = "NewPlane";

        int n_Verts = Mathf.RoundToInt(Mathf.Pow((n_Subdivides + 1), 2));
        verts = new Vector3[n_Verts];
        Vector3[,] verticeMatrix = new Vector3[n_Subdivides + 1, n_Subdivides + 1];
        float widthSpace = width / n_Subdivides; //this is the length inbetween each vertice
        float lengthSpace = length / n_Subdivides;
        Vector2[] uv = new Vector2[n_Verts];

        int i = 0;
        for (int x = 0; x < n_Subdivides + 1; x++)
            for (int z = 0; z < n_Subdivides + 1; z++)
            {
                verticeMatrix[x, z] = new Vector3(widthSpace * z, 0.01f, lengthSpace * x);
                verts[i] = verticeMatrix[x, z];
                uv[i] = new Vector2((float)x / n_Subdivides, (float)z / n_Subdivides);
                i++;
            }

        m_mesh.vertices = verts;
        m_mesh.uv = uv;

        int[] triangles = new int[n_Subdivides * n_Subdivides * 6];

        for (int ti = 0, vi = 0, y = 0; y < n_Subdivides; y++, vi++)
        {
            for (int x = 0; x < n_Subdivides; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + n_Subdivides + 1;
                triangles[ti + 5] = vi + n_Subdivides + 2;
            }
        }

        m_mesh.triangles = triangles;

        m_mesh.RecalculateNormals();
        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
        m_meshRenderer.material = m_textureMaterial;
        verts = m_meshFilter.mesh.vertices;

        for (int v = 0; v < m_mesh.vertices.Length; v++)
        {
            GameObject p = new GameObject();
            p.name = ("p" + (v+1));
            p.transform.position = m_mesh.vertices[v]+transform.position;
            p.transform.parent = this.gameObject.transform;
        }

    }

    private void UpdateMesh()
    {

        if (verts.Length != transform.childCount)
        {
            Debug.LogError("Vertices GameObject does not equal verts array");
            return;
        }

        Vector3[] newVerts = new Vector3[transform.childCount];

        for (int v = 0; v < transform.childCount; v++)
        {
            newVerts[v] = transform.GetChild(v).transform.position;
        }

        if (verts == newVerts)
            return;

        verts = newVerts;
        m_meshFilter.mesh.vertices = verts;
        m_meshCollider.sharedMesh = null;
        m_meshCollider.sharedMesh = m_meshFilter.mesh;
    }

    void FixedUpdate()
    {
        if (Editable == false)
            return;
        UpdateMesh();
    }

    private void OnDrawGizmos()
    {
        if (gameObject.transform.childCount != 0)
        {
            for (int v = 0; v < verts.Length; v++)
            {
                Gizmos.DrawIcon(transform.GetChild(v).transform.position, "Assets/NoiseMachine/Textures/Circle.png", false);
            }
        }
    }
}
