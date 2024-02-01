using UnityEngine;
using UnityEngine.EventSystems;

namespace Scene
{
    public class SceneBase : MonoBehaviour
    {
        #region UnityEvent
        public void Start()
        {
            // EventSystem�̗L���Ő������鏈��
            if (EventSystem.current == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }
        #endregion
    }
}