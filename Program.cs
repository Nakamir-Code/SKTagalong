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
            Vec3 targetPos = Vec3.Zero;
            Vec3 nextTargetPos = Vec3.Zero;

            Pose offsetMenuPose = Pose.Identity;
            Vec3 offsetTargetPos = Vec3.Zero;
            Vec3 offsetNextTargetPos = Vec3.Zero;
            Vec2 offsetPos = V.XY(15, -10) * U.cm;

            // Core application loop
            while (SK.Step(() =>
            {
                UIExtension.TagAlongBegin(ref menuPose, ref targetPos, ref nextTargetPos);
                UI.WindowBegin("Tagalong Window", ref menuPose, UIWin.Body, UIMove.None);
                UI.Label("Tagalong");
                UI.HSeparator();
                UI.Text("This window should always follow you and face you!", TextAlign.TopCenter);
                UI.WindowEnd();
                UIExtension.TagAlongEnd(ref menuPose, ref targetPos, ref nextTargetPos);

                UIExtension.TagAlongBegin(ref offsetMenuPose, ref offsetTargetPos, ref offsetNextTargetPos, offsetPos);
                UI.WindowBegin("Tagalong Window Offset", ref offsetMenuPose, UIWin.Body, UIMove.None);
                UI.Label("Tagalong Offset");
                UI.HSeparator();
                UI.Text("This window uses an XY offset!", TextAlign.TopCenter);
                UI.WindowEnd();
                UIExtension.TagAlongEnd(ref offsetMenuPose, ref offsetTargetPos, ref offsetNextTargetPos);
            })) ;
            SK.Shutdown();
        }
    }
}
