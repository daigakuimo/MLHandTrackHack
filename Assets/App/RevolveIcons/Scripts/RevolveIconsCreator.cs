using Cysharp.Threading.Tasks;
using MagicLeapTools;
using UniRx;
using UnityEngine;

namespace MLHandTrackHack.RevolveIcons
{
    public class RevolveIconsCreator : MonoBehaviour
    {
        [SerializeField] private HandGesture.GestureHand useHand = HandGesture.GestureHand.Right;
        [SerializeField] private GameObject revolveIconsPrefab = default;
        
        
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private Transform cameraTransform;
        public bool IsAppearRevolveIcons { get; set; }
    
        private void OnEnable()
        {
            IsAppearRevolveIcons = false;
            if (Camera.main is { })
            {
                cameraTransform = Camera.main.transform;
            }

            this.ObserveEveryValueChanged(pose => HandGesture.GetHandGesture(useHand))
                .Where(pose => pose == HandGesture.HandPoses.OpenHand && !IsAppearRevolveIcons)
                .Subscribe(_ =>
                {
                    
                    var transformPosition = transform.position;
                    
                    var direction = cameraTransform.position - transformPosition; 

                    direction.y = 0;
                    
                    Vector3 createPosition;
                    if (useHand == HandGesture.GestureHand.Right)
                    {
                        createPosition = HandTransformManager.HandPosition(useHand) + (cameraTransform.right * -0.1f + cameraTransform.up * 0.05f);
                    }
                    else
                    {
                        createPosition =  HandTransformManager.HandPosition(useHand) + (cameraTransform.right * 0.1f + cameraTransform.up * 0.05f);
                    }
                    var revolveIcons = Instantiate(revolveIconsPrefab, createPosition, Quaternion.LookRotation(direction, Vector3.up),  gameObject.transform.parent);
                    var revolveIconsController = revolveIcons.GetComponent<RevolveIconsController>();
                    revolveIconsController.Initialization();
                    revolveIconsController.OnFistAsObservable
                        .Subscribe(_ =>
                        {
                            IsAppearRevolveIcons = false;
                        })
                        .AddTo(revolveIconsController);
                    revolveIconsController.AppearRevolveIcons().Forget();
                    
                
                    IsAppearRevolveIcons = true;
                })
                .AddTo(_compositeDisposable);
        }
    
        private void OnDisable()
        {
            _compositeDisposable.Clear();
        }
    }
}