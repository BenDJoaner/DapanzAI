using System;
using UnityEngine;

namespace DapanzAI.Actions
{
    [CreateAssetMenu(order = 4, menuName = "行为/控制节点/执行N次", fileName = "执行N次")]
    [Serializable]
    public class RepeatNTimes : ActionBase
    {
        //[Name("次数")]
        public int Times;
        //[Name("行为")]
        public ActionBase[] actionList;
        public override BTNode TryAction(BTAgent behavier)
        {
            BTNode[] nodeList = new BTNode[actionList.Length];
            selfAction = BT.Repeat(Times).OpenBranch(nodeList);
            return base.TryAction(behavier);
        }
    }
}
