# ModernForm
WinformsのFormをモダンにしたもの
# ModernForms 利用説明書

## 1. 概要
**ModernForms** クラスは、Windows Forms アプリケーションにおける「モダンな外観・操作感を持つベースフォーム」です。  
従来の標準 Form を置き換えることで、以下の機能を簡単に利用できます。

- 枠なし・角丸・Mica/Acrylic 背景など Windows 11 スタイル対応  
- ダーク/ライトテーマの切替、Glow（発光ボーダー）効果  
- タイトルバー/システムボタン（最小化・最大化・閉じる）の組み込み  
- メニュー/コンテキストメニューのテーマ適用  
- 拡張しやすい設計（プリセット呼び出しで即テーマ切替）  

推奨 OS：Windows 11（Mica/Acrylic/角丸は OS 機能に依存）。Win10 でも実行可（該当効果は自動的に無効/代替）
---

## 2. 主な特徴

### 外観機能
- **枠なしウィンドウ**  
  標準の FormBorderStyle を非表示にし、モダンな見た目を実現。
- **ドラッグ/リサイズ対応**  
  タイトルバーやコンテンツ部分をドラッグ移動可能。四辺に「透明グリップ」を配置し、リサイズも可能。
- **Glow（発光ボーダー）**  
  枠に発光効果を追加し、アクティブ/非アクティブ時で色や強さを変更可能。
- **テーマ連動**  
  OS テーマに追従するほか、独自に「ライト/ダーク」を固定可能。

### テーマプリセット
ワンクリックで雰囲気を変えられるテーマを搭載。例：
- **Graphite** … 落ち着いた濃グレー + Mica  
- **PaperWhite** … 白基調 + 角丸  
- **Midnight** … 深い紺色 + 弱めの Glow  
- **AmberGlass** … 琥珀色アクセント + Acrylic  

### メニュー装飾
- `MenuStrip` / `StatusStrip` / `ToolStrip` / `ContextMenuStrip` に自動的にダーク/ライト配色を適用。  
- `DarkColorTable` / `LightColorTable` を内蔵し、従来の「灰色が残る」問題を解消。

---

## 3. 主なプロパティ

| プロパティ | 型 | 説明 |
|------------|----|------|
| `Theme` | ThemeMode | Light/Dark/System を指定 |
| `Backdrop` | BackdropKind | 背景効果（Mica/Acrylic/None/Tabbed/Auto） |
| `Corner` | CornerStyle | 角の丸み（Default/NoRound/Round/Small） |
| `LightBackColor` / `DarkBackColor` | Color | 背景色 |
| `LightForeColor` / `DarkForeColor` | Color | 文字色 |
| `ThemeMenus` | bool | メニューにテーマを適用するか |
| `AffectChildControlForeColor` | bool | 子コントロールの ForeColor を強制変更するか |
| `ResizeBorderThickness` | int | リサイズ境界の太さ |

### Glow 関連
| プロパティ | 説明 |
|------------|------|
| `AccentActive` / `AccentInactive` | アクティブ/非アクティブ時の発光色 |
| `GlowLayers` | 発光層数（0で無効） |
| `GlowStartAlphaActive` / `Inactive` | 開始アルファ値 |
| `GlowFalloffActive` / `Inactive` | 減衰量 |

---

## 4. 主なメソッド

| メソッド | 説明 |
|----------|------|
| `AttachChrome(...)` | 既存コントロールをモダンフォームに組み込む（タイトルバー、システムボタン、コンテンツなどを配線） |
| `SetGlow(...)` | 発光の色・強さを一括設定 |
| `SetGlowPreset(GlowPreset)` | FluentAzure / SoftTeal などプリセット適用 |
| `SetThemePreset(ThemePreset)` | Graphite / PaperWhite などテーマ切替 |
| `ResetChildForeColorToDefault()` | 子コントロールの文字色を既定色に戻す |

---

## 5. 利用手順
プロジェクトにこのプロジェクトのDLLまはたプロジェクトを参照設定する。


### (1) 継承する
```csharp
public partial class MainForm : ModernForms
{
    public MainForm()
    {
        InitializeComponent();

        // テーマプリセット適用
        SetThemePreset(ThemePreset.Graphite);
    }
}
```


### (2) デザイナー準備（フォーム上のコントロール）
- **2.1 タイトルバー（MenuStrip）
  例：menuStripTitle をフォーム上部に配置
  左端にアプリアイコン用 ToolStripLabel（または ToolStripDropDownButton）を置く
  右端に ToolStripButton ×3（最小化／最大化／閉じる）を置く（Alignment=Right 推奨）

-2.2 メイン領域（Content Host）
  例：panelContent をフォーム全面に Dock=Fill
-2.3 アイコンメニュー（任意）
  例：contextMenuAppIcon をフォームに追加（「設定」「終了」等を配置）




