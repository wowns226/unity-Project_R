using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscManager : MonoBehaviour
{
    public static bool escActivated = false;

    public static EscManager Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }

    [SerializeField]
    private GameObject go_setting;

    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Toggle masterToggle;

    private void Update()
    {
        TryOpenEsc();
    }

    private void TryOpenEsc()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            escActivated = !go_setting.activeSelf;
            if ( escActivated )
            {
                OpenEsc();
            }
            else 
            {
                CloseEsc();
            }
        }
    }
    private void OpenEsc()
    {
        escActivated = true;
        go_setting.SetActive(true);
    }

    private void CloseEsc()
    {
        escActivated = false;
        go_setting.SetActive(false);
    }

    public void onClickedContinueButton()
    {
        CloseEsc();
    }

    public void onClickedQuitButton()
    {
        Application.Quit();
    }

    public void ToggleMasterAudio()
    {
        bool isOn = masterToggle.isOn;
        SoundManager soundManager = SoundManager.Instance;

        if ( isOn )
        {
            foreach ( var source in soundManager.AudioSources )
            {
                source.mute = true;
            }
        }
        else
        {
            foreach ( var source in soundManager.AudioSources )
            {
                source.mute = false;
            }
        }
    }

    public void SliderMasterVolume()
    {
        float volume = masterSlider.value;
        SoundManager soundManager = SoundManager.Instance;

        if ( volume == -40f ) soundManager.masterMixer.SetFloat("Master", -80);
        else soundManager.masterMixer.SetFloat("Master", volume);
    }

    public void SliderBgmVolume()
    {
        float volume = bgmSlider.value;
        SoundManager soundManager = SoundManager.Instance;

        if ( volume == -40f ) soundManager.masterMixer.SetFloat("BGM", -80);
        else soundManager.masterMixer.SetFloat("BGM", volume);
    }

    public void SliderSfxVolume()
    {
        float volume = sfxSlider.value;
        SoundManager soundManager = SoundManager.Instance;

        if ( volume == -40f ) soundManager.masterMixer.SetFloat("SFX", -80);
        else soundManager.masterMixer.SetFloat("SFX", volume);
    }
}
