using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StageList stageList;

    public int areaIndex;          // 現在のエリア数。敵をすべて倒すとカウントアップ
    public int currentStageNo;     // 現在のステージ数。ボスを倒してクリアすると増える。GameDataに管理させる

    public MoveTest chara;

    public float leftLimitPos;
    public float rightLimitPos;

    public StageList.StageData currentStageData;

    public int generateCount;
    public int destroyCount;
    public bool isCompleteGenerate;

    // 生成した敵を入れるリスト
    public List<GameObject> enemyList = new List<GameObject>();

    // 敵のプレファブ
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public enum GameState {
        Play,
        Wait,
        Move,
    }

    public GameState gameState = GameState.Wait;

    public float generateTimer;

    public int appearTime;


    void Start()
    {
        InitStage();

        SetUpNextArea();
    }

    private void InitStage() {
        // TODO GameDataからステージ番号をセット
        currentStageNo = 0;
        

        // ステージ番号から、どのステージかを検索してStageDataを取得
        currentStageData = stageList.stageDatas.Find(x => x.stageNo == currentStageNo);

        //foreach (StageList.StageData stageData in stageList.stageDatas) {
        //    if (stage.stageNo == currentStageNo) {
        //        currentStageData = stageData;
        //    }
        //}


        // エリアの番号をセット
        areaIndex = 0;

    }


    private void SetUpNextArea() {
        // エリア番号からエリアの情報を取得

        // 移動範囲制限
        leftLimitPos = stageList.stageDatas[currentStageNo].areaDatas[areaIndex].startPos;
        rightLimitPos = stageList.stageDatas[currentStageNo].areaDatas[areaIndex].endPos;

        // 敵の生成数と討伐数を初期化
        generateCount = 0;
        destroyCount = 0;

        // 敵のリストクリア 
        enemyList.Clear();

        // 最初に敵が出現するまでの時間を設定
        appearTime = Random.Range(5, 10);

        // ゲームスタート
        gameState = GameState.Play;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Wait) {
            return;
        }

        if (gameState == GameState.Move) {
            if (chara.transform.position.x >= rightLimitPos) {

                gameState = GameState.Wait;
                areaIndex++;
                SetUpNextArea();
            }
        }

        // エリアごとの敵の生成数と現在の生成数を比べて、すべて生成済の場合には処理をしない
        if (generateCount >= stageList.stageDatas[currentStageNo].areaDatas[areaIndex].appearNum.Length) {
            return;
        }

        generateTimer += Time.deltaTime;

        if (generateTimer >= appearTime) {
            generateTimer = 0;

            float posX = chara.transform.position.x;
            float posZ = chara.transform.position.z;
            Vector3 generatePos = new Vector3(posX + Random.Range(-0.1f, 0.1f), 0.0f, posZ + Random.Range(-0.1f, 0.1f));

            // 敵生成
            GenerateEnemy(generatePos, generateCount);
            
        }
    }

    private void GenerateEnemy(Vector3 generatePos, int enemyIndex) {
        GameObject enemy = Instantiate(enemyPrefabs[stageList.stageDatas[currentStageNo].areaDatas[areaIndex].appearNum[enemyIndex]], generatePos, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetUpEnemy(this);        
        enemyList.Add(enemy);

        generateCount++;

        // エリア内の敵をすべて生成したか確認
        if (generateCount >= stageList.stageDatas[currentStageNo].areaDatas[areaIndex].appearNum.Length) {
            isCompleteGenerate = true;
        } else {
            // 敵が出現するまでの時間を設定
            appearTime = Random.Range(5, 10);
        }
    }


    public void RemoveEmenyList(GameObject enemy) {
        destroyCount++;

        enemyList.Remove(enemy);

        CheckAreaClear();
    }


    private void CheckAreaClear() {
        // 敵の生成がすべて終了していない場合にはクリアにしない
        if (!isCompleteGenerate) {
            return;
        }

        if (destroyCount >= generateCount) {
            gameState = GameState.Move;
        }
    }
}
