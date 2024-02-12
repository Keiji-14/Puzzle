using System.Collections.Generic;
using UnityEngine;

namespace Audio
{    /// <summary>
     /// Œø‰Ê‰¹‚ÌÄ¶ˆ—
     /// </summary>
    public class SE : MonoBehaviour
    {
        #region PublicField
        public static SE instance = null;
        #endregion

        #region SerializeField
        [SerializeField] AudioSource audioSource;
        /// <summary>Œø‰Ê‰¹ƒŠƒXƒg</summary>
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
        /// SE‚ğÄ¶
        /// </summary>
        /// <param name="seName">Œø‰Ê‰¹–¼</param>
        public void Play(SEName seName)
        {
            audioSource.PlayOneShot(seClipList[(int)seName]);
        }
        #endregion
    }
}