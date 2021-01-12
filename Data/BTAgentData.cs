using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DapanzAI
{
    [CreateAssetMenu(order = 3, menuName = "行为配置/敌人行为")]
    [System.Serializable]
    public class BTAgentData : AgentData
    {
        [Header("感知能力")]
        [ShowName("视野范围")]
        public float sight;//能发现该范围内事情变化（受到视野角度和遮挡影响）
        [Tooltip("前方扫描的范围，0是前方，90是上方，180是后面etc")]
        [Range(0.0f, 360.0f)]
        public float viewDirection = 0.0f;
        [Range(0.0f, 360.0f)]
        public float viewFov;
        [ShowName("听觉范围")]
        public float hearRange;//能发现该范围内事情变化
        [EnumName("听觉阈值")]
        public NioseLevel heroSill;//能发现该范围内事情变化（能听到的惊动等级）
        [ShowName("趋向距离")]
        public float Preference;//偏向和角色保持的最佳距离（为0时候不移动）
        [ShowName("丢失目标时间")]
        public float timeBeforeTargetLost;

        [Header("攻击性")]
        [EnumName("攻击欲望")]
        public AttackDesire desire;//能发现该范围内事情变化
        [ShowName("触发距离")]
        public float attackRange;//在距离内触发攻击
    }
}
