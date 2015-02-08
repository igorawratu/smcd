using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {

	//Array of floor pieces
    public GameObject floorPrefab;
    public GameObject groundPrefab;
    public GameObject skyPrefab;

    public int numFloorTiles = 3;
    public int numGroundTiles = 3;
    public int numSkyTiles = 3;
    GameObject[] floorArr = new GameObject[3];
    GameObject[] groundArr = new GameObject[3];
    GameObject[] skyArr = new GameObject[3];
    public MultiDimensionalSprite[] floorSprites = new MultiDimensionalSprite[1];
    public Sprite[] skySprites = new Sprite[1];

    [System.Serializable]
    public class MultiDimensionalSprite
    {
        public Sprite[] arr = new Sprite[6];
    }

    private Vector3 floorSize;
    private Vector3 groundSize;
    private Vector3 skySize;
    public Vector3 floorOffset = new Vector3(-20, 0, 0);
    public Vector3 skyOffset = new Vector3(-20, 0, 0);
    public Vector3 skySpeed = new Vector3(-0.2f, 0, 0);

	// Use this for initialization
	void Awake () {
        floorArr = new GameObject[numFloorTiles];
        groundArr = new GameObject[numGroundTiles];
        skyArr = new GameObject[numSkyTiles];

        floorSize = floorPrefab.transform.renderer.bounds.max - floorPrefab.transform.renderer.bounds.min;
        groundSize = groundPrefab.transform.renderer.bounds.max - groundPrefab.transform.renderer.bounds.min;
        //skySize = skyPrefab.transform.renderer.bounds.max - skyPrefab.transform.renderer.bounds.min;
	
        for (int i = 0; i < floorArr.Length; i++)
        {
            floorArr[i] = (GameObject)Instantiate(floorPrefab);
            setFloorSprite(floorArr[i]);
            
            Vector3 pos = floorOffset + new Vector3(floorSize.x * i, 0.0f, 0.0f);
            floorArr[i].transform.position = pos;
        }
        //groundArr = new GameObject[floorArr.Length];
        for (int i = 0; i < groundArr.Length; i++)
        {
            groundArr[i] = (GameObject)Instantiate(groundPrefab);
            Vector3 pos = floorOffset + new Vector3(groundSize.x * i, groundPrefab.transform.position.y, 0.0f);
            groundArr[i].transform.position = pos;
        }

        Sprite cloudSprite;
        switch (LevelTypeManager.currentLevel)
        {
            case LevelTypeManager.Level.standard:
                cloudSprite = skySprites[0];
                break;
            case LevelTypeManager.Level.evening:
                cloudSprite = skySprites[2];
                break;
            case LevelTypeManager.Level.sunset:
                cloudSprite = skySprites[1];
                break;
            case LevelTypeManager.Level.underground:
                cloudSprite = skySprites[3];
                break;
            default:
                cloudSprite = skySprites[0];
                break;
        }

        skySize = cloudSprite.bounds.max - cloudSprite.bounds.min;
        //skySize = skyArr[i].transform.renderer.bounds.max - skyArr[i].transform.renderer.bounds.min;
        for (int i = 0; i < skyArr.Length; i++)
        {
            skyArr[i] = (GameObject)Instantiate(skyPrefab);

            SpriteRenderer sr = skyArr[i].GetComponent<SpriteRenderer>();
            sr.sprite = cloudSprite;            


            Vector3 pos = skyOffset + new Vector3(skySize.x * i, skyPrefab.transform.position.y, 0.0f);
            skyArr[i].transform.position = pos;
            skyArr[i].transform.localScale = new Vector3(1.01f, 1, 1);
        }
    }

    void setFloorSprite(GameObject gObj)
    {
        int randomTile = Random.Range(0, floorSprites.Length);
        SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
        sr.sprite = floorSprites[0].arr[randomTile];
    }
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < skyArr.Length; i++)
        {
            //skyArr[i].transform.position = skyArr[i].transform.position + skySpeed * Time.deltaTime;
            skyArr[i].transform.position = skyArr[i].transform.position + skySpeed * Time.deltaTime;
        }
		
        if (rotateArray(floorArr, floorSize))
        {
            setFloorSprite(floorArr[floorArr.Length - 1]);
        }
        //this.
        rotateArray(groundArr, groundSize);
        rotateArray(skyArr, skySize);

	}

    bool rotateArray(GameObject[] arr, Vector3 tileSize)
    {
        if (arr[0].transform.position.x < Camera.main.transform.position.x)
        {
            if (!arr[0].transform.renderer.IsVisibleFrom(Camera.main))
            {
                //get position of last floor piece
                Transform lastFloor = arr[arr.Length - 1].transform;

                arr[0].transform.position = new Vector3(lastFloor.position.x + tileSize.x, arr[0].transform.position.y, arr[0].transform.position.z);

                //Shuffle all the pieces down
                GameObject temp = arr[0];
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    arr[i] = arr[i + 1];
                }
                arr[arr.Length - 1] = temp;
                return true;
            }
        }
        return false;
    }

}
