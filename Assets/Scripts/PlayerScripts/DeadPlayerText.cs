using UnityEngine;
using System.Collections;

public class DeadPlayerText : MonoBehaviour {
    public Font font;


    void Awake() {

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setName(string _name) {
        mName = _name.Contains("Arrow") ? _name.Substring(0, _name.Length - 5) : _name;
        StartCoroutine(countDown());
    }

    private IEnumerator countDown() {
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        int i = 0;
        while(true){
            if (i++ % 2 == 1)
                tm.text = "";
            else tm.text = mName;
            yield return new WaitForSeconds(0.4f);
        }
    }

    private string mName;
}
