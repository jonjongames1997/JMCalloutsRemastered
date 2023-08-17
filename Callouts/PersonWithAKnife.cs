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

    [CalloutInterface("Person With A Knife", CalloutProbability.High, "An individual carrying a deadly weapon", "Code 3", "LSPD")]

    public class PersonWithAKnife : Callout
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
            Spawnpoint = new Vector3(-668.56f, -234.05f, 36.91f); // Spawns the ped at a specific location //
            heading = 205.89f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMaximumDistanceCheck(50f, Spawnpoint);
            CalloutMessage = "A citizen reporting an individual carrying a deadly weapon.";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }


        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("CSB_ANITA", Spawnpoint, heading);
            Suspect.Tasks.FightAgainstClosestHatedTarget(10f);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reporting an individual carrying a deadly weapon. Approach with caution. Respond Code 3");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Brown;
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
                        Game.DisplaySubtitle("Player: Freeze, " + malefemale + ". Let me see your hands!");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: What do you want now, pigs?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: Drop the weapon! I don't want to shoot you.");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Fuck you! I can carry a knife how I like anytime! I'm not harming anyone!");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: I don't give a fuck! DROP THE WEAPON NOW! OR WE'LL FORCE TO SHOOT YOU GRAVEYARD DEAD!");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Bring it on, Cupcake!");
                    }
                    if(counter >= 7)
                    {
                        Game.DisplaySubtitle("No further speech.");
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

            Game.LogTrivial("JM Callouts Remastered - Person With A Knife is Code 4!");

        }

    }
}
