using System;

using UnityEngine;

namespace Core {

    public partial class GameStore {

        [Serializable]
        public struct Setting {
            public bool IsSerializedPlayerPrefs;
            public string ChannelPrefix;
            public float ValidAudioDistance;

            [Header("Module ID (If unity module type -> no need to fill)")]
            public string DummyId;
            public string DummyUTKitId;
            public string MainMenuId;
            public string ModeMenuId;
        }

        [Serializable]
        public struct Atlas {
            public string[] Atlases;
        }
    }
}