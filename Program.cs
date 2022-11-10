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

            Pose head = Input.Head;
            float forwardDistance = 0.5f;

            // Set the starting pose of the menu
            Vec3 targetPos = head.position + head.Forward * forwardDistance;
            Pose menuPose = new Pose(targetPos, Quat.LookAt(targetPos, head.position));
            Vec3 nextTargetPos = Vec3.Zero; // ignore, set inside TagAlong

            // Set the starting pose of the menu with an offset
            Vec2 offset = V.XY(15, -10) * U.cm;
            Vec3 offsetTargetPos = head.position + head.Forward * forwardDistance + head.Right * offset.x + head.Up * offset.y;
            Pose offsetMenuPose = new Pose(offsetTargetPos, Quat.LookAt(offsetTargetPos, head.position));
            Vec3 offsetNextTargetPos = Vec3.Zero; // ignore, set inside TagAlong

            // Core application
            SK.Run(() =>
            {
                UIExtension.TagAlongBegin(ref menuPose, ref targetPos, ref nextTargetPos, forwardDistance);
                UI.WindowBegin("Tagalong Window", ref menuPose, UIWin.Body, UIMove.None);
                UI.Label("Tagalong");
                UI.HSeparator();
                UI.Text("This window should always follow you and face you!", TextAlign.TopCenter);
                UI.WindowEnd();
                UIExtension.TagAlongEnd(ref menuPose, ref targetPos, ref nextTargetPos);

                UIExtension.TagAlongBegin(ref offsetMenuPose, ref offsetTargetPos, ref offsetNextTargetPos, offset, forwardDistance);
                UI.WindowBegin("Tagalong Window Offset", ref offsetMenuPose, UIWin.Body, UIMove.None);
                UI.Label("Tagalong Offset");
                UI.HSeparator();
                UI.Text("This window uses an XY offset!", TextAlign.TopCenter);
                UI.WindowEnd();
                UIExtension.TagAlongEnd(ref offsetMenuPose, ref offsetTargetPos, ref offsetNextTargetPos);
            });
        }
    }
}
