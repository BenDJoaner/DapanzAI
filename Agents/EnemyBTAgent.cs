using System.Collections;
using UnityEngine;

namespace DapanzAI
{
    /// <summary>
    /// BT 代理人
    /// </summary>
    public class EnemyBTAgent : BTAgent
    {
        [Space]
        ////[Name("[配置]敌人行为")]
        public BTAgentData ebData;
        ////[EnumName("攻击欲望")]
        public AttackDesire desire;

        ////[Name("检测频率")]
        public float UpdateRate = 1;

        [HideInInspector]
        public Transform target;

        [HideInInspector]
        public float target_direct;

        [HideInInspector]
        public float target_angle;
        [HideInInspector]
        public float target_distance;

        Vector2 lockPosition;

        Transform Pretarget;//预先检测的目标目前是玩家

        public virtual void DamangerEnable(bool flag) { }

        public override void ActiveAI()
        {
            _ = mainNode.OpenBranch(
                BT.While(() => { return m_AIState != AIState.shutdown; }).OpenBranch(
                    AIAction.TryAction(this)
                ),
                BT.Call(OnDead),
                BT.Terminate()
            );
        }

        protected override AgentData Init()
        {
            return ebData;
        }
        /// <summary>
        /// 追踪目标
        /// </summary>
        public virtual void TrackTarget() { }

        /// <summary>
        /// 停止追踪目标
        /// </summary>
        public virtual void EndTrackTarget() { }

        /// <summary>
        /// 调用这个开始检测玩家位置
        /// </summary>
        public void StartTargetCheck(Transform _target)
        {
            Pretarget = _target;
            _ = StartCoroutine(CheckPlayer());
        }


        /// <summary>
        /// 更新AI状态
        /// </summary>
        /// <param name="_state"></param>
        public override void SetAIState(AIState _state)
        {
            switch (_state)
            {
                case AIState.sleep:
                    target = null;
                    break;
                case AIState.patrol:
                    target = null;
                    break;
                case AIState.battle:
                    target = Pretarget;
                    break;
            }
            m_AIState = _state;
        }

        /// <summary>
        /// 检测是否在攻击范围内
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckAttackRange()
        {
            if (target)
            {
                return target_distance <= ebData.attackRange;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 扫描目标在不在视觉范围内
        /// </summary>
        public void ScanForTarget()
        {
            //Debug.Log(target_direct + ">" + ebData.sight * ebData.viewDirection + " / " + target_angle + ">" + (ebData.viewFov * 0.5f));
            if (target_direct > ebData.sight * ebData.sight)
            {
                SetAIState(AIState.patrol);
                return;
            }
            if (target_angle > ebData.viewFov * 0.5f)
            {
                SetAIState(AIState.patrol);
                return;
            }
            SetAIState(AIState.battle);
        }

        /// <summary>
        /// 执行面对目标
        /// </summary>
        public void FaceToTarget()
        {
            if (target.position.x - transform.position.x > 0)
            {
                forceFace = Facing.right;
            }
            else
            {
                forceFace = Facing.left;
            }
        }

        /// <summary>
        /// 检测是否接触墙面
        /// </summary>
        /// <returns></returns>
        public Facing IsAttachWall()
        {
            return attachWall;
        }

        /// <summary>
        /// 锁定目标
        /// </summary>
        public void LockTargetPosition()
        {
            lockPosition = target.position;
        }

        /// <summary>
        /// 获取锁定目标
        /// </summary>
        /// <returns></returns>
        public Vector2 GetLockedPosition()
        {
            return lockPosition;
        }

        /// <summary>
        /// 检测目标信息协程，检测速度为UpdateRate
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckPlayer()
        {
            while (Pretarget != null)
            {
                Vector3 dir = Pretarget.position - transform.position;
                target_direct = dir.sqrMagnitude;
                Vector2 curDire = AutoFace == Facing.right ? Vector2.right : Vector2.left;
                Vector3 testForward = Quaternion.Euler(0, 0, Mathf.Sign(curDire.x) * ebData.viewDirection) * curDire;
                target_angle = Vector3.Angle(testForward, dir);
                target_distance = Vector2.Distance(Pretarget.position, transform.position);
                //print("check >>>> " + target_direct + "/" + target_angle + "/" + target_distance);
                yield return new WaitForSeconds(UpdateRate);
            }
        }
    }

}
