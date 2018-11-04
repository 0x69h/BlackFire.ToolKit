/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BlackFire.ToolKit
{

    public sealed partial class Debugger : MonoBehaviour
    {
        #region Public

        public float WindowScale { get { return m_WindowScale; } set { m_WindowScale = value; } }

        public DebuggerStyle DebuggerStyle { get { return m_DebuggerStyle; } set { m_DebuggerStyle = value; } }

        public void RegisterModuleGUI(IDebuggerModuleGUI moduleGUIImpl)
        {
            if (null != moduleGUIImpl && !m_DrawModuleCallbackDic.ContainsKey(moduleGUIImpl.ModuleName))
            {
                m_DrawModuleCallbackDic.Add(moduleGUIImpl.ModuleName, moduleGUIImpl.OnModuleGUI);
                if (string.IsNullOrEmpty(m_SelectedModuleName))
                {
                    m_SelectedModuleName = moduleGUIImpl.ModuleName;
                }
            }
        }

        #endregion


        [SerializeField] private DebuggerStyle m_DebuggerStyle = DebuggerStyle.Hidden;
        [SerializeField][Range(1f,3f)] private float m_WindowScale=1f;

        private string m_SelectedModuleName = string.Empty;

        private Dictionary<string, Action> m_DrawModuleCallbackDic = new Dictionary<string, Action>();

        private List<IDebuggerModuleGUI> m_DebuggerModuleGUIList = new List<IDebuggerModuleGUI>();

        private string m_MiniDebuggerHexColor = "white";
        private bool HasErrorOrException = false;

        private Func<Debugger, bool> m_ChangeToHiddenStyleCallback = null;
        private Func<Debugger, bool> m_ChangeToMiniStyleCallback = null;
        private Func<Debugger, bool> m_ChangeToFullStyleCallback = null;


        #region LifeCircle

        private void Start()
        {
            CheckErrorOrException();
            InitDebuggerModuleGUI();
            InitDebuggerStyleChange();
            InitKvs();
        }

        private void Update()
        {
            if (null != m_ChangeToHiddenStyleCallback && m_ChangeToHiddenStyleCallback.Invoke(this))
            {
                m_DebuggerStyle = DebuggerStyle.Hidden;
            }
            if (null != m_ChangeToMiniStyleCallback && m_ChangeToMiniStyleCallback.Invoke(this))
            {
                m_DebuggerStyle = DebuggerStyle.Mini;
            }
            if (null != m_ChangeToFullStyleCallback && m_ChangeToFullStyleCallback.Invoke(this))
            {
                m_DebuggerStyle = DebuggerStyle.Full;
            }
        }

        private void OnDestroy()
        {
            DestroyDebuggerModuleGUI();
            DestroyKVS();
        }

        private void OnGUI()
        {
            switch (m_DebuggerStyle)
            {
                case DebuggerStyle.Hidden:
                    //Todo...
                    break;
                case DebuggerStyle.Mini:
                    DrawMiniDebugger("<b>DEBUGGER</b>");
                    break;
                case DebuggerStyle.Full:
                    DrawFullDebugger("<b>BLACKFIRE FRAMEWORK DEBUGGER</b>", 640f * m_WindowScale, 360f * m_WindowScale);
                    break;
                default:                   
                    //Todo...
                    break;
            }
        }

        #endregion



        #region Private

        private void DrawMiniDebugger(string title)
        {
          

            Rect rect = DebuggerGUIUtility.GetWindowRect(1);
            float x, y;
            if (rect != Rect.zero)
            {
                x = rect.x;
                y = rect.y;
            }
            else
            {
                x = 10;
                y = 10;
            }

            GUI.backgroundColor = Color.black;
            DebuggerGUIUtility.Window(0, title , x, y, 100, 50, id => {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                {

                    GUI.backgroundColor = Color.black;
                    if (GUILayout.Button(GetFpsString(), new GUIStyle("Button") { fontSize = 14, fixedHeight = 30 }))
                    {
                        m_DebuggerStyle =  DebuggerStyle.Full;
                    }
                    
                    GUI.backgroundColor = Color.black;

                }
                GUILayout.EndHorizontal();

            });
            GUI.backgroundColor = Color.black;
        }

        private void DrawFullDebugger(string title,float width,float height)
        {
            Rect rect = DebuggerGUIUtility.GetWindowRect(0);
            float x, y;
            if (rect != Rect.zero)
            {
                x = rect.x;
                y = rect.y;
            }
            else
            {
                x = 10;
                y = 10;
            }

            DebuggerGUIUtility.BackgroundColor(Color.black, () =>
            {

                DebuggerGUIUtility.Window(1, title, x, y, width, height, id => {

                    GUILayout.Space(10);

                    DebuggerGUIUtility.HorizontalLayout(() => {

                        //左侧导航栏
                        DebuggerGUIUtility.BoxVerticalLayout(()=> {

                            DebuggerGUIUtility.ScrollView(0, sid => {

                            GUILayout.BeginVertical();
                            {
                                foreach (var k in m_DrawModuleCallbackDic.Keys)
                                {
                                    if (GUILayout.Button(k.HexColor("#33CCFF"), new GUIStyle("Button") { fontSize = 14, fixedHeight = 25 }))
                                    {
                                        m_SelectedModuleName = k;
                                    }
                                }

                            }
                            GUILayout.EndVertical();

                        }, GUILayout.Width(130));

                        },GUILayout.Width(130));

                        //右侧内容栏
                        DebuggerGUIUtility.VerticalLayout(()=> {

                            DebuggerGUIUtility.BoxHorizontalLayout(()=> {

                                GUILayout.Label(m_SelectedModuleName, new GUIStyle("Label") { padding=new RectOffset(0,0,0,0),alignment = TextAnchor.MiddleRight, fontStyle = FontStyle.Bold});
                                GUILayout.Label(GetFpsString(), new GUIStyle("Label") { padding = new RectOffset(0, 0, 0, 0), alignment = TextAnchor.MiddleRight, fontStyle = FontStyle.Bold });

                            });

                            DebuggerGUIUtility.HorizontalLayout(() => {

                                DebuggerGUIUtility.ScrollView(1, sid => {

                                    DebuggerGUIUtility.VerticalLayout(() => {

                                        if (!string.IsNullOrEmpty(m_SelectedModuleName))
                                        {
                                            m_DrawModuleCallbackDic[m_SelectedModuleName].Invoke();
                                        }

                                    });

                                });
                            });

                        });



                    });

                });

            });
        }

        
        private void InitDebuggerModuleGUI()
        {
            Type[] acfp_types = null;
            try
            {
                acfp_types = GetImplTypes("Assembly-CSharp-firstpass", typeof(IDebuggerModuleGUI));
            }
            catch (Exception e)
            {
                
            }

            if(null!=acfp_types)
            for (int i = 0; i < acfp_types.Length; i++)
            {
                IDebuggerModuleGUI ins = (IDebuggerModuleGUI)New(acfp_types[i]);
                m_DebuggerModuleGUIList.Add(ins);
            }
            
            
            Type[] ac_types = null;
            try
            {
                ac_types = GetImplTypes("Assembly-CSharp", typeof(IDebuggerModuleGUI));
            }
            catch (Exception e)
            {
                
            }
            if(null!=ac_types)
            for (int i = 0; i < ac_types.Length; i++)
            {
                IDebuggerModuleGUI ins = (IDebuggerModuleGUI)New(ac_types[i]);
                m_DebuggerModuleGUIList.Add(ins);
            }

            m_DebuggerModuleGUIList.Sort((x,y)=>x.Priority-y.Priority);

            for (int i = 0; i < m_DebuggerModuleGUIList.Count; i++)
            {
                if (null != m_DebuggerModuleGUIList[i])
                {
                    m_DebuggerModuleGUIList[i].OnInit(this);
                    RegisterModuleGUI(m_DebuggerModuleGUIList[i]);
                }
            }


            
        }

        private void InitKvs()
        {
            //获取上一次最后一次的左侧栏。
            m_SelectedModuleName = PlayerPrefs.GetString("DebuggerManager/SelectedModuleName");
            if (null == m_DebuggerModuleGUIList.Find(v => v.ModuleName == m_SelectedModuleName))
            {
                m_SelectedModuleName = m_DebuggerModuleGUIList[0].ModuleName;
            }
        }

        private void InitDebuggerStyleChange()
        {
            Type[] acfp_types = null;
            try
            {
                acfp_types = GetImplTypes("Assembly-CSharp-firstpass", typeof(IDebuggerStyleChangeCallback));
            }
            catch (Exception e)
            {
                
            }            
            List<IDebuggerStyleChangeCallback> list = new List<IDebuggerStyleChangeCallback>();
            if(null!=acfp_types)
            for (int i = 0; i < acfp_types.Length; i++)
            {
                IDebuggerStyleChangeCallback ins = (IDebuggerStyleChangeCallback)New(acfp_types[i]);
                list.Add(ins);
            }
            
            
            Type[] ac_types = null;
            try
            {
                ac_types = GetImplTypes("Assembly-CSharp", typeof(IDebuggerStyleChangeCallback));
            }
            catch (Exception e)
            {
                
            } 
            if(null!=ac_types)
            for (int i = 0; i < ac_types.Length; i++)
            {
                IDebuggerStyleChangeCallback ins = (IDebuggerStyleChangeCallback)New(ac_types[i]);
                list.Add(ins);
            }
            

            list.Sort((x, y) => x.Priority - y.Priority);

            if (0<list.Count)
            {
                m_ChangeToHiddenStyleCallback = new Func<Debugger, bool>(list[0].HiddenStylePredicate);
                m_ChangeToMiniStyleCallback = new Func<Debugger, bool>(list[0].MiniStylePredicate);
                m_ChangeToFullStyleCallback = new Func<Debugger, bool>(list[0].FullStylePredicate);
            }
        }

        private static Type[] GetImplTypes(string assemblyName,Type typeBase)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            if (null != assembly)
            {
                List<Type> list = new List<Type>();
                var types = assembly.GetTypes();
                if (null != types)
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].IsClass && !types[i].IsAbstract && typeBase.IsAssignableFrom(types[i]))
                        {
                            list.Add(types[i]);
                        }
                    }
                return list.ToArray();
            }
            return null;
        }
    
        private static object New(Type type, params object[] args)
        {
            return Activator.CreateInstance(type,args);
        }
        
        private void CheckErrorOrException()
        {
            Application.logMessageReceived += (msg, st, tp) =>
            {
                if (!HasErrorOrException)
                {
                    if (tp == LogType.Error || tp == LogType.Exception)
                    {
                        HasErrorOrException = true;
                    }
                }
            };

        }

        private void DestroyDebuggerModuleGUI()
        {
            for (int i = 0; i < m_DebuggerModuleGUIList.Count; i++)
            {
                m_DebuggerModuleGUIList[i].OnDestroy();
            }
        }

        private void DestroyKVS()
        {
            //保存最后一次的左侧栏。
            PlayerPrefs.SetString("DebuggerManager/SelectedModuleName",m_SelectedModuleName);
        }

        private void SetMiniDebuggerColor(string hexColor)
        {
            m_MiniDebuggerHexColor = hexColor;
        }
     
	    private string GetFpsString()
        {
            var fps = Fps();
            if (HasErrorOrException)
            {
                SetMiniDebuggerColor("#CC0000");
            }
            else if (60f <= fps)
            {
                SetMiniDebuggerColor("#009900");
            }
            else if (60f > fps)
            {
                SetMiniDebuggerColor("#00CCFF");
            }
            else if (50f > fps)
            {
                SetMiniDebuggerColor("#FFFFCC");
            }
            else if (40f > fps)
            {
                SetMiniDebuggerColor("#FFFF99");
            }
            else if (30f > fps)
            {
                SetMiniDebuggerColor("#FFFF66");
            }
            else if (20f > fps)
            {
                SetMiniDebuggerColor("#FFFF33");
            }
            else if (10f > fps)
            {
                SetMiniDebuggerColor("#FFFF00");
            }

            return string.Format("FPS : {0:00.00}", fps).HexColor(m_MiniDebuggerHexColor);
        }

        #endregion


        #region Fps

        private static float s_FpsMeasuringDelta = 1f;
        private static float s_TimePassed = 0.0f;
        private static int s_FrameCount = 0;
        private static float s_FPS = 0.0f;

        public static float Fps()
        {
            s_FrameCount = s_FrameCount + 1;
            s_TimePassed = s_TimePassed + Time.unscaledDeltaTime;

            if (s_TimePassed > s_FpsMeasuringDelta)
            {
                s_FPS = s_FrameCount / s_TimePassed;

                s_TimePassed = 0.0f;
                s_FrameCount = 0;
            }
            return s_FPS;
        }

        #endregion
    
        
    }
    
    public static class DebuggerGUIStringExtension
    {
        public static string HexColor(this string text,string hexColor)
        {
            return string.Format("<color={0}>{1}</color>", hexColor, text);
        }

        public static string HexColor(this int intText, string hexColor)
        {
            return string.Format("<color={0}>{1}</color>", hexColor, intText);
        }

    }
        
    public static class DebuggerGUIUtility 
    {
        private static Dictionary<int,Rect> s_WindowRectDic = new Dictionary<int, Rect>();
        private static Dictionary<int,Vector2> s_ScrollViewVector2Dic = new Dictionary<int, Vector2>();
    
        #region ScrollView
    
        public static void ScrollView(int scrollId, Action<int> drawCallback,params GUILayoutOption[] gUILayoutOptions)
        {
            if (!s_ScrollViewVector2Dic.ContainsKey(scrollId))
            {
                s_ScrollViewVector2Dic.Add(scrollId,Vector2.zero);
            }
    
            s_ScrollViewVector2Dic[scrollId] = GUILayout.BeginScrollView(s_ScrollViewVector2Dic[scrollId],gUILayoutOptions);
            {
                if (null!= drawCallback)
                {
                    drawCallback.Invoke(scrollId);
                }
            }
            GUILayout.EndScrollView();
        }
    
        public static void ScrollView(string scrollIdText, Action<int> drawCallback, params GUILayoutOption[] gUILayoutOptions)
        {
            ScrollView(GetId(scrollIdText),drawCallback, gUILayoutOptions);
        }
    
    
        #endregion
    
        #region Window
    
        public static Rect GetWindowRect(int windowId) { return s_WindowRectDic.ContainsKey(windowId) ? s_WindowRectDic[windowId] : Rect.zero; }
        public static Rect GetWindowRect(string windowIdText) { return GetWindowRect(GetId(windowIdText)); }
    
        public static void Window(int windowId,string title,float x,float y,float width,float height,Action<int> drawWindowCallbak,float dragHeight=15f,Texture texture=null)
        {
            if (!s_WindowRectDic.ContainsKey(windowId))
            {
                s_WindowRectDic.Add(windowId, new Rect(x,y,width,height));
            }
    
            s_WindowRectDic[windowId] = new Rect(s_WindowRectDic[windowId].x, s_WindowRectDic[windowId].y, width,height);
    
            s_WindowRectDic[windowId] = null == texture 
            
            ? GUILayout.Window(windowId, s_WindowRectDic[windowId], id =>
            {
    
                GUI.DragWindow(new Rect(0, 0, s_WindowRectDic[windowId].width, dragHeight));
                if (null != drawWindowCallbak)
                {
                    drawWindowCallbak.Invoke(id);
                }
    
            }, title)
            : GUILayout.Window(windowId, s_WindowRectDic[windowId], id =>
            {
    
                GUI.DragWindow(new Rect(0, 0, s_WindowRectDic[windowId].width, dragHeight));
                if (null != drawWindowCallbak)
                {
                    drawWindowCallbak.Invoke(id);
                }
    
            },texture, title); 
    
    
        }
    
        public static void Window(string windowIdText, string title, float x, float y, float width, float height, Action<int> drawWindowCallbak, float dragHeight = 15f, Texture texture = null)
        {
            Window(GetId(windowIdText), title,x,y,width,height,drawWindowCallbak, dragHeight,texture);
        }
    
    
    
        #endregion
    
        #region Layout
    
        public static void DrawItem(string title,string content)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(title, new GUIStyle("Label") { wordWrap = false, alignment = TextAnchor.MiddleLeft });
                GUILayout.Label(content,new GUIStyle("Label") { wordWrap = false, alignment = TextAnchor.MiddleRight });
            }
            GUILayout.EndHorizontal();
        }
    
        public static void DrawItem(Action head, Action body)
        {
            GUILayout.BeginHorizontal();
            {
                if (null != head) head.Invoke();
                if (null != body) body.Invoke();
            }
            GUILayout.EndHorizontal();
        }
    
        public static void BackgroundColor(Color color,Action callback)
        {
            GUI.backgroundColor = color;
            if (null!= callback)
            {
                callback.Invoke();
            }
            GUI.backgroundColor = Color.white;
        }
    
        public static void HorizontalLayout(Action callback,params GUILayoutOption[] gUILayoutOptions)
        {
            GUILayout.BeginHorizontal(gUILayoutOptions);
            {
                if (null != callback)
                {
                    callback.Invoke();
                }
            }
            GUILayout.EndHorizontal();
        }
    
        public static void VerticalLayout(Action callback, params GUILayoutOption[] gUILayoutOptions)
        {
            GUILayout.BeginVertical(gUILayoutOptions);
            {
                if (null != callback)
                {
                    callback.Invoke();
                }
            }
            GUILayout.EndVertical();
        }
    
    
        public static void BoxHorizontalLayout(Action callback, params GUILayoutOption[] gUILayoutOptions)
        {
            GUILayout.BeginHorizontal("box", gUILayoutOptions);
            {
                if (null != callback)
                {
                    callback.Invoke();
                }
            }
            GUILayout.EndHorizontal();
        }
    
        public static void BoxVerticalLayout(Action callback, params GUILayoutOption[] gUILayoutOptions)
        {
            GUILayout.BeginVertical("box", gUILayoutOptions);
            {
                if (null != callback)
                {
                    callback.Invoke();
                }
            }
            GUILayout.EndVertical();
        }
    
    
    
        #endregion

        #region String <=> Id


        private static Dictionary<string, int> s_IdDic = new Dictionary<string, int>();

        private static readonly object s_Lock = new object();

        private static int s_CurrentId = 999;

        public static int GetId(string text)
        {
            if (!s_IdDic.ContainsKey(text))
            {
                lock (s_Lock)
                {
                    s_IdDic.Add(text, s_CurrentId++);
                }
            }
            return s_IdDic[text];
        }

        public static string GetText(int id)
        {
            foreach (var kv in s_IdDic)
            {
                if (kv.Value==id)
                {
                    return kv.Key;
                }
            }
            return string.Empty;
        }

        #endregion

    }
    
}
