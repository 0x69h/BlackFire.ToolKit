/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System;
using System.IO;
using UnityEngine;

namespace BlackFire.ToolKit
{
    public class SecurityFileForMac:MonoBehaviour
    {
        [SerializeField] private string m_Key = "123456789012345678901234567980aa";

        public event Action<bool> CheckMacEvent;
        
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value;}
        }
        
        private void Start()
        {
            var text = File.ReadAllText(Application.streamingAssetsPath+"/auth");
            text = SecurityFile.Decrypt(text,m_Key);
            var macs = text.Split(',');
            var result = ToolkitUtility.Device.HasMac(macs);

#if DEVELOP_ALAN_LOG
                Debug.Log("是否存在Mac:"+result);
#endif
            if (null!=CheckMacEvent)
            {
                CheckMacEvent.Invoke(result);
            }
        }
        
    }
    
    
    
 
    
}