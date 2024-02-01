using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        public RectTransform boardPanel; // 10x10のUI盤面のRectTransform
        public RectTransform squareUIPrefab; // 配置する四角いUIのプレハブ

        #region SerializeField
        //[SerializeField] private 
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // UIを盤面に配置する処理を呼び出す
            PlaceUIOnBoard();
        }

        void PlaceUIOnBoard()
        {
            // 10x10の盤面のサイズを取得
            float boardWidth = boardPanel.rect.width;
            float boardHeight = boardPanel.rect.height;

            // 10x10の盤面の各セルのサイズを計算
            float cellWidth = boardWidth / 10f;
            float cellHeight = boardHeight / 10f;

            // 10x10の盤面にUIを配置するループ
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    // 新しいUIを生成
                    RectTransform newUI = Instantiate(squareUIPrefab);

                    // UIの親を盤面に設定
                    newUI.SetParent(boardPanel);

                    // UIのサイズをセルのサイズに設定
                    newUI.sizeDelta = new Vector2(cellWidth, cellHeight);

                    // UIの位置を計算して設定
                    float xPosition = col * cellWidth + cellWidth / 2f;
                    float yPosition = row * cellHeight + cellHeight / 2f;
                    newUI.localPosition = new Vector3(xPosition, yPosition, 0f);
                }
            }
        }
        #endregion
    }
}