# Debugger

### Debugger 简介

> Debugger 是在UnityEngine编辑器运行时和发布后真机运行时提供给开发团队的高效程序调试器。

### Debugger 设计

> UnityEngine的GUI交互系统、模块化导航的GUI渲染模式、模块渲染和调试器事件的接口扩展。

* Mini Debugger Style

![Debugger Mini Style](https://github.com/BlackFire-Studio/BlackFireFramework/blob/wiki/Images/mini.png)

* Full Debugger Style

![Debugger Full Style](https://github.com/BlackFire-Studio/BlackFireFramework/blob/wiki/Images/full.png)

### Debugger 使用

* 扩展一个渲染模块

> 在项目工程中开发者只需要创建一个类实现IDebuggerModuleGUI接口就可以轻松地扩展一个Debugger的GUI渲染模块。

用例:

```csharp 
//这是一个名为Setting的Debugger Module GUI 渲染模块的实现类。
public sealed class DebuggerSettingGUI : IDebuggerModuleGUI
 {
        //这个是Debugger左侧导航栏的排序优先级(比如0的优先级高于1)。
        public int Priority 
        {
            get
            {
                return 2;
            }
        }

        //这个是模块名字，他将会显示在Debugger的左侧导航栏与右侧正文内容的标题栏。
        public string ModuleName
        {
            get
            {
                return "Setting";
            }
        }

        //模块的初始化事件。
        public void OnInit(DebuggerManager debuggerManager)
        {

        }

        //模块的GUI渲染事件。
        public void OnModuleGUI()
        {

        }
        
        //模块被销毁事件。
        public void OnDestroy()
        {
            
        }
}
```

# 实现一个内部事件

用例:

```csharp

    //Debugger 显示模式切换的事件接口的实现。
    public sealed class DebuggerStyleChangedCallback : IDebuggerStyleChangeCallback
    {
        //事件接口排序的优先级(比如0的优先级高于1)。项目开发中只会调用优先级最高的实现类。
        public int Priority
        {
            get
            {
                return 66666;
            }
        }

        //切换到Hidden模式的事件回调方法。
        public bool HiddenStylePredicate(DebuggerManager debuggerManager)
        {
            return Input.GetKeyDown(KeyCode.F1);
        }

        //切换到Mini模式的事件回调方法。
        public bool MiniStylePredicate(DebuggerManager debuggerManager)
        {
            return Input.GetKeyDown(KeyCode.F2);
        }

        //切换到Full模式的事件回调方法。
        public bool FullStylePredicate(DebuggerManager debuggerManager)
        {
            return Input.GetKeyDown(KeyCode.F3);
        }

    }
```

