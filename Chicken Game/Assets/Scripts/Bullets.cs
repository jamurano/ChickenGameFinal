using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {

	public int damage = 1;
	public int time = 5;

	// Use this for initialization
	void Start () {

		StartCoroutine(DestroyBullet());
		
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision other){

        Debug.Log("Collision enter");

		var hit = other.gameObject;
		var Health = hit.GetComponent<WolfHealth>();
		
		if(Health != null){
            Debug.Log("Wolf hit");

			Health.TakeDamage(damage);
		}
	}

	IEnumerator DestroyBullet(){
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}
