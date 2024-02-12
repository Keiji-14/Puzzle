using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class SE : MonoBehaviour
    {
        #region PrivateField
        public static SE instance = null;
        #endregion

        #region SerializeField
        [SerializeField] AudioSource audioSource;
        /// <summary>消滅時のエフェクト</summary>
        [SerializeField] List<AudioClip> seClipList;
        #endregion

        #region PublicMethod
        public enum SEName
        {
            ButtonSE,
            DragSE,
            DropSE,
            DestroySE,
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// SEを再生
        /// </summary>
        /// <param name="seName">効果音名</param>
        public void Play(SEName seName)
        {
            audioSource.PlayOneShot(seClipList[(int)seName]);
        }
        #endregion
    }
}