using System;
using System.Collections.Generic;
using System.Linq;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using CalloutInterfaceAPI;
using System.Windows.Forms;

namespace JMCalloutsRemastered.Callouts
{
    [CalloutInterface("Robbery of a Pedestrian", CalloutProbability.High, "Citizens reporting a robbery of a ped", "Code 3", "LSPD")]

    public class RobberyOfPedestrianCallout : Callout
    {
        private Ped Suspect;
        private Ped victim;
        private Vector3 spawnPoint;
        private Blip suspectBlip;
        private Blip victimBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            // Set the callout details
            CalloutMessage = "Robbery of a Pedestrian";
            CalloutPosition = Game.LocalPlayer.Character.Position;

            // Generate a random spawn point
            spawnPoint = World.GetNextPositionOnStreet(CalloutPosition.Around(30f));

            // Create the suspect ped
            Suspect = new Ped(spawnPoint);

            // Create the victim ped
            victim = new Ped(World.GetNextPositionOnStreet(Suspect.Position.Around(15f)));

            // Create the suspect blip
            suspectBlip = Suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.Red;

            // Create the victim blip
            victimBlip = victim.AttachBlip();
            victimBlip.Color = System.Drawing.Color.Yellow;

            // Set the callout as displayed
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            // Displays notification to the responding officer // 
            Game.DisplayNotification("Suspect may be armed. Request an additional officer if needed. Approach with caution.");

            // Assign a task to the suspect
            Suspect.Tasks.FightAgainstClosestHatedTarget(30f);


            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            // Check if the suspect has been apprehended
            if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
            {
                End();
            }

            base.Process();
        }

        public override void End()
        {
            // Clean up any remaining entities
            if (suspectBlip.Exists())
                suspectBlip.Delete();

            if (victimBlip.Exists())
                victimBlip.Delete();

            if (Suspect.Exists())
                Suspect.Dismiss();

            if (victim.Exists())
                victim.Dismiss();

            base.End();
        }
    }
}
