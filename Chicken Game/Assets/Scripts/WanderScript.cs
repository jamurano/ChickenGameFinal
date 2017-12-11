using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private Coroutine wanderRoutine;

    public void Update()
    {

        if (isWandering == false)
        {
            wanderRoutine = StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }

        if (isWalking == true)
        {   
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        wanderRoutine = StartCoroutine(Wander());
    }

    private void OnDisable()
    {
        Debug.Log(gameObject.name + " disabled");
        if (wanderRoutine != null)
        {
            StopCoroutine(wanderRoutine);
        }
    }

    IEnumerator Wander()
    {

        int rotTime = Random.Range(1, 3);
        int rotWait = Random.Range(1, 3);
        int rotLOrR = Random.Range(0, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotWait);
        if (rotLOrR == 1)
        {

            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotLOrR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }
}

