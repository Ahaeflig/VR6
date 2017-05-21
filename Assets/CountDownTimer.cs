using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {


	[SerializeField]
	public float timerInSeconds = 180;

	[SerializeField]
	GameObject startLine;


	private IEnumerator timeCoroutine;

	private bool isGameFinished = false;

	// Use this for initialization
	void Start () {


		timeCoroutine = UpdateTimer();
		StartCoroutine(timeCoroutine);

	}
	
	// Update is called once per frame
	void Update () {
		

	}


	private IEnumerator UpdateTimer() {

		print (startLine.GetComponent<StartLineController> ().hasGameStarted);
		while (true) {
			
			while (!isGameFinished && startLine.GetComponent<StartLineController> ().hasGameStarted) {


				if (timerInSeconds < 30) {

					gameObject.GetComponent<TextMesh> ().color = Color.red;
				}

				if (--timerInSeconds < 0) {
					isGameFinished = true;

					NetworkServer.SendToAll (NetworkMessageType.GameOver, new EmptyMessage ());


				} else {
					gameObject.GetComponent<TextMesh> ().text = timerInSeconds.ToString ();
				}

				yield return new WaitForSeconds (1);

			}
			yield return new WaitForSeconds(1);
		}	
			
	}



}
