using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using CalloutInterfaceAPI;
using System.Windows.Forms;

namespace JMCalloutsRemastered.Callouts
{
    [CalloutInterface("Home Invasion", CalloutProbability.VeryHigh, "Citizens report of a home burgarly", "Code 3", "LSPD")]
    public class HomeInvasionCallout : Callout
    {
        private Ped Suspect;
        private Vector3 spawnPoint;
        private Blip suspectBlip;
        private float heading;
        private int counter;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            // Set the callout details
            

            // Create the suspect ped
            

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            // Assign a task to the suspect
            Suspect.Tasks.FightAgainstClosestHatedTarget(30f);

            
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();





            // Check if the suspect has been apprehended
            if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
            {
                End();
            }
        }

        public override void End()
        {
            // Clean up any remaining entities
            if (suspectBlip.Exists())
                suspectBlip.Delete();

            if (Suspect.Exists())
                Suspect.Dismiss();

            base.End();
        }
    }
}

