using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPlane : MonoBehaviour
{

    //Modified Scripted by Matej Vanco https://youtu.be/c-pqEHR1jnw
    public bool Editable = false;

    //public List<Transform> points;

    public Material m_textureMaterial;

    public float width = 2;
    public float length = 2;
    [Range(2,3)]
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

        int n_Verts = Mathf.RoundToInt(Mathf.Sqrt(n_Subdivides));
        verts = new Vector3[n_Verts];
        Vector3[,] verticeMatrix = new Vector3[n_Subdivides, n_Subdivides];
        float widthSpace = width / n_Subdivides; //this is the length inbetween each vertice
        float lengthSpace = length / n_Subdivides;

        int orderInArray = -1;
        for (int x = 0; x < n_Subdivides; x++)
                for (int z = 0; z < n_Subdivides; z++)
                {
                    verticeMatrix[x,z] = new Vector3(widthSpace*z, 0.01f, lengthSpace*x);
                    orderInArray++;
                    verts[orderInArray] = verticeMatrix[x, z];
                }

        m_mesh.vertices = verts;
        //m_mesh.vertices = new Vector3[]
        //{
        //     new Vector3(-width, 0.01f, length),
        //     new Vector3(width, 0.01f, length),
        //     new Vector3(width, 0.01f, -length),
        //     new Vector3(-width, 0.01f, -length),
        //     new Vector3(0, 0.01f, 0)
        //};
        m_mesh.uv = new Vector2[n_Verts];

        Vector2[,] uvMatrix = new Vector2[n_Subdivides, n_Subdivides];
        for (int x = 0; x < n_Subdivides; x++)
            for (int z = 0; z < n_Subdivides; z++)
            {
                uvMatrix[x,z] = new Vector2(x/n_Subdivides , z/n_Subdivides);
                orderInArray++;
                m_mesh.uv[orderInArray] = uvMatrix[x, z];
            }
        //m_mesh.uv = new Vector2[]
        //{
        //     new Vector2 (0, 0),
        //     new Vector2 (0, 1),
        //     new Vector2 (1, 1),
        //     new Vector2 (1, 0),
        //     new Vector2 (.5f,.5f)
        //};

        //int nOfTriangles = Mathf.RoundToInt(Mathf.Sqrt(nOfSubdivides-1)*2);
        //int[,,] trisMatrix = new int[nOfVerts,nOfVerts,nOfVerts];
        //for (int x = 0; x < nOfSubdivides; x++)
        //    for (int y = 0; y < nOfSubdivides; y++)
        //        for (int z = 0; z < nOfSubdivides; z++)
        //        {
        //            trisMatrix[x,y,z] = 1;
        //        }

        m_mesh.triangles = new int[] {
            0, 1, 3,
            0, 2, 3,
        };

        //m_mesh.triangles = new int[] {
        //    0, 1, 4,
        //    1, 2, 4,
        //    2, 3, 4,
        //    3, 0, 4
        //};
        m_mesh.RecalculateNormals();       

        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
        m_meshRenderer.material = m_textureMaterial;
        

        for (int v = 0; v < m_mesh.vertices.Length; v++)
        {
            GameObject p = new GameObject();
            p.name = ("p" + (v+1));
            p.transform.position = m_mesh.vertices[v]+transform.position;
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
