using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Turret : MonoBehaviour {

    [SerializeField]
    public GameObject Projectile;
    [SerializeField]
    public float FireDelay = 5f;


    private DigitalRuby.PyroParticles.FireBaseScript currentPrefabScript;
    //FirePosition child must be at index 0
    private Transform firePositionTransform;
    private IEnumerator shootCoroutine;

    // Use this for initialization
    void Start () {
        currentPrefabScript = Projectile.GetComponent<DigitalRuby.PyroParticles.FireBaseScript>();
        firePositionTransform = transform.GetChild(0).GetComponent<Transform>();

        shootCoroutine = WaitAndFire(FireDelay);
        StartCoroutine(shootCoroutine);

    }
	
	// Update is called once per frame
	void Update () {

		
	}

    // every 2 seconds perform the print()
    private IEnumerator WaitAndFire(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            GameObject currentPrefabObject = GameObject.Instantiate(Projectile, firePositionTransform);
            //currentPrefabScript = currentPrefabObject.GetComponent<DigitalRuby.PyroParticles.FireBaseScript>();

            currentPrefabObject.transform.position = firePositionTransform.transform.position;
            currentPrefabObject.transform.rotation = firePositionTransform.transform.rotation;

        }
    }

}
