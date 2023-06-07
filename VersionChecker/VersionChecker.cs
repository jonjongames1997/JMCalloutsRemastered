using System;
using Rage;
using System.Net;

namespace JMCalloutsRemastered.VersionChecker
{
    public class VersionChecker
    {

        public static bool isUpdateAvailable()
        {
            string curVersion = Settings.CalloutVersion;

            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=43616&textOnly=1");
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;

            try
            {
                receivedData = webClient.DownloadString("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=43616&textOnly=1").Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~JMCalloutsRemastered Warning", "~y~Failed to check for a update", "Please check if you are ~o~online~w~, or try to reload the plugin.");

                Game.Console.Print();
                Game.Console.Print("================================================ JMCalloutsRemastered WARNING =====================================================");
                Game.Console.Print();
                Game.Console.Print("Failed to check for a update.");
                Game.Console.Print("Please check if you are online, or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================ JMCalloutsRemastered WARNING =====================================================");
                Game.Console.Print();
            }
            if (receivedData != Settings.CalloutVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~ExampleCallouts Warning", "~r~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~g~" + receivedData);

                Game.Console.Print();
                Game.Console.Print("================================================ JMCalloutsRemastered WARNING =====================================================");
                Game.Console.Print();
                Game.Console.Print("A new version of JMCalloutsRemastered is available! Update the Version, or play on your own risk.");
                Game.Console.Print("Current Version:  " + curVersion);
                Game.Console.Print("New Version:  " + receivedData);
                Game.Console.Print();
                Game.Console.Print("================================================ JMCalloutsRemastered WARNING =====================================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~JMCalloutsRemastered Information", "~g~You are playing on the newest version!", "Installed Version: ~g~" + curVersion + "");
                return false;
            }
        }

    }
}
