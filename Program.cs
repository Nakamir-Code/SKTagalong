namespace SKTagalong
{
    using StereoKit;
    using System;
    using static System.Net.Mime.MediaTypeNames;

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
            Vec3 targetPos = Vec3.Zero;
            Vec3 nextTargetPos = Vec3.Zero;

            // Core application loop
            while (SK.Step(() =>
            {
                UIExtension.TagAlongBegin(ref menuPose, ref targetPos, ref nextTargetPos);
                UI.WindowBegin("Tagalong Window", ref menuPose, UIWin.Body, UIMove.None);
                UI.Label("Tagalong & Billboard");
                UI.HSeparator();
                UI.Text("This window should always follow you and face you!", TextAlign.TopCenter);
                UI.WindowEnd();
                UIExtension.TagAlongEnd(ref menuPose, ref targetPos, ref nextTargetPos);
            })) ;
            SK.Shutdown();
        }
    }
}
