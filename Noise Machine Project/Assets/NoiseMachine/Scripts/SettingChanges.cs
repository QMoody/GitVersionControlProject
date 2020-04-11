using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingChanges : MonoBehaviour
{

    public GameObject DifE;
    public GameObject DifM;
    public GameObject DifH;
    public GameObject DifI;

    public GameObject difficulty;
    private float difficultyScale;

    public GameObject volMax;
    public GameObject volEffects;
    public GameObject volMusic;
    public GameObject mute;

    private float effectsVol;
    private float musicVol;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Code to delete new versions of SettingsHolder
        GameObject[] a = GameObject.FindGameObjectsWithTag("Settings");
        if (a.Length >= 2)
        {
            Destroy(a[1]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (volMax.GetComponent<Slider>().value > 0)
        {
            mute.SetActive(false);
        }

        //Edge case for first open
        if (DifM.active)
        {
            difficultyScale = 4;
        }

        UpdateValues();

    }

    void UpdateValues()
    {
        //difficultyScale = ;
        effectsVol = volEffects.GetComponent<Slider>().value * (volMax.GetComponent<Slider>().value/100);
        musicVol = volMusic.GetComponent<Slider>().value * (volMax.GetComponent<Slider>().value / 100);
    }
    public void EasyDifficulty()
    {
        DifE.SetActive(true);
        DifM.SetActive(false);
        DifH.SetActive(false);
        DifI.SetActive(false);
        difficultyScale = 2;
    }
    public void MediumDifficulty()
    {
        DifE.SetActive(false);
        DifM.SetActive(true);
        DifH.SetActive(false);
        DifI.SetActive(false);
        difficultyScale = 4;
    }
    public void HardDifficulty()
    {
        DifE.SetActive(false);
        DifM.SetActive(false);
        DifH.SetActive(true);
        DifI.SetActive(false);
        difficultyScale = 6;
    }
    public void XtremeDifficulty()
    {
        DifE.SetActive(false);
        DifM.SetActive(false);
        DifH.SetActive(false);
        DifI.SetActive(true);
        difficultyScale = 8;
    }

    public void MuteSound()
    {
        mute.SetActive(true);
        volMax.GetComponent<Slider>().value = 0;
    }

    public float GetDifficulty()
    {
        return difficultyScale;
    }

}
