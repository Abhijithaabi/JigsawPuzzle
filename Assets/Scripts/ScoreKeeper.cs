using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    float timeValue;
    [SerializeField]float highscoreTime = 0;
    
    [SerializeField] List<float> highScoreList = new List<float>(5);
    public static ScoreKeeper Instance;
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetHighScoreList();

        //highScoreList.Clear();
    }

    public void SetHighScoreList()
    {
        if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            highscoreTime = PlayerPrefs.GetFloat("HighScore16x", 0);
            highScoreList = PlayerPrefsExtra.GetList<float>("HighScore16xList");
            
        }
        else
        {
            highscoreTime = PlayerPrefs.GetFloat("HighScore9x", 0);
            highScoreList = PlayerPrefsExtra.GetList<float>("HighScore9xList");
        }
    }

    public void SetCurrentScore(float timeinSec)
    {
        Debug.Log("Inside ScoreKeeper SetCurrentScore");
        timeValue = timeinSec;
        //AddtoHighscoreList(timeValue);
        Debug.Log("Inside setCurrentScore before checking highscore Condition");
        
        if(highscoreTime > timeValue || highscoreTime == 0)
        {
            Debug.Log("Inside setCurrentScore checking highscore Condition");
             SetHighScore(timeValue);
        }
        
        
       
    }
    public void SetHighScore(float time)
    {
        highscoreTime = time;
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            PlayerPrefs.SetFloat("HighScore16x",highscoreTime);
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore9x",highscoreTime);
        }
        
        AddtoHighscoreList(highscoreTime);
    }
    public float GetHighScore()
    {
        
        return highscoreTime;
    }
    void AddtoHighscoreList(float value)
    {
        highScoreList.Sort();
        if(highScoreList.Count == 0 || highScoreList.Count<5)
        {
            highScoreList.Add(value);
        }
        else
        {
            // foreach(float highScoreValue in highScoreList)
            // {
            //     if(highScoreValue > value || highScoreValue == 0)
            //     {

            //         highScoreList.Add(value);
            //         break;
            //     }
            // }
            if(highScoreList[4]>value)
            {
                highScoreList[4] = value;
            }
        }
        
        highScoreList.Sort();
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            PlayerPrefsExtra.SetList("HighScore16xList",highScoreList);
        }
        else
        {
            PlayerPrefsExtra.SetList("HighScore9xList",highScoreList);
        }
        
    }
    public List<float> GetHighScoreList()
    {
        SetHighScoreList();
        return highScoreList;
    }
}
