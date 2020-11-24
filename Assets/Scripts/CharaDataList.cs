using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CharaDataList", menuName = "Create CharaDataList")]
public class CharaDataList : ScriptableObject
{
    public List<CharaData> charaDatas = new List<CharaData>();

    [Serializable]
    public class CharaData {
        public int no;
        public int hp;
        public float moveSpeed;
        public float jumpPower;
        public int attackPower;
        public CharaType charaType;
    }
}
