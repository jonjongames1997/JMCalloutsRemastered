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


    [CalloutInterface("Lost Individual", CalloutProbability.Medium, "An individual lost needs assistance", "Code 2", "LSPD")]


    public class LostIndividual : Callout
    {


        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnpoint;
        private float heading;
        private int Counter;
        private string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(-673.41f, -227.52f, 37.09f); // Near Vangelico //
            heading = 91.05f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMinimumDistanceCheck(100f, Spawnpoint);
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("IG_AMANDATOWNLEY", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizens report of a lost individual. Description: Female, possibly in her late 40's or 50's, Wife of a Deranged male. Needs assistance.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Purple;
            SuspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            Counter = 0;

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
                    Counter++;

                    if(Counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + malefemale + ". Are you ok? Do you need any assistance?");
                    }
                    if(Counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~Yes, I do Officer. My husband Michael, dropped me off to go shopping then I hit my head on something, then next thing I know is I don't know where I am.");
                    }
                    if(Counter == 3)
                    {
                        Game.DisplaySubtitle("Player: Are you sure you don't need medical attention? It sounds like you have Amnesia.");
                    }
                    if(Counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~Yes. I'm pretty sure I can sleep this off.");
                    }
                    if(Counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Ok, I'll call you an Uber to take you home.");
                    }
                    if(Counter == 6)
                    {
                        Game.DisplayNotification("Call an Uber to give the suspect a ride home, Officer.");
                    }
                    if(Counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation has ended!");
                        Suspect.Tasks.StandStill(300);
                    }
                }

                if (Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
                {
                    End();
                }
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


            Game.LogTrivial("JM Callouts Remastered - Lost Individual is Code 4!");
        }

    }
}
