using System.Collections.Generic;
using UnityEngine;

namespace Audio
{    /// <summary>
     /// ���ʉ��̍Đ�����
     /// </summary>
    public class SE : MonoBehaviour
    {
        #region PublicField
        public static SE instance = null;
        #endregion

        #region SerializeField
        [SerializeField] AudioSource audioSource;
        /// <summary>���ʉ����X�g</summary>
        [SerializeField] List<AudioClip> seClipList;
        #endregion

        #region UnityEvent
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
        #endregion

        #region PublicMethod
        public enum SEName
        {
            ButtonSE,
            DragSE,
            SetSE,
            DestroySE,
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