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

    [CalloutInterface("Trespassing On Construction Property", CalloutProbability.High, "A individual trespassing on construction property", "Code 2", "LSPD")]

    public class TrespassingOnConstructionProperty : Callout
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
            Spawnpoint = new Vector3(7.788585f, -394.9392f, 39.41744f);
            heading = 218.558f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 500f);
            AddMinimumDistanceCheck(100f, Spawnpoint);
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reporting a trespasser on construction property.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Beige;
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

            if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~r~E~~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Can you come talk to me for a minute?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want? I'm filming here.");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("Player: You can't be on constrcution property. It's against the law. Are you an employee of the construction company?");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes. I'm surveying the site to write down important information for the mayor. Is that a problem?");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("Player: No. Just need to verify. Do you mind if I run your information real quick? It's a procedure I have to follow.");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Oh, shit! They know!");
                        Suspect.Tasks.ReactAndFlee(Suspect);
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation Ended!");
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


            Game.LogTrivial("JM Callouts Remastered - Trespassing on Construction Property is Code 4!");
        }
    }
}
