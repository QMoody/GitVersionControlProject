using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPlane : MonoBehaviour
{

    //Modified Scripted by Matej Vanco https://youtu.be/c-pqEHR1jnw
    public bool Editable = false;

    public Transform p1, p2, p3, p4;

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
             new Vector3(-width, 0.01f, -length)
        };

        m_mesh.uv = new Vector2[]
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };

        m_mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m_mesh.RecalculateNormals();       

        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
        m_meshRenderer.material = m_textureMaterial;
        verts = m_meshFilter.mesh.vertices;

    }

    void FixedUpdate()
    {
        if (Editable == false)
            return;        

        Vector3[] newVerts = new Vector3[]
        {
             p1.position,
             p2.position,
             p3.position,
             p4.position,
        };

        if (verts == newVerts)
            return;

        verts = newVerts;
        m_meshFilter.mesh.vertices = verts;
        m_meshCollider.sharedMesh = null;
        m_meshCollider.sharedMesh = m_meshFilter.mesh;
    }
}
