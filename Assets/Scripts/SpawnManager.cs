using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] Pieces;
    [SerializeField] GameObject[] Transparent;
    [SerializeField] GameObject[] Zigzag_Pieces;
    [SerializeField] GameObject[] Zigzag_Transparent;
    [SerializeField] Image hintHolder;
    [SerializeField] GameObject AnimObject;
    [SerializeField] GameObject ZigZag_AnimObject;
    public List<Vector2> spawnPoints = new List<Vector2>();
    GameObject PiecesPrefab;
    GameObject randomTransparentPiece;
    int randomIndex;
    int randomChoice;

    Transform previewImage;
    GameObject previewImageObject;
    GameObject previewSpriteObject;


    public bool isPreview = false;

    void Awake()
    {
        if(MatchManager.Instance.isOnTutorial)return;
        AnimObject.SetActive(false);
        ZigZag_AnimObject.SetActive(false);

        if (MatchManager.Instance.jodoType == 0)
        {

            //randomChoice = Random.Range(0, 2); //random choice - 0 = normal piece, 1- zigzag piece
            randomChoice = 1;
        }
        else
        {
            randomChoice = 0; //normal type of pieces for cricket jodo feature 
        }
        
        if(randomChoice == 0)
        {//normal piece spawn
            AnimObject.SetActive(true); //animation starts
            randomIndex = Random.Range(0, Pieces.Length);
            InstantiateRandomPiece(randomChoice, randomIndex); //now it depends on random choice too
        }
        else if(randomChoice == 1)
        {
            //zigzag piece spawn
            ZigZag_AnimObject.SetActive(true);
            randomIndex = Random.Range(0, Zigzag_Pieces.Length);
            InstantiateRandomPiece(randomChoice, randomIndex); //now it depends on random choice too
        }
        
        
    }
    void Start()
    {
        if(MatchManager.Instance.isOnTutorial)return;
        if(randomChoice == 0)
        {

        randomTransparentPiece = Transparent[randomIndex]; //normal transparent
        Instantiate(randomTransparentPiece,randomTransparentPiece.transform.position,randomTransparentPiece.transform.rotation);

        }
        else if(randomChoice == 1)
        {
            randomTransparentPiece = Zigzag_Transparent[randomIndex]; //zigzag transparent
            Instantiate(randomTransparentPiece, randomTransparentPiece.transform.position, randomTransparentPiece.transform.rotation);
        }

        if (MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            for (int i = 0; i < 16; i++)
            {

                Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;
                Transform TransPrefabBorderInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("isRight");
                TransPrefabBorderInstance.GetComponent<SpriteRenderer>().enabled = false;



            }


            //Transform TransPrefabInstance = GameObject.Find("PuzzleTrans").transform;
            //TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;
           // Transform TransPrefabBorderInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("isRight");
          //  TransPrefabBorderInstance.GetComponent<SpriteRenderer>().enabled = false;

            //Transform TransPrefabInstance = GameObject.Find("PuzzleTrans").transform;
            //TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;
            // Transform TransPrefabBorderInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("isRight");
            //  TransPrefabBorderInstance.GetComponent<SpriteRenderer>().enabled = false;

        }
        else if (MatchManager.Instance.difficultystr.Equals("Easy"))
        {
            for (int i = 0; i < 9; i++)
            {

                Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;

            }


        }

        PiecesPrefab = GameObject.FindWithTag("PiecesPrefab");
        StartCoroutine(HideAnim());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InstantiateRandomPiece(int randChoice ,int randIndex)
    {
        if(randChoice == 0)
        {

        GameObject randomPiece = Pieces[randIndex];
        
        Instantiate(randomPiece,randomPiece.transform.position,randomPiece.transform.rotation);
        }
        else
        {
            GameObject randomPiece = Zigzag_Pieces[randIndex];

            Instantiate(randomPiece, randomPiece.transform.position, randomPiece.transform.rotation);
        }
        //hintHolder.sprite = Transparent[randomIndex];
        

    }

    public void setImagePreview()
    {
        if (!isPreview)
        {
            isPreview = true;
            if (MatchManager.Instance.difficultystr.Equals("Hard"))
            {
                for (int i = 0; i < 16; i++)
                {

                    Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                    TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = true;
                    Transform TransPrefabBorderInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("isRight");
                    TransPrefabBorderInstance.GetComponent<SpriteRenderer>().enabled = true;

                }
                //Transform TransPrefabInstance = GameObject.Find("PuzzleTrans").transform;
                //TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = true;


            }
            else if (MatchManager.Instance.difficultystr.Equals("Easy"))
            {
                for (int i = 0; i < 9; i++)
                {

                    Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                    TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = true;

                }


            }


        }
        else
        {
            isPreview = false;
            if (MatchManager.Instance.difficultystr.Equals("Hard"))
            {
                for (int i = 0; i < 16; i++)
                {

                    Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                    TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;
                    Transform TransPrefabBorderInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("isRight");
                    TransPrefabBorderInstance.GetComponent<SpriteRenderer>().enabled = false;

                }
                //Transform TransPrefabInstance = GameObject.Find("PuzzleTrans").transform;
                //TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;


            }
            else if (MatchManager.Instance.difficultystr.Equals("Easy"))
            {
                for (int i = 0; i < 9; i++)
                {

                    Transform TransPrefabInstance = GameObject.Find("TransPiece (" + i + ")").transform.Find("TransPuzzle");
                    TransPrefabInstance.GetComponent<SpriteRenderer>().enabled = false;

                }


            }
        }


    }
    IEnumerator HideAnim()
    {
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
                for(int i = 0; i<16;i++)
                {
                    PiecesPrefab.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                    PiecesPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                
                }
                yield return new WaitForSeconds(2.4f);
                for (int i=0;i<16;i++)
                {
                    PiecesPrefab.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                    PiecesPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }
            
                //PiecesPrefab.SetActive(true);
                if(randomChoice == 0)
            {

                AnimObject.SetActive(false);
            }
                else
            {
                ZigZag_AnimObject.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i<9;i++)
                {
                    PiecesPrefab.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                    PiecesPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                
                }
                yield return new WaitForSeconds(2.1f);
                for (int i=0;i<9;i++)
                {
                    PiecesPrefab.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                    PiecesPrefab.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

            //PiecesPrefab.SetActive(true);
            if (randomChoice == 0)
            {

                AnimObject.SetActive(false);
            }
            else
            {
                ZigZag_AnimObject.SetActive(false);
            }
        }
        

    }
    public Vector2 GetRandomSpawnPoint()
    {
        int randIndex = Random.Range(0,spawnPoints.Count);
        
        Vector2 randomSpawn = spawnPoints[randIndex];
        spawnPoints.RemoveAt(randIndex);
        return randomSpawn;


    }
   
}
