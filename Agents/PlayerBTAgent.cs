using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapanzAI
{
    public class PlayerBTAgent:BTAgent
    {
        protected override AgentData Init()
        {
            ActiveAI();
            return base.Init();
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
