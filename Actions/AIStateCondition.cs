using System;
using UnityEngine;

namespace DapanzAI.Actions
{
    [CreateAssetMenu(order = 1, menuName = "行为/控制节点/根据战斗状态执行", fileName = "状态执行")]
    [Serializable]
    public class AIStateCondition:ActionBase
    {
        public ActionBase[] sleepActionList;
        public ActionBase[] patrolActionList;
        public ActionBase[] battleActionList;

        BTNode[] _sleep_node_list;
        BTNode[] _patrol_node_list;
        BTNode[] _battle_node_list;
        public override BTNode TryAction(BTAgent behavier)
        {
            _sleep_node_list = new BTNode[sleepActionList.Length];
            for (int i = 0; i < sleepActionList.Length; i++)
            {
                ActionBase _action = (ActionBase)sleepActionList[i];
                _sleep_node_list[i] = _action.TryAction(behavier);
            }

            _patrol_node_list = new BTNode[patrolActionList.Length];
            for (int i = 0; i < patrolActionList.Length; i++)
            {
                ActionBase _action = (ActionBase)patrolActionList[i];
                _patrol_node_list[i] = _action.TryAction(behavier);
            }

            _battle_node_list = new BTNode[battleActionList.Length];
            for (int i = 0; i < battleActionList.Length; i++)
            {
                ActionBase _action = (ActionBase)battleActionList[i];
                _battle_node_list[i] = _action.TryAction(behavier);
            }

            selfAction = BT.Root().OpenBranch(
                BT.If(behavier.m_AIState == AIState.sleep).OpenBranch(
                    BT.While(behavier.m_AIState == AIState.sleep).OpenBranch(_sleep_node_list)
                ),
                BT.If(behavier.m_AIState == AIState.patrol).OpenBranch(
                    BT.While(behavier.m_AIState == AIState.patrol).OpenBranch(_patrol_node_list)
                ),
                BT.If(behavier.m_AIState == AIState.battle).OpenBranch(
                    BT.While(behavier.m_AIState == AIState.battle).OpenBranch(_battle_node_list)
                )
            );
            return base.TryAction(behavier);
        }
    }
}
