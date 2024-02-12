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
        /// <summary>���Ŏ��̃G�t�F�N�g</summary>
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
        /// SE���Đ�
        /// </summary>
        /// <param name="seName">���ʉ���</param>
        public void Play(SEName seName)
        {
            audioSource.PlayOneShot(seClipList[(int)seName]);
        }
        #endregion
    }
}