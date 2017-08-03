using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsApplication1.Properties;
namespace WindowsApplication1
    {
    public partial class frmSettings : Form
        {
        public frmSettings()
            {
            InitializeComponent();
            }
        private void frmSettings_Load(object sender, EventArgs e)
            {
            if (Settings.Default.Windowed)
                {
                radioButton1.Checked = true;
                }
            else
                {
                radioButton2.Checked = true;
                }
            checkBox2.Checked = Settings.Default.CensoredPass;
            checkBox3.Checked = Settings.Default.CensoredPins;
            checkBox1.Checked = Settings.Default.CloseOrNot;
            textBox1.Text = Settings.Default.knownId;
            textBox2.Text = Settings.Default.knownPass;
            textBox3.Text = Settings.Default.knownPin;
            textBox4.Text = Settings.Default.crackingId;
            textBox5.Text = Settings.Default.crackingPass;
            textBox6.Text = Settings.Default.crackingStartPin;
            textBox7.Text = Settings.Default.crackingEndPin;
            if (!Settings.Default.strProcessName.EndsWith(".exe"))
                textBox8.Text = Settings.Default.strProcessName + ".exe";
            else
                textBox8.Text = Settings.Default.strProcessName;
            string strDirectory = Settings.Default.Directory;
            strDirectory = strDirectory.Replace("\\\\", "\\");
            textBox9.Text = strDirectory;
            }

        private void button1_Click(object sender, EventArgs e)
            {
            this.Close();
            UpdateSettings();
            }

        private void button2_Click(object sender, EventArgs e)
            {
            this.Close();
            }

        public void UpdateSettings()
            {
            Settings.Default.CensoredPass = checkBox2.Checked;
            Settings.Default.CensoredPins = checkBox3.Checked;
            Settings.Default.CloseOrNot = checkBox1.Checked;
            Settings.Default.crackingEndPin = textBox7.Text;
            Settings.Default.crackingId = textBox4.Text;
            Settings.Default.crackingPass = textBox5.Text;
            Settings.Default.crackingStartPin = textBox6.Text;
            Settings.Default.Directory = textBox9.Text;
            Settings.Default.Directory = Settings.Default.Directory.Replace("//", "////");
            Settings.Default.knownId = textBox1.Text;
            Settings.Default.knownPass = textBox2.Text;
            Settings.Default.knownPin = textBox3.Text;
            Settings.Default.strProcessName = Settings.Default.strProcessName;
            if (!textBox8.Text.EndsWith(".exe"))
                Settings.Default.strProcessName = textBox8.Text;
            else
                Settings.Default.strProcessName = textBox8.Text.Replace(".exe", "");
            if (radioButton1.Checked)
                Settings.Default.Windowed = true;
            else
                Settings.Default.Windowed = false;
            Settings.Default.Save();
            }

        private void button3_Click(object sender, EventArgs e)
            {
            MessageBox.Show(Settings.Default.strProcessName);
            }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
            {
            if (checkBox3.Checked)
                {
                textBox3.PasswordChar = '*';
                textBox6.PasswordChar = '*';
                textBox7.PasswordChar = '*';
                }
            else
                {
                textBox3.PasswordChar = textBox1.PasswordChar;
                textBox6.PasswordChar = textBox1.PasswordChar;
                textBox7.PasswordChar = textBox1.PasswordChar;
                }
            }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
            {
            if (checkBox2.Checked)
                {
                textBox2.PasswordChar = '*';
                textBox5.PasswordChar = '*';
                }
            else
                {
                textBox2.PasswordChar = textBox1.PasswordChar;
                textBox5.PasswordChar = textBox1.PasswordChar;
                }
            }

        private void SettingsReset()
            {
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox1.Checked = false;
            radioButton2.Checked = true;
            textBox1.Text = "knownId";
            textBox2.Text = "knownPass";
            textBox3.Text = "0000";
            textBox4.Text = "crackingId";
            textBox5.Text = "crackingPass";
            textBox6.Text = "0000";
            textBox7.Text = "0000";
            textBox8.Text = "GGLes .40.exe";
            }

        private void button3_Click_1(object sender, EventArgs e)
            {
            SettingsReset();
            }

        private void textBox3_Leave(object sender, EventArgs e)
            {
            try
                {
                int Value = Convert.ToInt32(textBox3.Text);
                if (textBox3.Text.Length != 4)
                    {
                    if (Value == 0)
                        {
                        textBox3.Text = "0000";
                        }
                    else if (Value > 0 && Value < 10)
                        {
                        textBox3.Text = "000" + textBox3.Text;
                        }
                    else if (Value >= 10 && Value < 100)
                        {
                        textBox3.Text = "00" + textBox3.Text;
                        }
                    else if (Value >= 100 && Value < 1000)
                        {
                        textBox3.Text = "0" + textBox3.Text;
                        }
                    }
                }
            catch
                {
                MessageBox.Show("Invalid value in text box!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void textBox6_Leave(object sender, EventArgs e)
            {
            try
                {
                int Value = Convert.ToInt32(textBox6.Text);
                if (textBox6.Text.Length != 4)
                    {
                    if (Value == 0)
                        {
                        textBox6.Text = "0000";
                        }
                    else if (Value > 0 && Value < 10)
                        {
                        textBox6.Text = "000" + textBox6.Text;
                        }
                    else if (Value >= 10 && Value < 100)
                        {
                        textBox6.Text = "00" + textBox6.Text;
                        }
                    else if (Value >= 100 && Value < 1000)
                        {
                        textBox6.Text = "0" + textBox6.Text;
                        }
                    }
                }
            catch
                {
                MessageBox.Show("Invalid value in text box!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void textBox7_Leave(object sender, EventArgs e)
            {
            try
                {
                int Value = Convert.ToInt32(textBox7.Text);
                if (textBox7.Text.Length != 4)
                    {
                    if (Value == 0)
                        {
                        textBox7.Text = "0000";
                        }
                    else if (Value > 0 && Value < 10)
                        {
                        textBox7.Text = "000" + textBox7.Text;
                        }
                    else if (Value >= 10 && Value < 100)
                        {
                        textBox7.Text = "00" + textBox7.Text;
                        }
                    else if (Value >= 100 && Value < 1000)
                        {
                        textBox7.Text = "0" + textBox7.Text;
                        }
                    }
                }
            catch
                {
                MessageBox.Show("Invalid value in text box!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }