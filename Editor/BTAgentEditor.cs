using UnityEditor;
namespace DapanzAI
{
    [CustomEditor(typeof(BTAgent)), CanEditMultipleObjects]
    public class BTAgentEditor : Editor
    {
        /*
        JointAngularLimitHandle m_Handle = new JointAngularLimitHandle();

        // 默认情况下，OnSceneGUI回调使用Scene View相机绘制图柄
        protected virtual void OnSceneGUI()
        {
            var agent = (BTAgent)target;

            // 将目标对象的数据复制到Handle
            m_Handle.xMin = agent.xMin;
            m_Handle.xMax = agent.xMax;

            // CharacterJoint和ConfigurableJoint对称地实现y和z轴
            m_Handle.yMin = -agent.yMax;
            m_Handle.yMax = agent.yMax;

            m_Handle.zMin = -agent.zMax;
            m_Handle.zMax = agent.zMax;

            // 设置手柄矩阵以均匀地匹配对象的位置/旋转
            Matrix4x4 handleMatrix = Matrix4x4.TRS(
                agent.transform.position,
                agent.transform.rotation,
                Vector3.one
            );

            EditorGUI.BeginChangeCheck();

            using (new Handles.DrawingScope(handleMatrix))
            {
                // 根据手柄矩阵的原点，为手柄半径保持恒定的屏幕空间大小
                m_Handle.radius = HandleUtility.GetHandleSize(Vector3.zero);

                // draw the handle
                EditorGUI.BeginChangeCheck();
                m_Handle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    // 在设置新值之前记录目标对象，以便可以撤消/重做更改
                    Undo.RecordObject(agent, "Change Joint Example Properties");

                    // 将Handle的更新数据复制回目标对象
                    agent.xMin = m_Handle.xMin;
                    agent.xMax = m_Handle.xMax;

                    agent.yMax = m_Handle.yMax == agent.yMax ? -m_Handle.yMin : m_Handle.yMax;

                    agent.zMax = m_Handle.zMax == agent.zMax ? -m_Handle.zMin : m_Handle.zMax;
                }
            }
        }
                */
    }
}

/*
 变量
angleHandleDrawFunction	要在显示角度控制手柄时使用的 CapFunction。
angleHandleSizeFunction	用于指定角度控制手柄应该多大的 SizeFunction。
fillAlpha	返回或指定在渲染每个轴的运动范围的填充形状时要使用的不透明度。默认为 0.1。
radius	返回或指定手柄圆弧的半径。默认为 1.0。
wireframeAlpha	返回或指定用于沿运动弧线外侧的曲线的不透明度。默认为 1.0。
xHandleColor	返回或指定用于限制围绕 x 轴的运动的手柄的颜色。默认为 Handles.xAxisColor。
xMax	返回或指定关于 x 轴的角运动上限。
xMin	返回或指定关于 x 轴的角运动下限。
xMotion	返回或指定关于 x 轴的角运动的限制方式。默认为 ConfigurableJointMotion.Limited。
xRange	返回或指定关于 x 轴的角运动的有效值范围。默认为 [-180.0, 180.0]。
yHandleColor	返回或指定用于限制围绕 y 轴的运动的手柄的颜色。默认为 Handles.yAxisColor。
yMax	返回或指定关于 y 轴的角运动上限。
yMin	返回或指定关于 y 轴的角运动下限。
yMotion	返回或指定关于 y 轴的角运动的限制方式。默认为 ConfigurableJointMotion.Limited。
yRange	返回或指定关于 y 轴的角运动的有效值范围。默认为 [-180.0, 180.0]。
zHandleColor	返回或指定用于限制围绕 z 轴的运动的手柄的颜色。默认为 Handles.zAxisColor。
zMax	返回或指定关于 z 轴的角运动上限。
zMin	返回或指定关于 z 轴的角运动下限。
zMotion	返回或指定关于 z 轴的角运动的限制方式。默认为 ConfigurableJointMotion.Limited。
zRange	返回或指定关于 z 轴的角运动的有效值范围。默认为 [-180.0, 180.0]。
构造函数
JointAngularLimitHandle	创建 JointAngularLimitHandle 类的新实例。
公共函数
DrawHandle	使用实例的当前配置在当前手柄摄像机中显示此实例的函数。*/