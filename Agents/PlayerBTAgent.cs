using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapanzAI
{
    public class PlayerBTAgent:BTAgent
    {
        public BTAgentData BTData;
        protected BTNode selfAction;
        protected override AgentData Init()
        {
            ActiveAI();
            return BTData;
        }

        public override void ActiveAI()
        {
            base.ActiveAI();
            _ = mainNode.OpenBranch(
                BT.While(() => { return m_AIState != AIState.shutdown; }).OpenBranch(
                    //BT.Call(AIAction)
                ),
                BT.Call(OnDead),
                BT.Terminate()
            );
        }

        protected BTNode AIAction()
        {
            return BT.Root().OpenBranch(
                BT.If(() => { return m_AIState == AIState.sleep; }).OpenBranch(
                    
                ),
                BT.If(() => { return m_AIState == AIState.patrol; }).OpenBranch(
                    
                ),
                BT.If(() => { return m_AIState == AIState.battle; }).OpenBranch(
                    
                )
            );
        }



        public override void SetAIState(AIState _state)
        {
            base.SetAIState(_state);
            switch (_state)
            {
                case AIState.sleep:
                    break;
                case AIState.patrol:
                    break;
                case AIState.battle:
                    break;
            }
        }
    }
}
