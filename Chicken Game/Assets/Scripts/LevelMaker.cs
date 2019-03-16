using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMaker : MonoBehaviour {

    public int chickenAmount;
    public int wolfAmount;
    public Rigidbody chicken;
    public Rigidbody wolf;
    public Text chickenText;
    public Transform wolfSpawn;

    public static int chickensLeft = 0;

    public void Start()
    {
        CreateLevel();
    }


    public void CreateLevel(){

        AddChickens();
        RemoveWolves();
        AddWolves();
    }

    private void RemoveWolves()
    {
        var wolves = GameObject.FindGameObjectsWithTag("wolf");
        foreach(var wolf in wolves)
        {
            Destroy(wolf);
        }
    }

    private void AddWolves()
    {
        for(int i = 0; i < wolfAmount; i++)
        {
            Vector3 location = wolfSpawn.position;
            location.z = location.z + (wolf.transform.localScale.z * i);
            var w = Instantiate(wolf, location, Quaternion.Euler(Vector3.forward));
            w.GetComponent<WolfHealth>().spawnPoint = wolfSpawn;
            w.GetComponent<WolfAI>().spawn = wolfSpawn;
        }
    }

    public void ChickenRemoved() {
        chickensLeft -= 1;
        chickenText.text = chickensLeft.ToString();
        Debug.Log(string.Format("Chickens left: {0}", chickensLeft));

        if(chickensLeft <= 0) {
            chickenAmount *= 2;
            wolfAmount++;

            CreateLevel();
        }
    }

    public void AddChickens() {
        for (int i = 0; i < chickenAmount; i++) {
            Vector3 position = GetRandomChickenLocation();

            var c = Instantiate(chicken, position, UnityEngine.Random.rotation);

            var chickenAI = c.GetComponent<ChickenAI>();
            chickenAI.chickenPen = GameObject.Find("Chicken Pen").transform;
            chickenAI.target = GameObject.Find("Player").transform;
        }

        chickensLeft = chickenAmount;
        chickenText.text = chickensLeft.ToString();
    }

    public Vector3 GetRandomChickenLocation() {
        Vector3 position = new Vector3();

        var spawnPointBeginning = GameObject.Find("Chicken Spawn Begin").transform;
        var spawnPointEnd = GameObject.Find("Chicken Spawn End").transform;

        position.x = UnityEngine.Random.Range(spawnPointBeginning.localPosition.x, spawnPointEnd.localPosition.x);
        position.y = UnityEngine.Random.Range(spawnPointBeginning.localPosition.y, spawnPointEnd.localPosition.y);
        position.z = 0.5f;

        return position;
    }

		
	}
