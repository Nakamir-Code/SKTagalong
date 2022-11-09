using StereoKit;
using StereoKit.Framework;

/// Billboard and Tagalong
/// Calculates the pose of an object so that it follows the user and always faces the user
/// 

namespace SKTagalong
{
    internal class Tagalong : IStepper
    {
        // the current pose of the object
        private Pose _objPose = Pose.Identity;

#if DEBUG
        private Material _blueTransparent;
        private Material _redTransparent;
#endif

        // the desired pose of the object based on the user's head pose
        private Pose _targetPose = Pose.Identity;

        private Pose _oldTargetPose = Pose.Identity;

        // the desired distance of the object on front of the user
        private float _desiredDistance = 0.5f;

        // max allowed distance for the menu from the desired position before being pulled back
        private float _maxAllowedDistance = 0.1f;

        public Pose ObjPose => _objPose;

        public bool Enabled => true;

        public bool Initialize()
        {
            Matrix head = Input.Head.ToMatrix();

            // the object is currently always set to be _desiredDistance meters straight in front of the user
            Vec3 initPosition = head.Translation + head.Pose.Forward * _desiredDistance;
            Quat initRotation = Quat.LookAt(initPosition, head.Translation);

            _objPose = Matrix.TR(initPosition, initRotation).Pose;
            _targetPose = Matrix.TR(initPosition, initRotation).Pose;

#if DEBUG
            _blueTransparent = Material.Default.Copy();
            _blueTransparent.Transparency = Transparency.Add;
            _blueTransparent["color"] = Color.Hex(0x0000FFFF);

            _redTransparent = Material.Default.Copy();
            _redTransparent.Transparency = Transparency.Add;
            _redTransparent["color"] = Color.Hex(0xFF0000FF);
#endif
            return true;
        }

        public void Shutdown()
        {
        }

        public void Step()
        {
            Matrix head = Input.Head.ToMatrix();

            SetTargetPose(head);

            // Tagalong, move along with user
            _objPose.position = Vec3.Lerp(_objPose.position, _oldTargetPose.position, 0.05f);
            if (!_objPose.position.InRadius(_targetPose.position, _maxAllowedDistance))
            {
                _oldTargetPose = _targetPose;
            }

#if DEBUG
            Mesh.Sphere.Draw(_blueTransparent, _oldTargetPose.ToMatrix(_maxAllowedDistance));
            Mesh.Sphere.Draw(_redTransparent, _targetPose.ToMatrix(_maxAllowedDistance));
#endif

            // Billboarding, always face user
            Quat objRotation = Quat.LookAt(_objPose.position, head.Translation);

            _objPose = Matrix.TR(_objPose.position, objRotation).Pose;
        }

        // set the new world position where the object should be
        private void SetTargetPose(Matrix headPose)
        {
            _targetPose.position = headPose.Translation + headPose.Pose.Forward * _desiredDistance;
        }
    }
}
