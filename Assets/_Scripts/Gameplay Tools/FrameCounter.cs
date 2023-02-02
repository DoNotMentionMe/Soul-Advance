using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class FrameCounter : MonoBehaviour
    {
        #region 字段
        float updateInterval = 1.0f;
        private float accum = 0.0f;        // FPS accumulated over the interval
        private float frames = 0;      // Frames drawn over the interval
        private float timeleft; // Left time for current interval
        private float fps = 15.0f;         // Current FPS
        private float lastSample;
        private float gotIntervals = 0;
        #endregion
        #region 属性

        private GUIStyle fontStyle = new GUIStyle();

        #endregion
        #region Unity回调函数

        //private void Awake()
        //{
        //    Application.targetFrameRate = -1;
        //}

        void Start()
        {
            // fontStyle.normal.background = null;    //设置背景填充
            // fontStyle.normal.textColor = Color.white;   //设置字体颜色
            // fontStyle.fontSize = 40;


            timeleft = updateInterval;
            lastSample = Time.realtimeSinceStartup;
        }//Start ()_end
         // void Update()
        void Update()
        {
            ++frames;
            float newSample = Time.realtimeSinceStartup;
            float deltaTime = newSample - lastSample;
            lastSample = newSample;
            timeleft -= deltaTime;
            accum += 1.0f / deltaTime;
            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0f)
            {
                // display two fractional digits (f2 format)
                fps = accum / frames;
                // guiText.text = fps.ToString("f2");
                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
                ++gotIntervals;
            }
            //text.text = "FPS:" + fps.ToString("f2");
        }//Update ()_end
        #endregion
        #region 自建方法
        void OnGUI()
        {
            GUI.Label(new Rect(0, 50, 200, 200), "FPS:" + fps.ToString("f2"));
        }
        #endregion
    }
}
