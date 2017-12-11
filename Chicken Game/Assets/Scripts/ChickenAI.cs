using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAI : MonoBehaviour {

	public float moveSpeed;
	public Transform target;
	public Transform chickenPen;
    public int jumpHeight;
    private bool isCaught;
    private bool canRun = true;

	public int points = 10;

    public ChickenAI() {
    }

    void Update()
    {
        //This part makes it so if the chicken falls off the platform (pushed through the ground), it will pop back up above ground
        // the -1 is how low it will fall befor it pops back up and the 0.5 is how high it starts before gravity brings it down to the ground.
        if(transform.position.y <= -1){
            var chickenStart = GameObject.Find("Chicken Spawn Begin").gameObject.transform;
            var chickenEnd = GameObject.Find("Chicken Spawn End").gameObject.transform;

            transform.position = new Vector3(Random.Range(chickenStart.position.x, chickenEnd.position.x),
                                             0.5f, Random.Range(chickenStart.position.z, chickenEnd.position.z));

        }
        // This part allows player to touch the chicken in order to "pick it up"
        // The 2 is so the chicken isn't inside the player but infront of player
        if(isCaught){
            var player = GameObject.Find("Player");
            transform.position = player.transform.position + (player.transform.forward * 2);
            //Debug.Log(transform.position);
        }
    }

    // Use this for initialization
    void OnTriggerStay(Collider other){

        if(canRun && (other.gameObject.name == "Player" || other.gameObject.name.Contains("Wolf"))){
			
			//Debug.Log("Player has entered chickens trigger");
            //Wander script is active until player is in trigger, then Wanderscript is disabled 
            //so chicken can run without it stalling, then it is re-enabled
            var wanderScript = this.gameObject.GetComponent<WanderScript>();
            wanderScript.enabled = false;
			transform.LookAt(target);
			transform.Translate(Vector3.forward*-moveSpeed*Time.deltaTime);
            //wanderScript.enabled = true;
		}
        else if (other.gameObject.tag == "wall") {
            StartCoroutine(JumpBack(other.transform));
        }
	}

    private void OnTriggerExit(Collider other)
    {
        var wanderScript = gameObject.GetComponent<WanderScript>();
        if(!wanderScript.enabled) {
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
        else if(other.gameObject.name.Contains("Pen") && isCaught) {
            ScoreManager.AddPoints(points);
            Destroy(this.gameObject);
            //This part is for my chicken multiplying code
            var levelMaker = FindObjectOfType<LevelMaker>();
            levelMaker.ChickenRemoved();
        }
	}

    private IEnumerator JumpBack(Transform other) {
        canRun = false;
        transform.Translate((transform.up * jumpHeight * Time.deltaTime) + (transform.forward * moveSpeed * Time.deltaTime));
        yield return new WaitForSeconds(2);
        canRun = true;
    }
}
