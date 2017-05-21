using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour {

	[SerializeField]
	GameObject projectilePrefab;

	[SerializeField]
	public float reloadTime = 3;

	[SerializeField]
	GameObject shootPosition;

	[SerializeField]
	float projSpeed = 1000;

	private IEnumerator shootCoroutine;

	private bool hasFired = false;



	// Use this for initialization
	void Start () {
		shootCoroutine = WaitAndFire();
		StartCoroutine(shootCoroutine);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Fire() {

		if (!hasFired) {
			hasFired = true;

			GameObject proj = Instantiate (projectilePrefab);
			proj.transform.position = shootPosition.transform.position;
			proj.transform.rotation = shootPosition.transform.rotation;
			//Right because object importedf in wrong orientation
			proj.GetComponent<Rigidbody> ().AddForce (transform.forward * projSpeed);



		}
	}

	public void GetPressDown(Vector3 position) {
	
		this.gameObject.transform.position = position;
	
	}


	// every 2 seconds perform the print()
	private IEnumerator WaitAndFire()
	{
		while (true)
		{
			if (hasFired) {
				yield return new WaitForSeconds (reloadTime);

				hasFired = false;
			} else {
				yield return new WaitForEndOfFrame ();
			}
				
		}
	}
}
