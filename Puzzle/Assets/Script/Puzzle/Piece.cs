using UnityEngine;

namespace Puzzle
{
    public class Piece : MonoBehaviour
    {
        #region PublicField
        /// <summary>マスの番号</summary>
        public int squareID;
        /// <summary>マスの配置状態</summary>
        public bool isSetted;
        #endregion
    }
}