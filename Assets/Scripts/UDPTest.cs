using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class UDPTest : MonoBehaviour
{
    [SerializeField]
    private Button sendButton;

    [SerializeField]
    private Button stopButton;

    private void Start()
    {
        sendButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                UDPNetworkService.Instance.Send(new System.Random().Next().ToString());
            })
            .AddTo(gameObject);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ =>
            {
                UDPNetworkService.Instance.Send(new System.Random().Next().ToString());
            });

        stopButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                UDPNetworkService.Instance.Stop();
            })
            .AddTo(gameObject);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.X))
            .Subscribe(_ =>
            {
                UDPNetworkService.Instance.Stop();
            });
    }
}