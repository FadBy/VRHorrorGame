using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInputRotation : IPlayerVector2InputHandler<Vector2, Vector2>
{
    public Vector2 Handle(Vector2 input)
    {
        return Vector2.zero;
    }
}
