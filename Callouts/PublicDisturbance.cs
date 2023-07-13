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

namespace JMCalloutsRemastered
{

    [CalloutInterface("Public Disturbance", CalloutProbability.Medium, "A individual causing a scene in public", "Code 2", "LSPD")]

    public class PublicDisturbance : Callout
    {

        // General Variables //
        private Ped suspect;
        private Blip SuspectBlip;
        private Vector3 spawnPoint;
        private int counter;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 50f);
            AddMinimumDistanceCheck(80f, spawnPoint);
            CalloutMessage = "Public Disturbance";
            CalloutPosition = spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspect = new Ped(spawnPoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizens report of an individual causing a scene at the Lucky Plucker. Citizen claims to be under the influence of narcotics. Proceed with caution.");

            suspect.Tasks.Wander();

            SuspectBlip = suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Black;
            SuspectBlip.IsRouteEnabled = true;

            if (suspect.IsMale)
                malefemale = "sir";
            else
                malefemale = "ma'am";


            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (Game.LocalPlayer.Character.DistanceTo(suspect.Position) <= 10f)
            {
                // Perform actions when player is near the suspect
                // e.g., prompt for ID, perform an arrest, etc.

                // For example:
                Game.DisplayHelp("Approach the suspect and talk to them.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Can you tell me what's going on here?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Nothing, you stupid pigs. Mind your own goddamn business.");
                    }
                    if(counter == 3)
                    {
                        Game.DisplayNotification("Attempt to calm the suspect down.");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I need you to calm down please. I'm here to help you.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Fuck you, motherfucker. Time to give you an ass whooping!");
                        suspect.Tasks.FightAgainst(suspect);
                    }
                }
            }
            if (suspect.IsCuffed || suspect.IsDead || Game.LocalPlayer.Character.IsDead || !suspect.Exists())
            {
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (suspect.Exists())
            {
                suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }


            Game.LogTrivial("JM Callouts Remastered - Public Disturbance is Code 4!");
        }
    }
}
