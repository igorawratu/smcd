using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTypeManager : MonoBehaviour {

    public enum Level
    {
        standard,
        flappyBird,
        lowGravity,
        gravityFlip
    }
    private static Level _currentLevel = Level.standard;
    public static Level currentLevel
    {
        get
        {
            return _currentLevel;
        }
    }

    private static LevelTypeManager _instance = null;

    private int lastScene = 0;
    public static LevelTypeManager instance
    {
        get
        {
            return _instance;
        }
    }
    private List<Level> levelList;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
        levelList = new List<Level>();
    }
    void fillLevelList()
    {
        foreach(Level l in Level.GetValues(typeof(Level)))
        {
            levelList.Add(l);
        }
    }
    void setNextLevel()
    {
        if(levelList.Count<=0)
        {
            fillLevelList();
        }
        int index = Random.Range(0, levelList.Count);
        _currentLevel = levelList[index];
        levelList.RemoveAt(index);
    }
	// Use this for initialization
	void Start () 
    {
        lastScene = Application.loadedLevel;
	}
    void sceneChanged()
    {
        if (Application.loadedLevelName == "JoinScene")
        {
           setNextLevel();
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {

        }
        else if (Application.loadedLevelName == "EndScene")
        {
            setNextLevel();
            //System.Array values = Level.GetValues(typeof(Level));
            //Level newLevel = (Level)values.GetValue(Random.Range(0, values.Length));

            //while(_currentLevel == newLevel)
            //{
            //    //Debug.Log(newLevel);
            //    //Debug.Log(_currentLevel);
            //    newLevel = (Level)values.GetValue(Random.Range(0, values.Length));
            //}
            //_currentLevel = newLevel;
        }
        else
        {

        }
    }
	// Update is called once per frame
	void Update () 
    {
        if (Application.loadedLevel != lastScene)
        {
            lastScene = Application.loadedLevel;
            SoundManager.instance.sceneChanged();
            sceneChanged();
        }
	}
    public static void loadLevel()
    {
        switch (_currentLevel)
        {
            case Level.flappyBird:
                Application.LoadLevel("FlappyBirdScene");
                break;
            case Level.gravityFlip:
                Application.LoadLevel("GravityFlipScene");
                break;
            case Level.lowGravity:
                Application.LoadLevel("LowGravityScene");
                break;
            case Level.standard:
                Application.LoadLevel("StandardScene");
                break;
        }
    }
}
