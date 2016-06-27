namespace Spike_Strips_V
{
    using Rage;
    using Rage.Native;
    using System.Linq;
    using System.Drawing;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Collections.Generic;

    internal class Stinger : IUpdatable, IDeletable, IPersistable, ISpatial, IRotatable
    {
        static readonly AnimationDictionary P_ld_stinger_s = "P_ld_stinger_s";
        static readonly string P_Stinger_S_deploy = "P_Stinger_S_deploy", P_Stinger_S_idle_undeployed = "P_Stinger_S_idle_undeployed", P_Stinger_S_idle_deployed = "P_Stinger_S_idle_deployed";
        static Stinger()
        {
            P_ld_stinger_s.LoadAndWait();
        }


        public enum StingerAnimState { Undeployed, Deploy, Deployed }

        public static readonly Model Model = "p_ld_stinger_s";

        public StingerAnimState AnimState { get; set; }

        public Stinger(Vector3 position)
        {
            Prop = new Object(Model, position);
            AnimState = StingerAnimState.Undeployed;
            HandleAnimations();
        }
        public Stinger(Vector3 position, float rotYaw)
        {
            Prop = new Object(Model, position);
            Prop.SetRotationYaw(rotYaw);
            AnimState = StingerAnimState.Undeployed;
            HandleAnimations();
        }

        public Object Prop { get; private set; } 

        public Vector3 Position { get { return Prop.Position; } set { Prop.Position = value; } }
        public Rotator Rotation { get { return Prop.Rotation; } set { Prop.Rotation = value; } }
        public Quaternion Orientation { get { return Prop.Orientation; } set { Prop.Orientation = value; } }
        public Quaternion Quaternion { get { return Prop.Orientation; } set { Prop.Orientation = value; } }
        public bool IsPersistent { get { return Prop.IsPersistent; } set { Prop.IsPersistent = value; } }
        public Vector3 Direction { get { return Prop.Direction; } set { Prop.Direction = value; } }

        List<Vehicle> nearVehicles = new List<Vehicle>();
        public void Update()
        {
            if (Prop.Exists())
            {
                HandleAnimations();

                Vehicle[] vehicles = System.Array.ConvertAll(World.GetEntities(Prop.Position, 12.5f, GetEntitiesFlags.ConsiderAllVehicles), (x => (Vehicle)x));
                nearVehicles.AddRange(vehicles.Where(v => !nearVehicles.Contains(v)));

                for (int i = 0; i < nearVehicles.Count; i++)
                {
                    if (nearVehicles[i].Exists())
                    {
                        Vector3 modelMin, modelMax;
                        Common.GetModelDimensions(nearVehicles[i].Model, out modelMin, out modelMax);

                        Vector3 pointA = Prop.GetOffsetPosition(new Vector3(0.0f, 1.7825f, 0.034225f));
                        Vector3 pointB = Prop.GetOffsetPosition(new Vector3(0.0f, -1.7825f, 0.034225f));

                        BurstTyreMethod(nearVehicles[i], Bone.wheel_lf, EWheel.LeftFront, modelMin, pointA, pointB);
                        BurstTyreMethod(nearVehicles[i], Bone.wheel_lm1, EWheel.LeftMiddle_1, modelMin, pointA, pointB);
                        BurstTyreMethod(nearVehicles[i], Bone.wheel_lr, EWheel.LeftRear, modelMin, pointA, pointB);
                        BurstTyreMethod(nearVehicles[i], Bone.wheel_rf, EWheel.RightFront, modelMin, pointA, pointB);
                        BurstTyreMethod(nearVehicles[i], Bone.wheel_rm1, EWheel.RightMiddle_1, modelMin, pointA, pointB);
                        BurstTyreMethod(nearVehicles[i], Bone.wheel_rr, EWheel.RightRear, modelMin, pointA, pointB);

                        if (Vector3.Distance(Prop.Position, nearVehicles[i].Position) > 10.0f)
                            nearVehicles.RemoveAt(i);
                    
#if DEBUG
                        Common.DrawLine(pointA, pointB, Color.Red);
#endif
                    }
                    else
                    {
                        nearVehicles.RemoveAt(i);
                    }
                }

                if (Vector3.Distance(Game.LocalPlayer.Character.Position, Prop.Position) > 1250f)
                {
                    StingersPool.DeleteStinger(this);
                    return;
                }
            }
        }

        public bool Exists()
        {
            return Prop.Exists();
        }

        public void Delete()
        {
            if (Exists()) Prop.Delete();
        }

        public void Dismiss()
        {
            if (Exists()) Prop.Dismiss();
        }

        public float DistanceTo(Vector3 position)
        {
            return Prop.DistanceTo(position);
        }
        public float DistanceTo(ISpatial spatialObject)
        {
            return Prop.DistanceTo(spatialObject);
        }

        public float DistanceTo2D(Vector3 position)
        {
            return Prop.DistanceTo2D(position);
        }
        public float DistanceTo2D(ISpatial spatialObject)
        {
            return Prop.DistanceTo2D(spatialObject);
        }

        public float TravelDistanceTo(Vector3 position)
        {
            return Prop.TravelDistanceTo(position);
        }
        public float TravelDistanceTo(ISpatial spatialObject)
        {
            return Prop.TravelDistanceTo(spatialObject);
        }

        public void Face(Vector3 position)
        {
            Prop.Face(position);
        }
        public void Face(ISpatial spatialObject)
        {
            Prop.Face(spatialObject);
        }

        private void BurstTyreMethod(Vehicle veh, string wheelBone, EWheel wheel, Vector3 vehModelMinDim, Vector3 pointA, Vector3 pointB)
        {
            if (veh.HasBone(wheelBone) && !Common.IsVehicleTyreBurst(veh, wheel))
            {
                Vector3 wheelPos = veh.GetBonePosition(wheelBone);

                if (veh.IsOnAllWheels) wheelPos = veh.Wheels[(int)wheel].LastContactPoint;
                else wheelPos.Z += (vehModelMinDim.Z / 2); ;

                Vector3 wheelClosestPoint = Common.GetClosestPointOnLineSegment(pointA, pointB, wheelPos);
                float wheelClosestPointDistance = Vector3.Distance(wheelPos, wheelClosestPoint);

                if (wheelClosestPointDistance < 0.3f)
                    Common.SetVehicleTyreBurst(veh, wheel);

               
#if DEBUG
                Common.DrawLine(wheelPos, wheelClosestPoint, Color.Green);
                    new ResText(wheelBone + "~n~" + wheelClosestPointDistance.ToString(), new Point((int)World.ConvertWorldPositionToScreenPosition(wheelPos).X, (int)World.ConvertWorldPositionToScreenPosition(wheelPos).Y), 0.235f, Color.Green).Draw();
#endif
            }
        }

        private void HandleAnimations()
        {
            switch (AnimState)
            {
                case StingerAnimState.Undeployed:
                    if (!Common.IsEntityPlayingAnim(Prop, P_ld_stinger_s, P_Stinger_S_idle_undeployed))
                        Common.PlayEntityAnim(Prop, P_ld_stinger_s, P_Stinger_S_idle_undeployed, false);

                    if (Prop.Speed <= 0.1f && (NativeFunction.CallByName<float>("GET_ENTITY_HEIGHT_ABOVE_GROUND", Prop) < 0.15f))    // start anim after falling
                        AnimState = StingerAnimState.Deploy;
                    break;
                case StingerAnimState.Deploy:
                    if (!Common.IsEntityPlayingAnim(Prop, P_ld_stinger_s, P_Stinger_S_deploy))
                    {
                        Common.PlayEntityAnim(Prop, P_ld_stinger_s, P_Stinger_S_deploy, false);
                        Common.SetEntityAnimSpeed(Prop, P_ld_stinger_s, P_Stinger_S_deploy, 1.75f);
                    }
                    if (Common.GetEntityAnimCurrentTime(Prop, P_ld_stinger_s, P_Stinger_S_deploy) > 0.99f)
                    {
                        
                        AnimState = StingerAnimState.Deployed;
                    }
                    break;
                case StingerAnimState.Deployed:
                    break;
            }
        }
    }
}
