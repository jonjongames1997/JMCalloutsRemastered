using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using System.Reflection;

namespace JMCalloutsRemastered
{
    public class Main : Plugin
    {

        public static String Version = "2.3.1";

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin JM Callouts Remastered" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " by OfficerMorrison has been initialized!");
            Game.LogTrivial("Go On Duty to fully load JM Callouts Remastered");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("JM Callouts has been cleaned up!");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts();
                Game.DisplayNotification("JM Callouts Remastered by OfficerMorrison | ~y~Version 2.3.1 | BETA | has been successfully loaded");
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.IntoxicatedIndividual));
            Functions.RegisterCallout(typeof(Callouts.IllegalCampfireOnPublicBeach));
            Functions.RegisterCallout(typeof(Callouts.TrespassingOnPrivateProperty));
            Functions.RegisterCallout(typeof(Callouts.RefuseToLeave));
            Functions.RegisterCallout(typeof(Callouts.PossibleProstitution));
            Functions.RegisterCallout(typeof(Callouts.RefuseToPay));
            Functions.RegisterCallout(typeof(Callouts.Soliciting));
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }

        public static bool IsLSPDFRPluginIsRunning(string Plugin, Version minversion = null)
        {
            foreach(Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if(an.Name.ToLower() == Plugin.ToLower())
                {
                    if(minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}