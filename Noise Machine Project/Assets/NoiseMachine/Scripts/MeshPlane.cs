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

    void Start()
    {
        if (!GetComponent<MeshFilter>())
        {
            transform.gameObject.AddComponent<MeshFilter>();
        }

        if (!GetComponent<MeshRenderer>())
        {
            transform.gameObject.AddComponent<MeshRenderer>();
        }

        if (!GetComponent<MeshCollider>())
        {
            transform.gameObject.AddComponent<MeshCollider>();
        }

        if (m_textureMaterial == null)
        {
            m_textureMaterial = new Material(Shader.Find("Diffuse"));
        }

        Mesh m = new Mesh();
        m.name = "NewPlane";
        m.vertices = new Vector3[]
        {
             new Vector3(-width, 0.01f, length),
             new Vector3(width, 0.01f, length),
             new Vector3(width, 0.01f, -length),
             new Vector3(-width, 0.01f, -length)
        };

        m.uv = new Vector2[]
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };

        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();       

        GetComponent<MeshFilter>().mesh = m;
        GetComponent<MeshCollider>().sharedMesh = m;
        GetComponent<MeshRenderer>().material = m_textureMaterial;

    }

    void Update()
    {
        if (Editable == false)
            return;
        if (!GetComponent<MeshFilter>())
        {
            Debug.LogError("Mesh does not contain Mesh Filter!");
            return;
        }

        Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
        verts = new Vector3[]
        {
             p1.position,
             p2.position,
             p3.position,
             p4.position,
        };
       
        GetComponent<MeshFilter>().mesh.vertices = verts;
        GetComponent<MeshCollider>().sharedMesh.vertices = verts;
    }
}
