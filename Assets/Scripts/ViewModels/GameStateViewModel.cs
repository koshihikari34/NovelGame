using System;

// Modelを監視し、Viewに通知する役割を持つクラス
// Viewはこのイベントを購読してUIを更新する
public class GameStateViewModel {
    private readonly GameState _model;

    // フェーズが変わったときに通知するイベント
    public event Action<GamePhase> OnPhaseChanged;

    public GameStateViewModel(GameState model) {
        _model = model;
    }

    // フェーズを変更し、イベントを発火させる
    public void ChangePhase(GamePhase phase) {
        _model.SetPhase(phase);
        OnPhaseChanged?.Invoke(phase);
    }
}
