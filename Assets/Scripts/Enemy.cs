using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;

    public bool isDebug;

    public void SetUpEnemy(GameManager gameManager) {
        this.gameManager = gameManager;
    }



    void Start()
    {
        if (isDebug) {
            StartCoroutine(DestroyEnemy(3.0f));
        }
    }

    private IEnumerator DestroyEnemy(float waitTime = 0.0f) {
        yield return new WaitForSeconds(waitTime);
        gameManager.RemoveEmenyList(gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
