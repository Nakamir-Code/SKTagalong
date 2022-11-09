using StereoKit;
using StereoKit.Framework;

/// Billboard and Tagalong
/// Script calculates the pose of an object so that it follows the user and always faces the user
/// 

namespace SKTagalong
{
    internal class Tagalong : IStepper
    {
        // the current pose of the object
        private Pose _objPose = Pose.Identity;

        // the desired pose of the object based on the head pose
        private Pose _targetPose = Pose.Identity;

        private bool _isLerping = false;

        // the desired distance of the object on front of the user
        private float _distance = 0.5f;
        public bool Enabled => true;

        public bool Initialize()
        {
            Matrix head = Input.Head.ToMatrix();

            // the object is currently always set to be _distance meters straight in front of the user
            Vec3 initPosition = head.Translation + head.Pose.Forward * _distance;
            Quat initRotation = Quat.LookAt(initPosition, head.Translation);

            _objPose = Matrix.TR(initPosition, initRotation).Pose;
            _targetPose = Matrix.TR(initPosition, initRotation).Pose;

            return true;
        }

        public void Shutdown()
        {
        }

        public void Step()
        {
            Matrix head = Input.Head.ToMatrix();

            // Tagalong, move along with user
            if (_isLerping)
            {
                _objPose.position = Vec3.Lerp(_objPose.position, _targetPose.position, 0.1f);
                if (_objPose.position.InRadius(_targetPose.position, 0.001f))
                {
                    _isLerping = false;
                }
            }
            else if (!_objPose.position.InRadius(_targetPose.position, 0.3f))
            {
                _isLerping = true;
            }
            else
            {
                SetTargetPose(head);
            }

            // Billboarding, always face user
            Quat objRotation = Quat.LookAt(_objPose.position, head.Translation);

            _objPose = Matrix.TR(_objPose.position, objRotation).Pose;
        }

        public Pose ObjPose => _objPose;

        // set the new world position where the object should be
        private void SetTargetPose(Matrix headPose)
        {
            _targetPose.position = headPose.Translation + headPose.Pose.Forward * _distance;
        }
    }
}
