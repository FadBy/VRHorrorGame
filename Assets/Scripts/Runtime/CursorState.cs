using UnityEngine;

public class CursorState
{
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }
}
