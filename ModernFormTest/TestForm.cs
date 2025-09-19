using Lps.ModernWindowKit;
using System.Drawing;

namespace Lps
{
    public partial class TestForm : ModernForms      // ← Form ではなく ModernForm を継承
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
}
