using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Noise))]
public class NoiseEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
            Noise noiseGen = (Noise)target;

            if(DrawDefaultInspector()) {
                if (noiseGen.autoUpdate)
                {
                    noiseGen.GenerateNoiseField();
                }
            }

            if (GUILayout.Button("Generate"))
            {
                noiseGen.GenerateNoiseField();
            }
    }
    */
}
