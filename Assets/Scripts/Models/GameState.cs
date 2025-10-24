// ゲームの状態を保持するModel
// 「唯一の真実のソース (Single Source of Truth)」として扱う
public class GameState {
    // 現在のゲームフェーズ
    public GamePhase Phase { get; private set; } = GamePhase.Title;

    // フェーズを変更するメソッド
    public void SetPhase(GamePhase phase) {
        Phase = phase;
    }
}
