using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class ZZXQ追踪小球 : MonoBehaviour
    {
        [SerializeField] float BurstSpeed;
        [SerializeField] float BurstDeceleration;
        [SerializeField] float BurstEndTime = 0.5f;
        [SerializeField] float MoveSpeed;
        [SerializeField] float Acceleration;
        [SerializeField] float Deceleration;
        [SerializeField] float RotateSpeed;
        private bool Can转向 = false;
        private bool ZX转向ing;
        private SDZT速度状态s SDZT速度状态;
        private Transform player;
        private float currentMoveSpeed;
        private Coroutine JJS加减速协程;
        private Tween 转向Tween;
        private WaitForSeconds waitForStart转向;

        private void OnEnable()
        {
            player = PlayerFSM.Player.transform;
            currentMoveSpeed = BurstSpeed;
            Can转向 = false;
            ZX转向ing = false;
            waitForStart转向 = new WaitForSeconds(BurstEndTime);
            StartCoroutine(激活冲出一段距离());
        }

        private void OnDisable()
        {
            Can转向 = false;
            ZX转向ing = false;
            StopAllCoroutines();
            转向Tween.Kill();
        }

        IEnumerator 激活冲出一段距离()
        {
            while (currentMoveSpeed > MoveSpeed / 2)
            {
                currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, MoveSpeed / 2, BurstDeceleration * Time.deltaTime);
                yield return null;
            }


            SDZT速度状态 = SDZT速度状态s.满速;
            yield return waitForStart转向;
            Can转向 = true;
        }

        IEnumerator ZX转向()
        {
            //到玩家的方向
            var direction = player.position - transform.position;
            //和玩家的角度
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//-180~180
            float angleComparison;
            float rotationZ = GetRotationZ();
            //旋转方向
            int rotateDirection;
            //确定自旋转角和angle之间的角度 和 旋转方向
            if (Math.Sign(rotationZ) == Math.Sign(angle))
            {
                angleComparison = Mathf.Abs(rotationZ - angle);
                if (rotationZ - angle > 0)
                    rotateDirection = -1;
                else
                    rotateDirection = 1;
            }
            else
            {
                var angleSum = Mathf.Abs(rotationZ) + Mathf.Abs(angle);
                if (angleSum >= 180)
                {
                    angleSum = 360 - angleSum;
                    if (Math.Sign(angle) > 0)
                        rotateDirection = -1;
                    else
                        rotateDirection = 1;
                }
                else
                {
                    if (Math.Sign(angle) > 0)
                        rotateDirection = 1;
                    else
                        rotateDirection = -1;
                }
                angleComparison = angleSum;
            }

            var rotateSpeed = rotateDirection * RotateSpeed;
            while (angleComparison > 5)
            {
                //到玩家的方向
                direction = player.position - transform.position;
                //和玩家的角度
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//-180~180

                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, rotateSpeed * Time.deltaTime);


                yield return null;
                //自旋转角和angle之间的角度
                rotationZ = GetRotationZ();
                if (Math.Sign(rotationZ) == Math.Sign(angle))
                {
                    angleComparison = Mathf.Abs(rotationZ - angle);
                }
                else
                {
                    var angleSum = Mathf.Abs(rotationZ) + Mathf.Abs(angle);
                    if (angleSum > 180)
                        angleSum = 360 - angleSum;
                    angleComparison = angleSum;
                }

                if (angleComparison > 60)
                {
                    Start加速或减速(false);
                }
                else
                {
                    Start加速或减速(true);
                }
            }
            ZX转向ing = false;
        }

        IEnumerator JJS加减速(bool 加速)
        {
            if (加速)
            {
                SDZT速度状态 = SDZT速度状态s.加速;
                while (currentMoveSpeed < MoveSpeed)
                {
                    currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, MoveSpeed, Acceleration * Time.deltaTime);
                    yield return null;
                }
                SDZT速度状态 = SDZT速度状态s.满速;
            }
            else
            {
                SDZT速度状态 = SDZT速度状态s.减速;
                while (currentMoveSpeed > 0)
                {
                    currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0, Deceleration * Time.deltaTime);
                    yield return null;
                }
                SDZT速度状态 = SDZT速度状态s.零速;
            }

            JJS加减速协程 = null;
        }

        private void Update()
        {
            transform.Translate(currentMoveSpeed * Vector3.right * Time.deltaTime);
            //到玩家的方向
            if (Can转向 && !ZX转向ing)
            {
                var direction = player.position - transform.position;
                //和玩家的角度
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//-180~180
                float angleComparison;
                //自旋转角和angle之间的角度
                if (Math.Sign(GetRotationZ()) == Math.Sign(angle))
                {
                    angleComparison = Mathf.Abs(GetRotationZ() - angle);
                }
                else
                {
                    var angleSum = Mathf.Abs(GetRotationZ()) + Mathf.Abs(angle);
                    if (angleSum > 180)
                        angleSum = 360 - angleSum;
                    angleComparison = angleSum;
                }
                if (angleComparison > 10)
                {
                    ZX转向ing = true;
                    StartZX转向();
                }
            }
        }

        public void StartZX转向()
        {
            StartCoroutine(nameof(ZX转向));
        }

        public void Start加速或减速(bool 加速)
        {
            if (加速 && SDZT速度状态 != SDZT速度状态s.加速 && SDZT速度状态 != SDZT速度状态s.满速 || !加速 && SDZT速度状态 != SDZT速度状态s.减速 && SDZT速度状态 != SDZT速度状态s.零速)
            {
                if (JJS加减速协程 != null)
                    StopCoroutine(JJS加减速协程);
                JJS加减速协程 = StartCoroutine(JJS加减速(加速));
            }
        }

        private float GetRotationZ()
        {
            float z = transform.eulerAngles.z;
            if (z > 180)
            {
                z = z - 360;
            }
            return z;
        }

        private enum SDZT速度状态s
        {
            加速,
            减速,
            零速,
            满速
        }
    }
}
