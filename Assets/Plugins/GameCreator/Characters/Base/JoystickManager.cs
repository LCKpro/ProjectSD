namespace GameCreator.Characters
{
    using UnityEngine;

    public class JoystickManager : MonoBehaviour
    {
        #region SingleTon

        private static JoystickManager _instance = null;

        public static JoystickManager Instance
        {
            get
            {
                SetInstance();
                return _instance;
            }
        }

        public static void SetInstance()
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("JoystickManager");
                if (go != null)
                {
                    _instance = go.GetComponent<JoystickManager>();
                    if (_instance == null)
                    {
                        Debug.Log("JoystickManager Instance Null");
                    }
                }
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void OnDisable()
        {
            _instance = null;
        }

        #endregion

        public FloatingJoystick joystick;
    }
}