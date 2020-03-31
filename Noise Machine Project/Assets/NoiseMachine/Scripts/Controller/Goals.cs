using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals : MonoBehaviour
{
    public MeshRenderer mesh;
    public Color startColor;
    public Color changeColor;
    bool trigger;

    // Start is called before the first frame update
    void Start()
    {
        if (startColor == null)
            startColor = mesh.material.color;
        trigger = false;
    }

    public void Touched()
    {
        if (!trigger)
        {
            mesh.material.color = changeColor;
            Traker.inst.AddScore();
            trigger = true;
        }
    }
}
