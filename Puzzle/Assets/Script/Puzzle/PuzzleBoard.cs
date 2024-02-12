using System.Collections;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleBoard : MonoBehaviour
    {
        #region PublicField
        /// <summary>配置したパズルピース</summary>
        public GameObject setPieceObj;
        #endregion

        #region SerializeField
        /// <summary>消滅時のエフェクト</summary>
        [SerializeField] ParticleSystem destroyParticle;
        #endregion

        #region PublicMethod
        /// <summary>
        /// ラインが消えた時にエフェクトを発生させる
        /// </summary>
        public void DestroyParticle()
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);

            Destroy(setPieceObj);

            setPieceObj = null;
        }
        #endregion
    }
}