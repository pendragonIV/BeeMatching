using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public GameScene gameScene;
    public SceneChanger sceneChanger;
    public Level currentLevelData;

    #region Game Status
    public  int achivement = 0;
    private float timeLeft;
    private bool isGameLose = false;
    private bool isGameWin = false;
    #endregion

    private void Start()
    {
        PrepareLevel();
        Time.timeScale = 1;
    }

    private void PrepareLevel()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);

        timeLeft = currentLevelData.time;
        gameScene.UpdateTimeLeftText(timeLeft);

        InitHexagons(currentLevelData.hexagonTypes);
    }

    private void InitHexagons(HexagonType[] hexagonTypes)
    {
        GameObject hexagonPrefab = ObjManager.instance.hexagonPrefab;
        Transform container = ObjManager.instance.objectContainer;
        foreach (HexagonType hexagonType in hexagonTypes)
        {
            foreach(Hexagon hexagon in hexagonType.hexagons)
            {
                GameObject initializedHexagon = Instantiate(hexagonPrefab, container);

                initializedHexagon.name = hexagonType.color.ToString();
                initializedHexagon.transform.position = GridCellManager.instance.PositonToMove(hexagon.spawnPosition);

                MovingHexagon movingHexagon = initializedHexagon.GetComponent<MovingHexagon>();
                movingHexagon.SetMoveDirection(hexagon.moveDirection);
                movingHexagon.SetObjectColor(hexagonType.color);
            }
        }

        ObjManager.instance.SetPosList();
    }

    private void Update()
    {
        SetGameTime();
        gameScene.UpdateTimeLeftText(timeLeft);
        CheckLose();
    }

    public void CheckLose()
    {
        if (timeLeft <= 0 && !isGameLose && !isGameWin)
        {
            Lose();
        }
    }

    public void CheckWin(int numberOfBlock)
    {
        if(numberOfBlock == currentLevelData.hexagonTypes.Length && !isGameLose && !isGameWin)
        {
            int count = 0;
            foreach(Transform child in ObjManager.instance.objectContainer)
            {
                if(child.GetComponent<MovingHexagon>().IsPushable())
                {
                    count++;
                }
            }
            if(count == 0)
            {
                StartCoroutine(Win());
            }
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 0;
        isGameWin = true;
        achivement = SetAchivement();
        gameScene.ShowWinPanel();
    }

    private int SetAchivement()
    {
        return (int)((timeLeft / currentLevelData.time) * 3) + 1;
    }

    public void Lose()
    {
        isGameLose = true;

        achivement = 0;
        Time.timeScale = 0;
        gameScene.ShowLosePanel();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    private void SetGameTime()
    {
        timeLeft = timeLeft > 0 ? timeLeft - Time.deltaTime : 0;
    }
}
