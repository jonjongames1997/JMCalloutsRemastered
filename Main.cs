using Rage;
using LSPD_First_Response.Mod.API;
using JMCalloutsRemastered.Callouts;
using System.Reflection;
using JMCalloutsRemastered.VersionChecker;

namespace JMCalloutsRemastered
{
    public class Main : Plugin
    {
        public override void Finally() { }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
            Settings.LoadSettings();
        }
        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                    Game.Console.Print();
                    Game.Console.Print("=============================================== JM Callouts Remastered by OfficerMorrison ================================================");
                    Game.Console.Print();
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Game.Console.Print("[LOG]: Checking for a new JM Callouts Remastered version...");
                    Game.Console.Print();
                    Game.Console.Print("=============================================== JM Callouts Remastered by OfficerMorrison ================================================");
                    Game.Console.Print();

                    
                    Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "JM Callouts Remastered", "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~o~by OfficerMorrison", "~b~successfully loaded!");

                    VersionChecker.VersionChecker.isUpdateAvailable();
                    GameFiber.Wait(300);
                });
        }
        private static void RegisterCallouts()
        {
            Game.Console.Print();
            Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
            Game.Console.Print();
            if (Settings._911HangUp) { Functions.RegisterCallout(typeof(_911HangUp)); }
            if(Settings.CodeKaren) { Functions.RegisterCallout(typeof(CodeKaren)); }
            if(Settings.IllegalCampfireOnPublicBeach) { Functions.RegisterCallout(typeof(IllegalCampfireOnPublicBeach)); }
            if(Settings.IllegalProstitution) { Functions.RegisterCallout(typeof(IllegalProstitution)); }
            if(Settings.IntoxicatedIndividual) { Functions.RegisterCallout(typeof(IntoxicatedIndividual)); }
            if(Settings.PersonWithAKnife) { Functions.RegisterCallout(typeof(PersonWithAKnife)); }
            if(Settings.PossibleProstitution) { Functions.RegisterCallout(typeof(PossibleProstitution)); }
            if(Settings.PublicDisturbance) { Functions.RegisterCallout(typeof(PublicDisturbance)); }
            if(Settings.RefuseToLeave) { Functions.RegisterCallout(typeof(RefuseToLeave)); }
            if(Settings.RefuseToPay) { Functions.RegisterCallout(typeof(RefuseToPay)); }
            if(Settings.Soliciting) { Functions.RegisterCallout(typeof(Soliciting)); }
            if(Settings.TrespassingOnPrivateProperty) { Functions.RegisterCallout(typeof(TrespassingOnPrivateProperty)); }
            if(Settings.TrespassingOnRailRoadProperty) { Functions.RegisterCallout(typeof(TrespassingOnRailRoadProperty)); }
            Game.Console.Print("[LOG]: All callouts of the JMCalloutsRemastered.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
            Game.Console.Print();
        }
    }
}