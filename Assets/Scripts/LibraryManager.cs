using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using ImageCropperNamespace;
using TMPro;

public class LibraryManager : MonoBehaviour
{
    [SerializeField] GameObject rowPanel;
    public List<GameObject> rowPanels = new List<GameObject>();
    string imagePath;
    byte[] imageData = null;
    [SerializeField] List<Sprite> playerImages = new List<Sprite>();
    [SerializeField] UploadImage uploadImage;
    [SerializeField] List<string> playerNames = new List<string>();
    int index;
    void Start()
    {
        index = 0;
        StartCoroutine(SetLibrary());
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
            playerImages.Add(playerImage);
            if(image == 17 || image == 18)
            {
                Debug.Log(playerImage.name +" "+ playerImage.texture.width+" "+playerImage.texture.height);
                Debug.Log(imageData.Length);
            }

        }
        for (int i = 0; i < 17; i++)
        {
            Debug.Log("from here");
            GameObject instance = Instantiate(rowPanel, rowPanel.transform.parent);
            instance.SetActive(true);
            rowPanels.Add(instance);
            for (int j = 0; j < 3; j++)
            {
                
                rowPanels[i].transform.GetChild(j).transform.GetChild(2).GetComponent<Image>().sprite = playerImages[index];
                if(index >= 16)
                {
                    rowPanels[i].transform.GetChild(j).transform.GetChild(1).GetComponent<TMP_Text>().text = playerNames[index+1];
                }
                else
                {
                    rowPanels[i].transform.GetChild(j).transform.GetChild(1).GetComponent<TMP_Text>().text = playerNames[index];
                }
                
                index++;

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UploadLibraryImage(Image image)
    {
       uploadImage.CallLoadImageCoroutine(image.sprite.texture);
    }
}
