﻿namespace Spike_Strips_V
{
    // System
    using System;
    using System.Collections.Generic;

    // RPH
    using Rage;

    internal static class StingersPool
    {
        public const float SeparationFromPlayer = 3.4f;
        public const float SeparationBetweenStingers = 4.825f;

        public static List<Stinger> Stingers = new List<Stinger>();

        public static GameFiber UpdateFiber = new GameFiber(UpdateLoop);

        public static void Initalize()
        {
            UpdateFiber.Start();
        }

        private static bool isCreatingStingersFromPlayer;
        public static bool IsCreatingStingersFromPlayer { get { return isCreatingStingersFromPlayer; } }

        static AnimationTask createStingersPlayerAnimTask;
        public static void CreateStingersFromPlayer(int num)
        {
            isCreatingStingersFromPlayer = true;

            if (Settings.EnableAnimations)
            {
                GameFiber.StartNew(() =>
                {
                    Vector3 playerPos = Game.LocalPlayer.Character.Position;
                    Vector3 playerForwardVect = Game.LocalPlayer.Character.ForwardVector;
                    float playerYaw = Game.LocalPlayer.Character.Rotation.Yaw;
                    Stinger prevStinger = null;
                    createStingersPlayerAnimTask = Game.LocalPlayer.Character.Tasks.PlayAnimation("mp_weapons_deal_sting", "crackhead_bag_loop", -1, 0.925f, 0.825f, 0.0f, AnimationFlags.Loop);
                    for (int i = 0; i < num; i++)
                    {
                        Stinger s = new Stinger(playerPos + playerForwardVect * (SeparationFromPlayer + (SeparationBetweenStingers * i)), playerYaw);
                        if (i != 0)
                            s.Position = new Vector3(s.Position.X, s.Position.Y, prevStinger.Position.Z + 0.5f);
                        Logger.LogDebug("CreateStingers(" + num + ")", "Created Stinger #" + Stingers.Count);
                        Stingers.Add(s);
                        DateTime timeout = DateTime.UtcNow.AddSeconds(15.0f);
                        while (s != null && s.Exists() && s.AnimState != Stinger.StingerAnimState.Deployed && DateTime.UtcNow < timeout)
                            GameFiber.Sleep(25);

                        if (s == null || !s.Exists())
                        {
                            break;
                        }
                        else
                        {
                            prevStinger = s;
                        }
                    }
                    Game.LocalPlayer.Character.Tasks.Clear();
                    createStingersPlayerAnimTask = null;
                    isCreatingStingersFromPlayer = false;
                    Logger.LogDebug("CreateStingers(" + num + ")", "Current Total Stingers " + Stingers.Count);
                });
            }
            else
            {
                Vector3 playerPos = Game.LocalPlayer.Character.Position;
                Vector3 playerForwardVect = Game.LocalPlayer.Character.ForwardVector;
                float playerYaw = Game.LocalPlayer.Character.Rotation.Yaw;
                for (int i = 0; i < num; i++)
                {
                    Stinger s = new Stinger(playerPos + playerForwardVect * (SeparationFromPlayer + (SeparationBetweenStingers * i)), playerYaw);
                    Logger.LogDebug("CreateStingers(" + num + ")", "Created Stinger #" + Stingers.Count);
                    Stingers.Add(s);
                }
                Logger.LogDebug("CreateStingers(" + num + ")", "Current Total Stingers " + Stingers.Count);
                isCreatingStingersFromPlayer = false;
            }
        }

        public static void CreateStingerFromBehindVehicle(Vehicle veh)
        {
            Vector3 finalPos = veh.RearPosition + veh.ForwardVector * -0.725f;
            float yaw = veh.Rotation.Yaw + 90.0f;
            Stinger s = new Stinger(finalPos, yaw);
            Logger.LogDebug("CreateStingerFromBehindVehicle()", "Created Stinger #" + Stingers.Count);
            Stingers.Add(s);
            Logger.LogDebug("CreateStingerFromBehindVehicle()", "Current Total Stingers " + Stingers.Count);
        }

        private static void UpdateLoop()
        {
            while (true)
            {
                GameFiber.Yield();

                Vector3 playerPos = Game.LocalPlayer.Character.Position;
                for (int i = Stingers.Count - 1; i >= 0; i--)
                {
                    Stinger s = Stingers[i];
                    if (s.Exists())
                    {
                        if (Vector3.DistanceSquared(playerPos, s.Position) > 1000f * 1000f)
                        {
                            DeleteStinger(s);
                            continue;
                        }


                        s.Update();
                    }
                    else
                    {
                        Stingers.RemoveAt(i);
                        continue;
                    }
                }
            }
        }


        public static void DeleteAllStingers()
        {
            if (createStingersPlayerAnimTask != null && createStingersPlayerAnimTask.IsPlaying)
            {
                Game.LocalPlayer.Character.Tasks.Clear();
                createStingersPlayerAnimTask = null;
            }

            for (int i = 0; i < Stingers.Count; i++)
            {
                Stingers[i].Delete();
                Logger.LogDebug("DeleteStingers()", "Deleted Stinger #" + i);
            }
            Stingers.Clear();
        }

        public static void DeleteStinger(Stinger stingerToRemove)
        {
            stingerToRemove.Delete();
            Stingers.Remove(stingerToRemove);
        }
    }
}
