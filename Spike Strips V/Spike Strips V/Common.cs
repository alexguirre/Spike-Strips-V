namespace Spike_Strips_V
{
    // System
    using System;
    using System.Drawing;
    using System.Diagnostics;

    // RPH
    using Rage;
    using Rage.Native;

    internal static class Common
    {
#if DEBUG
        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            NativeFunction.CallByName<uint>("DRAW_LINE", from.X, from.Y, from.Z, to.X, to.Y, to.Z, (int)color.R, (int)color.G, (int)color.B, (int)color.A);
        }
#endif

        public static Vector3 GetClosestPointOnLineSegment(Vector3 linePointStart, Vector3 linePointEnd, Vector3 testPoint)
        {
            Vector3 lineDiffVect = linePointEnd - linePointStart;
            float lineSegSqrLength = lineDiffVect.LengthSquared();

            Vector3 lineToPointVect = testPoint - linePointStart;
            float dotProduct = Vector3.Dot(lineDiffVect, lineToPointVect);

            float percAlongLine = dotProduct / lineSegSqrLength;

            if (percAlongLine < 0.0f)
            {
                return linePointStart;
            }
            else if (percAlongLine > 1.0f)
            {
                return linePointEnd;
            }

            return (linePointStart + (percAlongLine * (linePointEnd - linePointStart)));
        }

        public static string GetFileVersion(string filePath)
        {
            try
            {
                var versInfo = FileVersionInfo.GetVersionInfo(filePath);
                string myVers = String.Format("{0}.{1}.{2}.{3}", versInfo.FileMajorPart, versInfo.FileMinorPart, versInfo.FileBuildPart, versInfo.FilePrivatePart);
                return myVers;
            }
            catch (System.Exception e)
            {
                Logger.LogTrivial("Exception handled: Error Loading Version of " + filePath);
                Logger.LogException(e);
                return "Error Loading Version!";
            }
        }

        public static void GetModelDimensions(Model model, out Vector3 minimun, out Vector3 maximun)
        {
            unsafe
            {
                Vector3 min, max;
                NativeFunction.CallByName<uint>("GET_MODEL_DIMENSIONS", model.Hash, &min, &max);
                minimun = min;
                maximun = max;
            }
        }

        public static void SetVehicleTyreBurst(Vehicle vehicle, EWheel wheel, bool onRim, float damage)
        {
            NativeFunction.CallByName<uint>("SET_VEHICLE_TYRE_BURST", vehicle, (int)wheel, onRim, damage);
        }

        public static bool IsVehicleTyreBurst(Vehicle vehicle, EWheel wheel)
        {
            return NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", vehicle, (int)wheel, false);
        }

        public static void PlayEntityAnim(Entity entity, AnimationDictionary animDict, string animName, bool loop, AnimationFlags flags = AnimationFlags.None)
        {
            animDict.LoadAndWait();
            NativeFunction.CallByName<uint>("PLAY_ENTITY_ANIM", entity, animName, animDict.Name, 1000f, loop, 1, 0, 0f, (uint)flags);
        }

        public static bool IsEntityPlayingAnim(Entity entity, AnimationDictionary animDict, string animName)
        {
            animDict.LoadAndWait();
            return NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", entity, animDict.Name, animName, 3);
        }

        public static float GetEntityAnimCurrentTime(Entity entity, AnimationDictionary animDict, string animName)
        {
            animDict.LoadAndWait();
            return NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", entity, animDict.Name, animName);
        }

        public static void SetEntityAnimCurrentTime(Entity entity, AnimationDictionary animDict, string animName, float value)
        {
            animDict.LoadAndWait();
            NativeFunction.CallByName<uint>("SET_ENTITY_ANIM_CURRENT_TIME", entity, animDict.Name, animName, value);
        }

        public static void SetEntityAnimSpeed(Entity entity, AnimationDictionary animDict, string animName, float speedMultiplier)
        {
            animDict.LoadAndWait();
            NativeFunction.CallByName<uint>("SET_ENTITY_ANIM_SPEED", entity, animDict.Name, animName, speedMultiplier);
        }

        public static void StopEntityAnim(Entity entity, AnimationDictionary animDict, string animName)
        {
            animDict.LoadAndWait();
            NativeFunction.CallByName<uint>("STOP_ENTITY_ANIM", entity, animName, animDict.Name, -1000f);
        }
    }
}
