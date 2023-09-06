using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JodoScoreKeeper : MonoBehaviour
{
    
    float timeValue;
    string teamCityName;
    string difficultyGame;
    bool isTeamCompleted = false;

    [SerializeField] float highscoreTime = 0;

    [SerializeField] List<float> highScoreList = new List<float>(5);
    public static JodoScoreKeeper Instance;
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

        
    }

    public void SetHighScoreList()
    {
        if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            Debug.Log("40L setHighScoreList jodoScoreManager hard diff");
            highscoreTime = getHighScoreTeam(LibraryMatchManager.Instance.teamNumber);
            highScoreList = PlayerPrefsExtra.GetList<float>("HighScore16xListJodo");

        }
        else
        {
            Debug.Log("40L setHighScoreList jodoScoreManager easy diff");
            highscoreTime = PlayerPrefs.GetFloat("HighScore9xJodo", 0);
            highScoreList = PlayerPrefsExtra.GetList<float>("HighScore9xListJodo");
        }
    }

    public float getHighScoreTeam(int _teamNumber)
    {
        return 0f;
    }

    public void SetCurrentScoreJodo(float timeinSec)
    {
        timeValue = timeinSec; //store this value in this variable

    }
    public void SetCurrentScore(float timeinSec)
    {
        
        //call function 
        timeValue = timeinSec;
        highscoreTime = LibraryMatchManager.Instance.GetHighScoreJodo(timeValue);
        Debug.Log("71L  setCurrentScore checking highscore Condition, timeValue is " + timeValue); 
        Debug.Log("64L jodoScoreKeeper, returned highScoreTime " + highscoreTime);

        //AddtoHighscoreList(timeValue);
        Debug.Log("Inside setCurrentScore before checking highscore Condition");

        if (highscoreTime > timeValue || highscoreTime == 0)
        {
            Debug.Log("71L  setCurrentScore checking highscore Condition, timeValue is " + timeValue); 
            //since this timeValue is small, we'll add this to top of our score list and push

            SetHighScoreJodo(timeValue);
        }

        else
        {
            //if its not smaller than highScoreTime, just add it to list then
            
            LibraryMatchManager.Instance.setHighscoreInData(timeValue);

            //SetHighScoreJodo(highscoreTime);
        }


    }

    public void SetTeamData(string cityName, string difficulty, bool isCompleted)
    {
        teamCityName = cityName;
        difficultyGame = difficulty;
        isTeamCompleted = isCompleted;
    }
    public void SetHighScoreJodo(float time)
    {
        Debug.Log("78L set high score jodoScoreKeeper " + time);

        highscoreTime = time;

        if(MatchManager.Instance.jodoType == 0)
        {
            if (MatchManager.Instance.difficultystr.Equals("Hard"))
            {
                Debug.Log("82L set high score jodoScoreKeeper hard diff " + highscoreTime);
                PlayerPrefs.SetFloat("HighScore16xJodo", highscoreTime);
            }
            else
            {
                Debug.Log("87L set high score jodoScoreKeeper easy diff " + highscoreTime);
                PlayerPrefs.SetFloat("HighScore9xJodo", highscoreTime);
            }

            AddtoHighscoreList(highscoreTime);
        }
        else if(MatchManager.Instance.jodoType == 1)
        {
            Debug.Log("108L SetHighScoreJodo  jodoScoreKeeper");
            LibraryMatchManager.Instance.setHighscoreInData(time);
            Debug.Log("110L SetHighScoreJodo  jodoScoreKeeper, called sethighScoreData, time is " + time);
        }
        
    }
    
    void AddtoHighscoreList(float value)
    {
        Debug.Log("100L add to high score list jodoScoreKeeper value " + value);
        highScoreList.Sort();
        if (highScoreList.Count == 0 || highScoreList.Count < 5)
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
            if (highScoreList[4] > value)
            {
                highScoreList[4] = value;
            }
        }

        highScoreList.Sort();
        if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            Debug.Log("126L set high score jodoScoreKeeper hard diff highScoreList " + highScoreList);
            PlayerPrefsExtra.SetList("HighScore16xListJodo", highScoreList);
        }
        else
        {
            Debug.Log("131L set high score jodoScoreKeeper easy diff highScoreList " + highScoreList);
            PlayerPrefsExtra.SetList("HighScore9xListJodo", highScoreList);
        }

    }
    public List<float> GetHighScoreList()
    {
        Debug.Log("138L getHighScoreList jodoScoreManager ");
        SetHighScoreList();
        return highScoreList;
    }

    
}
