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

    [CalloutInterface("9-1-1 Hang Up", CalloutProbability.High, "An individual hung up on 911.", "Code 1", "LSPD")]

    public class _911HangUp : Callout
    {

        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private float heading;
        private string malefemale;
        private int counter;


        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(-12.48f, 7.16f, 71.32f); // Spawns the ped at a specific location //
            heading = 336.35f; // heading must match or it will glitch //
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 4500f);
            AddMinimumDistanceCheck(4500f, Spawnpoint);
            CalloutMessage = "A citizen called 911 then hung up on dispatch"; // Brief description of callout //
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A civilian called 9-1-1 then immediately hung up. Deal with this, Officer.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.BlueViolet;
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
                        Game.DisplaySubtitle("Player: Excuse me " + malefemale + ", Can I speak to you for a moment?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Sure, Officer. What seems to be the problem?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: We received a call from your cell phone ping. Did you call 9-1-1?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Oh, shit. I think Siri misheard what I've said. Oh, my lord, I do apologize about thi... Am I getting arrested?");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Ok, let me see some identification from you and we'll go from there. Do you have any warrants that I should know about?");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Sure, here's my ID and no, officer, no warrants. I never been arrested before.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Player: Ok, let me run your information real quick and we'll go from there.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Snitch! I'm gonna give you the ass whooping of your life, Officer.");
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


            Game.LogTrivial("JM Callouts Remastered - 911 Hang Up is Code 4!");
        }

    }
}
