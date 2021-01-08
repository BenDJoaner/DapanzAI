# DapanzAI 大片紫AI工具（2D）

> 这是一个在《记忆岛》中分离出来的功能，整合了行为树、移动控制器、行为控制器、Inspector参数重命名...

**简述:**
  - 可以提供一套方块快捷建立AI敌人、队友和操作人物角色的简单逻辑
  - 生成Asset文件，进行组件赋值，方面给美术和策划同学编辑敌人的AI行为

## BT 行为树/Behavior Tree
| 节点函数(func) | 描述 (summary) | 参数(param) | 参数含义(param means) |
| :----: | ----------------------------- | :----------: |:----------: |
| Root |节点，可用于任意节点操作|	 -  |	 -  |
| Sequence |执行序列，如果子节点执行失败，则终止|	 -  |	 -  |
| Selector |执行每个子节点，如果有一个成功就返回true，否则返回false|	 bool  |	 -  |
| RunCoroutine |调用协程|	 IEnumerator  |	 -  |
| Action |调用方法|	 function  |	 -  |
| If |运行结果为true执行|	 function  |	 -  |
| If |参数为true执行|	 bool  |	 -  |
| While |运行结果为true则一直执行|	 function  |	 -  |
| While |参数结果为true则一直执行|	 bool  |	 -  |
| Condition |调用一个方法，如果方法返回true，则返回成功，否则返回失败|	 function  |	 -  |
| Repeat |重复运行N次|	 int  |	 -  |
| Wait |  	节点，可用于任意节点操作         |	 float  |	 -  |
| Trigger |（Animator）相当于动画状态机SetTrigger|	 Animator,string,bool  |	 -  |
| WaitForAnimatorState |（Animator）等待动画进入到某个Clip后继续|	 Animator,string,int  |	 -  |
| SetBool |（Animator）相当于动画状态机SetBool|	 Animator,string,bool  |	 -  |
| SetActive |设置对象可见|	 GameObject,bool  |	 Animator,string,bool  |
| WaitForAnimatorSignal |等待动画器上的SendSignal状态机行为收到信号|	 Animator,string,string,int  |	 -  |
| LoopCountdown | 在时间内持续执行，时间结束跳出|	 float  |	 -  |
| RandomSequence | 随机序列|	 int[]  |	 -  |
| Log |打印日志|	 string  |	 -  |
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
| 节点函数 | 描述                          | 参数 |
| :----: | ----------------------------- | :----------: |
| Root |  	节点，可用于任意节点操作         |	 -  |
## Agents 控制器/Controller

## Data 可编程数据/Scriptable Data

## Editor

## Episode 控制数据封装
