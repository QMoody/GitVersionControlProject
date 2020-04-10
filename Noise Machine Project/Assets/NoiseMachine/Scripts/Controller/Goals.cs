using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals : MonoBehaviour
{
    public MeshRenderer mesh;
    public Material startColor;
    public Material changeColor;
    bool trigger;

    // Start is called before the first frame update
    void Start()
    {
        if (startColor == null)
            startColor = mesh.material;
        trigger = false;
    }

    public void OnEnable()
    {
        mesh.material = startColor;
        trigger = false;
    }

    public void Touched()
    {
        if (!trigger)
        {
            mesh.material = changeColor;
            Traker.inst.AddScore();
            trigger = true;
        }
    }
}
