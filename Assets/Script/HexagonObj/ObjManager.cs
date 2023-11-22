using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum ObjectColor
{
    Orange,
    Red,
    Blue,
    Green,
    Yellow,
    SkyBlue
}

public class ObjManager : MonoBehaviour
{ 
    public static ObjManager instance;
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

    public Transform objectContainer;
    public Dictionary<GameObject, Vector3Int> objectsPosition;
    public Dictionary<Vector3Int, GameObject> recentObjects;
    public Dictionary<Vector3Int, GameObject> cantMovePos;
    public GameObject hexagonPrefab;
    public Sprite[] hexagonColors;

    private void Start()
    {
        objectsPosition = new Dictionary<GameObject, Vector3Int>();
        recentObjects = new Dictionary<Vector3Int, GameObject>();
        cantMovePos = new Dictionary<Vector3Int, GameObject>();
    }

    public void SetPosList()
    {
        foreach (Transform child in objectContainer)
        {
            objectsPosition.Add(child.gameObject, GridCellManager.instance.GetObjCell(child.position));
            recentObjects.Add(GridCellManager.instance.GetObjCell(child.position), child.gameObject);
        }
    }

    public void SetObstacle(GameObject representObj, Vector3Int position)
    {
        cantMovePos.Add(position, representObj);
    }

    public void DisableObjs(GameObject objToDisable, Vector3Int objPos)
    { 
        objectsPosition.Remove(recentObjects[objPos]);
        recentObjects.Remove(objPos);
        objToDisable.transform.GetChild(0).gameObject.SetActive(false);
        objToDisable.GetComponent<MovingHexagon>().IsPushable(false);
    }

    public bool IsNextSameColor(Vector3Int nextPosition, GameObject objToCheck)
    {
        if(!recentObjects.ContainsKey(nextPosition))
        {
            return false;
        }

        ObjectColor color =  recentObjects[nextPosition].GetComponent<MovingHexagon>().GetObjectColor();
        ObjectColor colorToCheck = objToCheck.GetComponent<MovingHexagon>().GetObjectColor();
        if (color == colorToCheck)
        {
            return true;
        }
        return false;
    }

    public bool IsBlockSameColor(Vector3Int blockPosition, GameObject objToCheck)
    {
        if (!cantMovePos.ContainsKey(blockPosition))
        {
            return false;
        }

        ObjectColor color = cantMovePos[blockPosition].GetComponent<MovingHexagon>().GetObjectColor();
        ObjectColor colorToCheck = objToCheck.GetComponent<MovingHexagon>().GetObjectColor();
        if (color == colorToCheck)
        {
            return true;
        }
        return false;
    }
}
