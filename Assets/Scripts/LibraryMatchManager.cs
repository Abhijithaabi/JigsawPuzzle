using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using ImageCropperNamespace;
using TMPro;
using System;

public class LibraryMatchManager : MonoBehaviour
{

    public static LibraryMatchManager Instance;

    #region libraryData

    string imagePath;
    byte[] imageData = null;

    #region team list 
    [SerializeField] List<Sprite> _1ChennaiPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _2DelhiPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _3GujaratPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _4KolkataPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _5LucknowPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _6MumbaiPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _7PunjabPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _8BangalorePlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _9RajasthanPlayerImages = new List<Sprite>();
    [SerializeField] List<Sprite> _10HyderabadPlayerImages = new List<Sprite>();

    #endregion 
    [SerializeField] GameObject uploadImagePanel;
    [SerializeField] UploadImage uploadImage;
    [SerializeField] PuzzleSelector puzzleSelector;


    [SerializeField] public List<float> scoreList = new List<float>();
    [SerializeField] public List<Sprite> currentTeamImages = new List<Sprite>();
    [SerializeField] public List<Sprite> inGamePlayerSprites = new List<Sprite>(); //this will store 4 images
    public int index;
    public int playerImageIndex = 0;
    public int inGamePlayerSpriteIndex = -1;
    public int currentPlayerImageIndex = 1; //for team identifier button at top of game screen
    public int congratsImageCounter = 0;


    public string leaderboardDifficulty = "easy"; //easy and hard
    public int completedTeamsCount = 0;
    public string currentTeamName;
    public string currentCityName;
    public int teamNumber;

    public bool isFirstTeam = true;

    [SerializeField] public List<TeamScoreData> teamScoreDataList = new List<TeamScoreData>();
    

    #endregion

    private void Awake()
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
        index = 0;
        StartCoroutine(SetLibrary());

        cricketJodoManager.Instance.easyBtn.GetComponent<Image>().sprite = cricketJodoManager.Instance.easyBtnSprites[0]; //on 
        cricketJodoManager.Instance.hardBtn.GetComponent<Image>().sprite = cricketJodoManager.Instance.hardBtnSprites[1]; //off
        leaderboardDifficulty = "Easy";

        //SetCompletedTeamsCount(5);

        //completedTeamsCount = GetCompletedTeamsCount(); //this will be used to set up progress bar in library panel



        //teamScoreDataList = FileHandler.ReadListFromJSON<TeamScoreData>("teamScoreData.json");
        //teamScoreDataList.Clear();
        //teamScoreDataList.RemoveAt(2);

        //FileHandler.SaveToJSON<TeamScoreData>(teamScoreDataList, "teamScoreData.json");



    }

    public void getTeamListFromDisk()
    {
        teamScoreDataList = FileHandler.ReadListFromJSON<TeamScoreData>("teamScoreData.json");

    }

    public void emptyTeamList()
    {
        teamScoreDataList = FileHandler.ReadListFromJSON<TeamScoreData>("teamScoreData.json");
        teamScoreDataList.Clear();
        FileHandler.SaveToJSON<TeamScoreData>(teamScoreDataList, "teamScoreData.json");

    }
    IEnumerator SetLibrary()
    {

        for (int image = 1; image <= 50; image++)
        {
            imagePath = Path.Combine(Application.streamingAssetsPath, image + ".jpg");
            Debug.Log("imagePath" + imagePath);
#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, files in the StreamingAssets folder are compressed in a .jar file
            // so we need to use Unity's WWW class to load them.
            UnityWebRequest www = UnityWebRequest.Get(imagePath);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                imageData = www.downloadHandler.data;
            }
            else
            {
                continue;
            }
            //imageData = www.downloadHandler.data;
#else
            // On other platforms, we can use standard file I/O methods to load the file.
            imageData = File.ReadAllBytes(imagePath);
            yield return null;
#endif
            Sprite playerImage = MatchManager.Instance.createSpriteFromBytes(imageData);
            // Texture2D imageTexture = new Texture2D(2, 2);
            // imageTexture.LoadImage(imageData);

            //adding sprites in all team lists
            if (image >= 1 && image <= 5)
            {
                _1ChennaiPlayerImages.Add(playerImage);

            }
            if (image >= 6 && image <= 10)
            {
                _2DelhiPlayerImages.Add(playerImage);
            }
            if (image >= 11 && image <= 15)
            {
                _3GujaratPlayerImages.Add(playerImage);

            }
            if (image >= 16 && image <= 20)
            {
                _4KolkataPlayerImages.Add(playerImage);
            }
            if (image >= 21 && image <= 25)
            {
                _5LucknowPlayerImages.Add(playerImage);
            }
            if (image >= 26 && image <= 30)
            {
                _6MumbaiPlayerImages.Add(playerImage);

            }
            if (image >= 31 && image <= 35)
            {
                _7PunjabPlayerImages.Add(playerImage);
            }
            if (image >= 36 && image <= 40)
            {
                _8BangalorePlayerImages.Add(playerImage);

            }
            if (image >= 41 && image <= 45)
            {
                _9RajasthanPlayerImages.Add(playerImage);
            }
            if (image >= 46 && image <= 50)
            {
                _10HyderabadPlayerImages.Add(playerImage);
            }

        }



    }



    public void setDifficultyImage(string teamNameCity)
    {
        //this opens up when we click on play button from any team card. opens up difficulty panel with all 5 images.

        index = 0;
        checkWhichTeam(teamNameCity);
        UIManager.Instance.ChooseDifficulty();

        LoadTextureImage(currentTeamImages[playerImageIndex]); //this function will load the image on puzzle pieces and also check if it needs to be loaded or not
                                                               //if playerImageIndex == currentTeamImages.Count then we wont load up anything and just open congrats screen.
                                                               //right now playerImageIndex is 0


    }

    public float calculateScoreSum()
    {
        if(MatchManager.Instance.gameMode == 0)
        {
            float sum = 0;
            for (int i = 0; i < MatchManager.Instance.teamScoreList.Count; i++)
            {
                Debug.Log("calculating sum 191L");
                sum += MatchManager.Instance.teamScoreList[i];
            }
            return sum;
        }
        else
        {
            float sum = 0;
            for (int i = 0; i < MatchManager.Instance.multiTeamScoreList.Count; i++)
            {
                Debug.Log("calculating sum 191L");
                sum += MatchManager.Instance.multiTeamScoreList[i];
            }
            return sum;
        }

        
    }
    public void LoadTextureImage(Sprite playerImage)
    {




        //extract the scaling width height of texture of this sprite so we can set up this sprite on
        Texture2D playerImageTexture = playerImage.texture;

        Sprite playerImageSprite = Sprite.Create(playerImageTexture, new Rect(0, 0, playerImageTexture.width, playerImageTexture.height), new Vector2(0.5f, 0.5f));

        float x = 1573.7025f / playerImageTexture.width;
        float y = 1563.381f / playerImageTexture.height;
        float ninex = 1176.4278f / playerImageTexture.width;
        float niney = 1185.2622f / playerImageTexture.height;

        //setup the uploaded Image to our current player image, next puzzle selector will pickup this image from matchmanager uploadImage 
        MatchManager.Instance.SetUploadedImageData(playerImageSprite, x, y, ninex, niney);

        puzzleSelector = GetComponent<PuzzleSelector>(); //getting puzzle selector as we need that to setup sprite on puzzle pieces.




    }

    public void checkWhichTeamMulti(int _teamNumber)
    {
        if(_teamNumber == 1)
        {
            currentCityName = "Chennai";
            currentTeamName = "Chennai Super Kings";
            currentTeamImages = _1ChennaiPlayerImages;
        }
        if (_teamNumber == 2)
        {
            currentCityName = "Delhi";
            currentTeamName = "Delhi Capitals";
            currentTeamImages = _2DelhiPlayerImages;

        }
        if (_teamNumber == 3)
        {
            currentCityName = "Gujarat";
            currentTeamName = "Gujarat Titans";
            currentTeamImages = _3GujaratPlayerImages;
        }
        if (_teamNumber == 4)
        {
            currentCityName = "Kolkata";
            currentTeamName = "Kolkata Knight Riders";
            currentTeamImages = _4KolkataPlayerImages;
        }
        if (_teamNumber == 5)
        {
            currentCityName = "Lucknow";
            currentTeamName = "Lucknow Super Giants";
            currentTeamImages = _5LucknowPlayerImages;
        }
        if (_teamNumber == 6)
        {
            currentCityName = "Mumbai";
            currentTeamName = "Mumbai Indians";
            currentTeamImages = _6MumbaiPlayerImages;
        }
        if (_teamNumber == 7)
        {
            currentCityName = "Punjab";
            currentTeamName = "Punjab Kings";
            currentTeamImages = _7PunjabPlayerImages;
        }
        if (_teamNumber == 8)
        {
            currentCityName = "Bangalore";
            currentTeamName = "Royal Challengers Bangalore";
            currentTeamImages = _8BangalorePlayerImages;
        }
        if (_teamNumber == 9)
        {
            currentCityName = "Rajasthan";
            currentTeamName = "Rajasthan Royals";
            currentTeamImages = _9RajasthanPlayerImages;
        }
        if (_teamNumber == 10)
        {
            currentCityName = "Hyderabad";
            currentTeamName = "Sunrisers Hyderabad";
            currentTeamImages = _10HyderabadPlayerImages;
        }
    }
    public void checkWhichTeam(string teamNameCity)
    {
        if (teamNameCity == "Chennai")
        {
            teamNumber = 1;
            currentCityName = "Chennai";
            currentTeamName = "Chennai Super Kings";
            currentTeamImages = _1ChennaiPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];

        }
        if (teamNameCity == "Delhi")
        {
            teamNumber = 2;
            currentCityName = "Delhi";
            currentTeamName = "Delhi Capitals";
            currentTeamImages = _2DelhiPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
        if (teamNameCity == "Punjab")
        {
            teamNumber = 7;
            currentCityName = "Punjab";
            currentTeamName = "Punjab Kings";
            currentTeamImages = _7PunjabPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];

        }
        if (teamNameCity == "Lucknow")
        {
            teamNumber = 5;
            currentCityName = "Lucknow";
            currentTeamName = "Lucknow Super Giants";
            currentTeamImages = _5LucknowPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
        if (teamNameCity == "Hyderabad")
        {
            teamNumber = 10;
            currentCityName = "Hyderabad";
            currentTeamName = "Sunrisers Hyderabad";
            currentTeamImages = _10HyderabadPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
        if (teamNameCity == "Mumbai")
        {
            teamNumber = 6;
            currentCityName = "Mumbai";
            currentTeamName = "Mumbai Indians";
            currentTeamImages = _6MumbaiPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];

        }
        if (teamNameCity == "Kolkata")
        {
            teamNumber = 4;
            currentCityName = "Kolkata";
            currentTeamName = "Kolkata Knight Riders";
            currentTeamImages = _4KolkataPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
        if (teamNameCity == "Bangalore")
        {
            teamNumber = 8;
            currentCityName = "Bangalore";
            currentTeamName = "Royal Challengers Bangalore";
            currentTeamImages = _8BangalorePlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];

        }
        if (teamNameCity == "Rajasthan")
        {
            teamNumber = 9;
            currentCityName = "Rajasthan";
            currentTeamName = "Rajasthan Royals";
            currentTeamImages = _9RajasthanPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
        if (teamNameCity == "Gujarat")
        {
            teamNumber = 3;
            currentCityName = "Gujarat";
            currentTeamName = "Gujarat Titans";
            currentTeamImages = _3GujaratPlayerImages;
            UIManager.Instance.difficultyImage.sprite = currentTeamImages[0];
        }
    }

    public void endGameIdentifier(string teamNameCity)
    {
        TMP_Text imageCounter = cricketJodoManager.Instance.teamIdentifier.transform.GetChild(1).GetComponent<TMP_Text>();
        imageCounter.text = currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString();

        
        if (teamNameCity == "Chennai")
        {
         
            imageCounter.text = "<color=#0051A0>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
           


        }
        if (teamNameCity == "Delhi")
        {
           
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
          

        }
        if (teamNameCity == "Punjab")
        {
           
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
           


        }
        if (teamNameCity == "Lucknow")
        {
           
            imageCounter.text = "<color=#0057E1>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
           

        }
        if (teamNameCity == "Hyderabad")
        {
           
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
           

        }
        if (teamNameCity == "Mumbai")
        {
           
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
         


        }
        if (teamNameCity == "Kolkata")
        {
            
            imageCounter.text = "<color=#FAE991>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
           

        }
        if (teamNameCity == "Bangalore")
        {
            
            imageCounter.text = "<color=#E8B77F>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
            


        }
        if (teamNameCity == "Rajasthan")
        {
           
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
            

        }
        if (teamNameCity == "Gujarat")
        {
            
            imageCounter.text = "<color=#FFFFFF>" + currentTeamImages.Count.ToString() + "/" + currentTeamImages.Count.ToString() + "</color>";
          

        }

    }

    public void changeTeamIdentifer_inGame(string teamNameCity)
    {
        //this fucntion will get the currentCityname from library manager

        // we need to change the color of background of this identifier image,text & color of city name and color of counter

        //access these things from cricketJodoUImanager
        TMP_Text cityName = cricketJodoManager.Instance.teamIdentifier.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text imageCounter = cricketJodoManager.Instance.teamIdentifier.transform.GetChild(1).GetComponent<TMP_Text>();
        if (teamNameCity == "Chennai")
        {
            cityName.text = "<color=#0051A0>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#0051A0>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#FEC803";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }


        }
        if (teamNameCity == "Delhi")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#033364";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
        if (teamNameCity == "Punjab")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#E51A1C";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }


        }
        if (teamNameCity == "Lucknow")
        {
            cityName.text = "<color=#0057E1>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#0057E1>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#FFFFFF";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
        if (teamNameCity == "Hyderabad")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#F8700D";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
        if (teamNameCity == "Mumbai")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#004A9F";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }


        }
        if (teamNameCity == "Kolkata")
        {
            cityName.text = "<color=#FAE991>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FAE991>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#2D1A5C";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
        if (teamNameCity == "Bangalore")
        {
            cityName.text = "<color=#E8B77F>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#E8B77F>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#650806";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }


        }
        if (teamNameCity == "Rajasthan")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#E50595";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
        if (teamNameCity == "Gujarat")
        {
            cityName.text = "<color=#FFFFFF>" + currentCityName.ToUpper() + "</color>";
            imageCounter.text = "<color=#FFFFFF>" + (currentPlayerImageIndex + 1) + "/" + currentTeamImages.Count + "</color>";
            Debug.Log("currentPlayerImageIndex " + (currentPlayerImageIndex + 1));

            Image identifierImage = cricketJodoManager.Instance.teamIdentifier.GetComponent<Image>();
            string hexColorCode = "#0E1F34";

            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
            {
                identifierImage.color = newColor;
            }

        }
    }

    public void setColorsForIdentifer(string textColor, string boxColor)
    {

    }

   
    public void gamePreviousImage()
    {
        if (congratsImageCounter == 0)
        {
            Debug.Log("image counter is 0 so no images 553L");
            return;
        }
        else
        {
            congratsImageCounter--;
            Debug.Log("congratsImageCounter 559L " + congratsImageCounter);
            UIManager.Instance.congratsImage.GetComponent<Image>().sprite = currentTeamImages[congratsImageCounter];
        }
    }
    public void gameNextImage()
    {
        Debug.Log("current images count " + currentTeamImages.Count);
        if (congratsImageCounter == currentTeamImages.Count - 1) //4
        {
            Debug.Log("image counter is max so no images ahead 568L");
            return;
        }
        else
        {
            congratsImageCounter++;
            Debug.Log("congratsImageCounter 574L " + congratsImageCounter);
            UIManager.Instance.congratsImage.GetComponent<Image>().sprite = currentTeamImages[congratsImageCounter];
        }
    }

    public void multiNextImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            int teamSize = currentTeamImages.Count;

            if (index > teamSize - 2)
            {

                return;

            }
            else
            {

                index++;
                Launcher.instance._uploadedImage.sprite = currentTeamImages[index];
                
                Debug.Log("636 ImageIndex " + index);
            }
        }
    }
    public void multiPrevImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("667L current team images size " + currentTeamImages.Count);
            if (index == 0)
            {
                return;

            }
            else
            {
                index--;
                Launcher.instance._uploadedImage.sprite = currentTeamImages[index];
                //playerImageIndex = index; //we'll store this playerImageIndex and use this index to load up first image in game
                Debug.Log("678 ImageIndex " + index);
            }
        }
    }

    public void nextImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            int teamSize = currentTeamImages.Count;

            if (index > teamSize - 2)
            {

                return;

            }
            else
            {

                index++;
                UIManager.Instance.difficultyImage.sprite = currentTeamImages[index];
                playerImageIndex = index;
                Debug.Log("playerImageIndex " + playerImageIndex);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            int teamSize = currentTeamImages.Count;
            Debug.Log(" 530L current team images size " + teamSize);
            if (index == teamSize - 1)
            {

                return;

            }
            else
            {

                index++;
                Debug.Log("nextImage 541L LMM ");
                UIManager.Instance.congratsImage.GetComponent<Image>().sprite = currentTeamImages[index];

            }
        }
    }

    public void previousImage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("551L current team images size " + currentTeamImages.Count);
            if (index == 0)
            {
                return;

            }
            else
            {
                index--;
                UIManager.Instance.difficultyImage.sprite = currentTeamImages[index];
                playerImageIndex = index; //we'll store this playerImageIndex and use this index to load up first image in game
                Debug.Log("playerImageIndex " + playerImageIndex);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (index == 0)
            {
                return;

            }
            else
            {
                index--;
                Debug.Log("previousImage 573L LMM ");
                UIManager.Instance.congratsImage.GetComponent<Image>().sprite = currentTeamImages[index];

            }
        }
    }

 
    
    public void resetIndexesMulti()
    {
        index = 0;
        playerImageIndex = 0;
        teamNumber = 0;
        currentPlayerImageIndex = 0;
        MatchManager.Instance.jodoTeamTimer = 0;
        MatchManager.Instance.multiTeamScoreList.Clear();
        //MatchManager.Instance.jodoTeamScoreSum = 0;
    }
    public void resetMultiJodo()
    {
        index = 0;
        playerImageIndex = 0;
        teamNumber = 0;
        currentPlayerImageIndex = 0;
        MatchManager.Instance.multiTeamScoreList.Clear();
        MatchManager.Instance.jodoTeamScoreSum = 0;
        MatchManager.Instance.jodoTeamTimer = 0;
        currentCityName = "";
        currentTeamName = "";
        currentTeamImages.Clear();
    }
    //public void SetCompletedTeamsCount(int count)
    //{

    //    PlayerPrefs.SetInt("CompletedTeams", count);
    //    completedTeamsCount = count;
    //}

    //public int GetCompletedTeamsCount()
    //{
    //    completedTeamsCount = PlayerPrefs.GetInt("CompletedTeams", -1);
    //    return completedTeamsCount;
    //}

    public void LoadNextTeam()
    {
        isFirstTeam = false;

        int randomNumber = UnityEngine.Random.Range(1, 11);
        while (randomNumber == teamNumber)
        {
            randomNumber = UnityEngine.Random.Range(1, 11);

        }
        loadRandomTeam(randomNumber);
        resetIndexes();
        clearScores();
        Sprite playerSprite = currentTeamImages[playerImageIndex];
        LoadTextureImage(playerSprite);
        UIManager.Instance.StartGame();
    }
    public void ReplayTeamSet()
    {
        isFirstTeam = false;
        loadSameTeam(currentCityName);
        resetIndexes();
        Sprite playerSprite = currentTeamImages[playerImageIndex];
        LoadTextureImage(playerSprite);
        UIManager.Instance.StartGame();
    }

    public void loadSameTeam(string currentCity)
    {

        if (currentCity == "Chennai")
        {

            currentCityName = "Chennai";
            currentTeamName = "Chennai Super Kings";
            currentTeamImages = _1ChennaiPlayerImages;

        }
        if (currentCity == "Delhi")
        {

            currentCityName = "Delhi";
            currentTeamName = "Delhi Capitals";
            currentTeamImages = _2DelhiPlayerImages;
        }
        if (currentCity == "Gujarat")
        {

            currentCityName = "Gujarat";
            currentTeamName = "Gujarat Titans";
            currentTeamImages = _3GujaratPlayerImages;
        }
        if (currentCity == "Kolkata")
        {

            currentCityName = "Kolkata";
            currentTeamName = "Kolkata Knight Riders";
            currentTeamImages = _4KolkataPlayerImages;
        }
        if (currentCity == "Lucknow")
        {

            currentCityName = "Lucknow";
            currentTeamName = "Lucknow Super Giants";
            currentTeamImages = _5LucknowPlayerImages;
        }
        if (currentCity == "Mumbai")
        {

            currentCityName = "Mumbai";
            currentTeamName = "Mumbai Indians";
            currentTeamImages = _6MumbaiPlayerImages;
        }
        if (currentCity == "Punjab")
        {

            currentCityName = "Punjab";
            currentTeamName = "Punjab Kings";
            currentTeamImages = _7PunjabPlayerImages;
        }
        if (currentCity == "Bangalore")
        {

            currentCityName = "Bangalore";
            currentTeamName = "Royal Challengers Bangalore";
            currentTeamImages = _8BangalorePlayerImages;
        }
        if (currentCity == "Rajasthan")
        {

            currentCityName = "Rajasthan";
            currentTeamName = "Rajasthan Royals";
            currentTeamImages = _9RajasthanPlayerImages;
        }
        if (currentCity == "Hyderabad")
        {

            currentCityName = "Hyderabad";
            currentTeamName = "Sunrisers Hyderabad";
            currentTeamImages = _10HyderabadPlayerImages;
        }
    }
    public void loadRandomTeam(int num)
    {
        if (num == 1)
        {
            teamNumber = 1;
            currentCityName = "Chennai";
            currentTeamName = "Chennai Super Kings";
            currentTeamImages = _1ChennaiPlayerImages;

        }
        if (num == 2)
        {
            teamNumber = 2;
            currentCityName = "Delhi";
            currentTeamName = "Delhi Capitals";
            currentTeamImages = _2DelhiPlayerImages;
        }
        if (num == 3)
        {
            teamNumber = 3;
            currentCityName = "Gujarat";
            currentTeamName = "Gujarat Titans";
            currentTeamImages = _3GujaratPlayerImages;
        }
        if (num == 4)
        {
            teamNumber = 4;
            currentCityName = "Kolkata";
            currentTeamName = "Kolkata Knight Riders";
            currentTeamImages = _4KolkataPlayerImages;
        }
        if (num == 5)
        {
            teamNumber = 5;
            currentCityName = "Lucknow";
            currentTeamName = "Lucknow Super Giants";
            currentTeamImages = _5LucknowPlayerImages;
        }
        if (num == 6)
        {
            teamNumber = 6;
            currentCityName = "Mumbai";
            currentTeamName = "Mumbai Indians";
            currentTeamImages = _6MumbaiPlayerImages;
        }
        if (num == 7)
        {
            teamNumber = 7;
            currentCityName = "Punjab";
            currentTeamName = "Punjab Kings";
            currentTeamImages = _7PunjabPlayerImages;
        }
        if (num == 8)
        {
            teamNumber = 8;
            currentCityName = "Bangalore";
            currentTeamName = "Royal Challengers Bangalore";
            currentTeamImages = _8BangalorePlayerImages;
        }
        if (num == 9)
        {
            teamNumber = 9;
            currentCityName = "Rajasthan";
            currentTeamName = "Rajasthan Royals";
            currentTeamImages = _9RajasthanPlayerImages;
        }
        if (num == 10)
        {
            teamNumber = 10;
            currentCityName = "Hyderabad";
            currentTeamName = "Sunrisers Hyderabad";
            currentTeamImages = _10HyderabadPlayerImages;
        }


    }

    public void resetIndexes()
    {
        index = 0;
        playerImageIndex = 0;
        inGamePlayerSpriteIndex = -1;
        currentPlayerImageIndex = 0;
        congratsImageCounter = 0;



    }
    public void clearScores()
    {
        MatchManager.Instance.jodoTeamScoreSum = 0;
        MatchManager.Instance.jodoTeamTimer = 0;
        MatchManager.Instance.teamScoreList.Clear();

    }

    public void resetLibraryMatchManager()
    {
        index = 0;
        playerImageIndex = 0;
        congratsImageCounter = 0;
        // MatchManager.Instance.teamScoreList.Clear();
        //currentTeamImages.Clear();
        inGamePlayerSprites.Clear();
        inGamePlayerSpriteIndex = -1;
        currentPlayerImageIndex = 0;
        isFirstTeam = true;
        MatchManager.Instance.jodoType = 0; //set back to normal mode
        //cricketJodoManager.Instance.deactivateJodoUI();
        currentCityName = "";
        currentTeamName = "";
        teamNumber = 0;
        MatchManager.Instance.jodoTeamScoreSum = 0;
        MatchManager.Instance.jodoTeamTimer = 0;
        MatchManager.Instance.teamScoreList.Clear();
        teamScoreDataList.Clear();
        completedTeamsCount = 0;
        MatchManager.Instance.difficultystr = "Easy";
        leaderboardDifficulty = "Easy";
        //cricketJodoManager.Instance.completedTeams.Clear();


        ////UIManager.Instance.uploadPanel.SetActive(true);

        //GameObject uploadImageGO = GameObject.Find("UploadPanel");
        //uploadImageGO.SetActive(true);
        //uploadImage = uploadImageGO.GetComponent<UploadImage>();
        //uploadImageGO.SetActive(false);



    }
    public float GetHighScoreJodo(float highScoreTimeJodo)
    {
        //first we recieve the list
        Debug.Log("107L JodoScoreKeeper GetHighScore " + highScoreTimeJodo);
        //List<TeamScoreData> recievedList = LibraryMatchManager.Instance.GetHighScoreInData(LibraryMatchManager.Instance.teamNumber, MatchManager.Instance.difficultystr);

        //if (recievedList == null || recievedList.Count == 0)
        if (teamScoreDataList.Count != 0)
        {
           
            foreach(TeamScoreData _teamData in teamScoreDataList )
            {
                if (_teamData.teamNumber == teamNumber && _teamData.gameDifficulty == MatchManager.Instance.difficultystr)
                {
                    Debug.Log("895 teamScore[0] " + _teamData.scores[0]);
                    //if both same
                    return _teamData.scores[0]; 
                    //return the first score on list as its a high score

                }
                

            }
              
        }
        else
        {
            //return 0f;
            return 0f;
        }
        //now we run a for loop to look for a object having same teamNumber and difficulty string

        Debug.Log("92L gethighscore highScoretime " + highScoreTimeJodo + " jodo score manager");
        return 0f;
    }

    public void setHighscoreInData(float highScoredata)
    {
        Debug.Log("929L teamListDataCount " + teamScoreDataList.Count);

        if (teamScoreDataList.Count != 0)
        {
            bool teamFound = false;
            bool diffFound = false;

            Debug.Log("933L LMM in setHighScoredata ");
            //when we get a element from the list
            
            foreach(TeamScoreData _teamScoreData in teamScoreDataList)
            {
                
                if(_teamScoreData.teamNumber == teamNumber && _teamScoreData.gameDifficulty == MatchManager.Instance.difficultystr)
                {
                    

                    teamFound = true;
                    diffFound = true;
                    //when diff equal, add scores in same list 

                        Debug.Log("942L LMM in setHighScoredata diff and team number is there ");
                        _teamScoreData.scores.Sort();

                        if(_teamScoreData.scores.Count == 0 || _teamScoreData.scores.Count < 5)
                        {
                            //if there are less than 5 elements in array, just add the high score data
                            Debug.Log("948L LMM in setHighScoredata adding scores in list ");
                            _teamScoreData.scores.Add(highScoredata);


                        }
                        else
                        {
                            if(_teamScoreData.scores[4] > highScoredata)
                            {
                                Debug.Log("957L LMM in setHighScoredata adding at last of list ");
                                _teamScoreData.scores[4] = highScoredata;
                            }
                           
                        }
                        _teamScoreData.scores.Sort(); //sort it again
                        

                    //}
                    //if(diffFound == false)
                    //{
                    //    TeamScoreData teamScoreDataNew = new TeamScoreData((int)teamNumber, (string)MatchManager.Instance.difficultystr, (bool)true, new List<float>(5));
                    //    teamScoreDataNew.scores.Add(highScoredata); //add the score we've gotten
                    //    teamScoreDataNew.scores.Sort();
                    //    Debug.Log("971L LMM in setHighScoredata creating a new list and adding, team is there but not difficulty");
                    //    teamScoreDataList.Add(teamScoreDataNew); // now add it to the list
                    //}
                    break;
                    //as we have found the team and we dont have to parse further

                    

                }
                
                
                
            }
            
            if(teamFound == false && diffFound == false)
            {
                //if even after parsing we have not found the team, now we create one new object for that teamNumber

                TeamScoreData teamScoreDataNew = new TeamScoreData((int)teamNumber, (string)MatchManager.Instance.difficultystr, (bool)true, new List<float>(5));
                teamScoreDataNew.scores.Add(highScoredata); //add the score we've gotten
                teamScoreDataNew.scores.Sort();
                Debug.Log("1000L LMM in setHighScoredata creating a new list and adding, team is there but not difficulty");
                teamScoreDataList.Add(teamScoreDataNew); // now add it to the list



            }

            
            //when we parse through all objects and not able to find that object with same team number, now we create a new one for that



            //for (int i = 0; i < teamScoreDataList.Count; i++)
            //{
            //    if (teamScoreDataList[i].teamNumber == teamNumber)
            //    {
            //        if (teamScoreDataList[i].gameDifficulty == MatchManager.Instance.difficultystr)
            //        {
            //            Debug.Log("840L LMM in setHighScoredata diff and team number is there ");
            //            //if there's an object that has same teamNumber and gameDifficulty

            //            teamScoreDataList[i].scores.Sort(); //sort it out first
            //            if (teamScoreDataList[i].scores.Count == 0 || teamScoreDataList[i].scores.Count < 5)
            //            {
            //                //if there is 0 or less than 5 elements in the array, just add the highScoreData
            //                Debug.Log("847L LMM in setHighScoredata adding scores in list ");
            //                teamScoreDataList[i].scores.Add(highScoredata);
                           

            //            }

            //            else
            //            {
            //                if (teamScoreDataList[i].scores[4] > highScoredata)
            //                {
            //                    Debug.Log("856L LMM in setHighScoredata adding at last of list ");
            //                    teamScoreDataList[i].scores[4] = highScoredata; //set the score value we've received to that value
                             
            //                }

            //            }
            //            //sort list again
            //            teamScoreDataList[i].scores.Sort();
                      
            //        }

            //        else if (teamScoreDataList[i].gameDifficulty != MatchManager.Instance.difficultystr && teamScoreDataList[i].gameDifficulty != "" && teamScoreDataList[i].gameDifficulty != null)
            //        {
            //            //when teamNumber is same but just for different difficulty
            //            //for this we need to create a new oject with matchmanager difficulty string and add it to teamScoreDataList
            //            //this object will have data with teamNumber, matchmanager difficulty string , isCompleted set as true, and a new list
            //            TeamScoreData _teamScoreData = new TeamScoreData((int)teamNumber, (string)MatchManager.Instance.difficultystr, (bool)true, new List<float>(5));
            //            _teamScoreData.scores.Add(highScoredata); //add the score we've gotten
            //            _teamScoreData.scores.Sort();
            //            Debug.Log("872L LMM in setHighScoredata creating a new list and adding, team is there but not difficulty");
            //            teamScoreDataList.Add(_teamScoreData); // now add it to the list
                        
                       

            //        }

            //    }
            //    else
            //    {
            //        //when team number is not equal to that one present in teamScoreDataList
            //        //we just create one object for it and add it to list

            //        TeamScoreData _teamScoreData = new TeamScoreData((int)teamNumber, (string)MatchManager.Instance.difficultystr, true, new List<float>(5));
            //        _teamScoreData.scores.Add(highScoredata); //add the score to this 
            //        _teamScoreData.scores.Sort();
            //        Debug.Log("989L LMM in setHighScoredata creating a new list and adding ");

            //        teamScoreDataList.Add(_teamScoreData); //add it to list
            //        break;

            //    }

            //}





        }

        else //there is no object for that teamNumber so we create one now
        {
            //CREATE  OBJECT and set isCompleted true
            TeamScoreData teamScoreDataNew = new TeamScoreData((int)teamNumber, (string)MatchManager.Instance.difficultystr, true, new List<float>(5));
           teamScoreDataNew.scores.Add(highScoredata); //add the score to this 
            teamScoreDataNew.scores.Sort();
            Debug.Log("893L LMM in setHighScoredata creating a new list and adding ");
           
            teamScoreDataList.Add(teamScoreDataNew); //add it to list




        }
        Debug.Log("900L LMM in setHighScoredata updating json file ");
       
        //here we save the content now
        FileHandler.SaveToJSON<TeamScoreData>(teamScoreDataList,"teamScoreData.json");
       
      
    }


    public void showScoreOnScreen(int _teamNum, string _ledrDiff)
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {

        displayLineForLeaderboard(_teamNum);
        }

        //this function will fetch the score from teamScoreDataList and show it on screen in the 
       foreach(TeamScoreData _teamScoreData in teamScoreDataList)
        {
            if(_teamScoreData.teamNumber == _teamNum && _teamScoreData.gameDifficulty == _ledrDiff)
            {
                //if team number and difficulty matches then we use the array of tmp text we got in cricketJodoUiManager and input scores in them

                for(int i = 0; i < _teamScoreData.scores.Count; i++)
                {
                    int minutes = Mathf.FloorToInt(_teamScoreData.scores[i] / 60f); //this is the minutes we calculating out of jodoTeamScoreSum
                    int seconds = Mathf.FloorToInt(_teamScoreData.scores[i] % 60f);//this is the second we calculating out of jodoTeamScoreSum

                    if(minutes == 0)
                    {
                        if (seconds <= 9)
                        {
                            seconds = 00 + seconds;
                        cricketJodoManager.Instance.scoreToDisplay[i].GetComponent<TMP_Text>().text = string.Format("0{0}s", seconds);
                        }
                        else
                        {
                            cricketJodoManager.Instance.scoreToDisplay[i].GetComponent<TMP_Text>().text = string.Format("{0}s", seconds);
                        }
                    }
                    else
                    {
                        if (seconds <= 9)
                        {
                            seconds = 00 + seconds;
                        cricketJodoManager.Instance.scoreToDisplay[i].GetComponent<TMP_Text>().text = string.Format("{0}m 0{1}s", minutes, seconds);
                        }
                        else
                        {
                            cricketJodoManager.Instance.scoreToDisplay[i].GetComponent<TMP_Text>().text = string.Format("{0}m {1}s", minutes, seconds);
                        }
                    }
                }

            }
        }

    }

    public void displayLineForLeaderboard(int _teamNo)
    {

        string puzzleType = "";

        if(MatchManager.Instance.difficultystr == "Easy")
        {
            puzzleType = "9";

        }
        else
        {
            puzzleType = "16";
        }
        string teamName = determineTeamName(_teamNo);
        Debug.Log("1143L");
        cricketJodoManager.Instance.gameBoardInfo.GetComponent<TMP_Text>().text = "Your top 5 five completion time for " + teamName + " for " + puzzleType + " pieces jigsaw";
    }

    public string determineTeamName(int _teamNumb)
    {
        if (_teamNumb == 1)
        {
            
            return "Chennai Super Kings";
            

        }
        if (_teamNumb == 2)
        {

            return "Delhi Capitals";
            
        }
        if (_teamNumb == 3)
        {

            return "Gujarat Titans";
           
        }
        if (_teamNumb == 4)
        {

            return "Kolkata Knight Riders";
            
        }
        if (_teamNumb == 5)
        {

            return "Lucknow Super Giants";
           
        }
        if (_teamNumb == 6)
        {

            return "Mumbai Indians";
            
        }
        if (_teamNumb == 7)
        {

            return "Punjab Kings";
           
        }
        if (_teamNumb == 8)
        {

            return "Royal Challengers Bangalore";
            
        }
        if (_teamNumb == 9)
        {

            return "Rajasthan Royals";
            
        }
        if (_teamNumb == 10)
        {

            return "Sunrisers Hyderabad";
           
        }
        return "";
    }
    public List<TeamScoreData> GetHighScoreInData(int _teamNumber, string _gameDiff)
    {
        return teamScoreDataList;
        //    try
        //    {
        //        string jsonRead = System.IO.File.ReadAllText(Application.persistentDataPath + "/teamScoreData.json");
        //        teamScoreDataList = JsonUtility.FromJson<List<TeamScoreData>>(jsonRead); //read the list

        //        for(int i = 0; i < teamScoreDataList.Count; i++)
        //        {
        //            //parse through all the elements of list and get the scores according to the teamNumber and gameDifficulty
        //            if(teamScoreDataList[i].gameDifficulty == _gameDiff && teamScoreDataList[i].teamNumber == _teamNumber)
        //            {
        //                Debug.Log("913L LMM getHighScoreData ");
        //                teamScoreList = teamScoreDataList;
        //                return teamScoreDataList;


        //            }
        //            else if((teamScoreDataList[i].gameDifficulty != MatchManager.Instance.difficultystr && teamScoreDataList[i].gameDifficulty != "" && teamScoreDataList[i].gameDifficulty != null) && teamScoreDataList[i].teamNumber == _teamNumber)
        //            {

        //                //when team is there but for different difficulty, so create a new object in list for same team with different difficulty
        //                TeamScoreData _teamScoreData = new TeamScoreData((int)_teamNumber, (string)_gameDiff, (bool)true, new List<float>(5));
        //                teamScoreDataList.Add(_teamScoreData);
        //                teamScoreList = teamScoreDataList;
        //                Debug.Log("924L LMM getHighScoreData ");
        //                return teamScoreDataList;


        //            }
        //            else if(teamScoreDataList[i].teamNumber != _teamNumber)
        //            {
        //                Debug.Log("931L LMM returning null getHighScoredata ");
        //                teamScoreList = teamScoreDataList;
        //                //when the team is not there, that means there is no data for that team also, neither for easy nor for hard diff
        //                return null;
        //            }
        //        }

        //        return null;
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.Log("no team record set yet" + e);
        //        teamScoreList = teamScoreDataList;
        //        return null;
        //    }
        //}

        



    }

   
    
}
[Serializable]
public class TeamScoreData
{
    public int teamNumber;
    public string gameDifficulty; //easy or hard
    public bool isCompleted;   // false for no, true for yes    
    public List<float> scores; //to score all list
    public TeamScoreData(int _teamNumber, string _gameDifficulty, bool _isCompleted, List<float> _score)
    {
        teamNumber = _teamNumber;
        gameDifficulty = _gameDifficulty;
        isCompleted = _isCompleted;
        scores = _score;
    }
}

