using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;

public class Traker : MonoBehaviour
{
    public static Traker inst = null;


    //Initializations for positions used in calculations
    float YSPos;
    float ZSPos;

    float YCPos;
    float ZCPos;

    float YPOSroc;
    float ZPOSroc;

    float totalDis;
    public float goalDis = 250;

    //UI references
    public GameObject distance;
    public GameObject winText;
    public GameObject speedometer;
    public float currentSpeed;
    float fastestSpeed;
    bool isPaused;
    public GameObject PauseMenu;
    public GameObject dTravel;
    public GameObject mSpeed;
    public int flagScore;
    public GameObject flagScoreText;
    private int hitScore;

    public GameObject PauseButton;
    public Text PauseText;

    float startTime;
    float endTime;

    public GameObject Achieved;
    public GameObject Trophy;

    //Making this a singleton
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            Debug.Log("Inst is made");
        }
        else if (inst != this)
        {
            Debug.LogWarning("Multible Trakers. One was destroy");
            Destroy(this);
        }

        startTime = Time.time;

        YSPos = transform.position.y;
        ZSPos = transform.position.z;
        fastestSpeed = 0;
        totalDis = 0;
        isPaused = false;
        Time.timeScale = 1;

    }

    internal void Lose()
    {
        
        Analytics.CustomEvent("GameOver", new Dictionary<string, object>
        {
            { "Time", Time.time-startTime },
            { "FastestSpeed", fastestSpeed },
            {"DistanceTraveled",totalDis },
            { "ObstaclesHit", hitScore },
            { "FlagScore", flagScore }
        }
        );

        PauseText.GetComponent<Text>().text = "Game Over";
        PauseButton.SetActive(false);
        Paused();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    internal void AddScore()
    {
        flagScore++;
        flagScoreText.GetComponent<Text>().text = flagScore.ToString();
    }

    internal void AddHit()
    {
        hitScore++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        currentSpeed = (Mathf.FloorToInt(GetComponent<Rigidbody>().velocity.magnitude * 3.6F));

        UpdateCurrentSpeed();
        DistanceTravelled();


        if (totalDis > goalDis)
        {
            endTime = Time.time;
            winText.SetActive(true); //Shows text that you have won
            Invoke("Win", 3); //Restarts the game after 3 seconds

        }
        if (isPaused)
        {
            Paused();
        }

        CheckForAchievements();
    }

    void UpdateCurrentSpeed()
    {
        //UI update for players current speed in km/h
        speedometer.GetComponent<Text>().text = currentSpeed + " km/h";
        if (currentSpeed > fastestSpeed)
        {
            fastestSpeed = currentSpeed;
        }
        //Will be a speedometer arrow that shows that same thing
    }

    void DistanceTravelled()
    {
        //calculates total distance travelled
        YCPos = transform.position.y;
        ZCPos = transform.position.z;

        YPOSroc = (YSPos - YCPos);
        ZPOSroc = (ZCPos - ZSPos);

        totalDis = Mathf.FloorToInt(Mathf.Sqrt((YPOSroc * YPOSroc) + (ZPOSroc * ZPOSroc)));


        //UI update that shows the elevation (distance travelled) of the player moving towards the goal by moving the text displaying the elevation from the start until the goal (happens in the position.y)
        distance.GetComponent<RectTransform>().position = new Vector3(distance.GetComponent<RectTransform>().position.x, 500 - (500 * (totalDis / goalDis)), distance.GetComponent<RectTransform>().position.z);

        //UI update that shows the elevation (distance travelled) in text
        distance.GetComponent<Text>().text = (totalDis).ToString() + "m";
    }

    void Win()
    {
        Analytics.CustomEvent("ReachEnd", new Dictionary<string, object>
        {
            { "Time", Time.time-startTime },
            { "FastestSpeed", fastestSpeed },
            {"DistanceTraveled",totalDis },
            { "ObstaclesHit", hitScore },
            { "FlagScore", flagScore }
        }
        );

        PauseText.text = "You Reached The Bottom! Good Job";
        PauseButton.SetActive(false);
        Paused();
    }

    void Paused()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        dTravel.GetComponent<Text>().text = "Distance Travelled: " + (totalDis).ToString() + "m";
        mSpeed.GetComponent<Text>().text = "Fastest Speed Recorded: " + (fastestSpeed).ToString() + "km/h";
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            PauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else if (!isPaused)
        {
            isPaused = true;
        }

    }

    public GameObject GetTracker()
    {
        return Achieved;
    }

    public void SetTracker(GameObject t)
    {
        Achieved = t;
    }

    public void GoHome()
    {
        Paused();
        SceneManager.LoadScene("MainMenu");
    }

    void ResetTrophy()
    {
        Trophy.SetActive(false);
    }

    public void CheckForAchievements()
    {

        if (flagScore == 6 && Achieved.GetComponent<AchievementTracker>().Get6Flag() == false)
        {
            //get 6 flags
            Achieved.GetComponent<AchievementTracker>().Set6Flag();
            Trophy.SetActive(true);
            Invoke("ResetTrophy", 2);
        }
        if (totalDis > 1000 && Achieved.GetComponent<AchievementTracker>().GetWin() == false)
        {
            //make it to the bottom for the first time
            Achieved.GetComponent<AchievementTracker>().SetWin();
            Trophy.SetActive(true);
            Invoke("ResetTrophy", 2);
        }
        if (hitScore == 0 && totalDis > 1000 && Achieved.GetComponent<AchievementTracker>().GetNotHit() == false)
        {
            //get to end without hiting anything
            Achieved.GetComponent<AchievementTracker>().SetNotHit();
            Trophy.SetActive(true);
            Invoke("ResetTrophy", 2);
        }
        if (fastestSpeed >= 155 && Achieved.GetComponent<AchievementTracker>().GetCheater() == false)
        {
            //fall off side
            Achieved.GetComponent<AchievementTracker>().SetCheater();
            Trophy.SetActive(true);
            Invoke("ResetTrophy", 2);
        }
        if (endTime != null && endTime - startTime > 180 && Achieved.GetComponent<AchievementTracker>().GetSlowpoke() == false)
        {
            //reach bottom after 3 min
            Achieved.GetComponent<AchievementTracker>().SetSlowpoke();
            Trophy.SetActive(true);
            Invoke("ResetTrophy", 2);
        }
    }
}
