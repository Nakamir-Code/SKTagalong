namespace SKTagalong
{
    using StereoKit;
    using System;

    internal class Program
    {
        static void Main(string[] _)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "SKTagalong",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);

            Pose menuPose = Pose.Identity;
            Vec3 targetPose = Vec3.Zero;

            // Core application loop
            while (SK.Step(() =>
            {
                UIExtension.WindowBegin("Tagalong Window", ref menuPose, ref targetPose, UIWin.Body);
                UI.Label("Tagalong & Billboard");
                UI.HSeparator();
                UI.Text("This window should always follow you and face you!", TextAlign.TopCenter);
                UI.WindowEnd();
            })) ;
            SK.Shutdown();
        }
    }
}
