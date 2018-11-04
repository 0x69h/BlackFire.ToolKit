# Timer

> Black ToolKit 提供定时器服务。

### 属性

**<font color=red>游戏世界是受Time Scale影响的，现实世界不受Time Scale影响。</font>**


### 方法

- 延迟n秒执行



```csharp
 Timer.Delay(1f).On(()=>{ Debug.Log("延迟一秒执行");}); //游戏世界里面延迟一秒后执行
 
 Timer.RealDelay(1f).On(()=>{ Debug.Log("延迟一秒执行");}); //现实世界里面延迟一秒后执行
```


- 延迟n帧执行


```csharp
 Timer.DelayFrame(60).On(()=> { Debug.Log("延迟60帧执行"); }); 
```


- 每隔n个间隔时间后执行


```csharp
 Timer.Interval(3f).On(() => { Debug.Log("每隔3s执行");});//游戏世界里面每隔3s后执行
 
 Timer.RealInterval(3f).On(() => { Debug.Log("每隔3s执行");});//现实世界里面每隔3s后执行
```

- 每隔n帧后执行


```csharp
 Timer.IntervalFrame(3).On(() => { Debug.Log("每隔3帧执行"); });
```


- 循环执行n次


```csharp
Timer.Loop(3f).On(() => { Debug.Log("3s内每一帧循环执行"); }); //游戏世界里面3s内每一帧执行

Timer.RealLoop(3f).On(() => { Debug.Log("3s内每一帧循环执行"); }); //现实世界里面3s内每一帧执行

Timer.LoopFrame(60).On(() => { Debug.Log("60帧内每一帧循环执行"); }); //游戏世界里面60帧内每一帧执行
```
-