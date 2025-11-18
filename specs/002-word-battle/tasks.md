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
- [ ] Enterキー押下でInitializePlayer()呼び出し
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
    - [ ] Keyboard: Start Game [Enter]（UIボタン設置しない）
- [ ] `EquipmentDisplayPanel`を作成（初期は非表示）
  - [ ] Text: `WeaponText` ("Weapon: {char} (ATK: {value})")
  - [ ] Text: `ShieldText` ("Shield: {char} (DEF: {value})")
  - [ ] Text: `PotionText` ("Potions: {count}")
  - [ ] Keyboard: Begin Battle [Enter]（UIボタン設置しない）
- [ ] `OperationPanel`を作成（常時表示、FR-020対応）
  - [ ] Text: `OperationText` - 現在選択可能な操作を表示
  - [ ] OperationPanelコンポーネントをアタッチ
- [ ] NameInputManagerスクリプトをアタッチし、各UI要素を接続
- [ ] TextMesh Proで適切なフォントとスタイル設定

**Files**:
- `Assets/Scenes/TextTextGame.unity` (既存シーン編集)
- `Assets/Scripts/OperationPanel.cs` (新規作成済み)