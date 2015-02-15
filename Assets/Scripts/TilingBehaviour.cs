using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilingBehaviour : MonoBehaviour 
{
    public GameObject tilePrefab;
    public int numberOfTiles = 0;
    public int numberOfTilesY = 1;
    public bool tilingYFlipped = false;

    GameObject[,] tileArr;

    private Vector3 tileSize;
    public Vector2 speedMinMax = new Vector2(0.0f, 0.0f);
    
    public List<Sprite> spriteList;
    float timeElapsed = 0.0f;
    float currentSpeed = 0.0f;

	// Use this for initialization
	void Start () 
    {
        gameObject.name = tilePrefab.name + " tiled";
        tileArr = new GameObject[numberOfTiles, numberOfTilesY];
        tileSize = tilePrefab.transform.renderer.bounds.max - tilePrefab.transform.renderer.bounds.min;
        
        for (int i = 0; i < numberOfTiles; i++)
        {
            for (int j = 0; j < numberOfTilesY; j++)
            {
                tileArr[i,j] = (GameObject)Instantiate(tilePrefab);
                tileArr[i, j].transform.parent = transform;
                setSprite(tileArr[i, j]);

                Vector3 pos;
                if (!tilingYFlipped)
                {
                    pos = transform.position + new Vector3(tileSize.x * i, tileSize.y * -j, 0.0f);
                }
                else
                {
                    pos = transform.position + new Vector3(tileSize.x * i, tileSize.y * j, 0.0f);
                }
                tileArr[i, j].transform.position = pos;
                tileArr[i, j].transform.rotation = transform.rotation;
                tileArr[i, j].transform.localScale = tileArr[i, j].transform.localScale + new Vector3(0.01f, 0, 0);
            }
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine(levelSpeedController());
	}

    void setSprite(GameObject gObj)
    {
        if (spriteList.Count > 0)
        {
            int randomTile = Random.Range(0, spriteList.Count);
            SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
            sr.sprite = spriteList[randomTile];
        }
    }
    //// Update is called once per frame
    //void Update ()
    //{
        
	
    //}
    // Update is called once per frame
    void FixedUpdate()
    {
        rotateArray();
        for (int i = 0; i < numberOfTiles; i++)
        {
            for (int j = 0; j < numberOfTilesY; j++)
            {
                tileArr[i, j].transform.position = tileArr[i, j].transform.position + new Vector3(currentSpeed, 0, 0) * Time.deltaTime;
            }
        }
    }
    private IEnumerator levelSpeedController()
    {
        bool canUpdate = true;

        while (canUpdate)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed < VariableSpeed.speedUpDuration)
            {
                currentSpeed = Mathf.Lerp(speedMinMax.x, speedMinMax.y, timeElapsed / VariableSpeed.speedUpDuration);
                yield return null;
            }
            else
            {
                currentSpeed = speedMinMax.y;
                canUpdate = false;
            }
        }
    }

    void rotateArray()
    {
        for (int j = 0; j < numberOfTilesY; j++)
        {
            rotateArrayY(j);
        }
    }

    bool rotateArrayY(int j)
    {
        if (tileArr[0,j].transform.position.x < Camera.main.transform.position.x)
        {
            Vector3 worldSpaceLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,Camera.main.nearClipPlane));

            //if (!tileArr[0].transform.renderer.IsVisibleFrom(Camera.main))
            if (tileArr[0, j].transform.position.x < worldSpaceLeft.x - tileSize.x)
            {
                //get position of last floor piece
                Transform lastFloor = tileArr[numberOfTiles - 1, j].transform;

                tileArr[0, j].transform.position = 
                    new Vector3(lastFloor.position.x + tileSize.x,
                        tileArr[0, j].transform.position.y,
                        tileArr[0, j].transform.position.z);

                //Shuffle all the pieces down
                GameObject temp = tileArr[0, j];
                for (int i = 0; i < numberOfTiles - 1; i++)
                {
                    tileArr[i, j] = tileArr[i + 1, j];
                }
                tileArr[numberOfTiles - 1, j] = temp;
                return true;
                //break;
            }
        }
        return false;
    }
}
