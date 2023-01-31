using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

namespace Adv
{
    /// <summary>
    /// 游戏整体流程控制器，进入关卡流程：开始关卡-加载中界面开启-生成地图-等待怪物生成完成-设置相机跟随对象-启动输入检测-加载中界面关闭-等待BOSS被击杀-结算界面开启
    /// </summary>
    public class GameController : PersistentSingleton<GameController>
    {
        [SerializeField] LDtkMapGenerator MapGenerator;
        [SerializeField] UIController UIController;
        [SerializeField] CinemachineVirtualCamera VirtualCamera;
        [SerializeField] PlayerProperty playerProperty;
        [SerializeField] VoidEventChannel OnBOSS被击杀Event;

        protected override void Awake()
        {
            base.Awake();
            UIController.StartGame.onClick.AddListener(StartGame);
            UIController.PlayAgain.onClick.AddListener(StartGame);
            OnBOSS被击杀Event.AddListener(QKCJ清空场景);
        }

        private void OnDestroy()
        {
            OnBOSS被击杀Event.RemoveListenner(QKCJ清空场景);
        }

        /// <summary>
        /// 开始游戏 或 再来一次
        /// </summary>
        IEnumerator StartGameCoroutine()
        {
            //开始加载
            UIController.EnableJZZ加载中UI();
            //生成地图
            MapGenerator.DTMS地图模式 = LDtkMapGenerator.DTMS地图模式s.GD固定模式;
            MapGenerator.ReGenerateMap();
            //等待怪物全部加载完
            yield return new WaitForSeconds(2.5f);
            //玩家回满血
            playerProperty.FullHP();
            playerProperty.DQLJS当前连击数 = 0;
            //设置相机跟随
            VirtualCamera.Follow = PlayerFSM.Player.transform;//TODO暂时直接跟随玩家 后面通过相机控制器进行设置
            //启动输入控制
            PlayerFSM.Player.input.EnableGameplayInput();
            //结束加载
            UIController.DisableJZZ加载中UI();
        }

        public void QKCJ清空场景()
        {
            MapGenerator.QKCJ清空场景();
            UIController.EnableJS结算UI();
        }

        public void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }
    }
}
