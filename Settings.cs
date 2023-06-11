using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Rage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace JMCalloutsRemastered
{
    internal static class Settings
    {
        internal static bool IllegalCampfireOnPublicBeach = true;
        internal static bool IntoxicatedIndividual = true;
        internal static bool PossibleProstitution = true;
        internal static bool RefuseToLeave = true;
        internal static bool RefuseToPay = true;
        internal static bool Soliciting = false; // Change this to true. False by Default. //
        internal static bool TrespassingOnPrivateProperty = true;
        internal static bool PersonWithAKnife = true;
        internal static bool HelpMessages = false; // Displays Important information //
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialogue = Keys.Y;
        

        internal static void LoadSettings() 
        {
            Game.LogTrivial("[LOG]: Loading config from JMCalloutsRemastered");
            var path = "Plugins/LSPDFR/JMCalloutsRemastered/JMCalloutsRemastered.ini";
            var ini = new InitializationFile(path);
            ini.Create();
            IllegalCampfireOnPublicBeach = ini.ReadBoolean("Callouts", "IllegalCampfireOnPublicBeach", true);
            IntoxicatedIndividual = ini.ReadBoolean("Callouts", "IntoxicatedIndividual", true);
            PossibleProstitution = ini.ReadBoolean("Callouts", "PossibleProstitution", true);
            RefuseToLeave = ini.ReadBoolean("Callouts", "RefuseToLeave", true);
            RefuseToPay = ini.ReadBoolean("Callouts", "RefuseToPay", true);
            Soliciting = ini.ReadBoolean("Callouts", "Soliciting", false);
            TrespassingOnPrivateProperty = ini.ReadBoolean("Callouts", "TrespassingOnPrivateProperty", true);
            PersonWithAKnife = ini.ReadBoolean("Callouts", "PersonWithAKnife", true);
            HelpMessages = ini.ReadBoolean("HelpMessages", "HelpMessages", false);
            EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
            Dialogue = ini.ReadEnum("Keys", "Dialog", Keys.Y);
        }
        public static readonly string PluginVersion = "2.4.1.0";
    }
}
