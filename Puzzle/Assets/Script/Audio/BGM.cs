using UnityEngine;

namespace Audio
{
    /// <summary>
    /// BGMの再生処理
    /// </summary>
    public class BGM : MonoBehaviour
    {
        public static BGM instance = null;

        [SerializeField] AudioSource bgm;

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
    }
}