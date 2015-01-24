using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateItems : MonoBehaviour {
	public GameObject prefab;

	// Use this for initialization
	void Start () {
        mItems = new GameObject[1];
        mItems[0] = prefab;
        mRng = new System.Random();
        mTimeLimit = 2;
        mTimeSinceLastObstacle = 0;
        mItemsList = new List<GameObject>();
        mTimeLowerBound = 0.5f;
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
            

        if (mRng.Next(0, 100) < 1 && mTimeSinceLastObstacle > mTimeLimit){
            int itemType = mRng.Next(0, mItems.Length);
            GameObject newItem = (GameObject)Instantiate(mItems[itemType]);
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

    private GameObject[] mItems;
    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private float mTimeLowerBound;
    private List<GameObject> mItemsList;
}
