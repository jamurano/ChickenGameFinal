using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	public const int maxHealth = 25;
	public int currentHealth = maxHealth;

    public int levelToLoad;

	public Text hp;
	public Text maxHP;
	
	// Update is called once per frame
	void Update () {
		hp.text = currentHealth.ToString();
		maxHP.text = maxHealth.ToString();
		
	}

	public void TakeDamage(int amount){
        Debug.Log(string.Format("Take damage {0}", currentHealth));
		currentHealth -= amount;
		if(currentHealth <= 0){
			currentHealth=0;
            SceneManager.LoadScene(levelToLoad);
		}
	}
}
