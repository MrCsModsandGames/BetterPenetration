﻿using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;
using KKAPI.Studio;
using KKAPI.Studio.UI;
using KKAPI.Chara;
using UniRx;
using System;
using System.Linq;
using System.Reflection;
using Core_BetterPenetration;

namespace KK_Studio_BetterPenetration
{
    [BepInPlugin(GUID, PluginName, VERSION)]
    [BepInDependency("com.deathweasel.bepinex.uncensorselector", "3.11.1")]
    [BepInDependency("com.rclcircuit.bepinex.modboneimplantor", "1.0")]
    [BepInProcess("CharaStudio")]
    public class KK_Studio_BetterPenetration : BaseUnityPlugin
    {
        internal const string GUID = "com.animal42069.kkstudiobetterpenetration";
        internal const string PluginName = "KK Studio Better Penetration";
        internal const string VERSION = "1.0.0.0";
        internal const string BEHAVIOR = "BetterPenetrationController";
        internal const string StudioCategoryName = "Better Penetration";
        internal Harmony harmony;

        internal void Main()
        {
            CharacterApi.RegisterExtraBehaviour<BetterPenetrationController>(BEHAVIOR);

            harmony = new Harmony("KK_Studio_BetterPenetration");
            harmony.PatchAll(typeof(KK_Studio_BetterPenetration));

            Chainloader.PluginInfos.TryGetValue("com.deathweasel.bepinex.uncensorselector", out PluginInfo pluginInfo);
            if (pluginInfo == null || pluginInfo.Instance == null)
                return;

            Type nestedType = pluginInfo.Instance.GetType().GetNestedType("UncensorSelectorController", AccessTools.all);
            if (nestedType == null)
                return;

            MethodInfo methodInfo = AccessTools.Method(nestedType, "ReloadCharacterPenis", null, null);
            if (methodInfo == null)
                return;

            harmony.Patch(methodInfo, prefix: new HarmonyMethod(typeof(KK_Studio_BetterPenetration), "BeforeDanCharacterReload"),
                                      postfix: new HarmonyMethod(typeof(KK_Studio_BetterPenetration), "AfterDanCharacterReload"));
            Debug.Log("Studio_BetterPenetration: patched UncensorSelectorController::ReloadCharacterPenis correctly");

            methodInfo = AccessTools.Method(nestedType, "ReloadCharacterBalls", null, null);
            if (methodInfo == null)
                return;

            harmony.Patch(methodInfo, prefix: new HarmonyMethod(typeof(KK_Studio_BetterPenetration), "BeforeTamaCharacterReload"),
                                      postfix: new HarmonyMethod(typeof(KK_Studio_BetterPenetration), "AfterTamaCharacterReload"));
            Debug.Log("Studio_BetterPenetration: patched UncensorSelectorController::ReloadCharacterBalls correctly");

            Chainloader.PluginInfos.TryGetValue("com.joan6694.illusionplugins.nodesconstraints", out pluginInfo);
            if (pluginInfo == null || pluginInfo.Instance == null)
                return;

            Type pluginType = pluginInfo.Instance.GetType();
            if (pluginType == null)
                return;

            methodInfo = AccessTools.Method(pluginType, "AddConstraint", null, null);
            if (methodInfo == null)
                return;

            harmony.Patch(methodInfo, postfix: new HarmonyMethod(typeof(KK_Studio_BetterPenetration), "AfterAddConstraint"));
            Debug.Log("Studio_BetterPenetration: patched NodeConstraints::AddConstraint correctly");
        }

        public static void RegisterStudioControls()
        {
            if (!StudioAPI.InsideStudio)
                return;

            var bpEnable = new CurrentStateCategorySwitch("Enable BP Controller", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().enabled);
            bpEnable.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.enabled = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(bpEnable);

            var lengthSlider = new CurrentStateCategorySlider("Length Squish", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanLengthSquish, 0f, 1f);
            lengthSlider.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanLengthSquish = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(lengthSlider);

            var girthSlider = new CurrentStateCategorySlider("Girth Squish", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanGirthSquish, 0f, 1f);
            girthSlider.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanGirthSquish = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(girthSlider);

            var thresholdSlider = new CurrentStateCategorySlider("Squish Threshold", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanSquishThreshold, 0f, 1f);
            thresholdSlider.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanSquishThreshold = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(thresholdSlider);

            var colliderRadius = new CurrentStateCategorySlider("Collilder Radius", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanColliderRadius, 0f, 0.1f);
            colliderRadius.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanColliderRadius = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(colliderRadius);

            var colliderLength = new CurrentStateCategorySlider("Collilder Length", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanColliderLength, 0f, 0.1f);
            colliderLength.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanColliderLength = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(colliderLength);

            var colliderVertical = new CurrentStateCategorySlider("Collilder Vertical", c => StudioAPI.GetSelectedControllers<BetterPenetrationController>().First().DanColliderVertical, -0.01f, 0.01f);
            colliderVertical.Value.Subscribe(value =>
            {
                foreach (var controller in StudioAPI.GetSelectedControllers<BetterPenetrationController>())
                    controller.DanColliderVertical = value;
            });
            StudioAPI.GetOrCreateCurrentStateCategory(StudioCategoryName).AddControl(colliderVertical);
        }

        internal static void BeforeDanCharacterReload(object __instance)
        {
            ChaControl chaControl = (ChaControl)__instance.GetPrivateProperty("ChaControl");
            if (chaControl == null)
                return;

            var controller = chaControl.GetComponent<BetterPenetrationController>();
            if (controller != null)
                controller.ClearDanAgent();
        }

        internal static void AfterDanCharacterReload(object __instance)
        {
            ChaControl chaControl = (ChaControl)__instance.GetPrivateProperty("ChaControl");
            if (chaControl == null)
                return;

            var controller = chaControl.GetComponent<BetterPenetrationController>();
            if (controller != null)
                controller.InitializeDanAgent();
        }

        internal static void BeforeTamaCharacterReload(object __instance)
        {
            ChaControl chaControl = (ChaControl)__instance.GetPrivateProperty("ChaControl");
            if (chaControl == null)
                return;

            var controller = chaControl.GetComponent<BetterPenetrationController>();
            if (controller != null)
                controller.ClearTama();
        }

        internal static void AfterTamaCharacterReload(object __instance)
        {
            ChaControl chaControl = (ChaControl)__instance.GetPrivateProperty("ChaControl");
            if (chaControl == null)
                return;

            var controller = chaControl.GetComponent<BetterPenetrationController>();
            if (controller != null)
                controller.InitializeTama();
        }

        internal static void AfterAddConstraint(Transform parentTransform, Transform childTransform)
        {
            if (childTransform.name != BoneNames.BPDanEntryTarget)
                return;

            var controller = childTransform.GetComponentInParent<BetterPenetrationController>();
            if (controller == null)
                return;

            if (parentTransform.name != BoneNames.BPKokanTarget)
            {
                controller.RemoveCollisionAgent();
                return;
            }

            var targetChaControl = parentTransform.GetComponentInParent<ChaControl>();
            if (targetChaControl == null)
                return;

            controller.SetCollisionAgent(targetChaControl);
        }
    }
}