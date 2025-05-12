using System;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private Type _type = Type.UI;
    [SerializeField, Range(0f, 270f)] private float maxAngle = 150f;

    public bool Enabled { get; set; } = true;

    private void LateUpdate()
    {
        if (!Enabled) return;
        if (Camera.main == null) return;

        switch (_type)
        {
            case Type.UI:
                transform.forward = Camera.main.transform.forward;
                break;

            case Type.Head:
                Vector3 toCamera = Camera.main.transform.position - transform.position;
                toCamera.y = 0f;
                toCamera.Normalize();

                Vector3 currentForward = transform.forward;
                currentForward.y = 0f;
                currentForward.Normalize();

                float angle = Vector3.Angle(currentForward, toCamera);
                if (angle <= maxAngle)
                {
                    transform.forward = toCamera;
                }
                else
                {
                    Quaternion limitedRotation = Quaternion.RotateTowards(
                        Quaternion.LookRotation(currentForward),
                        Quaternion.LookRotation(toCamera),
                        maxAngle
                    );
                    transform.rotation = limitedRotation;
                }
                break;
        }
    }

    private enum Type
    {
        UI,
        Head
    }
}