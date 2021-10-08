using MagicLeapTools;
using UnityEngine;

public class HandTransformManager : MonoBehaviour
{
    public static Vector3 HandPosition(HandGesture.GestureHand useHand)
    {
        return useHand == HandGesture.GestureHand.Right
            ? HandInput.Right.Skeleton.Position
            : HandInput.Left.Skeleton.Position;
    }
    
    public static Quaternion HandQuaternion(HandGesture.GestureHand useHand)
    {
        return useHand == HandGesture.GestureHand.Right
            ? HandInput.Right.Skeleton.Rotation
            : HandInput.Left.Skeleton.Rotation;
    }
}
