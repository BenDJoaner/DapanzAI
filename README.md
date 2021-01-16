# DapanzAI Tools(2D)

> 这是一个在《记忆岛》中分离出来的功能，整合了行为树、移动控制器、行为控制器、Inspector参数重命名...

**简述:**
  - 可以提供一套方块快捷建立AI敌人、队友和操作人物角色的简单逻辑
  - 生成Asset文件，进行组件赋值，方面给美术和策划同学编辑敌人的AI行为

## BT 行为树/Behavior Tree
| 节点函数(func) | 描述 (summary) | 参数(param) | 参数含义(param means) |
| :----: | ----------------------------- | :----------: |:----------: |
| Root |节点，可用于任意节点操作|	 -  |	 -  |
| Sequence |执行序列，如果子节点执行失败，则终止|	 -  |	 -  |
| Selector |执行每个子节点，如果有一个成功就返回true，否则返回false|	 bool  |	 是否随机执行  |
| RunCoroutine |调用协程|	 IEnumerator  |	 协程  |
| Action |调用方法|	 function  |	 方法  |
| If |运行结果为true执行|	 function  |	 运行的方法  |
| If（重载） |参数为true执行|	 bool  |	 参数  |
| While |运行结果为true则一直执行|	 function  |	 运行的方法  |
| While（重载） |参数结果为true则一直执行|	 bool  |	 参数  |
| Condition |调用一个方法，如果方法返回true，则返回成功，否则返回失败|	 function  |	 运行的方法  |
| Repeat |重复运行N次|	 int  |	 运行的次数  |
| Wait |  	节点，可用于任意节点操作         |	 float  |	 等待的时间  |
| Trigger |（Animator）相当于动画状态机SetTrigger|	 Animator,string,bool  |	 状态机，状态名称，不用Reset=true  |
| WaitForAnimatorState |（Animator）等待动画进入到某个Clip后继续|	 Animator,string,int  |	 状态机，状态名，Layer  |
| SetBool |（Animator）相当于动画状态机SetBool|	 Animator,string,bool  |	 状态机，状态名，值  |
| SetActive |设置对象可见|	 GameObject,bool  |	 对象,可见 |
| WaitForAnimatorSignal |等待动画器上的SendSignal状态机行为收到信号|	 Animator,string,string,int  |	 状态机，状态名，动画状态，Layer  |
| LoopCountdown | 在时间内持续执行，时间结束跳出|	 float  |	 执行时间  |
| RandomSequence | 随机序列|	 int[]  |	 权重（不填则全为1）  |
| Log |打印日志|	 string  |	 文本内容  |
| Terminate | 终止BT|	 -  |	 -  |

## Actions 行为节点/Action Nodes
### ActionBase 为节点基类
可以继承此类来创造新需要的节点，并且互相合并
```
/*
 * 赋值该参数
 * selfAction = BT.Root().OpenBranch();
 */
    
//重写该方法：
public override BTNode TryAction(BTAgent behavier)
{
    return base.TryAction(selfAction);
}

```
### 举例：
```

using System;
using DapanzAI;
using UnityEngine;

[CreateAssetMenu(order = 2, menuName = "行为/技能/普通攻击")]
[Serializable]
public class AttackOnce : ActionBase
{
    public float formerTime;
    [FieldLabel("生效时间")]
    public float effectTime;
    [FieldLabel("结束时间")]
    public float overTime;
    public string attackTrigger = "attack";
    public string waitForState = "Idle";
    public string trackTrigger = "Idle";

    public override BTNode TryAction(BTAgent behavier)
    {
        Animator anim = behavier.Anim;
        selfAction = BT.Root().OpenBranch(
            BT.If(behavier.CheckAttackRange).OpenBranch(
                BT.Trigger(anim, attackTrigger),
                BT.Wait(formerTime),
                BT.Call(() => { behavier.DamangerEnable(true); }),
                BT.Wait(effectTime),
                BT.Call(()=> { behavier.DamangerEnable(false); }),
                BT.Wait(overTime),
                BT.WaitForAnimatorState(anim, waitForState)
            ),
            BT.Call(behavier.FaceToTarget),
            BT.Call(behavier.TrackTarget),
            BT.SetBool(anim, trackTrigger, true)

        );
        return base.TryAction(behavier);
    }
}
```
### 通用节点
| 节点函数 | 类别 | 描述 |
| :----: | ----------------------------- | :----------: |
| AIStateCondition |控制节点|	 根据战斗状态执行  |
| LoopSequence |控制节点|	 循环执行序列  |
| RandomSequence |控制节点|	 随机执行  |
| RepeatNTimes |控制节点|	 执行N次  |
## Agents 控制器/Controller

## Data 可编程数据/Scriptable Data

## Editor

## Episode 控制数据封装
