using Rage;
using LSPD_First_Response.Mod.API;
using JMCalloutsRemastered.Callouts;
using JMCalloutsRemastered.VersionChecker;
using JMCalloutsRemastered;
using System.Reflection;

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
                    Game.Console.Print("=============================================== JMCalloutsRemastered by OfficerMorrison ================================================");
                    Game.Console.Print();
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Game.Console.Print("[LOG]: Checking for a new JMCalloutsRemastered version...");
                    Game.Console.Print();
                    Game.Console.Print("=============================================== JMCalloutsRemastered by OfficerMorrison ================================================");
                    Game.Console.Print();

                    // You can find all textures/images in OpenIV
                    Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "JMCalloutsRemastered", "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~o~by OfficerMorrison", "~b~successfully loaded!");
                    // Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "UnitedCallouts", "~y~Unstable Build", "This is an ~r~unstable build~w~ of UnitedCallouts for testing. You may notice bugs while playing the unstable build.");

                    PluginCheck.isUpdateAvailable();
                    GameFiber.Wait(300);
                });
        }
        private static void RegisterCallouts() //Register all your callouts here
        {
            Game.Console.Print();
            Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
            Game.Console.Print();
            if (Settings.IllegalCampfireOnPublicBeach) { Functions.RegisterCallout(typeof(IllegalCampfireOnPublicBeach)); }
            if(Settings.IntoxicatedIndividual) { Functions.RegisterCallout(typeof(IntoxicatedIndividual)); }
            if(Settings.RefuseToLeave) { Functions.RegisterCallout(typeof(RefuseToLeave)); }
            if(Settings.RefuseToPay) { Functions.RegisterCallout(typeof(RefuseToPay)); }
            if(Settings.PossibleProstitution) { Functions.RegisterCallout(typeof(PossibleProstitution)); }
            if(Settings.Soliciting) { Functions.RegisterCallout(typeof(Soliciting)); }
            if(Settings.TrespassingOnPrivateProperty) { Functions.RegisterCallout(typeof(TrespassingOnPrivateProperty)); }
            Game.Console.Print("[LOG]: All callouts of the JMCalloutsRemastered.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
            Game.Console.Print();
        }
    }
}