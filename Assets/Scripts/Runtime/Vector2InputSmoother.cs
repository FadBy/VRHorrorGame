using System;
using UnityEngine;
using Zenject;

[Serializable]
public class Vector2InputSmoother : IPlayerVector2InputHandler<Vector2, Vector2>
{
    [SerializeField] private float _sensitivity = 7f;
    [SerializeField] private float _gravity = 5f;
    [SerializeField] private bool _snap = true;
    [SerializeField] private AnimationCurve _smoothCurve;

    private float _lastHandleTime;
    
    private AxisInputSmoother _xInputSmoother;
    private AxisInputSmoother _yInputSmoother;
    
    private AxisInputSmoother XInputSmoother => GetOrInitialize(ref _xInputSmoother);

    private AxisInputSmoother YInputSmoother => GetOrInitialize(ref _yInputSmoother);

    private AxisInputSmoother GetOrInitialize(ref AxisInputSmoother smoother)
    {
        return smoother ??= new AxisInputSmoother(_sensitivity, _gravity, _snap, _smoothCurve);
    }

    public Vector2 Handle(Vector2 input)
    {
        var x = XInputSmoother.SmoothInput(input.x, CalculateDeltaTime());
        var y = YInputSmoother.SmoothInput(input.y, CalculateDeltaTime());
        SaveHandleTime();
        return new Vector2(x, y);
    }

    private void SaveHandleTime()
    {
        _lastHandleTime = Time.time;
    }

    private float CalculateDeltaTime()
    {
        var currentHandleTime = Time.time;
        return currentHandleTime - _lastHandleTime;
    }

    private class AxisInputSmoother
    {
        private float _sensitivity;
        private float _gravity;
        private bool _snap;
        private AnimationCurve _smoothCurve;
        
        private float _smoothTime;
        
        private float Sensitivity => _sensitivity;

        private float Gravity => _gravity;

        private bool Snap => _snap;

        private float SmoothTime
        {
            get => _smoothTime;
            set
            {
                value = Mathf.Clamp(value, -1f, 1f);
                _smoothTime = value;
            }
        }
        
        public AxisInputSmoother(float sensitivity, float gravity, bool snap, AnimationCurve smoothCurve)
        {
            _sensitivity = sensitivity;
            _gravity = gravity;
            _snap = snap;
            _smoothCurve = smoothCurve;
        }
        
        public float SmoothInput(float rawInputValue, float deltaTime)
        {
            if (rawInputValue > 0f)
            {
                if (Snap && SmoothTime < 0f)
                {
                    SmoothTime = 0f;
                }
                SmoothTime += Sensitivity * deltaTime;
            }
            else if (rawInputValue == 0f)
            {
                if (SmoothTime > 0f)
                {
                    SmoothTime = Mathf.MoveTowards(SmoothTime, 0f, Gravity * deltaTime);
                }
                else if (SmoothTime < 0f)
                {
                    SmoothTime = Mathf.MoveTowards(SmoothTime, 0f, Gravity * deltaTime);
                }
            }
            else
            {
                if (Snap && SmoothTime > 0f)
                {
                    SmoothTime = 0f;
                }
                SmoothTime -= Sensitivity * deltaTime;
            }

            return _smoothCurve.Evaluate(SmoothTime) * Mathf.Abs(rawInputValue);
        }
    }
}
