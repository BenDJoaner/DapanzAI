namespace DapanzAI
{
    /// <summary>
    /// 接触地面类型
    /// 用于播放特效、音效、动画、移动速度等 
    /// </summary>
    public enum GroundType
    {
        [EnumName("在空中")]
        Air,
        [EnumName("草地上")]
        Grass,
        [EnumName("石头上")]
        Rock,
        [EnumName("水面上")]
        Water,
        [EnumName("沙地上")]
        Sand,
    }

    /// <summary>
    /// 控制器状态
    /// </summary>
    public enum ControlState
    {
        [EnumName("休眠")]
        sleep,
        [EnumName("启动")]
        awake
    }

    /// <summary>
    /// 面朝方向
    /// </summary>
    public enum Facing
    {
        [EnumName("无")]
        none,
        [EnumName("右")]
        right,
        [EnumName("左")]
        left
    }

    /// <summary>
    /// 攻击欲望
    /// </summary>
    public enum AttackDesire
    {
        [EnumName("胆小")]
        cowardly,
        [EnumName("木头")]
        woody,
        [EnumName("小心")]
        careful,
        [EnumName("积极")]
        positive,
        [EnumName("疯狂")]
        craze
    }

    /// <summary>
    /// 警觉程度
    /// </summary>
    public enum NioseLevel
    {
        [EnumName("无声")]
        silent,
        [EnumName("轻声")]
        whisper,
        [EnumName("小声")]
        low,
        [EnumName("正常")]
        normal,
        [EnumName("大声")]
        loud,
        [EnumName("超大声")]
        extreme,
    }

    /// <summary>
    /// AI状态
    /// </summary>
    public enum AIState
    {
        /// <summary>
        /// 关机状态，不进行BT.Tick，没有任何感知，无碰撞伤害
        /// </summary>
        [EnumName("关机")] shutdown,
        /// <summary>
        /// 休眠状态，进行休眠的BT逻辑，对声音和受击有感知，无碰撞伤害
        /// </summary>
        [EnumName("睡觉")] sleep,
        /// <summary>
        /// 巡逻状态，进行巡逻，对声音，可见区域的事件有感知，有碰撞伤害
        /// </summary>
        [EnumName("巡视")] patrol,
        /// <summary>
        /// 有且仅有一个目标，进行跟踪和攻击，有碰撞伤害
        /// </summary>
        [EnumName("战斗")] battle,//战斗
    }

    public static class AIConst
    {
        //地面检测相关
        public static float k_GroundedRadius = 0.2f;     //地面检测半径
        public static float k_WallRadius = .05f;        //墙壁检测半径
    }
}