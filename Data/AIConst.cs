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
        [EnumName("在冰上")]
        Ice,
    }

    /// <summary>
    /// 攻击欲望系数
    /// </summary>
    public enum DesierItem {
        /// <summary>
        /// 感知范围
        /// </summary>
        Sense,
        /// <summary>
        /// 攻击前摇
        /// </summary>
        PreAttack,
        /// <summary>
        /// 攻击持续时间
        /// </summary>
        CtnAttack,
        /// <summary>
        /// 攻击后摇
        /// </summary>
        AftAttack,
        /// <summary>
        /// 移动速度
        /// </summary>
        MoveSpeed,
        /// <summary>
        /// 优势位置
        /// </summary>
        IntrestPos,
        /// <summary>
        /// 丢失目标时间
        /// </summary>
        LostTarget,
        /// <summary>
        /// 伤害百分比
        /// </summary>
        damagerPct
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
        [EnumName("正常")]
        normal,
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
        /// <summary>
        /// 地面检测半径
        /// </summary>
        public static float k_GroundedRadius = 0.2f; 
        /// <summary>
        /// 墙壁检测半径
        /// </summary>
        public static float k_WallRadius = .05f;
        /// <summary>
        /// 在不同材质上行走速度
        /// </summary>
        public static float[] WalkSpeedPct = {0.8f,1,1,0.5f,0.8f};
        /// <summary>
        /// 难度系数
        /// {[0]=感知，[1]=前摇，[2]=攻击持续, [3]=后摇，[4]=速度，[5]=优势距离}
        /// </summary>
        public static float[,] AttackDesireDetail ={
        //      感知      前摇          攻击持续      后摇            速度        优势距离         丢失目标         伤害比值
            {0,         5,         0.1f,        1.7f,          -1,          999,           0.1f,          1         },//胆小
            {0.5f,      2,         0.5f,        1.5f,          0.3f,        2f,            0.3f,          1          },//木头
            {0.7f,    1.5f,        0.6f,        1.2f,          0.7f,        1.5f,            0.7f,          1         },//小心
            {1,         1,           1,          1,              1,           1,               1,             1             },//正常
            {1.5f,     0.7f,         1,         0.8f,          1.5f,        0.5f,            1.2f,          2          },//积极
            {2f,       0.5f,        1.5f,       0.6f,          2,           0.2f,            1.5f,          4          }//疯狂
        };
    }


}