﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StageList stageList;    // スクリプタブル・オブジェクトを登録

    public int areaIndex;          // 現在のエリア数。敵をすべて倒すとカウントアップ
    //public int currentStageNo;     // 現在のステージ数。ボスを倒してクリアすると増える。GameDataに管理させる

    public PlayerController playerController;

    public float leftLimitPos;
    public float rightLimitPos;
    public float forwordLimitPos;
    public float backLimitPos;

    public StageList.StageData currentStageData;

    public int generateCount;

    public float generateTimer;

    public int appearTime;

    // 生成した敵を入れるリスト
    public List<Enemy> enemyList = new List<Enemy>();

    public int destroyCount;

    public bool isCompleteGenerate;

    // 敵のプレファブ
    public List<Enemy> enemyPrefabs = new List<Enemy>();

    public enum GameState {
        Play,
        Wait,
        Move,
    }

    public GameState gameState = GameState.Wait;


    //* Rayによる移動制限判定用  *//

    public GameObject limitObjPrefab;      // 移動制限用オブジェクト

    public GameObject rayObj;              // BoxCast用のRayを発射するオブジェクト

    float scale;                           // BoxCastの判定に使うコライダーのサイズ設定用

    bool isHit;                            // Rayの判定結果の戻り値代入用

    RaycastHit hit;                        // BoxCastのoutキーワードへの取得値代入用

    List<GameObject> limitObjList = new List<GameObject>();   // 移動制限用オブジェクトを代入するリスト


    // 未
    public bool isDebugAreaMoving;

    public AreaCameraManager areaCameraManager;


    void Start() {
        // ステージの番号を取得してステージの準備を行う
        InitStage();

        // エリア番号からエリアの情報を取得してエリアの準備を行う
        SetUpNextArea();
    }

    /// <summary>
    /// ステージの番号を取得してステージの準備を行う
    /// </summary>
    private void InitStage() {
        // 初期値
        gameState = GameState.Wait;
        Debug.Log(gameState);

        //areaIndex = 0;

        // TODO GameDataからステージ番号をセット
        //currentStageNo = 0;


        // ステージ番号から、どのステージかを検索してStageDataを取得
        currentStageData = stageList.stageDatas.Find(x => x.stageNo == GameData.instance.currentStageNo);

        //foreach (StageList.StageData stageData in stageList.stageDatas) {
        //    if (stage.stageNo == GameData.instance.currentStageNo) {
        //        currentStageData = stageData;
        //    }
        //}

        // エリアの番号をセット
        areaIndex = 0;

    }

    /// <summary>
    /// GizmosでRayの結果を表示するコールバック。この中でGizmosの処理を記述しないとエラーになる
    /// </summary>
    private void OnDrawGizmos() {
        // Rayがヒットしなかった場合
        if (!isHit) {
            Gizmos.DrawRay(rayObj.transform.position, -transform.up * 100);
        } else {　　//　Rayがヒットした場合
            Gizmos.DrawRay(rayObj.transform.position, -transform.up * hit.distance);
            Gizmos.DrawWireCube(rayObj.transform.position - transform.up * hit.distance, Vector3.one * scale * 2);
        }
    }

    /// <summary>
    /// 移動制限範囲用のオブジェクトの生成
    /// </summary>
    private void CreateLimitObjs() {
        // BoxCastの判定に使うコライダーのサイズを移動制限用のゲームオブジェクトを元に設定
        // ここでは判定を行うサイズの半分の大きさにする(直径でBoxCastを発射するため)
        scale = limitObjPrefab.transform.localScale.x * 0.5f;

        // 生成個数の指定
        int range = (int)(forwordLimitPos - backLimitPos) / 2;
        Debug.Log(range);

        // 画面左側に、移動できる範囲を制限するオブジェクトを生成
        for (int i = 0; i < range; i++) {
            // 左側

            // Rayの発射位置を設定
            rayObj.transform.position = new Vector3(leftLimitPos, rayObj.transform.position.y, backLimitPos + i * 2.5f);

            // Rayを発射。Rayの接触したコライダーを持つゲームオブジェクトとの接触を判定
            isHit = Physics.BoxCast(rayObj.transform.position, Vector3.one * scale, -transform.up, out hit);

            Debug.Log(isHit);
            Debug.Log(hit.collider.gameObject.layer);

            // Rayが接触したコライダーのゲームオブジェクトのLayeyが Obstacle というLayerではなかったら
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacle")) {
                // 移動制限用のオブジェクトを生成してリストへ追加
                limitObjList.Add(Instantiate(limitObjPrefab, new Vector3(leftLimitPos, 8.0f, backLimitPos + i * 2.5f), limitObjPrefab.transform.rotation));
            }

            // 右側
            // Rayの発射位置を設定
            rayObj.transform.position = new Vector3(rightLimitPos, rayObj.transform.position.y, backLimitPos + i * 2.5f);

            // Rayを発射。Rayの接触したコライダーを持つゲームオブジェクトとの接触を判定
            isHit = Physics.BoxCast(rayObj.transform.position, Vector3.one * scale, -transform.up, out hit);

            Debug.Log(isHit);
            Debug.Log(hit.collider.gameObject.layer);

            // Rayが接触したコライダーのゲームオブジェクトのLayeyが Obstacle というLayerではなかったら
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacle")) {
                // 移動制限用のオブジェクトを生成してリストへ追加
                limitObjList.Add(Instantiate(limitObjPrefab, new Vector3(rightLimitPos, 8.0f, backLimitPos + i * 2.5f), limitObjPrefab.transform.rotation));
            }
        }
    }

    /// <summary>
    /// 移動制限範囲用のオブジェクトの削除
    /// </summary>
    private void DeleteLimitObjs() {
        for (int i = 0; i < limitObjList.Count; i++) {
            Destroy(limitObjList[i]);
        }
        limitObjList.Clear();
    }

    /// <summary>
    /// エリア番号からエリアの情報を取得してエリアの準備を行う
    /// </summary>
    private void SetUpNextArea() {
        // カメラの優先順位　切り替え
        areaCameraManager.ChengeVirtualCamera(areaIndex);

        // 移動範囲制限
        leftLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.left;
        rightLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.right;
        forwordLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.forword;
        backLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.back;


        // 移動範囲制限用のオブジェクトを画面の左右に設置
        CreateLimitObjs();


        if (isDebugAreaMoving) {
            generateCount = currentStageData.areaDatas[areaIndex].appearNum.Length;

            isCompleteGenerate = true;
        } else {
            // 敵の生成数と討伐数を初期化
            generateCount = 0;
            destroyCount = 0;

            // エリアの敵をすべて生成しているか確認用の変数を初期化
            isCompleteGenerate = false;
        }     

        // 敵のリストクリア 
        enemyList.Clear();

        // 最初に敵が出現するまでの時間を設定
        appearTime = Random.Range(5, 10);

        // ゲームスタート
        gameState = GameState.Play;
        Debug.Log(gameState);
    }

    void Update()
    {
        if (isDebugAreaMoving) {
            if (Input.GetKeyDown(KeyCode.O)) {
                OnClickChangeAreaNo(-1);
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                OnClickChangeAreaNo(1);
            }
        }

        if (gameState == GameState.Wait) {
            return;
        }

        if (gameState == GameState.Move) {
            if (playerController.transform.position.x >= rightLimitPos) {

                gameState = GameState.Wait;
                Debug.Log(gameState);

                areaIndex++;

                // ステージをクリアしたか確認
                if (CheckStageClear() == true) {  // true かどうか判定
                    // クリアしていれば、次のステージをスタート
                    StartCoroutine(NextStage());
                } else {
                    // クリアしていなければ、次のエリアをスタート                 
                    SetUpNextArea();
                }
            }
        }

        // 用意してあるエリアの数を超えたら処理しない
        if (areaIndex >= currentStageData.areaDatas.Count) {
            return;
        }

        // エリアごとの敵の生成数と現在の生成数を比べて、すべて生成済の場合には処理をしない
        if (generateCount >= currentStageData.areaDatas[areaIndex].appearNum.Length) { 
            return;
        }

        // 敵の生成まで時間をカウントアップ
        generateTimer += Time.deltaTime;

        // 生成までの時間が待機時間を超えたら
        if (generateTimer >= appearTime) {
            generateTimer = 0;

            // 敵生成
            GenerateEnemy(playerController.transform.position, generateCount);         
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="charaPos"></param>
    /// <param name="enemyIndex"></param>
    private void GenerateEnemy(Vector3 charaPos, int enemyIndex) {
        // 左右のどちらに生成するかランダムで決める

        int direction = Random.Range(0, 2);
        charaPos.x = direction == 0 ? charaPos.x += 2.5f : charaPos.x -= 2.5f;

        // ランダムな位置を設定

        float posX = charaPos.x;
        float posZ = charaPos.z;

        Vector3 generatePos = new Vector3(posX + Random.Range(-0.5f, 0.5f), charaPos.y, posZ + Random.Range(-0.5f, 0.5f));

        Enemy enemy = Instantiate(enemyPrefabs[currentStageData.areaDatas[areaIndex].appearNum[enemyIndex]], generatePos, Quaternion.identity);
        
        enemy.SetUpEnemy(this);

        // 生成された敵の情報を Enemy クラス単位でListに追加する
        enemyList.Add(enemy);

        generateCount++;

        // エリア内の敵をすべて生成したか確認
        if (generateCount >= currentStageData.areaDatas[areaIndex].appearNum.Length) {
            isCompleteGenerate = true;
        } else {
            // 敵が出現するまでの時間を設定
            appearTime = Random.Range(5, 10);
        }
    }

    /// <summary>
    /// 討伐数を加算してenemyListから倒した敵を削除
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEmenyList(Enemy enemy) {
        destroyCount++;

        enemyList.Remove(enemy);

        // エリア内の敵の生成状況を確認
        CheckAreaClear();
    }

    /// <summary>
    /// エリア内の敵の生成状況を確認
    /// </summary>
    private void CheckAreaClear() {
        // 敵の生成がすべて終了していない場合にはクリアにしない
        if (!isCompleteGenerate) {
            return;
        }

        if (destroyCount >= generateCount) {
            Debug.Log("エリア　クリア");
            gameState = GameState.Move;
            Debug.Log(gameState);

            // 移動範囲制限用のオブジェクトを削除
            DeleteLimitObjs();
        }
    }

    /// <summary>
    /// ステージのクリア判定
    /// </summary>
    private bool CheckStageClear() {

        // エリアの番号がステージの最後のエリアの番号かどうか確認する
        if (areaIndex >= currentStageData.areaDatas.Count) {
            Debug.Log("ステージ　クリア");
            return true;
        }
        Debug.Log("ステージ　未クリア。次のエリアへ");
        return false;
    }

    /// <summary>
    /// 次のステージへ移行
    /// </summary>
    private IEnumerator NextStage() {
        // ステージ番号を次に送る
        GameData.instance.currentStageNo++;

        // すべてのステージをクリアしたか確認する
        if (GameData.instance.currentStageNo >= stageList.stageDatas.Count) {
            // ゲームクリア
            Debug.Log("ゲームクリア");
        } else {
            // すべてのステージをクリアしていなければ
            Debug.Log("次のステージへ");        

            // TODO ステージクリアの演出
            yield return StartCoroutine(SceneStory());

            // 次のステージのデータを取得
            InitStage();

            // 次のステージの最初のエリアを用意
            SetUpNextArea();
        }
    }

    /// <summary>
    /// イベント用ストーリーを進める、あるいはイベント用のシーンへ遷移する
    /// </summary>
    /// <returns></returns>
    private IEnumerator SceneStory() {
        // TODO このシーン内でストーリー展開

        // TODO あるいはイベント用のシーンへ遷移する

        yield return null;
    }


    // 未

    /// <summary>
    /// エリアの番号を切り替える(Debug用)
    /// </summary>
    /// <param name="amountNo"></param>
    public void OnClickChangeAreaNo(int amountNo) {
        // エリアの番号をセット
        areaIndex += amountNo;

        // エリア番号からエリアの情報を取得してエリアの準備を行う
        SetUpNextArea();
    }
}
