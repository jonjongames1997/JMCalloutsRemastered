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
        private Ped _Suspect;
        private Blip _SuspectBlip;
        private Vector3 _Spawnpoint;
        private float _heading;
        private string _malefemale;
        private int _counter;


        public override bool OnBeforeCalloutDisplayed()
        {
            _Spawnpoint = new Vector3(7.788585f, -394.9392f, 39.41744f);
            _heading = 218.558f;
            ShowCalloutAreaBlipBeforeAccepting(_Spawnpoint, 1900f);
            AddMinimumDistanceCheck(100f, _Spawnpoint);
            CalloutPosition = _Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            _Suspect = new Ped(_Spawnpoint, _heading);
            _Suspect.IsPersistent = true;
            _Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen reporting a trespasser on construction property.");

            _SuspectBlip = _Suspect.AttachBlip();
            _SuspectBlip.Color = System.Drawing.Color.Beige;
            _SuspectBlip.IsRouteEnabled = true;

            if (_Suspect.IsMale)
                _malefemale = "Sir";
            else
                _malefemale = "Ma'am";

            _counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (Game.LocalPlayer.Character.DistanceTo(_Suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~r~Y~~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                {
                    _counter++;

                    if (_counter == 1)
                    {
                        Game.DisplaySubtitle("Player: Excuse me, " + _malefemale + ". Can you come talk to me for a minute?");
                    }
                    if (_counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What do you want? I'm filming here.");
                    }
                    if (_counter == 3)
                    {
                        Game.DisplaySubtitle("Player: You can't be on constrcution property. It's against the law. Are you an employee of the construction company?");
                    }
                    if (_counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yes. I'm surveying the site to write down important information for the mayor. Is that a problem?");
                    }
                    if (_counter == 5)
                    {
                        Game.DisplaySubtitle("Player: No. Just need to verify. Do you mind if I run your information real quick? It's a procedure I have to follow.");
                    }
                    if (_counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Oh, shit! They know!");
                        _Suspect.Tasks.ReactAndFlee(_Suspect);
                    }
                    if (_counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation Ended!");
                    }
                }
            }

            if (_Suspect.IsCuffed || _Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !_Suspect.Exists())
            {
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (_Suspect.Exists())
            {
                _Suspect.Dismiss();
            }
            if (_SuspectBlip.Exists())
            {
                _SuspectBlip.Delete();
            }


            Game.LogTrivial("JM Callouts Remastered - Trespassing on Construction Property is Code 4!");
        }
    }
}
