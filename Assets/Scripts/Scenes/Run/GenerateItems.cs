using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum ObstacleSequence{S, B, SS, BB, SBS, BSB, SBSBS, BSBSB, BSBSBSB};

public class GenerateItems : MonoBehaviour {
    public GameObject deadPlayer;

    public List<GameObject> obstaclePrefabs;
    public List<GameObject> environmentalObjectPrefabs;
    public List<GameObject> powerupPrefabs;
    public List<List<int>> mObstacleSequences;

    public float mPowerupTimeLimit;
    public float mObstacleTimeLimit;
    public float mEnvObjTimeLimit;

    public float powerupYPosNearObstacleMin;
    public float powerupYPosNearObstacleMax;
    public float powerupYPosNotNearObstacleMin;
    public float powerupYPosNotNearObstacleMax;

    public float spawnAheadDistance;

    public float envObjXJitter;
    public float envObjYJitter;

    public int powerupProbabilityPerc;
    public int envobjProbabilityPerc;
    public int obstacleProbabilityPerc;

    public int obstacleTypeTimer;

    public float obstacleSequenceSpacing;

    public LevelTypeManager.Level levelType;

    private class DeadPlayerInfo:System.Object {
        public DeadPlayerInfo(KeyCode _name, Color _col) {
            name = _name;
            colour = _col;
        }
        public KeyCode name;
        public Color colour;

        public override bool Equals(System.Object o) {
            DeadPlayerInfo obj = o as DeadPlayerInfo;
            return (name == obj.name && colour == obj.colour);
        }
    }

    private class ObstacleSequenceInfo {
        public ObstacleSequenceInfo(List<int> _types, List<Vector2> _positions) {
            types = _types;
            positions = _positions;
        }

        public List<int> types;
        public List<Vector2> positions;
    }

	// Use this for initialization
	void Start () {
        mRng = new System.Random();
        mItems = new List<GameObject>();        
        mDeadPlayers = new List<DeadPlayerInfo>();
        mSpawnedItems = new List<GameObject>();

        mTimeSinceLastPowerup = 0;
        mTimeSinceLastEnvObject = 0;
        mTimeSinceLastObstacle = 0;
        mSequenceTimer = 0;
	}
	
	// Update is called once per frame
	void Update(){
	        
	}

	void FixedUpdate(){
        bool itemCreated = false;

        if(spawnObstacle())
            itemCreated = true;

        if(spawnEnvironmentalObject())
            itemCreated = true;

        if(spawnPowerup())
            itemCreated = true;

        if(itemCreated)
            cleanupSpawnedItems();
	}

    public void removeGameObject(GameObject _obj) {
        mSpawnedItems.Remove(_obj);
        Destroy(_obj);
    }

    public void playerDied(KeyCode _tag, Color _col, GameObject lastCollision) {
        GameObject wc = GameObject.Find("WinnerChecker");
        WinnerChecker wcscript = wc.GetComponent<WinnerChecker>();

        if(lastCollision != null && lastCollision.tag == "deadplayer" && wcscript.getNumPlayersActive() > 1) {
            KeyCode rpk;
            Color rpc;
            DeadPlayer rpscript = lastCollision.GetComponent<DeadPlayer>();
            rpscript.getInfo(out rpk, out rpc);
            DeadPlayerInfo respawnPlayerInfo = new DeadPlayerInfo(rpk, rpc);
            if(!mDeadPlayers.Contains(respawnPlayerInfo))
                obstaclePrefabs.Add(deadPlayer);
            else {
                mDeadPlayers.Remove(respawnPlayerInfo);
                GameObject ps = GameObject.Find("PlayerSpawner");
                PlayerSpawner psscript = ps.GetComponent<PlayerSpawner>();
                psscript.respawnPlayer(rpk, rpc);
            }

            mDeadPlayers.Add(new DeadPlayerInfo(_tag, _col));
        } else {
            mDeadPlayers.Add(new DeadPlayerInfo(_tag, _col));
            mItems.Add(deadPlayer);
        }
    }

    public void smashRock(GameObject _obj) {
        mSpawnedItems.Remove(_obj);
        Destroy(_obj);
    }

    private void cleanupSpawnedItems() {
        List<GameObject> tempItemList = new List<GameObject>();
        List<GameObject> destroyList = new List<GameObject>();
        foreach(GameObject item in mSpawnedItems) {
            float difx = Camera.main.transform.position.x - item.transform.position.x;
            if(difx < spawnAheadDistance)
                tempItemList.Add(item);
            else destroyList.Add(item);

            mSpawnedItems = tempItemList;
        }

        foreach(GameObject item in destroyList)
            Destroy(item);
    }

    private bool spawnObstacle() {
        if(obstaclePrefabs.Count == 0)
            return false;

        mTimeSinceLastObstacle += Time.deltaTime;
        mSequenceTimer += Time.deltaTime;

        if(mSequenceTimer > obstacleTypeTimer * mObstacleSequenceTypes && mObstacleSequenceTypes < mObstacleSequences.Count - 1) {
            mSequenceTimer = 0;
            mObstacleSequenceTypes++;
        }

        if(mRng.Next(0, 100) < obstacleProbabilityPerc && mTimeSinceLastObstacle > mObstacleTimeLimit) {
            int itemType = mRng.Next(0, obstaclePrefabs.Count);

            if(obstaclePrefabs[itemType] == deadPlayer) {
                GameObject newItem = (GameObject)Instantiate(obstaclePrefabs[itemType]);
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
                newItem.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance, mItems[itemType].transform.position.y);
                mSpawnedItems.Add(newItem);
            } 
            else {
                List<GameObject> newObstacles = generateObstacleSequence();
                mSpawnedItems.AddRange(newObstacles);
            }

            mTimeSinceLastObstacle = 0;
            return true;
        }

        return false;
    }

    private bool spawnPowerup() {
        if(powerupPrefabs.Count == 0)
            return false;

        mTimeSinceLastPowerup += Time.deltaTime;

        if(mRng.Next(0, 100) < powerupProbabilityPerc && mTimeSinceLastPowerup > mPowerupTimeLimit) {
            float max = mTimeSinceLastObstacle > 0.5 || (mObstacleTimeLimit - mTimeSinceLastObstacle) < 0.5 ? powerupYPosNearObstacleMax : powerupYPosNotNearObstacleMax;
            float min = mTimeSinceLastObstacle > 0.5 || (mObstacleTimeLimit - mTimeSinceLastObstacle) < 0.5 ? powerupYPosNearObstacleMin : powerupYPosNotNearObstacleMin;
            float ypos = (float)mRng.NextDouble() * (max - min) + min;

            int powerupType = mRng.Next(0, powerupPrefabs.Count);
            GameObject newPowerup = (GameObject)Instantiate(powerupPrefabs[powerupType]);

            newPowerup.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance, powerupPrefabs[powerupType].transform.position.y + ypos);
            mSpawnedItems.Add(newPowerup);

            mTimeSinceLastPowerup = 0;

            return true;
        }

        return false;
    }

    private bool spawnEnvironmentalObject(){
        if(environmentalObjectPrefabs.Count == 0)
            return false;

        mTimeSinceLastEnvObject += Time.deltaTime;

        if(mRng.Next(0, 100) < envobjProbabilityPerc && mTimeSinceLastEnvObject > mEnvObjTimeLimit)
        {
            float xJitterMax = envObjXJitter;
            float xJitterMin = -envObjXJitter;
            float yJitterMax = envObjYJitter;
            float yJitterMin = -envObjYJitter;

            float ypos = (float)mRng.NextDouble() * (yJitterMax - yJitterMin) + yJitterMin;
            float xpos = (float)mRng.NextDouble() * (xJitterMax - xJitterMin) + xJitterMin;

            int envObjType = mRng.Next(0, environmentalObjectPrefabs.Count);
            GameObject newEnvObj = (GameObject)Instantiate(environmentalObjectPrefabs[envObjType]);

            newEnvObj.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance + xpos, newEnvObj.transform.position.y + ypos);
            mSpawnedItems.Add(newEnvObj);
            mTimeSinceLastEnvObject = 0;

            return true;
        }
        return false;
       
    }

    private List<GameObject> generateObstacleSequence(){
        List<int> obstacleSequenceInf = mObstacleSequences[mRng.Next(0, mObstacleSequenceTypes)];
        List<GameObject> sequence = new List<GameObject>();


        float screenEndWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x;
        float generationBox = spawnAheadDistance - screenEndWorldPos;

        float spacing = 0;

        if(generationBox < 0)
            generationBox = spawnAheadDistance;

        if(obstacleSequenceInf.Count * obstacleSequenceSpacing < (generationBox * 2))
            spacing = obstacleSequenceSpacing;
        else spacing = (generationBox * 2) / obstacleSequenceInf.Count;

        float range = spacing * obstacleSequenceInf.Count;

        for(int k = 0; k < obstacleSequenceInf.Count; ++k ) {
            if(obstacleSequenceInf[k] >= obstaclePrefabs.Count)
                continue;

            float xSpawnPos = ((k * spacing + (k + 1) * spacing) / 2) + spawnAheadDistance - (range / 2);
            GameObject newObstacle = (GameObject)GameObject.Instantiate(obstaclePrefabs[obstacleSequenceInf[k]]);
            newObstacle.transform.position = new Vector2(xSpawnPos, obstaclePrefabs[obstacleSequenceInf[k]].transform.position.y);
            sequence.Add(newObstacle);
        }

        return sequence;
    }

    private List<GameObject> mItems;
    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    
    private float mSequenceTimer;

    private float mTimeSinceLastObstacle;
    private float mTimeSinceLastPowerup;
    private float mTimeSinceLastEnvObject;
    private List<GameObject> mSpawnedItems;
    private int mObstacleSequenceTypes;
}
