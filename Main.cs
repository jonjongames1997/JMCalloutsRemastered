using Rage;
using System;
using System.Collections.Generic;
using System.Collections;
using LSPD_First_Response.Mod.API;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace JMCalloutsRemastered
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin JMCalloutsRemastered" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "by OfficerMorrison has been initialized!");
            Game.LogTrivial("Go on duty to fully load JMCalloutsRemastered");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("JM Callouts Remasterd has successfully cleaned up!");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts();

                Game.DisplayNotification("JM Callouts Remastered by OfficerMorrison | ~y~Version 2.10.3 | has successfully loaded!");
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.IllegalCampfireOnPublicBeach));
            Functions.RegisterCallout(typeof(Callouts.IntoxicatedIndividual));
            Functions.RegisterCallout(typeof(Callouts.PossibleProstitution));
            Functions.RegisterCallout(typeof(Callouts.RefuseToLeave));
            Functions.RegisterCallout(typeof(Callouts.RefuseToPay));
            Functions.RegisterCallout(typeof(Callouts.TrespassingOnPrivateProperty));
            Functions.RegisterCallout(typeof(Callouts.Soliciting));
            Functions.RegisterCallout(typeof(Callouts.IllegalProstitution));
            Functions.RegisterCallout(typeof(Callouts.TrespassingOnRailRoadProperty));
            Functions.RegisterCallout(typeof(Callouts._911HangUp));
            Functions.RegisterCallout(typeof(Callouts.CodeKaren));
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

        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
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