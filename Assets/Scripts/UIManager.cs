using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Photon.Pun.Demo.PunBasics;
using Unity.VisualScripting;
//using UnityEditor.U2D.Animation;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject SplashScreen;
    [SerializeField] GameObject SignupPanel;
    [SerializeField] GameObject GuestUsernamePanel;
    [SerializeField] GameObject PlayPanel;
    [SerializeField] public GameObject uploadPanel;
    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject Preview;
    [SerializeField] GameObject settingsMultiPanel;
    [SerializeField] GameObject libraryPanel;
    // [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject LeaderboardPanel;
    [SerializeField] GameObject SettingsPanel;

    [SerializeField] GameObject resumeGamePanel;
    [SerializeField] GameObject SingleLeaveGamePanel;
    [SerializeField] GameObject MultiLeaveGamePanel;

    [SerializeField] GameObject CongratsPanel;
    [SerializeField] GameObject profilePicPanel;
    [SerializeField] GameObject headerPanel;
    [SerializeField] public Image congratsImage;
    [SerializeField] GameObject MultiplayerCongratspanel;
    [SerializeField] GameObject WelcomePanel;
    [SerializeField] public GameObject teamLeaderboardPanel;
    [SerializeField] public GameObject globalLeaderboardpanel;
    [SerializeField] public GameObject jodoInstructionPanel;
    
    public GameObject leaderboardLabel;
    public GameObject congratsRibbon;
    public GameObject congratsStars;
    public GameObject GameOverTxt;
    public GameObject loseRibbon;
    public TMP_Text winnerLabel;

    [SerializeField] GameObject errorPanel;

    [SerializeField] TMP_Text diffInfo;
    [SerializeField] Image puzzleImage;

    [SerializeField] TMP_Text roomDiffHeading;
    public TMP_Text roomDiffInfo;
    
    [SerializeField] Sprite[] EasyBtnSprites;
    
    [SerializeField] Button Easy;

    [SerializeField] Sprite[] HardBtnSprites;
   
    [SerializeField] Button Hard;
    [SerializeField] Animator transition;
    
    
    [SerializeField] TMP_Text EndTime;
    [SerializeField] TMP_Text HighScoreText;

    [SerializeField] public Sprite[] EyeButtonSprites;
    [SerializeField] public Sprite[] voiceButtonSprites;
    [SerializeField] public Sprite[] musicButtonSprites;

    [SerializeField] public Image eyeButton;
    
    

    [SerializeField] public GameObject pauseGameBtn;
    
    [SerializeField] public GameObject playerCountGO;
    public TMP_Text PlayerCountText;

    [SerializeField] TMP_Text[] bestScoresList;

    [SerializeField] TMP_Text BestScore1;
    [SerializeField] TMP_Text BestScore2;
    [SerializeField] TMP_Text BestScore3;
    [SerializeField] TMP_Text BestScore4;
    [SerializeField] TMP_Text BestScore5;
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_Text userNameTxt;
    [SerializeField] GameObject multiplayerBtn;
    public Button playBtn;
    public Sprite proceedImage;

    public TMP_InputField usernameInputField;
    [SerializeField] GameObject[] profilePicArray;
    [SerializeField] GameObject profilePicBtn;
    [SerializeField] Image profilePic;
    [SerializeField] GameObject ProfilePicSettings;
    [SerializeField] TMP_InputField userNameSettings;
    public Sprite defaultleaderboardLabel;
    public Sprite myleaderboardLabel;
    [SerializeField] GameObject profileSelected;
    List<GameObject> profileSelectedList = new List<GameObject>();
    [SerializeField] Sprite[] puzzleBoardSprites;
    [SerializeField] GameObject puzzleBoard;
    [SerializeField] Sprite[] bottomDeckSprites;
    [SerializeField] GameObject bottomDeck;
    [SerializeField] Image boardThemeBtn;
    [SerializeField] public Sprite[] themeButtonSprites;
    [SerializeField] public GameObject voiceBtn;
    [SerializeField] public GameObject musicBtn;



    //[SerializeField] cricketJodoManager cricketJodoManager;

    private Image profilePicSelected;

    public TMP_Text timer;
    bool isPreview = false;
    bool isprofilePicOpenedfromSettings = false;
    bool isProfilePanelClosed = false;
    Sprite puzzle;
    public static UIManager Instance;
    float[] min;
    float[] sec;
    float backgroundTime;
    bool isOpeningLibFirstTime;

    [SerializeField] GameObject LibraryMatchManagerGO;
    [SerializeField] public Image difficultyImage;
    
    void Awake()
    {
        Instance = this;
    }

    void Start() { 
        isOpeningLibFirstTime = true;
    //{   if(MatchManager.Instance.isMuted == false)
    //    {

    //    SoundManager.Instance.PlayMusic();
    //    }
    
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SoundManager.Instance.stopMusicForHome();
            ClosePanels();
            if(MatchManager.Instance.GetUserName() == null || MatchManager.Instance.GetUserName() == ""|| MatchManager.Instance.profilePicture == null)
            {
                Debug.Log("SplashScreenActive");
                Debug.Log("Inside Ui Mnager profilePicture null" + MatchManager.Instance.GetUserName() == "");
                StartCoroutine("SplashScreenActive");
                SignupPanel.SetActive(true);
            }
            else
            {
                SetProfilePicAndName();
                Debug.Log(MatchManager.Instance.GetUserName());
                PlayPanel.SetActive(true);
                
            }
            
            
            
            // Easy.GetComponent<Image>().sprite = EasyBtnSprites[0];
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                cricketJodoManager.Instance.setJodoUi16x();
            }

            SoundManager.Instance.checkForGameMusic();
            ClosePanels();
            puzzle = MatchManager.Instance.uploadedImage;
           // puzzleImage.sprite = puzzle;
            if(MatchManager.Instance.gameMode == 1)
            {
                pauseGameBtn.SetActive(false);
                playerCountGO.SetActive(true);
                SetPlayerCount();
                if (MatchManager.Instance.jodoType == 1) //cricketJodo
                {

                    LibraryMatchManager.Instance.changeTeamIdentifer_inGame(LibraryMatchManager.Instance.currentCityName);

                }
            }
            else if(MatchManager.Instance.gameMode == 0)
            {


                pauseGameBtn.SetActive(true);
                playerCountGO.SetActive(false);

                if(MatchManager.Instance.jodoType == 1) //cricketJodo
                {

                    LibraryMatchManager.Instance.changeTeamIdentifer_inGame(LibraryMatchManager.Instance.currentCityName);
                    
                }
            }
            ApplyChangeToAllProfileBars();
            //TopHeadingActive();
        }
        //transition.SetTrigger("Start");


        

        
        
    }

    private IEnumerator SplashScreenActive()
    {
        float waitTime = Random.Range(3f, 6f);
        SplashScreen.SetActive(true);
        yield return new WaitForSeconds(waitTime);

        SplashScreen.SetActive(false);
    }

    public void loadNextTeam()
    {
        LibraryMatchManager.Instance.scoreList = null;
        LibraryMatchManager.Instance.LoadNextTeam();
    }
    public void SetPlayerCount()
    {
        playerCountGO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = MatchManager.Instance.GetGamePlayerCount().ToString();
    }

    public void ClosePanels()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SplashScreen.SetActive(false);
            SignupPanel.SetActive(false);
            GuestUsernamePanel.SetActive(false);
            PlayPanel.SetActive(false);
            uploadPanel.SetActive(false);
            difficultyPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            profilePicPanel.SetActive(false);
            WelcomePanel.SetActive(false);
            libraryPanel.SetActive(false);
            teamLeaderboardPanel.SetActive(false);
            jodoInstructionPanel.SetActive(false);
            //LibraryMatchManagerGO.GetComponent<LibraryMatchManager>().enabled = true;

        }
        else if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            // gameOverPanel.SetActive(false);
            CongratsPanel.SetActive(false);
            LeaderboardPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            MultiplayerCongratspanel.SetActive(false);
            
        }
        
    }


    public void Play()
    {   
        MatchManager.Instance.gameMode = 0;

        ClosePanels();
        Launcher.instance.CloseMenus();
        
        uploadPanel.SetActive(true);
    }

    public void toggleVoiceSettings()
    {
        if (SoundManager.Instance.isVoiceMute == false)
        {
            voiceBtn.GetComponent<Image>().sprite = UIManager.Instance.voiceButtonSprites[1];
            SoundManager.Instance.isVoiceMute = true;
        }
        else
        {
            voiceBtn.GetComponent<Image>().sprite = UIManager.Instance.voiceButtonSprites[0];
            SoundManager.Instance.isVoiceMute = false;
        }
    }

    public void toggleMusicSettings()
    {
        if (SoundManager.Instance.isMusicMute == false)
        {
            SoundManager.Instance.musicAudiosrc.Pause();
            musicBtn.GetComponent<Image>().sprite = UIManager.Instance.musicButtonSprites[1];
            SoundManager.Instance.isMusicMute = true;
        }
        else
        {
            SoundManager.Instance.musicAudiosrc.UnPause();
            musicBtn.GetComponent<Image>().sprite = UIManager.Instance.musicButtonSprites[0];
            SoundManager.Instance.isMusicMute = false;
        }



    }


    public void openLibrary()
    {
        if(MatchManager.Instance.gameMode == 0)
        {

        LibraryMatchManager.Instance.getTeamListFromDisk(); //this fetches the teamScoreDataList in which we'll be scoring scores
        }
        else
        {
            LibraryMatchManager.Instance.getTeamListFromDisk(); //this fetches the teamScoreDataList in which we'll be scoring scores
        }
        
        cricketJodoManager.Instance.setActiveJodoUI();
        //LibraryMatchManagerGO.GetComponent<LibraryMatchManager>().enabled = true;
        MatchManager.Instance.jodoType = 1; //now its cricketJodo
        ClosePanels();
        if(isOpeningLibFirstTime)
        {
            StartCoroutine("SplashScreenActive");
            isOpeningLibFirstTime = false;
        }
        
        
        libraryPanel.SetActive(true);
        //jodoInstructionPanel.SetActive(true);
        uploadPanel.SetActive(true);

        //cricketJodoManager.Instance.updateProgress(); //this will update the progress everytime we click on play from library button
    }

    public void closeLibrary()
    {
        if(MatchManager.Instance.gameMode == 0)
        {

        LibraryMatchManager.Instance.resetLibraryMatchManager();
        libraryPanel.SetActive(false);
         uploadPanel.SetActive(true);
        MatchManager.Instance.jodoType = 0; //back to normalJodo 
        LibraryMatchManager.Instance.currentCityName = "";
        LibraryMatchManager.Instance.currentTeamName = "";
        }
        else
        {
            LibraryMatchManager.Instance.resetLibraryMatchManager();
            libraryPanel.SetActive(false);
            
            Launcher.instance.backToMultiplayerPanel();
            MatchManager.Instance.jodoType = 0; //back to normalJodo 
            LibraryMatchManager.Instance.currentCityName = "";
            LibraryMatchManager.Instance.currentTeamName = "";
        }
    }

    public void logOutRemovedata()
    {
        LibraryMatchManager.Instance.emptyTeamList();

        //string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/profilePicData.json");
        //json = "";
        System.IO.File.Delete(Application.persistentDataPath + "/profilePicData.json");
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/profilePicData.json", json);
        MatchManager.Instance.SetUserName(null);
        MatchManager.Instance.ResetMatchManager();
        ClosePanels();
        Launcher.instance.CloseMenus();
        PlayerPrefs.DeleteKey("HighScore9xList");
        PlayerPrefs.DeleteKey("HighScore16xList");
        SignupPanel.SetActive(true);
        //PlayerPrefsExtra.SetList("HighScore9xList", null);
        //PlayerPrefsExtra.SetList("HighScore9xList", highScoreList);
        //PlayerPrefsExtra.SetList("HighScore9xList", highScoreList);

    }


    public void QuitGame(){
        Debug.Log("Quit game");
        if(MatchManager.Instance.gameState && MatchManager.Instance.gameMode==1)
        {
            MatchManager.Instance.LeaveGameSend();
            
        }
        Application.Quit();
    }
    public void ChooseDifficulty()
    {
        Launcher.instance.CloseMenus();
        playBtn.onClick.AddListener(PlayBtnOnClick);
        libraryPanel.SetActive(false);
        //ClosePanels();
        Debug.Log("Inside Choose Difficulty");
        difficultyPanel.SetActive(true);
        Easy.GetComponent<Image>().sprite = EasyBtnSprites[0];
        Hard.GetComponent<Image>().sprite = HardBtnSprites[1];
        if(MatchManager.Instance.gameMode == 1)
        {
            playBtn.image.sprite = proceedImage;
        }
        

        
        
    }
    void PlayBtnOnClick()
    {
        if(MatchManager.Instance.gameMode == 1)
        {
            //multiplayer
            if(MatchManager.Instance.jodoType == 1)
            {
                LibraryMatchManager.Instance.playerImageIndex = 0;
                LibraryMatchManager.Instance.index = 0;
                loadMPimages_cj(); //this will load up the first image for ingame pictures arrays.
                Launcher.instance.CreateRoom();

            }
            else
            {
                Launcher.instance.CreateRoom();

            }

        }
        else if (MatchManager.Instance.gameMode == 0)
        {
            if(MatchManager.Instance.jodoType == 1)
            {
                //now we set the new images 
                for(int i = 0; i < LibraryMatchManager.Instance.currentTeamImages.Count; i++)
                {
                    if( i != LibraryMatchManager.Instance.playerImageIndex)
                    {
                        Debug.Log(LibraryMatchManager.Instance.inGamePlayerSprites);
                        Debug.Log(LibraryMatchManager.Instance.currentTeamImages);
                        LibraryMatchManager.Instance.inGamePlayerSprites.Add(LibraryMatchManager.Instance.currentTeamImages[i]);
                        

                    }
                }
                Debug.Log("inGamePlayerSprites count " +  LibraryMatchManager.Instance.inGamePlayerSprites.Count);

                LibraryMatchManager.Instance.LoadTextureImage(LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.playerImageIndex]);
                Debug.Log("frst image index " + LibraryMatchManager.Instance.playerImageIndex);
            }
            StartGame(); 
            

        }
    }
    public void loadMPimages_cj()
    {

    }

    public void TopHeadingActive()
    {

        if (MatchManager.Instance.gameMode == 0)
        {
            pauseGameBtn.SetActive(true);
            playerCountGO.SetActive(false);
        }
        else if (MatchManager.Instance.gameMode == 1)
        {
            pauseGameBtn.SetActive(false);
            playerCountGO.SetActive(true);
        }
    }

    public void replayGame()
    {
        if(MatchManager.Instance.jodoType == 0)
        {
            StartGame();
        }
        else
        {
            LibraryMatchManager.Instance.ReplayTeamSet();
            //LibraryMatchManager.Instance.scoreList.Clear();
            MatchManager.Instance.teamScoreList.Clear();
        }
    }
    public void StartGame()

    {


        if (MatchManager.Instance.gameMode == 0)
        {
            Debug.Log("gamemode 0 in start game");
            LoadSinglePlayer();
        }
        else if(MatchManager.Instance.gameMode == 1)
        {
            Debug.Log("in start game");
            LoadMultiplayer();
            

            
        }

    }

    private static void LoadMultiplayer()
    {
        Time.timeScale = 1;
        if (MatchManager.Instance.difficultystr.Equals("Easy"))
        {
            Launcher.instance.StartMultiplayerGame(2);
            //MatchManager.Instance.updateGamePlayerCount();
            
        }
        else if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            Launcher.instance.StartMultiplayerGame(1);
            //MatchManager.Instance.updateGamePlayerCount();
            

        }

        
        MatchManager.Instance.placedPieces = 0;
        MatchManager.Instance.gameState = true;
        Debug.Log("MatchManager.Instance.gameState = true;");
        MatchManager.Instance.GameStateSend(true);
    }

    private static void LoadSinglePlayer()
    {
        Time.timeScale = 1;
        if (MatchManager.Instance.difficultystr.Equals("Easy"))
        {
            SceneManager.LoadScene(2);
            
            
        }
        else if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            SceneManager.LoadScene(1);
            
        }
        

        MatchManager.Instance.placedPieces = 0;
        MatchManager.Instance.gameState = true;
    }

    public void setUploadPanelActive()
    {
       
        uploadPanel.SetActive(true);
    }
    public void setDifficultyImage(string teamNameCity)
    {
        LibraryMatchManager.Instance.index = 0;
        LibraryMatchManager.Instance.setDifficultyImage(teamNameCity);
    }

    public void BacktoUploadImage()
    {
        if(MatchManager.Instance.jodoType == 0)
        {

        diffInfo.text = "Game will be a <color=#19B9FF>9 piece jigsaw</color>";
        if(MatchManager.Instance.gameMode == 0)
        {
        ClosePanels();
        uploadPanel.SetActive(true);

        }
        else
        {
            ClosePanels();
                
                LibraryMatchManager.Instance.inGamePlayerSprites.Clear();
            Launcher.instance.backToMultiplayerPanel();
            uploadPanel.SetActive(true);
            PlayPanel.SetActive(true);
        }
        }
        else
        {
            ClosePanels();
            difficultyPanel.SetActive(false);
            libraryPanel.SetActive(true);
            LibraryMatchManager.Instance.index = 0;
            LibraryMatchManager.Instance.playerImageIndex = 0;
            LibraryMatchManager.Instance.inGamePlayerSpriteIndex = -1;
            LibraryMatchManager.Instance.currentPlayerImageIndex = 0;
            LibraryMatchManager.Instance.currentCityName = "";
            LibraryMatchManager.Instance.currentTeamName = "";

        }
    }

    public void eyeSpriteChange(){
        if(eyeButton.sprite == EyeButtonSprites[0]){
            eyeButton.sprite = EyeButtonSprites[1];
            return;
        }
        eyeButton.sprite = EyeButtonSprites[0];
    }

    public void boardThemeSpriteChange()
    {
        if (boardThemeBtn.sprite == themeButtonSprites[0])
        {
            puzzleBoard.GetComponent<SpriteRenderer>().sprite = puzzleBoardSprites[1];
            bottomDeck.GetComponent<SpriteRenderer>().sprite = bottomDeckSprites[1];

            boardThemeBtn.sprite = themeButtonSprites[1];
            return;
            
        }
        puzzleBoard.GetComponent<SpriteRenderer>().sprite = puzzleBoardSprites[0];
        bottomDeck.GetComponent<SpriteRenderer>().sprite = bottomDeckSprites[0];
        boardThemeBtn.sprite = themeButtonSprites[0];
        
    }
    
    public void voiceSpriteChange(Image voiceSpriteBtn)
    {   
        if (voiceSpriteBtn.sprite == voiceButtonSprites[0])
        {
            voiceSpriteBtn.sprite = voiceButtonSprites[1];
            //return;
        }
        else
        {
        voiceSpriteBtn.sprite = voiceButtonSprites[0];

        }
    }
    public void musicSpriteChange(Image musicSpriteBtn)
    {
        if (musicSpriteBtn.sprite == musicButtonSprites[0])
        {
            musicSpriteBtn.sprite = musicButtonSprites[1];
            
        }
        else
        {

        musicSpriteBtn.sprite = musicButtonSprites[0];
        }
    }

    public void SetPreview()
    {
        if(!isPreview)
        {
            isPreview = true;
        }
        else
        {
            isPreview = false;
        }
        Preview.SetActive(isPreview);
    }
    public void SetDifficulty(string difficulty)
    {
        if(difficulty == "Easy")
        {
           

            diffInfo.text = "Game will be a <color=#19B9FF>9 piece jigsaw</color>";
            
            
            roomDiffHeading.text = "Easy level";
            //roomDiffInfo.text = "Puzzle will be divided into 9 pieces";

            Hard.GetComponent<Image>().sprite = HardBtnSprites[1];
            Easy.GetComponent<Image>().sprite = EasyBtnSprites[0];

            //Easy.onClick.AddListener(EasySelected);
            MatchManager.Instance.difficultystr = "Easy";
            LibraryMatchManager.Instance.leaderboardDifficulty = "Easy";
            
            
           
        }
        else if(difficulty == "Hard")

        {
            

            diffInfo.text = "Game will be a <color=#EB6C11>16 piece jigsaw</color>";
            //diffInfo.GetComponent<TextMeshProUGUI>().faceColor = HexToColor("EB6C11");
           
            roomDiffHeading.text = "Hard level";
            //roomDiffInfo.text = "Puzzle will be divided into 16 pieces";

            Easy.GetComponent<Image>().sprite = EasyBtnSprites[1];
            Hard.GetComponent<Image>().sprite = HardBtnSprites[0];
            //Hard.onClick.AddListener(HardSelected);
            MatchManager.Instance.difficultystr = "Hard";
            LibraryMatchManager.Instance.leaderboardDifficulty = "Hard";
        }

        if(MatchManager.Instance.jodoType == 0)
        {

        ScoreKeeper.Instance.SetHighScoreList();
        }
        else
        {
            JodoScoreKeeper.Instance.SetHighScoreList();
        }
        // if(MatchManager.Instance.gameMode == 1)
        // {
        //     MatchManager.Instance.GameDifficultySend(difficulty);
        // }
        
    }

    private Color HexToColor(string hex)
    {
        // Remove the '#' from the beginning of the string
        hex = hex.Replace("#", "");

        // Convert the hex string to a Color object
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, a);
    }
    public void EasySelected(){
        Easy.GetComponent<Image>().sprite = EasyBtnSprites[0];
        if(Hard.GetComponent<Image>().sprite = HardBtnSprites[0]){
            Hard.GetComponent<Image>().sprite = HardBtnSprites[1];
        }
        
    }
    public void HardSelected(){
        if(Easy.GetComponent<Image>().sprite == EasyBtnSprites[0]){
            Easy.GetComponent<Image>().sprite = EasyBtnSprites[1];
        }
        Hard.GetComponent<Image>().sprite = HardBtnSprites[0];
        
    }


    public void SetTimer(float minutes,float seconds)
    {
        timer.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
    public void SetEndTime(float minutes,float seconds)
    {
        Debug.Log("Set end time 623 UI manager");
        Debug.Log("624L minutes " + minutes + " UI manager" );
        Debug.Log("625L secondss " + seconds + " UI manager");
        
        float _highScore = 0f;
        //EndTime.text = string.Format("{0:00}:{1:00}",minutes,seconds);
        if(minutes == 0)
        {
            if (seconds <= 9)
            {
                seconds = 00 + seconds;
            EndTime.text = string.Format("0{0}s", seconds);
            }
            else
            {
                EndTime.text = string.Format("{0}s", seconds);
            }
        }
        else
        {
            if (seconds <= 9)
            {
                seconds = 00 + seconds;
            EndTime.text = string.Format("{0}m 0{1}s", minutes, seconds);

            }

            else
            {
                EndTime.text = string.Format("{0}m {1}s", minutes, seconds);
            }

        }
        Debug.Log("629L end time text " + EndTime.text);

        if(MatchManager.Instance.jodoType == 0)
        {

         _highScore = ScoreKeeper.Instance.GetHighScore();
            Debug.Log("634L scorekeeper getHighScore " + _highScore + " UI manager");
        }
        else if (MatchManager.Instance.jodoType == 1)
        {
            //get the highscore from getHighScoreInData from LibraryMatchManager
            Debug.Log("653L UIManager SetEndTime ");
            _highScore = LibraryMatchManager.Instance.GetHighScoreJodo(MatchManager.Instance.jodoTeamScoreSum);
            Debug.Log("655L UIManager SetEndTime, highScoreTime " + _highScore) ;



            // highScore = JodoScoreKeeper.Instance.GetHighScore();
            //Debug.Log("640L scorekeeper getHighScore " + highScore+ " UI manager");
        }
        DisplayHighScoreTime(_highScore);
    }
    // public void GameOverScreen()
    // {
    //     ClosePanels();
    //     Debug.Log("Hi");
    //     gameOverPanel.SetActive(true);
    // }
    

    
    public void CongratsScreen()
    {

        Debug.Log("656L congratsScreen UI manager ");
        ClosePanels();
        Debug.Log("hi in congratsPanel");
        if(MatchManager.Instance.jodoType == 0)
        {

        congratsImage.GetComponent<Image>().sprite = MatchManager.Instance.uploadedImage; 
        }
        else if(MatchManager.Instance.jodoType == 1)
        {
           
            LibraryMatchManager.Instance.congratsImageCounter = 0;
            congratsImage.GetComponent<Image>().sprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.congratsImageCounter];
        }
        
        
        
        CongratsPanel.SetActive(true);
        SoundManager.Instance.stopMusic();
        SoundManager.Instance.congratsSFX();

    }

    public void leftBtnOnClick()
    {
       
        LibraryMatchManager.Instance.previousImage();
       
    }

    public void gameLeftBtnClick()
    {
        LibraryMatchManager.Instance.gamePreviousImage();
    }
    public void rightBtnOnClick()
    {
       
            LibraryMatchManager.Instance.nextImage();
       
           
        
    }

    public void gameRightBtnonClick()
    {
        LibraryMatchManager.Instance.gameNextImage();
    }

    public void multiRightClick()
    {
        LibraryMatchManager.Instance.multiNextImage();
    }

    public void multiLeftClick()
    {
        LibraryMatchManager.Instance.multiPrevImage();
    }
    public void MultiplayerCongratsScreen()
    {
        ClosePanels();
        SoundManager.Instance.stopMusic();
        SoundManager.Instance.congratsSFX();
        MultiplayerCongratspanel.SetActive(true);

        
    }
    public void PlayTransitionEffect()
    {
        Debug.Log("Just Checking");
        //transition.SetBool("transition",true);
        //StartCoroutine(TransitionDelay(transition.playbackTime));
    }
    IEnumerator TransitionDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        transition.SetBool("transition",false);
    }
    // public void DisplayHighScoreTime(float timeToDisplay)
    //{
    //    if (MatchManager.Instance.jodoType == 0)
    //    {
    //        if (timeToDisplay < 0)
    //        {

    //            timeToDisplay = 0;
    //        }
    //    }

    //    else if(MatchManager.Instance.jodoType == 1)
    //    {
    //        if (timeToDisplay <= 0)
    //        {

    //            timeToDisplay = MatchManager.Instance.jodoTeamScoreSum;
    //        }
    //    }
    //    float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
    //    float seconds = Mathf.FloorToInt(timeToDisplay % 60f);
    //    Debug.Log("736 displayHighScoreTime UIMng, minutes" + minutes + " and seconds " + seconds);
    //    HighScoreText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    //}
    public void DisplayHighScoreTime(float timeToDisplay)
    {
        if (MatchManager.Instance.jodoType == 0)
        {
            if (timeToDisplay < 0)
            {

                timeToDisplay = 0;
            }
        }

        else if (MatchManager.Instance.jodoType == 1)
        {
            if (timeToDisplay <= 0 || timeToDisplay > MatchManager.Instance.jodoTeamScoreSum)
            {

                timeToDisplay = MatchManager.Instance.jodoTeamScoreSum;
            }
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        Debug.Log("736 displayHighScoreTime UIMng, minutes" + minutes + " and seconds " + seconds);
        if(minutes == 0)
        {
            if(seconds <= 9)
            {
            HighScoreText.text = string.Format("0{0}s", seconds);

            }
            else
            {
                HighScoreText.text = string.Format("{0}s", seconds);
            }
        }
        else
        {
            if(seconds <= 9)
            {
            HighScoreText.text = string.Format("{0}m 0{1}s", minutes, seconds);

            }
            else
            {
                HighScoreText.text = string.Format("{0}m {1}s", minutes, seconds);
            }

        }

    }
    public void OpenLeaderboard()
    {
        //int[] min;
        SoundManager.Instance.stopMusic();
        ClosePanels();
        List<float> highScores = new List<float>();
        LeaderboardPanel.SetActive(true);
        if(MatchManager.Instance.jodoType == 0)
        {

        highScores = ScoreKeeper.Instance.GetHighScoreList();
            
            for(int i = 0; i < highScores.Count; i++)
            {
                int _minutes = Mathf.FloorToInt(highScores[i] / 60f);
                int _seconds = Mathf.FloorToInt(highScores[i] % 60f);
                if(_minutes == 0)
                {
                    if(_seconds <= 9)
                    {
                        _seconds = 00 + _seconds;
                    bestScoresList[i].GetComponent<TMP_Text>().text = string.Format("0{0}s", _seconds);
                    }
                    else
                    {
                        bestScoresList[i].GetComponent<TMP_Text>().text = string.Format("{0}s", _seconds);
                    }
                }
                else
                {
                    if (_seconds <= 9)
                    {
                        _seconds = 00 + _seconds;
                        bestScoresList[i].GetComponent<TMP_Text>().text = string.Format("{0}m 0{1}s", _minutes, _seconds);
                    }
                    else
                    {
                        bestScoresList[i].GetComponent<TMP_Text>().text = string.Format("{0}m {1}s", _minutes, _seconds);
                    }
                        
                }
            }

            //BestScore1.text = string.Format("{0}m {1}s", Mathf.FloorToInt(highScores[0] / 60f), Mathf.FloorToInt(highScores[0] % 60f));
            //BestScore2.text = string.Format("{0}m {1}s", Mathf.FloorToInt(highScores[1] / 60f), Mathf.FloorToInt(highScores[1] % 60f));
            //BestScore3.text = string.Format("{0}m {1}s", Mathf.FloorToInt(highScores[2] / 60f), Mathf.FloorToInt(highScores[2] % 60f));
            //BestScore4.text = string.Format("{0}m {1}s", Mathf.FloorToInt(highScores[3] / 60f), Mathf.FloorToInt(highScores[3] % 60f));
            //BestScore5.text = string.Format("{0}m {1}s", Mathf.FloorToInt(highScores[4] / 60f), Mathf.FloorToInt(highScores[4] % 60f));
        }
        else if(MatchManager.Instance.jodoType == 1)
        {
            //just call function from libraryMatchmanager giving teamNumber and game diff
            LibraryMatchManager.Instance.showScoreOnScreen(LibraryMatchManager.Instance.teamNumber, MatchManager.Instance.difficultystr);
        }
        //for(int i =0;i<highScores.Count;i++)
        // {
        //     Debug.Log("highScores:"+highScores[i]/60);
        //     min[i] = Mathf.FloorToInt(highScores[i]/60f);
        //     sec[i] = Mathf.FloorToInt(highScores[i]%60f);
            
        // }
        
    }
    public void OpenSettings()
    {
        if(MatchManager.Instance.gameMode == 0)
        {
            if (!SettingsPanel.activeInHierarchy)
            {
                //ClosePanels();
                SettingsPanel.SetActive(true);
                if (MatchManager.Instance.gameMode != 1)
                    Time.timeScale = 0;
            }
        }
        else if(MatchManager.Instance.gameMode == 1)
        {
            if (!settingsMultiPanel.activeInHierarchy)
            {
                //ClosePanels();
                settingsMultiPanel.SetActive(true);
                
            }
        }
        
        
    }
    public void CloseSettings()
    {
        if(MatchManager.Instance.gameMode == 0)
        {
        Time.timeScale=1;
        SettingsPanel.SetActive(false);

        }
        else if(MatchManager.Instance.gameMode == 1)
        {
            settingsMultiPanel.SetActive(false);
        }
    }

    public void openResumeGamePanel()
    {
        if (MatchManager.Instance.gameMode == 0)
        {
            if (!resumeGamePanel.activeInHierarchy)
            {
                //ClosePanels();
                resumeGamePanel.SetActive(true);
                if (MatchManager.Instance.gameMode != 1)
                    Time.timeScale = 0;
            }
        }
        
    }
    public void CloseResumePanel()
    {
        if (MatchManager.Instance.gameMode == 0)
        {
            Time.timeScale = 1;
            resumeGamePanel.SetActive(false);

        }
        //else if (MatchManager.Instance.gameMode == 1)
        //{
        //    settingsMultiPanel.SetActive(false);
        //}
    }
    public void openLeaveGamePanel()
    {
        if (MatchManager.Instance.gameMode == 0)
        {
            if (!SingleLeaveGamePanel.activeInHierarchy)
            {
                //ClosePanels();
                SingleLeaveGamePanel.SetActive(true);
                if (MatchManager.Instance.gameMode != 1)
                    Time.timeScale = 0;
            }
        }
        else if (MatchManager.Instance.gameMode == 1)
        {
            if (!MultiLeaveGamePanel.activeInHierarchy)
            {
                //ClosePanels();
                MultiLeaveGamePanel.SetActive(true);

            }
        }

    }
    public void CloseLeaveGamePanel()
    {
        if (MatchManager.Instance.gameMode == 0)
        {
            Time.timeScale = 1;
            SingleLeaveGamePanel.SetActive(false);

        }
        else if (MatchManager.Instance.gameMode == 1)
        {
            MultiLeaveGamePanel.SetActive(false);
        }
    }
    public void OpenHomeScreen()
    {   
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        MatchManager.Instance.gameState = false;
        MatchManager.Instance.ResetMatchManager();
        
        
    }
    public void SetUserNameAndProfilePic()
    {
        Debug.Log("here");
        // MatchManager.Instance.SetUserName(userNameInput.text);
        // userNameTxt.text = MatchManager.Instance.GetUserName();
        MatchManager.Instance.SetProfileDetails(userNameInput.text,profilePicBtn.GetComponent<Image>().sprite.texture);
        SetProfilePicAndName();
        //Launcher.instance.ResetInputFields(usernameInputField);
    }  

    //public void GetRoomCode()
    //{
    //    Debug.Log("room text copied");
    //    GUIUtility.systemCopyBuffer = Launcher.instance.copiedRoomText.text;
    //}

    public void closeHomeSettings()
    {
        SettingsPanel.SetActive(false);
    }
    public void GoToHomeMultiPlayer()
    {
        if(MatchManager.Instance.gameMode == 1)
        {
            if(MatchManager.Instance.jodoType == 1)
            {
                LibraryMatchManager.Instance.resetMultiJodo();
            }
        }
        Time.timeScale = 1;
        //SceneManager.LoadScene(0);
        MatchManager.Instance.isComingFromInGame = true;
        MatchManager.Instance.ReturnHome();
    }
    public void OnButtonClick(PointerEventData eventData)
    {
        Debug.Log("test");
        GameObject buttonClicked = eventData.pointerCurrentRaycast.gameObject;
        for(int i=0;i<profilePicArray.Length;i++)
        {
            if(buttonClicked == profilePicArray[i])
            {
                Debug.Log(profilePicArray[i].name+" Clicked");
                
            }
        }
    }
    public void UploadProfilePic(Image image)
    {
        
        if(image == profilePicBtn.GetComponent<Image>().sprite)
        {
            Instantiate(profileSelected, image.transform);
            

        }



        Debug.Log("test");
        
        Debug.Log("profileSelectedList" + profileSelectedList.Count);
        foreach(GameObject border in profileSelectedList)
        {
            if(border != null)
            {
                Destroy(border.gameObject);
            }
        }
        profileSelectedList.Clear();
        GameObject clickedGameObject = EventSystem.current.currentSelectedGameObject;
        Debug.Log("clickedGameObject" + clickedGameObject.name);
        GameObject profileSelectedCheck = Instantiate(profileSelected,clickedGameObject.transform);
        profileSelectedList.Add(profileSelectedCheck);
        profilePicSelected = image;
        profilePicBtn.GetComponent<Image>().sprite = image.sprite;
        ProfilePicSettings.GetComponent<Image>().sprite= image.sprite;
        profilePicPanel.SetActive(false);
        if (isProfilePanelClosed == true)
        {
            //from settings so
            SettingsPanel.SetActive(true);

        }
        else
        {

        }
        
       
    }
    public void resetCheckBox(Image image)
    {
        Debug.Log("test");

        Debug.Log("profileSelectedList" + profileSelectedList.Count);
        foreach (GameObject border in profileSelectedList)
        {
            if (border != null)
            {
                Destroy(border.gameObject);
            }
        }
        profileSelectedList.Clear();
        GameObject clickedGameObject = EventSystem.current.currentSelectedGameObject;
        //Debug.Log("clickedGameObject" + clickedGameObject.name);
        GameObject profileSelectedCheck = Instantiate(profileSelected, image.transform);
        profileSelectedList.Add(profileSelectedCheck);
        profilePicSelected = image;
        profilePicBtn.GetComponent<Image>().sprite = image.sprite;
        ProfilePicSettings.GetComponent<Image>().sprite = image.sprite;


    }
    public void UpdateProfileDetails()
    {
        //Implement method for changing profile pic and usersame from homescreen settings.
        MatchManager.Instance.SetProfileDetails(userNameSettings.text,ProfilePicSettings.GetComponent<Image>().sprite.texture);
        SetProfilePicAndName();
    }
    void SetProfilePicAndName()
    {
        //profilePicSelected.sprite = MatchManager.Instance.GetProfilePic();
        profilePic.sprite = MatchManager.Instance.GetProfilePic();
        userNameTxt.text = MatchManager.Instance.GetUserName();
        ProfilePicSettings.GetComponent<Image>().sprite = MatchManager.Instance.GetProfilePic();
        userNameSettings.text = MatchManager.Instance.GetUserName();
        ApplyChangeToAllProfileBars();
    }

    public void retainLastUsername()
    {
        userNameSettings.text = MatchManager.Instance.GetUserName();
    }

    public void cancelProfileChanges()
    {
        userNameSettings.text = MatchManager.Instance.GetUserName();
        ProfilePicSettings.GetComponent<Image>().sprite = MatchManager.Instance.GetProfilePic();

    }
    public void OpenProfilePicPanel(bool isFromSettings)
    {
        isProfilePanelClosed = isFromSettings;
        profilePicPanel.SetActive(true);
        if(isFromSettings)
        {
            SettingsPanel.SetActive(false);
            isprofilePicOpenedfromSettings = true;
        }

    }
    public void CloseProfilePicPanel()
    {
        profilePicPanel.SetActive(false);
        if(isprofilePicOpenedfromSettings)
        {
            SettingsPanel.SetActive(true);
            isprofilePicOpenedfromSettings=false;

            foreach(GameObject border in profileSelectedList)
            {
            if(border != null)
            {
                Destroy(border.gameObject);
            }
            }
            profileSelectedList.Clear();
        }
    }
    void ApplyChangetoProfileBar(GameObject instance)
    {
        instance.transform.GetChild(0).GetComponent<Image>().sprite = MatchManager.Instance.GetProfilePic();
        instance.transform.GetChild(1).GetComponent<TMP_Text>().text = MatchManager.Instance.GetUserName();
    }
    void ApplyChangeToAllProfileBars()
    {
        //GameObject[] instances = GameObject.FindGameObjectsWithTag("ProfileBar");
        GameObject[] instances = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> prefabInstances = new List<GameObject>();

        foreach (GameObject instance in instances)
        {
            if (instance.CompareTag("ProfileBar"))
            {
                prefabInstances.Add(instance);
            }
        }
        
        Debug.Log("instance count: " + prefabInstances.Count);
        foreach (GameObject instance in prefabInstances)
        {
            ApplyChangetoProfileBar(instance);
        }
    }
    public void WatchTutorial()
    {
        MatchManager.Instance.isOnTutorial = true;
        SceneManager.LoadScene(3);
    }
    public void IsLoggingInFirstTime()
    {
        Debug.Log("Playing for 1st time before if");
        if (PlayerPrefs.GetInt("hasPlayedBefore") == 0)
        {
            // Player is playing for the first time
            PlayerPrefs.SetInt("hasPlayedBefore", 1);
            PlayerPrefs.Save();
            Debug.Log("Playing for 1st time");
            // Do first-time setup here
            WelcomePanel.SetActive(true);
        }
        else
        {
            Debug.Log("Playing for 1st time in else");
            SkipTutorial();
        }
    }

    public void SkipTutorial()
    {
        GuestUsernamePanel.SetActive(false);
        WelcomePanel.SetActive(false);
        PlayPanel.SetActive(true);
    }




    //private void OnApplicationFocus(bool isFocused)
    //{
    //    if (MatchManager.Instance.gameMode == 1)
    //    {
    //        if (!isFocused)
    //        {
    //            backgroundTime = Time.time;
    //        }
    //        // Game has been resumed, check if the timeout has elapsed
    //        else if (Time.time - backgroundTime > 180f)
    //        {
    //            MultiplayerCongratspanel.SetActive(false);
    //            errorPanel.SetActive(true);
    //        }
    //        else
    //        {
    //            backgroundTime = 0f;
    //        }
    //    }
    //}

    //public void backToHome()
    //{
    //    SceneManager.LoadScene(0);
    //    MatchManager.Instance.ResetMatchManager();
    //    MatchManager.Instance.gameMode = 0;

    //}

    

    public void openTeamLeaderboard(string teamName)
    {
        
        teamLeaderboardPanel.SetActive(true);
        cricketJodoManager.Instance.setTeamLeaderboardTexts(teamName);
       // cricketJodoManager.setTeamLeaderboardTexts(teamName);

        //cricketJodoManager._LbHeadingTxtTeam.text = teamNameCity + "Leader Board";
        //cricketJodoManager._LbInfoTxtTeam.text = "Your top 5 completion time for " + teamName + " for easy difficulty";



    }



    public void closeTeamLeaderboard()
    {
        cricketJodoManager.Instance.currentTeamNumber = -1;
        LibraryMatchManager.Instance.index = 0;
        LibraryMatchManager.Instance.playerImageIndex = 0;
        LibraryMatchManager.Instance.inGamePlayerSpriteIndex = -1;
        LibraryMatchManager.Instance.currentPlayerImageIndex = 0;
        LibraryMatchManager.Instance.inGamePlayerSprites.Clear();
        teamLeaderboardPanel.SetActive(false);
        LibraryMatchManager.Instance.currentTeamName = "";
        LibraryMatchManager.Instance.currentCityName = "";
    }

    public void openJodoInstruction()
    {
        jodoInstructionPanel.SetActive(true);
    }
    public void closeJodoInstruction()
    {
        jodoInstructionPanel.SetActive(false);
    }

    public void resetLibraryMatchManager()
    {
        LibraryMatchManager.Instance.resetLibraryMatchManager();
    }

    public void previousImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (LibraryMatchManager.Instance.index == 0)
            {
                return;

            }
            else
            {
                LibraryMatchManager.Instance.index--;
                difficultyImage.sprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.index];
                LibraryMatchManager.Instance.playerImageIndex = LibraryMatchManager.Instance.index; //we'll store this playerImageIndex and use this index to load up first image in game
                Debug.Log("playerImageIndex " + LibraryMatchManager.Instance.playerImageIndex);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            
            if (LibraryMatchManager.Instance.index == 0)
            {
                return;

            }
            else
            {
                LibraryMatchManager.Instance.index--;
                congratsImage.GetComponent<Image>().sprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.index];

            }
        }
    }

    public void nextImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            int teamSize = LibraryMatchManager.Instance.currentTeamImages.Count;

            if (LibraryMatchManager.Instance.index > teamSize - 2)
            {

                return;

            }
            else
            {

                LibraryMatchManager.Instance.index++;
                difficultyImage.sprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.index];
                LibraryMatchManager.Instance.playerImageIndex = LibraryMatchManager.Instance.index;
                Debug.Log("playerImageIndex " + LibraryMatchManager.Instance.playerImageIndex);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            LibraryMatchManager.Instance.index = 0;
            int teamSize = LibraryMatchManager.Instance.currentTeamImages.Count;

            if (LibraryMatchManager.Instance.index > teamSize - 1)
            {

                return;

            }
            else
            {

                LibraryMatchManager.Instance.index++;
                congratsImage.GetComponent<Image>().sprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.index];

            }
        }
    }

}
