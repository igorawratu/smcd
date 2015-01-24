using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {
    public GameObject platform;

	// Use this for initialization
	void Start () {
        mRng = new System.Random();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate(){
        if (mRng.Next(0, 100) < 10 && mTimeSinceLastPlatform > mTimeLimit)
        {
            int itemType = mRng.Next(0, mItems.Count);
            GameObject newItem = (GameObject)Instantiate(mItems[itemType]);

            if (mItems[itemType] == deadPlayer)
            {
                int chosenDeadPlayer = mRng.Next(0, mDeadPlayers.Count);
                DeadPlayer deadScript = newItem.GetComponent<DeadPlayer>();
                deadScript.setInfo(mDeadPlayers[chosenDeadPlayer].name, mDeadPlayers[chosenDeadPlayer].colour);
            }

            newItem.transform.position = new Vector2(Camera.main.transform.position.x + 30, 0.85f);
            mItemsList.Add(newItem);
            mTimeSinceLastObstacle = 0;

            List<GameObject> tempItemList = new List<GameObject>();
            List<GameObject> destroyList = new List<GameObject>();
            foreach (GameObject item in mItemsList)
            {
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

    private System.Random mRng;
    private float mTimeLimit;
    private float mTimeSinceLastObstacle;
    private List<GameObject> mPlatforms;
}
