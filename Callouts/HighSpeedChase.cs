using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using CalloutInterfaceAPI;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using System.Windows.Forms;
using Functions = LSPD_First_Response.Mod.API.Functions;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("High Speed Chase", CalloutProbability.High, "Car going at excessive speeds", "Code 3", "SAHP")]

    public class HighSpeedChase : Callout
    {

        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vehicle SuspectVehicle;
        private LHandle Pursuit;
        private Vector3 Spawnpoint;
        private bool isPursuitCreated;


        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = World.GetRandomPositionOnStreet();
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 30f);
            AddMaximumDistanceCheck(900f, Spawnpoint);
            CalloutMessage = "High speed chase in progress";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            SuspectVehicle = new Vehicle("BULLET", Spawnpoint);
            SuspectVehicle.IsPersistent = true;

            Suspect = new Ped(SuspectVehicle.GetOffsetPositionFront(5f));
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            Suspect.WarpIntoVehicle(SuspectVehicle, -1);

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.BlueViolet;
            SuspectBlip.IsRouteEnabled = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizens report a vehicle going at excesssive speeds. Respond Code 3");

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();


            if(!isPursuitCreated && Game.LocalPlayer.Character.DistanceTo(SuspectVehicle) <= 50f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                isPursuitCreated = true;
            }

            if(isPursuitCreated && !Functions.IsPursuitStillRunning(Pursuit))
            {
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }
            if (SuspectVehicle.Exists())
            {
                SuspectVehicle.Dismiss();
            }

            Game.LogTrivial("JM Callouts Remastered - High Speed Chase is Code 4!");
        }
    }
}
