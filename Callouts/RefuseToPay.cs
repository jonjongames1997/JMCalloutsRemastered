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
    [CalloutInterface("Refuse To Pay", CalloutProbability.Medium, "An Individual refusing to pay", "Code 2", "LSPD")]

    public class RefuseToPay : Callout
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
            Spawnpoint = new Vector3(-1018.68f, -1356.36f, 5.55f);
            heading = 338.10f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 2500f);
            AddMaximumDistanceCheck(2500f, Spawnpoint);
            CalloutMessage = "Individual Refusing to pay";

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "An Individual is refusing to pay for their food. The individual is being little aggressive. Approach with caution");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.HotPink;
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

                    if (counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", can you come talk to me for a minute?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ About fucking time you showed up, officer!");
                    }
                    if (counter == 3)
                    {
                        Game.DisplayNotification("Tip: Try to calm the suspect down before it gets out of control.");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I need you to calm down and tell me what happened.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I was ordering my food and went to sit down then they served me my food, I found hair in the food. I told them that I wasn't gonna pay for this. The cook or whatever said said that I put my own hair in the food.");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("Player: Go on.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I told them, 'No I didn't this is what was served to me and I'm not paying for this food'. Then they called the cops and now I'm talking to you.");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("Player: Alright, I will review the CCTV footage and see what happened. Can you have a sit for me on the ground until I have more info?");
                    }
                    if (counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yeah, fine. I'm in a hurry to a film production in Rockford Hills.");
                    }
                    if (counter == 10)
                    {
                        Game.DisplayHelp("Go up to the business and review the CCTV footage. Click 'Y' to continue the investigation.");
                    }
                    if (counter == 11)
                    {
                        Game.DisplayNotification("You reviewed the CCTV footage. Suspect DID put hair into his own food. Arrest the suspect and charge him/her for simple theft.");
                    }
                    if (counter == 12)
                    {
                        Game.DisplaySubtitle("Player: Alright " + malefemale + ", I reviewed the footage and your story DIDN'T match. The footage never lies.");
                    }
                    if (counter == 13)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Fuck this shit, I'm outta here!");
                    }
                    if (counter == 14)
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


            Game.LogTrivial("JM Callouts Remastered BETA - Refuse to pay is Code 4!");
        }
    }
}
