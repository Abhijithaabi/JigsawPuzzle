using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public float timeValue = 120f;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MatchManager.Instance.gameState)
        {
            if(timeValue<0)
            {
                timeValue=0;
                
            }
            else
            {
                timeValue+= Time.deltaTime;
                // MatchManager.Instance.gameState=false;
                // MatchManager.Instance.GameOver();
            }
            DisplayTime(timeValue);
        }
        else
        {
            //ScoreKeeper.Instance.SetCurrentScore(timeValue);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay<0)
        {
            timeToDisplay=0;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay/60f);
        float seconds = Mathf.FloorToInt(timeToDisplay%60f);
        //UIManager.Instance.SetTimer(minutes,seconds);
        MatchManager.Instance.Settime(minutes,seconds,timeToDisplay);
    }
}
