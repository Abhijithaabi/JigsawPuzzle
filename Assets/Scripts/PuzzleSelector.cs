using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using ImageCropperNamespace;
public class PuzzleSelector : MonoBehaviour
{
    

    private UploadImage inst1;
    [SerializeField] Image preview;
    float x;
    float y;

    void Start()
    {

        Sprite uplaodedImage = MatchManager.Instance.uploadedImage;
        if(MatchManager.Instance.difficultystr.Equals("Easy"))
        {
            x = MatchManager.Instance.scale9X;
            y = MatchManager.Instance.scale9Y;
        }
        else if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            x = MatchManager.Instance.scale16X;
            y= MatchManager.Instance.scale16Y;
        }
        
        SetPuzzlePhoto(uplaodedImage,x,y);
        preview.sprite = uplaodedImage;
        
    }

   
    public void SetPuzzlePhoto(Sprite imageSprite,float scaleX,float scaleY)
    {
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            for (int i = 0; i < 16; i++)
            {

            //GameObject.Find("Piece (" + i + ")").transform.Find("Puzzle").GetComponent<SpriteRenderer>().sprite = imageSprite;
            Transform instance = GameObject.Find("Piece (" + i + ")").transform.Find("Puzzle");
            Transform Animinstance = GameObject.Find("AnimPiece (" + i + ")").transform.Find("Puzzle");
                Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                //Transform TransPrefabInstance = GameObject.Find("PuzzleTrans").transform;
                instance.GetComponent<SpriteRenderer>().sprite = imageSprite;
            instance.transform.localScale = new Vector3(scaleX,scaleY,1f);
            Animinstance.GetComponent<SpriteRenderer>().sprite = imageSprite;
            Animinstance.transform.localScale = new Vector3(scaleX,scaleY,1f);
            TransPrefabInstance.GetComponent<SpriteRenderer>().sprite = imageSprite;
            TransPrefabInstance.transform.localScale = new Vector3(scaleX, scaleY, 1f);

                Debug.Log("the 16x sprite has been set");
            }
        }
        else if (MatchManager.Instance.difficultystr.Equals("Easy"))
        {
            for (int i = 0; i < 9; i++)
            {

            //GameObject.Find("Piece (" + i + ")").transform.Find("Puzzle").GetComponent<SpriteRenderer>().sprite = imageSprite;
            Transform instance = GameObject.Find("Piece (" + i + ")").transform.Find("Puzzle");
            Transform Animinstance = GameObject.Find("AnimPiece (" + i + ")").transform.Find("Puzzle");
                Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                instance.GetComponent<SpriteRenderer>().sprite = imageSprite;
            instance.transform.localScale = new Vector3(scaleX,scaleY,1f);
            Animinstance.GetComponent<SpriteRenderer>().sprite = imageSprite;
            Animinstance.transform.localScale = new Vector3(scaleX,scaleY,1f);
                TransPrefabInstance.GetComponent<SpriteRenderer>().sprite = imageSprite;
                TransPrefabInstance.transform.localScale = new Vector3(scaleX, scaleY, 1f);

                Debug.Log("the 9x sprite has been set");
            }
        }

    }
    public void Back()
    {
        // for(int i = 0; i < 16; i++){
        //     GameObject piece = GameObject.Find("Piece ("+i+")");
        //     piece.transform.position = piece.GetComponent<PiecesScript>().RightPosition;
        //     piece.GetComponent<PiecesScript>().InRighPosition = false;
        //     piece.GetComponent<PiecesScript>().Selected = false;
        //     piece.GetComponent<SortingGroup>().sortingOrder = 0;
        // }
        //StartPanel.SetActive(true);
    }

     // void Start()
    // {
    //     List<GameObject> existingPuzzlePieces = new List<GameObject>();
    //     for (int i = 0; i < 16; i++)
    //     {
    //         existingPuzzlePieces.Add((GameObject.Find("Piece (" + i + ")"))); 
    //     }
    //      for (int i = 0; i < existingPuzzlePieces.Count; i++)
    //     {
    //         Destroy(existingPuzzlePieces[i]);
    //     }


    //     existingPuzzlePieces = GameObject.FindGameObjectsWithTag("Piece (" + i + ")");
    //     for (int i = 0; i < existingPuzzlePieces.Length; i++)
    //     {
    //         Destroy(existingPuzzlePieces[i]);
    //     }

    // }
}

/*This code defines a public variable "StartPanel" of type "GameObject" and a public method "SetPuzzlePhoto" which takes in one parameter of type "Image".

The method is iterating through 36 iterations and in each iteration, it is using the GameObject.Find("Piece ("+i+")") method to find a game object with the name "Piece (i)" where i is the current iteration number. It then uses the transform.Find("Puzzle") method to find a child object with the name "Puzzle" of the found game object.
Finally, it is using the GetComponent<SpriteRenderer>().sprite to get the sprite renderer component of the child object and then assigns the sprite of the passed in "Photo" object to the child object's sprite renderer component.

After the for loop, it is using the StartPanel.SetActive(false) to set the "StartPanel" game object inactive, likely hiding it from the scene.

So this code is likely used to change the puzzle pieces' sprites to the one passed in the "Image" object, and also hide the "StartPanel" game object after the sprites are set.*/