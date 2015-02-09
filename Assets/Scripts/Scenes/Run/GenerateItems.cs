using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum ObstacleSequence{S, B, SS, BB, SBS, BSB, SBSBS, BSBSB, BSBSBSB};

public class GenerateItems : MonoBehaviour {
    private class DeadPlayerInfo : System.Object{
        public DeadPlayerInfo(KeyCode _name, Color _col){
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

	public GameObject prefab;
    public GameObject deadPlayer;
    public GameObject rockbig;
    public GameObject rocksmall;
    public GameObject doubleJumpPU;
    public GameObject boostedJumpPU;
    public GameObject glidePU;
    public GameObject smashPU;
    public GameObject cloudObject;

	// Use this for initialization
	void Start () {
        mItems = new List<GameObject>();
        mRng = new System.Random();
        mTimeLimit = 2;
        mTimeSinceLastObstacle = 0;
        mItemsList = new List<GameObject>();
        mPowerups = new List<GameObject>();
        mTimeLowerBound = 0.25f;
        mDeadPlayers = new List<DeadPlayerInfo>();
        mDecTimer = 0;
        mTimeSinceLastPowerup = 0;
        mPowerupTimeLimit = 2;
        mTimeSinceLastCloud = 0;
        mCloudTimeLimit = 1;

        mItems.Add(rockbig);
        mItems.Add(rocksmall);
        mPowerups.Add(doubleJumpPU);
        mPowerups.Add(boostedJumpPU);
        mPowerups.Add(glidePU);
        mPowerups.Add(smashPU);

        initObstacleSequenceTypes();
	}
	
	// Update is called once per frame
	void Update(){
	        
	}

	void FixedUpdate(){
        //Debug.Log(Time.deltaTime);
        mTimeSinceLastObstacle += Time.deltaTime;
        mTimeSinceLastPowerup += Time.deltaTime;
        mTimeSinceLastCloud += Time.deltaTime;
        mDecTimer += Time.deltaTime;
        bool itemCreated = false;

        /*if(mDecTimer > 1 && mTimeLimit > mTimeLowerBound){
            mTimeLimit -= 0.01f;
            mDecTimer = 0;
        }*/

        if (mDecTimer > 3 * mObstacleTypes && mObstacleTypes < 9) {
            mDecTimer = 0;
            mObstacleTypes++;
        }

        spawnClouds();

        if(mRng.Next(0, 100) < 1 && mTimeSinceLastPowerup > mPowerupTimeLimit){
            float max = mTimeSinceLastObstacle > 0.5 || (mTimeLimit - mTimeSinceLastObstacle) < 0.5 ? 3 : 4;
            float min = mTimeSinceLastObstacle > 0.5 || (mTimeLimit - mTimeSinceLastObstacle) < 0.5 ? 1.5f : 2.5f;
            float ypos = (float)mRng.NextDouble() * (max - min) + min;

            int powerupType = mRng.Next(0, mPowerups.Count);
            GameObject newPowerup = (GameObject)Instantiate(mPowerups[powerupType]);

            newPowerup.transform.position = new Vector2(Camera.main.transform.position.x + 20, mPowerups[powerupType].transform.position.y + ypos);
            mItemsList.Add(newPowerup);
            mTimeSinceLastPowerup = 0;

            itemCreated = true;
        }
        
        if (mRng.Next(0, 100) < 2 && mTimeSinceLastObstacle > mTimeLimit){
            int itemType = mRng.Next(0, mItems.Count);
            
            if (mItems[itemType] == deadPlayer) {
                GameObject newItem = (GameObject)Instantiate(mItems[itemType]);
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
                newItem.transform.position = new Vector2(Camera.main.transform.position.x + 20, mItems[itemType].transform.position.y);
                mItemsList.Add(newItem);
            }
            else {
                List<GameObject> newObstacles = generateObstacleSequence();
                mItemsList.AddRange(newObstacles);
            }

            mTimeSinceLastObstacle = 0;
            itemCreated = true;
        }

        if(itemCreated){
            List<GameObject> tempItemList = new List<GameObject>();
            List<GameObject> destroyList = new List<GameObject>();
            foreach (GameObject item in mItemsList){
                float difx = Camera.main.transform.position.x - item.transform.position.x;
                if (difx < 20)
                    tempItemList.Add(item);
                else destroyList.Add(item);

                mItemsList = tempItemList;
            }

            foreach (GameObject item in destroyList)
                Destroy(item);
        }
	}

    public bool spawnClouds(){
        if (mRng.Next(0, 100) < 10 && mTimeSinceLastCloud > mCloudTimeLimit)
        {
            float max = 1;
            float min = -1;
            float ypos = (float)mRng.NextDouble() * (max - min) + min;
            float xpos = (float)mRng.NextDouble() * (max - min) + min;

            GameObject newCloud = (GameObject)Instantiate(cloudObject);

            newCloud.transform.position = new Vector2(Camera.main.transform.position.x + 15 + xpos, cloudObject.transform.position.y + ypos);
            mItemsList.Add(newCloud);
            mTimeSinceLastCloud = 0;

            return true;
        }
        return false;
       
    }

    public void removeGameObject(GameObject _obj){
        mItemsList.Remove(_obj);
        Destroy(_obj);
    }

    public void playerDied(KeyCode _tag, Color _col, GameObject lastCollision){
        GameObject wc = GameObject.Find("WinnerChecker");
        WinnerChecker wcscript = wc.GetComponent<WinnerChecker>();

        if(lastCollision != null && lastCollision.tag == "deadplayer" && wcscript.getNumPlayersActive() > 1) {
            KeyCode rpk;
            Color rpc;
            DeadPlayer rpscript = lastCollision.GetComponent<DeadPlayer>();
            rpscript.getInfo(out rpk, out rpc);
            DeadPlayerInfo respawnPlayerInfo = new DeadPlayerInfo(rpk, rpc);
            if(!mDeadPlayers.Contains(respawnPlayerInfo))
                mItems.Add(deadPlayer);
            else {
                mDeadPlayers.Remove(respawnPlayerInfo);
                GameObject ps = GameObject.Find("PlayerSpawner");
                PlayerSpawner psscript = ps.GetComponent<PlayerSpawner>();
                psscript.respawnPlayer(rpk, rpc);
            }

            mDeadPlayers.Add(new DeadPlayerInfo(_tag, _col)); 
        }
        else {
            mDeadPlayers.Add(new DeadPlayerInfo(_tag, _col));
            mItems.Add(deadPlayer);
        }
    }

    public void smashRock(GameObject _obj){
        mItemsList.Remove(_obj);
        Destroy(_obj);
    }

    private List<GameObject> generateObstacleSequence(){
        ObstacleSequenceInfo sInfo = mObstacleSequences[mPossibleSequences[mRng.Next(0, mObstacleTypes)]];
        List<GameObject> sequence = new List<GameObject>();

        for (int k = 0; k < sInfo.types.Count; ++k) {
            GameObject instantiateMe = sInfo.types[k] == 0 ? rocksmall : rockbig;
            GameObject newObstacle = (GameObject)Instantiate(instantiateMe);

            newObstacle.transform.position = new Vector2(Camera.main.transform.position.x + 20 + sInfo.positions[k].x, instantiateMe.transform.position.y + sInfo.positions[k].y);
            sequence.Add(newObstacle);
        }

        return sequence;
    }

    private void initObstacleSequenceTypes() {
        mObstacleTypes = 1;
        mPossibleSequences = new ObstacleSequence[9] { ObstacleSequence.S, ObstacleSequence.B, ObstacleSequence.SS, 
            ObstacleSequence.BB, ObstacleSequence.SBS, ObstacleSequence.BSB, ObstacleSequence.SBSBS, ObstacleSequence.BSBSB, ObstacleSequence.BSBSBSB };
        mObstacleSequences = new Dictionary<ObstacleSequence, ObstacleSequenceInfo>();
        mObstacleSequences[ObstacleSequence.S] = new ObstacleSequenceInfo(new List<int>(){0}, new List<Vector2>(){new Vector2(0, 0)});
        mObstacleSequences[ObstacleSequence.B] = new ObstacleSequenceInfo(new List<int>(){1}, new List<Vector2>(){new Vector2(0, 0)});
        mObstacleSequences[ObstacleSequence.BB] = new ObstacleSequenceInfo(new List<int>(){1, 1}, new List<Vector2>(){new Vector2(2.5f, 0), new Vector2(-2.5f, 0)});
        mObstacleSequences[ObstacleSequence.SS] = new ObstacleSequenceInfo(new List<int>(){0, 0}, new List<Vector2>(){new Vector2(2.5f, 0), new Vector2(-2.5f, 0)});
        mObstacleSequences[ObstacleSequence.BSB] = new ObstacleSequenceInfo(new List<int>(){1, 0, 1},
            new List<Vector2>(){new Vector2(-5, 0), new Vector2(0, 0), new Vector2(5, 0)});
        mObstacleSequences[ObstacleSequence.SBS] = new ObstacleSequenceInfo(new List<int>(){0, 1, 0}, 
            new List<Vector2>(){new Vector2(-5, 0), new Vector2(0, 0), new Vector2(5, 0)});
        mObstacleSequences[ObstacleSequence.SBSBS] = new ObstacleSequenceInfo(new List<int>(){0, 1, 0, 1, 0}, 
            new List<Vector2>(){new Vector2(-10, 0), new Vector2(-5, 0), new Vector2(0, 0), new Vector2(5, 0), new Vector2(10, 0)});
        mObstacleSequences[ObstacleSequence.BSBSB] = new ObstacleSequenceInfo(new List<int>(){1, 0, 1, 0, 1}, 
            new List<Vector2>(){new Vector2(-10, 0), new Vector2(-5, 0), new Vector2(0, 0), new Vector2(5, 0), new Vector2(10, 0)});
        mObstacleSequences[ObstacleSequence.BSBSBSB] = new ObstacleSequenceInfo(new List<int>(){1, 0, 1, 0, 1, 0, 1}, 
            new List<Vector2>(){new Vector2(-15, 0), new Vector2(-10, 0), new Vector2(-5, 0), new Vector2(0, 0), new Vector2(5, 0), new Vector2(10, 0), new Vector2(15, 0)});

    }

    private List<GameObject> mItems;
    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private float mTimeSinceLastPowerup;
    private float mPowerupTimeLimit;
    private float mTimeLowerBound;
    private float mTimeSinceLastCloud;
    private float mCloudTimeLimit;
    private List<GameObject> mItemsList;
    private List<GameObject> mPowerups;
    private List<GameObject> mClouds;
    private float mDecTimer;
    private Dictionary<ObstacleSequence, ObstacleSequenceInfo> mObstacleSequences;
    private ObstacleSequence[] mPossibleSequences;
    private int mObstacleTypes;
}
