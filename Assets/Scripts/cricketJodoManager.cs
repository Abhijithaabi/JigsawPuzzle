using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class cricketJodoManager : MonoBehaviour
{
    public static cricketJodoManager Instance;

    #region home play screen

    [SerializeField] GameObject _globalLeaderboardBtn;
    [SerializeField] Button _instructionBtn;
    [SerializeField] public GameObject instructionPopup;
    
    [SerializeField] TMP_Text progressText;

    [SerializeField] GameObject[] teamButtons;
    [SerializeField] Slider _progressBar;
    [SerializeField] Image progressFillImage;

    [SerializeField] public GameObject leftBtnHome;
    [SerializeField] public GameObject rightBtnHome;

    [SerializeField] GameObject[] leaderboardIcons;
    [SerializeField] GameObject infoBtnRoomPanel;
    
    #endregion

    #region game screen
    [SerializeField] public GameObject teamIdentifierBG;
    [SerializeField] public GameObject teamIdentifier; //team identifier card
    [SerializeField] public TMP_Text congratsTextJodo;
    [SerializeField] public GameObject playNextTeamBtn;

    [SerializeField] public GameObject leftBtnGame;
    [SerializeField] public GameObject rightBtnGame;

    [SerializeField] public GameObject leftBtnMulti;
    [SerializeField] public GameObject rightBtnMulti;

    [SerializeField] public GameObject playAgainBtn;
    [SerializeField] public GameObject playNextBtn;
    [SerializeField] public GameObject playNextBtn_leaderboard;

    #endregion

    #region leaderboards ui
    [SerializeField] public TMP_Text _LbHeadingTxtTeam;
    [SerializeField] public TMP_Text _LbInfoTxtTeam;
    [SerializeField] public GameObject easyBtn;
    [SerializeField] public GameObject hardBtn;
    [SerializeField] public Sprite[] easyBtnSprites; //0 on, 1 off
    [SerializeField] public Sprite[] hardBtnSprites; //0 on, 1 off
  

    [SerializeField] public TMP_Text[] scoreToDisplay;

    [SerializeField] public int currentTeamNumber = -1;

    [SerializeField] public TMP_Text gameBoardInfo;
    [SerializeField] TMP_Text positiveMessageText;



    // Create a HashSet to keep track of completed teams
    public HashSet<int> completedTeams = new HashSet<int>();




    #endregion


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        if(MatchManager.Instance.jodoType == 1)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {

                for (int i = 0; i < teamButtons.Length; i++)
                {

                    teamButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                    
                }
                if(MatchManager.Instance.gameMode == 0)
                {

                leftBtnHome.SetActive(true);
                rightBtnHome.SetActive(true);
                }
                else
                {
                    infoBtnRoomPanel.SetActive(true);
                    leftBtnMulti.SetActive(true);
                    rightBtnMulti.SetActive(true);
                }
                //this code is just setting tickmarks false
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
            {
                if(MatchManager.Instance.gameMode == 0)
                {
                    playNextTeamBtn.SetActive(true);
                    leftBtnGame.SetActive(true);
                    rightBtnGame.SetActive(true);
                    teamIdentifierBG.SetActive(true);
                    teamIdentifier.SetActive(true);
                    playNextBtn.SetActive(true);
                    playNextBtn_leaderboard.SetActive(true);
                    playAgainBtn.SetActive(false);
                }
                else
                {
                    teamIdentifierBG.SetActive(true);
                    teamIdentifier.SetActive(true);
                    
                }
               



            }
        }
        else if(MatchManager.Instance.jodoType == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {

                for (int i = 0; i < teamButtons.Length; i++)
                {
                    teamButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                if(MatchManager.Instance.gameMode == 0)
                {
                    leftBtnHome.SetActive(false);
                    rightBtnHome.SetActive(false);
                }
                else
                {
                   
                    infoBtnRoomPanel.SetActive(false);
                    leftBtnMulti.SetActive(false);
                    rightBtnMulti.SetActive(false);
                }
                

                //this code is just setting tickmarks false
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
            {
                if(MatchManager.Instance.gameMode == 0)
                {

                playNextTeamBtn.SetActive(false);
                leftBtnGame.SetActive(false);
                rightBtnGame.SetActive(false);
                teamIdentifierBG.SetActive(false);
                teamIdentifier.SetActive(false);
                playNextBtn.SetActive(false);
                playNextBtn_leaderboard.SetActive(false);
                playAgainBtn.SetActive(true);
                }
                else
                {
                    teamIdentifierBG.SetActive(false);
                    teamIdentifier.SetActive(false);
                }


            }
        }
        

    }
    public void setJodoUi16x()
    {
        if(MatchManager.Instance.jodoType == 1)
        {
            playAgainBtn.SetActive(false);
            playNextBtn.SetActive(true);
            playNextBtn_leaderboard.SetActive(true);
        }
        else
        {
            playAgainBtn.SetActive(true);
            playNextBtn.SetActive(false);
            playNextBtn_leaderboard.SetActive(false);
        }
    }

    public void setActiveJodoUI()
    {
        completedTeams.Clear();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            
            if (MatchManager.Instance.gameMode == 0)
            {
                
                

                for(int i = 0; i < leaderboardIcons.Length; i++)
                {
                    leaderboardIcons[i].SetActive(true);
                }

                _globalLeaderboardBtn.SetActive(true);


                updateProgressAndTicks();

                updateProgress();
                

                leftBtnHome.SetActive(true);
                rightBtnHome.SetActive(true);
            }
            else if(MatchManager.Instance.gameMode == 1)
            {
                updateProgressAndTicks();
                updateProgress();
                leftBtnMulti.SetActive(true);
                rightBtnMulti.SetActive(true);
                leftBtnHome.SetActive(true);
                rightBtnHome.SetActive(true);
                infoBtnRoomPanel.SetActive(true);
                

                for (int i = 0; i < leaderboardIcons.Length; i++)
                {
                    leaderboardIcons[i].SetActive(false);
                }

                _globalLeaderboardBtn.SetActive(false);
            }
            //this code is just setting tickmarks false
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(MatchManager.Instance.gameMode == 0)
            {
                playNextTeamBtn.SetActive(true);
                leftBtnGame.SetActive(true);
                rightBtnGame.SetActive(true);
                teamIdentifierBG.SetActive(true);
                teamIdentifier.SetActive(true);
                playNextBtn.SetActive(true);
                playNextBtn_leaderboard.SetActive(true);
                playAgainBtn.SetActive(false);
            }
            else if(MatchManager.Instance.gameMode == 1)
            {
                teamIdentifierBG.SetActive(true);
                teamIdentifier.SetActive(true);
            }
           


        }
    }

    public void updateProgressAndTicks()
    {
        for (int i = 0; i < teamButtons.Length; i++)
        {

            teamButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            foreach (TeamScoreData teamScoreData in LibraryMatchManager.Instance.teamScoreDataList)
            {
                if (teamScoreData.isCompleted == true)
                {
                    if (teamScoreData.teamNumber.ToString() == teamButtons[i].transform.GetChild(1).tag)
                    {
                        teamButtons[i].transform.GetChild(1).gameObject.SetActive(true);

                    }
                }
            }





        }

        for (int i = 0; i < teamButtons.Length; i++)
        {
            foreach (TeamScoreData teamScoreData in LibraryMatchManager.Instance.teamScoreDataList)
            {
                // Check if the team has been completed for any difficulty and has not been counted already
                if (teamScoreData.teamNumber.ToString() == teamButtons[i].transform.GetChild(1).tag
                    && teamScoreData.isCompleted
                    && !completedTeams.Contains(teamScoreData.teamNumber))
                {
                    // Add the team to the set of completed teams
                    completedTeams.Add(teamScoreData.teamNumber);
                    LibraryMatchManager.Instance.completedTeamsCount++;
                    Debug.Log("completedTeamsCount " + LibraryMatchManager.Instance.completedTeamsCount);
                    break;
                }
            }
        }
    }
    public void deactivateJodoUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            //for (int i = 0; i < teamButtons.Length; i++)
            //{
            //    teamButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            //}
            for (int i = 0; i < leaderboardIcons.Length; i++)
            {
                leaderboardIcons[i].SetActive(false);
            }

            _globalLeaderboardBtn.SetActive(false);

            if (MatchManager.Instance.gameMode == 0)
            {

            leftBtnHome.SetActive(false);
            rightBtnHome.SetActive(false);
            }
            else
            {   
                infoBtnRoomPanel.SetActive(false);
                leftBtnHome.SetActive(false);
                rightBtnHome.SetActive(false);
                leftBtnMulti.SetActive(false);
                rightBtnMulti.SetActive(false);
            }

            //this code is just setting tickmarks false
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {

            if(MatchManager.Instance.gameMode == 0)
            {
                playNextTeamBtn.SetActive(false);
                leftBtnGame.SetActive(false);
                rightBtnGame.SetActive(false);
                teamIdentifierBG.SetActive(false);
                teamIdentifier.SetActive(false);
                playNextBtn.SetActive(false);
                playNextBtn_leaderboard.SetActive(false);
                playAgainBtn.SetActive(true);
            }
            else
            {
                teamIdentifierBG.SetActive(false);
                teamIdentifier.SetActive(false);
            }
            


        }
    }


    public void setTeamLeaderboardTexts(string teamName)
    {
        //UIManager.Instance.teamLeaderboardPanel.SetActive(true);
        //_LbHeadingTxtTeam.text = teamNameCity + " Leader Board";
        currentTeamNumber = determineTeamNumber(teamName);
        showEasyLeaderboard();
        LibraryMatchManager.Instance.currentTeamName = teamName; //will use this TeamName further in libraryMatchManager
       // _LbInfoTxtTeam.text = "Your top 5 completion time for " + LibraryMatchManager.Instance.currentTeamName + " for " + LibraryMatchManager.Instance.leaderboardDifficulty +  " difficulty";
        _LbInfoTxtTeam.text = "Your top 5 completion time";
        checkTeamCityName(LibraryMatchManager.Instance.currentTeamName);
        

    }

    public int determineTeamNumber(string _teamName)
    {
        if (_teamName == "Chennai Super Kings")
        {
            return 1;
            

        }
        if (_teamName == "Delhi Capitals")
        {
            return 2;
           
        }
        if (_teamName == "Gujarat Titans")
        {
            return 3;
            
        }
        if (_teamName == "Kolkata Knight Riders")
        {
            return 4;
           
        }
        if (_teamName == "Lucknow Super Giants")
        {
            return 5;
           
        }
        if (_teamName == "Mumbai Indians")
        {
            return 6;
           
        }
        if (_teamName == "Punjab Kings")
        {
            return 7;
            
        }
        if (_teamName == "Royal Challengers Bangalore")
        {
            return 8;
           
        }
        if (_teamName == "Rajasthan Royals")
        {
            return 9;
            
        }
        if (_teamName == "Sunrisers Hyderabad")
        {
            return 10;
           
        }
        return -1;
    }
    
    public void checkTeamCityName(string teamName)
    {
        string teamCityName = "";
        if(teamName == "Delhi Capitals")
        {
            LibraryMatchManager.Instance.teamNumber = 2;
            teamCityName = "Delhi";
            
        }
        if (teamName == "Punjab Kings")
        {
            LibraryMatchManager.Instance.teamNumber = 7;
            teamCityName = "Punjab";

        }
        if (teamName == "Lucknow Super Giants")
        {
            LibraryMatchManager.Instance.teamNumber = 5;
            teamCityName = "Lucknow";

        }
        if (teamName == "Mumbai Indians")
        {
            LibraryMatchManager.Instance.teamNumber = 6;
            teamCityName = "Mumbai";

        }
        if (teamName == "Sunrisers Hyderabad")
        {
            LibraryMatchManager.Instance.teamNumber = 10;
            teamCityName = "Hyderabad";

        }
        if (teamName == "Royal Challengers Bangalore")
        {
            LibraryMatchManager.Instance.teamNumber = 8;
            teamCityName = "Bangalore";

        }
        if (teamName == "Kolkata Knight Riders")
        {
            LibraryMatchManager.Instance.teamNumber = 4;
            teamCityName = "Kolkata";

        }
        if (teamName == "Rajasthan Royals")
        {
            LibraryMatchManager.Instance.teamNumber = 9;
            teamCityName = "Rajasthan";

        }
        if (teamName == "Chennai Super Kings")
        {
            LibraryMatchManager.Instance.teamNumber = 1;
            teamCityName = "Chennai";

        }
        if (teamName == "Gujarat Titans")
        {
            LibraryMatchManager.Instance.teamNumber = 3;
            teamCityName = "Gujarat";

        }

        LibraryMatchManager.Instance.currentCityName = teamCityName; //will use this currentCityName further in libraryMatchmManager
        _LbHeadingTxtTeam.text = teamCityName + " Leaderboard";
    }
    public void closeTeamLeaderBoard()
    {
        UIManager.Instance.teamLeaderboardPanel.SetActive(false);
        MatchManager.Instance.difficultystr = "Easy";
    }

    
    public void showEasyLeaderboard()
    {
        for (int i = 0; i < scoreToDisplay.Length; i++)
        {
            scoreToDisplay[i].GetComponent<TMP_Text>().text = "--";
        }
        easyBtn.GetComponent<Image>().sprite = easyBtnSprites[0]; //on
        hardBtn.GetComponent<Image>().sprite = hardBtnSprites[1]; //off
        LibraryMatchManager.Instance.leaderboardDifficulty = "Easy";
        MatchManager.Instance.difficultystr = "Easy";

        _LbInfoTxtTeam.text = "Your top 5 completion time";
        //_LbInfoTxtTeam.text = "Your top 5 completion time for " + LibraryMatchManager.Instance.currentTeamName + " for " + LibraryMatchManager.Instance.leaderboardDifficulty + " difficulty";
        //show data stored from libraryMatchManager

        LibraryMatchManager.Instance.showScoreOnScreen(currentTeamNumber, LibraryMatchManager.Instance.leaderboardDifficulty);
        

    }

   

    public void showHardLeaderboard()
    {
        for(int i= 0; i < scoreToDisplay.Length; i++)
        {
            scoreToDisplay[i].GetComponent<TMP_Text>().text = "--";
        }
        easyBtn.GetComponent<Image>().sprite = easyBtnSprites[1]; //off
        hardBtn.GetComponent<Image>().sprite = hardBtnSprites[0]; //on
        LibraryMatchManager.Instance.leaderboardDifficulty = "Hard";
        MatchManager.Instance.difficultystr = "Hard";
        _LbInfoTxtTeam.text = "Your top 5 completion time";
        //_LbInfoTxtTeam.text = "Your top 5 completion time for " + LibraryMatchManager.Instance.currentTeamName + " for " + LibraryMatchManager.Instance.leaderboardDifficulty + " difficulty";
        //show data stored from libraryMatchManager

        LibraryMatchManager.Instance.showScoreOnScreen(currentTeamNumber, LibraryMatchManager.Instance.leaderboardDifficulty);

    }
    
    public void updateProgress()
    {
        if(LibraryMatchManager.Instance.completedTeamsCount == 0)
        {
            positiveMessageText.GetComponent<TMP_Text>().text = "Get started! You got this!";
        }
        else if (LibraryMatchManager.Instance.completedTeamsCount >= 1 && LibraryMatchManager.Instance.completedTeamsCount <= 5)
        {
            positiveMessageText.GetComponent<TMP_Text>().text = "Great job! Keep it up!";
        }
        else if (LibraryMatchManager.Instance.completedTeamsCount >= 6 && LibraryMatchManager.Instance.completedTeamsCount <= 9)
        {
            positiveMessageText.GetComponent<TMP_Text>().text = "You're almost there! Keep pushing!";
        }
        else
        {
            positiveMessageText.GetComponent<TMP_Text>().text = "Hurray! You solved all of them! You're amazing!";
        }

        Debug.Log(LibraryMatchManager.Instance.completedTeamsCount + " completedteamsCount 504L");
        progressText.GetComponent<TMP_Text>().text = LibraryMatchManager.Instance.completedTeamsCount + "/10";
        progressFillImage.fillAmount = (float)LibraryMatchManager.Instance.completedTeamsCount / 10;
       
    }

    
}
