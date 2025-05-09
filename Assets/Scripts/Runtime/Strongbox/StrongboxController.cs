using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StrongboxController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textDisplay;
    [SerializeField] private Animator _animator;
    
    private int _correctCode = 3456;

    private string _currentCode = "";

    public UnityEvent DoorOpened;
    public UnityEvent DoorUnlocked;

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
        if (CurrentCode.Length >= 5) return;
        CurrentCode = (int.Parse(CurrentCode == "" ? "0" : CurrentCode) * 10 + digit).ToString();
    }

    public void OnClearPressed()
    {
        CurrentCode = "";
    }

    public void OnEnterPressed()
    {
        if (IsCodeCorrect(CurrentCode))
        {
            UnlockDoor();
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

    private bool IsCodeCorrect(string code)
    {
        return _correctCode.ToString() == code;
    }

    private void UnlockDoor()
    {
        _animator.SetTrigger("Unlock");
    }
}
