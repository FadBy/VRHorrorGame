using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class StrongboxController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textDisplay;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private AudioSource _wrongSound;
    [SerializeField] private AudioSource _numberSound;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    
    private int _correctCode = 3456;

    private string _currentCode = "";

    public UnityEvent DoorOpened;
    public UnityEvent DoorUnlocked;

    private CodeController _codeController;
    
    [Inject]
    public void Construct(CodeController codeController)
    {
        _codeController = codeController;
    }

    private void Start()
    {
        _correctCode = _codeController.Code;
    }

    private string CurrentCode
    {
        get => _currentCode;
        set
        {
            _currentCode = value;
            _textDisplay.text = _currentCode;
        }
    }

    public void OnDigitPressed(int digit)
    {
        if (digit >= 1 && digit <= 9)
        {
            PlayNumberSound(digit);
        }
        else
        {
            PlayNumberSound(10);
        }
        if (CurrentCode.Length >= 5) return;
        CurrentCode = (int.Parse(CurrentCode == "" ? "0" : CurrentCode) * 10 + digit).ToString();
    }

    public void OnClearPressed()
    {
        PlayNumberSound(9);
        CurrentCode = "";
    }

    public void OnEnterPressed()
    {
        if (IsCodeCorrect(CurrentCode))
        {
            _correctSound.Play();
            UnlockDoor();
        }
        else
        {
            _wrongSound.Play();
        }
    }

    public void OnDoorOpened()
    {
        DoorOpened.Invoke();
    }

    public void OnDoorUnlocked()
    {
        DoorUnlocked.Invoke();
    }

    public void Open()
    {
        _animator.SetBool("Open", true);
    }

    private void PlayNumberSound(int buttonNumber)
    {
        _numberSound.pitch = _minPitch + (_maxPitch - _minPitch) / 11 * (buttonNumber - 1);
        _numberSound.Play();

    }

    private bool IsCodeCorrect(string code)
    {
        return _correctCode.ToString() == code;
    }

    private void UnlockDoor()
    {
        _animator.SetTrigger("Unlock");
    }
}
