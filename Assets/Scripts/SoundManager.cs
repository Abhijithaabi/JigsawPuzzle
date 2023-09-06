using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public AudioSource vfxAudiosrc;
    [SerializeField] public AudioSource musicAudiosrc;
    [SerializeField] AudioClip musicClip;
    [SerializeField] AudioClip voiceEffect;
    [SerializeField] AudioClip congratsSound;

    
    


    public bool isMusicMute = false;
    public bool isVoiceMute = false;
    public bool canPlayMusic = false;

    void Awake()
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
        //Instance = this;
        canPlayMusic = false;
        musicAudiosrc.enabled = false;
        stopMusicForHome();
    }


    public void Start()
    {
        
        //isMusicMute = MatchManager.Instance.isMusicMute;
        //isVoiceMute = MatchManager.Instance.isVoiceMute;
        isMusicMute = false; //initially muted as we dont music to be there in home screen, only in game scene
        isVoiceMute = false; 




    }
    public void checkToPlay()
    {
        canPlayMusic = false;
        isMusicMute = false;
        isVoiceMute = false;

    }
    public void startMusic()
    {
        //we check for these conditions in ui manager only, for mute music and voice settings.

    }


    public void stopMusicForHome()
    {
        
        canPlayMusic = false;
        if(canPlayMusic == false)
        {
            musicAudiosrc.Pause();
           

        }



    }

    public void checkForGameMusic()
    {
        canPlayMusic = true;
        if(canPlayMusic == true)
        {
            musicAudiosrc.enabled = true;
            if (isMusicMute == true)
            {
                //stop the music and change sprites to off
                musicAudiosrc.Pause();
                UIManager.Instance.musicBtn.GetComponent<Image>().sprite = UIManager.Instance.musicButtonSprites[1];  //off sprite



            }
            else if(isMusicMute == false)
            {

                musicAudiosrc.UnPause();
                UIManager.Instance.musicBtn.GetComponent<Image>().sprite = UIManager.Instance.musicButtonSprites[0];  //on sprite
                //play the music and change sprites to on

            }

            if(isVoiceMute == true)
            {
                UIManager.Instance.voiceBtn.GetComponent<Image>().sprite = UIManager.Instance.voiceButtonSprites[1];  //off sprite
                //stop the voice effects and change sprites to off
            }

            else if(isVoiceMute == false)
            {
                UIManager.Instance.voiceBtn.GetComponent<Image>().sprite = UIManager.Instance.voiceButtonSprites[0];  //on sprite
                //play the vocie effects and change sprites to on
            }


        }
    }
    
    

    public void stopMusic()
    {
        musicAudiosrc.Pause();
    }
   


    public void atRightPositionSFX()
    {

        if(isVoiceMute == false && !MatchManager.Instance.isOnTutorial)
        {
            vfxAudiosrc.volume = 1f;
            vfxAudiosrc.clip = voiceEffect;
            vfxAudiosrc.PlayOneShot(voiceEffect);
        }

    }

    public void congratsSFX()
    {
        if (isMusicMute == false)
        {
            vfxAudiosrc.volume = 0.3f;
            vfxAudiosrc.clip = congratsSound;
            vfxAudiosrc.PlayOneShot(congratsSound);
        }

    }



}
