using UnityEngine;
using System.Collections.Generic;

public class CurrentPlayerKeys : Singleton<CurrentPlayerKeys> {
	protected CurrentPlayerKeys () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	public List<KeyCode> playerKeys = new List<KeyCode>();
	public List<Color> playerColors = new List<Color>();
}