using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ComboDetail : MonoBehaviour
{
    [SerializeField]
    private Image imgCombo;

    [SerializeField]
    private Text txtHitCount;

    [SerializeField]
    private CanvasGroup canvasGroup;

    /// <summary>
    /// ComboDetailの設定
    /// </summary>
    public void SetUpComboDetail(int comboCount) {
        // ヒット数を表示
        txtHitCount.text = comboCount + "Hit!";

        // サイズを調整
        Vector3 scale = Vector3.one * (1 + comboCount * 0.1f);

        transform.position = new Vector2(transform.position.x + Random.Range(-7.5f, 7.5f), transform.position.y + Random.Range(-7.5f, 7.5f));

        // アニメ演出して、演出後に破棄
        Sequence sequence = DOTween.Sequence();

        sequence.Append(imgCombo.transform.DOShakePosition(0.25f)).SetEase(Ease.Flash);
        sequence.Join(imgCombo.transform.DOScale(scale, 0.25f)).SetEase(Ease.Flash);
        sequence.AppendInterval(0.25f);
        sequence.Append(canvasGroup.DOFade(0f, 1.5f)).OnComplete(() => { Destroy(gameObject); });
    }
}
