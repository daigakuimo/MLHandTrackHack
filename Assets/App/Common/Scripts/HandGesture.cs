using System.Threading;
using Cysharp.Threading.Tasks;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandGesture : MonoBehaviour
{
    // 使用するジェスチャーをEnumで準備
    public enum HandPoses { C, Finger, Thumb, OpenHand, Fist, NoPose, Pinch, Ok };
    
    public enum GestureHand { Left, Right };
    // 現在のジェスチャーを保存
    public static HandPoses RightPose { get; private set; }
    
    public static HandPoses LeftPose { get; private set; }

    // 使用するジェスチャーの配列
    private MLHandTracking.HandKeyPose[] _gestures;

    readonly CancellationTokenSource _handGestureCts = new CancellationTokenSource();
    
    void Start()
    {
        // ハンドトラッキングをスタート

        RightPose = HandPoses.NoPose;
        LeftPose = HandPoses.NoPose;

        InvokeHandGestureUpdate(_handGestureCts.Token).Forget();
    }

    private void OnDestroy()
    {
        _handGestureCts.Cancel();
        _handGestureCts.Dispose();
    }
    
    private async UniTask InvokeHandGestureUpdate(CancellationToken cancellationToken)
    {
        while (true)
        {
            var isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();

            if (isCanceled)
            {
                break;
            }

            if (!HandInput.Right.Visible && !HandInput.Left.Visible)
            {
                RightPose = HandPoses.NoPose;
                LeftPose = HandPoses.NoPose;
                continue;
            }
            
            if (!HandInput.Right.Visible)
            {
                RightPose = HandPoses.NoPose;
            }
            
            if (!HandInput.Left.Visible)
            {
                LeftPose = HandPoses.NoPose;
            }

            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.OpenHand))
            {
                RightPose = HandPoses.OpenHand;
            }

            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Fist))
            {
                RightPose = HandPoses.Fist;
            }
            
            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.C))
            {
                RightPose = HandPoses.C;
            }
            
            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Ok))
            {
                RightPose = HandPoses.Ok;
            }
            
            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Finger))
            {
                RightPose = HandPoses.Finger;
            }
            
            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Thumb))
            {
                RightPose = HandPoses.Thumb;
            }
            
            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Pinch))
            {
                RightPose = HandPoses.Pinch;
            }

            if (IsGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.NoPose))
            {
                RightPose = HandPoses.NoPose;
            }

            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.OpenHand))
            {
                LeftPose = HandPoses.OpenHand;
            }

            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist))
            {
                LeftPose = HandPoses.Fist;
            }
            
            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.C))
            {
                LeftPose = HandPoses.C;
            }
            
            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Ok))
            {
                LeftPose = HandPoses.Ok;
            }
            
            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
            {
                LeftPose = HandPoses.Finger;
            }
            
            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Thumb))
            {
                LeftPose = HandPoses.Thumb;
            }
            
            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Pinch))
            {
                LeftPose = HandPoses.Pinch;
            }

            if (IsGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.NoPose))
            {
                LeftPose = HandPoses.NoPose;
            }
        }
    }

    /// <summary>
    /// ジェスチャーを取得するためのメソッド
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool IsGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand == null) return false;
        if (hand.KeyPose != type) return false;
        return hand.HandKeyPoseConfidence > 0.9f;
    }

    public static HandPoses GetHandGesture(GestureHand useHand)
    {
        return useHand == GestureHand.Right ? RightPose : LeftPose;
    }
}
