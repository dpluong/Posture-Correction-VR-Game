using UnityEngine;
using UnityEngine.SpatialTracking;

public class AmplifiedPoseDriver : TrackedPoseDriver
{
    public float gain = 1f;

    Pose localPose = new Pose();
    override protected void PerformUpdate()
    {
        // fetch pose from the source
        PoseDataSource.TryGetDataFromSource(poseSource, out localPose);

        // fiddle with the pose's data
        Vector3 euler = localPose.rotation.eulerAngles;
        euler.y *= 1f + gain;
        localPose.rotation.eulerAngles = euler;

        // use adjusted pose to set our local transform
        SetLocalTransform(localPose.position, localPose.rotation,
          PoseDataFlags.Position | PoseDataFlags.Rotation);
    }
}
