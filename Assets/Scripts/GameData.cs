using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int currentStageNo;

    public int battleTime;


    //[System.Serializable]
    //public class CharaData {
    //    public int no;
    //    public int hp;
    //    public float moveSpeed;
    //    public float jumpPower;
    //    public int attackPower;
    //}


    //public List<CharaData> charaDataList = new List<CharaData>();

    [Header("使用するキャラの番号")]
    public int currentCharaNo;

    [Header("キャラデータのデータベース")]
    public CharaDataList charaDataList;

    [Header("現在使用中のキャラのキャラデータ")]
    public CharaDataList.CharaData playableCharaData;

    [Header("使用可能なキャラ数")]
    public int usePlayableCharaCount;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 使用キャラの情報(CharaData)を設定
    /// </summary>
    /// <param name="charaNo"></param>
    public void SetUpPlayableCharaData(int charaNo) {
        playableCharaData = GetCharaData(charaNo);
    }

    /// <summary>
    /// CharaDetaの取得
    /// </summary>
    /// <param name="charaNo"></param>
    /// <returns></returns>
    public CharaDataList.CharaData GetCharaData(int charaNo) {
        return charaDataList.charaDatas.Find(x => x.no == charaNo);
    }

    /// <summary>
    /// 使用するキャラの番号を設定(未使用)
    /// </summary>
    /// <param name="charaNo"></param>
    public void SetUpCurrentCharaNo(int charaNo) {
        currentCharaNo = charaNo;
    }
}
