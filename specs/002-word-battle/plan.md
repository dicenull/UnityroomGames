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
  - Keyboard: Start Game [Enter]（UIボタン除去）