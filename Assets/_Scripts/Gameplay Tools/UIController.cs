using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class UIController : PersistentSingleton<UIController>
    {
        public Button StartGame;
        public Button PlayAgain;
        [SerializeField] Button PMQH屏幕切换;
        [SerializeField] Canvas KS开始UI;
        [SerializeField] Canvas JZZ加载中UI;
        [SerializeField] Canvas JS结算UI;
        [SerializeField] TextMeshProUGUI ZS帧数显示;
        [SerializeField] TextMeshProUGUI LJ连击数;
        [SerializeField] TextMeshProUGUI XL血量;
        [SerializeField] TextMeshProUGUI NL能量;
        [SerializeField] FloatEventChannel On玩家连击Event;
        [SerializeField] float updateTimeval;//更新帧数的时间间隔
        [SerializeField] PlayerProperty playerProperty;

        private int frame;//帧数

        private float timer = 0;//计时器

        protected override void Awake()
        {
            base.Awake();
            PMQH屏幕切换.onClick.AddListener(屏幕切换);
            StartGame.onClick.AddListener(DisableStartGameButton);
            PlayAgain.onClick.AddListener(DisableJS结算UI);
            On玩家连击Event.AddListener(Update连击数);
        }

        private void 屏幕切换()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        private void Update连击数(float 连击数)
        {
            LJ连击数.text = string.Concat("Combo ", ((int)连击数).ToString());
            if (连击数 > 40)
            {
                LJ连击数.text += "\r\n攻击力、攻速、移速提升!";
            }
        }

        private void Update()
        {
            XL血量.text = string.Concat("血量：", playerProperty.HP.ToString());
            NL能量.text = string.Concat("能量：", playerProperty.DQNL当前能量.ToString());
            if (timer >= updateTimeval)
            {
                ZS帧数显示.text = ((int)(Time.timeScale / Time.deltaTime)).ToString();

                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            PMQH屏幕切换.onClick.RemoveListener(屏幕切换);
            StartGame.onClick.RemoveAllListeners();
            PlayAgain.onClick.RemoveAllListeners();
            On玩家连击Event.RemoveListenner(Update连击数);
        }

        public void DisableStartGameButton()
        {
            //暂时
            KS开始UI.enabled = false;
            //StartGame.gameObject.SetActive(false);
        }

        public void EnableJS结算UI()
        {
            JS结算UI.enabled = true;
        }

        public void DisableJS结算UI()
        {
            JS结算UI.enabled = false;
        }

        public void EnableJZZ加载中UI()
        {
            JZZ加载中UI.enabled = true;
        }

        public void DisableJZZ加载中UI()
        {
            JZZ加载中UI.enabled = false;
        }
    }
}
