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
        Air,//在空中
        Grass,//草地上
        Rock,//石头上
        Water,//水上
        Sand,//沙上
    }

    /// <summary>
    /// 控制器状态
    /// </summary>
    public enum ControlState
    {
        sleep,//休眠
        awake//启动
    }

    /// <summary>
    /// 面朝方向
    /// </summary>
    public enum Facing
    {
        none,//没有
        right,
        left
    }

    /// <summary>
    /// 攻击欲望
    /// </summary>
    public enum AttackDesire
    {
        cowardly,//胆小
        woody,//木头
        careful,//小心
        positive,//积极
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
        sleep,//休眠
        patrol,//巡逻（非战斗状态）
        battle,//战斗
    }

    public static class AIConst
    {
        //地面检测相关
        public static float k_GroundedRadius = 0.2f;     //地面检测半径
        public static float k_WallRadius = .05f;        //墙壁检测半径
    }
}