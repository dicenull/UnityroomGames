# Implementation Tasks: 英単語ローグライクバトル

**Feature Branch**: `002-word-battle`  
**Created**: 2025-11-12  
**Status**: Ready to Start  
**Plan**: [plan.md](./plan.md)

---

## Task List

### Phase 1: プレイヤー名入力と初期装備システム (P1)

#### Task 1.1: CharacterStats.cs - 文字パラメータシステム
**Priority**: P1  
**Estimate**: 1-2時間  
**Dependencies**: なし

**Description**:
英字A-Zに対応する攻撃力・防御力のマッピングを実装し、文字列から総合パラメータを計算するstaticクラスを作成。

**Acceptance Criteria**:
- [ ] `CharacterStats.cs`を`Assets/Scripts/`に作成
- [ ] A-Zの攻撃力・防御力マッピングを定義
  - 母音 (A,E,I,O,U): 攻撃力2, 防御力3
  - 通常子音: 攻撃力3, 防御力2
  - レア文字 (Q,X,Z): 攻撃力4, 防御力4
- [ ] `CalculateAttackPower(string weapon)`メソッド実装
- [ ] `CalculateDefensePower(string shield)`メソッド実装
- [ ] 大文字小文字両対応
- [ ] 単体テスト: "C" → 攻撃力3, "CAT" → 攻撃力8 (3+2+3)

**Files**:
- `Assets/Scripts/CharacterStats.cs` (新規)

---

#### Task 1.2: GameData拡張 - プレイヤーデータ管理
**Priority**: P1  
**Estimate**: 1時間  
**Dependencies**: Task 1.1

**Description**:
既存のGameData.csにプレイヤーの名前、装備、HP、ポーションを管理するReactivePropertyを追加。名前から装備を生成するロジックを実装。

**Acceptance Criteria**:
- [ ] `PlayerName`, `Weapon`, `Shield`, `PotionCount`のReactivePropertyを追加
- [ ] `PlayerHP`, `PlayerMaxHP`のReactivePropertyを追加（初期値20）
- [ ] `InitializePlayer(string name)`メソッド実装
  - 最初の文字→武器
  - 最後の文字→盾（2文字以上の場合）
  - 中間文字数→ポーション数
- [ ] `Reset()`メソッドを更新してプレイヤーデータもリセット
- [ ] 単体テスト: "CAT" → 武器"C", 盾"T", ポーション1, HP20

**Files**:
- `Assets/Scripts/GameData.cs` (既存ファイル拡張)

---

#### Task 1.3: NameInputManager - 名前入力制御
**Priority**: P1  
**Estimate**: 2-3時間  
**Dependencies**: Task 1.2

**Description**:
名前入力UIの制御スクリプトを作成。英字のみ入力を許可し、ボタンクリックでGameData.InitializePlayer()を呼び出す。

**Acceptance Criteria**:
- [ ] `NameInputManager.cs`を`Assets/Scripts/`に作成
- [ ] InputFieldコンポーネントへの参照を設定
- [ ] 英字のみ入力を許可（正規表現で検証: `^[a-zA-Z]+$`）
- [ ] StartButtonクリックでInitializePlayer()呼び出し
- [ ] 入力が空またはエラー時にエラーメッセージ表示
- [ ] 装備生成後、EquipmentDisplayPanelに遷移
- [ ] 入力中はリアルタイムでプレビュー表示（オプション）

**Files**:
- `Assets/Scripts/NameInputManager.cs` (新規)

---

#### Task 1.4: 名前入力UI作成
**Priority**: P1  
**Estimate**: 2-3時間  
**Dependencies**: Task 1.3

**Description**:
TextTextGame.unityに名前入力画面と装備確認画面のUIを追加。

**Acceptance Criteria**:
- [ ] `NameInputPanel`を作成（Canvas配下）
  - [ ] タイトルText: "Enter Your Name (English)"
  - [ ] InputField: `NameInputField`（PlaceholderText: "Your Name"）
  - [ ] Button: `StartButton` ("Start Game")
- [ ] `EquipmentDisplayPanel`を作成（初期は非表示）
  - [ ] Text: `WeaponText` ("Weapon: {char} (ATK: {value})")
  - [ ] Text: `ShieldText` ("Shield: {char} (DEF: {value})")
  - [ ] Text: `PotionText` ("Potions: {count}")
  - [ ] Button: `BeginBattleButton` ("Begin Battle")
- [ ] NameInputManagerスクリプトをアタッチし、各UI要素を接続
- [ ] TextMesh Proで適切なフォントとスタイル設定

**Files**:
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)

---

#### Task 1.5: Phase 1 統合テスト
**Priority**: P1  
**Estimate**: 1時間  
**Dependencies**: Task 1.4

**Description**:
Phase 1の全機能が正しく動作することを確認。

**Acceptance Criteria**:
- [ ] "CAT"入力 → 武器"C" (ATK:3), 盾"T" (DEF:2), ポーション1個
- [ ] "AT"入力 → 武器"A" (ATK:2), 盾"T" (DEF:2), ポーション0個
- [ ] "X"入力 → 武器"X" (ATK:4), 盾なし, ポーション0個
- [ ] 数字・記号入力 → エラーメッセージ表示
- [ ] 装備画面でパラメータが正しく表示
- [ ] BeginBattleButtonクリックで次のフェーズへ遷移準備完了

**Files**:
- なし（テストのみ）

---

### Phase 2: ターン制バトルと敵の次の行動予告 (P2)

#### Task 2.1: EnemyData - 敵データ定義
**Priority**: P2  
**Estimate**: 1時間  
**Dependencies**: Task 1.5

**Description**:
敵の英単語リストと、HP・攻撃力を計算するstaticクラスを作成。

**Acceptance Criteria**:
- [ ] `EnemyData.cs`を`Assets/Scripts/`に作成
- [ ] 難易度別の敵リストを定義
  - `EasyEnemies`: "CAT", "DOG", "RAT", "BAT"
  - `MediumEnemies`: "BIRD", "FISH", "BEAR", "WOLF"
  - `HardEnemies`: "TIGER", "EAGLE", "SHARK"
- [ ] `CalculateEnemyHP(string word)`実装 (文字数 * 5)
- [ ] `CalculateEnemyAttack(string word)`実装 (文字数 + 2)
- [ ] `GetRandomEnemy(int difficulty)`実装（0=Easy, 1=Medium, 2=Hard）
- [ ] 単体テスト: "CAT" → HP15, ATK5

**Files**:
- `Assets/Scripts/EnemyData.cs` (新規)

---

#### Task 2.2: GameData拡張 - 戦闘状態管理
**Priority**: P2  
**Estimate**: 1時間  
**Dependencies**: Task 2.1

**Description**:
GameData.csに敵の状態とターン管理のReactivePropertyを追加。

**Acceptance Criteria**:
- [ ] `CurrentEnemy` (string)を追加
- [ ] `EnemyHP`, `EnemyMaxHP` (int)を追加
- [ ] `EnemyAttack` (int)を追加
- [ ] `EnemyNextAction` (string)を追加
- [ ] `IsPlayerTurn` (bool)を追加（初期値true）
- [ ] `DefeatedEnemies` (int)を追加（初期値0）
- [ ] `Reset()`を更新して戦闘状態もリセット

**Files**:
- `Assets/Scripts/GameData.cs` (既存ファイル拡張)

---

#### Task 2.3: BattleManager - 戦闘ロジック実装
**Priority**: P2  
**Estimate**: 4-5時間  
**Dependencies**: Task 2.2

**Description**:
戦闘の中核となるロジックを実装。敵の生成、プレイヤーの行動処理、敵のターン処理、勝敗判定を行う。

**Acceptance Criteria**:
- [ ] `BattleManager.cs`を`Assets/Scripts/`に作成
- [ ] `SpawnEnemy(int difficulty)`メソッド実装
  - 難易度に応じた敵をランダム選択
  - GameDataに敵情報を設定
  - 次の行動を予告（"攻撃: Xダメージ" など）
- [ ] `PlayerAttack()`メソッド実装
  - CharacterStats.CalculateAttackPower(Weapon)で攻撃力計算
  - 敵HPを減算
  - BattleLog更新
  - 敵HP≤0なら勝利処理
  - そうでなければEnemyTurn()呼び出し
- [ ] `PlayerDefend()`メソッド実装
  - 防御フラグを立てる
  - EnemyTurn()呼び出し
- [ ] `EnemyTurn()`メソッド実装
  - 予告された行動を実行
  - プレイヤーが防御中なら盾の防御力で軽減
  - プレイヤーHPを減算
  - プレイヤーHP≤0ならGameOver()呼び出し
  - 次の行動を新たに予告
- [ ] `CheckVictory()`メソッド: 敵撃破時の処理（現段階では新しい敵生成）
- [ ] `CheckDefeat()`メソッド: GameData.GameOver()呼び出し

**Files**:
- `Assets/Scripts/BattleManager.cs` (新規)

---

#### Task 2.4: 戦闘UI作成
**Priority**: P2  
**Estimate**: 3-4時間  
**Dependencies**: Task 2.3

**Description**:
TextTextGame.unityに戦闘画面のUIを追加。プレイヤー/敵の情報、行動ボタン、ログを表示。

**Acceptance Criteria**:
- [ ] `BattlePanel`を作成（初期は非表示、Phase 1完了後に表示）
  - [ ] Text: `PlayerHPText` ("HP: {current}/{max}")
  - [ ] Text: `EnemyInfoText` ("Enemy: {word} HP: {current}/{max}")
  - [ ] Text: `EnemyNextActionText` ("Next: {action}")
  - [ ] Text: `PlayerEquipmentText` ("Weapon: {weapon} (ATK: {atk}) / Shield: {shield} (DEF: {def})")
  - [ ] Button: `AttackButton` ("Attack")
  - [ ] Button: `DefendButton` ("Defend")
  - [ ] Button: `PotionButton` ("Use Potion ({count})") (グレーアウト状態)
  - [ ] Text: `BattleLogText` (ScrollView推奨、最新の行動結果を表示)
- [ ] BattleManagerスクリプトをアタッチし、各UI要素を接続
- [ ] ReactivePropertyでUI自動更新を設定
- [ ] BeginBattleButtonクリックでBattlePanel表示、最初の敵生成

**Files**:
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)

---

#### Task 2.5: Phase 2 統合テスト
**Priority**: P2  
**Estimate**: 1-2時間  
**Dependencies**: Task 2.4

**Description**:
Phase 2の戦闘システムが正しく動作することを確認。

**Acceptance Criteria**:
- [ ] 戦闘開始時に敵が正しく生成される（名前、HP、攻撃力）
- [ ] 敵の次の行動が予告表示される
- [ ] Attackボタン → 敵HPが減少、BattleLogに結果表示
- [ ] Defendボタン → 敵の攻撃が軽減される
- [ ] 敵を倒す → 新しい敵が生成される
- [ ] プレイヤーHP≤0 → GameOverCanvas表示
- [ ] ターン交互に進行する

**Files**:
- なし（テストのみ）

---

### Phase 3: 敵を倒して文字獲得と成長システム (P3)

#### Task 3.1: RewardManager - 報酬選択ロジック
**Priority**: P3  
**Estimate**: 3-4時間  
**Dependencies**: Task 2.5

**Description**:
敵撃破時に報酬画面を表示し、文字を選択して武器または盾に追加するロジックを実装。

**Acceptance Criteria**:
- [ ] `RewardManager.cs`を`Assets/Scripts/`に作成
- [ ] `ShowRewardPanel(string enemyWord)`メソッド実装
  - 敵の英単語から文字ボタンを動的生成
  - 各ボタンに文字を表示
  - ボタンクリックで文字選択
- [ ] `OnLetterSelected(char letter)`メソッド実装
  - EquipChoicePanelを表示
  - 武器/盾選択ボタンを表示
- [ ] `AddToWeapon(char letter)`メソッド実装
  - GameData.AddToWeapon()呼び出し
  - UI更新
  - 次の敵生成
- [ ] `AddToShield(char letter)`メソッド実装
  - GameData.AddToShield()呼び出し
  - UI更新
  - 次の敵生成

**Files**:
- `Assets/Scripts/RewardManager.cs` (新規)

---

#### Task 3.2: GameData拡張 - 装備強化メソッド
**Priority**: P3  
**Estimate**: 30分  
**Dependencies**: Task 3.1

**Description**:
GameData.csに武器・盾に文字を追加するメソッドを実装。

**Acceptance Criteria**:
- [ ] `AddToWeapon(char letter)`メソッド実装
  - Weapon.Valueに文字を追加
- [ ] `AddToShield(char letter)`メソッド実装
  - Shield.Valueに文字を追加
- [ ] 単体テスト: Weapon="C"、AddToWeapon('A') → Weapon="CA"

**Files**:
- `Assets/Scripts/GameData.cs` (既存ファイル拡張)

---

#### Task 3.3: 報酬UI作成
**Priority**: P3  
**Estimate**: 2-3時間  
**Dependencies**: Task 3.2

**Description**:
TextTextGame.unityに報酬選択画面のUIを追加。

**Acceptance Criteria**:
- [ ] `RewardPanel`を作成（初期は非表示）
  - [ ] Text: "Enemy Defeated! Choose a letter:"
  - [ ] Transform: `LetterButtonContainer` (動的ボタン生成用)
  - [ ] Panel: `EquipChoicePanel` (初期は非表示)
    - [ ] Text: "Add '{char}' to..."
    - [ ] Button: `AddToWeaponButton` ("Weapon")
    - [ ] Button: `AddToShieldButton` ("Shield")
- [ ] RewardManagerスクリプトをアタッチし、各UI要素を接続
- [ ] ボタンプレハブを作成（動的生成用）

**Files**:
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)
- `Assets/Prefabs/LetterButton.prefab` (新規)

---

#### Task 3.4: 難易度スケーリング実装
**Priority**: P3  
**Estimate**: 1時間  
**Dependencies**: Task 3.3

**Description**:
BattleManager.csに難易度計算ロジックを追加し、敵撃破数に応じて難易度を上昇させる。

**Acceptance Criteria**:
- [ ] `BattleManager.SpawnEnemy()`を更新
  - 難易度を計算: `DefeatedEnemies / 2`
  - Clampで0～2に制限
  - EnemyData.GetRandomEnemy(difficulty)で敵選択
- [ ] `CheckVictory()`を更新
  - DefeatedEnemies++
  - RewardManager.ShowRewardPanel()呼び出し
- [ ] 単体テスト: 0-1体目=Easy, 2-3体目=Medium, 4体目以降=Hard

**Files**:
- `Assets/Scripts/BattleManager.cs` (既存ファイル拡張)

---

#### Task 3.5: Phase 3 統合テスト
**Priority**: P3  
**Estimate**: 1-2時間  
**Dependencies**: Task 3.4

**Description**:
Phase 3の成長システムが正しく動作することを確認。

**Acceptance Criteria**:
- [ ] 敵撃破 → RewardPanel表示、文字ボタンが表示される
- [ ] 文字クリック → EquipChoicePanel表示
- [ ] Weaponに追加 → 攻撃力が上昇、UI更新
- [ ] Shieldに追加 → 防御力が上昇、UI更新
- [ ] 複数の敵を倒す → 徐々に難しい敵が出現
- [ ] 装備が長くなっても正常に動作

**Files**:
- なし（テストのみ）

---

### Phase 4: 回復ポーション使用システム (P4)

#### Task 4.1: ポーション使用ロジック実装
**Priority**: P4  
**Estimate**: 1-2時間  
**Dependencies**: Task 3.5

**Description**:
BattleManager.csにポーション使用ロジックを追加。

**Acceptance Criteria**:
- [ ] `UsePotion()`メソッド実装
  - PotionCount > 0を確認
  - PlayerHPを+10（最大HPまで）
  - PotionCount--
  - BattleLog更新
  - EnemyTurn()呼び出し
- [ ] PotionButtonのクリックイベントを設定
- [ ] PotionCountが0の場合はボタン無効化

**Files**:
- `Assets/Scripts/BattleManager.cs` (既存ファイル拡張)

---

#### Task 4.2: ポーションUI更新
**Priority**: P4  
**Estimate**: 1時間  
**Dependencies**: Task 4.1

**Description**:
PotionButtonの有効/無効切り替えとReactiveProperty連携を実装。

**Acceptance Criteria**:
- [ ] PotionCountのReactivePropertyをSubscribe
- [ ] PotionCount > 0 → ボタン有効
- [ ] PotionCount == 0 → ボタングレーアウト
- [ ] ボタンテキスト: "Use Potion ({count})"

**Files**:
- `Assets/Scripts/BattleManager.cs` (既存ファイル拡張)
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)

---

#### Task 4.3: Phase 4 統合テスト
**Priority**: P4  
**Estimate**: 30分  
**Dependencies**: Task 4.2

**Description**:
Phase 4のポーション機能が正しく動作することを確認。

**Acceptance Criteria**:
- [ ] ポーション所持時 → ボタン有効
- [ ] ポーション使用 → HP+10、ポーション数-1
- [ ] ポーション0個 → ボタン無効
- [ ] HP満タンで使用 → 最大HPを超えない
- [ ] 使用後は敵のターンに移行

**Files**:
- なし（テストのみ）

---

### Phase 5: ボス戦とゲームクリア (P5)

#### Task 5.1: ボス敵データ追加
**Priority**: P5  
**Estimate**: 1時間  
**Dependencies**: Task 4.3

**Description**:
EnemyData.csにボス敵のデータと計算ロジックを追加。

**Acceptance Criteria**:
- [ ] `BossEnemies`配列を追加: "DRAGON", "PHOENIX", "KRAKEN"
- [ ] `CalculateBossHP(string word)`実装 (文字数 * 10)
- [ ] `CalculateBossAttack(string word)`実装 (文字数 + 5)
- [ ] `GetRandomBoss()`メソッド実装
- [ ] `IsBoss`フラグをGameDataに追加

**Files**:
- `Assets/Scripts/EnemyData.cs` (既存ファイル拡張)
- `Assets/Scripts/GameData.cs` (既存ファイル拡張)

---

#### Task 5.2: ボス出現条件とロジック
**Priority**: P5  
**Estimate**: 2-3時間  
**Dependencies**: Task 5.1

**Description**:
BattleManager.csにボス出現条件と特殊攻撃パターンを実装。

**Acceptance Criteria**:
- [ ] `SpawnNextEnemy()`メソッド実装
  - DefeatedEnemies >= 5 → SpawnBoss()
  - そうでなければ → SpawnEnemy()
- [ ] `SpawnBoss()`メソッド実装
  - ボスをランダム選択
  - ボスのHP・攻撃力を設定
  - IsBoss=trueに設定
  - ボス出現演出（BGM変化など、オプション）
- [ ] `EnemyTurn()`を拡張
  - IsBossの場合、ランダムで特殊攻撃
  - 「強攻撃」: ダメージ2倍
  - 「防御無視」: 盾無効

**Files**:
- `Assets/Scripts/BattleManager.cs` (既存ファイル拡張)

---

#### Task 5.3: VictoryManager - クリア処理
**Priority**: P5  
**Estimate**: 2時間  
**Dependencies**: Task 5.2

**Description**:
ボス撃破時のクリア処理とクリア画面表示を実装。

**Acceptance Criteria**:
- [ ] `VictoryManager.cs`を`Assets/Scripts/`に作成
- [ ] `ShowVictoryPanel()`メソッド実装
  - VictoryPanelを表示
  - 統計情報を表示（倒した敵数、最終装備など）
  - Time.timeScale = 0f
- [ ] RestartButtonクリックで名前入力画面に戻る

**Files**:
- `Assets/Scripts/VictoryManager.cs` (新規)

---

#### Task 5.4: クリアUI作成
**Priority**: P5  
**Estimate**: 2時間  
**Dependencies**: Task 5.3

**Description**:
TextTextGame.unityにクリア画面のUIを追加。

**Acceptance Criteria**:
- [ ] `VictoryPanel`を作成（初期は非表示）
  - [ ] Text: "VICTORY!" (大きく表示)
  - [ ] Text: `StatsText` ("Defeated Enemies: {count}")
  - [ ] Text: `FinalWeaponText` ("Final Weapon: {weapon} (ATK: {atk})")
  - [ ] Text: `FinalShieldText` ("Final Shield: {shield} (DEF: {def})")
  - [ ] Button: `RestartButton` ("Play Again")
- [ ] VictoryManagerスクリプトをアタッチし、各UI要素を接続

**Files**:
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)

---

#### Task 5.5: BattleManagerとの統合
**Priority**: P5  
**Estimate**: 1時間  
**Dependencies**: Task 5.4

**Description**:
BattleManager.CheckVictory()にボス撃破時の処理を追加。

**Acceptance Criteria**:
- [ ] `CheckVictory()`を更新
  - IsBoss && EnemyHP <= 0 → VictoryManager.ShowVictoryPanel()
  - そうでなければ → RewardManager.ShowRewardPanel()

**Files**:
- `Assets/Scripts/BattleManager.cs` (既存ファイル拡張)

---

#### Task 5.6: Phase 5 統合テスト
**Priority**: P5  
**Estimate**: 1-2時間  
**Dependencies**: Task 5.5

**Description**:
Phase 5のボス戦とクリア機能が正しく動作することを確認。

**Acceptance Criteria**:
- [ ] 5体撃破後 → ボス出現
- [ ] ボスが特殊攻撃を使用
- [ ] ボス撃破 → VictoryPanel表示、統計表示
- [ ] RestartButton → 名前入力画面に戻る
- [ ] リスタート後、全データがリセットされる

**Files**:
- なし（テストのみ）

---

### 最終フェーズ: ポリッシュとWebGLビルド

#### Task 6.1: WebGLビルド確認
**Priority**: 最終  
**Estimate**: 1-2時間  
**Dependencies**: Task 5.6

**Description**:
WebGLビルドが正常に動作することを確認し、必要に応じて調整。

**Acceptance Criteria**:
- [ ] WebGLビルド成功
- [ ] 全機能が正常に動作
- [ ] フレームレート60fps維持
- [ ] UI表示が正しい
- [ ] 入力が正常に動作

**Files**:
- `ProjectSettings/ProjectSettings.asset` (ビルド設定)

---

#### Task 6.2: バランス調整
**Priority**: 最終  
**Estimate**: 2-3時間  
**Dependencies**: Task 6.1

**Description**:
ゲーム全体をプレイテストし、難易度バランスを調整。

**Acceptance Criteria**:
- [ ] プレイヤーの80%が3体以上の敵を倒せる
- [ ] 平均プレイ時間が5～15分
- [ ] ボス戦が適切な難易度
- [ ] パラメータ調整（必要に応じて）

**Files**:
- `Assets/Scripts/CharacterStats.cs` (パラメータ調整)
- `Assets/Scripts/EnemyData.cs` (パラメータ調整)

---

#### Task 6.3: Success Criteria検証
**Priority**: 最終  
**Estimate**: 1時間  
**Dependencies**: Task 6.2

**Description**:
仕様書のSuccess Criteriaがすべて達成されていることを確認。

**Acceptance Criteria**:
- [ ] SC-001: 名前入力から戦闘開始まで10秒以内
- [ ] SC-002: 各ターンの行動選択から結果表示まで1秒以内
- [ ] SC-003: 初心者が5回以内のプレイで基本ルールを理解できる
- [ ] SC-004: 平均プレイ時間5～15分
- [ ] SC-005: 80%以上が3体以上撃破
- [ ] SC-006: クリア画面が3秒以内に表示
- [ ] SC-007: WebGLで60fps維持

**Files**:
- なし（検証のみ）

---

## Task Summary

| Phase | Tasks | Estimated Time |
|-------|-------|----------------|
| Phase 1 (P1) | 5 tasks | 7-11時間 |
| Phase 2 (P2) | 5 tasks | 10-13時間 |
| Phase 3 (P3) | 5 tasks | 7-11時間 |
| Phase 4 (P4) | 3 tasks | 2.5-3.5時間 |
| Phase 5 (P5) | 6 tasks | 9-13時間 |
| 最終 | 3 tasks | 4-6時間 |
| **Total** | **27 tasks** | **39.5-57.5時間** |

---

## 実装順序の推奨

1. **Phase 1を完了してテスト** → 動作確認
2. **Phase 2を完了してテスト** → 戦闘ループ確認
3. **Phase 3を完了してテスト** → 成長システム確認
4. **Phase 4を完了してテスト** → 回復システム確認
5. **Phase 5を完了してテスト** → ゲーム完結
6. **最終フェーズ** → ポリッシュとリリース準備

各フェーズ完了時にコミットして、動作確認を行うことを推奨します。
