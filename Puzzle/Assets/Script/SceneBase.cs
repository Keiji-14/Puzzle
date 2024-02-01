using UnityEngine;
using UnityEngine.EventSystems;

namespace Scene
{
    public class SceneBase : MonoBehaviour
    {
        #region UnityEvent
        public void Start()
        {
            // EventSystem‚Ì—L–³‚Å¶¬‚·‚éˆ—
            if (EventSystem.current == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }
        #endregion
    }
}