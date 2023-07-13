using System;
using Rage;
using System.Net;

namespace JMCalloutsRemastered.VersionChecker
{
    public class VersionChecker
    {

        public bool isUpdateAvailable()
        {
            string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=43616&textOnly=1");
            WebClient webClient = new WebClient();
            string receiveData = string.Empty;
            try
            {
                receiveData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~JMCallouts Warning", "~r~Failed to check for an update", "Please make sure you're ~y~connected~w~ to the internet or try ~y~reload~w~ the plugin");
                Game.Console.Print();
                Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: Failed to check for an update.");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
                Game.Console.Print();
                return false;
            }
            if(receiveData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~JMCallouts Warning", "~y~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~o~" + receiveData + "<br>~r~Please update to the latest build!");
                Game.Console.Print();
                Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: A new version of JM Callouts Remastered is available! Update to the latest build. Old versions ~r~MAY~w~ cause a crash or 'Code 4' error message. You have been ~y~Warned.~w~");
                Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                Game.Console.Print("[LOG]: New Version:  " + receiveData);
                Game.Console.Print();
                Game.Console.Print("================================================== JM Callouts Remastered ===================================================");
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
