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

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Illegal Campfire On Public Beach", CalloutProbability.High, "An individual started a campfire on Vespucci Beach.", "Code 3", "LSPD")]

    public class IllegalCampfireOnPublicBeach : Callout
    {

        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private float heading;
        private int counter;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(-1539.16f, -1215.71f, 1.88f); // Campfire Spawns at night //
            heading = 250.07f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f); // Blips the area of the callout //
            AddMaximumDistanceCheck(10f, Spawnpoint); // Checks to see if the player is in range of the callout //
            CalloutMessage = "Individual started an illegal campfire on the beach!"; // Brief description of the call //
            CalloutPosition = Spawnpoint; // Gives the position of where the callout is located at //

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "Vespucci Beach Security reporting an individual starting a campfire on the beach. Suspect refused to put out the fire as requested by security. Suspect also refused to leave the beach. Approach with caution. Suspect may be armed.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Chocolate;
            SuspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "sir";
            else
                malefemale = "ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            {

                Game.DisplayHelp("Press 'Y' to speak with the ~r~suspect.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Good evening" + malefemale + ", May I speak with you for a moment?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Good evening to you as well officer. What seems to be the problem?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: Are you aware of the City Wide Ban on campfires on public beaches?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ No, officer. Is there really a ban on campfires on beaches?");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Yes there is. The city said there is a a heat wave that's in effect until further notice and it's a high risk of causing wild fires.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ RISK OF WILDFI.... That's asinine! This is sand. Are they mentally retarded? They need to go back to science class.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I'm gonna ask you to put out the fire and leave or I'll place you under arrest for failure to comply with a lawful order.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Fuck you, Dick Tickler! I'll do my campfires any time anywhere I want. It's my right as a US Citizen.");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("Conversation has ended!");
                        Game.DisplayNotification("Arrest the suspect, Officer.");
                    }
                    if(counter == 10)
                    {
                        Suspect.Tasks.FightAgainst(Suspect);
                    }
                }
            }

            if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
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


            Game.LogTrivial("JM Callouts Remastered - Illegal Campfire On Public Beach is Code 4!");

        }
    }
}
