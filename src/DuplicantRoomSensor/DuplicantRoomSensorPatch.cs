﻿using Database;
using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;

namespace DuplicantRoomSensor
{


    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public static class DupRoomSensorStringsPatch
    {
        public static void Prefix()
        {
            string upperCaseID = DuplicantRoomSensorConfig.ID.ToUpperInvariant();
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + upperCaseID + ".NAME",
                UI.FormatAsLink(DuplicantRoomSensorConfig.NAME, DuplicantRoomSensorConfig.ID)
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + upperCaseID + ".DESC",
                   DuplicantRoomSensorConfig.DESC
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS." + upperCaseID + ".EFFECT",
                   DuplicantRoomSensorConfig.EFFECT
            });
            ModUtil.AddBuildingToPlanScreen("Automation", DuplicantRoomSensorConfig.ID);

        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class DupRoomSensorTechPatch
    {
        public static void Prefix()
        {
            String techName = "LogicControl";
            List<string> list = new List<string>(Techs.TECH_GROUPING[techName]);
            list.Add(DuplicantRoomSensorConfig.ID);
            Techs.TECH_GROUPING[techName] = list.ToArray();
        }
    }
}
