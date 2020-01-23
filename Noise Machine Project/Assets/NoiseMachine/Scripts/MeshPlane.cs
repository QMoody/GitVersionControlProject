using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshPlane : MonoBehaviour
{

    //Modified Scripted by Matej Vanco https://youtu.be/c-pqEHR1jnw
    //Add https://catlikecoding.com/unity/tutorials/procedural-grid/
    public bool Editable = false;

    //public List<Transform> points;

    public Material m_textureMaterial;

    public float width = 2;
    public float length = 2;
    [Range(1, 2)]
    public int n_Subdivides;

    Vector3[] verts;

    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    void Start()
    {
        if (!GetComponent<MeshFilter>())
        {
            transform.gameObject.AddComponent<MeshFilter>();
        }
        m_meshFilter = GetComponent<MeshFilter>();

        if (!GetComponent<MeshRenderer>())
        {
            transform.gameObject.AddComponent<MeshRenderer>();
        }
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (!GetComponent<MeshCollider>())
        {
            transform.gameObject.AddComponent<MeshCollider>();
        }
        m_meshCollider = GetComponent<MeshCollider>();

        if (m_textureMaterial == null)
        {
            m_textureMaterial = new Material(Shader.Find("Diffuse"));
        }

        Mesh m_mesh = new Mesh();
        m_mesh.name = "NewPlane";

        int n_Verts = Mathf.RoundToInt(Mathf.Pow((n_Subdivides+1), 2));
        verts = new Vector3[n_Verts];
        Vector3[,] verticeMatrix = new Vector3[n_Subdivides+1, n_Subdivides+1];
        float widthSpace = width / n_Subdivides; //this is the length inbetween each vertice
        float lengthSpace = length / n_Subdivides;

        int orderInArray = 0;
        for (int x = 0; x < n_Subdivides+1; x++)
            for (int z = 0; z < n_Subdivides+1; z++)
            {
                verticeMatrix[x, z] = new Vector3(widthSpace * z, 0.01f, lengthSpace * x);                
                verts[orderInArray] = verticeMatrix[x, z];
                orderInArray++;
            }

        m_mesh.vertices = verts;

        //m_mesh.vertices = new Vector3[]
        //{
        //     new Vector3(-width, 0.01f, length),
        //     new Vector3(width, 0.01f, length),
        //     new Vector3(width, 0.01f, -length),
        //     new Vector3(-width, 0.01f, -length),
        //};

        //m_mesh.uv = new Vector2[n_Verts];
        //orderInArray = 0;
        //Vector2[,] uvMatrix = new Vector2[n_Subdivides, n_Subdivides];
        //for (int x = 0; x < n_Subdivides; x++)
        //    for (int z = 0; z < n_Subdivides; z++)
        //    {
        //        uvMatrix[x, z] = new Vector2(x / n_Subdivides / 2, z / n_Subdivides / 2);
        //        m_mesh.uv[orderInArray] = uvMatrix[x, z];
        //        orderInArray++;
        //    }

        //m_mesh.uv = new Vector2[]
        //{
        //     new Vector2 (0, 0),
        //     new Vector2 (0, 1),
        //     new Vector2 (1, 1),
        //     new Vector2 (1, 0),
        //};

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

        //int n_Triangles = Mathf.RoundToInt(Mathf.Pow(n_Subdivides, 2) * 2);
        //List<int> triangles = new List<int>(n_Triangles);
        //List<int> triangle1 = new List<int> { 0, 2, 1 };
        //List<int> triangle2 = new List<int> { 2, 3, 1 };
        //for (int t = 0; t < n_Triangles + 1; t++)
        //{
        //    List<int> triangle = new List<int> { 0, 2, 1 };
        //    triangles.AddRange(triangle);
        //}
        //    triangles.AddRange(triangle1);
        //triangles.AddRange(triangle2);

        m_mesh.triangles = triangles;

        //int[,,] trisMatrix = new int[n_Verts, n_Verts, n_Verts];
        //for (int x = 0; x < n_Subdivides + 1; x++)
        //{

        //    for (int y = 0; y < n_Subdivides + 1; y++)
        //    {

        //        for (int z = 0; z < n_Subdivides + 1; z++)
        //        {
        //            trisMatrix[x, y, z] = x;
        //        }
        //    }
        //}

        //m_mesh.triangles = new int[] {
        //    0, 1, 2,
        //    2, 1, 3,
        //};

        m_mesh.RecalculateNormals();
        
        //m_mesh.normals = new Vector3[n_Verts];
        //for (int v = 0; v < n_Verts; v++)
        //{
        //    m_mesh.normals[v] = Vector3.up;
        //}


        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
        m_meshRenderer.material = m_textureMaterial;


        for (int v = 0; v < m_mesh.vertices.Length; v++)
        {
            GameObject p = new GameObject();
            p.name = ("p" + v );
            p.transform.position = m_mesh.vertices[v] + transform.position;
            p.transform.parent = this.gameObject.transform;
        }

    }

    void FixedUpdate()
    {
        if (Editable == false)
            return;

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
