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
        m_mesh.vertices = new Vector3[]
        {
             new Vector3(-width, 0.01f, length),
             new Vector3(width, 0.01f, length),
             new Vector3(width, 0.01f, -length),
             new Vector3(-width, 0.01f, -length),
             new Vector3(0, 0.01f, 0)
        };

        m_mesh.uv = new Vector2[]
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2 (1, 1),
             new Vector2 (1, 0),
             new Vector2 (.5f,.5f)
        };

        m_mesh.triangles = new int[] {
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
        };
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
