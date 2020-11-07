using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StageList stageList;    // スクリプタブル・オブジェクトを登録

    public int areaIndex;          // 現在のエリア数。敵をすべて倒すとカウントアップ
    public int currentStageNo;     // 現在のステージ数。ボスを倒してクリアすると増える。GameDataに管理させる

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


    // 未
    public bool isDebugAreaMoving;

    public AreaCameraManager areaCameraManager;


    void Start()
    {
        // 現在のステージ番号を取得
        if (GameData.instance != null) {
            currentStageNo = GameData.instance.currentStageNo;
        } else {
            currentStageNo = 0;  // 初期化
        }

        InitStage();

        SetUpNextArea();
    }

    /// <summary>
    /// エリアの番号を切り替える
    /// </summary>
    /// <param name="amountNo"></param>
    public void OnClickChangeAreaNo(int amountNo) {
        // エリアの番号をセット
        areaIndex += amountNo;

        // エリア番号からエリアの情報を取得
        SetUpNextArea();
    }

    /// <summary>
    /// ステージの番号を取得してステージの準備を行う
    /// </summary>
    private void InitStage() {
        // 初期値
        gameState = GameState.Wait;
        Debug.Log(gameState);

        // TODO GameDataからステージ番号をセット
        //currentStageNo = 0;
        

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

    /// <summary>
    /// エリア番号からエリアの情報を取得
    /// </summary>
    private void SetUpNextArea() {
        // カメラの優先順位　切り替え
        areaCameraManager.ChengeVirtualCamera(areaIndex);

        // 移動範囲制限
        leftLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.left;
        rightLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.horizontalLimit.right;
        forwordLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.forword;
        backLimitPos = currentStageData.areaDatas[areaIndex].areaMoveLimit.depthLimit.back;

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
                    NextStage();
                } else {
                    // クリアしていなければ、次のエリアをスタート                 
                    SetUpNextArea();
                }
            }
        }

        // エリアごとの敵の生成数と現在の生成数を比べて、すべて生成済の場合には処理をしない
        if (generateCount >= currentStageData.areaDatas[areaIndex].appearNum.Length) {
            return;
        }

        generateTimer += Time.deltaTime;

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
        }
    }

    /// <summary>
    /// ステージのクリア判定
    /// </summary>
    private bool CheckStageClear() {

        // エリアの番号がステージの最後のエリアの番号かどうか確認する
        if (areaIndex >= currentStageData.areaDatas.Count) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 次のステージへ移行
    /// </summary>
    private IEnumerator NextStage() {
        // ステージ番号を次に送る
        currentStageNo++;

        // すべてのステージをクリアしたか確認する
        if (currentStageNo >= stageList.stageDatas.Count) {
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

    private IEnumerator SceneStory() {
        // TODO ストーリー展開

        yield return null;
    }
}
