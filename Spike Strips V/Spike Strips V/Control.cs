namespace Spike_Strips_V
{
    // System
    using System.Windows.Forms;

    // RPH
    using Rage;
    using Rage.Native;

    internal enum Control
    {
        Deploy,
        Remove,
        Increase,
        Decrease,
    }

    internal static class ControlExtensions
    {
        public static bool IsUsingController => !NativeFunction.CallByHash<bool>(0xa571d46727e2b718, 2);

        public static bool IsJustPressed(this Control control)
        {
            if (NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") == 0)
                return false;

            if (Settings.UseController && IsUsingController)
            {
                bool modifierButtonPressed = Settings.ModifierButton == ControllerButtons.None ? true : Game.IsControllerButtonDownRightNow(Settings.ModifierButton);
                ControllerButtons bttn = GetControllerButton(control);

                return modifierButtonPressed && (bttn == ControllerButtons.None ? false : Game.IsControllerButtonDown(bttn));
            }
            else if(Settings.UseKeyboard)
            {
                bool modifierKeyPressed = Settings.ModifierKey == Keys.None ? true : Game.IsKeyDownRightNow(Settings.ModifierKey);
                Keys key = GetKey(control);

                return modifierKeyPressed && (key == Keys.None ? false : Game.IsKeyDown(key));
            }
            return false;
        }

        private static ControllerButtons GetControllerButton(Control control)
        {
            switch (control)
            {
                case Control.Deploy:
                    return Settings.DeployStingerButton;
                case Control.Remove:
                    return Settings.DeleteStingersButton;
                case Control.Increase:
                    return Settings.IncreaseSizeButton;
                case Control.Decrease:
                    return Settings.DecreaseSizeButton;
                default:
                    return ControllerButtons.None;
            }
        }

        private static Keys GetKey(Control control)
        {
            switch (control)
            {
                case Control.Deploy:
                    return Settings.DeployStingerKey;
                case Control.Remove:
                    return Settings.DeleteStingersKey;
                case Control.Increase:
                    return Settings.IncreaseSizeKey;
                case Control.Decrease:
                    return Settings.DecreaseSizeKey;
                default:
                    return Keys.None;
            }
        }
    }
}
