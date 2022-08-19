using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using StarGarden.Core.SaveData;

namespace StarGarden.Core
{
    public class AudioSettings : MonoBehaviour, Manager
    {
        public Slider musicSlider, SFXSlider;

        FMOD.Studio.Bus Music;
        FMOD.Studio.Bus SFX;

        public void Initialise() { }

        public void LateInitialise()
        {
            float musicVol = SaveDataManager.SaveData.MusicVolume;
            musicSlider.value = musicVol;
            float SFXVol = SaveDataManager.SaveData.SFXVolume;
            SFXSlider.value = SFXVol;

            SetVolume();
        }

        void Awake()
        {
            Music = RuntimeManager.GetBus("bus:/Music");
            SFX = RuntimeManager.GetBus("bus:/SFX");
        }

        public void SetVolume()
        {
            Music.setVolume(musicSlider.value);
            SFX.setVolume(SFXSlider.value);
        }

        public void SaveSettings()
        {
            SaveDataManager.SaveData.MusicVolume = musicSlider.value;
            SaveDataManager.SaveData.SFXVolume = SFXSlider.value;
            SaveDataManager.SaveAll();
        }
    }
}