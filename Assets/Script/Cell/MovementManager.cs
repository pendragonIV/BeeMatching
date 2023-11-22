using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum MoveDirection
{
    Right,
    UpRight,
    UpLeft,
    Left,
    DownLeft,
    DownRight
}

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    private List<GameObject> pushingObjs;

    private void Start()
    {
        pushingObjs = new List<GameObject>();
    }

    public void ObjectMoveController(GameObject hexagon, MoveDirection moveDirection)
    {
        Vector3Int moveFrom;
        Vector3Int moveTo;
        moveFrom = GridCellManager.instance.GetObjCell(hexagon.transform.position);
        moveTo = GetCellToMove(moveFrom, moveDirection);

        if (ObjManager.instance.objectsPosition.ContainsValue(moveTo))
        {
            PushingObj(hexagon,moveFrom, moveDirection);
        }
        else
        {
            MoveObj(hexagon, moveFrom, moveTo, true);
        }
    }

    private void PushingObj(GameObject startObject, Vector3Int startPosition, MoveDirection moveDirection)
    {
        pushingObjs.Clear();

        Vector3Int cellPosition = startPosition;

        GameObject currentCheck = startObject;

        while (ObjManager.instance.objectsPosition.ContainsValue(cellPosition))
        {

            pushingObjs.Add(ObjManager.instance.recentObjects[cellPosition]);
            cellPosition = GetCellToMove(cellPosition, moveDirection);

            if (ObjManager.instance.IsNextSameColor(cellPosition, currentCheck))
            {
                pushingObjs.Remove(currentCheck);
                pushingObjs.Remove(ObjManager.instance.recentObjects[cellPosition]);
                MoveObj(currentCheck, ObjManager.instance.objectsPosition[currentCheck], cellPosition, false);
                break;
            }
            if (ObjManager.instance.recentObjects.ContainsKey(cellPosition))
            {
                currentCheck = ObjManager.instance.recentObjects[cellPosition];
            }

        }

        for (int i = pushingObjs.Count - 1; i >= 0; i--)
        {
            cellPosition = GetCellToMove(ObjManager.instance.objectsPosition[pushingObjs[i]], moveDirection);
            if (!ObjManager.instance.objectsPosition.ContainsValue(cellPosition))
            {
                MoveObj(pushingObjs[i], ObjManager.instance.objectsPosition[pushingObjs[i]], cellPosition, true);
            }
        }
    }


    public Vector3Int GetCellToMove(Vector3Int moveFrom, MoveDirection moveDirection)
    {
        Vector3Int moveToCell = Vector3Int.zero;

        switch (moveDirection)
        {
            case MoveDirection.Left:
                moveToCell = new Vector3Int(moveFrom.x - 1, moveFrom.y, 0);
                break;
            case MoveDirection.Right:
                moveToCell = new Vector3Int(moveFrom.x + 1, moveFrom.y, 0);
                break;
            case MoveDirection.UpLeft:
                if ((moveFrom.y + 1) % 2 == 0)
                {
                    moveToCell = new Vector3Int(moveFrom.x, moveFrom.y + 1, 0);
                }
                else
                {
                    moveToCell = new Vector3Int(moveFrom.x - 1, moveFrom.y + 1, 0);
                }
                break;
            case MoveDirection.UpRight:
                if ((moveFrom.y + 1) % 2 == 0)
                {
                    moveToCell = new Vector3Int(moveFrom.x + 1, moveFrom.y + 1, 0);
                }
                else
                {
                    moveToCell = new Vector3Int(moveFrom.x, moveFrom.y + 1, 0);
                }
                break;
            case MoveDirection.DownLeft:
                if ((moveFrom.y + 1) % 2 == 0)
                {
                    moveToCell = new Vector3Int(moveFrom.x, moveFrom.y - 1, 0);
                }
                else
                {
                    moveToCell = new Vector3Int(moveFrom.x - 1, moveFrom.y - 1, 0);
                }
                break;
            case MoveDirection.DownRight:
                if ((moveFrom.y + 1) % 2 == 0)
                {
                    moveToCell = new Vector3Int(moveFrom.x + 1, moveFrom.y - 1, 0);
                }
                else
                {
                    moveToCell = new Vector3Int(moveFrom.x, moveFrom.y - 1, 0);
                }
                break;
        }

        return moveToCell;
    }

    private void MoveObj(GameObject hexagon, Vector3Int moveFrom, Vector3Int moveTo, bool isNormalMove)
    {
        if (!ObjManager.instance.cantMovePos.ContainsKey(moveTo) && GridCellManager.instance.IsPlaceableArea(moveTo))
        {
            hexagon.transform.DOMove(GridCellManager.instance.PositonToMove(moveTo), .3f).SetEase(Ease.InOutSine);
            if (isNormalMove)
            {
                ObjManager.instance.objectsPosition[hexagon] = moveTo;

                ObjManager.instance.recentObjects.Remove(moveFrom);
                if (!ObjManager.instance.recentObjects.ContainsKey(moveTo))
                {
                    ObjManager.instance.recentObjects[moveTo] = hexagon;
                }
            }
            else
            {
                ObjManager.instance.SetObstacle(ObjManager.instance.recentObjects[moveTo], moveTo);
                ObjManager.instance.DisableObjs(hexagon, moveFrom);
                ObjManager.instance.DisableObjs(ObjManager.instance.recentObjects[moveTo], moveTo);
            }
        }
        else
        {
            if(ObjManager.instance.IsBlockSameColor(moveTo, hexagon))
            {
                hexagon.transform.DOMove(GridCellManager.instance.PositonToMove(moveTo), .3f).SetEase(Ease.InOutSine);
                ObjManager.instance.DisableObjs(hexagon, moveFrom);
            }
        }

        GameManager.instance.CheckWin(ObjManager.instance.cantMovePos.Count);
    }
}
