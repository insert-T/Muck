using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MuckHack.Features.Visuals;
using Mucks;
using UnityEngine;
using static MuckHack.Features.Visuals.RenderESP;

namespace Mucks.Loader
{
    [BepInPlugin("ins.insert.insightware", "Insightware", "1.1.0")]
    public class BepLoader : BaseUnityPlugin
    {
        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("INSERT.CC");
        public static GameObject gameObject;
        public static Harmony harmony;

        public void Awake()
        {
            Logger.LogInfo("Plugin Awake method called");

            try
            {
                harmony = new Harmony("ins.insert.insightware");
                harmony.PatchAll();
                Logger.LogInfo("Harmony patches applied");
                Logger.LogInfo("====RESULT====");

                // Создаем GameObject с компонентом MAIN
                gameObject = new GameObject("Class1", typeof(MAIN));
                DontDestroyOnLoad(gameObject);

                if (gameObject != null)
                {
                    Logger.LogInfo("GameObject created");

                    RenderESP.Toggle4();

                    Logger.LogInfo("instance created");
                }
                else
                {
                    Logger.LogFatal("Failed to create GameObject");
                }

                Logger.LogInfo("Initialized!");
            }
            catch (Exception ex)
            {
                Logger.LogFatal("Error during Awake: " + ex.Message);
                Logger.LogFatal(ex.StackTrace);
            }
        }
    }
}
