using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpawner : MonoBehaviour
{   [SerializeField] GameObject[] Pieces;
[SerializeField] GameObject Anim;
int randomIndex;
// public Vector3 InitialPosition;

GameObject border;
GameObject ps;
float x;
float y;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake(){
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
        InstantiateAnimation(uplaodedImage,x,y);
    }

    void InstantiateAnimation(Sprite imageSprite, float scaleX, float scaleY){
        randomIndex = Random.Range(0,Pieces.Length);
        GameObject randomPiece = Pieces[randomIndex];
        
        // GameObject instance = Instantiate(randomPiece,transform.position,randomPiece.transform.rotation);
       
        // randomPiece.SetActive(true);
        

        
        for(int i = 0 ; i < randomPiece.transform.childCount;i++){
            Transform child = randomPiece.transform.GetChild(i);
            child.GetChild(0).GetComponent<SpriteRenderer>().sprite = imageSprite;
            child.GetChild(0).transform.localScale = new Vector3(scaleX,scaleY,1f);
            child.GetComponent<PiecesScript>().enabled = false;

            border = child.GetChild(1).gameObject;
            ps= child.GetChild(2).gameObject;
            border.SetActive(false);
            ps.SetActive(false);

            Instantiate(child,randomPiece.transform.position,Quaternion.identity);
        }
        // randomPiece.transform.SetParent(Anim.transform);
        

        // InitialPosition = transform.position;

        
       
    }
}