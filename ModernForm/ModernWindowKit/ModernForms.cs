//=======================================================================
//  ClassName : ModernForms
//  概要      : モダンウインドウ
//  Date      : 2025/08/31
//
//  Update    :
//
//
//
//  ClalisApiSystem
//  Copyright(c) 2009-2025 LipliStyle sachin. All Rights Reserved.
//=======================================================================

using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Lps.ModernWindowKit
{
    /// <summary>
    /// すべての画面で共通利用する“モダンウィンドウ”のベースクラス。
    /// - 枠なし/ドラッグ移動/ダブルクリック最大化
    /// - リサイズ（ヒットテスト & グリップ）
    /// - 発光ボーダー（色/強さをプロパティ化）
    /// - ダークテーマ連動、角丸、Mica（Win11+）
    /// - メニュー/アイコン/システムボタンの配線は AttachChrome() で注入
    /// </summary>
    public class ModernForms : Form
    {
        #region Interop & Const

        [DllImport("dwmapi.dll")] private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")] private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int size);
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int HTLEFT = 10, HTRIGHT = 11, HTTOP = 12, HTTOPLEFT = 13, HTTOPRIGHT = 14, HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_NCRENDERING_POLICY = 2;
        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        private struct MARGINS { public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight; }
        private enum DwmWindowCornerPreference { DWMWCP_DEFAULT = 0, DWMWCP_DONOTROUND = 1, DWMWCP_ROUND = 2, DWMWCP_ROUNDSMALL = 3 }
        private enum DwmSystemBackdropType { DWMSBT_AUTO = 0, DWMSBT_NONE = 1, DWMSBT_MICA = 2, DWMSBT_ACRYLIC = 3, DWMSBT_TABBED = 4 }

        // タイトルバー用の自前ダブルクリック検出
        private long _tbLastDownTicks;
        private Point _tbLastDownScreen;

        #endregion

        #region State / Options

        /// <summary>ドラッグ・リサイズ用の当たり判定幅（DPI考慮）。</summary>
        [Browsable(true), Category("Window/Resize"), DefaultValue(12)]
        public int ResizeBorderThickness { get; set; } = 12;

        private bool _isActive;
        private readonly System.Collections.Generic.List<Panel> _grips = new();

        /// <summary>グリップパネルを使って“掴みやすさ”を底上げする。</summary>
        [Browsable(true), Category("Window/Resize"), DefaultValue(true)]
        public bool UseGripPanels { get; set; } = true;

        /// <summary>Mica（Win11 22H2+）バックドロップを使う。</summary>
        [Browsable(true), Category("Window/Appearance"), DefaultValue(true)]
        public bool UseMica { get; set; } = true;

        /// <summary>角丸を適用（Win11）。</summary>
        [Browsable(true), Category("Window/Appearance"), DefaultValue(true)]
        public bool UseRoundedCorners { get; set; } = true;

        /// <summary>OSのダークテーマに自動追従。</summary>
        [Browsable(true), Category("Window/Appearance"), DefaultValue(true)]
        public bool FollowSystemTheme { get; set; } = true;

        #endregion

        #region Glow (Designer settable)

        private Color _accentActive = Color.FromArgb(0, 120, 215);
        private Color _accentInactive = Color.FromArgb(160, 160, 160);
        private int _borderAlphaActive = 200, _borderAlphaInactive = 120;
        private int _glowLayers = 4;
        private int _glowStartAlphaActive = 60, _glowStartAlphaInactive = 30;
        private int _glowFalloffActive = 10, _glowFalloffInactive = 6;

        [Browsable(true), Category("Glow"), Description("アクティブ時の発光色")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color AccentActive { get => _accentActive; set { _accentActive = value; Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("非アクティブ時の発光色")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color AccentInactive { get => _accentInactive; set { _accentInactive = value; Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("外周線アルファ（アクティブ）"), DefaultValue(200)]
        public int BorderAlphaActive { get => _borderAlphaActive; set { _borderAlphaActive = ClampAlpha(value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("外周線アルファ（非アクティブ）"), DefaultValue(120)]
        public int BorderAlphaInactive { get => _borderAlphaInactive; set { _borderAlphaInactive = ClampAlpha(value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("グロー層数（0で無効）"), DefaultValue(4)]
        public int GlowLayers { get => _glowLayers; set { _glowLayers = Math.Max(0, value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("開始アルファ（アクティブ）"), DefaultValue(60)]
        public int GlowStartAlphaActive { get => _glowStartAlphaActive; set { _glowStartAlphaActive = ClampAlpha(value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("開始アルファ（非アクティブ）"), DefaultValue(30)]
        public int GlowStartAlphaInactive { get => _glowStartAlphaInactive; set { _glowStartAlphaInactive = ClampAlpha(value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("減衰量/層（アクティブ）"), DefaultValue(10)]
        public int GlowFalloffActive { get => _glowFalloffActive; set { _glowFalloffActive = Math.Max(0, value); Invalidate(); } }

        [Browsable(true), Category("Glow"), Description("減衰量/層（非アクティブ）"), DefaultValue(6)]
        public int GlowFalloffInactive { get => _glowFalloffInactive; set { _glowFalloffInactive = Math.Max(0, value); Invalidate(); } }

        #endregion

        #region Attach points (DI)

        /// <summary>タイトルバー（MenuStrip推奨）。</summary>
        protected MenuStrip TitleBar { get; private set; }
        /// <summary>タイトルバー左端アイコン。</summary>
        protected ToolStripItem AppIcon { get; private set; }
        /// <summary>右上：最小化/最大化/閉じる。</summary>
        protected ToolStripButton BtnMin { get; private set; }
        protected ToolStripButton BtnMax { get; private set; }
        protected ToolStripButton BtnClose { get; private set; }
        /// <summary>メインコンテンツ領域（ドラッグ可にする対象）。</summary>
        protected Control ContentHost { get; private set; }
        /// <summary>アプリアイコンのコンテキスト。</summary>
        protected ContextMenuStrip AppIconMenu { get; private set; }

        #endregion

        #region Lifecycle

        /// <summary>ベースの初期化。継承側は AttachChrome を呼ぶだけでOK。</summary>
        public ModernForms()
        {
            FormBorderStyle = FormBorderStyle.None;
            Padding = new Padding(1);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>DWM影など（デザイン時はスキップ）。</summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (IsDesignMode()) return;

            TryEnableDwmShadow();

            if (FollowSystemTheme) SyncThemeWithSystem();
            HookThemeChange();
        }

        /// <summary>破棄時：フック解除。</summary>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!IsDesignMode()) UnhookThemeChange();
            base.OnHandleDestroyed(e);
        }

        /// <summary>アクティブ化/非アクティブ化で枠の発光を更新。</summary>
        protected override void OnActivated(EventArgs e) { base.OnActivated(e); _isActive = true; Invalidate(); }
        protected override void OnDeactivate(EventArgs e) { base.OnDeactivate(e); _isActive = false; Invalidate(); }

        /// <summary>発光ボーダー描画。</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
            var accent = _isActive ? _accentActive : _accentInactive;

            int borderAlpha = _isActive ? _borderAlphaActive : _borderAlphaInactive;
            using (var pen = new Pen(Color.FromArgb(ClampAlpha(borderAlpha), accent), 1))
                g.DrawRectangle(pen, r);

            int layers = Math.Max(0, _glowLayers);
            int start = _isActive ? _glowStartAlphaActive : _glowStartAlphaInactive;
            int fall = _isActive ? _glowFalloffActive : _glowFalloffInactive;

            for (int i = 1; i <= layers; i++)
            {
                r.Inflate(-1, -1);
                int a = start - i * fall;
                if (a <= 0) break;
                using var p = new Pen(Color.FromArgb(ClampAlpha(a), accent), 1);
                g.DrawRectangle(p, r);
            }
        }

        /// <summary>端のヒットテスト（リサイズ）。</summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST && WindowState == FormWindowState.Normal)
            {
                int l = m.LParam.ToInt32();
                Point pt = new Point(unchecked((short)(l & 0xFFFF)), unchecked((short)((l >> 16) & 0xFFFF)));
                pt = PointToClient(pt);

                var r = ClientRectangle;
                int w = Scale(ResizeBorderThickness);

                bool left = pt.X <= w, right = pt.X >= r.Width - w, top = pt.Y <= w, bottom = pt.Y >= r.Height - w;

                if (left && top) { m.Result = (IntPtr)HTTOPLEFT; return; }
                if (right && top) { m.Result = (IntPtr)HTTOPRIGHT; return; }
                if (left && bottom) { m.Result = (IntPtr)HTBOTTOMLEFT; return; }
                if (right && bottom) { m.Result = (IntPtr)HTBOTTOMRIGHT; return; }
                if (left) { m.Result = (IntPtr)HTLEFT; return; }
                if (right) { m.Result = (IntPtr)HTRIGHT; return; }
                if (top) { m.Result = (IntPtr)HTTOP; return; }
                if (bottom) { m.Result = (IntPtr)HTBOTTOM; return; }
            }
            base.WndProc(ref m);
        }

        #endregion

        #region Public API

        /// <summary>
        /// 既存フォームの任意のコントロールを“モダンウィンドウの部品”として紐付ける。
        /// - タイトルバー：ドラッグ/ダブルクリック最大化
        /// - アイコン：クリックでコンテキスト
        /// - 右上ボタン：最小/最大/閉じる
        /// - コンテンツ：ドラッグ移動
        /// </summary>
        public void AttachChrome(
            MenuStrip titleBar,
            ToolStripItem appIcon,
            ToolStripButton btnMin, ToolStripButton btnMax, ToolStripButton btnClose,
            Control contentHost,
            ContextMenuStrip appIconMenu = null)
        {
            TitleBar = titleBar;
            AppIcon = appIcon;
            BtnMin = btnMin; BtnMax = btnMax; BtnClose = btnClose;
            ContentHost = contentHost;
            AppIconMenu = appIconMenu;

            // ドラッグ & ダブルクリック
            titleBar.MouseDown += TitleBar_MouseDown;

            // アイテム上は、システムボタンとアイコンを除きドラッグ可
            foreach (ToolStripItem it in titleBar.Items)
            {
                it.MouseDown -= TitleBar_MouseDown; // 念のため
                if (!IsExcludedFromDrag(it)) it.MouseDown += TitleBar_MouseDown;
            }

            // コンテンツ領域もドラッグ可
            contentHost.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); }
            };

            // システムボタン
            btnMin.Click += (s, e) => WindowState = FormWindowState.Minimized;
            btnMax.Click += (s, e) => ToggleMaximize();
            btnClose.Click += (s, e) => Close();

            // アイコン → コンテキスト
            if (appIcon != null)
            {
                if (appIcon is ToolStripItem tsi)
                {
                    tsi.Click += (s, e) => ShowAppMenu();
                    tsi.MouseDown += (s, e) => { if (e.Button == MouseButtons.Right) ShowAppMenu(); };
                }
            }

            // グリップ
            if (UseGripPanels) CreateGrips();
        }

        /// <summary>発光色＆強さを一括設定。</summary>
        public void SetGlow(Color active, Color inactive, int borderActive, int borderInactive, int layers, int startActive, int startInactive, int falloffActive, int falloffInactive)
        {
            AccentActive = active; AccentInactive = inactive;
            BorderAlphaActive = borderActive; BorderAlphaInactive = borderInactive;
            GlowLayers = layers; GlowStartAlphaActive = startActive; GlowStartAlphaInactive = startInactive;
            GlowFalloffActive = falloffActive; GlowFalloffInactive = falloffInactive;
        }

        // ===== Glow Presets =====
        public enum GlowPreset
        {
            FluentAzure,
            SoftTeal,
            Emerald,
            LimePop,
            SunsetAmber,
            OrchidMagenta,
            IceCyan,
            SlateBlue,
        }

        /// <summary>
        /// 事前定義された“おすすめ配色”を適用します。
        /// </summary>
        public void SetGlowPreset(GlowPreset preset)
        {
            switch (preset)
            {
                case GlowPreset.FluentAzure:
                    // #0078D7（落ち着き・Windows既定っぽい）
                    SetGlow(
                        active: Color.FromArgb(0, 120, 215),
                        inactive: Color.FromArgb(120, 0, 120, 215),
                        borderActive: 210, borderInactive: 130,
                        layers: 4, startActive: 60, startInactive: 30,
                        falloffActive: 10, falloffInactive: 6
                    );
                    break;

                case GlowPreset.SoftTeal:
                    // #2AA198（やさしい青緑）
                    SetGlow(
                        active: Color.FromArgb(42, 161, 152),
                        inactive: Color.FromArgb(120, 42, 161, 152),
                        borderActive: 205, borderInactive: 120,
                        layers: 4, startActive: 55, startInactive: 28,
                        falloffActive: 9, falloffInactive: 6
                    );
                    break;

                case GlowPreset.Emerald:
                    // #109E65（爽やか）
                    SetGlow(
                        active: Color.FromArgb(16, 158, 101),
                        inactive: Color.FromArgb(120, 16, 158, 101),
                        borderActive: 220, borderInactive: 140,
                        layers: 5, startActive: 70, startInactive: 35,
                        falloffActive: 10, falloffInactive: 7
                    );
                    break;

                case GlowPreset.LimePop:
                    // #32CD32（キビキビ）
                    SetGlow(
                        active: Color.LimeGreen,
                        inactive: Color.FromArgb(110, Color.LimeGreen),
                        borderActive: 230, borderInactive: 150,
                        layers: 6, startActive: 80, startInactive: 40,
                        falloffActive: 8, falloffInactive: 6
                    );
                    break;

                case GlowPreset.SunsetAmber:
                    // #FF8C00（暖色・目にやさしい）
                    SetGlow(
                        active: Color.FromArgb(255, 140, 0),
                        inactive: Color.FromArgb(120, 255, 140, 0),
                        borderActive: 215, borderInactive: 135,
                        layers: 4, startActive: 65, startInactive: 32,
                        falloffActive: 9, falloffInactive: 6
                    );
                    break;

                case GlowPreset.OrchidMagenta:
                    // MediumVioletRed（鮮やか）
                    SetGlow(
                        active: Color.MediumVioletRed,
                        inactive: Color.FromArgb(120, Color.MediumVioletRed),
                        borderActive: 220, borderInactive: 140,
                        layers: 5, startActive: 80, startInactive: 40,
                        falloffActive: 8, falloffInactive: 6
                    );
                    break;

                case GlowPreset.IceCyan:
                    // #00BCD4（クール・ガラスっぽい）
                    SetGlow(
                        active: Color.FromArgb(0, 188, 212),
                        inactive: Color.FromArgb(120, 0, 188, 212),
                        borderActive: 210, borderInactive: 130,
                        layers: 5, startActive: 70, startInactive: 35,
                        falloffActive: 10, falloffInactive: 7
                    );
                    break;

                case GlowPreset.SlateBlue:
                    // #6476BF（落ち着いた青紫）
                    SetGlow(
                        active: Color.FromArgb(100, 118, 191),
                        inactive: Color.FromArgb(120, 100, 118, 191),
                        borderActive: 205, borderInactive: 125,
                        layers: 4, startActive: 55, startInactive: 28,
                        falloffActive: 9, falloffInactive: 6
                    );
                    break;
            }
        }

        // 使いやすいエイリアス（任意）：SetGlowFluentAzure() など
        public void SetGlowFluentAzure() => SetGlowPreset(GlowPreset.FluentAzure);
        public void SetGlowSoftTeal() => SetGlowPreset(GlowPreset.SoftTeal);
        public void SetGlowEmerald() => SetGlowPreset(GlowPreset.Emerald);
        public void SetGlowLimePop() => SetGlowPreset(GlowPreset.LimePop);
        public void SetGlowSunsetAmber() => SetGlowPreset(GlowPreset.SunsetAmber);
        public void SetGlowOrchidMagenta() => SetGlowPreset(GlowPreset.OrchidMagenta);
        public void SetGlowIceCyan() => SetGlowPreset(GlowPreset.IceCyan);
        public void SetGlowSlateBlue() => SetGlowPreset(GlowPreset.SlateBlue);

        #endregion

        #region Internal Wiring

        /// <summary>タイトルバーの MouseDown：システムボタン／アイコン上ではドラッグしない。</summary>
        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // まず：ボタン/アイコン上は何もしない（クリック動作を優先）
            var screenPt = TitleBar.PointToScreen(e.Location);
            if (IsOnSystemItemsScreen(screenPt)) return;

            // --- 自前ダブルクリック判定 ---
            // 時間しきい値
            int dt = (int)(Environment.TickCount64 - _tbLastDownTicks);
            // 距離しきい値（SystemInformation.DoubleClickSize の半分相当を使うと誤爆しにくい）
            var dsz = SystemInformation.DoubleClickSize;
            int dx = Math.Abs(screenPt.X - _tbLastDownScreen.X);
            int dy = Math.Abs(screenPt.Y - _tbLastDownScreen.Y);
            bool isDouble = dt <= SystemInformation.DoubleClickTime &&
                            dx <= dsz.Width / 2 && dy <= dsz.Height / 2;

            // クリック時刻・座標を更新（次回判定用）
            _tbLastDownTicks = Environment.TickCount64;
            _tbLastDownScreen = screenPt;

            if (isDouble)
            {
                // 背景（＝ドラッグ可領域）でのダブルクリックのみ最大化/復元
                ToggleMaximize();
                return;
            }

            // シングルクリック：ドラッグ移動（最大化中は“復元ドラッグ”になる）
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        /// <summary>システムボタン/アイコンはドラッグ除外。</summary>
        private bool IsExcludedFromDrag(ToolStripItem it)
            => ReferenceEquals(it, BtnMin) || ReferenceEquals(it, BtnMax) || ReferenceEquals(it, BtnClose) || ReferenceEquals(it, AppIcon);

        /// <summary>最大化/復元を切替。</summary>
        private void ToggleMaximize()
        {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        /// <summary>アイコンのコンテキストメニューを表示。</summary>
        private void ShowAppMenu()
        {
            if (AppIconMenu == null || TitleBar == null || AppIcon == null) return;
            var r = AppIcon.Bounds;
            AppIconMenu.Show(TitleBar, new Point(r.Left, r.Bottom));
        }

        #endregion

        #region DWM / Theme

        /// <summary>DWM の影/枠色を軽く有効化。</summary>
        private void TryEnableDwmShadow()
        {
            try
            {
                var m = new MARGINS { cxLeftWidth = 1, cxRightWidth = 1, cyTopHeight = 1, cyBottomHeight = 1 };
                DwmExtendFrameIntoClientArea(Handle, ref m);

                int on = 2; // DWMNCRP_ENABLED
                DwmSetWindowAttribute(Handle, DWMWA_NCRENDERING_POLICY, ref on, sizeof(int));

                int borderArgb = Color.FromArgb(64, 64, 64).ToArgb();
                DwmSetWindowAttribute(Handle, DWMWA_BORDER_COLOR, ref borderArgb, sizeof(int));
            }
            catch { }
        }

        /// <summary>OS がダークか（AppsUseLightTheme=0）。</summary>
        private static bool IsSystemDark()
        {
            try
            {
                using var k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (k?.GetValue("AppsUseLightTheme") is int v) return v == 0;
            }
            catch { }
            return false;
        }

        /// <summary>OSテーマに合わせて配色/DWM属性を適用。</summary>
        private void SyncThemeWithSystem()
        {
            if (!FollowSystemTheme) return;
            ApplyTheme(IsSystemDark());
        }

        /// <summary>ダーク/ライトで背景色などを切替（必要なら継承で上書き）。</summary>
        protected virtual void ApplyTheme(bool dark)
        {
            SetImmersiveDarkMode(Handle, dark);
            if (UseRoundedCorners) SetRoundedCorners(Handle, DwmWindowCornerPreference.DWMWCP_ROUND);
            if (UseMica) SetBackdrop(Handle, DwmSystemBackdropType.DWMSBT_MICA);

            var bg = dark ? Color.FromArgb(32, 32, 32) : Color.White;
            var fg = dark ? Color.Gainsboro : Color.Black;
            BackColor = bg; ForeColor = fg;

            Invalidate();
        }

        private static void SetImmersiveDarkMode(IntPtr hWnd, bool enabled)
        {
            int on = enabled ? 1 : 0;
            if (DwmSetWindowAttribute(hWnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref on, sizeof(int)) != 0)
                DwmSetWindowAttribute(hWnd, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref on, sizeof(int));
        }
        private static void SetRoundedCorners(IntPtr hWnd, DwmWindowCornerPreference pref)
        {
            int p = (int)pref; try { DwmSetWindowAttribute(hWnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref p, sizeof(int)); } catch { }
        }
        private static void SetBackdrop(IntPtr hWnd, DwmSystemBackdropType type)
        {
            int t = (int)type; try { DwmSetWindowAttribute(hWnd, DWMWA_SYSTEMBACKDROP_TYPE, ref t, sizeof(int)); } catch { }
        }

        private void HookThemeChange() => SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        private void UnhookThemeChange() => SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        private void SystemEvents_UserPreferenceChanged(object? sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.Color)
                BeginInvoke(new Action(SyncThemeWithSystem));
        }

        #endregion

        #region Grips

        /// <summary>透明グリップを四辺/角に生成（掴みやすさUP）。</summary>
        private void CreateGrips()
        {
            if (IsDesignMode()) return;

            int edge = Scale(10);
            int corner = Scale(16);

            _grips.Add(MakeGrip(DockStyle.Left, edge, HTLEFT, Cursors.SizeWE));
            _grips.Add(MakeGrip(DockStyle.Right, edge, HTRIGHT, Cursors.SizeWE));
            _grips.Add(MakeGrip(DockStyle.Bottom, edge, HTBOTTOM, Cursors.SizeNS));

            _grips.Add(MakeCorner(new Size(corner, corner), new Point(ClientSize.Width - corner, ClientSize.Height - corner), HTBOTTOMRIGHT, Cursors.SizeNWSE));
            _grips.Add(MakeCorner(new Size(corner, corner), new Point(0, ClientSize.Height - corner), HTBOTTOMLEFT, Cursors.SizeNESW));

            SizeChanged += (s, e) => UpdateGripVisibility();
            UpdateGripVisibility();
        }

        private Panel MakeGrip(DockStyle dock, int size, int ht, Cursor cur)
        {
            var p = new Panel { Dock = dock, BackColor = Color.Transparent, TabStop = false };
            if (dock == DockStyle.Left || dock == DockStyle.Right) p.Width = size; else p.Height = size;
            p.Cursor = cur;
            p.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, ht, 0); } };
            p.MouseEnter += (s, e) => p.BackColor = Color.FromArgb(24, 0, 120, 215);
            p.MouseLeave += (s, e) => p.BackColor = Color.Transparent;
            Controls.Add(p); p.BringToFront();
            return p;
        }

        private Panel MakeCorner(Size size, Point loc, int ht, Cursor cur)
        {
            var p = new Panel { Size = size, BackColor = Color.Transparent, Cursor = cur, TabStop = false };
            p.Anchor = (loc.X == 0 ? AnchorStyles.Left : AnchorStyles.Right) | AnchorStyles.Bottom;
            p.Location = loc;
            p.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, ht, 0); } };
            Controls.Add(p); p.BringToFront();
            return p;
        }

        private void UpdateGripVisibility()
        {
            bool show = WindowState == FormWindowState.Normal;
            foreach (var g in _grips) g.Visible = show;
        }

        #endregion

        #region Utils

        /// <summary>96dpi 基準スケール。</summary>
        protected int Scale(int px) { using var g = CreateGraphics(); return (int)Math.Round(px * (g.DpiX / 96f)); }

        /// <summary>0-255 にクランプ。</summary>
        private static int ClampAlpha(int a) => a < 0 ? 0 : (a > 255 ? 255 : a);

        /// <summary>デザインモード判定。</summary>
        protected static bool IsDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try { return string.Equals(System.Diagnostics.Process.GetCurrentProcess().ProcessName, "devenv", StringComparison.OrdinalIgnoreCase); }
            catch { return false; }
        }

        /// <summary>ToolStripItem のスクリーン矩形を取得します。</summary>
        private Rectangle GetItemScreenRect(ToolStripItem it)
        {
            if (it == null || TitleBar == null) return Rectangle.Empty;
            Point scr = TitleBar.PointToScreen(it.Bounds.Location);
            return new Rectangle(scr, it.Bounds.Size);
        }

        /// <summary>スクリーン座標が [最小/最大/閉じる/アイコン] のどれか上にあるか。</summary>
        private bool IsOnSystemItemsScreen(Point screenPt)
        {
            Rectangle r(ToolStripItem it) => GetItemScreenRect(it);
            return r(BtnMin).Contains(screenPt) ||
                   r(BtnMax).Contains(screenPt) ||
                   r(BtnClose).Contains(screenPt) ||
                   r(AppIcon).Contains(screenPt);
        }



        #endregion
    }
}
