using UnityEngine;

public abstract class PlayerMovementInputReader
{
     public abstract Vector2 PlayerMovement { get; }

     public float PlayerMovementX => PlayerMovement.x;

     public float PlayerMovementY => PlayerMovement.y;
}
