using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Use this for initialization
	void Start () {
		rotate = true;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			Collect();
		}
	}

	public void Collect()
	{
		if (collectSound)
        {
			Debug.Log("Playing collect sound...");
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		}
				
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
