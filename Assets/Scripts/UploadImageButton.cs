using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;
using UnityEngine.Rendering;
using System.IO;

public class UploadImageButton : MonoBehaviour
{
    // public Button button;
    public string FinalPath;

    public GameObject DifficultyScreen;
    public Image myButtonImage;
    public Sprite newSprite;

    public Sprite savedSprite;

    // public PuzzleSelector inst1;


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
            
            GameObject myButton = GameObject.FindWithTag("UploadedImage");
            float x = 1080.0f/texture.width;
            float y = 1080.0f/texture.height;
            Debug.Log("Float x,y "+ x+","+y);
            
            myButtonImage = myButton.GetComponent<Image>();
            myButtonImage.sprite = newSprite;
            
            

            Debug.Log("Sprite Rect: " +newSprite.rect);
            
            // inst1 = GetComponent<PuzzleSelector>();
            // inst1.SetPuzzlePhoto(newSprite,x,y);

        // if (uwr.result != UnityWebRequest.Result.Success)
        // {
        //     Debug.LogError(uwr.error);

        // }
        // else
        // {
        //     Texture2D texture = _texture;

        //     //Texture2D texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
        //     if(texture != null)
        //     {
        //         Debug.Log("Not Null");
        //     }

        //     Texture2D temp = new Texture2D(1024,1024,TextureFormat.RGB24,false);
        //     // temp.SetPixels(texture.GetPixels(0));
        //     // temp.Apply();
        //     // byte[] png = temp.EncodeToPNG();
        //     // Destroy(temp);
        //     // texture.LoadImage(png);
        //     // texture.Reinitialize(1024,1024,TextureFormat.RGBAFloat,false);
        //     // texture.Apply();
        //     newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //     //newSprite = Sprite.Create(texture, new Rect(0, 0, 933f, 798f), new Vector2(0.5f, 0.5f));
        //     GameObject myButton = GameObject.FindWithTag("UploadedImage");
        //     myButtonImage = myButton.GetComponent<Image>();
        //     myButtonImage.sprite = newSprite;
        //     Debug.Log("Sprite Rect: " +newSprite.rect);
            
        //     inst1 = GetComponent<PuzzleSelector>();
        //     inst1.SetPuzzlePhoto(newSprite);
        // }
    }
    void Start()
    {

        
        // if (button == null)
        // {
        //     Debug.LogError("Button object is not assigned in the inspector");
        //     return;
        // }
        // button.onClick.AddListener(LoadFile);
    }
    private void ButtonClicked()
    {
        Debug.Log("Button was clicked!");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
