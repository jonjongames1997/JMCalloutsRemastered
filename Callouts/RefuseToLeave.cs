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

    [CalloutInterface("Refuse To Leave", CalloutProbability.Medium, "An individual refuses to leave property", "Code 2", "BSCO")]

    public class RefuseToLeave : Callout
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
            Spawnpoint = new Vector3(1198.61f, 2706.30f, 38.22f);
            heading = 182.75f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 1900f);
            AddMaximumDistanceCheck(900f, Spawnpoint);
            CalloutMessage = "Individual refusing to leave property by business owner/employee.";
            CalloutPosition = Spawnpoint;
            CalloutInterfaceAPI.Functions.SendMessage(this, "Route 68 Business employee told the individual to leave the property but refuses to. Employee suspects the individual to be under the nfluence.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("A_F_Y_RURMETH_01", Spawnpoint, heading); // For Testing purposes //
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

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

                Game.DisplayHelp("Press 'E' to interact with suspect. ~y~Approach with caution.");

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", Can I talk to you for a second?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: What now donut pigs?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: Can you tell me what's going on?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: That bitch over there told me I can't come in here.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Is there a reason why she can't let you come in here?");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: I was outside the door asking people for money. She called the cops and they told me that I was trespassed from the property.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplayNotification("Tip: ~o~If the suspect was trespassed from the property before, that's an arrestable offense.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("Player: Ok. Well, you know you can be arrested for trespassing, right?");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect: WHAT?! Are you fucking with me?");
                    }
                    if(counter == 10)
                    {
                        Game.DisplaySubtitle("Player: No, I'm not. Don't try anything stupid, you'll make things worse on yourself.");
                    }
                    if(counter == 11)
                    {
                        Game.DisplaySubtitle("~r~Suspect: Fuck you and fuck her! I'm outta here, playa!");
                    }
                    if(counter == 12)
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


            Game.LogTrivial("JM Callouts Remastered BETA - Refuse to leave is Code 4!");
        }

    }
}
