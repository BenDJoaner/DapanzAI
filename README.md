# DapanzAI 大片紫AI工具（2D）

> 这是一个在《记忆岛》中分离出来的功能，整合了行为树、移动控制器、行为控制器、Inspector参数重命名...

**简述:**
  - 可以提供一套方块快捷建立AI敌人、队友和操作人物角色的简单逻辑
  - 生成Asset文件，进行组件赋值，方面给美术和策划同学编辑敌人的AI行为

## BT 行为树/Behavior Tree
| 节点函数(func) | 描述 (summary) | 参数(param) | 返回(return) |
| :----: | ----------------------------- | :----------: |:----------: |
| Root |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Sequence |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Selector |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Action |  	节点，可用于任意节点操作         |	 -  |	 -  |
| ConditionalBranch |  	节点，可用于任意节点操作         |	 function  |	 -  |
| ConditionalBranch |  	节点，可用于任意节点操作         |	 -  |	 -  |
| While |  	节点，可用于任意节点操作         |	 function  |	 -  |
| While |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Condition |  	节点，可用于任意节点操作         |	 function  |	 -  |
| Repeat |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Wait |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Trigger |  	节点，可用于任意节点操作         |	 -  |	 -  |
| WaitForAnimatorState |  	节点，可用于任意节点操作         |	 -  |	 -  |
| SetBool |  	节点，可用于任意节点操作         |	 -  |	 -  |
| SetActive |  	节点，可用于任意节点操作         |	 -  |	 -  |
| WaitForAnimatorSignal |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Terminate |  	节点，可用于任意节点操作         |	 -  |	 -  |
| Log |  	节点，可用于任意节点操作         |	 -  |	 -  |
| RandomSequence |  	节点，可用于任意节点操作         |	 -  |	 -  

## Actions 行为节点/Action Nodes
### ActionBase 为节点基类
可以继承此类来创造新需要的节点，并且互相合并
```
    protected BTNode selfAction;
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
