// ゲーム全体のフェーズを表す列挙型
// シナリオ進行とは独立して「今ゲームがどの段階か」を管理する
public enum GamePhase {
    Title,      // タイトル画面
    InGame,     // プレイ中（シナリオ進行中）
    GameOver,   // ゲームオーバー画面
    GameClear   // ゲームクリア画面
}
