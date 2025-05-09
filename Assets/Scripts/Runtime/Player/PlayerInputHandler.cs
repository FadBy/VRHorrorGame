using UnityEngine;

public interface IPlayerVector2InputHandler<in TInput, out TOutput>
{
    TOutput Handle(TInput input);
}
