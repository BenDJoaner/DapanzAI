using System;
using UnityEngine;

namespace DapanzAI.Actions
{
    [CreateAssetMenu(order = 1, menuName = "行为/控制节点/根据战斗状态执行", fileName = "状态执行")]
    [Serializable]
    public class AIStateCondition:ActionBase
    {
        [Header("睡眠时行为")]
        public ActionBase[] sleepActionList;
        [Header("巡视时行为")]
        public ActionBase[] patrolActionList;
        [Header("战斗时行为")]
        public ActionBase[] battleActionList;
        private BTNode[] _sleep_node_list;
        private BTNode[] _patrol_node_list;
        private BTNode[] _battle_node_list;
        public override BTNode TryAction(BTAgent behavier)
        {
            _sleep_node_list = new BTNode[sleepActionList.Length];
            for (int i = 0; i < sleepActionList.Length; i++)
            {
                ActionBase _action = sleepActionList[i];
                _sleep_node_list[i] = _action.TryAction(behavier);
            }

            _patrol_node_list = new BTNode[patrolActionList.Length];
            for (int i = 0; i < patrolActionList.Length; i++)
            {
                ActionBase _action = patrolActionList[i];
                _patrol_node_list[i] = _action.TryAction(behavier);
            }

            _battle_node_list = new BTNode[battleActionList.Length];
            for (int i = 0; i < battleActionList.Length; i++)
            {
                ActionBase _action = battleActionList[i];
                _battle_node_list[i] = _action.TryAction(behavier);
            }

            selfAction = BT.Root().OpenBranch(
                BT.If(() => { return behavier.m_AIState == AIState.sleep; }).OpenBranch(
                    _sleep_node_list
                ),
                BT.If(() => { return behavier.m_AIState == AIState.patrol; }).OpenBranch(
                    _patrol_node_list
                ),
                BT.If(() => { return behavier.m_AIState == AIState.battle; }).OpenBranch(
                    _battle_node_list
                )
            );
            return base.TryAction(behavier);
        }
    }
}
