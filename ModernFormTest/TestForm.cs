using Lps.ModernWindowKit;

namespace Lps
{
    public partial class TestForm : ModernForms      // ← Form ではなく ModernForm を継承
    {
        public TestForm()
        {
            InitializeComponent();

            //■1. タイトルバー関連部品をデザイナーの通り用意し、このメソッドで上位に渡す。
            AttachChrome(
                titleBar: this.titleBar,
                appIcon: this.appIcon,
                btnMin: this.btnMin,
                btnMax: this.btnMax,
                btnClose: this.btnClose,
                contentHost: this.content,
                appIconMenu: this.ctxAppIcon              // 無ければ null 可
            );

            //■2. ウィンドウの外観を設定する。

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

            // 今回は SoftTeal を指定
            SetGlowSoftTeal();

            //■3. アイコンの設定

            // アイコンに画像を設定せずに、以下の記述をすることでアプリケーションのアイコンを自動的に取得してくれる
            //appIcon.Image = this.Icon?.ToBitmap() ?? SystemIcons.Application.ToBitmap();
        }
    }
}
