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

    [CalloutInterface("Intoxicated Individual - Grove Street", CalloutProbability.Medium, "Code 2", "LSPD")]

    public class IntoxicatedIndividual_Location2 : Callout
    {


        // General Variables //
        private Ped Suspect;
        private Blip SuspectBlip;
        private Vector3 SpawnPoint;
        private float heading;
        private int counter;
        private string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = new Vector3();
            heading = 135.69f;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 2500f);
            AddMinimumDistanceCheck(500f, SpawnPoint);
            CalloutPosition = SpawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("A_F_Y_HIPSTER_03", SpawnPoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A well known gang reported an individual who is under the influence of alcohol.");

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.Chartreuse;
            SuspectBlip.IsRouteEnabled = true;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();


        }

        public override void End()
        {
            base.End();


        }
    }
}
