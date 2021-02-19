using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DapanzAI
{
    /// <summary>
    /// Agent数据
    /// </summary>
    [CreateAssetMenu(order = 3, menuName = "行为配置/敌人行为")]
    [System.Serializable]
    public class BTAgentData : AgentData
    {
        [Header("感知能力")]
        [Name("视野范围")]
        public float sight;//能发现该范围内事情变化（受到视野角度和遮挡影响）
        [Tooltip("前方扫描的范围，0是前方，90是上方，180是后面etc")]
        [Range(0.0f, 360.0f)]
        public float viewDirection = 0.0f;
        [Range(0.0f, 360.0f)]
        public float viewFov;
        [Name("听觉范围")]
        public float hearRange;//能发现该范围内事情变化
        [EnumName("听觉阈值")]
        public NioseLevel heroSill;//能发现该范围内事情变化（能听到的惊动等级）
        [Name("趋向距离")]
        public float Preference;//偏向和角色保持的最佳距离（为0时候不移动）
        [Name("丢失目标时间")]
        public float timeBeforeTargetLost;

        [Header("攻击性")]
        [Name("触发距离")]
        public float attackRange;//在距离内触发攻击
        [Name("攻击伤害值")]
        public int attackDamage;
        [Name("碰撞伤害值")]
        public int bodyDamage;
        [Name("攻击前摇")]
        public float preAttackTime;
        [Name("攻击持续")]
        public float continueAttackTime;
        [Name("攻击后摇")]
        public float aftterAttackTime;

        /// <summary>
        /// 通过攻击欲望获取
        /// </summary>
        /// <param name="desier">攻击欲望系数</param>
        /// <returns>Agent数据</returns>
        public static BTAgentData GetAgenData(AttackDesire desier)
        {
            BTAgentData _data = new BTAgentData();
            float[,] _enum = AIConst.AttackDesireDetail;
            int _desierIndex = (int)desier;
            _data.acceleration *= _enum[_desierIndex, (int)DesierItem.MoveSpeed];
            _data.maxSpeed *= _enum[_desierIndex, (int)DesierItem.MoveSpeed];

            _data.sight *= _enum[_desierIndex, (int)DesierItem.Sense];
            _data.viewFov *= _enum[_desierIndex, (int)DesierItem.Sense];
            _data.attackRange *= _enum[_desierIndex, (int)DesierItem.Sense];

            _data.timeBeforeTargetLost *= _enum[_desierIndex, (int)DesierItem.LostTarget];

            _data.Preference *= _enum[_desierIndex, (int)DesierItem.IntrestPos];

            _data.attackDamage = Mathf.RoundToInt(_data.attackDamage * _enum[_desierIndex, (int)DesierItem.damagerPct]);

            _data.preAttackTime *= _enum[_desierIndex, (int)DesierItem.PreAttack];
            _data.continueAttackTime *= _enum[_desierIndex, (int)DesierItem.CtnAttack];
            _data.aftterAttackTime *= _enum[_desierIndex, (int)DesierItem.AftAttack];
            return _data;
        }
    }
}
