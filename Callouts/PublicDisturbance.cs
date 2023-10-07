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
using Rage.Native;
using JMCalloutsRemastered.Callouts;
using JMCalloutsRemastered.Stuff;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Public Disturbance", CalloutProbability.Medium, "A individual causing a scene in public", "Code 3", "LSPD")]

    public class PublicDisturbance : Callout
    {

        // General Variables //
        private Ped suspect;
        private Blip SuspectBlip;
        private Vector3 spawnPoint;
        private int counter;
        private string malefemale;
        private float heading;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = new Vector3(-174.17f, -1427.77f, 31.25f);
            heading = 178.40f;
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 1000f);
            AddMinimumDistanceCheck(100f, spawnPoint);
            CalloutMessage = "An citizen's reporting a public disturbance.";
            CalloutPosition = spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspect = new Ped("A_M_Y_BREAKDANCE_01", spawnPoint, heading);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen's reporting a disturbance with a white male with no shirt. Threatening the victim's life with a deadly weapon. Respond code 3.");

            SuspectBlip = suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.BurlyWood;
            SuspectBlip.IsRouteEnabled = true;

            if (suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(Game.LocalPlayer.Character.DistanceTo(suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~'E'~w~ to talk to suspect. ~r~Approach with caution.");

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("~b~You (Officier)~w~: Excuse me, " + malefemale + ", Can you come talk to me for a second?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want now, you donut eaters?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("~b~You (Officer):~w~ Can you explain to me on what's to be the problem?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~I have a bipolar disorder which I can't control and it makes me say offensive things.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("~b~You (Officer): ~w~Do you take any type of medication for your disorder?");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~Yes, I have forget about it. I do apologize for y'all to be called out here.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("~b~You (Officer): ~w~Why did threat someone's life for when they didn't do anything to you?");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~I didn't mean anything by it. I do apologize about it.");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~b~You (Officer): ~w~Next time, take your medicine when you are supposed to take it, " + malefemale + ".");
                    }
                    if(counter == 10)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~I understand that loud and clear, Officer.");
                    }
                    if(counter == 11)
                    {
                        Game.DisplaySubtitle("Conversation Ended! Deal with this situation to your satisfaction.");
                        suspect.Tasks.StandStill(500);
                    }
                }
                if (suspect.IsCuffed || suspect.IsDead || Game.LocalPlayer.Character.IsDead || !suspect.Exists())
                {
                    End();
                }
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
