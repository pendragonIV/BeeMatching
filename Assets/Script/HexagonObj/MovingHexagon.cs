using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHexagon : MonoBehaviour
{
    [SerializeField]
    private MoveDirection moveDirection;
    [SerializeField]
    private ObjectColor objectColor;

    [SerializeField]
    private SpriteRenderer hexagonSR;
    [SerializeField]
    private Transform directionArrow;

    private bool isPushable = true;

    private void OnMouseDown()
    {
        if (isPushable)
        {
            MovementManager.instance.ObjectMoveController(this.gameObject, moveDirection);
        }
    }

    //private void OnValidate()
    //{
    //    hexagonSR = GetComponent<SpriteRenderer>();
    //    directionArrow = this.transform.GetChild(0);
    //    hexagonSR.sprite = GameObject.FindObjectOfType<ObjManager>().hexagonColors[(int)objectColor];
    //    directionArrow.localRotation = Quaternion.Euler(0, 0, (int)moveDirection * 60);
    //}

    public void SetMoveDirection(MoveDirection moveDirection)
    {
        this.moveDirection = moveDirection;
        directionArrow.localRotation = Quaternion.Euler(0, 0, (int)moveDirection * 60);
    }

    public void SetObjectColor(ObjectColor objectColor)
    {
        this.objectColor = objectColor;
        hexagonSR.sprite = GameObject.FindObjectOfType<ObjManager>().hexagonColors[(int)objectColor];
    }

    public ObjectColor GetObjectColor()
    {
        return objectColor;
    }

    public MoveDirection GetMoveDirection()
    {
        return moveDirection;
    }

    public void IsPushable(bool isPushable)
    {
        this.isPushable = isPushable;
    }

    public bool IsPushable()
    {
        return isPushable;
    }
}
