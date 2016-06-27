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
                if (Settings.UseKeyboard)
                {
                    if (!Settings.UseModifierKey)
                    {
                        if (Game.IsKeyDown(Settings.IncreaseSizeKey))
                        {
                            numStinger += numAddStinger;
                            numSize++;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }
                        else if (Game.IsKeyDown(Settings.DecreaseSizeKey))
                        {
                            numStinger -= numAddStinger;
                            numSize--;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }

                        if (Game.IsKeyDown(Settings.DeployStingerKey) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                        {
                            StingersPool.CreateStingers(numStinger);
                        }

                        if (Game.IsKeyDownRightNow(Settings.DeleteStingersKey))
                        {
                            StingersPool.DeleteAllStingers();
                        }
                    }
                    else if (Settings.UseModifierKey)
                    {
                        if (Game.IsKeyDownRightNow(Settings.ComboKey) && Game.IsKeyDown(Settings.IncreaseSizeKey))
                        {
                            numStinger += numAddStinger;
                            numSize++;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }
                        else if (Game.IsKeyDownRightNow(Settings.ComboKey) && Game.IsKeyDown(Settings.DecreaseSizeKey))
                        {
                            numStinger -= numAddStinger;
                            numSize--;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }


                        if (Game.IsKeyDownRightNow(Settings.ComboKey) && Game.IsKeyDown(Settings.DeployStingerKey) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                        {
                            StingersPool.CreateStingers(numStinger);
                        }

                        if (Game.IsKeyDownRightNow(Settings.ComboKey) && Game.IsKeyDownRightNow(Settings.DeleteStingersKey))
                        {
                            StingersPool.DeleteAllStingers();
                        }
                    }
                }


                if (Settings.UseController)
                {
                    if (Settings.UseModifierButton)
                    {
                        if (Game.IsControllerButtonDownRightNow(Settings.ComboButton) && Game.IsControllerButtonDown(Settings.IncreaseSizeButton))
                        {
                            numStinger += numAddStinger;
                            numSize++;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }
                        else if (Game.IsControllerButtonDownRightNow(Settings.ComboButton) && Game.IsControllerButtonDown(Settings.DecreaseSizeButton))
                        {
                            numStinger -= numAddStinger;
                            numSize--;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }


                        if (Game.IsControllerButtonDownRightNow(Settings.ComboButton) && Game.IsControllerButtonDown(Settings.DeployStingerButton) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                        {
                            StingersPool.CreateStingers(numStinger);
                        }

                        if (Game.IsControllerButtonDownRightNow(Settings.ComboButton) && Game.IsControllerButtonDownRightNow(Settings.DeleteStingersButton))
                        {
                            StingersPool.DeleteAllStingers();
                        }
                    }
                    else if (!Settings.UseModifierButton)
                    {
                        if (Game.IsControllerButtonDown(Settings.IncreaseSizeButton))
                        {
                            numStinger -= numAddStinger;
                            numSize++;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }
                        else if (Game.IsControllerButtonDown(Settings.DecreaseSizeButton))
                        {
                            numStinger -= numAddStinger;
                            numSize--;
                            if (numSize < 7)
                            {
                                Game.DisplaySubtitle("~r~Spike Strips ~n~~b~Size: " + numSize.ToString(), 1000);
                            }
                        }

                        if (Game.IsControllerButtonDown(Settings.DeployStingerButton) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInCover && !Game.LocalPlayer.Character.IsJumping)
                        {
                            StingersPool.CreateStingers(numStinger);
                        }

                        if (Game.IsControllerButtonDownRightNow(Settings.DeleteStingersButton))
                        {
                            StingersPool.DeleteAllStingers();
                        }
                    }
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
