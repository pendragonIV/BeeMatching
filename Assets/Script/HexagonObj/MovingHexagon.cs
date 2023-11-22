using DG.Tweening;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rotate"))
        {
            MoveDirection moveDirection = collision.GetComponent<RotateArrow>().GetRotateDirection();
            directionArrow.DORotate(new Vector3(0, 0, (int)moveDirection * 60), .5f)
                .SetEase(Ease.OutBack);
            SetMoveDirection(moveDirection);
        }
    }

    public void SetMoveDirection(MoveDirection moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void RotateArrow()
    {
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
