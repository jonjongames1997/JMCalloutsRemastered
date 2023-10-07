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

    [CalloutInterface("Soliciting", CalloutProbability.High, "An individual soliciting on private property", "Code 3", "LSPD")]

    public class Soliciting : Callout
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
            Spawnpoint = new Vector3(154.39f, -987.48f, 30.09f); // Legion Square in Mission Row //
            heading = 165.04f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2000f);
            AddMaximumDistanceCheck(100f, Spawnpoint);
            CalloutMessage = "An Individual asking people for money.";

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("IG_LAMARDAVIS", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "An individual is asking people for money and harassing them. Deal with this, Officer.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Indigo;
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
                Game.DisplayHelp("Press ~y~E ~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Can you stop and talk to me please?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Oh, Shit. The one time!");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Conversation ended!");
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


            Game.LogTrivial("JM Callouts Remastered - Soliciting is Code 4!");
        }

    }
}
