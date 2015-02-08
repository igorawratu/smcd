using UnityEngine;
using System.Collections;

public class LevelTypeManager : MonoBehaviour {

    public enum Level
    {
        standard,
        sunset,
        evening,
        underground
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
            System.Array values = Level.GetValues(typeof(Level));
            _currentLevel = (Level)values.GetValue(Random.Range(0, values.Length));
        }
        else if (Application.loadedLevelName == "RunScene")
        {
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {

        }
        else if (Application.loadedLevelName == "EndScene")
        {
            System.Array values = Level.GetValues(typeof(Level));
            _currentLevel = (Level)values.GetValue(Random.Range(0, values.Length));
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
}
