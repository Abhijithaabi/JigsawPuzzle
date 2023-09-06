using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject PlayScreen;
    public GameObject UploadScreen;
    public GameObject DifficultyScreen;
    public Image imageUploaded;
    public Sprite uploadedSprite;

     public string FinalPath;
     

    private Sprite newSprite;
    public GameObject easy9x;
    public GameObject medium16x;
    
   


    public void PlayClick(){
        PlayScreen.SetActive(false);
        UploadScreen.SetActive(true);
    }

    public void UploadClick(){
        DifficultyScreen.SetActive(true);
        LoadFile();
    }

    public void LoadFile()
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpeg,jpg");

        // NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        // {
        //     if (path == null)
        //     {
        //         Debug.Log("Operations Cancelled");

        //     }
        //     else
        //     {
        //         FinalPath = path;
        //         Debug.Log("Picked Files: " + FinalPath);
        //         StartCoroutine("LoadImage");
        //     }
        // }, new string[] { FileType });
        // Debug.Log( "Permission result: " + permission );
        #if UNITY_ANDROID
            NativeGallery.GetImageFromGallery( ( path ) =>
                {
                 if( path != null )
                     {
                // Create Texture from selected image
                        Texture2D pickedImage = NativeGallery.LoadImageAtPath( path, 1024 );
                        StartCoroutine(LoadImage(pickedImage));
                    }
                });
        #endif
    }

public IEnumerator LoadImage(Texture2D _texture)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + FinalPath);
        yield return uwr.SendWebRequest();
        Texture2D texture = _texture;

            //Texture2D texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
            if(texture != null)
            {
                Debug.Log("Not Null");
            }

            Texture2D temp = new Texture2D(1024,1024,TextureFormat.RGB24,false);
            // temp.SetPixels(texture.GetPixels(0));
            // temp.Apply();
            // byte[] png = temp.EncodeToPNG();
            // Destroy(temp);
            // texture.LoadImage(png);
            // texture.Reinitialize(1024,1024,TextureFormat.RGBAFloat,false);
            // texture.Apply();

            newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //newSprite = Sprite.Create(texture, new Rect(0, 0, 933f, 798f), new Vector2(0.5f, 0.5f));
            uploadedSprite = newSprite;
            GameObject myButton = GameObject.FindWithTag("UploadedImage");
            float x = 1080.0f/texture.width;
            float y = 1080.0f/texture.height;
            Debug.Log("Float x,y "+ x+","+y);
            
            imageUploaded = myButton.GetComponent<Image>();
            imageUploaded.sprite = newSprite;
            
            

            Debug.Log("Sprite Rect: " +newSprite.rect);
    }
    // Start is called before the first frame update

    public void easyDiff(){
        medium16x.SetActive(false);
        easy9x.SetActive(true);

    }

    public void mediumDiff(){
        easy9x.SetActive(false);
        medium16x.SetActive(true);
    }

    public void gamePlayButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void Back(){
        DifficultyScreen.SetActive(false);
        easy9x.SetActive(false);
        medium16x.SetActive(false);
        
    }
    void Start()
    {
        UploadScreen.SetActive(false);
        DifficultyScreen.SetActive(false);
        easy9x.SetActive(false);
        medium16x.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
