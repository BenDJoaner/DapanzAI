
using UnityEngine;

namespace DapanzAI
{
    /// <summary>
    /// 行为节点基类
    /// </summary>
    public class ActionBase : ScriptableObject
    {
        /// <summary>
        /// 用于赋值执行的节点
        /// </summary>
        protected BTNode selfAction;
        /// <summary>
        /// 尝试执行节点
        /// </summary>
        /// <param name="behavier"> 传入 BTAgent 方可获取信息</param>
        /// <returns> 返回BTNode 可用于赋值其他 ActionBase 的派生类</returns>
        public virtual BTNode TryAction(EnemyBTAgent behavier)
        {
            return selfAction;
        }
    }

}
