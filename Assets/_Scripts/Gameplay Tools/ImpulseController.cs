using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace Adv
{
    // public class ImpulseController : PersistentSingleton<ImpulseController>
    // {
    //     CinemachineImpulseSource cIS;

    //     private Coroutine WaitImpulseEnd;
    //     private WaitForSeconds waitForImpulseEnd;

    //     protected override void Awake()
    //     {
    //         base.Awake();
    //         cIS = GetComponentInChildren<CinemachineImpulseSource>();
    //         waitForImpulseEnd = new WaitForSeconds(0.15f);
    //     }

    //     /// <summary>
    //     /// 重复调用这个API将只产生一个震动，直到震动结束才会产生下一个
    //     /// 会大量出现的对象可以调用：敌人
    //     /// </summary>
    //     public void ProduceImpulseAndDontRepeat(Vector3 position, float m_AmplitudeGain, float m_FrequencyGain)
    //     {
    //         if (WaitImpulseEnd != null) return;
    //         WaitImpulseEnd = StartCoroutine(WaitImpulseEndCorotine(() => ProduceImpulse(position, m_AmplitudeGain, m_FrequencyGain)));
    //     }

    //     IEnumerator WaitImpulseEndCorotine(Action producInpulse)
    //     {
    //         producInpulse?.Invoke();
    //         yield return waitForImpulseEnd;
    //         WaitImpulseEnd = null;
    //     }


    //     public void ProduceImpulse(Vector3 position)
    //     {
    //         cIS.GenerateImpulseAt(position, Vector3.down);
    //     }

    //     public void ProduceImpulse(Vector3 position, Vector3 velocity)
    //     {
    //         cIS.GenerateImpulseAt(position, velocity);
    //     }

    //     public void ProduceImpulse(Vector3 position, float m_AmplitudeGain)
    //     {
    //         cIS.m_ImpulseDefinition.m_AmplitudeGain = m_AmplitudeGain;
    //         cIS.GenerateImpulseAt(position, Vector3.down);
    //     }

    //     public void ProduceImpulse(Vector3 position, Vector3 velocity, float m_AmplitudeGain)
    //     {
    //         cIS.m_ImpulseDefinition.m_AmplitudeGain = m_AmplitudeGain;
    //         cIS.GenerateImpulseAt(position, velocity);
    //     }

    //     public void ProduceImpulse(Vector3 position, float m_AmplitudeGain, float m_FrequencyGain)
    //     {
    //         cIS.m_ImpulseDefinition.m_AmplitudeGain = m_AmplitudeGain;
    //         cIS.m_ImpulseDefinition.m_FrequencyGain = m_FrequencyGain;
    //         cIS.GenerateImpulseAt(position, Vector3.down);
    //     }

    //     public void ProduceImpulse(Vector3 position, Vector3 velocity, float m_AmplitudeGain, float m_FrequencyGain)
    //     {
    //         cIS.m_ImpulseDefinition.m_AmplitudeGain = m_AmplitudeGain;
    //         cIS.m_ImpulseDefinition.m_FrequencyGain = m_FrequencyGain;
    //         cIS.GenerateImpulseAt(position, velocity);
    //     }

    //     private void OnDestroy()
    //     {
    //         cIS = null;
    //     }
    // }
}