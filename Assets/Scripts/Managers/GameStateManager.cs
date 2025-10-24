using UnityEngine;

// ゲーム全体の状態を管理するシングルトン
// ModelとViewModelを生成・保持し、シーン遷移を指示する
public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }

    // ModelとViewModelを保持
    public GameState GameState { get; private set; }
    public GameStateViewModel GameStateVM { get; private set; }

    private void Awake() {
        // シングルトン化（重複したら破棄）
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 状態の初期化
        GameState = new GameState();
        GameStateVM = new GameStateViewModel(GameState);
    }

    // フェーズを変更し、対応するシーンに遷移する
    public void ChangePhase(GamePhase phase) {
        GameStateVM.ChangePhase(phase);

        switch (phase) {
            case GamePhase.Title:
                SceneTransitionManager.Instance.LoadSceneWithFade("TitleScene");
                break;
            case GamePhase.InGame:
                SceneTransitionManager.Instance.LoadSceneWithFade("MainScene");
                break;
            case GamePhase.GameOver:
                SceneTransitionManager.Instance.LoadSceneWithFade("GameOverScene");
                break;
            case GamePhase.GameClear:
                SceneTransitionManager.Instance.LoadSceneWithFade("GameClearScene");
                break;
        }
    }
}
