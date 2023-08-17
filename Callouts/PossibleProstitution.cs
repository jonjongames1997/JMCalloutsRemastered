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

    [CalloutInterface("Possible Prostitution", CalloutProbability.High, "A female possibly selling her body for money", "Code 2", "LCSO")]

    public class PossibleProstitution : Callout
    {

        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private int counter;
        private float heading;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(481.524f, -1689.046f, 29.240f);
            heading = 223.525f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMaximumDistanceCheck(100f, Spawnpoint);
            CalloutMessage = "Citizens reporting a young female possibly selling her body for money.";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("S_F_Y_HOOKER_02", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reported a young female selling her body for money. Talk to her and see if the claim is true. Approach with caution.");
            Game.DisplayNotification("Tip: This callout works better at night time when other prostitutes are on the streets.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Cyan;
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

            if(Game.LocalPlayer.Character.DistanceTo(Suspect) <= 0f)
            {

                Game.DisplayHelp("Press 'Y' to interact with suspect.");

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Good evening " + malefemale + ", Can I ask you some questions?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Sure. What seems to be the problem, Officer?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: I have gotten reports of you possibly selling your body for money. Is it true?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes. I need the money to pay off my college debt.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: You know that's illegal in the state of San Andreas. Which I can arrest you for that.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What you gonna do? I'm not going anywhere!");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", You can get a job anywhere here in the city. We can help you get a job through a vocational school.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplayNotification("Chief: What the fuck is going on out there, Deputy?!");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Come on, Officer. First time is free.");
                    }
                    if(counter == 10)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", You're under arrest for prostitution.");
                    }
                    if(counter == 11)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Fuck you then, pigs.");
                    }
                    if(counter == 12)
                    {
                        Game.DisplaySubtitle("Conversation ended.");
                        Suspect.Tasks.ReactAndFlee(Suspect);
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


            Game.LogTrivial("JM Callouts Remastered BETA - Possible Prostitution is Code 4!");
        }
    }
}
