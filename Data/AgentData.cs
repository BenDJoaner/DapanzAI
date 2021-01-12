using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DapanzAI
{
    [CreateAssetMenu(order = 2, menuName = "行为配置/基础行为配置")]
    [Serializable]
    public class AgentData : ScriptableObject
    {
        [Header("移动")]
        [ShowName("加速度")]
        public float acceleration;// = 300;
        [ShowName("最大速度")]
        public float maxSpeed;// = 13;
        [ShowName("最小转身速度")]
        public float minFlipSpeed;// = 0.4f;
        [ShowName("空中移动速度比例")]
        public float airMoveScale;// = 1f;

        [Header("重力")]
        [ShowName("跳跃时重力")]
        public float jumpGravityScale;// = 1.0f;
        [ShowName("掉落时重力")]
        public float fallGravityScale;// = 17f;
        [ShowName("地面时重力")]
        public float groundedGravityScale;// = 1.0f;

        [Header("跳跃行为(弹跳力为0不检测地面)")]
        [ShowName("弹跳力")]
        public float jumpForce;// = 15;
        [ShowName("按住最大时间")]
        public float pressJumpMaxTime;// = 0.4f;//按住最大时间
        [ShowName("按住时向上推力")]
        public float pressJumpFoece;// = 2.5f;//按住jumpfoce
    }
}

