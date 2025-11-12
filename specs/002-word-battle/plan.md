# Implementation Plan: 英単語ローグライクバトル

**Feature Branch**: `002-word-battle`  
**Created**: 2025-11-12  
**Status**: Ready for Implementation  
**Spec**: [spec.md](./spec.md)

## 実装の全体方針

既存の `GameData`, `GameManager`, `TextTextGame` シーンを活用し、段階的に機能を追加する。各優先度（P1～P5）ごとに独立して動作確認可能な状態を保ちながら実装を進める。

### 既存システムの活用
- **GameData**: ゲームオーバー管理を拡張し、プレイヤー/敵のHP、装備、戦闘状態を管理
- **GameManager**: ゲームオーバーCanvas制御を継続使用
- **TextTextGame.unity**: 既存シーンに新しいUIとロジックを追加

---

## Phase 1: プレイヤー名入力と初期装備システム (Priority: P1)

### 目的
プレイヤーが名前を入力し、文字から武器・盾・回復ポーションを生成する基本システム。

### 実装タスク

#### 1.1 データ構造の設計
**File**: `Assets/Scripts/CharacterStats.cs` (新規作成)
- 英字A-Zに対応する攻撃力・防御力のマッピング
- 文字列から総合パラメータを計算するメソッド
  - `CalculateAttackPower(string weapon)`: 武器文字列から攻撃力を計算
  - `CalculateDefensePower(string shield)`: 盾文字列から防御力を計算

**パラメータ設計案**:
```
母音 (A,E,I,O,U): 攻撃力2, 防御力3 (バランス型)
子音 (B,C,D,F,G,H,J,K,L,M,N,P,Q,R,S,T,V,W,X,Y,Z): 攻撃力3, 防御力2 (攻撃寄り)
レア文字 (Q,X,Z): 攻撃力4, 防御力4 (高性能)
```

#### 1.2 GameDataの拡張
**File**: `Assets/Scripts/GameData.cs` (既存ファイルの拡張)
- プレイヤーデータの追加
  ```csharp
  public ReactiveProperty<string> PlayerName = new("");
  public ReactiveProperty<string> Weapon = new("");
  public ReactiveProperty<string> Shield = new("");
  public ReactiveProperty<int> PotionCount = new(0);
  public ReactiveProperty<int> PlayerHP = new(20);
  public ReactiveProperty<int> PlayerMaxHP = new(20);
  ```
- 名前から装備を生成するメソッド
  ```csharp
  public void InitializePlayer(string name)
  {
      PlayerName.Value = name;
      Weapon.Value = name.Length >= 1 ? name[0].ToString() : "";
      Shield.Value = name.Length >= 2 ? name[name.Length - 1].ToString() : "";
      PotionCount.Value = Mathf.Max(0, name.Length - 2);
      PlayerHP.Value = PlayerMaxHP.Value;
  }
  ```
- `Reset()`の更新: プレイヤーデータもリセット

#### 1.3 名前入力UI
**File**: `Assets/Scripts/NameInputManager.cs` (新規作成)
- InputFieldコンポーネントの制御
- 英字のみ入力を許可（正規表現で検証）
- 入力完了ボタンのクリックで `GameData.InitializePlayer()` 呼び出し
- 装備生成後、戦闘画面に遷移

**UI構成** (TextTextGame.unityに追加):
- Panel: NameInputPanel
  - Text: "Enter Your Name (English)"
  - InputField: NameInputField (英字のみ)
  - Button: StartButton ("Start Game")
- Panel: EquipmentDisplayPanel (装備確認画面)
  - Text: "Weapon: {char} (ATK: {value})"
  - Text: "Shield: {char} (DEF: {value})"
  - Text: "Potions: {count}"
  - Button: BeginBattleButton ("Begin Battle")

#### 1.4 テスト計画
- [ ] "CAT"入力 → 武器"C", 盾"T", ポーション1個
- [ ] "AT"入力 → 武器"A", 盾"T", ポーション0個
- [ ] "X"入力 → 武器"X", 盾"", ポーション0個
- [ ] 数字・記号入力 → 除外またはエラー表示
- [ ] 装備画面でパラメータが正しく表示される

---

## Phase 2: ターン制バトルと敵の次の行動予告 (Priority: P2)

### 目的
プレイヤーと敵のターン制戦闘システム。敵の次の行動を予告表示し、戦略性を追加。

### 実装タスク

#### 2.1 敵データ構造
**File**: `Assets/Scripts/EnemyData.cs` (新規作成)
- 敵の英単語リスト（難易度別に分類）
  ```csharp
  public static readonly string[] EasyEnemies = { "CAT", "DOG", "RAT", "BAT" };
  public static readonly string[] MediumEnemies = { "BIRD", "FISH", "BEAR", "WOLF" };
  public static readonly string[] HardEnemies = { "TIGER", "EAGLE", "SHARK" };
  ```
- 敵のHP・攻撃力計算
  ```csharp
  public static int CalculateEnemyHP(string word) => word.Length * 5;
  public static int CalculateEnemyAttack(string word) => word.Length + 2;
  ```

#### 2.2 GameDataに戦闘状態を追加
**File**: `Assets/Scripts/GameData.cs` (拡張)
```csharp
public ReactiveProperty<string> CurrentEnemy = new("");
public ReactiveProperty<int> EnemyHP = new(0);
public ReactiveProperty<int> EnemyMaxHP = new(0);
public ReactiveProperty<int> EnemyAttack = new(0);
public ReactiveProperty<string> EnemyNextAction = new(""); // "攻撃: 5ダメージ" など
public ReactiveProperty<bool> IsPlayerTurn = new(true);
public ReactiveProperty<int> DefeatedEnemies = new(0);
```

#### 2.3 戦闘マネージャー
**File**: `Assets/Scripts/BattleManager.cs` (新規作成)
- 敵の生成: `SpawnEnemy(int difficulty)`
  - 難易度に応じて敵をランダム選択
  - HP, 攻撃力を計算してGameDataに設定
  - 次の行動を予告（ランダムで攻撃または防御）
- プレイヤーの行動処理
  - `PlayerAttack()`: 武器の攻撃力で敵にダメージ
  - `PlayerDefend()`: 防御態勢（次のターンで盾の防御力を適用）
- 敵のターン処理
  - `EnemyTurn()`: 予告された行動を実行
  - 攻撃の場合、プレイヤーが防御していれば軽減
  - ターン終了後、次の行動を予告
- 勝敗判定
  - 敵HP≤0 → P3の報酬画面へ（現段階では次の敵生成）
  - プレイヤーHP≤0 → ゲームオーバー

#### 2.4 戦闘UI
**UI構成** (TextTextGame.unityに追加):
- Panel: BattlePanel
  - Text: プレイヤーHP表示 "HP: {current}/{max}"
  - Text: 敵情報 "Enemy: {word} HP: {current}/{max}"
  - Text: 敵の次の行動 "Next: {action}"
  - Text: プレイヤー装備 "Weapon: {weapon} (ATK: {atk}) / Shield: {shield} (DEF: {def})"
  - Button: AttackButton ("Attack")
  - Button: DefendButton ("Defend")
  - Button: PotionButton ("Use Potion ({count})") (P4で完全実装)
  - Text: BattleLog (行動結果表示)

#### 2.5 テスト計画
- [ ] 敵が正しく生成される（名前、HP、攻撃力）
- [ ] 敵の次の行動が予告表示される
- [ ] プレイヤーが攻撃 → 敵HPが減少
- [ ] プレイヤーが防御 → 敵の攻撃が軽減される
- [ ] 敵を倒す → 新しい敵が生成される（P3実装後は報酬画面）
- [ ] プレイヤーHP≤0 → ゲームオーバーCanvas表示

---

## Phase 3: 敵を倒して文字獲得と成長システム (Priority: P3)

### 目的
敵撃破後に文字を獲得し、装備を強化する成長システム。

### 実装タスク

#### 3.1 報酬選択UI
**File**: `Assets/Scripts/RewardManager.cs` (新規作成)
- 敵撃破時に報酬画面を表示
- 敵の英単語から文字をボタンで表示
- 選択した文字を武器または盾に追加するか選択
- 選択完了後、次の敵を生成

**UI構成** (TextTextGame.unityに追加):
- Panel: RewardPanel
  - Text: "Enemy Defeated! Choose a letter:"
  - Button[]: 敵の文字ごとにボタン生成（動的）
  - Panel: EquipChoicePanel (文字選択後に表示)
    - Text: "Add '{char}' to..."
    - Button: AddToWeaponButton ("Weapon")
    - Button: AddToShieldButton ("Shield")

#### 3.2 装備強化ロジック
**File**: `Assets/Scripts/GameData.cs` (拡張)
```csharp
public void AddToWeapon(char letter)
{
    Weapon.Value += letter;
}

public void AddToShield(char letter)
{
    Shield.Value += letter;
}
```

#### 3.3 難易度スケーリング
**File**: `Assets/Scripts/BattleManager.cs` (拡張)
- `SpawnEnemy()`の難易度計算
  ```csharp
  int difficulty = GameData.Instance.DefeatedEnemies.Value / 2; // 2体ごとに難易度上昇
  difficulty = Mathf.Clamp(difficulty, 0, 2); // 0=Easy, 1=Medium, 2=Hard
  ```

#### 3.4 テスト計画
- [ ] 敵撃破 → 報酬画面表示、文字ボタンが表示される
- [ ] 文字選択 → 武器/盾選択画面表示
- [ ] 武器に追加 → 攻撃力が上昇、UI更新
- [ ] 盾に追加 → 防御力が上昇、UI更新
- [ ] 複数の敵を倒す → 徐々に難しい敵が出現

---

## Phase 4: 回復ポーション使用システム (Priority: P4)

### 目的
戦闘中の回復オプションを追加し、戦略性を深める。

### 実装タスク

#### 4.1 ポーション使用ロジック
**File**: `Assets/Scripts/BattleManager.cs` (拡張)
```csharp
public void UsePotion()
{
    if (GameData.Instance.PotionCount.Value > 0)
    {
        int healAmount = 10; // 固定回復量
        GameData.Instance.PlayerHP.Value = Mathf.Min(
            GameData.Instance.PlayerHP.Value + healAmount,
            GameData.Instance.PlayerMaxHP.Value
        );
        GameData.Instance.PotionCount.Value--;
        EnemyTurn(); // ポーション使用後は敵のターン
    }
}
```

#### 4.2 UI更新
- PotionButtonの有効/無効切り替え
- ポーション数が0の場合はグレーアウト
- 使用時のアニメーション（オプション）

#### 4.3 テスト計画
- [ ] ポーション所持時 → ボタンが有効
- [ ] ポーション使用 → HP回復、ポーション数減少
- [ ] ポーション0個 → ボタン無効
- [ ] HP満タンで使用 → 最大HPを超えない

---

## Phase 5: ボス戦とゲームクリア (Priority: P5)

### 目的
ゲームの目標とクリア条件を実装。

### 実装タスク

#### 5.1 ボス敵データ
**File**: `Assets/Scripts/EnemyData.cs` (拡張)
```csharp
public static readonly string[] BossEnemies = { "DRAGON", "PHOENIX", "KRAKEN" };
public static int CalculateBossHP(string word) => word.Length * 10;
public static int CalculateBossAttack(string word) => word.Length + 5;
```

#### 5.2 ボス出現条件
**File**: `Assets/Scripts/BattleManager.cs` (拡張)
```csharp
public void SpawnNextEnemy()
{
    if (GameData.Instance.DefeatedEnemies.Value >= 5)
    {
        SpawnBoss();
    }
    else
    {
        SpawnEnemy();
    }
}
```

#### 5.3 特殊攻撃パターン
- ボスの次の行動に「強攻撃」「防御無視」などバリエーション追加
- ランダムで特殊攻撃を選択

#### 5.4 クリア画面
**File**: `Assets/Scripts/VictoryManager.cs` (新規作成)
**UI構成**:
- Panel: VictoryPanel
  - Text: "VICTORY!"
  - Text: "Defeated Enemies: {count}"
  - Text: "Final Weapon: {weapon} (ATK: {atk})"
  - Text: "Final Shield: {shield} (DEF: {def})"
  - Button: RestartButton ("Play Again")

#### 5.5 テスト計画
- [ ] 5体撃破後 → ボス出現、演出（BGMなど）
- [ ] ボスが特殊攻撃を使用
- [ ] ボス撃破 → クリア画面表示、統計表示
- [ ] クリア画面でリスタート → 名前入力画面に戻る

---

## 技術的実装詳細

### 使用技術スタック
- **Unity**: 2022 LTS以降
- **R3**: ReactiveProperty for Unity (既存使用中)
- **TextMesh Pro**: UI表示
- **Input System**: プレイヤー入力

### アーキテクチャパターン
- **シングルトン**: GameData（既存）
- **Observer**: R3のReactivePropertyでUI更新
- **状態管理**: GameData中心のデータ駆動設計

### ファイル構成
```
Assets/
├── Scenes/
│   └── TextTextGame.unity (既存、UI拡張)
├── Scripts/
│   ├── GameData.cs (既存、拡張)
│   ├── GameManager.cs (既存、そのまま使用)
│   ├── CharacterStats.cs (新規)
│   ├── EnemyData.cs (新規)
│   ├── NameInputManager.cs (新規)
│   ├── BattleManager.cs (新規)
│   ├── RewardManager.cs (新規)
│   └── VictoryManager.cs (新規)
```

---

## リスクと対策

### パフォーマンス
- **リスク**: 文字パラメータ計算の頻繁な実行
- **対策**: 計算結果をキャッシュ、ReactivePropertyで変更時のみ再計算

### バランス調整
- **リスク**: 敵の難易度が高すぎる/低すぎる
- **対策**: パラメータを定数として外部化（CharacterStatsクラス）、調整しやすい構造

### UI複雑化
- **リスク**: 画面遷移が多く、管理が煩雑
- **対策**: PanelのActive/Inactive切り替えで単純化、状態機械パターンの導入（必要に応じて）

---

## 次のステップ

1. **Phase 1から順次実装**: 各フェーズ完了後に動作確認
2. **WebGLビルドテスト**: 各フェーズ完了時にビルド確認
3. **バランス調整**: Phase 5完了後、全体のプレイテストとパラメータ調整
4. **ポリッシュ**: アニメーション、サウンド、エフェクトの追加（オプション）

---

## 実装チェックリスト

### Phase 1 (P1)
- [ ] CharacterStats.cs作成、パラメータマッピング実装
- [ ] GameData.cs拡張、プレイヤーデータ追加
- [ ] NameInputManager.cs作成
- [ ] 名前入力UI作成（NameInputPanel, EquipmentDisplayPanel）
- [ ] テスト実施

### Phase 2 (P2)
- [ ] EnemyData.cs作成、敵データ定義
- [ ] GameData.cs拡張、戦闘状態追加
- [ ] BattleManager.cs作成、戦闘ロジック実装
- [ ] 戦闘UI作成（BattlePanel）
- [ ] テスト実施

### Phase 3 (P3)
- [ ] RewardManager.cs作成
- [ ] 報酬UI作成（RewardPanel）
- [ ] 装備強化ロジック実装
- [ ] 難易度スケーリング実装
- [ ] テスト実施

### Phase 4 (P4)
- [ ] ポーション使用ロジック実装
- [ ] UI更新（ボタン有効/無効）
- [ ] テスト実施

### Phase 5 (P5)
- [ ] ボス敵データ追加
- [ ] ボス出現条件実装
- [ ] 特殊攻撃パターン実装
- [ ] VictoryManager.cs作成
- [ ] クリアUI作成（VictoryPanel）
- [ ] テスト実施

### 最終確認
- [ ] WebGLビルド成功
- [ ] 全機能動作確認
- [ ] バランス調整完了
- [ ] Success Criteria達成確認
