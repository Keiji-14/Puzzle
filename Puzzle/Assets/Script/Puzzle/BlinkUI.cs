using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlinkUI : MonoBehaviour
{
    #region SerializeField
    /// <summary>点滅の間隔</summary>
    [SerializeField] private float blinkInterval;
    #endregion

    /// <summary>
    /// UIを点滅表示する
    /// </summary>
    /// 
    public void ShowUI()
    {
        // 点滅アニメーションを設定
        Sequence blinkSequence = DOTween.Sequence();
        blinkSequence.Append(DOTween.To(() => GetComponent<Image>().color.a, alpha => SetAlpha(alpha), 0, blinkInterval / 2));
        blinkSequence.Append(DOTween.To(() => GetComponent<Image>().color.a, alpha => SetAlpha(alpha), 1, blinkInterval / 2));
        
        // 無限ループ
        blinkSequence.SetLoops(-1);

        // アニメーションを再生
        blinkSequence.Play();
    }

    /// <summary>
    /// 透明度を設定する処理
    /// </summary>
    /// <param name="puzzlePiece">アルファ値</param>
    private void SetAlpha(float alpha)
    {
        var currentColor = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}
