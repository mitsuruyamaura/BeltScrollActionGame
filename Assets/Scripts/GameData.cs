using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [System.Serializable]
    public class CharaData {
        public int no;
        public int hp;
        public float moveSpeed;
        public float jumpPower;
        public int attackPower;
    }


    public List<CharaData> charaDataList = new List<CharaData>();

    public CharaData currentPlayerData;

    public int currentStageNo;

    public int battleTime;



    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
