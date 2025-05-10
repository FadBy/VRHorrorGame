using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Transform _container;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _container.gameObject.SetActive(!_container.gameObject.activeSelf);
            if (_container.gameObject.activeSelf)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    
    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    
}