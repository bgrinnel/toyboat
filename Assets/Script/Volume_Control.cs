using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{

    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer sfxMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    public void UpdateMusicVolume(){
        //Debug.Log("music vol");
        musicMixer.SetFloat("musicVol", musicSlider.value);
    }

    public void UpdateSFXVolume(){
        musicMixer.SetFloat("sfxVol", sfxSlider.value);
    }
   
}