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

        private string[] pedList = new string[] { "A_F_M_SouCent_01", "A_F_M_SouCent_02", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01",
                                                  "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01",
                                                  "A_M_M_Skidrow_01", "A_M_Y_MexThug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02",
                                                  "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };

        private Ped Suspect;
        private Vector3 Spawnpoint;
        private Vector3 SearchArea;
        private Blip suspectBlip;
        private LHandle pursuit;
        private int scenario = 0;
        private bool hasBegunAttacking = false;
        private bool isArmed = false;
        private bool hasPursuitBegun = false;
        private bool hasSpoke = false;
        private bool pursuitCreated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            scenario = new Random().Next(0, 100);
            Spawnpoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 1000f);
            AddMinimumDistanceCheck(100f, Spawnpoint);
            CalloutMessage = "Citizen's reporting a person carrying a deadly weapon";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped(pedList[new Random().Next((int)pedList.Length)], Spawnpoint, 0f);
            Suspect.BlockPermanentEvents = true;
            Suspect.IsPersistent = true;
            Suspect.Tasks.Wander();

            SearchArea = Spawnpoint.Around2D(1f, 2f);
            suspectBlip = new Blip(SearchArea, 80f);
            suspectBlip.Color = Color.Yellow;
            suspectBlip.EnableRoute(Color.Yellow);
            suspectBlip.Alpha = 0.5f;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspectBlip) suspectBlip.Delete();
            if (Suspect) Suspect.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {

            GameFiber.StartNew(delegate
            {
                if (Suspect.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !isArmed)
                {
                    Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                    isArmed = true;
                }
                if (Suspect && Suspect.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !hasBegunAttacking)
                {
                    if (scenario > 40)
                    {
                        Suspect.KeepTasks = true;
                        Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        hasBegunAttacking = true;
                        switch (new Random().Next(1, 3))
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect: ~w~I'm gonna kill everyone!", 4000);
                                hasSpoke = true;
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect: ~w~You'll never take me alive, Coppers!", 4000);
                                hasSpoke = true;
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect: ~w~Take your last breath of fresh air, motherfucker!", 4000);
                                hasSpoke = true;
                                break;
                            default: break;
                        }
                        GameFiber.Wait(2000);
                    }
                    else
                    {
                        if (!hasPursuitBegun)
                        {
                            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, Suspect);
                            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                            hasPursuitBegun = true;
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCalloutKey)) End();
                if (Suspect && Suspect.IsDead) End();
                if (Suspect && Suspect.IsCuffed) End();

            }, "Person With A Knife [JM Callouts Remastered]");

            base.Process();
        }

        public override void End()
        {

            if (Suspect) Suspect.Dismiss();
            if (suspectBlip) suspectBlip.Delete();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~JM Callouts Remastered", "~y~Person With a Knife", "~b~You: ~w~Dispatch we're code 4. Show me back ~g~10-8.");
            Game.DisplayNotification("Good Job, Officer! You are getting a promotion.");

            base.End();
        }
    }
}
