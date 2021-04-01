using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DapanzAI
{
    public class BTAgent: AgentBase
    {
        [Space]
        //[EnumName("AI状态")]
        public AIState m_AIState;
        //[Name("AI文件")]
        public ActionBase AIAction;

        protected Root mainNode = BT.Root();

        protected override AgentData Init()
        {
            return base.Init();
        }

        /// <summary>
        /// 启动AI
        /// </summary>
        public virtual void ActiveAI()
        {
            SetAIState(AIState.sleep);
        }

        /// <summary>
        /// 更新AI状态
        /// </summary>
        /// <param name="_state"></param>
        public virtual void SetAIState(AIState _state)
        {
            m_AIState = _state;
        }

        protected override void UpdateInput()
        {
            if (m_AIState != AIState.shutdown)
            {
                mainNode.Tick();
            }
        }

        /// <summary>
        /// 死亡
        /// </summary>
        protected virtual void OnDead() { }
    }
}
