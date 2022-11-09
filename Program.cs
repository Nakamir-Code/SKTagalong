using StereoKit;
using System;

namespace SKTagalong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "SKTagalong",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);
                        
            // The Tagalong class including tagalong and billboard functionality
            Tagalong _tagalong = SK.AddStepper<Tagalong>();

            // Core application loop
            while (SK.Step(() =>
            {
                Pose menuPose = _tagalong.ObjPose;

                UI.WindowBegin("Tagalong Window", ref menuPose, UIWin.Body, UIMove.None);

                UI.Label("Tagalong & Billboard");
                UI.HSeparator();
                UI.Text("This window should always follow you and face you!", TextAlign.TopCenter);

                UI.WindowEnd();

            })) ;
            SK.Shutdown();
        }
    }
}
