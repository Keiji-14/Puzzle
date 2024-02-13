using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    /// <summary>
    /// パズルピース
    /// </summary>
    public class Piece : MonoBehaviour
    {
        #region PublicField
        /// <summary>マスの番号</summary>
        public int squareID;
        /// <summary>マスの配置状態</summary>
        public bool isSetted;
        #endregion

        #region PrivateField
        /// <summary>通常時のスプライト</summary>
        private Sprite defaultSprite;
        /// <summary>自身の画像</summary>
        private Image image;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            image = GetComponent<Image>();

            defaultSprite = image.sprite;
        }

        /// <summary>
        /// 通常のスプライトを設定する
        /// </summary>
        public void SetDefaultSprite()
        {
            image.sprite = defaultSprite;
        }

        /// <summary>
        /// 配置場所が無い時のスプライトを設定する
        /// </summary>
        public void SetUnSetSprite(Sprite unSetSprite)
        {
            image.sprite = unSetSprite;
        }
        #endregion
    }
}