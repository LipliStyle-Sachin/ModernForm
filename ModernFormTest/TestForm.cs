using Lps.ModernWindowKit;

namespace Lps
{
    public partial class TestForm : ModernForms      // �� Form �ł͂Ȃ� ModernForm ���p��
    {
        public TestForm()
        {
            InitializeComponent();

            //��1. �^�C�g���o�[�֘A���i���f�U�C�i�[�̒ʂ�p�ӂ��A���̃��\�b�h�ŏ�ʂɓn���B
            AttachChrome(
                titleBar: this.titleBar,
                appIcon: this.appIcon,
                btnMin: this.btnMin,
                btnMax: this.btnMax,
                btnClose: this.btnClose,
                contentHost: this.content,
                appIconMenu: this.ctxAppIcon              // ������� null ��
            );

            //��2. �E�B���h�E�̊O�ς�ݒ肷��B

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

            // ����� SoftTeal ���w��
            SetGlowSoftTeal();

            //��3. �A�C�R���̐ݒ�

            // �A�C�R���ɉ摜��ݒ肹���ɁA�ȉ��̋L�q�����邱�ƂŃA�v���P�[�V�����̃A�C�R���������I�Ɏ擾���Ă����
            //appIcon.Image = this.Icon?.ToBitmap() ?? SystemIcons.Application.ToBitmap();
        }
    }
}
