using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] Transform puzzleParent;
        [Header("List")]
        /// <summary>パズルの生成座標</summary>
        [SerializeField] List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>パズル</summary>
        [SerializeField] Puzzle puzzle;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            CreatePuzzle();
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// パズルを生成する処理
        /// </summary>
        private void CreatePuzzle()
        {
            // パズルを生成する処理
            foreach (var createPos in createPosList)
            {
                var puzzleObj = Instantiate(puzzle, puzzleParent).GetComponent<Puzzle>();

                puzzleObj.Init();

                // 指定した座標に移動
                puzzleObj.transform.localPosition = createPos;
            }
        }
        #endregion
    }
}