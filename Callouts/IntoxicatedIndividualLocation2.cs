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
    // Description of Callout //

    [CalloutInterface("Intoxicated Individual - Chumash", CalloutProbability.Medium, "An individual under the influence", "Code 2", "SAHP")]

    public class IntoxicatedIndividualLocation2 : Callout
    {

        // General Variables //
        private Ped Suspect; // Defines the ped //
        private Blip SuspectBlip; // Attach a blip to the ped //
        private Vector3 Spawnpoint; // Defines a location of the ped //
        private float heading; 
        private int counter;
        private string malefemale; // Defines whether or not if suspect is male or female //


        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(-3096.49f, 305.3093f, 8.301878f);
            heading = 116.7359f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMaximumDistanceCheck(10f, Spawnpoint);
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("IG_MOLLY", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reporting an individual being under the influence of alcohol. Respond Code 2");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Black;
            SuspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~Y ~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Can you talk to me for a minute?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want, pigs?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: We've gotten reports of you being intoxicated. Did you have anything to drink?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Just a bottle of 'Mind yo fucking business' *hiccup*.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Why did you just hiccuped?");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Just *hiccup* had a *hiccuo* burger a while *iccup* ago.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplayNotification("Do a Field Sobriety Test on the suspect, Officer.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I'm gonna have to give you a Field Sobriety Test just to make sure you are not under the influence of alcohol.");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I DO NOT CONSENT TO THIS TYPE OF INTERROGATION!");
                    }
                    if (counter == 10)
                    {
                        Game.DisplaySubtitle("Conversation has ended!");
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


            Game.LogTrivial("JM Callouts Remastered - Intoxicated Individual - Chumash is Code 4!");
        }

    }
}
