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

	// Use this for initialization
	void Start () {
        mItems = new List<GameObject>();
        mRng = new System.Random();
        mTimeLimit = 2;
        mTimeSinceLastObstacle = 0;
        mItemsList = new List<GameObject>();
        mTimeLowerBound = 0.5f;
        mItems.Add(prefab);
        mDeadPlayers = new List<DeadPlayerInfo>();
        mDeadPlayers.Add(new DeadPlayerInfo("s", new Color(0, 255, 0)));
        mItems.Add(deadPlayer);
        mItems.Add(deadPlayer);
	}
	
	// Update is called once per frame
	void Update(){
	        
	}

	void FixedUpdate(){
        mTimeSinceLastObstacle += Time.deltaTime;
        float tElapsed = Time.realtimeSinceStartup;
        int tElapsedInt = (int)tElapsed;
        float delta = tElapsed - (int)tElapsed;
        if (delta < 0)
            delta = -delta;
        float epsilon = 0.001f;

        if (delta < epsilon && mTimeLimit > mTimeLowerBound)
            mTimeLimit -= 0.01f;
            

        if (mRng.Next(0, 100) < 10 && mTimeSinceLastObstacle > mTimeLimit){
            int itemType = mRng.Next(0, mItems.Count);
            GameObject newItem = (GameObject)Instantiate(mItems[itemType]);

            if (mItems[itemType] == deadPlayer){
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
            }

            newItem.transform.position = new Vector2(Camera.main.transform.position.x + 3, 5);
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
        mItems.Add(deadPlayer);
    }

    private List<GameObject> mItems;
    private List<DeadPlayerInfo> mDeadPlayers;
    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private float mTimeLowerBound;
    private List<GameObject> mItemsList;
}
