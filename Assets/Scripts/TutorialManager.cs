using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public GameObject[] popUps;
    [SerializeField]private int popUpIndex = 0;
     public List<Vector2> tutspawnPoints = new List<Vector2>();
    [SerializeField] GameObject handAnim;
    [SerializeField] GameObject tutorialPiece;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] GameObject overLayPanel;
    [SerializeField] GameObject dragAndDropScript;
    [SerializeField] GameObject preview;
    [SerializeField] GameObject theme;
    GameObject peice4;
    GameObject peice8;
    bool isAnim = false;
    bool pauseLoop;
    bool isThemeClicked = false;
    Vector2 randomSpawn;

    void Awake()
    {
        Instantiate(tutorialPiece,tutorialPiece.transform.position,tutorialPiece.transform.rotation);
    }

    void Start()
    {
        popUpIndex = 0;
        spawnManager.isPreview = true;
        spawnManager.setImagePreview();
        
        peice4 = GameObject.Find("Piece (" + 4 + ")");//Piece (4)
        peice8 = GameObject.Find("Piece (" + 8 + ")");
        peice4.GetComponent<BoxCollider2D>().enabled = false;
        peice8.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Update()
    {
        if(!pauseLoop)
        {
            for(int i=0;i<popUps.Length;i++)
            {
                if(i==popUpIndex)
                {
                    Debug.Log("Inside tutorialManager Update loop i==popUpIndex");
                    popUps[popUpIndex].SetActive(true);
                }
                else
                {
                    Debug.Log("Inside tutorialManager Update loop else statement");
                    popUps[i].SetActive(false);
                }
           
            }
        }
        
        if(popUpIndex == 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 2)
        {
            if(Input.GetMouseButtonDown(0))
            {
                popUps[popUpIndex].SetActive(false);
                ClosePanels();
                if(isAnim == false)
                {
                    
                    StartCoroutine("HandAnim");

                }
                pauseLoop = true;
            }
            if(MatchManager.Instance.placedPieces == 7)
            {
                
                popUpIndex++;
                OpenPanels();
                pauseLoop = false;
            }
        }
        else if(popUpIndex == 3)
        {
            if(Input.GetMouseButtonDown(0))
            {
                popUps[popUpIndex].SetActive(false);
                ClosePanels();
                pauseLoop = true;
            }
            if(spawnManager.isPreview == true)
            {
                popUpIndex++;
                OpenPanels();
                pauseLoop = false;
            }
        }
        else if(popUpIndex == 4)
        {
            if(Input.GetMouseButtonDown(0))
            {
                peice4.GetComponent<BoxCollider2D>().enabled=true;
                popUps[popUpIndex].SetActive(false);
                ClosePanels();
                pauseLoop = true;
            }
            if(MatchManager.Instance.placedPieces == 8)
            {
                popUpIndex++;
                OpenPanels();
                pauseLoop = false;
            }
        }
        else if(popUpIndex == 5)
        {
            if(Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
                theme.SetActive(true);
                
            }
        }
        else if(popUpIndex == 6)
        {
            if(Input.GetMouseButtonDown(0))
            {
                
                popUps[popUpIndex].SetActive(false);
                ClosePanels();
                pauseLoop = true;
            }
            if(isThemeClicked)
            {
                peice8.GetComponent<BoxCollider2D>().enabled=true;
            }
            if(MatchManager.Instance.placedPieces == 9)
            {
                popUpIndex++;
                OpenPanels();
                pauseLoop = false;
            }
        }
        else if(popUpIndex == 7)
        {
            

            // if(Input.GetMouseButtonDown(0))
            //{
            //    MatchManager.Instance.ResetMatchManager();
            //    SceneManager.LoadScene(0);
            //}
        }
    }

    public void homeButonTut()
    {
        
           MatchManager.Instance.ResetMatchManager();
           SceneManager.LoadScene(0);
     
    }

    public Vector2 GetTutPeiceSpawnPoint()
    {
        
        int randIndex = 0;
        if(randIndex<tutspawnPoints.Count)
        {
            randomSpawn = tutspawnPoints[randIndex];
        }
        
        tutspawnPoints.RemoveAt(randIndex);
        randIndex++;
        return randomSpawn;


    }
    void ClosePanels()
    {   
        if(isAnim == false)
        {
            overLayPanel.SetActive(false);
            dragAndDropScript.GetComponent<DragAndDrop>().enabled = false;
            preview.SetActive(false);
            theme.SetActive(false);
            
        }
        preview.SetActive(false);
        theme.SetActive(false);
        overLayPanel.SetActive(false);
        dragAndDropScript.GetComponent<DragAndDrop>().enabled = true;
        
    }
    void OpenPanels()
    {
        overLayPanel.SetActive(true);
        dragAndDropScript.GetComponent<DragAndDrop>().enabled = false;
        if(popUpIndex == 3)
        {
            preview.SetActive(true);
        }
        if(popUpIndex == 6)
        {
            theme.SetActive(true);
        }
    }
    public void ThemeClicked()
    {
        isThemeClicked = true;
    }

    private IEnumerator HandAnim()
    {   

        handAnim.SetActive(true);
        yield return new WaitForSeconds(3f);
        dragAndDropScript.GetComponent<DragAndDrop>().enabled = true;
        isAnim = true;
        Debug.Log("hand anim 194");
        handAnim.SetActive(false);
        
    }


}
