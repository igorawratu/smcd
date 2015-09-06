using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTypeManager : MonoBehaviour {

    public enum Level
    {
        standard,
        flappyBird,
        lowGravity,
        gravityFlip,
        end
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
    private List<string> levelChangedList = new List<string>(new string[] { "JoinScene", "EndScene", "FlappyBirdScene", "GravityFlipScene", "LowGravityScene", "StandardScene" });

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
        fillLevelList();
    }
    void fillLevelList()
    {
        foreach(Level l in Level.GetValues(typeof(Level)))
        {
            if (l != Level.end)
            {
                levelList.Add(l);
            }
        }
    }
    void setNextLevel()
    {
        if (levelList.Count <= 0)
        {
            fillLevelList();
            _currentLevel = Level.end;
        }
        else
        {
            int index = Random.Range(0, levelList.Count);
            _currentLevel = levelList[index];
            levelList.RemoveAt(index);
        }
    }
	// Use this for initialization
	void Start () 
    {
        lastScene = Application.loadedLevel;
	}
    void sceneChanged()
    {
        //if (levelChangedList.Contains(Application.loadedLevelName))
        //{
           setNextLevel();
        //}
        /*else if (Application.loadedLevelName == "TitleScreen")
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

        }*/
    }
	// Update is called once per frame
	void Update () 
    {
        if (Application.loadedLevel != lastScene)
        {
            lastScene = Application.loadedLevel;
            
        }
	}
    public static void loadLevel()
    {
        SoundManager.instance.sceneChanged();
        instance.sceneChanged();
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
            case Level.end:
                CurrentPlayerKeys.Instance.playerScores.Clear();
                CurrentPlayerKeys.Instance.bunnies_killed.Clear();
                CurrentPlayerKeys.Instance.pots_smashed.Clear();
                CurrentPlayerKeys.Instance.powerups_acquired.Clear();
                CurrentPlayerKeys.Instance.distance_traveled.Clear();
                CurrentPlayerKeys.Instance.num_jumps.Clear();
                CurrentPlayerKeys.Instance.players.Clear();
                Application.LoadLevel("EndScene");
                break;
            default:
                break;
        }
    }
}
