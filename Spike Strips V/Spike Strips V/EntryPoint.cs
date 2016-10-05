namespace Spike_Strips_V
{
    // RPH
    using Rage;

    internal static class EntryPoint
    {
        public static int NumberOfStingersToSpawn = 1;

        public static StaticFinalizer Finalizer;

        public static void Main()
        {
            while (Game.IsLoading)
                GameFiber.Yield();

            Logger.LogWelcome();

            Finalizer = new StaticFinalizer(delegate { StingersPool.DeleteAllStingers(); });

            StingersPool.Initalize();

            while (true)
            {
                GameFiber.Yield();

                //** CHANGE SIZE
                if (Control.Increase.IsJustPressed())
                {
                    NumberOfStingersToSpawn = MathHelper.Clamp(NumberOfStingersToSpawn + 1, 1, 7);
                    if (NumberOfStingersToSpawn == 7)
                        NumberOfStingersToSpawn = 1;
                    Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + NumberOfStingersToSpawn.ToString(), 1000);
                }
                else if (Control.Decrease.IsJustPressed())
                {
                    NumberOfStingersToSpawn = MathHelper.Clamp(NumberOfStingersToSpawn - 1, 0, 6);
                    if (NumberOfStingersToSpawn == 0)
                        NumberOfStingersToSpawn = 6;
                    Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + NumberOfStingersToSpawn.ToString(), 1000);
                }

                if (Control.Deploy.IsJustPressed())
                {
                    if (!StingersPool.IsCreatingStingersFromPlayer && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                    {
                        StingersPool.CreateStingersFromPlayer(NumberOfStingersToSpawn);
                    }
                    else if(Settings.AllowDeployFromPoliceCars && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && Game.LocalPlayer.Character.CurrentVehicle.Model.IsCar)
                    {
                        StingersPool.CreateStingerFromBehindVehicle(Game.LocalPlayer.Character.CurrentVehicle);
                    }
                }
                else if (Control.Remove.IsJustPressed())
                {
                    StingersPool.DeleteAllStingers();
                }
            }
        }
    }
}
