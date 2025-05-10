using System;
using UnityEngine;
using Zenject;

public class AudioManager : IInitializable
{
    private GameSettings _settings;

    public float Volume
    {
        get => _settings.Volume;
        set
        {
            AudioListener.volume = value;
            _settings.Volume = value;
        }
    }

    public AudioManager(GameSettings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        AudioListener.volume = _settings.Volume;
    }
    
    
}
