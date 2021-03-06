﻿/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace BlackFire.ToolKit.Editor
{
    public sealed class SecurityFileWindow : Alan.Editor.EditorWindowBase<SecurityFileWindow>
    {
        
        [MenuItem("Window/BlackFire ToolKit/Security File")]
        private static void Security_File()
        {
            var window = EditorWindow.GetWindow(typeof(BlackFire.ToolKit.Editor.SecurityFileWindow), false, "Security File") as BlackFire.ToolKit.Editor.SecurityFileWindow;
            window.position = new UnityEngine.Rect((1920f-730f)/2,(1080f-650f)/2,400f,150f);
        }
        
        
        private string m_OringinalText="Please enter your oringinal text here!";
        private string m_Path = string.Empty;
        private string m_Password = "123456789012345678901234567980aa";

        protected override void OnDrawWindow()
        {
            GUILayout.Space(5);
            m_OringinalText = EditorGUILayout.TextArea(m_OringinalText,GUILayout.MinHeight(100f));

            EditorGUILayout.BeginHorizontal();
            {
                m_Path = EditorGUILayout.TextArea(m_Path);
                if (GUILayout.Button("Path",GUILayout.MaxWidth(100)))
                {
                    m_Path = EditorUtility.SaveFilePanel("Security File",EditorApplication.applicationPath,"your file",null);
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                m_Password = EditorGUILayout.TextArea(m_Password);
                if (GUILayout.Button("Encrypt",GUILayout.MaxWidth(100)))
                {
                    if (string.IsNullOrEmpty(m_Password))
                    {
                        Debug.LogError("The password can not empty!");
                        return;
                    }else if (m_Password.Length!=32)
                    {
                        Debug.LogError("The password must be 32 length.");
                        return;
                    }
                    
                    var enStr = ToolkitUtility.Security.AES_Encrypt(m_OringinalText,m_Password);
                    File.WriteAllText(m_Path,enStr);
                    Debug.Log("Info: Encrypt done!");
                }
            }
            EditorGUILayout.EndHorizontal();
        }
     }
}
