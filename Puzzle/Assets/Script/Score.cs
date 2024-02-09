using UnityEngine;
using TMPro;

namespace Puzzle
{
    public class Score : MonoBehaviour
    {
        #region SerializeField 
        /// <summary>スコアのUI</summary>
        [SerializeField] private TextMeshProUGUI scoreText;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // スコア状態を初期化する
            scoreText.text = "0";
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