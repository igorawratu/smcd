using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilingBehaviour : MonoBehaviour 
{
    public GameObject tilePrefab;
    public int numberOfTiles = 0;

    GameObject[] tileArr;
    private Vector3 tileSize;


    public List<Sprite> spriteList;

	// Use this for initialization
	void Start () 
    {
        gameObject.name = tilePrefab.name + " tiled";
        tileArr = new GameObject[numberOfTiles];
        tileSize = tilePrefab.transform.renderer.bounds.max - tilePrefab.transform.renderer.bounds.min;
        
        for (int i = 0; i < numberOfTiles; i++)
        {
            tileArr[i] = (GameObject)Instantiate(tilePrefab);
            tileArr[i].transform.parent = transform;
            setSprite(tileArr[i]);

            Vector3 pos = transform.position + new Vector3(tileSize.x * i, 0.0f, 0.0f);
            tileArr[i].transform.position = pos;
            tileArr[i].transform.rotation = transform.rotation;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
	// Update is called once per frame
	void Update ()
    {
        rotateArray();
	
	}
    bool rotateArray()
    {
        if (tileArr[0].transform.position.x < Camera.main.transform.position.x)
        {
            Vector3 worldSpaceLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,Camera.main.nearClipPlane));

            //if (!tileArr[0].transform.renderer.IsVisibleFrom(Camera.main))
            if (tileArr[0].transform.position.x < worldSpaceLeft.x - tileSize.x)
            {
                //get position of last floor piece
                Transform lastFloor = tileArr[tileArr.Length - 1].transform;

                tileArr[0].transform.position = 
                    new Vector3(lastFloor.position.x + tileSize.x, 
                        tileArr[0].transform.position.y, 
                        tileArr[0].transform.position.z);

                //Shuffle all the pieces down
                GameObject temp = tileArr[0];
                for (int i = 0; i < tileArr.Length - 1; i++)
                {
                    tileArr[i] = tileArr[i + 1];
                }
                tileArr[tileArr.Length - 1] = temp;
                return true;
            }
        }
        return false;
    }
}
