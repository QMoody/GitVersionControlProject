using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public float timeTillRestart = 0.5f;
    Traker p;
    float onCollisionSpeed;

    void Start()
    {
        p = Traker.inst;
    }    

    private void OnCollisionEnter(Collision c)
    {
       //print(c.gameObject);
       if (c.gameObject.tag == "Obstacle")
       {
            onCollisionSpeed = p.currentSpeed;
            //end run
            //display score
            //reload scene
            StartCoroutine("TestIfStopped");
       }
    }

    IEnumerator TestIfStopped()
    {
        yield return new WaitForSeconds(timeTillRestart);
        if ((onCollisionSpeed - p.currentSpeed) >= 40)
        {
            p.Ragdoll();
            //Debug.Log("Hit my head. Going to reload level");
            yield return new WaitForSeconds(4);
            p.Lose();
            
        }
        
    }

}
