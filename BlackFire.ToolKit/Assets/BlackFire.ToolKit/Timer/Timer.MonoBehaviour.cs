/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System;
using UnityEngine;

namespace BlackFire.ToolKit
{
    /// <summary>
    /// Sparrow Timer.
    /// </summary>
    public partial class Timer:MonoBehaviour
    {
        public static event Action OnAct;

        private static Timer m_Instance;
        
        private void Awake()
        {
            if (null==m_Instance)
            {
                m_Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }


        private void OnDestroy()
        {
            if (this.Equals(m_Instance))
            {
                m_Instance = null;
            }
        }

        private void Update()
        {
            if (null!=OnAct)
            {
                OnAct.Invoke();
            }
        }
    }
}







