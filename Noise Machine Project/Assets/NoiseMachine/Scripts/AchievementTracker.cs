using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class AchievementTracker : MonoBehaviour
{

    public bool Flag6;
    public bool Win;
    public bool NotHit;
    public bool Cheater;
    public bool Slowpoke;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Code to delete new versions of AchievementManager
        GameObject[] a = GameObject.FindGameObjectsWithTag("Respawn");
        if (a.Length >= 2)
        {
            Destroy(a[1]);
            GameObject.Find("Player").GetComponent<Traker>().SetTracker(a[0]);
        }

    }

    public bool Get6Flag()
    {
        return Flag6;
    }
    public void Set6Flag()
    {
        Flag6 = true;
        //gameObject.name = gameObject.name + "X";
    }

    public bool GetWin()
    {
        return Win;
    }
    public void SetWin()
    {
        Win = true;
        //gameObject.name = gameObject.name + "X";
    }

    public bool GetNotHit()
    {
        return NotHit;
    }
    public void SetNotHit()
    {
        NotHit = true;
        //gameObject.name = gameObject.name + "X";
    }

    public bool GetCheater()
    {
        return Cheater;
    }
    public void SetCheater()
    {
        Cheater = true;
        //gameObject.name = gameObject.name + "X";
    }

    public bool GetSlowpoke()
    {
        return Slowpoke;
    }
    public void SetSlowpoke()
    {
        Slowpoke = true;
        //gameObject.name = gameObject.name + "X";
    }

}
