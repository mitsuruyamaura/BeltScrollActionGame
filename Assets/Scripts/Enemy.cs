using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDebug;

    private GameManager gameManager;

    /// <summary>
    /// Enemyクラスの初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemy(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    void Start()
    {
        if (isDebug) {
            StartCoroutine(DestroyEnemy(3.0f));
        }
    }

    /// <summary>
    /// 敵を破壊
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator DestroyEnemy(float waitTime = 0.0f) {
        yield return new WaitForSeconds(waitTime);

        // 討伐数を加算し、敵管理用のリストから倒した敵を削除するメソッドの呼び出し
        gameManager.RemoveEmenyList(this);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
