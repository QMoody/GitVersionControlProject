using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public float timeTillRestart = 5;
    public Traker p;

    void Start()
    {
        if (p == null)
        {
            if (!GetComponent<Traker>())
            {
                Debug.LogError("Missing Traker");
            }
            p = GetComponent<Traker>();
        }
    }

    

    private void OnCollisionEnter(Collision c)
    {
       //print(c.gameObject);
       if (c.gameObject.tag == "Obstacle")
       {

            //end run
            //display score
            //reload scene
            StartCoroutine("TestIfStopped");
       }
    }

    IEnumerator TestIfStopped()
    {
        yield return new WaitForSeconds(timeTillRestart);
        if (p.currentSpeed <= 5)
        {
            Debug.Log("Hit my head. Going to reload level");
            yield return new WaitForSeconds(1);
            Traker.inst.Lose();
            
        }
        
    }

}
