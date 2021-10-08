using System.Threading;
using Cysharp.Threading.Tasks;
using MagicLeapTools;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace MLHandTrackHack.NewFingers
{
    public class NewFingerView : MonoBehaviour
    {
        public enum NewFingerType
        {
            Thumb,
            Index,
            Middle,
            Ring,
            Pinky,
            RingDown
        };

        [SerializeField] private HandGesture.GestureHand useHand = HandGesture.GestureHand.Right;
        [SerializeField] private GameObject thumbObj;
        [SerializeField] private GameObject indexObj;
        [SerializeField] private GameObject middleObj;
        [SerializeField] private GameObject ringObj;
        [SerializeField] private GameObject ringDownObj;

        private CancellationTokenSource _fingerTipPositionCts;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        // Start is called before the first frame update
        private void OnEnable()
        {
            _fingerTipPositionCts = new CancellationTokenSource();
            InvokeFingerTipPositionUpdate(_fingerTipPositionCts.Token).Forget();

            this.UpdateAsObservable()
                .First(ready => HandInput.Ready)
                .Subscribe(_ =>
                    CreateSetActive()
                );
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _fingerTipPositionCts.Cancel();
        }

        private void CreateSetActive()
        {
            this.ObserveEveryValueChanged(visible =>
                    useHand == HandGesture.GestureHand.Right ? HandInput.Right.Visible : HandInput.Left.Visible)
                .Subscribe(SetActiveFingerObject)
                .AddTo(_compositeDisposable);
        }



        // Updateが重いのでUniTaskでUpdate処理するといいらしい
        private async UniTask InvokeFingerTipPositionUpdate(CancellationToken cancellationToken)
        {
            while (true)
            {
                var isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken)
                    .SuppressCancellationThrow();

                if (isCanceled)
                {
                    break;
                }

                var handSkeleton = useHand == HandGesture.GestureHand.Right
                    ? HandInput.Right.Skeleton
                    : HandInput.Left.Skeleton;

                thumbObj.transform.position = handSkeleton.Thumb.Tip.GetPosition(FilterType.Filtered);
                indexObj.transform.position = handSkeleton.Index.Tip.GetPosition(FilterType.Filtered);
                middleObj.transform.position = handSkeleton.Middle.Tip.GetPosition(FilterType.Filtered);
                ringObj.transform.position = handSkeleton.Ring.Tip.GetPosition(FilterType.Filtered);
                ringDownObj.transform.position = handSkeleton.Ring.Knuckle.GetPosition(FilterType.Filtered);
            }
        }

        private void SetActiveFingerObject(bool isActive)
        {
            thumbObj.SetActive(isActive);
            indexObj.SetActive(isActive);
            middleObj.SetActive(isActive);
            ringObj.SetActive(isActive);
            ringDownObj.SetActive(isActive);
        }

    }
}