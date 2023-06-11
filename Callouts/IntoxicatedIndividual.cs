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
    [CalloutInterface("Intoxicated Individual", CalloutProbability.Medium, "An individual causing a scene possibly drunk or high", "Code 2", "LSPD")]


    public class IntoxicatedIndividual : Callout
    {

        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 Spawnnpoint;
        private float heading;
        private int counter;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnnpoint = new Vector3(94.63f, -217.37f, 54.49f);
            heading = 53.08f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnnpoint, 900f);
            AddMinimumDistanceCheck(900f, Spawnnpoint);
            CalloutPosition = Spawnnpoint;
           

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
        
        CalloutInterfaceAPI.Functions.SendMessage(this, "A business owner reported an individual being drunk on business property. Suspect refused to leave property. Owner said that suspect is possibly be drunk or under the influence of narcotics. Approach with caustion.");
        
            Suspect = new Ped(Spawnnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.CadetBlue;
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

                Game.DisplayHelp("Press ~y~Y~~ to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Good Afternoon " + malefemale + ", How are you today?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: I'm fine, Officer. What's the problem?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("Player: We've gotten reports from this business behind you that you were intoxicated. Did you have anything to drink today?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: I'm not **hiccup* drunk. I'm fine.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("Player: Let me give you a sobriety test to make sure you're not under the influence of alcohol or drugs.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: I DO NOT CONSENT TO THIS TYPE OF INTERROGATION!");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation has ended!");
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


            Game.LogTrivial("JM Callouts Remastered BETA - Intoxicated Individual is Code 4!");
        }
    }
}
