using UnityEngine;

namespace DapanzAI.Actions
{
    [CreateAssetMenu(order = 3, menuName = "行为/控制节点/随机执行", fileName = "随机执行")]
    public class RandomSequence:ActionBase
    {
        [Name("行为")]
        public ActionBase[] actionList;
        [Name("权重")]
        public int[] weights;
        public override BTNode TryAction(BTAgent behavier)
        {
            BTNode[] nodeList = new BTNode[actionList.Length];
            for (int i = 0; i < actionList.Length; i++)
            {
                ActionBase _action = (ActionBase)actionList[i];
                nodeList[i] = _action.TryAction(behavier);
            }
            selfAction = BT.RandomSequence(weights).OpenBranch(nodeList);
            return base.TryAction(behavier);
        }
    }
}
