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

    [CalloutInterface("Illegal Prostitution", CalloutProbability.High, "A female selling her body for money", "Code 2", "SAHP")]

    public class IllegalProstitution : Callout
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
            Spawnpoint = new Vector3(-49.50f, -214.03f, 45.80f);
            heading = 175.30f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMaximumDistanceCheck(100f, Spawnpoint);
            CalloutMessage = "Citizens reporting a young female selling her body for money.";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("IG_MOLLY", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen is reporting a young female, possibly in her 20s or 30s, selling her body for money. Handle it your way, officer.");
            Game.DisplayNotification("Tip: This callout works best at night time.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.DeepPink;
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

                Game.DisplayHelp("Press 'Y' to interact with suspect.");

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Can you talk to me for a minute?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Sure. What seems to be the problem, Officer?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: We've gotten reports of you selling your body for money. Can you explain why for me please? I want to remind you that you're under oath.");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Well, cutie.....");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Please don't call me 'cutie', " + malefemale + ". I am happily married and I got kids.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Sorry, Officer. I need money for food. You know, with inflation being out of control. I can't afford like a box of Cap'n Crunch which is $4.95 a box. Nobody is hiring and this is my only option.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Player: You know you can apply for Food Stamps and get benefits to help you get food. Selling your body is not the way to go. My job as an officer of the law is to prevent you from doing this. (1/2)");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("Player: Because I'm concerned for your safety. There are people out here that can rape, kidnap, or kill you. You know how many women were victims because of prostituting? (2/2)");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I see where you're coming from.");
                    }
                    if(counter == 10)
                    {
                        Game.DisplayNotification("Chief: Good Job, Officer. You're getting a promotion.");
                    }
                    if (counter == 11)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I'm trying to do my job and prevent you from doing this. I'm giving you the opportunity to walk home or I'll have to place you in cuffs and charge you with prostitution.");
                    }
                    if(counter == 12)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes, Officer. I promise I won't be back here again.");
                    }
                    if(counter == 13)
                    {
                        Game.DisplaySubtitle("Conversation ended. Arrest the suspect.");
                        Suspect.Tasks.Wander(); // What the suspect will do after the conversation ends //
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


            Game.LogTrivial("JM Callouts Remastered - Illegal Prostitution is Code 4!");
        }

    }
}
