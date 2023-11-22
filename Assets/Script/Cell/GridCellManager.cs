using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField] 
    private List<Vector3Int> tileLocations = new List<Vector3Int>();

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
    private void Start()
    {
        GetMoveAbleCell();
    }
    private void GetMoveAbleCell()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);
                if (tileMap.HasTile(localLocation))
                {
                    tileLocations.Add(localLocation);
                }
            }
        }
    }
    public void SetTileMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int cellPosition = GetObjCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(cellPosition);
        }
    }

    public bool IsPlaceableArea(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3Int>  GetCellsPosition()
    {
        return tileLocations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}
