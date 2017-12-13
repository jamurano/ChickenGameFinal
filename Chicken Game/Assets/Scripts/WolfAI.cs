using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WolfAI : MonoBehaviour {
	public float moveSpeed;
	public int damage;

    public Transform spawn;

    private WanderScript _wanderScript;
    private WanderScript WanderScript {
        get {
            if(_wanderScript == null) {
                _wanderScript = GetComponent<WanderScript>();
            }
            return _wanderScript;
        }
    }
    private bool pauseTrigger = false;

    private void Update()
    {
        if (transform.position.y <= -1)
        {
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }
    }

    // Use this for initialization
    void OnTriggerStay(Collider other){

        //Debug.Log(string.Format("Wolf trigger {0} - paused {1}", other.gameObject.name, pauseTrigger));

        if(!pauseTrigger && (other.gameObject.name == "Player" || other.gameObject.name.Contains("Chicken"))){
            
            WanderScript.enabled = false;
			//Debug.Log("Player or Chicken has entered wolfs trigger");
            transform.LookAt(other.transform);
			transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
            //WanderScript.enabled = true;
		}
	}

    private void OnTriggerExit(Collider other)
    {
        if (!WanderScript.enabled) {
            WanderScript.enabled = true;
        }
    }

    void OnCollisionEnter(Collision other)
	{
        StartCoroutine(HandleHitRoutine(other)); 
	}

    IEnumerator HandleHitRoutine(Collision other)
    {
        bool doWait = false;
        var hit = other.gameObject;
        float wolfWaitTime = 0f;

        if (hit.name == "Player")
        {
            Debug.Log("Player hit");
            var playerHealth = FindObjectOfType<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            wolfWaitTime = 1f;
            doWait = true;
        }

        var chicken = hit.gameObject;

        if(chicken != null && chicken.name.Contains("Chicken")) {
            Debug.Log("Chicken hit");

            Object.Destroy(chicken);
            var levelMaker = FindObjectOfType<LevelMaker>();
            levelMaker.ChickenRemoved();
            Debug.Log("Wolf is waiting");
            wolfWaitTime = 5f;
            doWait = true;

            ScoreManager.SubtractPoints(5);
        }

        if(doWait) {
            Debug.Log("Wolf waiting.");
            WanderScript.enabled = false;
            pauseTrigger = true;
            yield return new WaitForSeconds(wolfWaitTime);
            WanderScript.enabled = true;
            pauseTrigger = false;
        }
    }

}