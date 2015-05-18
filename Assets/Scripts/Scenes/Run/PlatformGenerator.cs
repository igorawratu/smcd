using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour {
    public GameObject platform;

	// Use this for initialization
	void Start () {
        mRng = new System.Random();
        mTimeLimit = 2;
        mTimeSinceLastPlatform = 0;
        mPlatforms = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate(){
        mTimeSinceLastPlatform += Time.deltaTime;
        if (mRng.Next(0, 100) < 10 && mTimeSinceLastPlatform > mTimeLimit)
        {
            int itemType = mRng.Next(0, mPlatforms.Count);
            GameObject newItem = (GameObject)Instantiate(platform);

            newItem.transform.position = new Vector2(Camera.main.transform.position.x + 30, 2.5f);
            mPlatforms.Add(newItem);
            mTimeSinceLastPlatform = 0;

            List<GameObject> tempItemList = new List<GameObject>();
            List<GameObject> destroyList = new List<GameObject>();
            foreach (GameObject item in mPlatforms)
            {
                float difx = Camera.main.transform.position.x - item.transform.position.x;
                if (difx < 20)
                    tempItemList.Add(item);
                else destroyList.Add(item);

                mPlatforms = tempItemList;
            }

            foreach (GameObject item in destroyList)
                Destroy(item);
        }
    }

    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastPlatform;
    private List<GameObject> mPlatforms;
}
