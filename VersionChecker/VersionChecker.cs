using System;
using Rage;
using System.Net;

namespace JMCalloutsRemastered.VersionChecker
{
    public class PluginCheck
    {
        public static bool isUpdateAvailable()
        {
            string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/downloads/gta5mods/scripts/43616-jm-callouts-remastered-beta-now-open-source-for-callout-devs/");
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;
            try
            {
                receivedData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~JMCalloutsRemastered Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: Failed to check for an update.");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
                Game.Console.Print();
                return false;
            }
            if (receivedData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~JMCalloutsRemastered Warning", "~y~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~o~" + receivedData + "<br>~r~Please update to the latest build!");
                Game.Console.Print();
                Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: A new version of UnitedCallouts is available! Update to the latest build or play on your own risk.");
                Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                Game.Console.Print("[LOG]: New Version:  " + receivedData);
                Game.Console.Print();
                Game.Console.Print("================================================== JMCalloutsRemastered ===================================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~JMCalloutsRemastered", "", "Detected the ~g~latest~w~ build of ~y~JMCalloutsRemastered~w~!");
                return false;
            }
        }
    }
}