﻿namespace Spike_Strips_V
{
    // System
    using System.Windows.Forms;

    // RPH
    using Rage;

    internal static class Settings
    {
        public static readonly InitializationFile INIFile = new InitializationFile(@"Plugins\Spike Strips V Configuration.ini");

        public static readonly bool EnableAnimations = INIFile.ReadBoolean("General", "EnableAnimations", true);

        public static readonly bool AllowDeployFromPoliceCars = INIFile.ReadBoolean("General", "AllowDeployFromPoliceCars", true);

        public static readonly bool UseKeyboard = INIFile.ReadBoolean("Keys", "UseKeyboard", true);

        public static readonly Keys DeployStingerKey = INIFile.ReadEnum<Keys>("Keys", "DeployKey", Keys.K);
        public static readonly Keys IncreaseSizeKey = INIFile.ReadEnum<Keys>("Keys", "IncreaseSizeKey", Keys.Right);
        public static readonly Keys DecreaseSizeKey = INIFile.ReadEnum<Keys>("Keys", "DecreaseSizeKey", Keys.Left);
        public static readonly Keys DeleteStingersKey = INIFile.ReadEnum<Keys>("Keys", "RemoveKey", Keys.O);
        public static readonly Keys ModifierKey = INIFile.ReadEnum<Keys>("Keys", "ModifierKey", Keys.LControlKey);



        public static readonly bool UseController = INIFile.ReadBoolean("ControllerButtons", "UseController", true);

        public static readonly ControllerButtons DeployStingerButton = INIFile.ReadEnum<ControllerButtons>("ControllerButtons", "DeployButton", ControllerButtons.DPadDown);
        public static readonly ControllerButtons DeleteStingersButton = INIFile.ReadEnum<ControllerButtons>("ControllerButtons", "RemoveButton", ControllerButtons.DPadUp);
        public static readonly ControllerButtons IncreaseSizeButton = INIFile.ReadEnum<ControllerButtons>("ControllerButtons", "IncreaseSizeButton", ControllerButtons.DPadRight);
        public static readonly ControllerButtons DecreaseSizeButton = INIFile.ReadEnum<ControllerButtons>("ControllerButtons", "IncreaseSizeButton", ControllerButtons.DPadLeft);
        public static readonly ControllerButtons ModifierButton = INIFile.ReadEnum<ControllerButtons>("ControllerButtons", "ModifierButton", ControllerButtons.LeftShoulder);
    }
}
