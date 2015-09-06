using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebriefSceneSCript : MonoBehaviour {
    private enum StatType
    {
        winner,
        bunnies,
        distance,
        pots,
        powerups,
        jumps
    }

    public GameObject PointElementPrefab;
    public GameObject PointElPanel;
    public Text HeaderText;
    public Image HeaderIcon;

    private List<StatType> stat_type_order;
    private int curr_stat_type = 0;

	// Use this for initialization
	void Start () {
        stat_type_order = new List<StatType>(new StatType[] { StatType.winner, StatType.distance, StatType.jumps,
            StatType.powerups, StatType.pots, StatType.bunnies});
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void displayFloatDat(string title, Dictionary<string, float> dat)
    {
        HeaderText.text = title;
        foreach (Transform child in PointElPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, float> player in dat)
        {
            GameObject player_score = (GameObject)Instantiate(PointElementPrefab);
            player_score.transform.parent = PointElPanel.transform;
            GameObject offset = player_score.transform.Find("offset").gameObject;
            GameObject points = offset.transform.Find("points").gameObject;
            Text pt_text = points.GetComponent<Text>();
            pt_text.text = player.Value.ToString();
        }
    }

    private void displayIntDat(string title, Dictionary<string, int> dat)
    {
        HeaderText.text = title;
        foreach (Transform child in PointElPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, int> player in dat)
        {
            GameObject player_score = (GameObject)Instantiate(PointElementPrefab);
            player_score.transform.parent = PointElPanel.transform;
            GameObject offset = player_score.transform.Find("offset").gameObject;
            GameObject points = offset.transform.Find("points").gameObject;
            Text pt_text = points.GetComponent<Text>();
            pt_text.text = player.Value.ToString();
        }
    }

    private void populateCanvas(){
        if (curr_stat_type >= stat_type_order.Count)
        {
            Application.LoadLevel("JoinScene");
        }
        else
        {
            StatType stat_type = stat_type_order[curr_stat_type++];
            switch (stat_type)
            {
                case StatType.winner:
                    displayIntDat("Total Number of Wins", CurrentPlayerKeys.Instance.playerScores);
                    break;
                case StatType.distance:
                    displayFloatDat("Total Distance Traveled", CurrentPlayerKeys.Instance.distance_traveled);
                    break;
                case StatType.bunnies:
                    displayIntDat("Bunnies Murdered", CurrentPlayerKeys.Instance.bunnies_killed);
                    break;
                case StatType.jumps:
                    displayIntDat("Total Jumps", CurrentPlayerKeys.Instance.num_jumps);
                    break;
                case StatType.pots:
                    displayIntDat("Pots Smashed", CurrentPlayerKeys.Instance.pots_smashed);
                    break;
                case StatType.powerups:
                    displayIntDat("Total Powerups", CurrentPlayerKeys.Instance.powerups_acquired);
                    break;
                default:
                    Debug.Log("WEEEEE I SHOULD NOT BE HERE");
                    break;
            }
        }
    }
}
