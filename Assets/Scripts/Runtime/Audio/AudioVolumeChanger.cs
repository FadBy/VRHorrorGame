using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioVolumeChanger : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    private AudioManager _audioManager;
    
    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this._audioManager = audioManager;
    }

    private void Start()
    {
        _slider.value = _audioManager.Volume;
    }

    public void OnVolumeChanged(float value)
    {
        _audioManager.Volume = value;
    }
}