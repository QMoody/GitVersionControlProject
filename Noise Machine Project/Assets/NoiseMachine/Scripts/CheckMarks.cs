using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMarks : MonoBehaviour
{

    public GameObject A1;
    public GameObject A2;
    public GameObject A3;
    public GameObject A4;
    public GameObject A5;

    GameObject Achieved;

    // Start is called before the first frame update
    void Awake()
    {
        print(Achieved);
        Achieved = GameObject.Find("AchievementTracker");
        if (Achieved != null)
        {
            //Achieved = GameObject.Find("AchievementTracker");

            if (Achieved.GetComponent<AchievementTracker>().Get6Flag() == true)
            {
                A1.SetActive(true);
            }
            if (Achieved.GetComponent<AchievementTracker>().GetWin() == true)
            {
                A2.SetActive(true);
            }
            if (Achieved.GetComponent<AchievementTracker>().GetNotHit() == true)
            {
                A3.SetActive(true);
            }
            if (Achieved.GetComponent<AchievementTracker>().GetCheater() == true)
            {
                A4.SetActive(true);
            }
            if (Achieved.GetComponent<AchievementTracker>().GetSlowpoke() == true)
            {
                A5.SetActive(true);
            }
        }
    }
}
