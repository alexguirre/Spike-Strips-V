namespace Spike_Strips_V
{
    // System
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

        public static void CreateStingers(int num)
        {
            GameFiber.StartNew(() =>
            {
                Vector3 playerPos = Game.LocalPlayer.Character.Position;
                Vector3 playerForwardVect = Game.LocalPlayer.Character.ForwardVector;
                float playerYaw = Game.LocalPlayer.Character.Rotation.Yaw;
                Stinger prevStinger = null;
                for (int i = 0; i < num; i++)
                {
                    Stinger s = new Stinger(playerPos + playerForwardVect * (SeparationFromPlayer + (SeparationBetweenStingers * i)), playerYaw);
                    if (i != 0)
                        s.Position = new Vector3(s.Position.X, s.Position.Y, prevStinger.Position.Z + 0.5f);
                    Logger.LogDebug("CreateStingers(" + num + ")", "Created Stinger #" + Stingers.Count);
                    Stingers.Add(s);
                    while (s.AnimState != Stinger.StingerAnimState.Deployed)
                        GameFiber.Yield();
                    prevStinger = s;
                }
            });
            Logger.LogDebug("CreateStingers(" + num + ")", "Current Total Stingers " + Stingers.Count);
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
                for (int i = 0; i < Stingers.Count; i++)
                {
                    if (Stingers[i].Exists())
                    {
                        Stingers[i].Update();
                    }
                    else
                    {
                        Stingers.RemoveAt(i);
                        break;
                    }
                }
            }
        }


        public static void DeleteAllStingers()
        {
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
