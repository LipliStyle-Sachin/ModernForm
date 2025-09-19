using Lps.ModernWindowKit;
using System.Drawing;

namespace Lps
{
    public partial class TestForm : ModernForms      // �� Form �ł͂Ȃ� ModernForm ���p��
    {
        public TestForm()
        {
            InitializeComponent();

            //��1. �^�C�g���o�[�֘A���i���f�U�C�i�[�̒ʂ�p�ӂ��A���̃��\�b�h�ŏ�ʂɓn���B
            //���̏����͕K�����s����K�v������B
            AttachChrome(
                titleBar: this.titleBar,
                appIcon: this.appIcon,
                btnMin: this.btnMin,
                btnMax: this.btnMax,
                btnClose: this.btnClose,
                contentHost: this.content,
                appIconMenu: this.ctxAppIcon              // ������� null ��
            );

            //�ȉ��̐ݒ�̓I�v�V�����B

            //��2. �A�C�R���̐ݒ�

            // �A�C�R���ɉ摜��ݒ肹���ɁA�ȉ��̋L�q�����邱�ƂŃA�v���P�[�V�����̃A�C�R���������I�Ɏ擾���Ă����
            //appIcon.Image = this.Icon?.ToBitmap() ?? SystemIcons.Application.ToBitmap();

            //��3. �E�C���h�E�O�ς̐ݒ�(OS�Ǐ])
            // 3-1) �e�[�}�Œ�i�_�[�N�Œ�ɂ������ꍇ�j
            //this.Theme = ThemeMode.System;�i����j
            //this.Theme = ThemeMode.Light;
            //this.Theme = ThemeMode.Dark;

            //��4. �E�C���h�E�O�ς̐ݒ�(�J�X�^��)
            //OS�̐ݒ�Ɋւ�炸�A�C�ӂ̐F����ݒ肵�����Ƃ��ɐݒ肷��B
            //�ׂ����I�v�V�����ŊO�ς�ݒ�ł���悤�ɂ��Ă��邪�A�ȒP�ɊO�ς�ݒ�ł���悤�ɁA�\�ߗp�ӂ��ꂽ�e�[�}������B
            //�����Ńe�[�}��ݒ肵�Ă����΁A���ׂ̍����ݒ�͕s�v�B
            //
            //�\�ߗp�ӂ���Ă���e�[�}�𗘗p����ꍇ�́A�ȉ��̃��\�b�h���g�p
            //
            // ��F�N�����ɁuGraphite�v�_�[�N���
            //SetThemeGraphite()

            //�ݒ�Ƀe�[�}��ۑ����Ă����A�؂�ւ���悤�ȏꍇ�́A�ȉ��̂悤�ɂ���B
            // ��F���[�U�[�̐ݒ��ʂ���ؑ�
            //cmbThemePreset.SelectedIndexChanged += (_, __) =>
            //{
            //    var p = (ThemePreset)cmbThemePreset.SelectedItem;
            //    SetThemePreset(p);
            //};

            // �\�ߗp�ӂ���Ă���e�[�}�𗘗p����ꍇ�́A�ȉ��̃��\�b�h���g�p
            //SetThemeSystemDefault();   // OS �Ǐ]�i����j
            //SetThemeGraphite();        // �����������Z�O���[ + Mica
            //SetThemePaperWhite();      // �܂����甒� + �p��
            //SetThemeOceanTeal();       // �΃A�N�Z���g + Acrylic
            //SetThemeMidnight();        // �[���� + Mica + ��߂̃O���[
            //SetThemeAmberGlass();      // ���߃A�N�Z���g + Acrylic
            //SetThemeSlateBlueSoft();   // ���A�N�Z���g�i���� Glow �Ɛe�a�j


            //��4. �E�B���h�E�̊O�g�̊O�ς�ݒ肷��B

            //Window�̉����A�e�A�O���[�̐ݒ�B�����̒l�̓f�U�C�i�[�Őݒ�I
            //AccentActive           : �A�N�e�B�u���̔����F
            //AccentInactive         : ��A�N�e�B�u���̔����F
            //BorderActive           : �A�N�e�B�u���̘g���̕s�����x (0-255)
            //BorderInactive         : ��A�N�e�B�u���̘g���̕s�����x (0-255)
            //GlowLayers             : �O���[�w���i0�Ŗ����j
            //GlowStartAlphaActive   : �J�n�A���t�@�i�A�N�e�B�u�j
            //GlowStartAlphaInactive : �J�n�A���t�@�i��A�N�e�B�u�j
            //GlowFalloffActive      : "������/�w�i�A�N�e�B�u�j
            //GlowFalloffInactive    : ������/�w�i��A�N�e�B�u�j

            //�܂��́A�ȉ��̃��\�b�h�ł��ݒ�\�B���\�b�h�Ŏw��̏ꍇ�́A��L�̃v���p�e�B�ݒ�͖��������B
            //SetGlow(
            //    active: Color.FromArgb(42, 161, 152),
            //    inactive: Color.FromArgb(120, 42, 161, 152),
            //    borderActive: 205, borderInactive: 120,
            //    layers: 4, startActive: 55, startInactive: 28,
            //    falloffActive: 9, falloffInactive: 6
            //);

            // �\�ߗp�ӂ���Ă���e�[�}�𗘗p����ꍇ�́A�ȉ��̃��\�b�h���g�p
            //SetGlowFluentAzure();
            //SetGlowSoftTeal();
            //SetGlowEmerald();
            //SetGlowLimePop();
            //SetGlowSunsetAmber();
            //SetGlowOrchidMagenta();
            //SetGlowIceCyan();
            //SetGlowSlateBlue();

            //��5. �ڍ׃J�X�^���ݒ�
            //�e�I�v�V�����̐ݒ�
            //UseGripPanels = true;        // �݂͂₷�������O���b�v
            //ResizeBorderThickness = 12;  // �[�̃q�b�g���iDPI�Ή��j
            //FollowSystemTheme = true;    // OS�_�[�N�ɒǏ]
            //UseRoundedCorners = true;    // �p��
            //UseMica = true;              // Mica�iWin11 22H2+�j

            //Backdrop �Ɗp��
            //this.Backdrop = BackdropKind.Mica;   // None / Mica / Acrylic / Tabbed / Auto
            //this.Corner = CornerStyle.Round;   // Default / NoRound / Round / RoundSmall

            //�w�i�F�ƕ����F
            //Light�n��Dark�n�ł��ꂼ��w�i�F�ƕ����F���w��ł���B
            //this.LightBackColor  = Color.White;
            //this.LightForeColor  = Color.Black;
            //this.DarkBackColor = Color.FromArgb(32, 32, 32);
            //this.DarkForeColor = Color.Gainsboro;

            //��6. ���̑����\�b�h
            //���ɕύX���ꂽ�����F�����Z�b�g����
            //this.ResetChildForeColorToDefault();
        }
    }
}
