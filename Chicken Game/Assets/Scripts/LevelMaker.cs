using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaker : MonoBehaviour {

    public int chickenAmount;
    public int wolfAmount;
    public Rigidbody chicken;
    public Rigidbody wolf;

    private int chickensLeft = 0;

    public void Start()
    {
        CreateLevel();
    }


    public void CreateLevel(){

        AddChickens();
        var wolfSpawn = GameObject.Find("Wolf Spawn").transform;

        wolf.transform.position = wolfSpawn.position;
    }

    public void ChickenRemoved() {
        chickensLeft -= 1;

        Debug.Log(string.Format("Chickens left: {0}", chickensLeft));

        if(chickensLeft <= 0) {
            chickenAmount *= 2;

            CreateLevel();
        }
    }

    public void AddChickens() {
        for (int i = 0; i < chickenAmount; i++) {
            Vector3 position = GetRandomChickenLocation();

            var c = Instantiate(chicken, position, Random.rotation);

            var chickenAI = c.GetComponent<ChickenAI>();
            chickenAI.chickenPen = GameObject.Find("Chicken Pen").transform;
            chickenAI.target = GameObject.Find("Player").transform;
        }

        chickensLeft = chickenAmount;
    }

    public Vector3 GetRandomChickenLocation() {
        Vector3 position = new Vector3();

        var spawnPointBeginning = GameObject.Find("Chicken Spawn Begin").transform;
        var spawnPointEnd = GameObject.Find("Chicken Spawn End").transform;

        position.x = Random.Range(spawnPointBeginning.localPosition.x, spawnPointEnd.localPosition.x);
        position.y = Random.Range(spawnPointBeginning.localPosition.y, spawnPointEnd.localPosition.y);
        position.z = 0.5f;

        return position;
    }

		
	}
