namespace Lps
{
    partial class TestForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ctxExit = new ToolStripMenuItem();
            ctxAbout = new ToolStripMenuItem();
            ctxSep2 = new ToolStripSeparator();
            ctxOpenLogs = new ToolStripMenuItem();
            ctxSep1 = new ToolStripSeparator();
            ctxSyncNow = new ToolStripMenuItem();
            ctxSettings = new ToolStripMenuItem();
            ctxAppIcon = new ContextMenuStrip(components);
            ctxSep3 = new ToolStripSeparator();
            btnMin = new ToolStripButton();
            btnMax = new ToolStripButton();
            btnClose = new ToolStripButton();
            mHelp = new ToolStripMenuItem();
            mView = new ToolStripMenuItem();
            終了XToolStripMenuItem = new ToolStripMenuItem();
            開くOToolStripMenuItem = new ToolStripMenuItem();
            新規NToolStripMenuItem = new ToolStripMenuItem();
            mFile = new ToolStripMenuItem();
            appIcon = new ToolStripLabel();
            titleBar = new MenuStrip();
            content = new Panel();
            ctxAppIcon.SuspendLayout();
            titleBar.SuspendLayout();
            SuspendLayout();
            // 
            // ctxExit
            // 
            ctxExit.Name = "ctxExit";
            ctxExit.Size = new Size(153, 22);
            ctxExit.Text = "終了(&X)";
            // 
            // ctxAbout
            // 
            ctxAbout.Name = "ctxAbout";
            ctxAbout.Size = new Size(153, 22);
            ctxAbout.Text = "バージョン情報(&A)…";
            // 
            // ctxSep2
            // 
            ctxSep2.Name = "ctxSep2";
            ctxSep2.Size = new Size(150, 6);
            // 
            // ctxOpenLogs
            // 
            ctxOpenLogs.Name = "ctxOpenLogs";
            ctxOpenLogs.Size = new Size(153, 22);
            ctxOpenLogs.Text = "ログフォルダを開く(&L)…";
            // 
            // ctxSep1
            // 
            ctxSep1.Name = "ctxSep1";
            ctxSep1.Size = new Size(150, 6);
            // 
            // ctxSyncNow
            // 
            ctxSyncNow.Name = "ctxSyncNow";
            ctxSyncNow.Size = new Size(153, 22);
            ctxSyncNow.Text = "同期を今すぐ実行(&Y)";
            // 
            // ctxSettings
            // 
            ctxSettings.Name = "ctxSettings";
            ctxSettings.Size = new Size(153, 22);
            ctxSettings.Text = "設定(&S)…";
            // 
            // ctxAppIcon
            // 
            ctxAppIcon.Items.AddRange(new ToolStripItem[] { ctxSettings, ctxSyncNow, ctxSep1, ctxOpenLogs, ctxSep2, ctxAbout, ctxSep3, ctxExit });
            ctxAppIcon.Name = "ctxAppIcon";
            ctxAppIcon.ShowImageMargin = false;
            ctxAppIcon.Size = new Size(154, 132);
            // 
            // ctxSep3
            // 
            ctxSep3.Name = "ctxSep3";
            ctxSep3.Size = new Size(150, 6);
            // 
            // btnMin
            // 
            btnMin.Alignment = ToolStripItemAlignment.Right;
            btnMin.AutoSize = false;
            btnMin.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMin.Margin = new Padding(0, 8, 6, 8);
            btnMin.Name = "btnMin";
            btnMin.Size = new Size(44, 24);
            btnMin.Text = "—";
            // 
            // btnMax
            // 
            btnMax.Alignment = ToolStripItemAlignment.Right;
            btnMax.AutoSize = false;
            btnMax.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMax.Margin = new Padding(0, 8, 6, 8);
            btnMax.Name = "btnMax";
            btnMax.Size = new Size(44, 24);
            btnMax.Text = "▢";
            // 
            // btnClose
            // 
            btnClose.Alignment = ToolStripItemAlignment.Right;
            btnClose.AutoSize = false;
            btnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnClose.Margin = new Padding(0, 8, 6, 8);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(44, 24);
            btnClose.Text = "✕";
            // 
            // mHelp
            // 
            mHelp.Name = "mHelp";
            mHelp.Size = new Size(65, 30);
            mHelp.Text = "ヘルプ(&H)";
            // 
            // mView
            // 
            mView.Name = "mView";
            mView.Size = new Size(58, 30);
            mView.Text = "表示(&V)";
            // 
            // 終了XToolStripMenuItem
            // 
            終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
            終了XToolStripMenuItem.Size = new Size(115, 22);
            終了XToolStripMenuItem.Text = "終了(&X)";
            // 
            // 開くOToolStripMenuItem
            // 
            開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
            開くOToolStripMenuItem.Size = new Size(115, 22);
            開くOToolStripMenuItem.Text = "開く(&O)";
            // 
            // 新規NToolStripMenuItem
            // 
            新規NToolStripMenuItem.Name = "新規NToolStripMenuItem";
            新規NToolStripMenuItem.Size = new Size(115, 22);
            新規NToolStripMenuItem.Text = "新規(&N)";
            // 
            // mFile
            // 
            mFile.DropDownItems.AddRange(new ToolStripItem[] { 新規NToolStripMenuItem, 開くOToolStripMenuItem, 終了XToolStripMenuItem });
            mFile.Name = "mFile";
            mFile.Size = new Size(67, 30);
            mFile.Text = "ファイル(&F)";
            // 
            // appIcon
            // 
            appIcon.AutoSize = false;
            appIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            appIcon.Image = Properties.Resources.icon_Main;
            appIcon.ImageScaling = ToolStripItemImageScaling.None;
            appIcon.Margin = new Padding(6, 6, 8, 6);
            appIcon.Name = "appIcon";
            appIcon.Size = new Size(24, 24);
            // 
            // titleBar
            // 
            titleBar.AutoSize = false;
            titleBar.ImageScalingSize = new Size(28, 28);
            titleBar.Items.AddRange(new ToolStripItem[] { appIcon, mFile, mView, mHelp, btnClose, btnMax, btnMin });
            titleBar.Location = new Point(1, 1);
            titleBar.Name = "titleBar";
            titleBar.RenderMode = ToolStripRenderMode.Professional;
            titleBar.Size = new Size(798, 34);
            titleBar.TabIndex = 4;
            // 
            // content
            // 
            content.Dock = DockStyle.Fill;
            content.Location = new Point(1, 1);
            content.Name = "content";
            content.Padding = new Padding(12);
            content.Size = new Size(798, 448);
            content.TabIndex = 3;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(titleBar);
            Controls.Add(content);
            ForeColor = Color.Black;
            Name = "TestForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ctxAppIcon.ResumeLayout(false);
            titleBar.ResumeLayout(false);
            titleBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripMenuItem ctxExit;
        private ToolStripMenuItem ctxAbout;
        private ToolStripSeparator ctxSep2;
        private ToolStripMenuItem ctxOpenLogs;
        private ToolStripSeparator ctxSep1;
        private ToolStripMenuItem ctxSyncNow;
        private ToolStripMenuItem ctxSettings;
        private ContextMenuStrip ctxAppIcon;
        private ToolStripSeparator ctxSep3;
        private ToolStripButton btnMin;
        private ToolStripButton btnMax;
        private ToolStripButton btnClose;
        private ToolStripMenuItem mHelp;
        private ToolStripMenuItem mView;
        private ToolStripMenuItem 終了XToolStripMenuItem;
        private ToolStripMenuItem 開くOToolStripMenuItem;
        private ToolStripMenuItem 新規NToolStripMenuItem;
        private ToolStripMenuItem mFile;
        private ToolStripLabel appIcon;
        private MenuStrip titleBar;
        private Panel content;
    }
}
