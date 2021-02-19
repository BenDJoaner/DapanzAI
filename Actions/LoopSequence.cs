using System;
using UnityEngine;

namespace DapanzAI.Actions
{
    [CreateAssetMenu(order = 2, menuName = "行为/控制节点/循环执行序列",fileName ="循序执行")]
    [Serializable]
    public class LoopSequence:ActionBase
    {
        [Name("行为")]
        public ActionBase[] actionList;
        public override BTNode TryAction(BTAgent behavier)
        {
            BTNode[] nodeList = new BTNode[actionList.Length];
            for (int i = 0; i < actionList.Length; i++)
            {
                ActionBase _action = (ActionBase)actionList[i];
                nodeList[i] = _action.TryAction(behavier);
            }
            selfAction = BT.Sequence().OpenBranch(nodeList);
            return base.TryAction(behavier);
        }
    }
}