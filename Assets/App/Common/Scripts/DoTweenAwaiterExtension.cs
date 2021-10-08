using System;
using System.Runtime.CompilerServices;
using System.Threading;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// DOTweenをasync/awaitに返還すDOTweenAwaiterの拡張メソッド
/// </summary>
public static class DOTweenAwaiterExtension
{
    public static DOTweenAwaiter ToAwaiter(this Tween tween,
        CancellationToken cancellationToken = default,
        TweenCancelBehaviour behaviour = TweenCancelBehaviour.Kill)
    {
        return new DOTweenAwaiter(tween, cancellationToken, behaviour);
    }
}

/// <summary>
/// DOTweenをawaiterに変換する
/// </summary>
public struct DOTweenAwaiter : ICriticalNotifyCompletion
{
    private Tween _tween;
    private CancellationToken _cancellationToken;
    private TweenCancelBehaviour _behaviour;

    public DOTweenAwaiter(Tween tween, CancellationToken cancellationToken, TweenCancelBehaviour behaviour)
    {
        _tween = tween;
        _cancellationToken = cancellationToken;
        _behaviour = behaviour;
    }

    public bool IsCompleted => _tween.IsPlaying() == false;

    public void GetResult() => _cancellationToken.ThrowIfCancellationRequested();

    public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);

    public void UnsafeOnCompleted(Action continuation)
    {
        DOTweenAwaiter tmpThis = this;
        var tween = _tween;
        var regist = tmpThis._cancellationToken.Register(() =>
        {
            // tokenが発火したらタイプをチェックしてTweenの終了振る舞いを変更する
            switch (tmpThis._behaviour)
            {
                case TweenCancelBehaviour.Kill:
                    tween.Kill();
                    break;
                case TweenCancelBehaviour.KillWithCompleteCallback:
                    tween.Kill(true);
                    break;
                case TweenCancelBehaviour.Complete:
                    tween.Complete();
                    break;
            }
        });
        
        _tween.OnKill(() =>
        {
            // CancellationTokenRegistrationを破棄する
            regist.Dispose();
            // 続きを実行
            continuation();
        });
    }

    public DOTweenAwaiter GetAwaiter() => this;
}

/// <summary>
/// Tweenキャンセル時の振る舞い
/// </summary>
public enum TweenCancelBehaviour
{
    Kill,
    KillWithCompleteCallback,
    Complete,
}