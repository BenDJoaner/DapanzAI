using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DapanzAI
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class AgentBase : MonoBehaviour
    {
        //[FieldLabel("基础行为状态")]
        public ControlState state = ControlState.sleep;
        //[FieldLabel("重力生效")]
        public bool updateGravity = true;
        //[FieldLabel("脚踩的东西")]
        public GroundType groundType;
        //[FieldLabel("角色模型")]
        public Transform puppet = null;
        //[FieldLabel("手部位置")]
        public Transform handAnchor = null;
        //[FieldLabel("自动面向")]
        public Facing AutoFace = Facing.right;
        //[FieldLabel("强制面向")]
        public Facing forceFace = Facing.none;
        //[FieldLabel("碰到墙")]
        public Facing attachWall = Facing.none;
        [Header("左右/地面检查点偏移")]
        [Range(0, -5)]
        public float leftCheckOffset = 1;
        [Range(0, 5)]
        public float rightCheckOffset = -1;
        [Range(0, -5)]
        public float groundCheckOffset = -0.3f;

        LayerMask grassGroundMask;
        LayerMask rockGroundMask;
        LayerMask waterGroundMask;
        LayerMask sandGroundMask;
        LayerMask wallMask;
        LayerMask breakableWallMask;

        Vector2 flippedScale;

        Transform leftChecker;
        Transform rightChecker;
        Transform groundChecker;
        AgentData data;

        private Animator anim;
        protected Rigidbody2D controllerRigidbody;

        bool isJumping;
        float jumpPressTime;

        public EpisodeBase episode = new EpisodeBase();
        public Animator Anim { get => anim; }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual AgentData Init() { return new AgentData(); }
        /// <summary>
        /// 输入更新
        /// Input操作在此函数中更新，以确保当前帧的指令可以执行
        /// </summary>
        protected virtual void UpdateInput() { }
        /// <summary>
        /// 更新特殊状态
        /// 此处为执行完Input和完成指令操作后执行
        /// </summary>
        protected virtual void UpdateState() { }
        /// <summary>
        /// 更新动画信息
        /// Update中最后才调用的方法
        /// </summary>
        protected virtual void UpdateAnim() { }
        /// <summary>
        /// 起跳
        /// 仅限于从陆地状态 -> 空中状态触发，其他技能、二段跳等不触发
        /// </summary>
        protected virtual void OnJump() { }
        /// <summary>
        /// 落地
        /// 任何时候在空中状态 -> 陆地状态触发
        /// </summary>
        protected virtual void OnLand() { }
        /// <summary>
        /// 碰到墙
        /// 任何状态只要有一边第一次接触墙体时候触发
        /// </summary>
        /// <returns>墙体相对角色的位置</returns>
        protected virtual Facing OnHitWall() { return attachWall; }

        void Start()
        {
            grassGroundMask = LayerMask.GetMask("Ground Grass");
            rockGroundMask = LayerMask.GetMask("Ground Rock");
            waterGroundMask = LayerMask.GetMask("Ground Water");
            sandGroundMask = LayerMask.GetMask("Ground Sand");

            wallMask = LayerMask.GetMask("wall");
            breakableWallMask = LayerMask.GetMask("breakable wall");

            anim = GetComponentInChildren<Animator>();
            controllerRigidbody = GetComponent<Rigidbody2D>();

            flippedScale = puppet.localScale;

            leftChecker = Instantiate(new GameObject("leftChecker"), transform).transform;
            rightChecker = Instantiate(new GameObject("rightChecker"), transform).transform;
            groundChecker = Instantiate(new GameObject("groundCheck"), transform).transform;

            leftChecker.localPosition = new Vector2(leftCheckOffset, 0.5f);
            rightChecker.localPosition = new Vector2(rightCheckOffset, 0.5f);
            groundChecker.localPosition = new Vector2(0, groundCheckOffset);

            data = Init();
        }

        void FixedUpdate()
        {
            //状态检测
            UpdateInput();
            UpdateGrounding();
            UpdateDirection();
            UpdateGravityScale();
            UpdateWallCheck();

            //计时器
            UpdateTimers(Time.deltaTime);

            //执行输入操作
            UpdateJump();
            UpdateVelocity();

            //其他状态更新
            UpdateState();

            //播放动画
            UpdateAnim();
        }


        /// <summary>
        /// 更新移动操作
        /// </summary>
        void UpdateVelocity()
        {
            if (state == ControlState.awake)
            {
                Vector2 movementInput;
                movementInput.x = episode.Horizontal;
                movementInput.y = 0;

                Vector2 velocity = controllerRigidbody.velocity;

                float tempScale = groundType != GroundType.Air ? data.airMoveScale : 1;
                velocity += movementInput * data.acceleration * tempScale * Time.fixedDeltaTime;

                velocity.x = Mathf.Clamp(velocity.x, -data.maxSpeed, data.maxSpeed);

                controllerRigidbody.velocity = velocity;
            }
        }

        /// <summary>
        /// 更新跳跃操作
        /// </summary>
        void UpdateJump()
        {
            if (state == ControlState.awake)
            {
                if (episode.Jump && groundType != GroundType.Air)
                {
                    controllerRigidbody.AddForce(new Vector2(0, data.jumpForce), ForceMode2D.Impulse);
                    isJumping = true;
                    OnJump();
                }
                else if (isJumping && groundType != GroundType.Air)
                {
                    isJumping = false;
                }

                if (episode.Jump && groundType == GroundType.Air && data.pressJumpMaxTime > jumpPressTime)
                {
                    controllerRigidbody.AddForce(new Vector2(0, data.pressJumpFoece), ForceMode2D.Impulse);
                }
                else if (!episode.Jump && groundType != GroundType.Air)
                {
                    jumpPressTime = 0;
                }
            }
        }


        /*
         * ==================================自动更新部分=================================
         * 以下部分为自动检测和更新
         */

        /// <summary>
        /// 更新定时器
        /// </summary>
        protected virtual void UpdateTimers(float _deltaTime)
        {
            if (jumpPressTime < data.pressJumpMaxTime)
            {
                jumpPressTime += _deltaTime;
            }
        }



        /// <summary>
        /// 更新行走站立状态
        /// </summary>
        void UpdateGrounding()
        {
            if (data.jumpForce == 0)
            {
                return;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker.position, AIConst.k_GroundedRadius);
            foreach (Collider2D c in colliders)
            {
                int layerFlag = 1 << c.gameObject.layer;
                if ((layerFlag & grassGroundMask) != 0)
                {
                    if (groundType == GroundType.Air) OnLand();
                    groundType = GroundType.Grass;
                    return;
                }
                else if ((layerFlag & rockGroundMask) != 0)
                {
                    if (groundType == GroundType.Air) OnLand();
                    groundType = GroundType.Rock;
                    return;
                }
                else if ((layerFlag & waterGroundMask) != 0)
                {
                    if (groundType == GroundType.Air) OnLand();
                    groundType = GroundType.Water;
                    return;
                }
                else if ((layerFlag & sandGroundMask) != 0)
                {
                    if (groundType == GroundType.Air) OnLand();
                    groundType = GroundType.Sand;
                    return;
                }
            }
            groundType = GroundType.Air;
        }

        /// <summary>
        /// 更新角色方向信息
        /// </summary>
        void UpdateDirection()
        {
            if (forceFace == Facing.none)
            {
                if (episode.Horizontal > data.minFlipSpeed && AutoFace != Facing.right)
                {
                    AutoFace = Facing.right;
                    puppet.localScale = flippedScale;
                }
                else if (episode.Horizontal < -data.minFlipSpeed && AutoFace != Facing.left)
                {
                    AutoFace = Facing.left;
                    puppet.localScale = new Vector2(-flippedScale.x, flippedScale.y);
                }
            }
            else
            {
                if (forceFace == Facing.right && AutoFace != Facing.right)
                {
                    AutoFace = Facing.right;
                    puppet.localScale = flippedScale;
                }
                else if(forceFace == Facing.left && AutoFace != Facing.left)
                {
                    AutoFace = Facing.left;
                    puppet.localScale = new Vector2(-flippedScale.x, flippedScale.y);
                }
            }
        }

        /// <summary>
        /// 更新重力信息
        /// </summary>
        void UpdateGravityScale()
        {
            if (updateGravity)
            {
                var gravityScale = data.groundedGravityScale;

                if (groundType == GroundType.Air)
                {
                    gravityScale = controllerRigidbody.velocity.y > 0.0f ? data.jumpGravityScale : data.fallGravityScale;
                }

                controllerRigidbody.gravityScale = gravityScale;
            }
        }

        /// <summary>
        /// 更新触碰到墙体信息
        /// </summary>
        void UpdateWallCheck()
        {
            Collider2D[] leftcolliders = Physics2D.OverlapCircleAll(leftChecker.position, AIConst.k_WallRadius);
            Collider2D[] rightcolliders = Physics2D.OverlapCircleAll(rightChecker.position, AIConst.k_WallRadius);
            foreach (Collider2D c in leftcolliders)
            {
                if (IsWall(1 << c.gameObject.layer))
                {
                    if (attachWall == Facing.none) OnHitWall();
                    attachWall = Facing.left;
                    return;
                }
            }
            foreach (Collider2D c in rightcolliders)
            {
                if (IsWall(1 << c.gameObject.layer))
                {
                    if (attachWall == Facing.none) OnHitWall();
                    attachWall = Facing.right;
                    return;
                }
            }
            attachWall = Facing.none;
        }

        bool IsWall(int layerFlag)
        {
            return ((layerFlag & wallMask) != 0) || ((layerFlag & breakableWallMask) != 0) || ((layerFlag & rockGroundMask) != 0);
        }

        /// <summary>
        /// 给原来的速度加上另外一个速度，一半用于平台
        /// </summary>
        /// <param name="bonus">额外速度</param>
        public void BonusVelocity(Vector2 bonus)
        {
            controllerRigidbody.velocity += bonus;
        }
    }
}