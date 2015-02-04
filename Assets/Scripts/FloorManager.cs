using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {

	//Array of floor pieces
    public GameObject floorPrefab;
    public GameObject groundPrefab;
    public GameObject skyPrefab;
    public GameObject[] floorArr = new GameObject[3];
    GameObject[] groundArr;
    public GameObject[] skyArr;
    public Sprite[] floorSprites = new Sprite[3];

    private Vector3 floorSize;
    private Vector3 groundSize;
    private Vector3 skySize;
    public Vector3 floorOffset = new Vector3(-20, 0, 0);
    public Vector3 skyOffset = new Vector3(-20, 0, 0);
    public Vector3 skySpeed = new Vector3(-0.2f, 0, 0);

	// Use this for initialization
	void Start () {


        floorSize = floorPrefab.transform.renderer.bounds.max - floorPrefab.transform.renderer.bounds.min;
        groundSize = groundPrefab.transform.renderer.bounds.max - groundPrefab.transform.renderer.bounds.min;
        skySize = skyPrefab.transform.renderer.bounds.max - skyPrefab.transform.renderer.bounds.min;
	
        for (int i = 0; i < floorArr.Length; i++)
        {
            floorArr[i] = (GameObject)Instantiate(floorPrefab);
            setFloorSprite(floorArr[i]);
            
            Vector3 pos = floorOffset + new Vector3(floorSize.x * i, 0.0f, 0.0f);
            floorArr[i].transform.position = pos;
        }
        groundArr = new GameObject[floorArr.Length];
        for (int i = 0; i < floorArr.Length; i++)
        {
            groundArr[i] = (GameObject)Instantiate(groundPrefab);
            Vector3 pos = floorOffset + new Vector3(groundSize.x * i, groundPrefab.transform.position.y, 0.0f);
            groundArr[i].transform.position = pos;
        }

        for (int i = 0; i < skyArr.Length; i++)
        {
            skyArr[i] = (GameObject)Instantiate(skyPrefab);
            Vector3 pos = skyOffset + new Vector3(skySize.x * i, skyPrefab.transform.position.y, 0.0f);
            skyArr[i].transform.position = pos;
        }
    }

    void setFloorSprite(GameObject gObj)
    {
        int randomTile = Random.Range(0, floorSprites.Length);
        SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
        sr.sprite = floorSprites[randomTile];
    }
	// Update is called once per frame
	void Update () {
		if (floorArr[0].transform.position.x < Camera.main.transform.position.x) {
			if (!floorArr[0].transform.renderer.IsVisibleFrom(Camera.main)) {
				//get position of last floor piece
				Transform lastFloor = floorArr[floorArr.Length-1].transform;

                floorArr[0].transform.position = new Vector3(lastFloor.position.x + floorSize.x, floorArr[0].transform.position.y, floorArr[0].transform.position.z);

				//Shuffle all the pieces down
				GameObject temp = floorArr[0];
				for (int i=0; i < floorArr.Length - 1; i++) {
					floorArr[i] = floorArr[i + 1];
				}
				floorArr[floorArr.Length - 1] = temp;

                setFloorSprite(floorArr[floorArr.Length - 1]);
			}
		}

        rotateArray(groundArr, groundSize);
        rotateArray(skyArr, skySize);
        for (int i = 0; i < skyArr.Length; i++)
        {
            //skyArr[i].transform.position = skyArr[i].transform.position + skySpeed * Time.deltaTime;
            skyArr[i].transform.position = skyArr[i].transform.position + skySpeed * Time.deltaTime;
        }

	}
    void rotateArray(GameObject[] arr, Vector3 tileSize)
    {
        if (arr[0].transform.position.x < Camera.main.transform.position.x)
        {
            if (!arr[0].transform.renderer.IsVisibleFrom(Camera.main))
            {
                //get position of last floor piece
                Transform lastFloor = arr[arr.Length - 1].transform;

                arr[0].transform.position = new Vector3(lastFloor.position.x + tileSize.x - 0.1f, arr[0].transform.position.y, arr[0].transform.position.z);

                //Shuffle all the pieces down
                GameObject temp = arr[0];
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    arr[i] = arr[i + 1];
                }
                arr[arr.Length - 1] = temp;
            }
        }
    }

}
