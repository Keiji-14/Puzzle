using UnityEngine;

namespace Puzzle
{
    public class PuzzleBoard : MonoBehaviour
    {
        #region PublicField
        /// <summary>盤面に配置済みかどうか</summary>
        public bool isSetted;
        /// <summary>配置したパズルピース</summary>
        public GameObject setPieceObj;
        #endregion
    }
}