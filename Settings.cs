using System.Windows.Forms;
using Rage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace JMCalloutsRemastered
{
    internal static class Settings
    {
        internal static bool IllegalCampfireOnPublicBeach = true;
        internal static bool IllegalProstitution = true;
        internal static bool IntoxicatedIndividual = true;
        internal static bool PersonWithAKnife = false; // Not an active callout //
        internal static bool _911HangUp = false; // Temporarily disabled until further notice //
        internal static bool PossibleProstitution = true;
        internal static bool PublicDisturbance = false; // Temporarily disabled //
        internal static bool RefuseToPay = true; // Must be in Sandy Shores to take this call //
        internal static bool RefuseToLeave = true;
        internal static bool Soliciting = false; // Disabled for testing and bug fixing //
        internal static bool TrespassingOnPrivateProperty = true;
        internal static bool TrespassingOnRailRoadProperty = true;
        internal static bool CodeKaren = false; // Temporarily Disabled //


        internal static void LoadSettings()
        {
            Game.LogTrivial("[LOG]: Loading settings from JM Callouts Remastered.");
            var path = "Plugins/LSPDFR/JMCalloutsRemastered/JMCalloutsRemastered.ini";
            var ini = new InitializationFile(path);
            ini.Create();
            CodeKaren = ini.ReadBoolean("Callouts", "CodeKarren", false);
            _911HangUp = ini.ReadBoolean("Callouts", "_911HangUp", false);
            IllegalCampfireOnPublicBeach = ini.ReadBoolean("Callouts", "IllegalCampfireOnPublicBeach", true);
            IllegalProstitution = ini.ReadBoolean("Callouts", "IllegalProstitution", true);
            IntoxicatedIndividual = ini.ReadBoolean("Callouts", "IntoxicatedIndividual", true);
            PersonWithAKnife = ini.ReadBoolean("Callouts", "PersonWithAKnife", false);
            PossibleProstitution = ini.ReadBoolean("Callouts", "PossibleProstitution", true);
            PublicDisturbance = ini.ReadBoolean("Callouts", "PublicDisturbance", false);
            RefuseToLeave = ini.ReadBoolean("Callouts", "RefuseToLeave", true);
            RefuseToPay = ini.ReadBoolean("Callouts", "RefuseToPay", true);
            Soliciting = ini.ReadBoolean("Callouts", "Soliciting", true);
            TrespassingOnPrivateProperty = ini.ReadBoolean("Callouts", "TrespassingOnPrivateProperty", true);
            TrespassingOnRailRoadProperty = ini.ReadBoolean("Callouts", "TrespassingOnRailRoadProperty", true);
        }

        public static readonly string PluginVersion = "3.0.1";
    }
}
