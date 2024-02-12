using UnityEngine;
using TMPro;

namespace Puzzle
{
    public class Score : MonoBehaviour
    {
        #region SerializeField 
        /// <summary>スコアのテキストUI</summary>
        [SerializeField] private TextMeshProUGUI scoreText;
        /// <summary>ハイスコアのテキストUI</summary>
        [SerializeField] private TextMeshProUGUI highScoreText;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // スコア状態を初期化する
            scoreText.text = "0";

            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }

        /// <summary>
        /// スコアを表示する
        /// </summary>
        /// <param name="scoreNum">スコアの値</param>
        public void UpdataScore(int scoreNum)
        {
            scoreText.text = scoreNum.ToString();
        }
        #endregion
    }
}