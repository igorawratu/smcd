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

	// Use this for initialization
	void Start () {
        mItems = new List<GameObject>();
        mRng = new System.Random();
        mTimeLimit = 1;
        mTimeSinceLastObstacle = 0;
        mItemsList = new List<GameObject>();
        mTimeLowerBound = 0.25f;
        mDeadPlayers = new List<DeadPlayerInfo>();
        mDecTimer = 0;

        /*mItems.Add(rockbig);
        mItems.Add(rocksmall);
        mItems.Add(deadPlayer);
        mItems.Add(deadPlayer);*/
        mItems.Add(doubleJumpPU);
        mItems.Add(boostedJumpPU);

        //mDeadPlayers.Add(new DeadPlayerInfo("q", new Color(255, 255, 255)));
	}
	
	// Update is called once per frame
	void Update(){
	        
	}

	void FixedUpdate(){
        mTimeSinceLastObstacle += Time.deltaTime;
        mDecTimer += Time.deltaTime;

        if(mDecTimer > 1 && mTimeLimit > mTimeLowerBound){
            mTimeLimit -= 0.01f;
            mDecTimer = 0;
        }   

        if (mRng.Next(0, 100) < 3 && mTimeSinceLastObstacle > mTimeLimit){
            int itemType = mRng.Next(0, mItems.Count);
            GameObject newItem = (GameObject)Instantiate(mItems[itemType]);

            if (mItems[itemType] == deadPlayer){
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
            }

            newItem.transform.position = new Vector2(Camera.main.transform.position.x + 30, 0.85f);
            mItemsList.Add(newItem);
            mTimeSinceLastObstacle = 0;

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

    private List<GameObject> mItems;
    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private float mTimeLowerBound;
    private List<GameObject> mItemsList;
    private float mDecTimer;
}
