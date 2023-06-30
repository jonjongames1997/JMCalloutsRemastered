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
            Spawnpoint = new Vector3(-885.71f, 41.33f, 48.76f);
            heading = 64.02f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 1900f);
            AddMaximumDistanceCheck(1900f, Spawnpoint);
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

                Game.DisplayHelp("Press ~r~E~ to talk to Suspect. Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", Can I talk to you for a moment?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~~ What do you want now pigs?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: What are you doing on this property? We gotten a call about you being on this property without permission.");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~~ Trying to ask my ex-boyfriend/girlfriend to pose as model for my photography ad. Is that a problem?");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Yes, the owner of this house doesn't want you here and they said you've been trespassed before. I need you to leave the property cause the owner is requesting a restraining order against you.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~~ That son of a bitch! I'll wait till I see her/him in public.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplayNotification("You noticed the suspect is getting hostile.");
                        Game.DisplaySubtitle("Player: Leave now, " + malefemale + "! Refusing to leave the property will have you in handcuffs. You will be charged with criminal mischief and disobeying a lawful order.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~~ You'll never take me alive, Copper!!");
                    }
                    if(counter == 9)
                    {
                        Game.DisplayNotification("Conversation has ended. Handle the situation your way, Officer.");
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


            Game.LogTrivial("JM Callouts Remastered BETA - Trespasing on Private Property is Code 4!");
        }

    }
}
