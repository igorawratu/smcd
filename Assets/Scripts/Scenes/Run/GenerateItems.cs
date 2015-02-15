using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateItems : MonoBehaviour {
    public GameObject deadPlayer;

    public List<GameObject> obstaclePrefabs;
    public List<EnvironmentObject> environmentalObjectPrefabs;
    public List<GameObject> powerupPrefabs;
    public List<ObstacleSequenceInfo> obstacleSequences;
    public List<GameObject> consistentObjects;

    public float powerupTimeLimit;
    public float obstacleTimeLimit;
    public float envObjTimeLimit;
    public float cObjTimeLimit;

    public float puYNearObsMin;
    public float puYNearObsMax;
    public float puYPosFarObsMin;
    public float puYPosFarObsMax;

    public float spawnAheadDistance;

    public int puPercChance;
    public int envObjPercChance;
    public int obsPercChance;

    public int obstacleTypeTimer;

    public float obsSeqSpacing;

    public float consistentObjDistance;

    

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

    [System.Serializable]
    public class ObstacleSequenceInfo {
        public List<int> types;
        public List<float> yoffset;
    }

    [System.Serializable]
    public class EnvironmentObject {
        public GameObject envObj;
        public float envObjXJitter;
        public float envObjYJitter;
    }

	// Use this for initialization
	void Start () {
        mRng = new System.Random();    
        mDeadPlayers = new List<DeadPlayerInfo>();
        mSpawnedItems = new List<GameObject>();

        mTimeSinceLastPowerup = 0;
        mTimeSinceLastEnvObject = 0;
        mTimeSinceLastObstacle = 0;
        mSequenceTimer = 0;
        mObstacleSequenceTypes = 1;
        mTimeSinceLastConsObj = 0;
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

        if(spawnConsistenObject())
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
            obstaclePrefabs.Add(deadPlayer);
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
            if(item == null)
                continue;
            float difx = Camera.main.transform.position.x - item.transform.position.x;
            if(difx < spawnAheadDistance)
                tempItemList.Add(item);
            else destroyList.Add(item);

            
        }
        mSpawnedItems = tempItemList;

        foreach(GameObject item in destroyList) {
            Destroy(item);
        }
    }

    private bool spawnObstacle() {
        if(obstaclePrefabs.Count == 0)
            return false;

        mTimeSinceLastObstacle += Time.deltaTime;
        mSequenceTimer += Time.deltaTime;

        if(mSequenceTimer > obstacleTypeTimer * mObstacleSequenceTypes && mObstacleSequenceTypes < obstacleSequences.Count) {
            mSequenceTimer = 0;
            mObstacleSequenceTypes++;
        }

        if(mRng.Next(0, 100) < obsPercChance && mTimeSinceLastObstacle > obstacleTimeLimit) {
            int itemType = mRng.Next(0, obstaclePrefabs.Count);

            if(obstaclePrefabs[itemType] == deadPlayer) {
                GameObject newItem = (GameObject)Instantiate(obstaclePrefabs[itemType]);
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
                newItem.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance, deadPlayer.transform.position.y);
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

        if(mRng.Next(0, 100) < puPercChance && mTimeSinceLastPowerup > powerupTimeLimit) {
            float max = mTimeSinceLastObstacle > 0.5 || (obstacleTimeLimit - mTimeSinceLastObstacle) < 0.5 ? puYNearObsMax : puYPosFarObsMax;
            float min = mTimeSinceLastObstacle > 0.5 || (obstacleTimeLimit - mTimeSinceLastObstacle) < 0.5 ? puYNearObsMin : puYPosFarObsMin;
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

        if(mRng.Next(0, 100) < envObjPercChance && mTimeSinceLastEnvObject > envObjTimeLimit)
        {
            int envObjType = mRng.Next(0, environmentalObjectPrefabs.Count);

            float xJitterMax = environmentalObjectPrefabs[envObjType].envObjXJitter;
            float xJitterMin = -environmentalObjectPrefabs[envObjType].envObjXJitter;
            float yJitterMax = environmentalObjectPrefabs[envObjType].envObjYJitter;
            float yJitterMin = -environmentalObjectPrefabs[envObjType].envObjYJitter;

            float ypos = (float)mRng.NextDouble() * (yJitterMax - yJitterMin) + yJitterMin;
            float xpos = (float)mRng.NextDouble() * (xJitterMax - xJitterMin) + xJitterMin;

            GameObject newEnvObj = (GameObject)Instantiate(environmentalObjectPrefabs[envObjType].envObj);

            newEnvObj.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance + xpos, newEnvObj.transform.position.y + ypos);
            mSpawnedItems.Add(newEnvObj);
            mTimeSinceLastEnvObject = 0;

            return true;
        }
        return false;
       
    }

    private bool spawnConsistenObject() {
        if(consistentObjects.Count == 0)
            return false;

        mTimeSinceLastConsObj += Time.deltaTime;

        if(mTimeSinceLastConsObj > cObjTimeLimit) {
            int objToSpawn = mRng.Next(0, consistentObjects.Count);

            Vector2 rayFrom = new Vector2(Camera.main.transform.position.x + spawnAheadDistance, consistentObjects[objToSpawn].transform.position.y + 1);
            Vector2 rayDir = new Vector2(1, 0);

            bool obsToLeft = Physics2D.Raycast(rayFrom, -rayDir, consistentObjDistance);
            bool obsToRight = Physics2D.Raycast(rayFrom, rayDir, consistentObjDistance);

            if(!(obsToLeft || obsToRight)) {
                GameObject newConsObj = (GameObject)Instantiate(consistentObjects[objToSpawn]);

                newConsObj.transform.position = new Vector2(Camera.main.transform.position.x + spawnAheadDistance, consistentObjects[objToSpawn].transform.position.y);
                mSpawnedItems.Add(newConsObj);

                mTimeSinceLastConsObj = 0;

                return true;
            }
            
            return false;
        }
        
        return false;

    }

    private List<int> generateSequenceFromLong(int _inSeq) {
        int mod = _inSeq;
        List<int> seq = new List<int>();

        while(mod != 0) {
            int num = mod % 10;
            seq.Insert(0, num);
            mod /= 10;
        }

        return seq;
    }

    private List<GameObject> generateObstacleSequence(){
        ObstacleSequenceInfo sInf = obstacleSequences[mRng.Next(0, mObstacleSequenceTypes)];
        List<int> obstacleSequenceInf = sInf.types;
        List<float> yOffsets = sInf.yoffset;
        List<GameObject> sequence = new List<GameObject>();


        float screenEndWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x;
        float midScreenPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane)).x;
        float generationBox = midScreenPos + spawnAheadDistance - screenEndWorldPos;

        float spacing = 0;

        if(generationBox < 0)
            generationBox = spawnAheadDistance;

        if(obstacleSequenceInf.Count * obsSeqSpacing < (generationBox * 2))
            spacing = obsSeqSpacing;
        else spacing = (generationBox * 2) / obstacleSequenceInf.Count;

        float range = spacing * obstacleSequenceInf.Count;

        for(int k = 0; k < obstacleSequenceInf.Count; ++k ) {
            if(obstacleSequenceInf[k] > obstaclePrefabs.Count)
                continue;

            float xSpawnPos = ((k * spacing + (k + 1) * spacing) / 2) + midScreenPos + spawnAheadDistance - (range / 2);
            GameObject newObstacle = (GameObject)GameObject.Instantiate(obstaclePrefabs[obstacleSequenceInf[k]]);
            newObstacle.transform.position = new Vector2(xSpawnPos, obstaclePrefabs[obstacleSequenceInf[k]].transform.position.y + yOffsets[k]);
            sequence.Add(newObstacle);
        }

        return sequence;
    }

    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    
    private float mSequenceTimer;

    private float mTimeSinceLastObstacle;
    private float mTimeSinceLastPowerup;
    private float mTimeSinceLastEnvObject;
    private List<GameObject> mSpawnedItems;
    private int mObstacleSequenceTypes;
    private float mTimeSinceLastConsObj;
}
