using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PiecesScript : MonoBehaviour
{
    public Vector3 RightPosition;
    public bool InRighPosition;
    public bool Selected;
    public RectTransform Content;
     
    public GameObject HorizontalScroll;
    GameObject ps;
    GameObject border;
    SpawnManager spawnmanager;
    TutorialManager tutorialManager;
    

    void Start()
    {
        
        RightPosition = transform.position;
        
        if(!MatchManager.Instance.isOnTutorial)
        {
            spawnmanager = FindObjectOfType<SpawnManager>();
            Vector2 newPos = spawnmanager.GetRandomSpawnPoint();
            transform.position = new Vector3(newPos.x,newPos.y);
            SetScale();
           
        }
        else
        {
            //do Something
            tutorialManager = FindObjectOfType<TutorialManager>();
            if(transform.name == "Piece (4)"||transform.name == "Piece (2)"||transform.name == "Piece (8)")
            {
                Vector2 newPos = tutorialManager.GetTutPeiceSpawnPoint();
                transform.position = (Vector3)newPos;
                SetScale();
            }
            
        }
        
        border = transform.GetChild(1).gameObject;
        ps= transform.GetChild(2).gameObject;
        border.SetActive(false);
        ps.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, RightPosition) < 0.3f)
        {
            if (!Selected)
            {
                if (InRighPosition == false)
                {
                    Debug.Log("In right position");

                    transform.position = RightPosition;
                    InRighPosition = true;
                    SoundManager.Instance.atRightPositionSFX();
                    ps.SetActive(true);
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    border.SetActive(true);
                    MatchManager.Instance.placedPieces++;
                    
                }
            }

        }
        
    }
    public void ResetScale()
    {
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            transform.localScale = new Vector3(0.2931958f,0.2937631f,1);
        }
        else
        {
            transform.localScale = new Vector3(0.388461f,0.389688f,1);
        }
        
    }
    public void SetScale()
    {
        if(MatchManager.Instance.difficultystr.Equals("Hard"))
        {
            transform.localScale = new Vector3(0.18f,0.18f,1);
        }
        else
        {
            transform.localScale = new Vector3(0.22f,0.22f,1);
        }
        
    }
}
