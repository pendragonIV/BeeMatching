using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateArrow : MonoBehaviour
{
    [SerializeField]
    private MoveDirection rotateDirection;

    public void SetRotateDirection(MoveDirection rotateDirection)
    {
        this.rotateDirection = rotateDirection;
        this.transform.localRotation = Quaternion.Euler(0, 0, (int)rotateDirection * 60);
    }

    public MoveDirection GetRotateDirection()
    {
        return rotateDirection;
    }

    private void OnValidate()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, (int)rotateDirection * 60);
    }
}
