using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveMesh : MonoBehaviour
{
    private Noise[] GeneratedMeshes;
    public bool RunThisScriptOnPlay;

    void Start()
    {
        if (RunThisScriptOnPlay)
        {
            GeneratedMeshes = FindObjectsOfType<Noise>();
            int ID = 0;
            foreach (Noise n in GeneratedMeshes)
            {
                //Create a mesh to make the generated one
                GameObject newMesh = n.gameObject;
                string localPath = "Assets/NoiseMachine/Models/GeneratedMeshes/LandMesh" + ID + ".prefab";

                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAssetAndConnect(newMesh,localPath,InteractionMode.UserAction);

                // Print the path of the created asset
                Debug.Log(AssetDatabase.GetAssetPath(newMesh));
                ID++;
            }
            AssetDatabase.SaveAssets();
        }
    }
}
