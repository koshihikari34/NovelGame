using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour
{
    [SerializeField] private Button startButton; // インスペクタでボタンをアサイン

    private void Awake()
    {
        // AwakeやStartでイベントをコードから登録する
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnDestroy()
    {
        // メモリリーク防止のため、イベント解除
        startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    // ボタンが押されたときの処理
    private void OnStartButtonClicked()
    {
        // GameStateManager経由でMainSceneへ遷移
        GameStateManager.Instance.ChangePhase(GamePhase.InGame);
    }
}
