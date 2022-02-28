using System.Collections.Generic; 
using UnityEditor;
using UnityEngine;

namespace AmbientUnityCompileTime
{
    internal static class CompileTimePreference
    {
        internal const string AmbientEnable = "ambient_enable";
        internal const string AmbientChannelId = "ambient_channel_id";
        internal const string AmbientWriteKey = "ambient_write_key";
        internal const string AmbientReadKey = "ambient_read_key";

        [SettingsProvider]
        public static SettingsProvider CreateExampleProvider()
        {
            var provider = new SettingsProvider("Preferences/", SettingsScope.User)
            {
                // タイトル
                label = "Compile Time",
                // GUI描画
                guiHandler = searchContext =>
                {
                    var ambientEnable = PlayerPrefs.GetInt(AmbientEnable);
                    var channelId = PlayerPrefs.GetString(AmbientChannelId);
                    var writeKey = PlayerPrefs.GetString(AmbientWriteKey);
                    var readKey = PlayerPrefs.GetString(AmbientReadKey);

                    ambientEnable = EditorGUILayout.Toggle("Use Ambient", ambientEnable > 0) ? 1 : 0;
                    EditorGUI.indentLevel++;
                    channelId = EditorGUILayout.TextField("Channel Id", channelId);
                    writeKey = EditorGUILayout.TextField("Write Key", writeKey);
                    readKey = EditorGUILayout.TextField("Read Key", readKey);
                    EditorGUI.indentLevel--;

                    PlayerPrefs.SetInt(AmbientEnable, ambientEnable);
                    PlayerPrefs.SetString(AmbientChannelId, channelId);
                    PlayerPrefs.SetString(AmbientWriteKey, writeKey);
                    PlayerPrefs.SetString(AmbientReadKey, readKey);
                },
                // 検索時のキーワード
                keywords = new HashSet<string>(new[] { "CompileTime" })
            };

            return provider;
        }
    }
}