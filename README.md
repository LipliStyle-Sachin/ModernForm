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
### 1. 継承する
```csharp
public partial class MainForm : ModernForms
{
    public MainForm()
    {
        InitializeComponent();
        SetThemePreset(ThemePreset.Graphite);
    }
}
```

### 2. コントロールを配置
- フォームに以下を追加します：
  - `MenuStrip`（タイトルバーに使用）  
    - 左端にアプリアイコン用 `ToolStripLabel` など
    - 右端に `ToolStripButton` ×3（最小化・最大化・閉じる）
  - `Panel`（メインコンテンツ領域、`Dock=Fill`）
  - 任意で `ContextMenuStrip`（アプリアイコンのメニューなど）


### 3. AttachChrome で配置したコントロールをセット
最低限、ここまで実施すれば適用されます。
```csharp
AttachChrome(
    this.menuStrip1,
    this.appIconItem,
    this.btnMinimize, this.btnMaximize, this.btnClose,
    this.panelContent,
    this.contextMenuIcon
);
```

### 4. テーマ切替
あとは必要に応じて外観テーマを設定します。
```csharp
SetThemePreset(ThemePreset.PaperWhite);
SetGlowPreset(GlowPreset.SoftTeal);
```
---

## 最小コード例

```csharp
public partial class MainForm : ModernForms
{
    public MainForm()
    {
        InitializeComponent();

        AttachChrome(
            this.menuStrip1,
            this.appIconItem,
            this.btnMinimize, this.btnMaximize, this.btnClose,
            this.panelContent,
            this.contextMenuIcon
        );
    }
}
```

---

## カスタム用
```csharp
public partial class MainForm : ModernForms      // ← Form ではなく ModernForm を継承
{
    public TestForm()
    {
        InitializeComponent();

        //■1. タイトルバー関連部品をデザイナーの通り用意し、このメソッドで上位に渡す。
        //この処理は必ず実行する必要がある。
        AttachChrome(
            titleBar: this.titleBar,
            appIcon: this.appIcon,
            btnMin: this.btnMin,
            btnMax: this.btnMax,
            btnClose: this.btnClose,
            contentHost: this.content,
            appIconMenu: this.ctxAppIcon              // 無ければ null 可
        );

        //以下の設定はオプション。

        //■2. アイコンの設定

        // アイコンに画像を設定せずに、以下の記述をすることでアプリケーションのアイコンを自動的に取得してくれる
        //appIcon.Image = this.Icon?.ToBitmap() ?? SystemIcons.Application.ToBitmap();

        //■3. ウインドウ外観の設定(OS追従)
        // 3-1) テーマ固定（ダーク固定にしたい場合）
        //this.Theme = ThemeMode.System;（既定）
        //this.Theme = ThemeMode.Light;
        //this.Theme = ThemeMode.Dark;

        //■4. ウインドウ外観の設定(カスタム)
        //OSの設定に関わらず、任意の色味を設定したいときに設定する。
        //細かいオプションで外観を設定できるようにしてあるが、簡単に外観を設定できるように、予め用意されたテーマもある。
        //ここでテーマを設定しておけば、他の細かい設定は不要。
        //
        //予め用意されているテーマを利用する場合は、以下のメソッドを使用
        //
        // 例：起動時に「Graphite」ダーク基調へ
        //SetThemeGraphite()

        //設定にテーマを保存しておき、切り替えるような場合は、以下のようにする。
        // 例：ユーザーの設定画面から切替
        //cmbThemePreset.SelectedIndexChanged += (_, __) =>
        //{
        //    var p = (ThemePreset)cmbThemePreset.SelectedItem;
        //    SetThemePreset(p);
        //};

        // 予め用意されているテーマを利用する場合は、以下のメソッドを使用
        //SetThemeSystemDefault();   // OS 追従（既定）
        //SetThemeGraphite();        // 落ち着いた濃グレー + Mica
        //SetThemePaperWhite();      // まっさら白基調 + 角丸
        //SetThemeOceanTeal();       // 青緑アクセント + Acrylic
        //SetThemeMidnight();        // 深い紺 + Mica + 低めのグロー
        //SetThemeAmberGlass();      // 琥珀アクセント + Acrylic
        //SetThemeSlateBlueSoft();   // 青紫アクセント（既存 Glow と親和）


        //■4. ウィンドウの外枠の外観を設定する。

        //Windowの縁取り、影、グローの設定。これらの値はデザイナーで設定！
        //AccentActive           : アクティブ時の発光色
        //AccentInactive         : 非アクティブ時の発光色
        //BorderActive           : アクティブ時の枠線の不透明度 (0-255)
        //BorderInactive         : 非アクティブ時の枠線の不透明度 (0-255)
        //GlowLayers             : グロー層数（0で無効）
        //GlowStartAlphaActive   : 開始アルファ（アクティブ）
        //GlowStartAlphaInactive : 開始アルファ（非アクティブ）
        //GlowFalloffActive      : "減衰量/層（アクティブ）
        //GlowFalloffInactive    : 減衰量/層（非アクティブ）

        //または、以下のメソッドでも設定可能。メソッドで指定の場合は、上記のプロパティ設定は無視される。
        //SetGlow(
        //    active: Color.FromArgb(42, 161, 152),
        //    inactive: Color.FromArgb(120, 42, 161, 152),
        //    borderActive: 205, borderInactive: 120,
        //    layers: 4, startActive: 55, startInactive: 28,
        //    falloffActive: 9, falloffInactive: 6
        //);

        // 予め用意されているテーマを利用する場合は、以下のメソッドを使用
        //SetGlowFluentAzure();
        //SetGlowSoftTeal();
        //SetGlowEmerald();
        //SetGlowLimePop();
        //SetGlowSunsetAmber();
        //SetGlowOrchidMagenta();
        //SetGlowIceCyan();
        //SetGlowSlateBlue();


        //■5. 詳細カスタム設定
        //各オプションの設定
        //UseGripPanels = true;        // 掴みやすい透明グリップ
        //ResizeBorderThickness = 12;  // 端のヒット幅（DPI対応）
        //FollowSystemTheme = true;    // OSダークに追従
        //UseRoundedCorners = true;    // 角丸
        //UseMica = true;              // Mica（Win11 22H2+）

        //Backdrop と角丸
        //this.Backdrop = BackdropKind.Mica;   // None / Mica / Acrylic / Tabbed / Auto
        //this.Corner = CornerStyle.Round;   // Default / NoRound / Round / RoundSmall

        //背景色と文字色
        //Light系とDark系でそれぞれ背景色と文字色を指定できる。
        //this.LightBackColor  = Color.White;
        //this.LightForeColor  = Color.Black;
        //this.DarkBackColor = Color.FromArgb(32, 32, 32);
        //this.DarkForeColor = Color.Gainsboro;


        //■6. その他メソッド
        //白に変更された文字色をリセットする
        //this.ResetChildForeColorToDefault();
    }
}
```

---

## License
MIT License

---

## Author
LipliStyle sachin (2009-2025)
