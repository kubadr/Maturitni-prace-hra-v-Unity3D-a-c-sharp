using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public int points;
	private int num;
	private int queue1 = 0;
	private int queue2 = 0;


	public Text pointsText;

	void Update() {
		pointsText.text = ("Points: " + points);
		Debug.Log (queue1);
		Debug.Log (queue2);
	}

	public IEnumerator addPoints(int value) {
		num = 0;
		queue2 += value;
		queue1 = queue2;
		while (num < queue1) {
			points++;
			num++;
			queue2--;
			yield return new WaitForSeconds (0.05f); 
		}
	}
}
