using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DapanzAI
{
    /// <summary>
    /// BT执行状态
    /// </summary>
    public enum BTState
    {
        /// <summary>
        /// 失败
        /// </summary>
        Failure,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 继续
        /// </summary>
        Continue,
        /// <summary>
        /// 放弃
        /// </summary>
        Abort
    }

    /// <summary>
    /// 行为树
    /// </summary>
    public static class BT
    {
        public static Root Root() { return new Root(); }
        public static Sequence Sequence() { return new Sequence(); }
        public static Selector Selector(bool shuffle = false) { return new Selector(shuffle); }
        public static Action RunCoroutine(System.Func<IEnumerator<BTState>> coroutine) { return new Action(coroutine); }
        public static Action Call(System.Action fn) { return new Action(fn); }
        public static ConditionalBranch If(System.Func<bool> fn) { return new ConditionalBranch(fn); }
        public static While While(System.Func<bool> fn) { return new While(fn); }
        public static Condition Condition(System.Func<bool> fn) { return new Condition(fn); }
        public static Repeat Repeat(int count) { return new Repeat(count); }
        public static Wait Wait(float seconds) { return new Wait(seconds); }

        public static Trigger Trigger(Animator animator, string name, bool set = true) { return new Trigger(animator, name, set); }
        public static WaitForAnimatorState WaitForAnimatorState(Animator animator, string name, int layer = 0) { return new WaitForAnimatorState(animator, name, layer); }
        public static SetBool SetBool(Animator animator, string name, bool value) { return new SetBool(animator, name, value); }
        
        public static SetActive SetActive(GameObject gameObject, bool active) { return new SetActive(gameObject, active); }
        public static WaitForAnimatorSignal WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0) { return new WaitForAnimatorSignal(animator, name, state, layer); }
        public static Terminate Terminate() { return new Terminate(); }
        public static Log Log(string msg) { return new Log(msg); }
        public static RandomSequence RandomSequence(int[] weights = null) { return new RandomSequence(weights); }

        //新增
        public static LoopCountdown LoopCountdown(float waitTime) { return new LoopCountdown(waitTime); }
    }

    public abstract class BTNode
    {
        public abstract BTState Tick();
    }

    public abstract class Branch : BTNode
    {
        protected int activeChild;
        protected List<BTNode> children = new List<BTNode>();
        public virtual Branch OpenBranch(params BTNode[] children)
        {
            for (var i = 0; i < children.Length; i++)
                this.children.Add(children[i]);
            return this;
        }

        public List<BTNode> Children()
        {
            return children;
        }

        public int ActiveChild()
        {
            return activeChild;
        }

        public virtual void ResetChildren()
        {
            activeChild = 0;
            for (var i = 0; i < children.Count; i++)
            {
                Branch b = children[i] as Branch;
                if (b != null)
                {
                    b.ResetChildren();
                }
            }
        }
    }

    public abstract class Decorator : BTNode
    {
        protected BTNode child;
        public Decorator Do(BTNode child)
        {
            this.child = child;
            return this;
        }
    }

    public class Sequence : Branch
    {
        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    else
                        return BTState.Continue;
                case BTState.Failure:
                    activeChild = 0;
                    return BTState.Failure;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("什么鬼???");
        }
    }

    /// <summary>
    ///执行每个子节点，直到一个子节点成功，然后返回true。
    ///如果没有子成功，则返回false。
    /// </summary>
    public class Selector : Branch
    {
        public Selector(bool shuffle)
        {
            if (shuffle)
            {
                var n = children.Count;
                while (n > 1)
                {
                    n--;
                    var k = Mathf.FloorToInt(Random.value * (n + 1));
                    var value = children[k];
                    children[k] = children[n];
                    children[n] = value;
                }
            }
        }

        public override BTState Tick()
        {
            var childState = children[activeChild].Tick();
            switch (childState)
            {
                case BTState.Success:
                    activeChild = 0;
                    return BTState.Success;
                case BTState.Failure:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Failure;
                    }
                    else
                        return BTState.Continue;
                case BTState.Continue:
                    return BTState.Continue;
                case BTState.Abort:
                    activeChild = 0;
                    return BTState.Abort;
            }
            throw new System.Exception("This should never happen, but clearly it has.");
        }
    }

    /// <summary>
    ///调用方法或协程。
    /// </summary>
    public class Action : BTNode
    {
        System.Action fn;
        System.Func<IEnumerator<BTState>> coroutineFactory;
        IEnumerator<BTState> coroutine;
        public Action(System.Action fn)
        {
            this.fn = fn;
        }
        public Action(System.Func<IEnumerator<BTState>> coroutineFactory)
        {
            this.coroutineFactory = coroutineFactory;
        }
        public override BTState Tick()
        {
            if (fn != null)
            {
                fn();
                return BTState.Success;
            }
            else
            {
                if (coroutine == null)
                    coroutine = coroutineFactory();
                if (!coroutine.MoveNext())
                {
                    coroutine = null;
                    return BTState.Success;
                }
                var result = coroutine.Current;
                if (result == BTState.Continue)
                    return BTState.Continue;
                else
                {
                    coroutine = null;
                    return result;
                }
            }
        }

        public override string ToString()
        {
            return "Action : " + fn.Method.ToString();
        }
    }

    /// <summary>
    ///调用一个方法，如果方法返回true，则返回成功，否则返回失败。
    /// </summary>
    public class Condition : BTNode
    {
        public System.Func<bool> fn;

        public Condition(System.Func<bool> fn)
        {
            this.fn = fn;
        }
        public override BTState Tick()
        {
            return fn() ? BTState.Success : BTState.Failure;
        }

        public override string ToString()
        {
            return "Condition : " + fn.Method.ToString();
        }
    }

    public class ConditionalBranch : Block
    {
        public System.Func<bool> fn;
        bool tested = false;
        public ConditionalBranch(System.Func<bool> fn)
        {
            this.fn = fn;
        }
        public override BTState Tick()
        {
            if (!tested)
            {
                tested = fn();
            }
            if (tested)
            {
                var result = base.Tick();
                if (result == BTState.Continue)
                    return BTState.Continue;
                else
                {
                    tested = false;
                    return result;
                }
            }
            else
            {
                return BTState.Failure;
            }
        }

        public override string ToString()
        {
            return "ConditionalBranch : " + fn.Method.ToString();
        }
    }

    /// <summary>
    ///运行所有子项，而方法返回true。
    /// </summary>
    public class While : Block
    {
        public System.Func<bool> fn;

        public While(System.Func<bool> fn)
        {
            this.fn = fn;
        }

        public override BTState Tick()
        {
            if (fn())
                base.Tick();
            else
            {
                //if we exit the loop
                ResetChildren();
                return BTState.Failure;
            }

            return BTState.Continue;
        }

        public override string ToString()
        {
            return "While : " + fn.Method.ToString();
        }
    }

    public abstract class Block : Branch
    {
        public override BTState Tick()
        {
            switch (children[activeChild].Tick())
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    activeChild++;
                    if (activeChild == children.Count)
                    {
                        activeChild = 0;
                        return BTState.Success;
                    }
                    return BTState.Continue;
            }
        }
    }

    public class Root : Block
    {
        public bool isTerminated = false;

        public override BTState Tick()
        {
            if (isTerminated) return BTState.Abort;
            while (true)
            {
                switch (children[activeChild].Tick())
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    case BTState.Abort:
                        isTerminated = true;
                        return BTState.Abort;
                    default:
                        activeChild++;
                        if (activeChild == children.Count)
                        {
                            activeChild = 0;
                            return BTState.Success;
                        }
                        continue;
                }
            }
        }
    }

    /// <summary>
    /// Run a block of children a number of times.
    /// 多次运行一个子节点块。
    /// </summary>
    public class Repeat : Block
    {
        public int count = 1;
        int currentCount = 0;
        public Repeat(int count)
        {
            this.count = count;
        }
        public override BTState Tick()
        {
            if (count > 0 && currentCount < count)
            {
                var result = base.Tick();
                switch (result)
                {
                    case BTState.Continue:
                        return BTState.Continue;
                    default:
                        currentCount++;
                        if (currentCount == count)
                        {
                            currentCount = 0;
                            return BTState.Success;
                        }
                        return BTState.Continue;
                }
            }
            return BTState.Success;
        }

        public override string ToString()
        {
            return "Repeat Until : " + currentCount + " / " + count;
        }
    }

    public class RandomSequence : Block
    {
        int[] m_Weight = null;
        int[] m_AddedWeight = null;

        /// <summary>
        /// 每当它再次被触发时，都会选择一个随机的子节点
        /// </summary>
        /// <param name="weight">Leave null so that all child node have the same weight. 
        /// If there is less weight than children, all subsequent child will have weight = 1</param>
        public RandomSequence(int[] weight = null)
        {
            activeChild = -1;

            m_Weight = weight;
        }

        public override Branch OpenBranch(params BTNode[] children)
        {
            m_AddedWeight = new int[children.Length];

            for (int i = 0; i < children.Length; ++i)
            {
                int weight = 0;
                int previousWeight = 0;

                if (m_Weight == null || m_Weight.Length <= i)
                {//如果没有权重，则将权重设置为1
                    weight = 1;
                }
                else
                    weight = m_Weight[i];

                if (i > 0)
                    previousWeight = m_AddedWeight[i - 1];

                m_AddedWeight[i] = weight + previousWeight;
            }

            return base.OpenBranch(children);
        }

        public override BTState Tick()
        {
            if (activeChild == -1)
                PickNewChild();

            var result = children[activeChild].Tick();

            switch (result)
            {
                case BTState.Continue:
                    return BTState.Continue;
                default:
                    PickNewChild();
                    return result;
            }
        }

        void PickNewChild()
        {
            int choice = Random.Range(0, m_AddedWeight[m_AddedWeight.Length - 1]);

            for (int i = 0; i < m_AddedWeight.Length; ++i)
            {
                if (choice - m_AddedWeight[i] <= 0)
                {
                    activeChild = i;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return "Random Sequence : " + activeChild + "/" + children.Count;
        }
    }


    /// <summary>
    ///暂停执行N秒钟。
    /// </summary>
    public class Wait : BTNode
    {
        public float seconds = 0;
        float future = -1;
        public Wait(float seconds)
        {
            this.seconds = seconds;
        }

        public override BTState Tick()
        {
            if (future < 0)
                future = Time.time + seconds;

            if (Time.time >= future)
            {
                future = -1;
                return BTState.Success;
            }
            else
                return BTState.Continue;
        }

        public override string ToString()
        {
            return "Wait : " + (future - Time.time) + " / " + seconds;
        }
    }

    /// <summary>
    /// ///在animator上SetTrigger。
    /// </summary>
    public class Trigger : BTNode
    {
        Animator animator;
        int id;
        string triggerName;
        bool set = true;

        //if set == false, it reset the trigger istead of setting it.
        public Trigger(Animator animator, string name, bool set = true)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.triggerName = name;
            this.set = set;
        }

        public override BTState Tick()
        {
            if (set)
                animator.SetTrigger(id);
            else
                animator.ResetTrigger(id);

            return BTState.Success;
        }

        public override string ToString()
        {
            return "Trigger : " + triggerName;
        }
    }

    /// <summary>
    ///在animator上SetBool。
    /// </summary>
    public class SetBool : BTNode
    {
        Animator animator;
        int id;
        bool value;
        string triggerName;

        public SetBool(Animator animator, string name, bool value)
        {
            this.id = Animator.StringToHash(name);
            this.animator = animator;
            this.value = value;
            this.triggerName = name;
        }

        public override BTState Tick()
        {
            animator.SetBool(id, value);
            return BTState.Success;
        }

        public override string ToString()
        {
            return "SetBool : " + triggerName + " = " + value.ToString();
        }
    }

    /// <summary>
    /// 等待animator进入状态。
    /// </summary>
    public class WaitForAnimatorState : BTNode
    {
        Animator animator;
        int id;
        int layer;
        string stateName;

        public WaitForAnimatorState(Animator animator, string name, int layer = 0)
        {
            this.id = Animator.StringToHash(name);
            if (!animator.HasState(layer, this.id))
            {
                Debug.LogError("The animator does not have state: " + name);
            }
            this.animator = animator;
            this.layer = layer;
            this.stateName = name;
        }

        public override BTState Tick()
        {
            var state = animator.GetCurrentAnimatorStateInfo(layer);
            if (state.fullPathHash == this.id || state.shortNameHash == this.id)
                return BTState.Success;
            return BTState.Continue;
        }

        public override string ToString()
        {
            return "Wait For State : " + stateName;
        }
    }

    /// <summary>
    /// 激活GameObject。
    /// </summary>
    public class SetActive : BTNode
    {

        GameObject gameObject;
        bool active;

        public SetActive(GameObject gameObject, bool active)
        {
            this.gameObject = gameObject;
            this.active = active;
        }

        public override BTState Tick()
        {
            gameObject.SetActive(this.active);
            return BTState.Success;
        }

        public override string ToString()
        {
            return "Set Active : " + gameObject.name + " = " + active;
        }
    }

    /// <summary>
    /// Wait for a signal to be received from a SendSignal state machine behaviour on an animator.
    /// 等待动画器上的SendSignal状态机行为收到信号。
    /// </summary>
    public class WaitForAnimatorSignal : BTNode
    {
        internal bool isSet = false;
        string name;
        int id;

        public WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0)
        {
            this.name = name;
            this.id = Animator.StringToHash(name);
            if (!animator.HasState(layer, this.id))
            {
                Debug.LogError("当前Animator没有该状态 >>" + name);
            }
            else
            {
                SendSignal.Register(animator, name, this);
            }
        }

        public override BTState Tick()
        {
            if (!isSet)
                return BTState.Continue;
            else
            {
                isSet = false;
                return BTState.Success;
            }

        }

        public override string ToString()
        {
            return "等待Animation >>" + name;
        }
    }

    public class Terminate : BTNode
    {

        public override BTState Tick()
        {
            return BTState.Abort;
        }

    }

    public class Log : BTNode
    {
        string msg;

        public Log(string msg)
        {
            this.msg = msg;
        }

        public override BTState Tick()
        {
            Debug.Log(msg);
            return BTState.Success;
        }
    }

    /// <summary>
    /// 在循环时间内连续执行
    /// </summary>
    public class LoopCountdown : Block
    {
        public float waitTime = 0;
        float future = 0;
        public LoopCountdown(float waitTime)
        {
            this.waitTime = waitTime;
        }

        public override BTState Tick()
        {
            if (future < waitTime)
            {
                future +=Time.deltaTime;
                base.Tick();
            }
            else
            {
                ResetChildren();
                return BTState.Failure;
            }
            return BTState.Continue;
        }

        public override string ToString()
        {
            return "Loop in time : " + waitTime;
        }
    }
}

#if UNITY_EDITOR
namespace BTAI
{
    public interface IBTDebugable
    {
        Root GetAIRoot();
    }
}
#endif