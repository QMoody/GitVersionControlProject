using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
       print(c.gameObject);
       if (c.gameObject.tag == "Respawn")
        {
            //end run
            //display score
            //reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
