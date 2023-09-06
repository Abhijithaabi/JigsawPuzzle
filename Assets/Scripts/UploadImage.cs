using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEngine.Android;
using UnityEngine.Rendering;
using System.IO;

namespace ImageCropperNamespace
{
    public class UploadImage : MonoBehaviour
{
    //public Button button;
    public string FinalPath;

    public Image myButtonImage;
        [SerializeField] public List<Sprite> teamSprites;
    public Sprite newSprite;

    public PuzzleSelector inst1;


    public void LoadFile()
    {
            cricketJodoManager.Instance.deactivateJodoUI();
            
            if(MatchManager.Instance.jodoType == 0)
            {


        string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpeg,jpg");
        int maxSize = 512;
        #if UNITY_ANDROID
            NativeGallery.GetImageFromGallery( ( path ) =>
                {
                 if( path != null )
                     {
                
                        Texture2D pickedImage = NativeGallery.LoadImageAtPath( path, maxSize);
                        //Texture2D imageTexture = newSprite.texture;
                        UIManager.Instance.PlayTransitionEffect();
                        
                        Cropping(pickedImage);
                        
                        
                    }
                });
        #endif
            }
            else if(MatchManager.Instance.jodoType == 1 && MatchManager.Instance.gameMode == 1) //cricketJodo
            {



                string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpeg,jpg");
                int maxSize = 512;
#if UNITY_ANDROID
                NativeGallery.GetImageFromGallery((path) =>
                {
                    if (path != null)
                    {

                        Texture2D pickedImage = NativeGallery.LoadImageAtPath(path, maxSize);
                        //Texture2D imageTexture = newSprite.texture;
                        UIManager.Instance.PlayTransitionEffect();

                        Cropping(pickedImage);


                    }
                });
#endif

            }

        }

    public IEnumerator LoadImage(Texture2D _texture)
    {
        
        yield return new WaitForSeconds(0.01f);
        Texture2D texture = _texture;

            
            if(texture != null)
            {
                Debug.Log("Not Null");
            }

           
           
            newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        float x = 1573.7025f / texture.width;
        float y = 1563.381f / texture.height;
        float ninex = 1176.4278f / texture.width;
        float niney = 1185.2622f / texture.height;

        Debug.Log("Float x,y "+ x+","+y+","+ninex+","+niney);
            UIManager.Instance.ChooseDifficulty();
            
            
            myButtonImage.sprite = newSprite;
            
            MatchManager.Instance.SetUploadedImageData(newSprite,x,y,ninex,niney);
            
            

            Debug.Log("Sprite Rect: " +newSprite.rect);
            
            inst1 = GetComponent<PuzzleSelector>();
            
    }
    public void CallLoadImageCoroutine(Texture2D _texture)
    {
            if(MatchManager.Instance.jodoType == 0)
            {
        Cropping(_texture);

            }
    }

    public void TakePicture()
    {
            cricketJodoManager.Instance.deactivateJodoUI();

        int maxSize = 512;
        
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
	    {
            UIManager.Instance.PlayTransitionEffect();
		Debug.Log( "Image path: " + path );
		if( path != null )
		{
			
			Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
			if( texture != null )
			{
                
				
                Cropping(texture);
                
			}
        }
        },maxSize);
    }
    private void Cropping(Texture2D myTexture)
        {
            bool autoZoom = true;
 
            ImageCropper.Instance.Show(myTexture, (bool result, Texture originalImage, Texture2D croppedImage) => {
 
                if (result)
                {
                    StartCoroutine(LoadImage(croppedImage));
                    
                }
            },
            settings: new ImageCropper.Settings()
            {
                autoZoomEnabled = autoZoom,
                imageBackground = Color.clear, 
                selectionMinAspectRatio = 1,
                selectionMaxAspectRatio = 1
                
 
            },
            croppedImageResizePolicy: (ref int width, ref int height) =>
            {
              
                width = 512;
                height = 512;
            });
        }
}
}


