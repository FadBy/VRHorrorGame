using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CodeController : IInitializable
{
    private int _code;

    public int Code => _code;
    
    public void Initialize()
    {
        _code = PickRandomCode();
        Debug.Log("Code: " + _code);
    }

    private int PickRandomCode()
    {
        return Random.Range(1000, 10000);
    }
}