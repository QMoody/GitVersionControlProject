using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshPlane))]
public class MeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshPlane meshGen = (MeshPlane)target;

        if (DrawDefaultInspector())
        {
            if (meshGen.Editable)
            {
                meshGen.UpdateMesh();
            }
        }

        if (GUILayout.Button("ApplyNoiseMap"))
        {
            meshGen.m_noise.GenerateNoiseField();
            meshGen.ApplyNoiseMap(meshGen.m_noise.noiseMap);
        }

        if (GUILayout.Button("Reset"))
        {
            meshGen.DestroyMesh();
            meshGen.CreateShape();
        }

    }
}
