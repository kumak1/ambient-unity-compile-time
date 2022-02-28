// Copyright © kumak1. All rights reserved. 
// Originally found here: https://answers.unity.com/questions/1131497/how-to-measure-the-amount-of-time-it-takes-for-uni.html

using UnityEngine;
using UnityEditor;
using AmbientUnityLib;

namespace AmbientUnityCompileTime
{
    [InitializeOnLoad]
    internal class CompileTime : EditorWindow
    {
        private static bool _isTrackingTime;
        private static double _startTime;

        static CompileTime()
        {
            EditorApplication.update += Update;
            _startTime = PlayerPrefs.GetFloat("CompileStartTime", 0);
            if (_startTime > 0)
            {
                _isTrackingTime = true;
            }
        }

        private static void Update()
        {
            if (EditorApplication.isCompiling && !_isTrackingTime)
            {
                _startTime = EditorApplication.timeSinceStartup;
                PlayerPrefs.SetFloat("CompileStartTime", (float) _startTime);
                _isTrackingTime = true;
            }
            else if (!EditorApplication.isCompiling && _isTrackingTime)
            {
                var finishTime = EditorApplication.timeSinceStartup;
                _isTrackingTime = false;
                var compileTime = finishTime - _startTime;
                PlayerPrefs.DeleteKey("CompileStartTime");

                var ambientEnable = PlayerPrefs.GetInt(CompileTimePreference.AmbientEnable);
                var channelId = PlayerPrefs.GetString(CompileTimePreference.AmbientChannelId);
                var writeKey = PlayerPrefs.GetString(CompileTimePreference.AmbientWriteKey);
                var readKey = PlayerPrefs.GetString(CompileTimePreference.AmbientReadKey);

                if (ambientEnable > 0 && !string.IsNullOrEmpty(channelId) && !string.IsNullOrEmpty(writeKey))
                {
                    var ambientClient = new Ambient(channelId, readKey, writeKey);
                    var response = ambientClient.Send(new AmbientSendParameter { D1 = compileTime.ToString("0.000") });
                    var validResponse = string.IsNullOrEmpty(response) ? "true" : "false";
                    
                    Debug.Log($"Script compilation time: \n{compileTime.ToString("0.000")}s\nAmbient Send: {validResponse}");
                }
                else
                {
                    Debug.Log($"Script compilation time: \n{compileTime.ToString("0.000")}s");
                }
            }
        }
    }
}