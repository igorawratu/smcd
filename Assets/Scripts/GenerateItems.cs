using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateItems : MonoBehaviour {
    private class DeadPlayerInfo{
        public DeadPlayerInfo(string _name, Color _col){
            name = _name;
            colour = _col;
        }
        public string name;
        public Color colour;
    }

	public GameObject prefab;
    public GameObject deadPlayer;
    public GameObject rockbig;
    public GameObject rocksmall;
    public GameObject doubleJumpPU;
    public GameObject boostedJumpPU;
    public GameObject glidePU;
    public GameObject smashPU;

	// Use this for initialization
	void Start () {
        mItems = new List<GameObject>();
        mRng = new System.Random();
        mTimeLimit = 1;
        mTimeSinceLastObstacle = 0;
        mItemsList = new List<GameObject>();
        mPowerups = new List<GameObject>();
        mTimeLowerBound = 0.25f;
        mDeadPlayers = new List<DeadPlayerInfo>();
        mDecTimer = 0;
        mTimeSinceLastPowerup = 0;
        mPowerupTimeLimit = 2;

        mItems.Add(rockbig);
        mItems.Add(rocksmall);
        mPowerups.Add(doubleJumpPU);
        mPowerups.Add(boostedJumpPU);
        mPowerups.Add(glidePU);
        mPowerups.Add(smashPU);

        /*mItems.Add(deadPlayer);
        mItems.Add(deadPlayer);
        mDeadPlayers.Add(new DeadPlayerInfo("q", new Color(255, 0, 255)));*/
	}
	
	// Update is called once per frame
	void Update(){
	        
	}

	void FixedUpdate(){
        mTimeSinceLastObstacle += Time.deltaTime;
        mTimeSinceLastPowerup += Time.deltaTime;
        mDecTimer += Time.deltaTime;
        bool itemCreated = false;

        if(mDecTimer > 1 && mTimeLimit > mTimeLowerBound){
            mTimeLimit -= 0.01f;
            mDecTimer = 0;
        }

        if(mRng.Next(0, 1000) < 5 && mTimeSinceLastPowerup > mPowerupTimeLimit){
            float max = mTimeSinceLastObstacle > 0.25 ? 3 : 4;
            float min = mTimeSinceLastObstacle > 0.25 ? 0 : 2;
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
            GameObject newItem = (GameObject)Instantiate(mItems[itemType]);

            if (mItems[itemType] == deadPlayer){
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
            }

            newItem.transform.position = new Vector2(Camera.main.transform.position.x + 20, mItems[itemType].transform.position.y);
            mItemsList.Add(newItem);
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

    public void removeGameObject(GameObject _obj){
        mItemsList.Remove(_obj);
        Destroy(_obj);
    }

    public void playerDied(string _tag, Color _col){
        mDeadPlayers.Add(new DeadPlayerInfo(_tag, _col));
        mItems.Add(deadPlayer);
        //mItems.Add(deadPlayer);
    }

    public void smashRock(GameObject _obj){
        mItemsList.Remove(_obj);
        GameObject.Destroy(_obj);
    }

    private List<GameObject> mItems;
    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private float mTimeSinceLastPowerup;
    private float mPowerupTimeLimit;
    private float mTimeLowerBound;
    private List<GameObject> mItemsList;
    private List<GameObject> mPowerups;
    private float mDecTimer;
}
