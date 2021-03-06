﻿using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Pholib.Utilities;

namespace Notepad
{


    public class OnLoadPatch
    {
        public static string modPath;

        public static void OnLoad(string modPath)
        {
            OnLoadPatch.modPath = modPath;
            ModUtil.RegisterForTranslation(typeof(PHO_STRINGS));
        }
    }


    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public static class DupRoomSensorStringsPatch
    {
        public static void Prefix()
        {
            AddBuilding("Furniture", NotepadConfig.ID, PHO_STRINGS.NOTEPAD.NAME, PHO_STRINGS.NOTEPAD.DESC, PHO_STRINGS.NOTEPAD.EFFECT);
        }
    }


    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class DupRoomSensorTechPatch
    {
        public static void Prefix()
        {
            AddBuildingTech("InteriorDecor", NotepadConfig.ID);

            GameObject o = new GameObject();
            o.AddComponent<NotepadSideScreen>();
        }
    }


    // Load translations files
    [HarmonyPatch(typeof(Localization))]
    [HarmonyPatch("Initialize")]
    class StringLocalisationPatch
    {
        public static void Postfix()
        {
            LoadTranslations(typeof(PHO_STRINGS), OnLoadPatch.modPath);
        }
    }



    [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
    public class DetailsScreen_OnPrefabInit_Patch
    {

        public static void Postfix()
        {
            List<DetailsScreen.SideScreenRef> sideScreens = Traverse.Create(DetailsScreen.Instance).Field("sideScreens").GetValue<List<DetailsScreen.SideScreenRef>>();
            GameObject sideScreenContentBody = Traverse.Create(DetailsScreen.Instance).Field("sideScreenContentBody").GetValue<GameObject>();


            NotepadControl controller = new NotepadControl();
            NotepadSideScreen screen = controller.RootPanel.AddComponent<NotepadSideScreen>();

            screen.Control = controller;
            screen.gameObject.transform.parent = sideScreenContentBody.transform;
            screen.gameObject.transform.localScale = Vector3.one;

            DetailsScreen.SideScreenRef myRef = new DetailsScreen.SideScreenRef
            {
                name = "NotepadSideScreen",
                screenPrefab = screen,
                offset = new Vector2(0f, 0f),
                screenInstance = screen
            };
            sideScreens.Add(myRef);
        }
    }


    [HarmonyPatch(typeof(SelectToolHoverTextCard))]
    [HarmonyPatch("UpdateHoverElements")]
    public class HoverText_ConfigureTitlePatch
    {
        public static void Postfix(SelectToolHoverTextCard __instance, List<KSelectable> hoverObjects)
        {

            foreach (KSelectable selectable in hoverObjects)
            {
                Notepad pad = selectable.gameObject.GetComponent<Notepad>();
                if (pad != null)
                {
                    HoverTextScreen instance = HoverTextScreen.Instance;
                    HoverTextDrawer hover = instance.BeginDrawing();
                    hover.BeginShadowBar();
                    hover.DrawIcon(Assets.GetSprite("icon_category_furniture"), 20);
                    hover.DrawText(pad.activateText, __instance.ToolTitleTextStyle);
                    hover.EndShadowBar();
                    hover.EndDrawing();

                }
            }
        }
    }
}
