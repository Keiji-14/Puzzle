using UnityEngine;

namespace Audio
{
    /// <summary>
    /// BGMの再生処理
    /// </summary>
    public class BGM : MonoBehaviour
    {
        #region PublicField
        public static BGM instance = null;
        #endregion

        #region SerializeField
        [SerializeField] private AudioSource bgm;
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

        void Start()
        {
            bgm.Play();
        }
        #endregion
    }
}