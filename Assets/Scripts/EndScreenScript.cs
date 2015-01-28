using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndScreenScript : MonoBehaviour {
    public Text title;
    public Text winner;
    public Text resetTimer;
    public Text record;
    public Text timeLeft;
    public GameObject textPrefab;


	// Use this for initialization
	void Start () {
        title.text = "GAME OVER:";
        string win = CurrentPlayerKeys.Instance.lastWinner;
        winner.text = "PLAYER " + win + " WINS";
        record.text = "RECORD:";
        float offSetNum = 30;

        List<KeyValuePair<string, int>> sortedScores = new List<KeyValuePair<string, int>>();
        foreach (KeyValuePair<string, int> player in CurrentPlayerKeys.Instance.playerScores){
            sortedScores.Add(player);
        }
        sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        foreach (KeyValuePair<string, int> player in sortedScores){
            GameObject canvas = GameObject.Find("Canvas");
            GameObject scoreObj = (GameObject)Instantiate(textPrefab);
            scoreObj.transform.SetParent(canvas.transform);
            Text score = scoreObj.GetComponent<Text>();
            //Debug.Log(record.rectTransform.position.x);
            //Debug.Log(record.rectTransform.position.y - 30);
            score.rectTransform.position = new Vector3(record.rectTransform.position.x - 15, record.rectTransform.position.y - offSetNum, 0);
            offSetNum += 30;
            score.text = "PLAYER " + player.Key + ": " + player.Value;
        }
        mBusy = false;
        StartCoroutine(countDown());
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKey)
            Application.LoadLevel("TestScene");
	}

    private IEnumerator countDown()
    {
        for (int i = 10; i >= 0; i--){
            if(i % 2 == 1)
                resetTimer.text = "";
            else resetTimer.text = "PRESS ANY KEY TO CONTINUE";
            timeLeft.text = "TIME LEFT: " + i;
            yield return new WaitForSeconds(1);
        }

        Application.LoadLevel(0);
    }

    bool mBusy;
}
