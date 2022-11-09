/// Billboard and Tagalong
/// Calculates the pose of an object so that it follows the user and always faces the user 
namespace SKTagalong
{
    using StereoKit;

    public static class UIExtension
    {
        public static void TagAlongBegin(ref Pose pose, ref Vec3 targetPos, ref Vec3 nextTargetPos, float forwardDistance = 0.5f, float lerpBlend = 0.05f)
        {
            Pose head = Input.Head;

            // The next target position will always be in front of the user.
            nextTargetPos = head.position + head.Forward * forwardDistance;

            // Tagalong, move the window along with user at the previous target.
            pose.position = Vec3.Lerp(pose.position, targetPos, lerpBlend);

            // Billboarding always faces the user.
            pose.orientation = Quat.LookAt(pose.position, head.position);
        }

        public static void TagAlongEnd(ref Pose pose, ref Vec3 targetPos, ref Vec3 nextTargetPos, float collisionRadius = 0.15f)
        {
            // Set the new target position for when it is outside the user's FOV
            // so that we lerp only when the new window position is out of range.
            if (!pose.position.InRadius(nextTargetPos, collisionRadius))
            {
                targetPos = nextTargetPos;
            }

#if DEBUG
            Material transparentMat = Material.Default.Copy();
            transparentMat.Transparency = Transparency.Add;
            Mesh.Sphere.Draw(transparentMat, Matrix.TS(targetPos, collisionRadius), Color.Hex(0x0000FFFF));
            Mesh.Sphere.Draw(transparentMat, Matrix.TS(nextTargetPos, collisionRadius), Color.Hex(0xFF0000FF));
#endif
        }
    }
}
