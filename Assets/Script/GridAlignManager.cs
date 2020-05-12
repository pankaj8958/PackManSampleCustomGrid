using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlignManager : MonoBehaviour {
    [SerializeField]
    private int columnNo;
    [SerializeField]
    private int rowNo;
    [SerializeField]
    private float sizeScale;

    [SerializeField]
    private GameObject gridNodePrefabs;

    [SerializeField]
    private Transform boundParent;

    private string minGrid {
        get {
            return (rowNo % 2 == 0 ? 1 : 0) + "," + (columnNo % 2 == 0 ? 1 : 0);
        }
    }

    private string maxGrid
    {
        get
        {
            return (rowNo % 2 == 0 ? rowNo : rowNo - 1) + "," + (columnNo % 2 == 0 ? columnNo : columnNo - 1);
        }
    }

    private Dictionary<string, Transform> gridNodeDict;
    private Dictionary<Wall, Transform> wallDict;

    public static GridAlignManager Instance;

    private void Awake ()
    {
        if(Instance == null)
        {
            Instance = this.GetComponent<GridAlignManager>();
        }
    }

    private void Start()
    {
        UpdateWallObjectDict();
    }

    private void Initialize()
    {
        if (gridNodeDict == null)
            gridNodeDict = new Dictionary<string, Transform>();
        if (wallDict == null)
            wallDict = new Dictionary<Wall, Transform>();

        CreateGrids();
        AlignBoundary();
    }

    /// <summary>
    /// Adjust all grid considering grid size is 1 x 1
    /// </summary>
    /// <param name="parent"></param>
    private void CreateGrids()
    {
        int rowCount = columnNo;
        int columnCount = rowNo;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                int x = i;
                int y = j;
                if (x > 0 && x > rowCount / 2)
                {
                    x = rowCount - i;
                    x = -x;
                }
                if (y > 0 && y > columnCount / 2)
                {
                    y = columnCount - j;
                    y = -y;
                }
                CreateGridNode(x, y);
            }
        }
    }


    /// <summary>
    /// Create death cell on grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CreateGridNode(int x, int y)
    {
        if (gridNodePrefabs)
        {
            GameObject obj = Instantiate(gridNodePrefabs);
            obj.transform.position = new Vector2((float)x / 1.3f, (float)y / 1.3f);
            obj.name = GetCellName(x, y);
            obj.transform.SetParent(this.transform);
            gridNodeDict.Add(obj.name, obj.transform);
        }
    }


    /// <summary>
    /// assign name of cell by x, y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private string GetCellName(int x, int y)
    {
        string name = string.Empty;
        name = (y + rowNo / 2) + "," + (x + columnNo / 2);
        return name;
    }

    private Transform GetCellByName(int x, int y)
    {
        Transform selectedTrans = null;
        string keyString = x + "," + y;
        Debug.Log("[keyString]: " + keyString);
        if(gridNodeDict != null && gridNodeDict.Count > 0)
        {
            if(gridNodeDict.ContainsKey(keyString))
            {
                selectedTrans = gridNodeDict[keyString];
            }
        }
        return selectedTrans;
    }


    private Transform GetCellByName(string gridName)
    {
        Transform selectedTrans = null;
        
        if (gridNodeDict != null && gridNodeDict.Count > 0)
        {
            if (gridNodeDict.ContainsKey(gridName))
            {
                selectedTrans = gridNodeDict[gridName];
            }
        }
        return selectedTrans;
    }

    public Transform GetBoundaryTransform(Wall wall)
    {
        if(wallDict != null && wallDict.Count > 0 && wallDict.ContainsKey(wall))
        {
            return wallDict[wall];
        }
        else
        {
            return null;
        }   
    }

    private void UpdateWallObjectDict()
    {
        if (wallDict == null)
            wallDict = new Dictionary<Wall, Transform>();

        if (boundParent == null)
            boundParent = transform.GetChild(0).transform;

        if (boundParent)
        {
            Transform[] wallTrans = boundParent.GetComponentsInChildren<Transform>();

            if (wallTrans != null && wallTrans.Length > 0)
            {
                foreach (Transform wall in wallTrans)
                {
                    switch (wall.name)
                    {
                        case "WallUp":  
                            if (wallDict.ContainsKey(Wall.WallUp))
                                    wallDict.Remove(Wall.WallUp);
                                wallDict.Add(Wall.WallUp, wall);
                            break;
                        case "WallDown":
                                if (!wallDict.ContainsKey(Wall.WallDown))
                                    wallDict.Remove(Wall.WallDown);
                                wallDict.Add(Wall.WallDown, wall);
                            break;
                        case "WallRight":
                               if (!wallDict.ContainsKey(Wall.WallRight))
                                    wallDict.Remove(Wall.WallRight);
                                wallDict.Add(Wall.WallRight, wall);
                            break;
                        case "WallLeft":
                                if (!wallDict.ContainsKey(Wall.WallLeft))
                                    wallDict.Remove(Wall.WallLeft);
                                wallDict.Add(Wall.WallLeft, wall);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Align the boundary outside the grids
    /// </summary>
    private void AlignBoundary()
    {
        if (boundParent)
        {
            Transform[] wallTrans = boundParent.GetComponentsInChildren<Transform>();

            Transform captureGrid;

            if (wallTrans != null && wallTrans.Length > 0)
            {
                foreach (Transform wall in wallTrans)
                {
                    switch (wall.name)
                    {
                        case "WallUp":
                            
                            captureGrid = GetCellByName(maxGrid);
                            if (captureGrid)
                            {
                                wall.position = new Vector2(wall.position.x, captureGrid.position.y);
                            }

                            break;
                        case "WallDown":
                            captureGrid = GetCellByName(minGrid);
                            if (captureGrid)
                            {
                                wall.position = new Vector2(wall.position.x, captureGrid.position.y);
                            }
                            break;
                        case "WallRight":
                            captureGrid = GetCellByName(maxGrid);
                            if (captureGrid)
                            {
                                wall.position = new Vector2(captureGrid.position.x, wall.position.y);
                            }
                            break;
                        case "WallLeft":
                            captureGrid = GetCellByName(minGrid);
                            if (captureGrid)
                            {
                                wall.position = new Vector2(captureGrid.position.x, wall.position.y);
                            }
                            break;
                    }
                }
            }
            
        }
    }

    public void Refresh()
    {
        DestroyGridNode();
        Initialize();
    }

    public void DestroyGridNode()
    {
        while (transform.childCount != 1)
        {
            DestroyImmediate(transform.GetChild(1).gameObject);
        }

        if (gridNodeDict != null && gridNodeDict.Count > 0)
        {
            gridNodeDict.Clear();
        }
    }

    public void UpdateSize()
    {
        transform.localScale = new Vector2(sizeScale, sizeScale);
    }
}

public enum Wall
{
    WallUp,
    WallDown,
    WallLeft,
    WallRight
}
