namespace Spike_Strips_V
{
    using Rage;

    internal static class EntryPoint
    {
        public static int numSize = 1;
        public static int numStinger = 1;
        public static int numAddStinger = 1;

        public static StaticFinalizer Finalizer;

        public static void Main()
        {
            while (Game.IsLoading)
                GameFiber.Yield();

            Logger.LogWelcome();

            Finalizer = new StaticFinalizer(delegate { StingersPool.DeleteAllStingers(); });

            StingersPool.Initalize();
            Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1500);

            while (true)
            {
                GameFiber.Yield();

                //** CHANGE SIZE
                if (Control.Increase.IsJustPressed())
                {
                    numStinger += numAddStinger;
                    numSize++;
                    if (numSize < 7)
                    {
                        Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                    }
                }
                else if (Control.Decrease.IsJustPressed())
                {
                    numStinger -= numAddStinger;
                    numSize--;
                    if (numSize < 7)
                    {
                        Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                    }
                }

                if (Control.Deploy.IsJustPressed() && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                {
                    StingersPool.CreateStingers(numStinger);
                }
                else if (Control.Remove.IsJustPressed())
                {
                    StingersPool.DeleteAllStingers();
                }


                if (numSize == 7)
                {
                    Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + "1", 1000);
                    //numCaltrops = 16;
                    numStinger = 1;
                    numSize = 1;
                }
                else if (numSize == 0)
                {
                    Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + "6", 1000);
                    //numCaltrops = 106;
                    numStinger = 6;
                    numSize = 6;
                }
            }
        }
    }
}
