﻿using Harmony;
using Pholib;

namespace WorldgenPack
{
    public class WorldAdds
    {
        public static LocString NAME = "Aquaria";
        public static LocString DESCRIPTION = "Test \n\n";

        public static void OnLoad()
        {
            Logs.Log(Utilities.IsOnWorld("Fuleria").ToString());

            Utilities.addWorldYaml(NAME, DESCRIPTION, null, typeof(WorldAdds));
        }

    }
}

