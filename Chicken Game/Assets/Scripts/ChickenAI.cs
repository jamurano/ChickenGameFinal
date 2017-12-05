using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAI : MonoBehaviour {

	public float moveSpeed;
	public Transform target;
	public Transform chickenPen;
    private bool isCaught;

	public int points = 10;

    public ChickenAI() {
    }

    void Update()
    {
        //This part makes it so if the chicken falls off the platform (pushed through the ground), it will pop back up above ground
        // the -1 is how low it will fall befor it pops back up and the 0.5 is how high it starts before gravity brings it down to the ground.
        if(transform.position.z <= -1){
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
        }
        // This part allows player to touch the chicken in order to "pick it up"
        // The 2 is so the chicken isn't inside the player but infront of player
        if(isCaught){
            var player = GameObject.Find("Player");
            transform.position = player.transform.position + (player.transform.forward * 2);
        }
    }

    // Use this for initialization
    void OnTriggerStay(Collider other){

        if(other.gameObject.name == "Player" || other.gameObject.name.Contains("Wolf")){
			
			Debug.Log("Player has entered chickens trigger");
            //Wander script is active until player is in trigger, then Wanderscript is disabled 
            //so chicken can run without it stalling, then it is re-enabled
            var wanderScript = this.gameObject.GetComponent<WanderScript>();
            wanderScript.enabled = false;
			transform.LookAt(target);
			transform.Translate(Vector3.forward*-moveSpeed*Time.deltaTime);
            wanderScript.enabled = true;
		}
	}

	void OnCollisionEnter(Collision other){
        //If player picks up chicken, wanderscript is disabled so chicken wont move out of hands
        if(other.gameObject.name == "Player"){
            var wanderScript = this.gameObject.GetComponent<WanderScript>();
            wanderScript.enabled = false;
            isCaught = true;

        }
        //When player touches the pen walls, chicken is then collected and destroyed so wolf doesn't
        //jump into pen to kill chickens in there. This was a hilarious problem
        if(other.gameObject.name.Contains("Pen") && isCaught) {
            ScoreManager.AddPoints(points);
            Destroy(this.gameObject);
            //This part is for my chicken multiplying code
            var levelMaker = FindObjectOfType<LevelMaker>();
            levelMaker.ChickenRemoved();
        }
	}
}
