/// Billboard and Tagalong
/// Calculates the pose of an object so that it follows the user and always faces the user 
namespace SKTagalong
{
    using StereoKit;

    public static class UIExtension
    {
        public static void WindowBegin(string text, ref Pose pose, ref Vec3 targetPos, Vec2 size, UIWin windowType = UIWin.Normal, float forwardDistance = 0.5f, float collisionRadius = 0.1f, float lerpBlend = 0.05f)
        {
            Pose head = Input.Head;

            // The next target position will always be in front of the user.
            Vec3 nextTargetPose = head.position + head.Forward * forwardDistance;

            // Tagalong, move the window along with user at the previous target.
            pose.position = Vec3.Lerp(pose.position, targetPos, lerpBlend);

            // Set the new target position for when it is outside the user's FOV
            // so that we lerp only when the new window position is out of range.
            if (!pose.position.InRadius(nextTargetPose, collisionRadius))
            {
                targetPos = nextTargetPose;
            }

            // Billboarding always faces the user.
            pose.orientation = Quat.LookAt(pose.position, head.position);

#if DEBUG
            Material blueTransparent = Material.Default.Copy();
            blueTransparent.Transparency = Transparency.Add;
            blueTransparent["color"] = Color.Hex(0x0000FFFF);

            Material redTransparent = Material.Default.Copy();
            redTransparent.Transparency = Transparency.Add;
            redTransparent["color"] = Color.Hex(0xFF0000FF);

            Mesh.Sphere.Draw(blueTransparent, Matrix.TS(targetPos, collisionRadius));
            Mesh.Sphere.Draw(redTransparent, Matrix.TS(nextTargetPose, collisionRadius));
#endif

            UI.WindowBegin(text, ref pose, size, windowType, UIMove.None);
        }

        public static void WindowBegin(string text, ref Pose pose, ref Vec3 targetPos, UIWin windowType = UIWin.Normal, float forwardDistance = 0.5f, float collisionRadius = 0.1f, float lerpBlend = 0.05f)
            => WindowBegin(text, ref pose, ref targetPos, Vec2.Zero, windowType, forwardDistance, collisionRadius, lerpBlend);
    }
}
