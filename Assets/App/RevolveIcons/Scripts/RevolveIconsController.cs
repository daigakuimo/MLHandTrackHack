using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UnityEngine.Events;

namespace MLHandTrackHack.RevolveIcons
{
    public class RevolveIconsController : MonoBehaviour
    {
        [SerializeField] private HandGesture.GestureHand useHand = HandGesture.GestureHand.Right;
    [SerializeField] private IconAnimManager iconAnimManager = default;
    [SerializeField] private GameObject iconRoot = default;
    [SerializeField] private float rotateThreshold = 20.0f;
    [SerializeField] private int iconNum = 4;
    [SerializeField] private int oneRoleIconNum = 3;
    // 補間の強さ（0f～1f） 。0なら追従しない。1なら遅れなしに追従する。
    [SerializeField, Range(0f, 1f)] private float followStrength;
    
    [SerializeField] private UnityEvent onAppearEvents;
    [SerializeField] private UnityEvent onRoleEvents;
    [SerializeField] private UnityEvent onFistEvents;
    
    private Transform _cameraTransform;

    private bool _isRotate;

    private int _rollNum = 0;
    private Quaternion _defaultHandQuaternion;     // Cジェスチャー時の回転前の手のクォータニオン
    private Quaternion _lastHandQuaternion;        // Cジェスチャー時の1フレーム前の手のクォータニオン
    private float _lastDefaultDiffAngle;           // Cジェスチャー時の1フレーム前の手の角度の差
    private float _sumDiffAngle;                   // Cジェスチャー時の手の回転角度の合計
    private Vector3 _defaultScale;                 // RevolveIconのデフォルトのスケール

    private CancellationTokenSource _revolveIconsCts = new CancellationTokenSource();
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    private readonly Subject<Unit> _onFistSubject = new Subject<Unit>();
    public IObservable<Unit> OnFistAsObservable => _onFistSubject;

    public void Initialization()
    {
        _isRotate = false;
        _defaultHandQuaternion = HandTransformManager.HandQuaternion(useHand);
        _lastHandQuaternion = HandTransformManager.HandQuaternion(useHand);
        _revolveIconsCts = new CancellationTokenSource();
        _defaultScale = transform.localScale;
        
        if (Camera.main is null) return;
        _cameraTransform = Camera.main.transform;
    }

    
    /// <summary>
    /// ジェスチャに応じて処理を登録
    /// </summary>
    private void StandByPose()
    {
        // Cポーズで回転
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (HandGesture.GetHandGesture(useHand) == HandGesture.HandPoses.C)
                {
                    if (_isRotate)
                    {
                        JudgeRoleIcons();
                    }
                    else
                    {
                        _defaultHandQuaternion = HandTransformManager.HandQuaternion(useHand);
                        _lastHandQuaternion = HandTransformManager.HandQuaternion(useHand);
                        _isRotate = true;
                        _sumDiffAngle = 0;
                    }
                }
                else
                {
                    _isRotate = false;
                    _sumDiffAngle = 0;
                }
            })
            .AddTo(_compositeDisposable);

        // 握ったら
        this.ObserveEveryValueChanged(pose => HandGesture.GetHandGesture(useHand))
            .Where(pose => pose == HandGesture.HandPoses.Fist)
            .Subscribe(_ =>
            {
                _onFistSubject.OnNext(Unit.Default);
                iconAnimManager.OnFist();
                onFistEvents?.Invoke();
                DeleteRevolveIcons();
            })
            .AddTo(_compositeDisposable);
        
        InvokeRevolveIconsMoveUpdate(_revolveIconsCts.Token).Forget();
        InvokeIconsRoleUpdate(_revolveIconsCts.Token).Forget();
        InvokeRevolveIconsRoleUpdate(_revolveIconsCts.Token).Forget();
    }
    
    /// <summary>
    /// 手が回転した角度からIconsを回すか判定
    /// </summary>
    private void JudgeRoleIcons()
    {
        var defaultDiffQuaternion = Quaternion.Inverse(HandTransformManager.HandQuaternion(useHand)) * _defaultHandQuaternion;
        var lastDiffQuaternion = Quaternion.Inverse(HandTransformManager.HandQuaternion(useHand)) * _lastHandQuaternion;

        defaultDiffQuaternion.ToAngleAxis(out var defaultDiffAngle, out _);
        lastDiffQuaternion.ToAngleAxis(out var lastDiffAngle, out _);

        if (Mathf.Abs(lastDiffAngle) < rotateThreshold && _lastDefaultDiffAngle < defaultDiffAngle)
        {
            _sumDiffAngle += Mathf.Abs(lastDiffAngle);

            if (_sumDiffAngle >= 90.0f / oneRoleIconNum)
            {
                _rollNum++;
                _sumDiffAngle = 0;
            }
        }

        _lastHandQuaternion = HandTransformManager.HandQuaternion(useHand);
        _lastDefaultDiffAngle = defaultDiffAngle;
    }

    /// <summary>
    /// Icon回転
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async UniTask InvokeIconsRoleUpdate(CancellationToken cancellationToken)
    {
        while (true)
        {
            var isCanceled =
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();

            if (isCanceled)
            {
                break;
            }

            if (_rollNum > 0)
            {
                onRoleEvents?.Invoke();
                await iconRoot.transform.DORotate(new Vector3(0, -(360.0f / iconNum), 0), 0.1f, RotateMode.FastBeyond360).SetRelative(true).ToAwaiter();
                iconAnimManager.OnRoleIcons();
                _rollNum--;
            }
        }
    }
    
    
    /// <summary>
    /// 本体移動
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async UniTask InvokeRevolveIconsMoveUpdate(CancellationToken cancellationToken)
    {
        
        while (true)
        {
            var isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();
               
            if (isCanceled)
            {
                break;
            }
            
            Vector3 movePosition;
            if (useHand == HandGesture.GestureHand.Right)
            {
                movePosition = HandTransformManager.HandPosition(useHand) + (_cameraTransform.right * -0.1f + _cameraTransform.up * 0.05f);
            }
            else
            {
                movePosition = HandTransformManager.HandPosition(useHand) + (_cameraTransform.right * 0.1f + _cameraTransform.up * 0.05f);
            }
            // 手の位置に線形補間で追従させる
            transform.position = Vector3.Lerp ( transform.position, movePosition, followStrength );
            
        }
    }
    
    /// <summary>
    /// 本体回転
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async UniTask InvokeRevolveIconsRoleUpdate(CancellationToken cancellationToken)
    {
        while (true)
        {
            var isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();
               
            if (isCanceled)
            {
                break;
            }
            var direction = _cameraTransform.position - transform.position;
            direction.y = 0;
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }
    }

    /// <summary>
    /// RevolveIcons出現
    /// </summary>
    public async UniTask AppearRevolveIcons()
    {
        onAppearEvents?.Invoke();
        
        transform.localScale = Vector3.zero;

        await transform.DOScale(_defaultScale, 0.7f).SetEase(Ease.InOutQuint).ToAwaiter();
        
        StandByPose();
    }

    /// <summary>
    /// RevolveIcons削除
    /// </summary>
    private void DeleteRevolveIcons()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        DeleteRevolveIcons();
        _compositeDisposable?.Clear();
        _revolveIconsCts?.Cancel();
        _revolveIconsCts?.Dispose();
    }
    }
}
