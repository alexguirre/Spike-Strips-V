namespace Spike_Strips_V
{
    using Rage;
    using Rage.Native;
    using System;
    using System.Drawing;
    using System.Diagnostics;

    internal static class Common
    {
        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            NativeFunction.CallByName<uint>("DRAW_LINE", from.X, from.Y, from.Z, to.X, to.Y, to.Z, (int)color.R, (int)color.G, (int)color.B, (int)color.A);
        }

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

        public static void SetVehicleTyreBurst(Vehicle vehicle, EWheel wheel)
        {
            NativeFunction.CallByName<uint>("SET_VEHICLE_TYRE_BURST", vehicle, (int)wheel, true, 1000.0f);
        }

        public static bool IsVehicleTyreBurst(Vehicle vehicle, EWheel wheel)
        {
            return NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", vehicle, (int)wheel, false);
        }

        //public static float GetGroundHeight(Entity ent)
        //{
        //    Vector3 start = ent.Position;
        //    Vector3 end = start + Vector3.WorldDown * 1000f;

        //    HitResult hr = World.TraceLine(start, end, TraceFlags.IntersectWorld, ent);
        //    return hr.HitPosition.Z;
        //}
        //public static float GetGroundHeight(Vector3 v3, Entity toIgnore)
        //{
        //    Vector3 start = v3;
        //    Vector3 end = start + Vector3.WorldDown * 50f;

        //    HitResult hr = World.TraceLine(start, end, TraceFlags.IntersectWorld, toIgnore);
        //    return hr.HitPosition.Z;
        //}

        //public static void DrawMarker(EMarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color)
        //{
        //    DrawMarker(type, pos, dir, rot, scale, color, false, false, 2, false, null, null, false);
        //}
        //public static void DrawMarker(EMarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, string textueDict, string textureName, bool drawOnEnt)
        //{
        //    dynamic dict = 0;
        //    dynamic name = 0;

        //    if (textueDict != null && textureName != null)
        //    {
        //        if (textueDict.Length > 0 && textureName.Length > 0)
        //        {
        //            dict = textueDict;
        //            name = textureName;
        //        }
        //    }
        //    NativeFunction.CallByName<uint>("DRAW_MARKER", (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, (int)color.R, (int)color.G, (int)color.B, (int)color.A, bobUpAndDown, faceCamY, unk2, rotateY, dict, name, drawOnEnt);
        //}

        //public static void StopAndLogTime(this Stopwatch sw, string title = "")
        //{
        //    sw.Stop();
        //    if (title != "")
        //        Logger.LogTrivial(title);
        //    Logger.LogTrivial("============================");
        //    Logger.LogTrivial("Time: " + sw.Elapsed);
        //    Logger.LogTrivial("Ms: " + sw.ElapsedMilliseconds);
        //    Logger.LogTrivial("Ticks: " + sw.ElapsedTicks);
        //    Logger.LogTrivial("============================");
        //}


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
