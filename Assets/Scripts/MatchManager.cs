using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System;
using System.Linq;

public class MatchManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    public static MatchManager Instance;
    public Sprite uploadedImage;
    public float scale16X;
    public float scale16Y;
    public float scale9X;
    public float scale9Y;
    public bool gameState = false;

    float minutes;
    float seconds;
    float timer;
    public float jodoTeamTimer;
    public float jodoTeamScoreSum;
    public List<float> teamScoreList = new List<float>(5);
    public List<float> multiTeamScoreList = new List<float>(5);
    public string difficultystr = "Easy";
    [SerializeField]private string userName;
    Texture2D _texture;
    RenderTexture  puzzleImage;
    bool winnerFound = false;
    [SerializeField] int winnerActorNumber = -1;
    public bool isComingFromInGame; //for going to home from in game end screen
    
    public int gameMode = 0; // 0 for singlePlayer, 1 for multiplayer
    public bool isMusicMute = false;
    public bool isVoiceMute = false;
    public int placedPieces = 0;
    public bool isOnTutorial = false;
    byte[] imageData;

    public int PlayersCount;
    public int allPlayersCount;
    public int gamePlayerCount;

    public int jodoType = 0; //0 for normalJodo, 1 for cricketJodo


    public enum EventCodes : byte
    {
        updateData,
        timerSync,
        endGame,
        newPlayer,
        gameState,
        listPlayers,
        leaveGame
    }

    public List<PlayerInfo> allPlayers = new List<PlayerInfo>();
    public List<PlayerInfo> allPlayersSorted = new List<PlayerInfo>();
    List<GameObject> leaderboardList = new List<GameObject>();
    int index;

    string actorName;
    public Sprite profilePicture = null;
    

    void Awake()
    {
        // PlayerPrefs.DeleteKey("UserName_");
        // PlayerPrefs.DeleteKey("hasPlayedBefore");//uncomment these 2 lines to test 1st time login and run and then comment again
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
        DontDestroyOnLoad(gameObject);
        GetProfilePic();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        difficultystr = "Easy";
        winnerActorNumber = -1;
        isComingFromInGame = false;
        userName=GetUserName();
        
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if(gameState)
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                if(placedPieces == 16)
                {

                    if(jodoType == 1)
                    {
                        if(gameMode == 0)
                        {
                            //cricket jodo singleplayer

                            if (LibraryMatchManager.Instance.isFirstTeam == true)
                            {

                                if (LibraryMatchManager.Instance.inGamePlayerSpriteIndex == LibraryMatchManager.Instance.currentTeamImages.Count - 2)
                                {
                                    teamScoreList.Add(jodoTeamTimer);
                                    // LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                    Debug.Log("timer value " + Timer.Instance.timeValue);
                                    jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                    //end the game and open congrats screen
                                    winnerFound = true;
                                    //use JodoTimer
                                    gameState = false;
                                    //LibraryMatchManager.Instance.currentPlayerImageIndex = 1;



                                }
                                else
                                {
                                    teamScoreList.Add(jodoTeamTimer);
                                    //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                    Debug.Log("timer value " + Timer.Instance.timeValue);
                                    jodoFor16xFirstTime(); //this will load up all remaining puzzles after user complete one image
                                    LibraryMatchManager.Instance.currentPlayerImageIndex++;
                                    //winnerFound = true;
                                }
                            }
                            else if (LibraryMatchManager.Instance.isFirstTeam == false)
                            {
                                if (LibraryMatchManager.Instance.currentPlayerImageIndex + 1 == LibraryMatchManager.Instance.currentTeamImages.Count)
                                {
                                    teamScoreList.Add(jodoTeamTimer);
                                    //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                    Debug.Log("timer value " + Timer.Instance.timeValue);
                                    jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                    winnerFound = true;
                                    LibraryMatchManager.Instance.resetIndexes();
                                    gameState = false;
                                }
                                else
                                {
                                    teamScoreList.Add(jodoTeamTimer);
                                    //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                    Debug.Log("timer value " + Timer.Instance.timeValue);
                                    jodoFor16x();
                                    LibraryMatchManager.Instance.currentPlayerImageIndex++;
                                }
                            }
                        }

                        else if(gameMode == 1)
                        {
                            //cricketJodo multiplayer
                            if (LibraryMatchManager.Instance.currentPlayerImageIndex + 1 == LibraryMatchManager.Instance.currentTeamImages.Count)
                            {
                                multiTeamScoreList.Add(Timer.Instance.timeValue);
                                Debug.Log("264 timer value " + Timer.Instance.timeValue);
                                jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                winnerFound = true;
                                LibraryMatchManager.Instance.endGameIdentifier(LibraryMatchManager.Instance.currentCityName);
                                LibraryMatchManager.Instance.resetIndexesMulti();
                                gameState = false;
                            }
                            else
                            {
                                multiTeamScoreList.Add(Timer.Instance.timeValue);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("274 timer value " + Timer.Instance.timeValue);
                                jodoFor16x();
                                LibraryMatchManager.Instance.currentPlayerImageIndex++;
                            }

                        }
                    }
                    else
                    {
                        winnerFound = true;
                        gameState = false;
                    }
                }


               
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (placedPieces == 9)
                {
                    if (jodoType == 1)
                    {

                        if(gameMode == 0)
                        {

                        if(LibraryMatchManager.Instance.isFirstTeam == true)
                        {
                            if (LibraryMatchManager.Instance.inGamePlayerSpriteIndex == LibraryMatchManager.Instance.currentTeamImages.Count - 2)
                            {
                                teamScoreList.Add(jodoTeamTimer);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("timer value " + Timer.Instance.timeValue);
                                jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                //end the game and open congrats screen
                                winnerFound = true;

                                //use JodoTimer

                                LibraryMatchManager.Instance.resetIndexes();
                                    gameState = false;
                                }
                            else
                            {
                                teamScoreList.Add(jodoTeamTimer);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("timer value " + Timer.Instance.timeValue);
                                jodoFor9xFirstTime(); //this will load up all remaining puzzles after user complete one image
                                LibraryMatchManager.Instance.currentPlayerImageIndex++;
                                //winnerFound = true;
                            }
                        }
                        else if(LibraryMatchManager.Instance.isFirstTeam == false)
                        {
                            if(LibraryMatchManager.Instance.currentPlayerImageIndex + 1 == LibraryMatchManager.Instance.currentTeamImages.Count )
                            {
                                teamScoreList.Add(jodoTeamTimer);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("timer value " + Timer.Instance.timeValue);
                                jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                winnerFound = true;
                                LibraryMatchManager.Instance.resetIndexes();
                                    gameState = false;
                                }
                            else
                            {
                                
                                teamScoreList.Add(jodoTeamTimer);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("timer value " + Timer.Instance.timeValue);
                                jodoFor9x();
                                LibraryMatchManager.Instance.currentPlayerImageIndex++;
                            }
                        }
                        }

                        else if(gameMode == 1)
                        {
                            if (LibraryMatchManager.Instance.currentPlayerImageIndex + 1 == LibraryMatchManager.Instance.currentTeamImages.Count)
                            {
                                
                                multiTeamScoreList.Add(Timer.Instance.timeValue);

                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("264 timer value " + Timer.Instance.timeValue);
                                jodoTeamScoreSum = LibraryMatchManager.Instance.calculateScoreSum();
                                winnerFound = true;
                                LibraryMatchManager.Instance.endGameIdentifier(LibraryMatchManager.Instance.currentCityName);
                                LibraryMatchManager.Instance.resetIndexesMulti();
                                gameState = false;
                            }
                            else
                            {

                                multiTeamScoreList.Add(Timer.Instance.timeValue);
                                //LibraryMatchManager.Instance.scoreList.Add(jodoTeamTimer);
                                Debug.Log("274 timer value " + Timer.Instance.timeValue);
                                jodoFor9x();
                                LibraryMatchManager.Instance.currentPlayerImageIndex++;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Inside MatchManager Update loop");
                        winnerFound = true;
                        gameState = false;
                    }
                }

               

            }
            if(PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
            {
                TimerSyncSend();
            }
            if(winnerFound)
            {
                if(gameMode == 0)
                {
                    Debug.Log("winner found 236");
                    EndGame();
                }
                else if(gameMode == 1)
                {
                    
                    EndGameSend();
                }
                winnerFound = false;
            }
        }
        
        
    }

    public void jodoFor16xFirstTime()
    {
       
        

            //all placed pieces are 16 right now, so that means one puzzle image has been changed, we need to inclement the playerImageIndex so we can move to next image
            LibraryMatchManager.Instance.inGamePlayerSpriteIndex++; //now its 0

            //set upload image sprite as next currentplayerteam sprite
            Sprite nextSprite = LibraryMatchManager.Instance.inGamePlayerSprites[LibraryMatchManager.Instance.inGamePlayerSpriteIndex]; //now we'll load up images from this array. 

            LibraryMatchManager.Instance.LoadTextureImage(nextSprite);

            //restart the game but this time with different sprite
            UIManager.Instance.StartGame();
        
    }

    public void jodoFor9x()
    {
        
        //after player has completed first set, this function will run on play more function
        LibraryMatchManager.Instance.playerImageIndex++;

        Sprite playerSprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.playerImageIndex];
        LibraryMatchManager.Instance.LoadTextureImage(playerSprite);
        
        //UIManager.Instance.StartGame();
        Launcher.instance.GotoNextImage(2);


        
    }
    public void jodoFor16x()
    {

        //after player has completed first set, this function will run on play more function
        LibraryMatchManager.Instance.playerImageIndex++;

        Sprite playerSprite = LibraryMatchManager.Instance.currentTeamImages[LibraryMatchManager.Instance.playerImageIndex];
        LibraryMatchManager.Instance.LoadTextureImage(playerSprite);

        //UIManager.Instance.StartGame();
        Launcher.instance.GotoNextImage(1);



    }
    public void jodoFor9xFirstTime()
    {

            //all placed pieces are 16 right now, so that means one puzzle image has been changed, we need to inclement the playerImageIndex so we can move to next image
            LibraryMatchManager.Instance.inGamePlayerSpriteIndex++; //now its 0

            //set upload image sprite as next currentplayerteam sprite
            Sprite nextSprite = LibraryMatchManager.Instance.inGamePlayerSprites[LibraryMatchManager.Instance.inGamePlayerSpriteIndex]; //now we'll load up images from this array. 

            LibraryMatchManager.Instance.LoadTextureImage(nextSprite);

            //restart the game but this time with different sprite
            UIManager.Instance.StartGame();
        
    }
    private void EndGame()
    {

        OpenGameOverScreen();
        gameState = false;
        if(jodoType == 1)
        {
            Debug.Log("inside end game jodo type 1");
            Debug.Log("jodo team sum " + jodoTeamScoreSum);
            Debug.Log("321 endGame");
            JodoScoreKeeper.Instance.SetCurrentScore(jodoTeamScoreSum);

            //we need to call this function and send our ScoreSum so it can be printed out on screen

        }
        else
        {
            Debug.Log("327L scorekeeper setcurrentScore timer " + timer);
        ScoreKeeper.Instance.SetCurrentScore(timer);
        }
        Debug.Log("In End Game");
        //winnerFound = true;
    }
    void MultiplayerEndGame(int _winner)
    {
        
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
        UIManager.Instance.MultiplayerCongratsScreen();
        gameState = false;
        if(jodoType == 1)
        {
            //JodoScoreKeeper.Instance.SetCurrentScore(jodoTeamScoreSum);
        }
        else if(jodoType == 0)
        {
            ScoreKeeper.Instance.SetCurrentScore(timer);
        }
        
        //UIManager.Instance.SetEndTime(minutes,seconds);
        string winner = ReturnActorName(_winner);
        Debug.Log("Winner" + winner);
        if(winner != PhotonNetwork.NickName)
        {
            UIManager.Instance.winnerLabel.text = winner+" has won the game";
        }
        else if(winner == PhotonNetwork.NickName)
        {
            UIManager.Instance.winnerLabel.text = "You have won the game";
        }

        
    }
    public string ReturnActorName(int actor)
    {
        foreach(PlayerInfo player in allPlayers)
        {
            if(player.actor == actor)
            {
                actorName  = player.name;
            }
            
        }
        return actorName;
    }

    public void SetUploadedImageData(Sprite image,float sixteenX,float sixteenY,float nineX,float nineY)
    {

        if(gameMode == 1 && jodoType == 1)
        {//if cricket jodo multiplayer then..

            //we only send scaling forward, and teamNumber, no need to send sprite
            Debug.Log("Inside MatchManager setdata");
            uploadedImage = image;
            scale16X = sixteenX;
            scale16Y = sixteenY;
            scale9X = nineX;
            scale9Y = nineY;
            _texture = uploadedImage.texture;
            Debug.Log("texture width height " + _texture.width + _texture.height);
            Texture2D tex = new Texture2D(_texture.width, _texture.height, TextureFormat.RGB24, false);
            RenderTexture _renderTexture = new RenderTexture(_texture.width, _texture.height, 0);
            Graphics.Blit(_texture, _renderTexture);
            RenderTexture.active = _renderTexture;
            tex.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
            tex.Apply();
            Debug.Log("Texture is readable " + tex.isReadable);

        }
        else
        {

        Debug.Log("Inside MatchManager setdata");
        uploadedImage = image;
        scale16X = sixteenX;
        scale16Y = sixteenY;
        scale9X = nineX;
        scale9Y = nineY;
        _texture = uploadedImage.texture;
        
        Debug.Log("texture width height "+_texture.width + _texture.height);
        Texture2D tex = new Texture2D(_texture.width, _texture.height, TextureFormat.RGB24, false);
        RenderTexture _renderTexture = new RenderTexture(_texture.width, _texture.height,0);
        Graphics.Blit(_texture,_renderTexture);
        RenderTexture.active = _renderTexture;
        tex.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
        tex.Apply();
        Debug.Log("Texture is readable " +tex.isReadable);
        imageData = tex.EncodeToPNG();

        //UpdateDataSend(imageData);
        }
        
    }
    public void OpenGameOverScreen()
    {
        // SceneManager.LoadScene("EndGame");
        Debug.Log("open game over screen 395 matchmanager");
        StartCoroutine(CongratsUI());
        
    }
    // public void GameOver()
    // {
    //     SceneManager.LoadScene("EndGame");
    //     StartCoroutine(Delay());
        
    // }
    public  void Settime(float min,float sec,float time)
    {
        minutes = min;
        seconds = sec;
        if (MatchManager.Instance.jodoType == 0)
        {

            timer = time;
            
            
        }
        else
        {
            jodoTeamTimer = time;
            //Debug.Log("413L setTime matchManager time " + time);
            

        }
        UIManager.Instance.SetTimer(minutes, seconds);

    }
    // IEnumerator Delay()
    // {
    //     yield return new WaitForSeconds(0.01f);
    //     // UIManager.Instance.GameOverScreen();
    //     UIManager.Instance.CongratsScreen();
    // }
    IEnumerator CongratsUI()
    {
        Debug.Log("congratsUI 429 matchmanager");
        yield return new WaitForSeconds(0.01f);

        if(jodoType == 0)
        {

        UIManager.Instance.SetEndTime(minutes,seconds);
        UIManager.Instance.CongratsScreen();
        }
        else if(jodoType == 1)
        {
            minutes = Mathf.FloorToInt(jodoTeamScoreSum / 60f); //this is the minutes we calculating out of jodoTeamScoreSum
            seconds = Mathf.FloorToInt(jodoTeamScoreSum % 60f);//this is the second we calculating out of jodoTeamScoreSum
            
            
            UIManager.Instance.SetEndTime(minutes, seconds); //this function will set up completion time on screen
            UIManager.Instance.CongratsScreen(); //only setting up congrats screen
           // LibraryMatchManager.Instance.setHighscoreInData(jodoTeamScoreSum); //this will setup the score in that list
        }
    }
    public void SetUserName(string name)
    {
        userName = name;
        PlayerPrefs.SetString("UserName_",userName);
        
    }
    public string GetUserName()
    {
        userName = PlayerPrefs.GetString("UserName_",null);
        return userName;
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code<200)
        {
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            //Debug.Log("The recieved eventcode: " + theEvent);
            object[] data = (object[]) photonEvent.CustomData;
            switch(theEvent)
            {
                case EventCodes.updateData:
                    UpdateDataReceive(data);
                    break;
                case EventCodes.timerSync:
                    TimerSyncReceive(data);
                    break;
                case EventCodes.endGame:
                    EndGameReceive(data);
                    break;
                case EventCodes.newPlayer:
                    NewPlayerReceive(data);
                    break;
                case EventCodes.gameState:
                    GameStateReceive(data);
                    break;
                case EventCodes.listPlayers:
                    ListPlayerReceive(data);
                    break;
                case EventCodes.leaveGame:
                    LeaveGameReceive(data);
                    break;
            }
        }
    }
    public void LeaveGameSend()
    {
        object[] package = new object[]{(int)PhotonNetwork.LocalPlayer.ActorNumber};
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.leaveGame,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.All},
            new SendOptions{Reliability = true}
        );
    }

    private void LeaveGameReceive(object[] data)
    {
        int actorNumber = (int)data[0];
        
        Debug.Log("GetGamePlayerCount: "+GetGamePlayerCount());
        // implement functionality if a player leaves the game
        for(int i=0;i<allPlayers.Count;i++)
        {
            if(allPlayers[i].actor == actorNumber)
            {
                allPlayers.Remove(allPlayers[i]);
            }
        }
        if(gameState)
        {
            UIManager.Instance.SetPlayerCount();
        }
        if(winnerActorNumber != -1)
        {
            UpdateLeaderboard();
        }
    }

    public void ListPlayerSend(List<PlayerInfo> sorted)
    {
        object[] package = new object[sorted.Count];
        for(int i =0;i<sorted.Count;i++)
        {
            object[] peice = new object[4];
            peice[0] = sorted[i].name;
            peice[1] = sorted[i].actor;
            peice[2] = sorted[i].score;
            peice[3] = sorted[i].profilePic.texture.EncodeToPNG();
            package[i] = peice;
        }
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.listPlayers,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.All},
            new SendOptions{Reliability = true}
        );
    }

    private void ListPlayerReceive(object[] data)
    {
        
        allPlayers.Clear();
        for(int i =0;i<data.Length;i++)
        {
            object[] peice = (object[])data[i];
            PlayerInfo player = new PlayerInfo(
                (string)peice[0],
                (int)peice[1],
                (float)peice[2],
                createSpriteFromBytes((byte[])peice[3])
                
            );
            if(!allPlayers.Contains(player))
            {
                Debug.Log("allPlayersSorted.Contains(player) : "+allPlayers.Contains(player));
                allPlayers.Add(player);
            }
            
        }
        if(winnerActorNumber != -1)
        {
            UpdateLeaderboard();
        }
        if(!gameState && winnerActorNumber == -1)
        {
            Launcher.instance.ListAllPlayers(allPlayers);
        }
        else
        {
            UIManager.Instance.playerCountGO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = GetGamePlayerCount().ToString();
        }
        
    }
    void UpdateLeaderboard()
    {
        if(winnerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            UIManager.Instance.congratsRibbon.gameObject.SetActive(true);
            UIManager.Instance.congratsStars.gameObject.SetActive(true);
        }
        
        if(winnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            UIManager.Instance.loseRibbon.gameObject.SetActive(true);
        }
        //Implement Leaderboard to display
        if(leaderboardList.Count != 0)
        {
            foreach (GameObject score in leaderboardList)
            {
                if(score!= null)
                Destroy(score.gameObject);
            }
        }
        
        for(int i=0;i<allPlayers.Count;i++)
        {
            Debug.Log("Isplayerinroom: "+allPlayers[i].name+":"+IsPlayerPresentInCurrentRoom(allPlayers[i].actor));
            
            GameObject newLeaderboardLabel = Instantiate(UIManager.Instance.leaderboardLabel,UIManager.Instance.leaderboardLabel.transform.parent);
            newLeaderboardLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString() + ".";
            if(allPlayers[i].score > 1)
            {
                float minutes = Mathf.FloorToInt(allPlayers[i].score/60f);
                float seconds = Mathf.FloorToInt(allPlayers[i].score%60f);
                newLeaderboardLabel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=allPlayers[i].name;
                newLeaderboardLabel.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = allPlayers[i].profilePic;
                if(i == 0)
                {
                    newLeaderboardLabel.transform.GetChild(1).transform.GetChild(2).GetComponent<Image>().enabled = true;
                }
                if(minutes == 0)
                {
                    if(seconds <= 9)
                    {

                    newLeaderboardLabel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("0{0}s", seconds);
                    }
                    else
                    {
                        newLeaderboardLabel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}s", seconds);
                    }
                }
                else
                {
                    if (seconds <= 9)
                    {

                        newLeaderboardLabel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}m 0{1}s",minutes, seconds);
                    }
                    else
                    {
                        newLeaderboardLabel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}m {1}s",minutes, seconds);
                    }
                }
                
                 //": "+ string.Format("{0:00}:{1:00}",minutes,seconds);
            }
            else
            {
                newLeaderboardLabel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=allPlayers[i].name;
                newLeaderboardLabel.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = allPlayers[i].profilePic;
                newLeaderboardLabel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "In Progress";
               // newLeaderboardLabel.text = allPlayersSorted[i].name + ": "+ "In Progress";   
            }
            if(allPlayers[i].actor == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                newLeaderboardLabel.GetComponent<Image>().sprite = UIManager.Instance.myleaderboardLabel;
            }
            else
            {
                newLeaderboardLabel.GetComponent<Image>().sprite = UIManager.Instance.defaultleaderboardLabel;
            }
            
            newLeaderboardLabel.gameObject.SetActive(true);
            leaderboardList.Add(newLeaderboardLabel);
            
        }
    }

    public void GameStateSend(bool state)
    {
        object[] package = new object[]{(bool)state};
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.gameState,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.All},
            new SendOptions{Reliability = true}
        );
    }

    private void GameStateReceive(object[] data)
    {
        gameState = (bool)data[0];
    }
    

    public void NewPlayerSend()
    {
        byte[] profilePicData = profilePicture.texture.EncodeToPNG();
        object[] package = new object[]{(string)userName,PhotonNetwork.LocalPlayer.ActorNumber,profilePicData};
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.newPlayer,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient},
            new SendOptions{Reliability = true}
        );
        
        
    }

    private void NewPlayerReceive(object[] data)
    {
        byte[] profilePicData = (byte[])data[2];
        Texture2D receivedImage = new Texture2D(2, 2);
        receivedImage.LoadImage(profilePicData);
        Sprite playerProfilePic = Sprite.Create(receivedImage, new Rect(0, 0, receivedImage.width, receivedImage.height), new Vector2(0.5f, 0.5f));
        PlayerInfo player = new PlayerInfo((string)data[0],(int)data[1],0,playerProfilePic);
        allPlayers.Add(player);

        allPlayersCount = allPlayers.Count;
        Debug.Log("allPlayers Count : " + allPlayersCount);
        PlayersCount = allPlayersCount;
        Debug.Log("Players Count : " + PlayersCount);

        ListPlayerSend(allPlayers);
        UpdateDataSend();
        

        
    }

    

    public void EndGameSend()
    {
        string actor = PhotonNetwork.NickName;
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        float score;
        if (jodoType == 0)
        {
        score = timer;
        }
        else
        {
            Debug.Log("816L MM score to send " + jodoTeamScoreSum);
        score = jodoTeamScoreSum;
        }
       
        object[] package = new object[]{(string)actor,(int)actorNum,(float)score};
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.endGame,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.All},
            new SendOptions{Reliability = true}
        );
    }

    private void EndGameReceive(object[] data)
    {
        string actor = (string)data[0];
        int actorNum = (int)data[1];
        float score = (float)data[2];
        if(winnerActorNumber == -1)
        {
            winnerActorNumber = actorNum;

        }
        
        if(actorNum == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            EndGameCheck(winnerActorNumber);
        }
        if(PhotonNetwork.IsMasterClient)
        {
            //need to do something
            for(int i=0;i<allPlayers.Count;i++)
            {
                if(allPlayers[i].actor == actorNum)
                {
                    allPlayers[i].score = score;
                }
                
                
            }
            List<PlayerInfo> sorted=SortPlayers(allPlayers);
            foreach (PlayerInfo sortedPlayer in sorted)
            {
                Debug.Log("Sorted List Player: "+sortedPlayer.name);
            }
            ListPlayerSend(sorted);
        }
        
        
    }

    private void EndGameCheck(int actor)
    {
        MultiplayerEndGame(actor);
    }

    public void TimerSyncSend()
    { //changes done here
        object[] package;
        if (jodoType == 1)
        {
            package = new object[] { (float)jodoTeamTimer };
        }
        else {
            package = new object[] { (float)timer };
        }
        
        //object[] package = new object[]{(float)jodoTeamScoreSum};
        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.timerSync,
            package,
            new RaiseEventOptions{Receivers = ReceiverGroup.All},
            new SendOptions{Reliability = true}
        );
        
    }
    public void TimerSyncReceive(object[] data)
    {
        timer = (float)data[0];
        UpdateMatchTimer(timer);
    }
    void UpdateMatchTimer(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay/60f);
        float seconds = Mathf.FloorToInt(timeToDisplay%60f);
        Settime(minutes,seconds,timeToDisplay); //this is just updating the timer on screen
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void UpdateDataSend()
    {   
        if(jodoType == 1)
        {
            object[] package = new object[] { (int)LibraryMatchManager.Instance.teamNumber, (float)scale16X, (float)scale16Y, (float)scale9X, (float)scale9Y, (string)difficultystr, (int)jodoType }; //send teamNumber, scaling and difficutly string
            Debug.Log("MessageBufferPoolSize: " + PhotonPeer.MessageBufferPoolSize());
            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.updateData,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            );

        }
        else
        {
            object[] package = new object[] { imageData, (float)scale16X, (float)scale16Y, (float)scale9X, (float)scale9Y, (string)difficultystr, (int)jodoType };
           
            Debug.Log("MessageBufferPoolSize: " + PhotonPeer.MessageBufferPoolSize());

            //PhotonNetwork.NetworkingClient.LoadBalancingPeer.SentCountAllowance = data.Length;
            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.updateData,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            );

        }




    }

    public void UpdateDataReceive(object[] dataReceived)
    {
        Debug.Log("936L jodoType received " + jodoType);
        jodoType = (int)dataReceived[6];

        if(jodoType == 0)
        {
            imageData = (byte[])dataReceived[0];
            Texture2D receivedImage = new Texture2D(2, 2);
            receivedImage.LoadImage(imageData);
            uploadedImage = Sprite.Create(receivedImage, new Rect(0, 0, receivedImage.width, receivedImage.height), new Vector2(0.5f, 0.5f));

            scale16X = (float)dataReceived[1];
            scale16Y = (float)dataReceived[2];
            scale9X = (float)dataReceived[3];
            scale9Y = (float)dataReceived[4];
            difficultystr = (string)dataReceived[5];
        }
        else
        {
            //fetch image from currentTeamImage player array and make it to uploaded image
            
            int teamNumber = (int)dataReceived[0]; //this received the teamNumber
                                                   //function that fills in currentImageData array with that teamNumber images and then gets the first image out of that array
            LibraryMatchManager.Instance.checkWhichTeamMulti(teamNumber); //this will set the team images in array.

            LibraryMatchManager.Instance.LoadTextureImage(LibraryMatchManager.Instance.currentTeamImages[0]); 

            //scale16X = (float)dataReceived[1];
            //scale16Y = (float)dataReceived[2];
            //scale9X = (float)dataReceived[3];
            //scale9Y = (float)dataReceived[4];
            difficultystr = (string)dataReceived[5];

        }
        
        // if(!PhotonNetwork.IsMasterClient)
        // {
        //     Launcher.instance.OpenRoomScreen();
        // }
        Launcher.instance.OpenRoomScreen();
    }

    public void setMultiCricJodoImage()
    {

    }
    List<PlayerInfo> SortPlayers(List<PlayerInfo> players)//need to uncomment if not working.
    {
        List<PlayerInfo> sorted = new List<PlayerInfo>();
        List<PlayerInfo> completedPlayers = new List<PlayerInfo>();
        List<PlayerInfo> incompletePlayers = new List<PlayerInfo>();
        // while(sorted.Count<players.Count)
        // {
        //     //float lowest=-1;
        //     PlayerInfo selectedPlayer = players[0];
        //     foreach(PlayerInfo player in players)
        //     {
        //         if(player.score == 0)
        //         {
        //             incompletePlayers.Add(player);
        //         }
        //         else
        //         {
        //             completedPlayers.Add(player);
        //             // if(!sorted.Contains(player))
        //             // {
        //             //     if(player.score>lowest)
        //             //     {
        //             //         selectedPlayer=player;
        //             //         lowest=player.score;
        //             //     }
        //             // }
        //         }
                
        //     }
        //     Debug.Log("Adding to Sorted List: "+selectedPlayer.name);
        //     completedPlayers = completedPlayers.OrderBy(player => player.score).ToList();

        //     //sorted.Add(selectedPlayer);
        //     sorted = completedPlayers.Concat(incompletePlayers).ToList();
        // }
         //float lowest=-1;
            PlayerInfo selectedPlayer = players[0];
            foreach(PlayerInfo player in players)
            {
                if(player.score == 0)
                {
                    incompletePlayers.Add(player);
                }
                else
                {
                    completedPlayers.Add(player);
                    // if(!sorted.Contains(player))
                    // {
                    //     if(player.score>lowest)
                    //     {
                    //         selectedPlayer=player;
                    //         lowest=player.score;
                    //     }
                    // }
                }
                
            }
            Debug.Log("Adding to Sorted List: "+selectedPlayer.name);
            completedPlayers = completedPlayers.OrderBy(player => player.score).ToList();

            //sorted.Add(selectedPlayer);
            sorted = completedPlayers.Concat(incompletePlayers).ToList();
        return sorted;
    }
    

    public void getPlayerCount(TMP_Text playerCountText)
    {
        

        int actorCount = PlayersCount;
        
        String count = actorCount.ToString();
        playerCountText.text = count ;
        Debug.Log("get player count : " + PlayersCount);
    }

    public int GetGamePlayerCount()
    {
        return allPlayers.Count;   
    }

    //public void updateGamePlayerCount()
    //{
    //    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    //    if (difficultystr.Equals("Easy"))
    //    {
    //        UIManager.Instance.easyPlayerCountText.GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    //    GameObject playerCountTxtEasy = Instantiate(UIManager.Instance.easyPlayerCountText);
    //    playerCountTxtEasy.GetComponent<TMP_Text>().text = playerCount.ToString();
    //    Debug.Log("player count in game set to  : " + playerCountTxtEasy.GetComponent<TMP_Text>().text);
    //    }
    //    else if(difficultystr.Equals("Hard"))
    //    {
    //    GameObject playerCountTxtHard = Instantiate(UIManager.Instance.HardPlayerCountText);
    //    playerCountTxtHard.GetComponent<TMP_Text>().text = playerCount.ToString();
    //    Debug.Log("player count in game set to  : " + playerCountTxtHard.GetComponent<TMP_Text>().text);

    //    }

    //    Debug.Log("player count in game set to  : " + playerCount);


    //}
    public void ReturnHome()
    {
        if(gameState)
        {
            LeaveGameSend();
        }
        Invoke("LeaveRoomInGame",1f);


    }

    private  void LeaveRoomInGame()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        if(isComingFromInGame)
        {
            Debug.Log("Here");
            base.OnLeftRoom();
            ResetMatchManager();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        }
        
    }
    public void ResetMatchManager()
    {
        //Reset all the feilds in MatchManager
        allPlayers.Clear();
        uploadedImage = null;
        scale16X=0;
        scale16Y=0;
        scale9X=0;
        scale9Y=0;
        winnerActorNumber = -1;
        gameState = false;
        difficultystr = "Easy";
        isComingFromInGame = false;
        placedPieces = 0;
        PlayersCount = 0;
        allPlayersCount = 0;
        isOnTutorial = false;
        jodoType = 0;
    }
    public bool IsPlayerPresentInCurrentRoom(int actorNumber)
    {
        
        Player[] players = PhotonNetwork.PlayerList;
        for(int i=0;i<players.Length;i++)
        {
            if(players[i].ActorNumber == actorNumber)
            {
                return true;
            }
        }
        return false;
    }
    public void SetProfileDetails(string name,Texture2D profilePic)
    {
        //Implement profile picture and username
        SetUserName(name);
        byte[] bytes = profilePic.EncodeToPNG();
        ProfilePictureData _profilePicData = new ProfilePictureData();
        _profilePicData.bytes = bytes;
        string json = JsonUtility.ToJson(_profilePicData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/profilePicData.json", json);
    }
    public Sprite GetProfilePic()
    {
        //Implement the method to return profile pic
        try
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/profilePicData.json");
            ProfilePictureData imageData = JsonUtility.FromJson<ProfilePictureData>(json);
            Texture2D _texture = new Texture2D(2, 2);
            _texture.LoadImage(imageData.bytes);
            profilePicture = Sprite.Create(
            _texture,
            new Rect(0, 0,_texture.width,_texture.height),
            new Vector2(0.5f, 0.5f));
        }catch(Exception e)
        {
            Debug.Log("Profile picture not found" + e);
            profilePicture = null;
        }
        

        return profilePicture;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ResetMatchManager();
        SceneManager.LoadScene(0);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(gameState && PhotonNetwork.IsMasterClient)
        {
            for(int i=0;i<allPlayers.Count;i++)
            {
                if(otherPlayer.ActorNumber == allPlayers[i].actor)
                {
                    allPlayers.RemoveAt(i);
                }
            }
            ListPlayerSend(allPlayers);
        }
    }
    public Func<byte[], Sprite> createSpriteFromBytes = (bytes) =>
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;
    };
    

}
[System.Serializable]
public class ProfilePictureData
{
    public byte[] bytes;
}
[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int actor;
    public float score;
    public Sprite profilePic;
    public PlayerInfo(string _name,int _actor,float _score,Sprite _profilePic)
    {
        name = _name;
        actor = _actor;
        score = _score;
        profilePic = _profilePic;
    }
}
