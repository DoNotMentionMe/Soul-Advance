using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("设置位置，可选择设置的坐标轴和设置方式")]
    public class SetPosition : Action
    {
        public enum Coord
        {
            X,
            Y,
            XY,
        }
        public enum SetMode
        {
            赋值,//赋值
            叠加,//叠加
            同步,//同步
        }
        [SerializeField] Coord coord;
        [SerializeField] SetMode setMode;
        [SerializeField] SharedVector2 SetValue;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("SetMode为Synchronization的时候有效，将自身坐标设置为该参数坐标的值")]
        [SerializeField] SharedTransform SetTransform;

        public override TaskStatus OnUpdate()
        {
            switch (coord)
            {
                case Coord.X:
                    {
                        if (setMode == SetMode.赋值)
                        {
                            SetPosX(SetValue.Value.x);
                        }
                        else if (setMode == SetMode.叠加)
                            transform.position += Vector3.right * SetValue.Value.x;
                        else
                            SetPosX(SetTransform.Value.position.x);
                        return TaskStatus.Success;
                    }
                case Coord.Y:
                    {
                        if (setMode == SetMode.赋值)
                        {
                            SetPosY(SetValue.Value.y);
                        }
                        else if (setMode == SetMode.叠加)
                            transform.position += Vector3.up * SetValue.Value.y;
                        else
                            SetPosY(SetTransform.Value.position.y);
                        return TaskStatus.Success;
                    }
                case Coord.XY:
                    {
                        if (setMode == SetMode.赋值)
                        {
                            SetPosX(SetValue.Value.x);
                            SetPosY(SetValue.Value.y);
                        }
                        else if (setMode == SetMode.叠加)
                        {
                            transform.position += Vector3.right * SetValue.Value.x;
                            transform.position += Vector3.up * SetValue.Value.y;
                        }
                        else
                        {
                            SetPosX(SetTransform.Value.position.x);
                            SetPosY(SetTransform.Value.position.y);
                        }
                        return TaskStatus.Success;
                    }
            }
            return TaskStatus.Failure;
        }

        private void SetPosX(float value)
        {
            var pos = transform.position;
            pos.x = value;
            transform.position = pos;
        }

        private void SetPosY(float value)
        {
            var pos = transform.position;
            pos.y = value;
            transform.position = pos;
        }

    }
}
