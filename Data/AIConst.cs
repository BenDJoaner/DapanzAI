using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DapanzAI
{
    /// <summary>
    /// 接触地面类型
    /// 用于播放特效、音效、动画、移动速度等 
    /// </summary>
    public enum GroundType
    {
        [EnumNameAttribute("在空中")]
        Air,//在空中
        [EnumNameAttribute("草地上")]
        Grass,//草地上
        [EnumNameAttribute("石头上")]
        Rock,//石头上
        [EnumNameAttribute("水面上")]
        Water,//水上
        [EnumNameAttribute("沙地上")]
        Sand,//沙上
    }

    /// <summary>
    /// 控制器状态
    /// </summary>
    public enum ControlState
    {
        [EnumNameAttribute("休眠")]
        sleep,//休眠
        [EnumNameAttribute("启动")]
        awake//启动
    }

    /// <summary>
    /// 面朝方向
    /// </summary>
    public enum Facing
    {
        [EnumNameAttribute("无")]
        none,//没有
        [EnumNameAttribute("右")]
        right,
        [EnumNameAttribute("左")]
        left
    }

    /// <summary>
    /// 攻击欲望
    /// </summary>
    public enum AttackDesire
    {
        [EnumNameAttribute("胆小")]
        cowardly,//胆小
        [EnumNameAttribute("木头")]
        woody,//木头
        [EnumNameAttribute("小心")]
        careful,//小心
        [EnumNameAttribute("积极")]
        positive,//积极
        [EnumNameAttribute("疯狂")]
        craze//疯狂
    }

    /// <summary>
    /// 警觉程度
    /// </summary>
    public enum NioseLevel
    {
        silent,//无声
        whisper,//轻声
        low,//小声
        normal,//正常
        loud,//大声
        extreme,//超大声
    }

    /// <summary>
    /// AI状态
    /// </summary>
    public enum AIState
    {
        /// <summary>
        /// 关机状态，不进行BT.Tick，没有任何感知
        /// </summary>
        shutdown,
        /// <summary>
        /// 休眠状态，进行休眠的BT逻辑，对声音和受击有感知，但对玩家不造成伤害
        /// </summary>
        sleep,
        /// <summary>
        /// 巡逻状态，进行巡逻，对声音，可见区域的事件有感知
        /// </summary>
        patrol,
        /// <summary>
        /// 有且仅有一个目标，进行跟踪和攻击
        /// </summary>
        battle,//战斗
    }

    public static class AIConst
    {
        //地面检测相关
        public static float k_GroundedRadius = 0.2f;     //地面检测半径
        public static float k_WallRadius = .05f;        //墙壁检测半径
    }
}