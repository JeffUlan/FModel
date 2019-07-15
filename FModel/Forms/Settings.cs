using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FModel.Properties;

namespace FModel.Forms
{
    public partial class Settings : Form
    {
        private static string _paKsPathBefore;
        private static string _outputPathBefore;
        private static string _oldLanguage;

        public Settings()
        {
            InitializeComponent();

            textBox2.Text = Properties.Settings.Default.PAKsPath;
            textBox1.Text = Properties.Settings.Default.ExtractOutput;

            checkBox1.Checked = Properties.Settings.Default.rarityNew;

            textBox6.Text = Properties.Settings.Default.challengesWatermark;
            checkBox2.Checked = Properties.Settings.Default.challengesDebug;
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                textBox6.Text = "{Bundle_Name} Generated using FModel & JohnWickParse - {Date}";
            }
            else { textBox6.Text = Properties.Settings.Default.challengesWatermark; }

            checkBox_tryToOpen.Checked = Properties.Settings.Default.tryToOpenAssets;

            //MERGER
            textBox3.Text                       = Properties.Settings.Default.mergerFileName;
            checkBoxSaveAsMergeImages.Checked   = Properties.Settings.Default.mergerImagesSaveAs;
            imgsPerRow.Value                    = Properties.Settings.Default.mergerImagesRow;

            //WATERMARK
            button1.Enabled     = Properties.Settings.Default.isWatermark;
            checkBox7.Checked   = Properties.Settings.Default.isWatermark;
            trackBar1.Enabled   = Properties.Settings.Default.isWatermark;
            trackBar2.Enabled   = Properties.Settings.Default.isWatermark;
            trackBar1.Value     = Properties.Settings.Default.wOpacity;
            trackBar2.Value     = Properties.Settings.Default.wSize;

            //FEATURED
            checkBox8.Checked = Properties.Settings.Default.loadFeaturedImage;
            if (File.Exists(Properties.Settings.Default.wFilename))
            {
                filenameLabel.Text = @"File Name: " + Path.GetFileName(Properties.Settings.Default.wFilename);

                Bitmap bmp = null;
                if (Properties.Settings.Default.loadFeaturedImage)
                {
                    bmp = new Bitmap(Properties.Settings.Default.rarityNew ? new Bitmap(Resources.wTemplateF) : new Bitmap(Resources.wTemplateFv1));
                }
                else
                {
                    bmp = new Bitmap(Properties.Settings.Default.rarityNew ? new Bitmap(Resources.wTemplate) : new Bitmap(Resources.wTemplatev1));
                }
                Graphics g = Graphics.FromImage(bmp);

                Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)Properties.Settings.Default.wOpacity / 100);
                g.DrawImage(ImageUtilities.ResizeImage(opacityImage, Properties.Settings.Default.wSize, Properties.Settings.Default.wSize), (522 - Properties.Settings.Default.wSize) / 2, (522 - Properties.Settings.Default.wSize) / 2, Properties.Settings.Default.wSize, Properties.Settings.Default.wSize);

                wPictureBox.Image = bmp;
            }

            _oldLanguage = Properties.Settings.Default.IconLanguage;
            comboBox1.SelectedIndex = comboBox1.FindStringExact(Properties.Settings.Default.IconLanguage);

            _paKsPathBefore = Properties.Settings.Default.PAKsPath;
            _outputPathBefore = Properties.Settings.Default.ExtractOutput;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            //INPUT
            Properties.Settings.Default.PAKsPath = textBox2.Text; //SET
            string paKsPathAfter = Properties.Settings.Default.PAKsPath;
            if (_paKsPathBefore != paKsPathAfter)
            {
                MessageBox.Show(@"Please, restart FModel to apply your new input path", @"Fortnite .PAK Path Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //OUTPUT
            Properties.Settings.Default.ExtractOutput = textBox1.Text; //SET
            if (!Directory.Exists(Properties.Settings.Default.ExtractOutput))
                Directory.CreateDirectory(Properties.Settings.Default.ExtractOutput);
            string outputPathAfter = Properties.Settings.Default.ExtractOutput;
            if (_outputPathBefore != outputPathAfter)
            {
                MessageBox.Show(@"Please, restart FModel to apply your new output path", @"FModel Output Path Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Properties.Settings.Default.challengesDebug     = checkBox2.Checked;
            Properties.Settings.Default.challengesWatermark = textBox6.Text;

            Properties.Settings.Default.tryToOpenAssets     = checkBox_tryToOpen.Checked;

            Properties.Settings.Default.rarityNew           = checkBox1.Checked;

            //MERGER
            Properties.Settings.Default.mergerFileName      = textBox3.Text;
            Properties.Settings.Default.mergerImagesSaveAs  = checkBoxSaveAsMergeImages.Checked;
            Properties.Settings.Default.mergerImagesRow     = Decimal.ToInt32(imgsPerRow.Value);

            //WATERMARK
            Properties.Settings.Default.isWatermark = checkBox7.Checked; 
            Properties.Settings.Default.wSize       = trackBar2.Value;
            Properties.Settings.Default.wOpacity    = trackBar1.Value;

            //FEATURED
            Properties.Settings.Default.loadFeaturedImage = checkBox8.Checked;

            //LOCRES
            Properties.Settings.Default.IconLanguage = comboBox1.SelectedItem.ToString();
            if (comboBox1.SelectedItem.ToString() != _oldLanguage)
            {
                LoadLocRes.LoadMySelectedLocRes(Properties.Settings.Default.IconLanguage);
            }

            Properties.Settings.Default.Save(); //SAVE
            Close();
        }

        #region SELECT WATERMARK
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = @"Choose your watermark";
            theDialog.Multiselect = false;
            theDialog.Filter = @"PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|DDS Files (*.dds)|*.dds|All Files (*.*)|*.*";

            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.wFilename = theDialog.FileName;
                Properties.Settings.Default.Save();
                filenameLabel.Text = @"File Name: " + Path.GetFileName(Properties.Settings.Default.wFilename);

                if (!string.IsNullOrEmpty(Properties.Settings.Default.wFilename))
                {
                    Bitmap bmp = new Bitmap(checkBox8.Checked ? Resources.wTemplateF : Resources.wTemplate);
                    Graphics g = Graphics.FromImage(bmp);

                    Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                    var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                    g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);

                    wPictureBox.Image = bmp;
                }
            }
        }
        #endregion

        #region RESIZE WATERMARK
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.wFilename))
            {
                Bitmap bmp = new Bitmap(checkBox8.Checked ? Resources.wTemplateF : Resources.wTemplate);
                Graphics g = Graphics.FromImage(bmp);

                Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);

                wPictureBox.Image = bmp;
                wPictureBox.Refresh();
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.wFilename))
            {
                Bitmap bmp = new Bitmap(checkBox8.Checked ? Resources.wTemplateF : Resources.wTemplate);
                Graphics g = Graphics.FromImage(bmp);

                Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);

                wPictureBox.Image = bmp;
                wPictureBox.Refresh();
            }
        }
        #endregion

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled     = checkBox7.Checked;
            trackBar1.Enabled   = checkBox7.Checked;
            trackBar2.Enabled   = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox8.Checked)
            {
                Bitmap bmp = checkBox1.Checked ? new Bitmap(Resources.wTemplate) : new Bitmap(Resources.wTemplatev1);
                Graphics g = Graphics.FromImage(bmp);
                if (File.Exists(Properties.Settings.Default.wFilename))
                {
                    Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                    var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                    g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);
                }
                wPictureBox.Image = bmp;
            }
            if (checkBox8.Checked)
            {
                Bitmap bmp = checkBox1.Checked ? new Bitmap(Resources.wTemplateF) : new Bitmap(Resources.wTemplateFv1);
                Graphics g = Graphics.FromImage(bmp);
                if (File.Exists(Properties.Settings.Default.wFilename))
                {
                    Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                    var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                    g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);
                }
                wPictureBox.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var assetsForm = new IconGeneratorAssets();
            if (Application.OpenForms[assetsForm.Name] == null)
            {
                assetsForm.Show();
            }
            else
            {
                Application.OpenForms[assetsForm.Name].Focus();
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                Bitmap bmp = checkBox8.Checked ? new Bitmap(Resources.wTemplateFv1) : new Bitmap(Resources.wTemplatev1);
                Graphics g = Graphics.FromImage(bmp);
                if (File.Exists(Properties.Settings.Default.wFilename))
                {
                    Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                    var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                    g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);
                }
                wPictureBox.Image = bmp;
            }
            if (checkBox1.Checked)
            {
                Bitmap bmp = checkBox8.Checked ? new Bitmap(Resources.wTemplateF) : new Bitmap(Resources.wTemplate);
                Graphics g = Graphics.FromImage(bmp);
                if (File.Exists(Properties.Settings.Default.wFilename))
                {
                    Image watermark = Image.FromFile(Properties.Settings.Default.wFilename);
                    var opacityImage = ImageUtilities.SetImageOpacity(watermark, (float)trackBar1.Value / 100);
                    g.DrawImage(ImageUtilities.ResizeImage(opacityImage, trackBar2.Value, trackBar2.Value), (522 - trackBar2.Value) / 2, (522 - trackBar2.Value) / 2, trackBar2.Value, trackBar2.Value);
                }
                wPictureBox.Image = bmp;
            }
        }
    }
}
