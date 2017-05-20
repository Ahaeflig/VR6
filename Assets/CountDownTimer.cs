using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {


	[SerializeField]
	public float timerInSeconds = 180;

	private IEnumerator timeCoroutine;

	private bool isGameFinished = false;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("UpdateTimer", 0, 1f);

		timeCoroutine = UpdateTimer();
		StartCoroutine(timeCoroutine);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private IEnumerator UpdateTimer() {
		
		while(!isGameFinished) {


			if (timerInSeconds < 30) {

				gameObject.GetComponent<TextMesh> ().color = Color.red;
			}

			if (--timerInSeconds < 0) {
				isGameFinished = true;

				NetworkServer.SendToAll(NetworkMessageType.GameOver, new EmptyMessage());


			} else {
				gameObject.GetComponent<TextMesh> ().text = timerInSeconds.ToString();
			}

			yield return new WaitForSeconds(1);

		}

	}


}
