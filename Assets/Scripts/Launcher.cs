 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{   
    [SerializeField] GameObject PlayWithFriendsPanel;
    [SerializeField] GameObject uploadImagePanel;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] TMP_Text loadingTxt;
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField createRoomTxt;
    [SerializeField] GameObject roomScreen;
    [SerializeField] TMP_Text roomNameTxt,playerNameLabel,difficultyLabel;
    [SerializeField] GameObject errorScreen;
    [SerializeField] TMP_Text errorTxt;
    [SerializeField] GameObject multiplayerPanel;
    [SerializeField] GameObject signUpPanel;
    [SerializeField] GameObject joinRoomPanel;
    [SerializeField] GameObject multiCreateRoomPanel;
    [SerializeField] GameObject multiJoinRoomPanel;
    [SerializeField] TMP_InputField joinRoomTxt;
    
    [SerializeField] GameObject startBtn;

    [SerializeField] GameObject joinRoomInputText;

    [SerializeField] GameObject uploadBackBtn;
    [SerializeField] GameObject uploadBackBtnMulti;
    [SerializeField] GameObject StartGameBtn;
    [SerializeField] GameObject dummyStartBtn;

    [SerializeField] GameObject homeButtonSingleP;
    [SerializeField] GameObject homeButtonMultiP;

    [SerializeField] GameObject CreatePuzzleMultiplayerBtn;
    [SerializeField] public Image _uploadedImage;

    [SerializeField] Sprite[] copyIconSprites;

    [SerializeField] GameObject createRoomBtn;
    [SerializeField] Sprite[] createRoomBtnSprites;

    [SerializeField] GameObject joinRoomBtn;
    [SerializeField] Sprite[] joinRoomBtnSprites;

    [SerializeField] Image myProfilePic;
    [SerializeField] TMP_Text myNametxt;
    [SerializeField] GameObject playerLabel;
    [SerializeField] GameObject noPlayerLabel;
    [SerializeField] GameObject startGameInfoHost;
    [SerializeField] GameObject startGameInfoClient;

    public TMP_Text lobbyPlayerCountText;
    public TMP_InputField roomnameInputField;

    public Button copyCodeBtn;
    
    private string copiedRoomName;
    private string roomName;
    private int maxPlayers = 5;//maximum players for multiplayer

     List<GameObject> allPlayerNames = new List<GameObject>();

    
    bool loadingAnim;
    
    public static Launcher instance;

    void Awake()
    {
        instance = this;
        PhotonNetwork.KeepAliveInBackground = 180;
    }
    void Start()
    {
        CloseMenus();
    }
    public void CloseMenus()
    {
        Debug.Log("Inside Closemenus in Launcher");
        LoadingScreen.SetActive(false);
        //createRoomPanel.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        multiplayerPanel.SetActive(false);
        //joinRoomPanel.SetActive(false);
        
        uploadImagePanel.SetActive(false);
        loadingTxt.text="";
        loadingAnim = false;

    }
    public void closeMultiIcons()
    {
        //homeButtonMultiP.SetActive(false);
        //homeButtonSingleP.SetActive(true);
        uploadBackBtn.SetActive(true);
        StartGameBtn.SetActive(true);
        CreatePuzzleMultiplayerBtn.SetActive(false);
        uploadBackBtnMulti.SetActive(false);
    }
    public void ConnectOnline()
    {   MatchManager.Instance.gameMode = 1;
        
        Debug.Log("in play with friends");
        CloseMenus();
        // LoadingScreen.SetActive(true);
        // loadingTxt.text = "Connecting To Network";
        OpenLoadingScreen("Connecting to Network");
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
           
        }

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //loadingTxt.text = "Joining Lobby...";
        OpenLoadingScreen("Joining Lobby");
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        CloseMenus();
        multiplayerPanel.SetActive(true);
        OpenCreateRoom();

        // multiplayerPanel.SetActive(true); Not required we changed the flow after joining lobby 
        // room will be automatically created and user will go to upload image screen.
        if(MatchManager.Instance.GetUserName() == null || MatchManager.Instance.GetUserName()=="")
        {
            CloseMenus();
            signUpPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.NickName = MatchManager.Instance.GetUserName();
        }
    }
    

    public void OpenCreateRoom()
    {

        //multiCreateRoomPanel.SetActive(true);
        //multiJoinRoomPanel.SetActive(false);

        ResetInputFields(joinRoomTxt);
        createRoomBtn.GetComponent<Image>().sprite = createRoomBtnSprites[0];
        joinRoomBtn.GetComponent<Image>().sprite = joinRoomBtnSprites[1];

        // CloseMenus();

        uploadImagePanel.SetActive(true);
        // uploadBackBtn.SetActive(false);
        // uploadBackBtnMulti.SetActive(true);

        // homeButtonSingleP.SetActive(false);
        // homeButtonMultiP.SetActive(true);

        createRoomPanel.SetActive(true);
        joinRoomPanel.SetActive(false);


    }
    public void CreateRoom()
    {
        roomName = GenerateRandomRoomName();
        copiedRoomName = roomName;
        Debug.Log("Room Name: "+ roomName);
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = ((byte)maxPlayers);
        PhotonNetwork.CreateRoom(roomName,options);
        CloseMenus();
        OpenLoadingScreen("Creating Room");
        // loadingTxt.text = "Creating Room...";
        // LoadingScreen.SetActive(true);
        //copiedRoomText.text = createRoomTxt.text;
    }

    
    public override void OnJoinedRoom()
    {
        // Debug.Log("In create room callback");
        //   if(PhotonNetwork.IsMasterClient){

        //     CloseMenus();
        //     uploadImagePanel.SetActive(true);
        //     uploadBackBtn.SetActive(false);

        // }


        //roomScreen.SetActive(true);
        // CloseMenus();
        // uploadImagePanel.SetActive(true);
        // uploadBackBtn.SetActive(false);
        // StartGameBtn.SetActive(false);
        // CreatePuzzleMultiplayerBtn.SetActive(true);
        // roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
        // //copiedRoomText.text = PhotonNetwork.CurrentRoom.Name;

        // ListAllPlayers();
        // if(PhotonNetwork.IsMasterClient)
        // {
        //     startBtn.SetActive(true);
        // }
        // else
        // {
        //     startBtn.SetActive(false);
        // }
        //string text = roomNameTxt.text;
        //copiedRoomText.text = text;

        //ListAllPlayers();

        if(PhotonNetwork.IsMasterClient)
        {
            MatchManager.Instance.NewPlayerSend();
            Debug.Log("masterclient player count  : " + MatchManager.Instance.allPlayersCount);
            MatchManager.Instance.getPlayerCount(lobbyPlayerCountText);
        }
        else
        {
            MatchManager.Instance.getPlayerCount(lobbyPlayerCountText);
        }
        


        



        onProceedDiff();


    }
    
    public void onProceedDiff()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            MatchManager.Instance.NewPlayerSend();
            CloseMenus();
            OpenLoadingScreen("Loading");
            // LoadingScreen.SetActive(true);
            // loadingTxt.text = "Loading...";
        }
        else
        {
            //OpenRoomScreen();
            CloseMenus();
            OpenLoadingScreen("Loading");
        }
        
    }

    public void setStartBtnActive()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startBtn.SetActive(true);
            dummyStartBtn.SetActive(false);
        }
        else
        {
            dummyStartBtn.SetActive(true);
            startBtn.SetActive(false);

        }
    }
    public void OpenRoomScreen()
    {
        string difficulty;
        CloseMenus();
        roomScreen.SetActive(true);
        roomNameTxt.text = "Game code : " + PhotonNetwork.CurrentRoom.Name;
        difficulty = MatchManager.Instance.difficultystr;
        difficultyLabel.text = "Level : "+difficulty;
        if(difficulty == "Easy")
        {
           // UIManager.Instance.roomDiffInfo.text = "Puzzle will be divided into 9 pieces";
        }
        else if(difficulty == "Hard")
        {
           // UIManager.Instance.roomDiffInfo.text = "Puzzle will be divided into 16 pieces";
        }
        SetUpladedImage();
        ListAllPlayers(MatchManager.Instance.allPlayers);

        //if (PhotonNetwork.IsMasterClient)
        //{

        //    startBtn.SetActive(true);
        //}

        //else
        //{
        //    startBtn.SetActive(false);
        //    dummyStartBtn.SetActive(false);
        //}
        if (!PhotonNetwork.IsMasterClient)
        {   
            startBtn.SetActive(false);
            dummyStartBtn.SetActive(false);
        }
    }

    public void SetUpladedImage()
    {
        _uploadedImage.sprite = MatchManager.Instance.uploadedImage;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CloseMenus();
        errorTxt.text = "Failed to connect to Room: "+message;
        errorScreen.SetActive(true);
    }

    public void LeaveLobbyfn(){
        Debug.Log("In LeaveLobby");
        PhotonNetwork.LeaveLobby();
        Debug.Log("LeaveLobby called");
        //loadingTxt.text = "Leaving...";
        OpenLoadingScreen("Leaving...");
        CloseMenus();
        LoadingScreen.SetActive(true);
        closeMultiIcons();
        StartCoroutine(LobbyDisconnectDelay());
        ResetInputFields(joinRoomTxt);
    }

    public IEnumerator LobbyDisconnectDelay(){
        yield return new WaitForSeconds(0.6f);
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from lobby");
        
    }
    public override void OnLeftLobby()
    {   
        Debug.Log("left lobby callback");
        CloseMenus();
        PlayWithFriendsPanel.SetActive(true);

        
       
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        CloseMenus();
        base.OnDisconnected(cause);
        Debug.Log("Disconnected from Photon network: " + cause.ToString());
        
        //if (cause != DisconnectCause.DisconnectByClientLogic)
        //{
        //    errorScreen.SetActive(true);
        //    errorTxt.text = "Disconnected from network: " + cause.ToString();
        //}


    }

    [PunRPC]
    void UpdatePlayerCount(int count)
    {
        MatchManager.Instance.PlayersCount = count;
        MatchManager.Instance.getPlayerCount(lobbyPlayerCountText);

        //sending rpc to all clients to update the player count
        photonView.RPC("UpdatePlayerCountForAll", RpcTarget.All, count);

    }
    [PunRPC]
    void UpdatePlayerCountForAll(int count)
    {
        MatchManager.Instance.PlayersCount = count;
        MatchManager.Instance.getPlayerCount(lobbyPlayerCountText);
    }


    public void ListAllPlayers(List<PlayerInfo> allPlayers)
    {
        Debug.Log("allplayers count: "+allPlayers.Count);
        // myProfilePic.sprite= MatchManager.Instance.profilePicture;
        // myNametxt.text = MatchManager.Instance.GetUserName();
        foreach(GameObject player in allPlayerNames)
        {
            Destroy(player.gameObject);
        }
        allPlayerNames.Clear();
        Player[] players = PhotonNetwork.PlayerList;

        int playerCount = players.Length;

        MatchManager.Instance.PlayersCount = playerCount;
        MatchManager.Instance.getPlayerCount(lobbyPlayerCountText);

        
        //photonView.RPC("UpdatedPlayerCount", RpcTarget.MasterClient, playerCount);

        for (int i=0; i<allPlayers.Count;i++)
        {
            if(MatchManager.Instance.IsPlayerPresentInCurrentRoom(allPlayers[i].actor))
            {
                if(allPlayers[i].actor == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                
                    myProfilePic.sprite=allPlayers[i].profilePic;
                    myNametxt.text = allPlayers[i].name;
                }
                else
                {
                    GameObject newPlayerLabel = Instantiate(playerLabel,playerLabel.transform.parent);
                    newPlayerLabel.transform.GetChild(0).GetComponent<TMP_Text>().text = allPlayers[i].name;
                    newPlayerLabel.GetComponent<Image>().sprite = allPlayers[i].profilePic;
                    newPlayerLabel.gameObject.SetActive(true);
                    allPlayerNames.Add(newPlayerLabel);
                }
            }
            else
            {
                allPlayers.RemoveAt(i);
            }
            
        }
        if(allPlayers.Count<maxPlayers)
        {
            for(int i =0;i<maxPlayers-allPlayers.Count;i++)
            {
                GameObject newnoPlayerLabel = Instantiate(noPlayerLabel,noPlayerLabel.transform.parent);
                newnoPlayerLabel.SetActive(true);
                allPlayerNames.Add(newnoPlayerLabel);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {

            setStartBtnActive();
            startGameInfoHost.SetActive(true);
            startGameInfoClient.SetActive(false);
            startGameInfoHost.GetComponent<TMP_Text>().text = "Since you are the host, you will start the game";

        }
        else if(!PhotonNetwork.IsMasterClient) {
            startGameInfoHost.SetActive(false);
            startGameInfoClient.SetActive(true);
            startGameInfoClient.GetComponent<TMP_Text>().text = $"Please wait...{PhotonNetwork.MasterClient.NickName}" + " will start the game";
        }

    }

    
    
    

    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Coming from OnPlayerEnteredRoom");
        GameObject  newPlayerLabel = Instantiate(playerLabel,playerLabel.transform.parent);
        newPlayerLabel.transform.GetChild(0).GetComponent<TMP_Text>().text = newPlayer.NickName;
        newPlayerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(newPlayerLabel);
        ListAllPlayers(MatchManager.Instance.allPlayers);
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("PlayerLeft");
        ListAllPlayers(MatchManager.Instance.allPlayers);
    }
    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        CloseMenus();
        errorTxt.text = "Failed to connect to Room: "+message;
        errorScreen.SetActive(true);
    }
    public void CloseErrorScreen()
    {
        CloseMenus();
        multiplayerPanel.SetActive(true);
    }
    public void LeaveRoom()
    {
        MatchManager.Instance.LeaveGameSend();
        PhotonNetwork.LeaveRoom();
        //OpenLoadingScreen("Leaving Room");
        //loadingTxt.text = "Leaving Room...";
        Debug.Log("in leave room");
        CloseMenus();
        if(MatchManager.Instance.gameMode == 1){
            UIManager.Instance.ClosePanels();
            multiplayerPanel.SetActive(true);
        }
        MatchManager.Instance.ResetMatchManager();
        
        //LoadingScreen.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        CloseMenus();
        multiplayerPanel.SetActive(true);
        //MatchManager.Instance.gameMode = 0;
        Debug.Log("gameMode set to 0 singleplayer");

        ResetInputFields(roomnameInputField);

        //TextMeshProUGUI tmp = joinRoomTxt.GetComponent<TextMeshProUGUI>();
        //tmp.SetText("");

        //uploadBackBtn.SetActive(true);
        //StartGameBtn.SetActive(true);
        //CreatePuzzleMultiplayerBtn.SetActive(false);




    }

    
    public void backToMultiplayerPanel()
    {
        CloseMenus();
        
        multiplayerPanel.SetActive(true);
        multiCreateRoomPanel.SetActive(true);
        createRoomBtn.GetComponent<Image>().sprite = createRoomBtnSprites[0];
        joinRoomBtn.GetComponent<Image>().sprite = joinRoomBtnSprites[1];
    }

    
    public void OpenJoinRoomScreen()
    {
        //CloseMenus();
        createRoomBtn.GetComponent<Image>().sprite = createRoomBtnSprites[1];
        joinRoomBtn.GetComponent<Image>().sprite = joinRoomBtnSprites[0];
        createRoomPanel.SetActive(false);
        joinRoomPanel.SetActive(true);

    }
    
    public void JoinRoom()
    {
        if(!string.IsNullOrEmpty(joinRoomTxt.text))
        {
            PhotonNetwork.JoinRoom(joinRoomTxt.text);
            CloseMenus();
            OpenLoadingScreen("Joining Room");
            // loadingTxt.text = "Joining Room";
            // LoadingScreen.SetActive(true);
        }
        ResetInputFields(joinRoomTxt);
    }
    public void StartMultiplayerGame(int lvlIndex)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(lvlIndex);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        else
        {
            PhotonNetwork.LoadLevel(lvlIndex);
        }
    }
    public void GotoNextImage(int levelIndex)
    {
        MatchManager.Instance.placedPieces = 0;
        PhotonNetwork.AutomaticallySyncScene=false;
        PhotonNetwork.LoadLevel(levelIndex);
    }








    public static string GenerateRandomRoomName()
    {
        var allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
        var length = 4;
        var randomName = new char[length];
        for(int i=0;i<length;i++)
        {
            randomName[i] = allChars[Random.Range(0,allChars.Length)];
        }
        
        return new string(randomName);
        

    }

    public void CopyRoomCode()
    {
        GUIUtility.systemCopyBuffer = copiedRoomName;
        if (copyCodeBtn.GetComponent<Image>().sprite == copyIconSprites[0])
        {
            
            copyCodeBtn.GetComponent<Image>().sprite = copyIconSprites[1];
            
            return;
        }
        copyCodeBtn.GetComponent<Image>().sprite = copyIconSprites[0];
        Debug.Log("copy room code");
        Debug.Log(copiedRoomName);
        
    }

    public void GoToHomeMulti()
    {   if(MatchManager.Instance.gameMode == 1)
        {
            UIManager.Instance.ClosePanels();
            UIManager.Instance.closeHomeSettings();
            //CloseMenus();
            LeaveLobbyfn();
            CloseMenus();
            multiplayerPanel.SetActive(true);
        }
       
    }
    void OpenLoadingScreen(string _loadingText)
    {
        float speed = 0.5f;
        LoadingScreen.SetActive(true);
        loadingAnim = true;
        StartCoroutine(AnimateLoadingText(_loadingText,speed));
        
        
    }
    IEnumerator AnimateLoadingText(string _loadingText,float speed)
    {
         while (loadingAnim)
        {
            loadingTxt.text = "loading"+".";
            yield return new WaitForSeconds(speed);
            loadingTxt.text = "loading"+"..";
            yield return new WaitForSeconds(speed);
            loadingTxt.text = "loading"+"...";
            yield return new WaitForSeconds(speed);
        }
    }
    public string GetRoomName()
    {
        return PhotonNetwork.CurrentRoom.Name;   
    }
    
    public void ResetInputFields(TMP_InputField inputField)
    {
        Debug.Log("Input field reset");
        inputField.SetTextWithoutNotify("");
    }

    

    public void backToMultiplayerRoom()
    {
        multiplayerPanel.SetActive(true);
    }
    
    
}




