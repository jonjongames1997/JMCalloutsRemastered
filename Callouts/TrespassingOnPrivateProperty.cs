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

    [CalloutInterface("Trespassing On Private Property", CalloutProbability.Medium, "An individual spotted on private property", "Code 2", "LSPD")]

    public class TrespassingOnPrivateProperty : Callout
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
            Spawnpoint = new Vector3(131.11f, -1301.31f, 29.23f); // Vanilla Unicorn //
            heading = 25.62f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2000f);
            AddMaximumDistanceCheck(100f, Spawnpoint);
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A homeowner reported an individual trespassing on their property. Issue them a citation or arrest them and charge them with criminal mischief. Your Choice.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Black;
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

                Game.DisplayHelp("Press ~r~E~ ~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", Can I talk to you for a moment?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want now pigs?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player: ~w~Can you tell me what's going on here? We gotten a call about you being on this property when you are trespassed.");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Trying to get a girl on my lap cause I have been working hard all day and I deserve to have fun. Is that a problem?");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Yes, the owner of this business doesn't want you here. I need you to leave the property cause the owner is requesting a restraining order against you.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~That mothafucka!");
                    }
                    if(counter == 7)
                    {
                        Game.DisplayNotification("You noticed the suspect is getting hostile.");
                        Game.DisplaySubtitle("Player: Leave now, " + malefemale + "! Refusing to leave the property will have you in handcuffs. You will be charged with criminal mischief and disobeying a lawful order.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Fine! I will have my revenge.");
                    }
                    if(counter == 9)
                    {
                        Game.DisplayNotification("Conversation has ended.");
                        Suspect.Tasks.Wander();
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


            Game.LogTrivial("JM Callouts Remastered - Trespasing on Private Property is Code 4!");
        }

    }
}
