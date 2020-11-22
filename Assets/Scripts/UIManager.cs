using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform comboObjTran;

    [SerializeField]
    private ComboDetail comboDetailPrefab;

    List<ComboDetail> comboDetailList = new List<ComboDetail>();

    /// <summary>
    /// コンボ数表示を生成
    /// </summary>
    public void CreateComboDetail(int comboCount) {

        // コンボ数表示を生成
        ComboDetail comboDetail = Instantiate(comboDetailPrefab, comboObjTran, false);

        //　コンボ数表示を設定
        comboDetail.SetUpComboDetail(comboCount);
    }
}
