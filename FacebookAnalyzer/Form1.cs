using OpenQA.Selenium;
using Bizness;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Resources;
using FacebookAnalyzer.Properties;
using System.Security.Policy;
using OpenQA.Selenium.Support.UI;
using System.Data.OleDb;

namespace FacebookAnalyzer
{
    public partial class Form1 : Form
    {


        public Configuration config;
        public string userNameF;
        public string passWordF;
        public Bitmap MyImage;
        public int STEP = 4;
        public bool allimages = false;
        public bool publicTargetImages = false;
        public bool screenshots = false;
        public bool profileFromUrl = false;
        public bool comments = false;
        public bool friends = false;
        Dictionary<string, string> dicoMessenger = new Dictionary<string, string>();
        public string pathToSave = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public ChromeDriver driver;
        public bool getMessenger = false;
        public bool getContactMessenger = false;
        Dictionary<string, string> dicoDestinatairesFromGrid = new Dictionary<string, string>();
        public bool selected = false;
        //public IList<string, string> listeForSearch = new List<string, string>();
        public List<string> sortedListForSearching = new System.Collections.Generic.List<string>();
        Dictionary<int, int> dicoResultsIndexSearch = new Dictionary<int, int>();
        string pathConfig = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string, string> videos = new Dictionary<string, string>();
        ChromeDriver driverMessenger;
        Rectangle resolution;
        static bool profilIsSet = false;
        OvalPictureBox oval;
        FileInfo[] fichiersJournal;
        FileInfo[] fichiersImages;
        FileInfo[] fichiersImagess;
        FileInfo[] fichiers;
        Dictionary<string, string> audios = new Dictionary<string, string>();
        static int Next;
        static int NextMessenger;
        static int Previous;
        public int STEPP = 100;
        bool ONLYSCREENSHOT = false;
        bool FASTMESSENGER = false;
        bool FASTJOURNAL = false;
        bool FASTJOURNALFORBUSINESS = false;
        bool BUSINESSMODE = false;
        public string LANGUAGESELECTED = "";

        //[DllImport("winmm.dll")]
        //private static extern long mciSendString(string command, StringBuilder retstrign, int Returnlength, IntPtr callback);
        // Define the output wav file of the recorded audio
        // Create class-level accessible variables to store the audio recorder and capturer instance
        private WaveFileWriter RecordedAudioWriter = null;
        private WasapiLoopbackCapture CaptureInstance = null;
        


        // Copyright (c) 2020 All Rights Reserved
        // </copyright>
        // <author>Frank Bastin</author>
        public Form1()
        {
            InitializeComponent();
            //dataGridViewMessenger
            //textBoxops.Select();
            GetResolutionScreen();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBox1.Width = this.Width;
            resolution = Screen.FromControl(this).WorkingArea;
            oval = new OvalPictureBox();

            oval.Anchor = System.Windows.Forms.AnchorStyles.Top;
            oval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            //oval.BackColor = Color.Black;
            //oval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            oval.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxtango.Image")));
            oval.Location = new System.Drawing.Point(pictureBoxtango.Location.X + 13, pictureBoxtango.Location.Y + 13);
            oval.Name = "pictureboxtango";
            oval.Size = new System.Drawing.Size(125, 125);
            oval.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            oval.TabIndex = 8;
            oval.TabStop = false;
            //panel22.SendToBack();
            oval.BringToFront();
            oval.Visible = true;
            pathToSave = pathToSave + @"\Facebook_Friends\" + textBoxops.Text;

            tabPageStart.Controls.Add(oval);
            dataGridViewPictures.ContextMenuStrip = contextMenuStrip3;
            dataGridViewJournal.ContextMenuStrip = contextMenuStrip1;
            flowLayoutPictures.ContextMenuStrip = contextMenuStrip4;
            dataGridView3.ContextMenuStrip = contextMenuStrip5;
            toolTip1.SetToolTip(panel24, "Sélectionnez amis, journal, images ...");
            toolTip1.SetToolTip(pictureBox35, "Sélectionnez amis, journal, images ...");

            //System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //gp.AddEllipse(0, 0, pictureBoxtango.Width - 3, pictureBoxtango.Height - 3);
            //Region rg = new Region(gp);
            //pictureBoxtango.Region = rg;


            if (!BUSINESSMODE)
            {
                tabControl1.TabPages.Remove(tabPage5);
            }


            if (backgroundWorker1 != null)
                if (backgroundWorker1.IsBusy)
                    return;

            if (backgroundWorker1 == null)
            {
                backgroundWorker1 = new BackgroundWorker();

                backgroundWorker1.WorkerReportsProgress = true;

                backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

                backgroundWorker1.WorkerSupportsCancellation = true;
            }
            else
            {
                backgroundWorker1.WorkerReportsProgress = true;

                backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

                backgroundWorker1.WorkerSupportsCancellation = true;
            }

            //progressBar1.Visible = true;
            progressBarfriends.Value = 0;
            progressBarfriends.Maximum = STEP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Dictionary<string, string> dico = new Dictionary<string, string>();
            //dico.Add("https://www.facebook.com/cybercaution/friends", "https://www.facebook.com/cybercaution/friends");

            //GetParameters();
            //return;

            if (textBoxops.Text == "")
            {
                textBoxops.BackColor = Color.Red;
                MessageBox.Show("Veuillez remplir le champ OPS");
                return;
            }
            else
                textBoxops.BackColor = Color.White;

            pathToSave +=  textBoxops.Text;

            if (checkBox1.Checked)
            {
                progressBarfriends.Visible = true;
                
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                dataGridView2.Rows.Clear();
                panelFriendss.Visible = true;

                IsANewThread("Friends");
            }
                

            if (checkBox2.Checked)
            {

                
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                panelJournalVisible.Visible = true;

                IsANewThread("Journal");
            }
                

            if (checkBox3.Checked)
            {
                //progressBarpictures.Visible = true;
                
                pictureBoxpictures.Visible = false;
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                paneImagesWaiting.Visible = true;

                IsANewThread("Pictures");
            }
                

            if (checkBox4.Checked)
            {
                
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                panelParmVisible.Visible = true;

                IsANewThread("Parametres");
            }

            if (checkBox5.Checked)
            {
                
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                panelCommentsVisible.Visible = true;

                IsANewThread("Comments");
            }

           

            

            if (checkBox6.Checked)
            {
                ONLYSCREENSHOT = true;
            }
            else
                ONLYSCREENSHOT = false;

            //if (checkBoxcomments.Checked || checkBoxfriends.Checked || checkBoxpictures.Checked)
            //{ labelanalyseencours.Visible = true; pictureBoxwaiting.Visible = true; }
            if (pictureBoxtango.Image == global::FacebookAnalyzer.Properties.Resources.target2)
                pictureBoxtango.Image = global::FacebookAnalyzer.Properties.Resources.target2;
            //pictureBoxlogofacebook.Visible = true;

            if (checkBoxBusinessPictures.Checked)
            {
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();
                IsANewThread("ImagesPictures");
            }

            if (checkBoxBusinessJournal.Checked)
            {
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();                
                panel43.Visible = true;

                IsANewThread("HomepageBusiness");
            }

            //IsANewThread();

            progressBarfriends.Value = 0;
            progressBarfriends.Maximum = STEP;

            progressBarcomments.Value = 0;
            progressBarcomments.Maximum = STEP;

            progressBarpictures.Value = 0;
            progressBarpictures.Maximum = STEP;

            panelIdentifiantsVisible.Visible = false;

            
            //pictureBoxcomments.Visible = false;
            //pictureBoxfriends.Visible = false;
            //pictureBoxpictures.Visible = false;
            //pictureBoxwaiting.Visible = false;
            
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            buttonanalyze.BackColor = Color.White;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            FillIdentifiants();
            //if(e.Result.ToString() == "Friends")
            //{
            //    //labelanalyseencours.Visible = false;
            //    //pictureBoxlogofacebook.Visible = false;
            //    //pictureBoxwaiting.Visible = false;
            //    //progressBarfriends.Value = 0;
            //    //progressBarfriends.Maximum = 0;
            //    //progressBarfriends.Visible = false;
            //    //pictureBoxfriends.Visible = true;
            //    //labelAMIS.Visible = true;
            //    //labelAMIS.Text = "AMIS : " + dataGridView2.Rows.Count;
            //    //Thread.Sleep(2500);
            //}

            //if (e.Result.ToString() == "Journal")
            //{
            //    //labelanalyseencours.Visible = false;
            //    //pictureBoxwaiting.Visible = false;
            //    //pictureBoxlogofacebook.Visible = false;
            //    //pictureBoxJournal.Visible = true;
            //    //labelJournal.Visible = true;
            //    //labelJournal.Text = "JOURNAL : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\HOMEPAGE\", ".png").Count();
            //    //Thread.Sleep(2500);
            //}

            //if (e.Result.ToString() == "Parametres")
            //{
            //    //labelanalyseencours.Visible = false;
            //    //pictureBoxlogofacebook.Visible = false;
            //    //pictureBoxwaiting.Visible = false;
            //    //pictureBoxParam.Visible = true;
            //    //labelParam.Visible = true;
            //    //labelParam.Text = "PARAMETRES : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\PARAMETERS\", ".txt").Count();

            //}

            //if (e.Result.ToString() == "Pictures")
            //{
            //    //labelanalyseencours.Visible = false;
            //    //pictureBoxwaiting.Visible = false;
            //    //pictureBoxlogofacebook.Visible = false;
            //    //progressBarpictures.Value = 0;
            //    //progressBarpictures.Maximum = 0;
            //    //progressBarpictures.Visible = false;
            //    //pictureBoxpictures.Visible = true;
            //    //labelIMAGES.Visible = true;
            //    //labelIMAGES.Text = "IMAGES : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\PICTURES\", ".png").Count();

            //}

            //if (e.Result.ToString() == "Comments")
            //{
            //    //labelanalyseencours.Visible = false;
            //    //pictureBoxwaiting.Visible = false;
            //    //pictureBoxlogofacebook.Visible = false;
            //}



            // StopProcess();
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
           
            pictureBoxwaiting.Visible = false;
        }
        [System.Runtime.InteropServices.DllImport("ole32.dll")]
        static extern void CoFreeUnusedLibraries();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {




            // while (true)
            //{
            while (!backgroundWorker1.CancellationPending)
            {

                if (getContactMessenger)
                    GetContactsMessenger();
                //else
                //if (getMessenger)
                //    Messenger(dicoDestinatairesFromGrid);
                //else
                ///////
                if (checkBox1.Checked)
                {
                    FindAllFriendsFromFacebook();
                    e.Result = "Friends";
                    //break;

                    if (backgroundWorker1.IsBusy)
                        backgroundWorker1.CancelAsync();
                }


                if (checkBox2.Checked)
                {
                    GetHomePage(dateTimePicker5.Value.Year, dateTimePicker4.Value.Year);
                    e.Result = "Journal";
                }


                if (checkBox3.Checked)
                {
                    FindAllPicturesFromFacebook(textBoxUSERNAMEFRIENDS.Text);
                    e.Result = "Pictures";
                }


                if (checkBox4.Checked)
                {
                    GetParameters();
                    e.Result = "Parameters";
                }

                if (checkBox5.Checked)
                {
                    GetAllComments(dateTimePicker1.Value.Year, dateTimePicker2.Value.Year);
                    e.Result = "Comments";
                }

                if (checkBoxBusinessFastPictures.Checked)
                {
                    GetPictures();
                    e.Result = "Pictures Business";
                }

                if (checkBoxBusinessJournal.Checked)
                {
                    GetHomePageForBusiness();
                }

                    //////////////////////////////////////
                    //else


                    //if (backgroundWorker1.CancellationPending)
                    //    {
                    //        e.Cancel = true;
                    //        //Reset();

                    //        backgroundWorker1.Dispose();
                    //        GC.Collect();

                    //        //FB : This code kill all the ressources no more read memory error
                    //        System.Windows.Forms.Application.ExitThread();
                    //        System.Windows.Forms.Application.DoEvents();
                    //        CoFreeUnusedLibraries();
                    //        this.Refresh();
                    //        break;
                    //    }
                    //}
                }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // First call, the percentage is negative to signal that UserState
            // contains the number of pages we loop on....

            string fichier = "";

            if (e.ProgressPercentage == -1)
                progressBarfriends.Maximum = Convert.ToInt32(e.UserState);
            else
                if (e.ProgressPercentage > 0)
                try
                {
                    progressBarfriends.Value = e.ProgressPercentage;
                }
                catch
                {

                }

            if (e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    int pos = (panel23.Width - labelID.Size.Width) / 2;
                    labelID.Visible = true;
                    labelID.Location = new Point(pos, labelID.Location.Y);
                    labelID.Refresh();
                }

            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
               

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                oval.Image = (Image)MyImage;
                oval.BringToFront();
                oval.Refresh();

                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }

            if (e.ProgressPercentage == -6)
            {
                ForGrid forG = ((ForGrid)e.UserState);
                Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                dataGridView2.Rows.Add(imgg, forG.Url, forG.Label, forG.Id, "");

                if (dataGridView2.Rows.Count != 0)
                {


                    string targetName = textBoxops.Text;

                    if (File.Exists(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg"))
                    {
                        Image img = Image.FromFile(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg");
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                        fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";
                    }
                    else
                    {
                        Image img = FacebookAnalyzer.Properties.Resources.anonymous;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = "anonymous";
                        fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";


                    }


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\friends.txt", true))
                    {
                        //if (File.Exists(saveFileDialog1.FileName))
                        //    File.Delete(saveFileDialog1.FileName);

                        file.Write(fichier);
                    }


                }

            }

            if (e.ProgressPercentage == -8)
            {
                ForGrid forG = ((ForGrid)e.UserState);
                Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                dataGridView1.Rows.Add(imgg, forG.Url, forG.Label, forG.Id, "");
                

                if (dataGridView1.Rows.Count != 0)
                {


                    string targetName = textBoxops.Text;

                    if (File.Exists(pathToSave + "\\PicturesProfilesFollowers\\" + forG.Label + "_" + forG.Id + ".jpg"))
                    {
                        Image img = Image.FromFile(pathToSave + "\\PicturesProfilesFollowers\\" + forG.Label + "_" + forG.Id + ".jpg");
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = img;
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfilesFollowers\\" + forG.Label + "_" + forG.Id + ".jpg";
                        fichier += dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";
                    }
                    else
                    {
                        Image img = FacebookAnalyzer.Properties.Resources.anonymous;
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = img;
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value = "anonymous";
                        fichier += dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";


                    }


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\followers.txt", true))
                    {
                        //if (File.Exists(saveFileDialog1.FileName))
                        //    File.Delete(saveFileDialog1.FileName);

                        file.Write(fichier);
                    }


                }

            }

            if (e.ProgressPercentage == -3)
            {
                progressBarcomments.Visible = false;
                pictureBoxcomments.Visible = true;


            }

            if (e.ProgressPercentage == -7)
            {

                try
                {


                    Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                    ForGrid forG = ((ForGrid)e.UserState);

                    dataGridView2.Rows.Add(imgg, forG.Url, forG.Label, forG.Id);

                    if (dataGridView2.Rows.Count != 0)
                    {


                        string targetName = textBoxops.Text;

                        if (File.Exists(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg"))
                        {
                            Image img = Image.FromFile(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg");
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                            fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";
                        }
                        else
                        {
                            Image img = FacebookAnalyzer.Properties.Resources.anonymous;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = "anonymous";
                            fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";

                        }
                    }
                }

                catch (Exception ex)//si trop d'image de profile, pas d'image
                {
                    string targetName = textBoxops.Text;

                    dataGridView2.Rows.Clear();
                    ForGrid forG = ((ForGrid)e.UserState);

                    dataGridView2.Rows.Add(null, forG.Url, forG.Label, forG.Id);

                    dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = null;
                    dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                    fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";

                }
                finally
                {
                    string targetName = textBoxops.Text;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\friends.txt", true))
                    {
                        file.Write(fichier);
                    }
                }


            }



            //Friends
            if (e.ProgressPercentage == -90)
            {
                
                pictureBoxlogofacebook.Visible = true;
                pictureBoxwaiting.Visible = true;
                
            }

           


            //// fermeture des Process par rapport au progresschange
            ///

            //Friends
            if (e.ProgressPercentage == -100)
            {
                
                pictureBoxlogofacebook.Visible = false;
                pictureBoxwaiting.Visible = false;
                progressBarfriends.Value = 0;
                progressBarfriends.Maximum = 0;
                progressBarfriends.Visible = false;
                pictureBoxfriends.Visible = true;
                labelAMIS.Visible = true;
                labelAMIS.Text = "AMIS / ABONNES : " + (dataGridView2.Rows.Count + dataGridView1.Rows.Count).ToString();
                Thread.Sleep(2500);
                panelFriendss.Visible = false;
            }

            //Journal
            if (e.ProgressPercentage == -101)
            {
                
                pictureBoxwaiting.Visible = false;
                pictureBoxlogofacebook.Visible = false;
                pictureBoxJournal.Visible = true;
                labelJournal.Visible = true;
                labelJournal.Text = "JOURNAL : " + Directory.GetFiles(pathToSave + @"\HOMEPAGE\", "*.png").Count();
                Thread.Sleep(2500);

                FillJournalView();
                panelJournalVisible.Visible = false;
            }

            //Images
            if (e.ProgressPercentage == -102)
            {
                             
                progressBarpictures.Value = 0;
                progressBarpictures.Maximum = 0;
                progressBarpictures.Visible = false;
                pictureBoxpictures.Visible = true;
                labelIMAGES.Visible = true;
                //labelIMAGES.Text = "IMAGES : " + Directory.GetFiles(pathToSave  + @"\PICTURES\", "*.jpg").Count();

                if (!ONLYSCREENSHOT)
                {
                    
                    labelIMAGES.Text = "IMAGES : " + Directory.GetFiles(pathToSave + @"\PICTURES\", "*.jpg").Count();
                    FillPicturesView();
                }
                else
                    labelIMAGES.Text = "IMAGES : " + Directory.GetFiles(pathToSave + @"\PICTURES_SCREENSHOTS\", "*.jpg").Count();

                paneImagesWaiting.Visible = false;

            }

            //Parametres
            if (e.ProgressPercentage == -103)
            {
                
                pictureBoxParam.Visible = true;
                labelParam.Visible = true;
                labelParam.Text = "PARAMETRES : " + Directory.GetFiles(pathToSave  + @"\PARAMETERS\", "*.txt").Count();
                FillParameters();
                panelParmVisible.Visible = false;
            }

            //Commentaires
            if (e.ProgressPercentage == -104)
            {
                
                panelCommentsVisible.Visible = false;
                labelCommentss.Visible = true;
                labelCommentss.Text = "COMMENTAIRES : " + Directory.GetFiles(pathToSave + @"\COMMENTS\", "*.txt").Count();

            }

            //Images for Business
            if (e.ProgressPercentage == -120)
            {

                panel46.Visible = true;

            }

            //Images for Business
            if (e.ProgressPercentage == -121)
            {

                panel46.Visible = false;
                pictureBox57.Visible = true;
                label25.Text = "IMAGES : " + Directory.GetFiles(pathToSave + @"\PICTURES\", "*.png").Count();
                FillPicturesViewForBusiness();

            }

            //Journal
            if (e.ProgressPercentage == -122)
            {

                pictureBoxwaiting.Visible = false;
                pictureBoxlogofacebook.Visible = false;
                
                
                label24.Text = "JOURNAL : " + Directory.GetFiles(pathToSave + @"\HOMEPAGE\", "*.png").Count();
                Thread.Sleep(2500);

                FillJournalViewForBusiness();
                panel43.Visible = false;
                pictureBox52.Visible = true;
            }




        }
        private void InitializeDriver()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            // progressBarfriends.Visible = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-notifications");

            driver = new ChromeDriver(driverService, chromeOptions);
            //driver.Manage().Timeouts().ImplicitWait.Add(System.TimeSpan.FromSeconds(300));
        }
        private void InitializeDriverForBusiness()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            // progressBarfriends.Visible = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-notifications");


            driver = new ChromeDriver(driverService, chromeOptions);
            driver.Manage().Window.Maximize();
        }


        //private void GetProfileInformations()
        //{

        //    string targetName = textBoxops.Text;
        //    string titrePage = "";
        //    string urlFriend = textBoxUSERNAMEFRIENDS.Text;
        //    string ID = "";
        //    //pour cacher fenetre DOS

        //    if (driver == null)
        //    {
        //        InitializeDriver();
        //        // 2. Go to the "Google" homepage
        //        driver.Navigate().GoToUrl("https://facebook.com/login");

        //        // 3. Find the username textbox (by ID) on the homepage
        //        var userNameBox = driver.FindElementById("email");

        //        // 4. Enter the text (to search for) in the textbox
        //        userNameBox.SendKeys(textBoxUSERNAME.Text);

        //        // 3. Find the username textbox (by ID) on the homepage
        //        var userpasswordBox = driver.FindElementById("pass");

        //        // 4. Enter the text (to search for) in the textbox
        //        userpasswordBox.SendKeys(textBoxPASSWORD.Text);
        //        Thread.Sleep(5000);

        //        // 5. Find the search button (by Name) on the homepage
        //        driver.FindElementById("loginbutton").Click();
        //        Thread.Sleep(2500);
        //        //searchButton.Click();

        //        //u_0_8
        //        //"menuBar']//*[@class='menuItem']"
        //        // 2. Go to the "Google" homepage
        //        driver.Navigate().GoToUrl(urlFriend);
        //        titrePage = driver.Title;
        //        Thread.Sleep(5000);
        //    }
        //    //var driverService = ChromeDriverService.CreateDefaultService();
        //    //driverService.HideCommandPromptWindow = true;
        //    // progressBarfriends.Visible = true;

        //    //var driver = new ChromeDriver(driverService, new ChromeOptions());

        //    //System.Diagnostics.Process.Start(filepath);
        //    //ChromeOptions chromeOptions = new ChromeOptions();
        //    //chromeOptions.AddArguments("--disable-notifications");
        //    System.Random rnd = new System.Random();

        //    driver.Navigate().GoToUrl(urlFriend);
        //    titrePage = driver.Title;
        //    Thread.Sleep(5000);

        //    if(profilIsSet == false)
        //    try
        //    {


        //        var image = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
        //        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
        //        if (el != null)
        //        {
        //            try
        //            {
        //                ID = el.GetAttribute("href");
        //                if (ID.Contains("profile_id="))
        //                {
        //                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
        //                    //backgroundWorkerFriends.ReportProgress(-2, ID);
        //                }
        //                IList<IWebElement> els = el.FindElements(By.TagName("img"));

        //                foreach (IWebElement ell in els)
        //                {
        //                    var linkToImage = ell.GetAttribute("src");

        //                    if (linkToImage != "")
        //                    {
        //                        try
        //                        {
        //                            using (var client = new WebClient())
        //                            {
        //                                if (!Directory.Exists(pathToSave + "\\"))
        //                                    Directory.CreateDirectory(pathToSave + "\\");

        //                                if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
        //                                {
        //                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
        //                                    Thread.Sleep(5000);


        //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
        //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

        //                                    Thread.Sleep(2000);
        //                                    profilIsSet = true;

        //                                }
        //                                else
        //                                {


        //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
        //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

        //                                    profilIsSet = true;
        //                                }


        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
        //                            return;
        //                        }


        //                    }
        //                }
        //            }
        //            catch
        //            {

        //            }
        //        }
        //        //clic sur image dans href
        //        //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
        //        //backgroundWorker1.ReportProgress(1);

        //        Thread.Sleep(5000);
        //    }
        //    catch (OpenQA.Selenium.NoSuchElementException ex)
        //    {

        //        try
        //        {
        //            var image = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
        //            IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
        //            if (el != null)
        //            {
        //                try
        //                {
        //                    ID = el.GetAttribute("href");
        //                    if (ID.Contains("profile_id="))
        //                    {
        //                        ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
        //                        //backgroundWorkerFriends.ReportProgress(-2, ID);
        //                    }
        //                }
        //                catch
        //                {

        //                }

        //                IList<IWebElement> els = el.FindElements(By.TagName("img"));
        //                foreach (IWebElement ell in els)
        //                {
        //                    var linkToImage = ell.GetAttribute("src");

        //                    if (linkToImage != "")
        //                    {
        //                        try
        //                        {
        //                            using (var client = new WebClient())
        //                            {

        //                                if (!Directory.Exists(pathToSave + "\\"))
        //                                    Directory.CreateDirectory(pathToSave + "\\");

        //                                if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
        //                                {
        //                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
        //                                    Thread.Sleep(5000);


        //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
        //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

        //                                    Thread.Sleep(2000);

        //                                }
        //                                else
        //                                {


        //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
        //                                    backgroundWorkerFriends.ReportProgress(-2, ID);


        //                                }

        //                                profilIsSet = true;
        //                            }
        //                        }
        //                        catch (Exception exx)
        //                        {
        //                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
        //                            return;
        //                        }


        //                    }
        //                }

        //            }
        //            //clic sur image dans href
        //            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
        //            //backgroundWorker1.ReportProgress(1);

        //            //Thread.Sleep(5000);

        //        }
        //        catch (OpenQA.Selenium.NoSuchElementException exx)
        //        {
        //            MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
        //            return;
        //        }


        //    }
        //}
        private void GetProfileInformations(BackgroundWorker t)
        {

            string targetName = textBoxops.Text;
            string titrePage = "";
            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            string ID = "";
            //pour cacher fenetre DOS

            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                Thread.Sleep(2500);
                //searchButton.Click();

                //u_0_8
                //"menuBar']//*[@class='menuItem']"
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl(urlFriend);
                titrePage = driver.Title;
                Thread.Sleep(5000);
            }
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;
            // progressBarfriends.Visible = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();

            driver.Navigate().GoToUrl(urlFriend);
            titrePage = driver.Title;
            Thread.Sleep(5000);

            if (profilIsSet == false)
                try
                {


                    //var image = driver.FindElementByXPath("//a[@class='_2dgj']");
                    IList<IWebElement> el = driver.FindElementsByTagName("img");
                    IList<IWebElement> svg = driver.FindElementsByTagName("svg");
                    IList<IWebElement> ids = driver.FindElementsByTagName("a");

                    //foreach(IWebElement elId in ids)
                    //{
                    //    var tmp = elId.GetAttribute("aria-label");
                    //    if(tmp != null)
                    //        if (tmp.ToLower().Contains("profil"))
                    //        {
                    //            ID = elId.GetAttribute("href");
                    //            if (ID.Contains("facebook.com/"))
                    //            {
                    //                ID = ID.Substring(ID.IndexOf("facebook.com/") + 13).Split('/')[0];
                    //                //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //            }

                    //            break;
                    //        }

                    //}

                    if (svg != null)//new look
                    {
                        try
                        {
                            foreach (IWebElement ell in svg)
                            {
                                if (ell.Size.Width == ell.Size.Height && ell.Size.Width == 132 && !isElementPresent(driver, "rq0escxv lpgh02oy tkr6xdv7 rek2kq2y"))
                                {
                                    IList<IWebElement> imgs = ell.FindElements(By.TagName("g"));
                                    IWebElement link = imgs[0];

                                    if (link.Size.Width == link.Size.Height && link.Size.Width == 132)// && link.Size.Width < 200)
                                    {
                                        var linkToImage = link.FindElement(By.TagName("image")).GetAttribute("xlink:href");

                                        if (linkToImage != "")
                                        {
                                            try
                                            {
                                                using (var client = new WebClient())
                                                {
                                                    if (!Directory.Exists(pathToSave + "\\"))
                                                        Directory.CreateDirectory(pathToSave + "\\");

                                                    if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                    {
                                                        client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        Thread.Sleep(5000);


                                                        t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        //t.ReportProgress(-2, ID);

                                                        Thread.Sleep(2000);
                                                        profilIsSet = true;

                                                    }
                                                    else
                                                    {


                                                        t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        //t.ReportProgress(-2, ID);

                                                        profilIsSet = true;
                                                    }


                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                                return;
                                            }


                                        }
                                        break;
                                    }
                                }

                                if (!profilIsSet)//si groupe
                                {
                                    if (ell.Size.Width == ell.Size.Height && ell.Size.Width == 40 && ell.GetAttribute("class") == "pzggbiyp")
                                    {
                                        IList<IWebElement> imgs = ell.FindElements(By.TagName("g"));
                                        IWebElement link = imgs[0];

                                        if (link.Size.Width == link.Size.Height && link.Size.Width == 40)// && link.Size.Width < 200)
                                        {
                                            var linkToImage = link.FindElement(By.TagName("image")).GetAttribute("xlink:href");

                                            if (linkToImage != "")
                                            {
                                                try
                                                {
                                                    using (var client = new WebClient())
                                                    {
                                                        if (!Directory.Exists(pathToSave + "\\"))
                                                            Directory.CreateDirectory(pathToSave + "\\");

                                                        if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                        {
                                                            client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            Thread.Sleep(5000);


                                                            t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            //t.ReportProgress(-2, ID);

                                                            Thread.Sleep(2000);
                                                            profilIsSet = true;

                                                        }
                                                        else
                                                        {


                                                            t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            //t.ReportProgress(-2, ID);

                                                            profilIsSet = true;
                                                        }


                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                                    return;
                                                }


                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        catch
                        {

                        }
                    }

                    if (el != null && !profilIsSet)
                    {
                        try
                        {
                            foreach (IWebElement elId in ids)
                            {
                                var tmp = elId.GetAttribute("href");
                                if (tmp != null)

                                ID = elId.GetAttribute("href");
                                if (ID.Contains("profile_id="))
                                {
                                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                                    t.ReportProgress(-2, ID);
                                    break;
                                }                                

                               
                                    
                            }




                            //ID = el[0].GetAttribute("href");
                            //if (ID.Contains("facebook.com/"))
                            //{
                            //    ID = ID.Substring(ID.IndexOf("facebook.com/") + 13).Split('/')[0];
                            //    //backgroundWorkerFriends.ReportProgress(-2, ID);
                            //}
                            IList<IWebElement> els = el[0].FindElements(By.TagName("img"));// html / body / div[1] / div / div / div[2] / div / div / div[1] / div / div[1] / div[2] / div / div / div / div / div[1] / div / div / a / div / svg / g / image

                            foreach (IWebElement ell in el)
                            {

                                if (ell.Size.Width == ell.Size.Height && ell.Size.Width >= 132 && ell.Size.Width < 180)
                                {


                                    var linkToImage = ell.GetAttribute("src");

                                    if (linkToImage != "")
                                    {
                                        try
                                        {
                                            using (var client = new WebClient())
                                            {
                                                if (!Directory.Exists(pathToSave + "\\"))
                                                    Directory.CreateDirectory(pathToSave + "\\");

                                                if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                {
                                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    Thread.Sleep(5000);


                                                    t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    //t.ReportProgress(-2, ID);

                                                    Thread.Sleep(2000);
                                                    profilIsSet = true;

                                                }
                                                else
                                                {


                                                    t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    //t.ReportProgress(-2, ID);

                                                    profilIsSet = true;
                                                }


                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                            return;
                                        }


                                    }

                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                        catch(Exception exx)
                        {

                        }
                    }
                    //clic sur image dans href
                    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //backgroundWorker1.ReportProgress(1);

                    Thread.Sleep(5000);
                }
                catch (OpenQA.Selenium.NoSuchElementException ex)
                {

                    try
                    {
                        var image = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
                        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                        if (el != null)
                        {
                            try
                            {
                                ID = el.GetAttribute("href");
                                if (ID.Contains("profile_id="))
                                {
                                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                                    //backgroundWorkerFriends.ReportProgress(-2, ID);
                                }
                            }
                            catch
                            {

                            }

                            IList<IWebElement> els = el.FindElements(By.TagName("img"));
                            foreach (IWebElement ell in els)
                            {
                                var linkToImage = ell.GetAttribute("src");

                                if (linkToImage != "")
                                {
                                    try
                                    {
                                        using (var client = new WebClient())
                                        {

                                            if (!Directory.Exists(pathToSave + "\\"))
                                                Directory.CreateDirectory(pathToSave + "\\");

                                            if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                                            {
                                                client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                                                Thread.Sleep(5000);


                                                t.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                                                t.ReportProgress(-2, ID);

                                                Thread.Sleep(2000);

                                            }
                                            else
                                            {


                                                t.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                                                t.ReportProgress(-2, ID);


                                            }

                                            profilIsSet = true;
                                        }
                                    }
                                    catch (Exception exx)
                                    {
                                        MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                        return;
                                    }


                                }
                            }

                        }
                        //clic sur image dans href
                        //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                        //backgroundWorker1.ReportProgress(1);

                        //Thread.Sleep(5000);

                    }
                    catch (OpenQA.Selenium.NoSuchElementException exx)
                    {
                        MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                        return;
                    }


                }
        }




        private void FindAllFriendsFromFacebook()
        {
            //IsANewThread();
             backgroundWorker1.ReportProgress(-90);

            
            string targetName = textBoxops.Text;
            string titrePage = "";
            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            string urlFollowers = textBoxUSERNAMEFRIENDS.Text;
            string ID = "";
            //pour cacher fenetre DOS

            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                Thread.Sleep(2500);
                //searchButton.Click();

                //u_0_8
                //"menuBar']//*[@class='menuItem']"
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl(urlFriend);
                titrePage = driver.Title;
                Thread.Sleep(5000);
            }
            
            System.Random rnd = new System.Random();

            driver.Navigate().GoToUrl(urlFriend);
            titrePage = driver.Title;
            Thread.Sleep(5000);

            if (!profilIsSet)
                GetProfileInformations(backgroundWorker1);

            try
            {
                if (!Directory.Exists(pathToSave + @"\FRIENDS\"))
                    Directory.CreateDirectory(pathToSave + @"\FRIENDS\");

                

                Thread.Sleep(5000);
            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {

                try
                {
                    

                }
                catch (OpenQA.Selenium.NoSuchElementException exx)
                {
                    MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    return;
                }


            }



            try
            {
                

                using (var client = new WebClient())
                {

                  


                }
                if (urlFriend.Contains("id="))
                {
                    urlFriend = urlFriend + "&sk=friends&source_ref=pb_friends_tl";
                }
                else
                {
                    urlFriend = urlFriend + "/friends";
                }


                driver.Navigate().GoToUrl(urlFriend);
                //driver.Manage().Window.Maximize();
                Thread.Sleep(1500);
                //_5h60 _30f


                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                long innerHeightt = (long)innerHeight;
                long scroll = (long)innerHeight;
                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                scrollHeight = scrollHeight + scroll;
                int hauteur = 450;

                IWebElement Element = driver.FindElementByXPath("//div[@class='_5h60 _30f']");
                //Element.FindElements(By.TagName("li"))[0].FindElements(By.TagName("a"))[0].GetAttribute("data-hovercard");


                //if (!Element.Text.ToLower().Contains("abonnés"))
                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                    int ii = 1;
                    while (scrollHeight >= innerHeightt)
                    {
                        //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        //Thread.Sleep(1000);

                        Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                        imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                        //Save the screenshot
                        imageScreenshott.SaveAsFile(pathToSave + @"\FRIENDS\" + "_" + ii + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                        Thread.Sleep(100);



                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(2500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        ii++;
                    }
                }
                catch
                {
                    //e.printStackTrace();
                }


                string codePage = driver.PageSource;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\FRIENDS\\Friends.html", false))
                {
                    //if (File.Exists(saveFileDialog1.FileName))
                    //    File.Delete(saveFileDialog1.FileName);

                    file.Write(codePage);
                }

              
                //string codeFriends = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                string codeFriends = driver.PageSource;
               
                //IWebElement Elementt = driver.FindElementByXPath("_5h60 _30f']");
                
                
                IList<IWebElement> els = driver.FindElementsByXPath("//li[@class='_698']");
                //if (!Element.Text.ToLower().Contains("abonnés"))
                try
                {
                    Dictionary<string, ForGrid> dicoProff = new Dictionary<string, ForGrid>();
                    Dictionary<string, string> dicoPicturess = new Dictionary<string, string>();
                    foreach (IWebElement ell in els)
                    {
                        if (ell.GetAttribute("class") != "_698")
                            continue;
                            
                            IWebElement aa = ell.FindElement(By.TagName("a"));
                        
                            //d'abord l'url et l'id
                            string urll = aa.GetAttribute("href");

                            if (!urll.Contains("profile.php?id="))
                                urll = urll.Split('?')[0];
                            else
                                urll = urll = urll.Split('&')[0];

                            if (urll.Contains("#"))
                                continue;

                            string idd = aa.GetAttribute("data-hovercard");
                            if(idd == null && ell.GetAttribute("innerHTML").Contains("data-profileid="))
                                idd = ell.GetAttribute("innerHTML").Substring(ell.GetAttribute("innerHTML").IndexOf("data-profileid=\"") + 16).Split('"')[0];
                            else
                            if (idd.Contains("data-profileid=\""))
                                idd = idd.Substring(idd.IndexOf("data-profileid=\"") + 16).Split('"')[0];
                            else
                                idd = idd.Substring(idd.IndexOf("?id=") + 4).Split('&')[0];

                            string imageProff = "";
                            string labelProff = "";

                            try
                            {
                                //maintenant l'image
                                imageProff = aa.FindElement(By.TagName("img")).GetAttribute("src");
                                labelProff = aa.FindElement(By.TagName("img")).GetAttribute("aria-label");
                            }
                            catch
                            {
                                //maintenant l'image
                                imageProff = ell.FindElement(By.TagName("img")).GetAttribute("src");
                                labelProff = ell.FindElement(By.TagName("img")).GetAttribute("aria-label");
                            }

                            ForGrid forGrid = new ForGrid();
                            forGrid.Url = urll;
                            forGrid.Id = idd;
                            forGrid.Label = labelProff;

                            if (!dicoProff.ContainsKey(imageProff))
                                dicoProff.Add(imageProff, forGrid);

                            if (!dicoPicturess.ContainsKey(imageProff))
                                dicoPicturess.Add(imageProff, idd);
                        
                        
                    }

                        if (!Directory.Exists(pathToSave + "\\PicturesProfiles"))
                            Directory.CreateDirectory(pathToSave + "\\PicturesProfiles");

                        int count = 0;
                        int j = 0;
                        backgroundWorker1.ReportProgress(-1, dicoProff.Count);

                        foreach (string file in Directory.GetFiles(pathToSave + "\\PicturesProfiles"))
                        {
                            File.Delete(file);
                        }
                        Thread.Sleep(500);

                        if(dicoProff.Count > 0)
                        foreach (string profile in dicoProff.Keys)
                        {
                            using (var client = new WebClient())
                            {


                                try

                                {

                                    client.DownloadFile(profile, pathToSave + "\\PicturesProfiles\\" + ((ForGrid)dicoProff[profile]).Label.ToString() + "_" + dicoPicturess[profile] + ".jpg");
                                    Thread.Sleep(500);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL");
                                    //return;
                                }



                            }


                            backgroundWorker1.ReportProgress(j);
                            Thread.Sleep(100);

                            ForGrid forGridd = dicoProff[profile];
                            backgroundWorker1.ReportProgress(-6, forGridd);
                            j++;
                        }




                }
                catch(Exception exx)
                {

                }

                /////////////////////////////////////////------------------------------------------------------ les followers
                ///
                if (urlFollowers.Contains("id="))
                {
                    urlFollowers = urlFollowers + "&sk=friends&source_ref=pb_friends_tl";
                }
                else
                {
                    urlFollowers = urlFollowers+ "/followers";
                }

                driver.Navigate().GoToUrl(urlFollowers);
                //driver.Manage().Window.Maximize();
                Thread.Sleep(1500);



                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                    int ii = 1;
                    while (scrollHeight >= innerHeightt)
                    {
                        //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        //Thread.Sleep(1000);

                        Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                        imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                        //Save the screenshot
                        imageScreenshott.SaveAsFile(pathToSave + @"\FRIENDS\" + "_" + ii + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                        Thread.Sleep(100);



                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(2500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        ii++;
                    }
                }
                catch
                {
                    //e.printStackTrace();
                }







                Element = driver.FindElementByXPath("//div[@class='followList']");
                //Element.FindElements(By.TagName("li"))[0].FindElements(By.TagName("a"))[0].GetAttribute("data-hovercard");
                els = Element.FindElements(By.TagName("li"));//[0].FindElements(By.TagName("a"));

                try
                {
                    Dictionary<string, ForGrid> dicoFollowers = new Dictionary<string, ForGrid>();
                    Dictionary<string, string> dicoPicturesFollowers = new Dictionary<string, string>();
                    foreach (IWebElement ell in els)
                    {
                        if (ell.GetAttribute("class") != "fbProfileBrowserListItem")
                            continue;

                        IWebElement aa = ell.FindElement(By.TagName("a"));

                        //d'abord l'url et l'id
                        string urll = aa.GetAttribute("href");

                        if (!urll.Contains("profile.php?id="))
                            urll = urll.Split('?')[0];
                        else
                            urll = urll = urll.Split('&')[0];

                        string idd = aa.GetAttribute("data-hovercard");

                        if (idd.Contains("data-profileid=\""))
                            idd = idd.Substring(idd.IndexOf("data-profileid=\"") + 16).Split('"')[0];
                        else
                            idd = idd.Substring(idd.IndexOf("?id=") + 4).Split('&')[0];

                        //maintenant l'image
                        string imageProff = aa.FindElement(By.TagName("img")).GetAttribute("src");
                        string labelProff = aa.FindElement(By.TagName("img")).GetAttribute("aria-label");

                        ForGrid forGrid = new ForGrid();
                        forGrid.Url = urll;
                        forGrid.Id = idd;
                        forGrid.Label = labelProff;

                        if (!dicoFollowers.ContainsKey(imageProff))
                            dicoFollowers.Add(imageProff, forGrid);

                        if (!dicoPicturesFollowers.ContainsKey(imageProff))
                            dicoPicturesFollowers.Add(imageProff, idd);


                    }

                    if (!Directory.Exists(pathToSave + "\\PicturesProfilesFollowers"))
                        Directory.CreateDirectory(pathToSave + "\\PicturesProfilesFollowers");

                    int count = 0;
                    int j = 0;
                    backgroundWorker1.ReportProgress(-1, 0);
                    backgroundWorker1.ReportProgress(-1, dicoFollowers.Count);

                    foreach (string file in Directory.GetFiles(pathToSave + "\\PicturesProfilesFollowers"))
                    {
                        File.Delete(file);
                    }
                    Thread.Sleep(500);

                    if(dicoFollowers.Count > 0)
                    foreach (string profile in dicoFollowers.Keys)
                    {
                        using (var client = new WebClient())
                        {


                            try

                            {

                                client.DownloadFile(profile, pathToSave + "\\PicturesProfilesFollowers\\" + ((ForGrid)dicoFollowers[profile]).Label.ToString() + "_" + dicoPicturesFollowers[profile] + ".jpg");
                                    Thread.Sleep(500);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL");
                                //return;
                            }



                        }


                        backgroundWorker1.ReportProgress(j);
                        Thread.Sleep(100);

                        ForGrid forGridd = dicoFollowers[profile];
                        backgroundWorker1.ReportProgress(-8, forGridd);
                        j++;
                    }




                }
                catch (Exception exx)
                {

                }



                //try
                //{
                //    string[] lignesFriends = codeFriends.Split(new string[] { "<li class=\"_698\">" }, StringSplitOptions.RemoveEmptyEntries);

                //    Dictionary<string, string> dicoProf = new Dictionary<string, string>();
                //    Dictionary<string, string> dicoPictures = new Dictionary<string, string>();
                //    foreach (string li in lignesFriends)
                //    {
                //        if (li.Contains("<!DOCTYPE html>") || li.StartsWith("<html"))
                //            continue;

                //        if (!li.Contains("<img class=\"_s0 _4ooo _1x2_ _1ve7 _rv img\""))
                //            continue;

                //        string tmp = li.Substring(li.IndexOf("<img class=\"_s0 _4ooo _1x2_ _1ve7 _rv img\"")).Split('>')[0];

                //        string labelProf = tmp.Substring(tmp.IndexOf("label=\"") + 7).Split('"')[0];
                //        string imageProf = tmp.Substring(tmp.IndexOf("src=\"") + 5).Split('"')[0].Replace("amp;", "");

                //        string idd = "";
                //        if (li.Contains("data-profileid=\""))
                //            idd = li.Substring(li.IndexOf("data-profileid=\"") + 16).Split('"')[0];
                //        else if (li.Contains("?id="))
                //            idd = li.Substring(li.IndexOf("?id=") + 4).Split('&')[0];

                //        if (!dicoProf.ContainsKey(imageProf))
                //            dicoProf.Add(imageProf, labelProf);

                //        if (!dicoPictures.ContainsKey(imageProf))
                //            dicoPictures.Add(imageProf, idd);
                //        //string[] lignesImage = codeFriends.Split(new string[] { "<li class=\"_698\">" }, StringSplitOptions.RemoveEmptyEntries);//<img class="_s0 _4ooo _1x2_ _1ve7 _rv img"
                //    }

                //    //maintenant on télécharge les images de profile des amis

                //    //label18.Visible = true;
                //    //backgroundWorker1.ReportProgress(-2);
                //    //Thread.Sleep(50);


                //    if (!Directory.Exists(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper() + "\\PicturesProfiles"))
                //        Directory.CreateDirectory(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper() + "\\PicturesProfiles");

                //    int count = 0;
                //    int j = 0;
                //    backgroundWorker1.ReportProgress(-1, dicoProf.Count);

                //    foreach (string file in Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper() + "\\PicturesProfiles"))
                //    {
                //        File.Delete(file);
                //    }
                //    Thread.Sleep(500);

                //    foreach (string profile in dicoProf.Keys)
                //    {
                //        using (var client = new WebClient())
                //        {


                //            try

                //            {

                //                client.DownloadFile(profile, pathToSave + "\\PicturesProfiles\\" + dicoProf[profile].ToString() + "_" + dicoPictures[profile] + ".jpg");


                //            }
                //            catch (Exception ex)
                //            {
                //                MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL");
                //                //return;
                //            }



                //        }


                //        backgroundWorker1.ReportProgress(j);
                //        Thread.Sleep(100);
                //        j++;
                //    }




                //}
                //catch
                //{

                //}



                //driver.Close();

                //string[] lignes = codeFriends.Split(new string[] { "<div class=\"fsl fwb fcb\">" }, StringSplitOptions.RemoveEmptyEntries);

                //string url = "";
                //string label = "";
                //string id = "";
                //string pathToPicture = "";

                ////progressBar1.Maximum = lignes.Count() - 1;
                //int i = 0;
                //foreach (string li in lignes)
                //{
                //    if (!li.StartsWith("<a href="))
                //        continue;

                //    url = li.Substring(li.IndexOf("<a href=\"") + 9).Split('"')[0].Replace("amp;", "");

                //    if (url == "#")
                //    {
                //        url = "désactivé";
                //        label = li.Substring(li.IndexOf("role=\"button\">") + 14).Split('<')[0];
                //    }
                //    else
                //        label = li.Substring(li.IndexOf("data-hovercard-prefer-more-content-show=\"1\">") + 44).Split('<')[0];

                //    if (label == "")
                //    {
                //        label = li.Substring(li.IndexOf("aria-owns=\"\">") + 13).Split('<')[0];//aria-owns="">
                //    }

                //    if (li.Contains("eng_tid&quot;:&quot;"))
                //    {
                //        id = li.Substring(li.IndexOf("eng_tid&quot;:&quot;") + 20).Split(new string[] { "&quot;" }, StringSplitOptions.None)[0];
                //    }


                //    if (!url.Contains("profile.php?id="))
                //        url = url.Split('?')[0];
                //    else
                //        url = url = url.Split('&')[0];

                //    //Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                //    ForGrid forGrid = new ForGrid();
                //    forGrid.Url = url;
                //    forGrid.Id = id;
                //    forGrid.Label = label;
                //    backgroundWorker1.ReportProgress(-6, forGrid);
                //    //dataGridView2.Rows.Add(imgg,url, label, id, "");



                //}


            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {
                if (isElementPresent(driver, "_n3']"))
                {
                    var imageDown = driver.FindElementByXPath("//div[@class='_n3']");

                    string codePage = (String)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerHTML;", imageDown);
                    //string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    string codeImage = codePage.Substring(codePage.IndexOf("<img src=\"") + 10).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];

                    if (!codeImage.Contains("html><html"))
                    {
                        //string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        string link = codeImage.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        link = link.Replace("\"", "").Replace("amp;", "");
                        codeImage = codeImage.Replace("\"", "").Replace("amp;", "");

                        using (var client = new WebClient())
                        {

                            if (!Directory.Exists(pathToSave + "\\PicturesProfiles"))
                                Directory.CreateDirectory(pathToSave + "\\PicturesProfiles");


                            if (File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                                File.Delete(pathToSave + "\\" + titrePage + ".jpg");

                            client.DownloadFile(link, pathToSave + "\\" + titrePage + ".jpg");
                            Thread.Sleep(5000);

                            // Sets up an image object to be displayed.
                            if (MyImage != null)
                            {
                                MyImage.Dispose();
                            }

                            backgroundWorker1.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");

                            
                        }
                    }

                    if (urlFriend.Contains("id="))
                    {
                        urlFriend = urlFriend + "&sk=friends&source_ref=pb_friends_tl";
                    }
                    else
                    {
                        urlFriend = urlFriend + "/friends";
                    }

                    driver.Navigate().GoToUrl(urlFriend);
                    //Thread.Sleep(5000);
                    //_5h60 _30f

                    while (!isElementPresent(driver))
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                        Thread.Sleep(1500);
                    }

                    string codeFriends = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    //IWebElement Element = driver.FindElementByXPath("mbm_5vf sectionHeader _4khu']").Displayed;
                    //IWebElement Element = driver.FindElementByXPath("_5h60 _30f']");

                    try
                    {
                        string[] lignesFriends = codeFriends.Split(new string[] { "<li class=\"_698\">" }, StringSplitOptions.RemoveEmptyEntries);

                        Dictionary<string, string> dicoProf = new Dictionary<string, string>();
                        Dictionary<string, string> dicoPictures = new Dictionary<string, string>();
                        foreach (string li in lignesFriends)
                        {
                            if (li.Contains("<!DOCTYPE html>") || li.StartsWith("<html"))
                                continue;
                            string tmp = li.Substring(li.IndexOf("<img class=\"_s0 _4ooo _1x2_ _1ve7 _rv img\"")).Split('>')[0];
                            string labelProf = tmp.Substring(tmp.IndexOf("label=\"") + 7).Split('"')[0];
                            string imageProf = tmp.Substring(tmp.IndexOf("src=\"") + 5).Split('"')[0].Replace("amp;", "");
                            string idd = li.Substring(li.IndexOf("data-profileid=\"") + 16).Split('"')[0];

                            if (!dicoProf.ContainsKey(imageProf))
                                dicoProf.Add(imageProf, labelProf);

                            if (!dicoPictures.ContainsKey(imageProf))
                                dicoPictures.Add(imageProf, idd);
                            //string[] lignesImage = codeFriends.Split(new string[] { "<li class=\"_698\">" }, StringSplitOptions.RemoveEmptyEntries);//<img class="_s0 _4ooo _1x2_ _1ve7 _rv img"
                        }

                        //maintenant on télécharge les images de profile des amis
                        //backgroundWorker1.ReportProgress(-2);


                        if (!Directory.Exists(pathToSave + "\\PicturesProfiles"))
                            Directory.CreateDirectory(pathToSave + "\\PicturesProfiles");

                        int count = 0;
                        int j = 0;
                        backgroundWorker1.ReportProgress(-1, dicoProf.Count);

                        foreach (string file in Directory.GetFiles(pathToSave + "\\PicturesProfiles"))
                        {
                            File.Delete(file);
                        }
                        Thread.Sleep(500);

                        foreach (string profile in dicoProf.Keys)
                        {
                            using (var client = new WebClient())
                            {

                               
                                try
                                {
                                  
                                    client.DownloadFile(profile, pathToSave + "\\PicturesProfiles\\" + dicoProf[profile].ToString() + "_" + dicoPictures[profile] + ".jpg");
                                    
                                }
                                catch
                                {
                                    MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL");
                                    //return;
                                }



                            }

                            backgroundWorker1.ReportProgress(j);
                            Thread.Sleep(100);
                            j++;

                        }




                    }
                    catch
                    {

                    }

                    //This will scroll the page till the element is found		


                    //driver.Close();

                    string[] lignes = codeFriends.Split(new string[] { "<div class=\"fsl fwb fcb\">" }, StringSplitOptions.RemoveEmptyEntries);

                    string url = "";
                    string label = "";
                    string id = "";
                    int i = 0;

                    //progressBar1.Maximum = lignes.Count() - 1;
                    foreach (string li in lignes)
                    {
                        if (!li.StartsWith("<a href="))
                            continue;

                        url = li.Substring(li.IndexOf("<a href=\"") + 9).Split('"')[0].Replace("amp;", "");

                        if (url == "#")
                        {
                            url = "désactivé";
                            label = li.Substring(li.IndexOf("role=\"button\">") + 14).Split('<')[0];
                        }
                        else
                            label = li.Substring(li.IndexOf("data-hovercard-prefer-more-content-show=\"1\">") + 44).Split('<')[0];

                        if (label == "")
                        {
                            label = li.Substring(li.IndexOf("aria-owns=\"\">") + 13).Split('<')[0];//aria-owns="">
                        }

                        if (li.Contains("eng_tid&quot;:&quot;"))
                        {
                            id = li.Substring(li.IndexOf("eng_tid&quot;:&quot;") + 20).Split(new string[] { "&quot;" }, StringSplitOptions.None)[0];
                        }

                        if (url.Contains("?fref="))
                        {
                            url = url.Split('?')[0];
                        }
                        else
                        {
                            url = url.Split('&')[0];
                        }


                        ForGrid forGrid = new ForGrid();
                        forGrid.Url = url;
                        forGrid.Id = id;
                        forGrid.Label = label;
                        backgroundWorker1.ReportProgress(-7, forGrid);

                       

                    }



                }

               

                //backgroundWorkerFriends.ReportProgress(-3);

            }
            //}
            
            backgroundWorker1.ReportProgress(-100);
            Thread.Sleep(2000);

            backgroundWorker1.CancelAsync();
            
                        

        }


        public Boolean isElementPresent(ChromeDriver driver, string path)
        {
            try
            {
                driver.FindElementByXPath(path);
                return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }

        public Boolean isElementPresentByClassName(ChromeDriver driver, string path)
        {
            try
            {
                driver.FindElementByClassName(path);
                return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }
        private void StopProcess()
        {

            //backgroundWorkerGetMessenger.CancelAsync();
            //backgroundWorkerGetMessenger.Dispose();
            //backgroundWorkerGetMessenger = null;

            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();

                }
            }
            catch
            {

            }


            GC.Collect();

            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");

            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }


            //try
            //{
            //    chromeDriverProcesses = Process.GetProcessesByName("FacebookAnalyzer");

            //    foreach (var chromeDriverProcess in chromeDriverProcesses)
            //    {
            //        chromeDriverProcess.Kill();
            //    }
            //}
            //catch
            //{

            //}

            //try
            //{
            //    chromeDriverProcesses = Process.GetProcessesByName("chrome");

            //    foreach (var chromeDriverProcess in chromeDriverProcesses)
            //    {
            //        chromeDriverProcess.Kill();
            //    }
            //}
            //catch
            //{

            //}


        }
        private void Reset()
        {

            progressBarfriends.Value = 0;
            progressBarcomments.Value = 0;
            progressBarpictures.Value = 0;
            progressBarfriends.Maximum = 0;
            progressBarcomments.Maximum = 0;
            progressBarpictures.Maximum = 0;
            progressBarfriends.Visible = false;
            progressBarcomments.Visible = false;
            progressBarpictures.Visible = false;
            pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ok;

            friends = false;
            comments = false;
            allimages = false;
            pictureBoxlogofacebook.Visible = false;
            pictureBoxwaiting.Visible = false;

            //pictureBoxcomments.Visible = false;
            //pictureBoxfriends.Visible = false;
            //pictureBoxpictures.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //labelanalyseencours.Visible = false;


        }

        public Boolean isElementPresent(ChromeDriver driver)
        {
            try
            {
                return driver.FindElementByXPath("//div[@class='mbm _5vf sectionHeader _4khu']").Displayed;
                //return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }
        public Boolean isElementPresentByID(ChromeDriver driver, string id)
        {
            try
            {
                return driver.FindElementById(id).Displayed;
                //return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }

        public Boolean isElementMessengerEndingPresent(ChromeDriver driver)
        {
            try
            {
                return driver.FindElementByXPath("//div[@class='_llj _2eu- clearfix']").Displayed;//_673w _6ynl _1_fz
                //return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {

                try
                {
                    return driver.FindElementByXPath("//div[@class='_673w _6ynl _1_fz']").Displayed;//_673w _6ynl _1_fz
                                                                                                      //return true;
                }
                catch (OpenQA.Selenium.NoSuchElementException ee)
                {
                    return false;
                }

                //return false;
            }
        }

        public Boolean isMessagePresent(ChromeDriver driver, string classe)
        {
            try
            {
                return driver.FindElementByXPath(classe).Displayed;//_673w _6ynl _1_fz
                //return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {

                return false;
            }
        }

        private void FindAllPicturesFromFacebook(string url)
        {

            //IsANewThread();
            backgroundWorker1.ReportProgress(-90);

            allimages = true;
            string targetName = textBoxops.Text.ToUpper();
            System.Random rnd = new System.Random();
            int nbreImages = 0;
            string titrePage = "";
            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            string dossierLocal = "";
            string ID = "";
            
            if(driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                Thread.Sleep(2500);


            }
            


                
                driver.Navigate().GoToUrl(urlFriend);
                titrePage = driver.Title;
                Thread.Sleep(5000);
                //backgroundWorker1.ReportProgress(-1, STEP);

                if (!profilIsSet)
                {
                    GetProfileInformations(backgroundWorker1);
                    //try
                    //{
                        
                    //    var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //    IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //    if (el != null)
                    //    {
                    //        try
                    //        {
                    //            ID = el.GetAttribute("href");
                    //            if (ID.Contains("profile_id="))
                    //            {
                    //                ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //            }
                    //        }
                    //        catch
                    //        {

                    //        }
                    //    }
                    ////clic sur image dans href
                    ////((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", imagee);
                    //    //backgroundWorker1.ReportProgress(1);


                    //    Thread.Sleep(5000);
                    //    backgroundWorkerPictures.ReportProgress(-6, ID);
                    //}
                    //catch (OpenQA.Selenium.NoSuchElementException ex)
                    //{

                    //    try
                    //    {
                    //        var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
                    //        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //        if (el != null)
                    //        {
                    //            try
                    //            {
                    //                ID = el.GetAttribute("href");
                    //                if (ID.Contains("profile_id="))
                    //                {
                    //                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                    //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //                }
                    //            }
                    //            catch
                    //            {

                    //            }
                    //        }
                    //        //clic sur image dans href
                    //        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", imagee);
                    //        backgroundWorkerPictures.ReportProgress(1);
                    //        backgroundWorkerPictures.ReportProgress(-6, ID);

                    //        Thread.Sleep(5000);

                    //    }
                    //    catch (OpenQA.Selenium.NoSuchElementException exx)
                    //    {
                    //    try
                    //    {


                    //        var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //        if (el != null)
                    //        {
                    //            try
                    //            {
                    //                ID = el.GetAttribute("href");
                    //                if (ID.Contains("profile_id="))
                    //                {
                    //                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                    //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //                }
                    //                IList<IWebElement> els = el.FindElements(By.TagName("img"));

                    //                foreach (IWebElement ell in els)
                    //                {
                    //                    var linkToImage = ell.GetAttribute("src");

                    //                    if (linkToImage != "")
                    //                    {
                    //                        try
                    //                        {
                    //                            using (var client = new WebClient())
                    //                            {
                    //                                if (!Directory.Exists(pathToSave + "\\"))
                    //                                    Directory.CreateDirectory(pathToSave + "\\");

                    //                                if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                    //                                {
                    //                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    Thread.Sleep(5000);


                    //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                    Thread.Sleep(2000);

                    //                                }
                    //                                else
                    //                                {


                    //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                    profilIsSet = true;
                    //                                }


                    //                            }
                    //                        }
                    //                        catch (Exception exxx)
                    //                        {
                    //                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //                            return;
                    //                        }


                    //                    }
                    //                }
                    //            }
                    //            catch
                    //            {

                    //            }
                    //        }
                    //        //clic sur image dans href
                    //        //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //        //backgroundWorker1.ReportProgress(1);

                    //        Thread.Sleep(5000);
                    //    }
                    //    catch (OpenQA.Selenium.NoSuchElementException exxx)
                    //    {

                    //        try
                    //        {
                    //            var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
                    //            IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //            if (el != null)
                    //            {
                    //                try
                    //                {
                    //                    ID = el.GetAttribute("href");
                    //                    if (ID.Contains("profile_id="))
                    //                    {
                    //                        ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                        //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //                    }
                    //                }
                    //                catch
                    //                {

                    //                }

                    //                IList<IWebElement> els = el.FindElements(By.TagName("img"));
                    //                foreach (IWebElement ell in els)
                    //                {
                    //                    var linkToImage = ell.GetAttribute("src");

                    //                    if (linkToImage != "")
                    //                    {
                    //                        try
                    //                        {
                    //                            using (var client = new WebClient())
                    //                            {

                    //                                if (!Directory.Exists(pathToSave + "\\"))
                    //                                    Directory.CreateDirectory(pathToSave + "\\");

                    //                                if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                    //                                {
                    //                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    Thread.Sleep(5000);


                    //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                    Thread.Sleep(2000);

                    //                                }
                    //                                else
                    //                                {


                    //                                    backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                    backgroundWorkerFriends.ReportProgress(-2, ID);


                    //                                }


                    //                            }
                    //                        }
                    //                        catch (Exception exxxx)
                    //                        {
                    //                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //                            return;
                    //                        }


                    //                    }
                    //                }

                    //            }
                    //            //clic sur image dans href
                    //            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //            //backgroundWorker1.ReportProgress(1);

                    //            //Thread.Sleep(5000);

                    //        }
                    //        catch (OpenQA.Selenium.NoSuchElementException exxxx)
                    //        {
                    //            MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //            return;
                    //        }


                    //    }
                    //}


                    //}
                }
                //else
                //{
                    //try
                    //{


                    //    var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //    IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //    if (el != null)
                    //    {
                    //        try
                    //        {
                    //            ID = el.GetAttribute("href");
                    //            if (ID.Contains("profile_id="))
                    //            {
                    //                ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //            }
                    //            IList<IWebElement> els = el.FindElements(By.TagName("img"));

                    //            foreach (IWebElement ell in els)
                    //            {
                    //                var linkToImage = ell.GetAttribute("src");

                    //                if (linkToImage != "")
                    //                {
                    //                    try
                    //                    {
                    //                        using (var client = new WebClient())
                    //                        {
                    //                            if (!Directory.Exists(pathToSave + "\\"))
                    //                                Directory.CreateDirectory(pathToSave + "\\");

                    //                            if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                    //                            {
                    //                                client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                    //                                Thread.Sleep(5000);


                    //                                backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                Thread.Sleep(2000);

                    //                            }
                    //                            else
                    //                            {


                    //                                backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                profilIsSet = true;
                    //                            }


                    //                        }
                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //                        return;
                    //                    }


                    //                }
                    //            }
                    //        }
                    //        catch
                    //        {

                    //        }
                    //    }
                    //    //clic sur image dans href
                    //    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //    //backgroundWorker1.ReportProgress(1);

                    //    Thread.Sleep(5000);
                    //}
                    //catch (OpenQA.Selenium.NoSuchElementException ex)
                    //{

                    //    try
                    //    {
                    //        var imagee = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
                    //        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                    //        if (el != null)
                    //        {
                    //            try
                    //            {
                    //                ID = el.GetAttribute("href");
                    //                if (ID.Contains("profile_id="))
                    //                {
                    //                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                    //                    //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //                }
                    //            }
                    //            catch
                    //            {

                    //            }

                    //            IList<IWebElement> els = el.FindElements(By.TagName("img"));
                    //            foreach (IWebElement ell in els)
                    //            {
                    //                var linkToImage = ell.GetAttribute("src");

                    //                if (linkToImage != "")
                    //                {
                    //                    try
                    //                    {
                    //                        using (var client = new WebClient())
                    //                        {

                    //                            if (!Directory.Exists(pathToSave + "\\"))
                    //                                Directory.CreateDirectory(pathToSave + "\\");

                    //                            if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                    //                            {
                    //                                client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                    //                                Thread.Sleep(5000);


                    //                                backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                backgroundWorkerFriends.ReportProgress(-2, ID);

                    //                                Thread.Sleep(2000);

                    //                            }
                    //                            else
                    //                            {


                    //                                backgroundWorkerFriends.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                    //                                backgroundWorkerFriends.ReportProgress(-2, ID);


                    //                            }


                    //                        }
                    //                    }
                    //                    catch (Exception exx)
                    //                    {
                    //                        MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //                        return;
                    //                    }


                    //                }
                    //            }

                    //        }
                    //        //clic sur image dans href
                    //        //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //        //backgroundWorker1.ReportProgress(1);

                    //        //Thread.Sleep(5000);

                    //    }
                    //    catch (OpenQA.Selenium.NoSuchElementException exx)
                    //    {
                    //        MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                    //        return;
                    //    }


                    //}

                //}
                

                try
                {
                    //var imageDown = driver.FindElementByXPath("clearfix fbPhotoSnowliftPopup']");
                    //string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    //string codeImage = codePage.Substring(codePage.IndexOf("<img class=\"spotlight")).Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //link = link.Replace("\"", "").Replace("amp;", "");


                    using (var client = new WebClient())
                    {
                        //try
                        //{
                        //    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg"))
                        //        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");
                        //}
                        //catch
                        //{
                        //    Process pro = new Process();
                        //    pro.StartInfo.UseShellExecute = false;
                        //    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        //    pro.StartInfo.CreateNoWindow = true;
                        //    pro.StartInfo.RedirectStandardOutput = true;
                        //    pro.StartInfo.FileName = "cmd.exe";
                        //    pro.StartInfo.Arguments = "/C del \"" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg";
                        //    pro.Start();
                        //    //Console.WriteLine(Process.StandardOutput.ReadToEnd());
                        //    pro.WaitForExit();
                        //    pro.Close();
                        //}
                        

                        //try
                        //{
                        //    client.DownloadFile(link, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");

                        //    // Sets up an image object to be displayed.
                        //    //if (MyImage != null)
                        //    //{
                        //    //    MyImage.Dispose();
                        //    //}

                        //    backgroundWorkerPictures.ReportProgress(-1, "\\" + titrePage + ".jpg");
                        //    //pictureBoxtango.SizeMode = PictureBoxSizeMode.StretchImage;
                        //    //MyImage = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");
                        //    ////pictureBox1.ClientSize = new Size(xSize, ySize);
                        //    //pictureBoxtango.Image = (Image)MyImage;
                        //    //pictureBoxtango.Refresh();
                        //    //Thread.Sleep(2000);
                        //    backgroundWorkerPictures.ReportProgress(2);
                            
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                        //    backgroundWorkerPictures.ReportProgress(-2);
                        //    //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                        //    //backgroundWorker1.CancelAsync();
                        //    return;
                        //}



                    }


                    //backgroundWorker1.ReportProgress(3);


                }
                catch (OpenQA.Selenium.NoSuchElementException ex)
                {
                    //if (isElementPresent(driver, "_n3']"))
                    //{
                    //    var imageDown = driver.FindElementByXPath("_n3']");
                    //    string codePage = (String)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerHTML;", imageDown);
                    //    //string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    //    string codeImage = codePage.Substring(codePage.IndexOf("<img src=\"") + 10).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];

                    //    if (!codeImage.Contains("html><html"))
                    //    {
                    //        //string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //        string link = codeImage.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //        link = link.Replace("\"", "").Replace("amp;", "");
                    //        codeImage = codeImage.Replace("\"", "").Replace("amp;", "");

                    //        using (var client = new WebClient())
                    //        {
                    //            try
                    //            {
                    //                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg"))
                    //                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");
                    //            }
                    //            catch
                    //            {
                    //                Process pro = new Process();
                    //                pro.StartInfo.UseShellExecute = false;
                    //                pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //                pro.StartInfo.CreateNoWindow = true;
                    //                pro.StartInfo.RedirectStandardOutput = true;
                    //                pro.StartInfo.FileName = "cmd.exe";
                    //                pro.StartInfo.Arguments = "/C del \"" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg";
                    //                pro.Start();
                    //                //Console.WriteLine(Process.StandardOutput.ReadToEnd());
                    //                pro.WaitForExit();
                    //                pro.Close();
                    //            }

                    //            client.DownloadFile(link, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");
                    //            Thread.Sleep(500);

                    //            // Sets up an image object to be displayed.
                    //            //if (MyImage != null)
                    //            //{
                    //            //    MyImage.Dispose();
                    //            //}

                    //            backgroundWorkerPictures.ReportProgress(-1, "\\" + titrePage + ".jpg");

                    //            //pictureBoxtango.SizeMode = PictureBoxSizeMode.StretchImage;
                    //            //MyImage = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + titrePage + ".jpg");
                    //            ////pictureBox1.ClientSize = new Size(xSize, ySize);
                    //            //pictureBoxtango.Image = (Image)MyImage;
                    //            backgroundWorkerPictures.ReportProgress(2);
                    //        }
                    //    }



                        //pictureBox9.Visible = true;




                 }

            //driver.Close();

            if (!Directory.Exists(pathToSave + @"\PICTURES_SCREENSHOTS"))
                Directory.CreateDirectory(pathToSave + @"\PICTURES_SCREENSHOTS");

            //maintenant les photos 
            int nbreRow = 0;
                int i = 0;
               

                //deux url sont possibles /photos_of et /photos_all
                string photos = "";
                string photosBis = "";
                string chemin = "";


                bool tout = false;
                int imgprec = 0;
                string pathToProfile = "";
                Dictionary<string, string> dicocomments = new Dictionary<string, string>();

                for (int j = 0; j < 2; j++)
                {


                    if (j == 0)
                    {
                        if (url.Contains("id="))
                        {
                            photos = "&sk=photos_of";
                        }
                        else
                            photos = "/photos_of";

                        driver.Navigate().GoToUrl(url + photos);
                        pathToProfile = url + photos;
                        titrePage = driver.Title;

                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                    long innerHeightt = (long)innerHeight;
                    long scroll = (long)innerHeight;
                    long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    scrollHeight = scrollHeight + scroll;
                    int hauteur = 450;



                    try
                    {
                        Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                        int ii = 1;
                        while (scrollHeight >= innerHeightt)
                        {
                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            //Thread.Sleep(1000);

                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\PICTURES_SCREENSHOTS\Screenshot" + "_" + ii + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);


                            //Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                            //if (newHeight.Equals(lastHeight))
                            //{
                            //    break;
                            //}
                            //lastHeight = newHeight;

                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            if ((scrollHeight - innerHeightt) < 200)
                            {
                                Thread.Sleep(5000);
                            }
                            else
                                Thread.Sleep(2500);


                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                            Thread.Sleep(2000);
                        

                            if (scrollHeight <= innerHeightt)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                                Thread.Sleep(2000);
                                scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                            }

                            scrollHeight = scrollHeight + scroll;
                            innerHeightt = innerHeightt + hauteur;
                            ii++;
                        }
                    }
                    catch
                    {
                        //e.printStackTrace();
                    }


                    //while (!isElementPresent(driver))
                    //    {
                    //        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                    //        Thread.Sleep(1500);
                    //    }


                    if (ONLYSCREENSHOT)
                    {
                        backgroundWorker1.ReportProgress(-102);
                        Thread.Sleep(2000);

                        //backgroundWorker1.ReportProgress(-3);
                        //Thread.Sleep(2000);
                        backgroundWorker1.CancelAsync();
                        allimages = false;
                        return;
                    }
                        


                        if (titrePage.Contains(") "))
                            titrePage = titrePage.Substring(titrePage.IndexOf(") ") + 2);

                        Thread.Sleep(rnd.Next(5500, 7500));
                    }
                    else
                        if (j == 1 && tout)
                    {
                        if (url.Contains("id="))
                        {
                            photosBis = "&sk=photos_all";
                        }
                        else
                            photosBis = "/photos_all";

                        driver.Navigate().GoToUrl(url + photosBis);
                        pathToProfile = url + photosBis;
                        //titrePage = driver.Title;

                        Thread.Sleep(rnd.Next(5500, 7500));
                    }
                    else
                    {
                        continue;
                    }



                    try
                    {
                        //var imageDown = driver.FindElementByXPath("tagWrapper']");//uiMediaThumb _6i9 uiMediaThumbMedium

                        if (isElementPresent(driver, "//div[@class='clearfix _1_ca']"))
                        {
                            var tmp = driver.FindElementByXPath("//div[@class='clearfix _1_ca']");
                            string tmpCode = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)tmp).WrappedDriver).PageSource;

                            string prenom = "";
                            if (tmpCode.Contains("<span class=\"_3sz\">Photos prises par "))
                                prenom = tmpCode.Substring(tmpCode.IndexOf("<span class=\"_3sz\">Photos prises par ") + 37).Split('<')[0];

                            if (tmpCode.Contains("name=\"Photos prises par " + prenom) && tmpCode.Contains("<span class=\"_3sz\">Photos de " + prenom))
                                tout = true;
                            else
                                tout = false;

                        }
                        var imageDown = driver.FindElementByXPath("//div[@class='_3i9']");
                        string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;



                        //string [] codeImages = codePage.Split(new string[] { "<div class=\"tagWrapper" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] codeImages = codePage.Split(new string[] { "<a class=\"uiMediaThumb _6i9 uiMediaThumbMedium" }, StringSplitOptions.RemoveEmptyEntries);



                        if (!Directory.Exists(pathToSave  + "\\PICTURES\\"))
                            Directory.CreateDirectory(pathToSave  + "\\PICTURES");

                        dossierLocal = pathToSave  + @"\PICTURES";

                        //backgroundWorker1.ReportProgress(-1, (codeImages.Length - 1) + imgprec);

                        int counter = 0;
                        foreach (string urll in codeImages)
                        {
                            if (!urll.Replace(" ", "").StartsWith("\"aria-label="))
                            {
                                //backgroundWorker1.ReportProgress(i);
                                i++;
                                continue;
                            }
                                

                            //if (allimagesFast && nbreImages == 5)
                            //{
                            //    nbreImages = 0;
                            //    break;
                            //}

                            string urlForD = urll.Substring(urll.IndexOf("href=\"") + 6).Split('"')[0].Replace("\"", "").Replace("amp;", "").Replace("&type=3&theater", "&type=3&theater");


                            ////////////////////////////////////////////////////////////////////////////////////////////////////////


                            try
                            {
                                //Thread.Sleep(rnd.Next(7500, 9500));
                                driver.Navigate().GoToUrl(urlForD);
                                Thread.Sleep(rnd.Next(2500, 4500));

                                var imageFinale = driver.FindElementByXPath("//div[@class='_2-sx']");
                                string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageFinale).WrappedDriver).PageSource;
                                string codeImage = codePagee.Substring(codePagee.IndexOf("<img class=\"spotlight")).Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                link = link.Replace("\"", "").Replace("amp;", "");





                                using (var client = new WebClient())
                                {
                                    //chemin = targetName + "\\" + titrePage + "\\" + titrePage + i + ".jpg";
                                    //string sc = targetName + "\\" + titrePage + "\\screenshot_" + titrePage + i + ".jpg";

                                    chemin = "image_" + i + ".jpg";
                                    string sc = "screenshot_" + i + ".jpg";

                                    client.DownloadFile(link, pathToSave + "\\PICTURES\\" + chemin);
                                    Thread.Sleep(500);
                                    //backgroundWorker1.ReportProgress(i);

                                    //Screenshot imageScreenshot = ((ITakesScreenshot)driver).GetScreenshot();
                                    ////Save the screenshot
                                    //imageScreenshot.SaveAsFile(pathToSave + @"\Facebook_Friends\" + targetName + "\\PICTURES\\" + sc, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);

                                    i++;

                                }

                                //on essaie de récupérer les identifiants des amis qui postent des commentaires

                                try
                                {
                                    var comments = driver.FindElementByXPath("//div[@class='_6iiv _6r_e']");
                                    string codeComments = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)comments).WrappedDriver).PageSource;
                                    string commentaire = codeComments.Substring(codeComments.IndexOf("_6iiv _6r_e"));
                                    string[] ligneCommentaire = commentaire.Split(new string[] { "_72vr" }, StringSplitOptions.RemoveEmptyEntries);

                                    string IDCommentaire = "";
                                    string userCommentaire = "";
                                    string commentairePost = "";
                                    string nomUrl = "";
                                    string aria = "";
                                    string urlComment = "";
                                    string photosComment = "";

                                    foreach (string l in ligneCommentaire)
                                    {
                                        if (l.StartsWith("_6iiv _6r_e"))
                                            continue;
                                        IDCommentaire = l.Substring(l.IndexOf("php?id=") + 7).Split('"')[0];
                                        string trouve = l.Substring(0, 200);


                                        if (trouve.Contains("href=\"/profile.php?id="))
                                        {
                                            photosComment = "&sk=photos";
                                            nomUrl = l.Substring(l.IndexOf("href=\"/profile.php?id=") + 22).Split('<')[0];

                                            if (nomUrl.Contains(">"))
                                            {
                                                nomUrl = nomUrl.Substring(nomUrl.IndexOf(">") + 1);
                                                urlComment = @"https://www.facebook.com/profile.php?id=" + IDCommentaire + photosComment;

                                            }
                                        }

                                        else
                                        {
                                            photosComment = "/photos";
                                            nomUrl = l.Substring(l.IndexOf("href=\"/") + 7).Split('"')[0];
                                            urlComment = @"https://www.facebook.com/" + nomUrl + photosComment;
                                        }






                                    }
                                }
                                catch
                                {
                                    //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                                }


                                //on essaie de récupérer les identifiants des amis qui postent des likes

                                try
                                {

                                    IWebElement el = driver.FindElementByXPath("//div[@class='_6iid']");////*[@id="u_o_2"]/div[2]/div/div[1]/div/div[1]
                                    IList<IWebElement> els = el.FindElements(By.TagName("a"));

                                    string urllll = "";
                                    foreach (IWebElement aelement in els)
                                    {
                                        urllll = aelement.GetAttribute("href");
                                        break;
                                    }
                                    driver.Navigate().GoToUrl(urllll);
                                    Thread.Sleep(2500);

                                    try
                                    {
                                        el = driver.FindElementByXPath("//*[@id=\"reaction_profile_browser1\"]");
                                        IList<IWebElement> elements = el.FindElements(By.TagName("li"));
                                        foreach (IWebElement ell in elements)
                                        {
                                            IList<IWebElement> aelements = ell.FindElements(By.TagName("a"));

                                            foreach (IWebElement ael in aelements)
                                            {
                                                string nom = aelements[1].GetAttribute("title");
                                                string idd = aelements[1].GetAttribute("data-hovercard");
                                                string urlll = aelements[1].GetAttribute("href").Replace("&amp;", "&");
                                                idd = idd.Substring(idd.IndexOf("id=") + 3).Split('&')[0];

                                                if (!dicocomments.ContainsKey(idd))
                                                {
                                                    dicocomments.Add(idd, nom + ";" + urlll + ";" + idd + "\n");
                                                }
                                                break;
                                            }
                                        }

                                        Thread.Sleep(rnd.Next(2500, 3500));
                                        //driver.FindElementByClassName("_xlt _418x").Click();

                                    }
                                    catch { }

                                    //IList<IWebElement> elss = driver.FindElementsByXPath("//*[@id=\"u_fetchstream_2_5\"]/div/div[3]/ul");

                                    //foreach(IWebElement ell in els)
                                    //{
                                    //    IList<IWebElement> divs = ell.FindElements(By.ClassName("_72vr"));
                                        
                                    //    foreach(IWebElement aelement in divs)
                                    //    {
                                    //        IWebElement ae = aelement.FindElement(By.TagName("a"));
                                            
                                    //        string nom = ae.Text;
                                    //        string idee = ae.GetAttribute("data-hovercard");
                                    //        string urlel = ae.GetAttribute("href");
                                    //        idee = idee.Substring(idee.IndexOf("id=") + 3).Split('&')[0].Replace("&amp;", "");

                                    //        if (!dicocomments.ContainsKey(idee))
                                    //        {
                                    //            dicocomments.Add(idee, nom + ";" + urlel + ";" + idee + "\n");

                                    //            break;
                                    //        }
                                    //    }

                                        
                                    //}
                                    Thread.Sleep(2500);
                                    driver.Navigate().GoToUrl(urlForD);
                                    Thread.Sleep(2500);




                                    IWebElement commel = driver.FindElementByXPath("//div[@class='_6iiv _6r_e']");
                                    IList<IWebElement> divs = commel.FindElements(By.ClassName("_72vr"));

                                    foreach (IWebElement aelement in divs)
                                    {
                                        IWebElement ae = aelement.FindElement(By.TagName("a"));

                                        string nom = ae.Text;
                                        string idee = ae.GetAttribute("data-hovercard");
                                        string urlel = ae.GetAttribute("href");
                                        idee = idee.Substring(idee.IndexOf("id=") + 3).Split('&')[0].Replace("&amp;", "");

                                        if (!dicocomments.ContainsKey(idee))
                                        {
                                            dicocomments.Add(idee, nom + ";" + urlel + ";" + idee + "\n");

                                            break;
                                        }
                                    }

                                    //IList<IWebElement> els = driver.FindElementsByXPath("//*[@id=\"u_fetchstream_2_5\"]/div/div[3]/ul");

                                    //foreach (IWebElement ell in els)
                                    //{
                                    //    IList<IWebElement> divs = ell.FindElements(By.ClassName("_72vr"));

                                    //    foreach (IWebElement aelement in divs)
                                    //    {
                                    //        IWebElement ae = aelement.FindElement(By.TagName("a"));

                                    //        string nom = ae.Text;
                                    //        string idee = ae.GetAttribute("data-hovercard");
                                    //        string urlel = ae.GetAttribute("href");
                                    //        idee = idee.Substring(idee.IndexOf("id=") + 3).Split('&')[0].Replace("&amp;", "");

                                    //        if (!dicocomments.ContainsKey(idee))
                                    //        {
                                    //            dicocomments.Add(idee, nom + ";" + urlel + ";" + idee + "\n");

                                    //            break;
                                    //        }
                                    //    }

                                    //}


                                    //string codeComments = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)comments).WrappedDriver).PageSource;
                                    //string commentaire = codeComments.Substring(codeComments.IndexOf("href=\"/ufi/reaction/profile/"));
                                    //string[] ligneCommentaire = commentaire.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
                                    //string linktolike = ligneCommentaire[1].Replace("amp;", "");

                                    //driver.Navigate().GoToUrl("https://www.facebook.com/" + linktolike);
                                    //Thread.Sleep(500);




                                    //Thread.Sleep(2500);
                                    ////on recupere les identifiants
                                    //comments = driver.FindElementByXPath("_8u _42ef']");
                                    //ligneCommentaire = commentaire.Split(new string[] { "<div class=\"_5j0e fsl fwb fcb\">" }, StringSplitOptions.RemoveEmptyEntries);//<div class="_5j0e fsl fwb fcb">




                                }
                                catch
                                {
                                    //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                                }



                                try
                                {
                                    IWebElement el = driver.FindElementByXPath("//*[@id=\"reaction_profile_browser1\"]");
                                    IList<IWebElement> elements = el.FindElements(By.TagName("li"));
                                    foreach(IWebElement ell in elements)
                                    {
                                        IList <IWebElement> aelements = ell.FindElements(By.TagName("a"));

                                        foreach(IWebElement ael in aelements)
                                        {
                                            string nom = aelements[1].GetAttribute("title");
                                            string idd = aelements[1].GetAttribute("data-hovercard");
                                            string urlll = aelements[1].GetAttribute("href").Replace("&amp;", "&");
                                            idd = idd.Substring(idd.IndexOf("id=") + 3).Split('&')[0];

                                            if (!dicocomments.ContainsKey(idd))
                                            {
                                                dicocomments.Add(idd, nom + ";" + urlll + ";" + idd + "\n");
                                            }
                                            break;
                                        }
                                    }

                                    Thread.Sleep(rnd.Next(2500, 3500));
                                    //driver.FindElementByClassName("_xlt _418x").Click();

                                }
                                catch { }
                            }
                            catch (Exception ex)
                            {
                                //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                            }
                            //i++;
                            nbreImages++;
                            //backgroundWorker1.ReportProgress(nbreImages);

                        }
                        imgprec = codeImages.Length;





                    }
                    catch (Exception ex)
                    {
                        

                        //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                    }

                }

                //on écrit les resultats des ID commentaires dans un fichier
                try
                {
                    string ligne = "";
                    string urll = "";
                    string user = "";
                    string iddd = "";
                    foreach (string valeur in dicocomments.Keys)
                    {
                        user = dicocomments[valeur].Split(';')[0];
                        urll = dicocomments[valeur].Split(';')[1];
                        //iddd = dicocomments[valeur].Split(';')[2];

                        ligne += urll + ";" + user + ";" + valeur.Replace("&amp", "") + ";" + "\r\n";
                    }

                    if (File.Exists(pathToSave + chemin + "\\friendsFromComments.txt"))
                        File.Delete(pathToSave + chemin + "\\friendsFromComments.txt");

                    File.WriteAllText(dossierLocal + @"\friendsFromComments.txt", ligne);
                    dicocomments = new Dictionary<string, string>();
                }
                catch (Exception ex)
                {

                }

                ////photos de profil
                driver.Navigate().GoToUrl(pathToProfile);
                //var image = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");

                ////clic sur image dans href
                //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);

                Thread.Sleep(5000);

                bool error = false;

                try
                {
                    //var imageDown = driver.FindElementByXPath("clearfix fbPhotoSnowliftPopup']");
                    //string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    //string codeImage = codePage.Substring(codePage.IndexOf("<img class=\"spotlight")).Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //link = link.Replace("\"", "").Replace("amp;", "");



                    //using (var client = new WebClient())
                    //{
                    //    string cheminProf = targetName + "\\" + titrePage + "\\" + titrePage + "_profile" + ".jpg";
                    //    client.DownloadFile(link, pathToSave + @"\Facebook_Friends\" + cheminProf);

                    //}

                    //Thread.Sleep(5000);


                }
                catch (Exception ex)
                {
                    
                    error = true;
                try
                {
                    string ligne = "";
                    string urll = "";
                    string user = "";
                    string iddd = "";
                    foreach (string valeur in dicocomments.Keys)
                    {
                        user = dicocomments[valeur].Split(';')[0];
                        urll = dicocomments[valeur].Split(';')[1];
                        //iddd = dicocomments[valeur].Split(';')[2];

                        ligne += urll + ";" + user + ";" + valeur.Replace("&amp", "") + ";" + "\r\n";
                    }

                    if (File.Exists(pathToSave + chemin + "\\friendsFromComments.txt"))
                        File.Delete(pathToSave  + chemin + "\\friendsFromComments.txt");

                    File.WriteAllText(dossierLocal + @"\friendsFromComments.txt", ligne);
                    dicocomments = new Dictionary<string, string>();
                }
                catch (Exception exx)
                {

                }

            }
                if (error)
                {
                    try
                    {
                    //var imageDown = driver.FindElementByXPath("_n3']");
                    //string codePage = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                    //string codeImage = codePage.Substring(codePage.IndexOf("class=\"_n3\"") + 11).Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //codeImage = codeImage.Substring(codeImage.IndexOf("<img src=\"") + 10).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                    ////string link = codeImage.Substring(codeImage.IndexOf("src=") + 4).Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    ////link = link.Replace("\"", "").Replace("amp;", "");
                    //codeImage = codeImage.Replace("\"", "").Replace("amp;", "");


                    //using (var client = new WebClient())
                    //{
                    //    string cheminProf = targetName + "\\" + titrePage + "\\" + titrePage + "_profile" + ".jpg";
                    //    client.DownloadFile(codeImage, pathToSave + @"\Facebook_Friends\" + cheminProf);
                    //}

                    //Thread.Sleep(5000);

                    try
                    {
                        string ligne = "";
                        string urll = "";
                        string user = "";
                        string iddd = "";
                        foreach (string valeur in dicocomments.Keys)
                        {
                            user = dicocomments[valeur].Split(';')[0];
                            urll = dicocomments[valeur].Split(';')[1];
                            //iddd = dicocomments[valeur].Split(';')[2];

                            ligne += urll + ";" + user + ";" + valeur.Replace("&amp", "") + ";" + "\r\n";
                        }

                        if (File.Exists(pathToSave + chemin + "\\friendsFromComments.txt"))
                            File.Delete(pathToSave  + chemin + "\\friendsFromComments.txt");

                        File.WriteAllText(dossierLocal + @"\friendsFromComments.txt", ligne);
                        dicocomments = new Dictionary<string, string>();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                    catch (Exception ex)
                    {
                        //pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
                    }
                    finally
                    {
                        i = 0;
                        nbreImages = 0;
                        photos = "";
                        photosBis = "";
                        backgroundWorker1.ReportProgress(++nbreRow);
                    }
                }

                i = 0;
                nbreImages = 0;
                photos = "";
                photosBis = "";
                //backgroundWorker1.ReportProgress(++nbreRow);
                error = false;



            backgroundWorker1.ReportProgress(-102);
            Thread.Sleep(2000);
           
            //backgroundWorker1.ReportProgress(-3);
            //Thread.Sleep(2000);
            backgroundWorker1.CancelAsync();
            allimages = false;
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            try
            {
                SaveCaseClosingForm();
                
                if(LANGUAGESELECTED == "fr_FR")
                    SetDefaultLanguage();

                Process[] chromeProcesses = Process.GetProcessesByName("chrome");

                foreach (var chromeProcess in chromeProcesses)
                {
                    chromeProcess.Kill();
                }
            }
            catch
            {

            }


        }
        private void GetAllComments(int debut, int fin)
        {
           

            backgroundWorker1.ReportProgress(-90);

            if (driver == null)
            {
                InitializeDriver();
                // 1. Maximize the browser
                //driver.Manage().Window.Maximize();

                

                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);

            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;

            //driver.FindElementById("loginbutton").Click();
            ////searchButton.Click();
            //Thread.Sleep(2500);
            //pour cacher fenetre DOS
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;

            ////var driver = new ChromeDriver(driverService, new ChromeOptions());

            ////System.Diagnostics.Process.Start(filepath);
            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;
            //using (var driver = new ChromeDriver(driverService, chromeOptions))
            //{


            //u_0_8
            //"menuBar']//*[@class='menuItem']"
            // 2. Go to the "Google" homepage
            //string urlInfoUser = "/allactivity?privacy_source=your_facebook_information&entry_point=settings_yfi";
            //string urlInfoUser = "/allactivity?privacy_source=your_facebook_information&entry_point=settings_yfi";
            //driver.Navigate().GoToUrl(urlFriend + urlInfoUser);
            //titrePage = driver.Title;

            //Thread.Sleep(5000);
            if (!profilIsSet)
                GetProfileInformations(backgroundWorker1);

                try
                {


                    string targetName = textBoxops.Text;

                    //récupération des années 
                    //var years = driver.FindElementByXPath("rightColWrap']").Text;
                    //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                    //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!Directory.Exists(pathToSave + @"\COMMENTS"))
                        Directory.CreateDirectory(pathToSave + @"\COMMENTS");

                    int nbreannee = fin - debut;

                    //backgroundWorkerComments.ReportProgress(-1, liYears.Length);
                    backgroundWorkerComments.ReportProgress(-1, (nbreannee == 0 ? 1 : nbreannee));

                    //label18.Text = "Téléchargement des comments en cours ";
                    for(int y = debut; y <= fin; y++)
                    {
                        driver.Navigate().GoToUrl(urlFriend + "/timeline/" + y);
                        Thread.Sleep(5000);

                        //while (!isElementPresentForComment(driver))
                        //{


                        //    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                        //    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                        //    Thread.Sleep(2500);



                        //}

                        try
                        {
                            Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                            while (true)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                Thread.Sleep(2000);

                                Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                                if (newHeight.Equals(lastHeight))
                                {
                                    break;
                                }
                                lastHeight = newHeight;
                            }
                        }
                        catch
                        {
                            //e.printStackTrace();
                        }


                        string codePage = driver.PageSource;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + @"\COMMENTS" + "\\timeline_" + y + ".html", false))
                        {
                            //if (File.Exists(saveFileDialog1.FileName))
                            //    File.Delete(saveFileDialog1.FileName);

                            file.Write(codePage);
                        }

                        //backgroundWorkerComments.ReportProgress(y);
                        Thread.Sleep(2500);
                        //nbreAnnee++;
                    }






                    //driver.Close();

                   

                    //progressBar1.Maximum = lignes.Count() - 1;



                }
                catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
                {
                    try
                    {
                        string targetName = textBoxops.Text;

                        
                        string[] liYears = new string[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year) - 1).ToString() };

                        if (!Directory.Exists(pathToSave))
                            Directory.CreateDirectory(pathToSave);

                        //backgroundWorker1.ReportProgress(-1, liYears.Length);

                        //label18.Text = "Téléchargement des comments en cours ";
                        foreach (string y in liYears)
                        {
                            driver.Navigate().GoToUrl(urlFriend + "/timeline/" + y);
                            Thread.Sleep(5000);

                            while (!isElementPresentForComment(driver))
                            {


                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                                //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                                Thread.Sleep(rnd.Next(500, 1500));



                            }


                            string codePage = driver.PageSource;

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\timeline_" + y + ".html", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                file.Write(codePage);
                            }

                            //backgroundWorkerComments.ReportProgress(nbreAnnee);
                            Thread.Sleep(100);
                            nbreAnnee++;
                        }






                        //driver.Close();

                        string url = "";
                        string label = "";
                        string id = "";

                    }
                    catch
                    {

                    }

                }


            backgroundWorker1.ReportProgress(-104);
            Thread.Sleep(2000);
            
            backgroundWorker1.CancelAsync();
            comments= false;
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;

        }
        private void GetHomePage()
        {
            backgroundWorker1.ReportProgress(-90);

            string resultat = "";
            Dictionary<string, string> resultats = new Dictionary<string, string>();
            Dictionary<string, string> mois = new Dictionary<string, string>();

            mois.Add("jan", "01");
            mois.Add("fév", "02");
            mois.Add("fev", "02");
            mois.Add("fèv", "02");
            mois.Add("mar", "03");
            mois.Add("avr", "04");
            mois.Add("mai", "05");
            mois.Add("jui", "06");
            mois.Add("juil", "07");
            mois.Add("janvier", "01");
            mois.Add("février", "02");
            mois.Add("mars", "03");
            mois.Add("avril", "04");
            mois.Add("juin", "06");
            mois.Add("juillet", "07");
            mois.Add("août", "08");
            mois.Add("septembre", "09");
            mois.Add("octobre", "10");
            mois.Add("novembre", "11");
            mois.Add("décembre", "12");

            List<IWebElement> earlier = new List<IWebElement>();
            List<DateTime> sameTime = new List<DateTime>();
            bool STOP = false;


            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;
            

            if (!profilIsSet)
                GetProfileInformations(backgroundWorker1);


                try
                {


                    string targetName = textBoxops.Text;
                    Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                    Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                    Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();
                    //Rectangle resolution = Screen.FromControl(this).WorkingArea;
                    //int hauteurtotale = resolution.Height;
                    //int resolutionEcran = resolution.Height;
                    int newHauteur = 0;

                    int width = driver.Manage().Window.Size.Width;
                    int height = driver.Manage().Window.Size.Height;



                    if (!Directory.Exists(pathToSave + @"\HOMEPAGE\"))
                        Directory.CreateDirectory(pathToSave + @"\HOMEPAGE\");

                if (checkBoxFrench.Checked && LANGUAGESELECTED =="")
                    SetFrenchLanguage();


                        driver.Navigate().GoToUrl(urlFriend);
                        Thread.Sleep(5000);

                        

                        try
                        {
                            Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                        int i = 1;
                    int hauteurr = 0;
                         //while (true)

                         ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top 
                         Thread.Sleep(500);


                        while (true)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0," + hauteurr + ");");
                            Thread.Sleep(500);
                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            //Thread.Sleep(1000);

                            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='_2pid _2pin _52jv']"));
                            IWebElement findePage = null;

                        //ceci récupère les années du journal
                        try
                        {
                            IWebElement testt = driver.FindElement(By.XPath("//div[@class='uiSelectorMenuWrapper uiToggleFlyout']"));
                            //driver.FindElement(By.XPath("//div[@class='uiSelectorMenuWrapper uiToggleFlyout']")).FindElements(By.XPath("//li[@class='uiMenuItem uiMenuItemRadio uiSelectorOption']"))[0].GetAttribute("data-label")
                        }
                        catch
                        {

                        }
                        

                        //calcul du temps
                        //DateTime datumm = new DateTime(1999,0,1);
                        //if (datumm.Year != 1900)
                        //{
                        //    IList<IWebElement> listeTimes = driver.FindElements(By.ClassName("timestampContent"));
                        //    Thread.Sleep(2000);

                        //    string dernieredate = "";
                        //    string datum = "";

                        //    if (listeTimes[0].Text.ToLower().Contains("à"))
                        //    {
                        //        datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                        //        if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                        //        {
                        //            datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                        //        }

                        //    }
                        //    else
                        //        datum = listeTimes[0].Text.Split(' ')[0];

                        //    CultureInfo culture = new CultureInfo("fr-FR");
                        //    DateTime firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste
                        //    DateTime tempss;

                        //    tempss = Convert.ToDateTime(datum, culture);
                        //    DateTime date2 = new DateTime(datumm.Year, datumm.Month, datumm.Day);

                        //    foreach (IWebElement el in listeTimes)
                        //    {

                        //        //DateTime firstDate;

                        //        datum = "";
                        //        if (listeTimes[0].Text.ToLower().Contains("à"))
                        //        {
                        //            datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                        //            if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                        //            {
                        //                datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                        //            }

                        //        }
                        //        else
                        //            datum = listeTimes[0].Text.Split(' ')[0];

                        //        firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste


                        //        datum = "";
                        //        if (el.Text.ToLower().Contains("à"))
                        //        {
                        //            datum = el.Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                        //            if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                        //            {
                        //                datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);

                        //                if (Convert.ToInt32(datum.Split('/')[0]) < 10 && !datum.Split('/')[0].StartsWith("0"))
                        //                    datum = "0" + datum;
                        //            }

                        //        }
                        //        else
                        //            datum = el.Text.Split(' ')[0];

                        //        try
                        //        {
                        //            tempss = Convert.ToDateTime(datum, culture);
                        //        }
                        //        catch
                        //        {
                        //            continue;
                        //        }

                        //        int result = 0;
                        //        try
                        //        {


                        //            result = DateTime.Compare(tempss, date2);
                        //            string relationship;

                        //            if (result < 0)
                        //            {
                        //                earlier.Add(el);
                        //                relationship = "is earlier than";
                        //            }
                        //            else
                        //            if (result == 0)
                        //            {
                        //                relationship = "is the same time as";
                        //                sameTime.Add(tempss);

                        //                if (firstDate != tempss)
                        //                {
                        //                    //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true) + 1600;", el);
                        //                    //Thread.Sleep(500);
                        //                    STOP = true;
                        //                    //top = "0px";
                        //                    //fromdate = el;
                        //                    break;
                        //                }


                        //            }
                        //            else
                        //            {
                        //                relationship = "is later than";
                        //            }



                        //        }
                        //        catch
                        //        {

                        //        }


                        //    }

                        //    if (!STOP)
                        //    {
                        //        int result = DateTime.Compare(firstDate, date2);
                        //        if (result < 0)
                        //        {
                        //            STOP = true;
                        //            //top = "0px";
                        //            //fromdate = earlier[earlier.Count() - 1];

                        //        }
                        //    }

                        //}

                        //if (STOP)
                        //    break;


                        
                        if(!FASTJOURNAL)
                            if (elements.Count > 0)
                            {
                                findePage = elements[0];
                                foreach (IWebElement el in elements)
                                {
                                    IList<IWebElement> ell = el.FindElements(By.TagName("i"));
                                    findePage = ell[0];
                                    break;
                                }
                            }
                            

                            Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                        if (!FASTJOURNAL)
                        {

                        
                            var messageFrom = driver.FindElementsByXPath("//div[@class='_5pcr userContentWrapper']");

                            

                            ////on récupère les message provenant de from
                            object[] messageFroms = messageFrom.ToArray();
                       
                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messageFroms)
                            {

                                string tentation = o.ToString();
                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                if (!dicoMessagesFrom.ContainsKey(idd))
                                {
                                    dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                }
                                //else
                                //    continue;

                                if (o.LocationOnScreenOnceScrolledIntoView.Y > 15 && o.LocationOnScreenOnceScrolledIntoView.Y < (height - 250))
                                {
                                    if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                    {
                                        messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                    }
                                }

                            }

                        }

                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);

                            string pathToFile = pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png";
                        
                            if (!FASTJOURNAL)
                            foreach (string cle in messagesVisibles.Keys)
                            {

                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                {

                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                    string tmp = "";
                                    foreach (string li in lignes)
                                    {
                                        tmp += li.Trim().Replace("\r", "").Replace(";","");
                                    }



                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                }

                            }
                            if(findePage != null)
                                if (((OpenQA.Selenium.Remote.RemoteWebElement)findePage).LocationOnScreenOnceScrolledIntoView.Y <= (height - 100))
                                    break;


                            hauteurr = 450;
                            i++;
                        }
                        }
                        catch(Exception ex)
                        {
                            //e.printStackTrace();
                        }

                try
                {
                    string codePagee = driver.PageSource;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                    {

                        file.Write(codePagee);
                    }

                    if (!FASTJOURNAL)
                    {                   
                        Dictionary<string, string> newDico = new Dictionary<string, string>();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt", false))
                    {
                        //on essaie de recuperer les identifiants
                        string[] idfs = codePagee.Split(new string[] { "<a class=\"_6qw4\"" }, StringSplitOptions.RemoveEmptyEntries);
                        //string resultat = "";
                        string id = "";
                        string username = "";
                        string url = "";
                        //Dictionary<string, string> resultats = new Dictionary<string, string>();



                        foreach (string li in idfs)
                        {
                            string tmp = li.Trim();
                            if (tmp.StartsWith("data-hovercard=") && tmp.Contains("?id="))
                            {
                                id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                if (tmp.Contains("href=\""))
                                {


                                    url = "https://www.facebook.com/" + tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                    username = tmp.Substring(tmp.IndexOf("\">") + 2).Split('<')[0];
                                }

                                resultat = url + ";" + username + ";" + id + "\n";
                                if (!resultats.ContainsKey(resultat))
                                {
                                    resultats.Add(resultat, resultat);
                                }

                            }
                        }

                        idfs = codePagee.Split(new string[] { "<a title=\"" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string li in idfs)
                        {
                            string tmp = li.Trim();
                            if (tmp.Contains("href=\"") && tmp.Contains("?id="))


                            {
                                id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                if (tmp.Contains("href=\""))
                                {
                                    url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                    username = tmp.Split('"')[0];
                                }

                                resultat = url + ";" + username + ";" + id + "\n";
                                if (!resultats.ContainsKey(resultat))
                                {
                                    resultats.Add(resultat, resultat);
                                }


                            }
                        }

                        idfs = codePagee.Split(new string[] { "<a class=\"profileLink" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string li in idfs)
                        {
                            string tmp = li.Trim();
                            if (tmp.StartsWith("\" title=\"") && tmp.Contains("?id="))
                            {
                                id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];



                                if (tmp.Contains("href=\""))
                                {
                                    url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                    username = tmp.Substring(tmp.IndexOf("\" title=\"") + 9).Split('"')[0];
                                }

                                resultat = url + ";" + username + ";" + id + "\n";
                                if (!resultats.ContainsKey(resultat))
                                {
                                    resultats.Add(resultat, resultat);
                                }


                            }
                        }
                    
                        //try
                        //{
                        //    IList<IWebElement> elements = driver.FindElements(By.TagName("a"));

                        //    foreach (IWebElement el in elements)
                        //    {
                        //        if (el.GetAttribute("href") != null && el.GetAttribute("data-hovercard") != null)
                        //        {
                        //            url = el.GetAttribute("href");
                        //            id = el.GetAttribute("data-hovercard").Substring(el.GetAttribute("data-hovercard").IndexOf("?id=") + 4).Split('&')[0];

                        //            if (url.StartsWith("/"))
                        //            {
                        //                if (url.Contains("profile"))
                        //                {
                        //                    url = "https://www.facebook.com" + url.Split('&');
                        //                }
                        //                else
                        //                    url = "https://www.facebook.com" + url.Split('?');

                        //                username = el.Text;
                        //            }
                        //            else
                        //            if (url.StartsWith("https://www.facebook.com"))
                        //            {

                        //                if (url.Contains("profile"))
                        //                {
                        //                    url = url.Split('&')[0];
                        //                }
                        //                else
                        //                    url = url.Split('?')[0]; ;


                        //                if (el.GetAttribute("title") != null)
                        //                {
                        //                    username = el.GetAttribute("title");

                        //                    if (username == "")
                        //                        username = el.Text;
                        //                }
                        //            }

                        //        }
                        //        else
                        //            continue;

                        //        if (!newDico.ContainsKey(url))
                        //            newDico.Add(url, url + ";" + username + ";" + id + "\n");
                        //    }


                        //}
                        //catch
                        //{
                        //    if (newDico.Count() > 0)
                        //    {
                        //        using (System.IO.StreamWriter filee = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\AllContactsFromHomepage.txt", false))
                        //        {
                        //            string pourFichierr = "";
                        //            foreach (string ll in newDico.Values)
                        //            {
                        //                pourFichierr += ll;
                        //            }
                        //            file.Write(pourFichierr);
                        //        }
                        //    }
                        //}

                        string pourFichier = "";
                        foreach (string l in resultats.Values)
                        {
                            if (l.Contains("<img class") || l.Contains("<div class"))
                                continue;
                            pourFichier += l;
                        }

                        file.Write(pourFichier);
                    }

                    //if (newDico.Count() > 0)
                    //{
                    //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\AllContactsFromHomepage.txt", false))
                    //    {
                    //        string pourFichier = "";
                    //        foreach (string ll in newDico.Values)
                    //        {
                    //            pourFichier += ll;
                    //        }
                    //        file.Write(pourFichier);
                    //    }
                    //}
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt", false))
                    {
                        string textes = "";
                        foreach (string t in messagesVisiblesForFile.Values)
                        {
                            textes += t;
                        }


                        file.Write(textes);
                        messagesVisiblesForFile = new Dictionary<string, string>();
                    }
                }
                    Thread.Sleep(2500);
                }
                catch
                {
                    if(!FASTJOURNAL)
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt", false))
                    {
                        string pourFichier = "";
                        foreach (string l in resultats.Values)
                        {
                            if (l.Contains("<img class") || l.Contains("<div class"))
                                continue;
                            pourFichier += l;
                        }

                        file.Write(pourFichier);
                    }

                }


                    //page about -------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                    if (!Directory.Exists(pathToSave + @"\ABOUT\"))
                        Directory.CreateDirectory(pathToSave + @"\ABOUT\");

                if (urlFriend.Contains("id="))
                {
                    driver.Navigate().GoToUrl(urlFriend + "&sk=about");
                    Thread.Sleep(5000);
                }
                else
                {
                    driver.Navigate().GoToUrl(urlFriend + "/about");
                    Thread.Sleep(5000);
                }
                

                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                long innerHeightt = (long)innerHeight;
                long scroll = (long)innerHeight;
                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                scrollHeight = scrollHeight + scroll;
                int hauteur = 450;



                try
                    {
                        Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                        int i = 1;
                        while (scrollHeight >= innerHeightt)
                        {
                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            //Thread.Sleep(1000);

                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\ABOUT\" + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);



                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(2500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        i++;
                        }
                    }
                    catch
                    {
                        //e.printStackTrace();
                    }


                    string codePage = driver.PageSource;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\ABOUT\\About.html", false))
                    {
                        //if (File.Exists(saveFileDialog1.FileName))
                        //    File.Delete(saveFileDialog1.FileName);

                        file.Write(codePage);
                    }


                }
                catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
                {
                    try
                    {
                        string targetName = textBoxops.Text;


                        string[] liYears = new string[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year) - 1).ToString() };

                        if (!Directory.Exists(pathToSave))
                            Directory.CreateDirectory(pathToSave);

                        backgroundWorkerJournal.ReportProgress(-1, liYears.Length);

                        //label18.Text = "Téléchargement des comments en cours ";
                        foreach (string y in liYears)
                        {
                            driver.Navigate().GoToUrl(urlFriend);
                            Thread.Sleep(5000);

                            while (!isElementPresentForComment(driver))
                            {


                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                                //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                                Thread.Sleep(rnd.Next(500, 1500));



                            }


                            string codePage = driver.PageSource;

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                file.Write(codePage);
                            }

                            backgroundWorkerJournal.ReportProgress(nbreAnnee);
                            Thread.Sleep(100);
                            nbreAnnee++;
                        }

                        

                    }
                    catch
                    {

                    }

                }


            backgroundWorker1.ReportProgress(-101);
            Thread.Sleep(2000);
            
            backgroundWorker1.CancelAsync();
            comments = false;
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;

        }
        private void GetHomePage(int debut, int fin)
        {
            backgroundWorker1.ReportProgress(-90);

            string resultat = "";
            Dictionary<string, string> resultats = new Dictionary<string, string>();
            Dictionary<string, string> mois = new Dictionary<string, string>();

            mois.Add("jan", "01");
            mois.Add("fév", "02");
            mois.Add("fev", "02");
            mois.Add("fèv", "02");
            mois.Add("mar", "03");
            mois.Add("avr", "04");
            mois.Add("mai", "05");
            mois.Add("jui", "06");
            mois.Add("juil", "07");
            mois.Add("janvier", "01");
            mois.Add("février", "02");
            mois.Add("mars", "03");
            mois.Add("avril", "04");
            mois.Add("juin", "06");
            mois.Add("juillet", "07");
            mois.Add("août", "08");
            mois.Add("septembre", "09");
            mois.Add("octobre", "10");
            mois.Add("novembre", "11");
            mois.Add("décembre", "12");

            List<IWebElement> earlier = new List<IWebElement>();
            List<DateTime> sameTime = new List<DateTime>();
            bool STOP = false;


            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;

            System.Random rnd = new System.Random();
            int nbreAnnee = 1;


            if (!profilIsSet)
                GetProfileInformations(backgroundWorker1);


            try
            {


                string targetName = textBoxops.Text;
                Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();
                //Rectangle resolution = Screen.FromControl(this).WorkingArea;
                //int hauteurtotale = resolution.Height;
                //int resolutionEcran = resolution.Height;
                int newHauteur = 0;

                int width = driver.Manage().Window.Size.Width;
                int height = driver.Manage().Window.Size.Height;



                if (!Directory.Exists(pathToSave + @"\HOMEPAGE\"))
                    Directory.CreateDirectory(pathToSave + @"\HOMEPAGE\");

                if (checkBoxFrench.Checked && LANGUAGESELECTED == "")
                    SetFrenchLanguage();

                int i = 0;
                for (int y = debut; y <= fin; y++)
                {
                    driver.Navigate().GoToUrl(urlFriend + "/timeline?year=" + y);//https://www.facebook.com/cybercaution/timeline?year=2018
                    Thread.Sleep(5000);

                    i++;
                    int try_times = 0;

                    try
                    {
                        Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");                        
                        int hauteurr = 0;
                        //while (true)

                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top 
                        Thread.Sleep(500);


                        while (true)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0," + hauteurr + ");");
                            Thread.Sleep(500);
                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            //Thread.Sleep(1000);

                            IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='_2pid _2pin _52jv']"));
                            IWebElement findePage = null;

                            //ceci récupère les années du journal
                            try
                            {
                                IWebElement testt = driver.FindElement(By.XPath("//div[@class='uiSelectorMenuWrapper uiToggleFlyout']"));
                                //driver.FindElement(By.XPath("//div[@class='uiSelectorMenuWrapper uiToggleFlyout']")).FindElements(By.XPath("//li[@class='uiMenuItem uiMenuItemRadio uiSelectorOption']"))[0].GetAttribute("data-label")
                            }
                            catch
                            {

                            }






                            //if (!FASTJOURNAL)
                                if (elements.Count > 0)
                                {
                                    findePage = elements[0];
                                    foreach (IWebElement el in elements)
                                    {
                                        IList<IWebElement> ell = el.FindElements(By.TagName("i"));
                                        findePage = ell[0];
                                        break;
                                    }
                                }


                            Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                            if (!FASTJOURNAL)
                            {


                                var messageFrom = driver.FindElementsByXPath("//div[@class='_5pcr userContentWrapper']");



                                ////on récupère les message provenant de from
                                object[] messageFroms = messageFrom.ToArray();

                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messageFroms)
                                {

                                    string tentation = o.ToString();
                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                    {
                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                    }
                                    //else
                                    //    continue;

                                    if (o.LocationOnScreenOnceScrolledIntoView.Y > 15 && o.LocationOnScreenOnceScrolledIntoView.Y < (height - 250))
                                    {
                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                        {
                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                        }
                                    }

                                }

                            }

                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);

                            string pathToFile = pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png";

                            if (!FASTJOURNAL)
                                foreach (string cle in messagesVisibles.Keys)
                                {

                                    if (!messagesVisiblesForFile.ContainsKey(cle))
                                    {

                                        string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                        string tmp = "";
                                        foreach (string li in lignes)
                                        {
                                            tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                        }



                                        messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                    }

                                }
                            if (findePage != null)
                                if (((OpenQA.Selenium.Remote.RemoteWebElement)findePage).LocationOnScreenOnceScrolledIntoView.Y <= (height - 100))
                                    break;

                            
                            //if (newHeight.Equals(lastHeight) && hauteurr > 0)
                            //{
                            //    try_times += 1;
                            //    if (try_times > 3) 
                            //    {
                            //        try_times = 0;
                            //        break;
                                    
                            //    }

                                
                            //}
                            //lastHeight = newHeight;

                            hauteurr = 450;
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        //e.printStackTrace();
                    }

                    try
                    {
                        string codePagee = driver.PageSource;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                        {

                            file.Write(codePagee);
                        }

                        if (!FASTJOURNAL)
                        {
                            Dictionary<string, string> newDico = new Dictionary<string, string>();
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt", false))
                            {
                                //on essaie de recuperer les identifiants
                                string[] idfs = codePagee.Split(new string[] { "<a class=\"_6qw4\"" }, StringSplitOptions.RemoveEmptyEntries);
                                //string resultat = "";
                                string id = "";
                                string username = "";
                                string url = "";
                                //Dictionary<string, string> resultats = new Dictionary<string, string>();



                                foreach (string li in idfs)
                                {
                                    string tmp = li.Trim();
                                    if (tmp.StartsWith("data-hovercard=") && tmp.Contains("?id="))
                                    {
                                        id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                        if (tmp.Contains("href=\""))
                                        {


                                            url = "https://www.facebook.com/" + tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                            username = tmp.Substring(tmp.IndexOf("\">") + 2).Split('<')[0];
                                        }

                                        resultat = url + ";" + username + ";" + id + "\n";
                                        if (!resultats.ContainsKey(resultat))
                                        {
                                            resultats.Add(resultat, resultat);
                                        }

                                    }
                                }

                                idfs = codePagee.Split(new string[] { "<a title=\"" }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string li in idfs)
                                {
                                    string tmp = li.Trim();
                                    if (tmp.Contains("href=\"") && tmp.Contains("?id="))


                                    {
                                        id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                        if (tmp.Contains("href=\""))
                                        {
                                            url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                            username = tmp.Split('"')[0];
                                        }

                                        resultat = url + ";" + username + ";" + id + "\n";
                                        if (!resultats.ContainsKey(resultat))
                                        {
                                            resultats.Add(resultat, resultat);
                                        }


                                    }
                                }

                                idfs = codePagee.Split(new string[] { "<a class=\"profileLink" }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string li in idfs)
                                {
                                    string tmp = li.Trim();
                                    if (tmp.StartsWith("\" title=\"") && tmp.Contains("?id="))
                                    {
                                        id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];



                                        if (tmp.Contains("href=\""))
                                        {
                                            url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                            username = tmp.Substring(tmp.IndexOf("\" title=\"") + 9).Split('"')[0];
                                        }

                                        resultat = url + ";" + username + ";" + id + "\n";
                                        if (!resultats.ContainsKey(resultat))
                                        {
                                            resultats.Add(resultat, resultat);
                                        }


                                    }
                                }



                                string pourFichier = "";
                                foreach (string l in resultats.Values)
                                {
                                    if (l.Contains("<img class") || l.Contains("<div class"))
                                        continue;
                                    pourFichier += l;
                                }

                                file.Write(pourFichier);
                            }


                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt", false))
                            {
                                string textes = "";
                                foreach (string t in messagesVisiblesForFile.Values)
                                {
                                    textes += t;
                                }


                                file.Write(textes);
                                messagesVisiblesForFile = new Dictionary<string, string>();
                            }
                        }
                        Thread.Sleep(2500);
                    }
                    catch
                    {
                        if (!FASTJOURNAL)
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt", false))
                            {
                                string pourFichier = "";
                                foreach (string l in resultats.Values)
                                {
                                    if (l.Contains("<img class") || l.Contains("<div class"))
                                        continue;
                                    pourFichier += l;
                                }

                                file.Write(pourFichier);
                            }

                    }

                }
                
                //page about -------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                if (!Directory.Exists(pathToSave + @"\ABOUT\"))
                    Directory.CreateDirectory(pathToSave + @"\ABOUT\");

                if (urlFriend.Contains("id="))
                {
                    driver.Navigate().GoToUrl(urlFriend + "&sk=about");
                    Thread.Sleep(5000);
                }
                else
                {
                    driver.Navigate().GoToUrl(urlFriend + "/about");
                    Thread.Sleep(5000);
                }


            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                long innerHeightt = (long)innerHeight;
                long scroll = (long)innerHeight;
                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                scrollHeight = scrollHeight + scroll;
                int hauteur = 450;



                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                    int ii = 1;
                    while (scrollHeight >= innerHeightt)
                    {
                        //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        //Thread.Sleep(1000);

                        Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                        imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                        //Save the screenshot
                        imageScreenshott.SaveAsFile(pathToSave + @"\ABOUT\" + "_" + ii + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                        Thread.Sleep(100);



                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(2500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        ii++;
                    }
                }
                catch
                {
                    //e.printStackTrace();
                }


                string codePage = driver.PageSource;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\ABOUT\\About.html", false))
                {
                    //if (File.Exists(saveFileDialog1.FileName))
                    //    File.Delete(saveFileDialog1.FileName);

                    file.Write(codePage);
                }


            }
            catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
            {
                try
                {
                    string targetName = textBoxops.Text;


                    string[] liYears = new string[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year) - 1).ToString() };

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    backgroundWorkerJournal.ReportProgress(-1, liYears.Length);

                    //label18.Text = "Téléchargement des comments en cours ";
                    foreach (string y in liYears)
                    {
                        driver.Navigate().GoToUrl(urlFriend);
                        Thread.Sleep(5000);

                        while (!isElementPresentForComment(driver))
                        {


                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                            Thread.Sleep(rnd.Next(500, 1500));



                        }


                        string codePage = driver.PageSource;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                        {
                            //if (File.Exists(saveFileDialog1.FileName))
                            //    File.Delete(saveFileDialog1.FileName);

                            file.Write(codePage);
                        }

                        backgroundWorkerJournal.ReportProgress(nbreAnnee);
                        Thread.Sleep(100);
                        nbreAnnee++;
                    }



                }
                catch
                {

                }

            }


            backgroundWorker1.ReportProgress(-101);
            Thread.Sleep(2000);

            backgroundWorker1.CancelAsync();
            comments = false;
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;

        }
        private void GetAllCommentsForTest(int debut, int fin)
        {
            
           
            pictureBoxlogofacebook.Visible = true;
            pictureBoxwaiting.Visible = true;
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Visible = true;
            pictureBoxlogofacebook.BringToFront();
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Refresh();
            comments = true;

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            //pour cacher fenetre DOS
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;
            using (var driver = new ChromeDriver(driverService, chromeOptions))
            {

                // 1. Maximize the browser
                //driver.Manage().Window.Maximize();



                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);

                //u_0_8
                //"menuBar']//*[@class='menuItem']"
                // 2. Go to the "Google" homepage
                //string urlInfoUser = "/allactivity?privacy_source=your_facebook_information&entry_point=settings_yfi";
                //string urlInfoUser = "/allactivity?privacy_source=your_facebook_information&entry_point=settings_yfi";
                //driver.Navigate().GoToUrl(urlFriend + urlInfoUser);
                //titrePage = driver.Title;

                //Thread.Sleep(5000);


                try
                {


                    string targetName = textBoxops.Text;

                    //récupération des années 
                    //var years = driver.FindElementByXPath("rightColWrap']").Text;
                    //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                    //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!Directory.Exists(pathToSave ))
                        Directory.CreateDirectory(pathToSave);

                    int nbreannee = fin - debut;

                    //backgroundWorker1.ReportProgress(-1, liYears.Length);
                   

                    //label18.Text = "Téléchargement des comments en cours ";
                    for (int y = debut; y <= fin; y++)
                    {
                        driver.Navigate().GoToUrl(urlFriend + "/timeline/" + y);
                        Thread.Sleep(5000);

                        

                        try
                        {
                            Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                            while (true)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                Thread.Sleep(2000);

                                Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                                if (newHeight.Equals(lastHeight))
                                {
                                    break;
                                }
                                lastHeight = newHeight;
                            }
                        }
                        catch
                        {
                            //e.printStackTrace();
                        }


                        //string codePage = driver.PageSource;

                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\timeline_" + y + ".html", false))
                        //{
                        //    //if (File.Exists(saveFileDialog1.FileName))
                        //    //    File.Delete(saveFileDialog1.FileName);

                        //    file.Write(codePage);
                        //}

                        
                        Thread.Sleep(2500);
                        //nbreAnnee++;
                    }






                    driver.Close();



                    //progressBar1.Maximum = lignes.Count() - 1;



                }
                catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
                {
                    try
                    {
                        string targetName = textBoxops.Text;


                        string[] liYears = new string[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year) - 1).ToString() };

                        if (!Directory.Exists(pathToSave ))
                            Directory.CreateDirectory(pathToSave);

                        backgroundWorker1.ReportProgress(-1, liYears.Length);

                        //label18.Text = "Téléchargement des comments en cours ";
                        foreach (string y in liYears)
                        {
                            driver.Navigate().GoToUrl(urlFriend + "/timeline/" + y);
                            Thread.Sleep(5000);

                            while (!isElementPresentForComment(driver))
                            {


                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                                //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                                Thread.Sleep(rnd.Next(500, 1500));



                            }


                            string codePage = driver.PageSource;

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\timeline_" + y + ".html", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                file.Write(codePage);
                            }

                            backgroundWorker1.ReportProgress(nbreAnnee);
                            Thread.Sleep(100);
                            nbreAnnee++;
                        }






                        driver.Close();

                        string url = "";
                        string label = "";
                        string id = "";

                    }
                    catch
                    {

                    }

                }


            }
            //Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(-3);
            Thread.Sleep(2000);
            backgroundWorker1.CancelAsync();
            comments = false;
            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;

        }

        public Boolean isElementPresentForComment(ChromeDriver driver)
        {
            try
            {
                //return driver.FindElementByXPath("//i[@class='img sp_jgaSVtiDmn_ sx_fa8e49']").Displayed;
                return driver.FindElementByXPath("//i[@class='img sp_jgaSVtiDmn__1_5x sx_dd9709']").Displayed;
                //return true;img sp_jgaSVtiDmn__1_5x sx_dd9709
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }

        public void IsANewThread(string param)
        {
            if (backgroundWorker1 != null && backgroundWorker1.IsBusy)
                return;

               else
                    if (backgroundWorker1 != null)
                        backgroundWorker1.RunWorkerAsync(param);
                    else
                    {
                        //Reset();

                        backgroundWorker1 = new BackgroundWorker();

                        backgroundWorker1.WorkerReportsProgress = true;

                        backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

                        backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

                        backgroundWorker1.WorkerSupportsCancellation = true;
                        backgroundWorker1.RunWorkerAsync(param);
                    }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (backgroundWorker1 != null)
            {
                StopProcess();

                try
                {
                    Process [] chromeDriverProcesses = Process.GetProcessesByName("FacebookAnalyzer");

                    foreach (var chromeDriverProcess in chromeDriverProcesses)
                    {
                        chromeDriverProcess.Kill();
                    }
                }
                catch
                {

                }

                Reset();
            }
                
        }

        private void tabPageStart_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBoxops.Focus();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxops.Text == "")
            {
                textBoxops.BackColor = Color.Red;
                MessageBox.Show("Veuillez remplir le champ OPS");
                return;
            }
            else
                textBoxops.BackColor = Color.White;

            if(!pathToSave.Contains(textBoxops.Text))
                pathToSave += textBoxops.Text;

            InitializeDatagridViewMessenger();
            if (!backgroundWorkerMessenger.IsBusy)
            {
                backgroundWorkerMessenger.RunWorkerAsync();
            }


            //getContactMessenger = true;
            //InitializeDatagridViewMessenger();
            //GetContactsMessenger();
            //IsANewThread();

        }

        private void Messenger()
        {
            
            pictureBoxlogofacebook.Visible = true;
            pictureBoxwaiting.Visible = true;
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Visible = true;
            pictureBoxlogofacebook.BringToFront();
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Refresh();
           

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            //pour cacher fenetre DOS
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;
            using (var driver = new ChromeDriver(driverService, chromeOptions))
            {

                // 1. Maximize the browser
                //driver.Manage().Window.Maximize();



                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);

               


                try
                {


                    string targetName = textBoxops.Text;

                    //récupération des années 
                    //var years = driver.FindElementByXPath("rightColWrap']").Text;
                    //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                    //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                   

                   
                  
                    driver.Navigate().GoToUrl("https://www.facebook.com/messages/t");//https://www.facebook.com/messages/t/MOD.orga
                        
                    Thread.Sleep(5000);



                    try
                    {
                        var imageDown = driver.FindElementByXPath("//div[@class='uiScrollableAreaWrap scrollable']");
                        Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight");

                        while (true)
                        {
                            //driver.execute_script('document.getElementById("viewport").scrollTop += 100')

                            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollTo(0, document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight)");
                            Thread.Sleep(2000);

                            Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight");
                            if (newHeight.Equals(lastHeight))
                            {
                                break;
                            }
                            lastHeight = newHeight;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    try
                    {
                        //var imageDown = driver.FindElementById("js_10");
                        //string messenger = (String)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerHTML;", imageDown);
                        //string messenger = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)imageDown).WrappedDriver).PageSource;
                        string messenger = driver.PageSource;

                        string debut = messenger.Substring(messenger.IndexOf("aria-label=\"Conversations\""));
                        string[] destinataires = debut.Split(new String[] { "data-href=\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] profilePictures = debut.Split(new string[] { "<img class=\"_87v3 img\"" },StringSplitOptions.RemoveEmptyEntries);

                        // on récupère les destinataires

                        foreach(string dest in destinataires)
                        {
                            if (!dest.StartsWith("https://www.facebook.com/messages/t/"))
                                continue;

                            string linkk = dest.Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            if (linkk.Contains("#"))
                                continue;

                            string username= dest.Substring(dest.LastIndexOf("data-tooltip-content=\"") + 21).Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            if (!dicoMessenger.Keys.Contains(linkk))
                            {
                                dicoMessenger.Add(linkk, username);
                            }
                        }

                        

                        if (dicoMessenger.Count > 0)
                        {
                            foreach(string link in dicoMessenger.Keys)
                            {
                                try
                                {
                                    driver.Navigate().GoToUrl(link);
                                    Thread.Sleep(5000);

                                    messenger = driver.PageSource;

                                    string[] id = messenger.Split(new String[] { "class=\"uiScrollableAreaWrap scrollable\""}, StringSplitOptions.RemoveEmptyEntries);
                                    string[] topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                    //on récupère la valeur Top de la scrollBar

                                    string top = "";
                                    try
                                    {
                                        foreach (string t in topScrollBar)
                                        {
                                            if (!t.Contains("top:"))
                                                continue;

                                            top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    
                                   
                                    string idTmp = "";
                                    foreach(string idd in id)
                                    {
                                        if (!idd.StartsWith("id") && !idd.Contains("Messages"))
                                            continue;


                                        idTmp = idd.Substring(idd.IndexOf("id=\"") + 4).Split('"')[0];
                                    }

                                    try
                                    {
                                        var imageDown = driver.FindElementById(idTmp);
                                        
                                        int lastHeight = imageDown.Size.Height;
                                        

                                        int hauteur = 100;
                                        int i = 1;

                                                                               

                                        Rectangle resolution = Screen.PrimaryScreen.WorkingArea;
                                        int hauteurtotale = resolution.Height;

                                       


                                        if (isElementMessengerEndingPresent(driver))
                                        {
                                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                            Thread.Sleep(2000);

                                            

                                        }
                                        else
                                            while (!isElementMessengerEndingPresent(driver))
                                        {
                                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) +");", imageDown);
                                            Thread.Sleep(2000);

                                            

                                            hauteur += 600;
                                            hauteurtotale += resolution.Height + (resolution.Height/2);
                                            //i++;
                                        }
                                        
                                        if (isElementMessengerEndingPresent(driver))
                                        {
                                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                            Thread.Sleep(2000);

                                        }



                                        hauteur = 0;
                                        
                                            Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                        try
                                        {
                                            messenger = driver.PageSource;
                                            topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);
                                            top = topScrollBar[1].Substring(topScrollBar[1].IndexOf("top: ") + 4).Split(';')[0];
                                        }
                                        catch
                                        {
                                            top = "";
                                        }
                                        

                                        if(top != "")
                                        {
                                            string topPrec = "";
                                            while (true)
                                            {
                                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTo(0," + hauteur + ");", imageDown);
                                                Thread.Sleep(1500);


                                                messenger = driver.PageSource;
                                                topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                                //on récupère la valeur Top de la scrollBar

                                                top = "";
                                                try
                                                {


                                                    top = topScrollBar[1].Substring(topScrollBar[1].IndexOf("top: ") + 4).Split(';')[0];

                                                    if (top == topPrec)
                                                        break;

                                                    topPrec = top;
                                                }
                                                catch
                                                {

                                                }

                                                //imageDown = driver.FindElementById(idTmp);
                                                //lastHeight = imageDown.Size.Height;

                                                Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                                                imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                                                //Save the screenshot
                                                imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                                Thread.Sleep(1000);

                                                hauteur += 400;
                                                i++;
                                            }                                            
                                        }
                                        else
                                        {
                                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                                            //Save the screenshot
                                            imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                            Thread.Sleep(1000);
                                        }




                                    }
                                    catch(Exception ex)
                                    {

                                    }

                                   
                                }
                                catch (Exception ex)
                                {

                                }

                                string codePagee = driver.PageSource;

                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\Messenger\\" + dicoMessenger[link] + "\\Messenger_" + dicoMessenger[link] + ".html", false))
                                {
                                    //if (File.Exists(saveFileDialog1.FileName))
                                    //    File.Delete(saveFileDialog1.FileName);

                                    file.Write(codePagee);
                                }

                                



                                Thread.Sleep(2500);
                            }
                        }

                        
                    }
                    catch
                    {

                    }


                        try
                        {
                            Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");

                            while (true)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                Thread.Sleep(2000);

                                Object newHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                                if (newHeight.Equals(lastHeight))
                                {
                                    break;
                                }
                                lastHeight = newHeight;
                            }
                        }
                        catch
                        {
                            //e.printStackTrace();
                        }



                        //string codePage = driver.PageSource;

                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\timeline_" + ".html", false))
                        //{
                            

                        //    file.Write(codePage);
                        //}


                        Thread.Sleep(2500);
                        //nbreAnnee++;
                    





                    driver.Close();



                    //progressBar1.Maximum = lignes.Count() - 1;



                }
                catch (OpenQA.Selenium.NoSuchElementException ex)
                {
                   

                }


            }
            //Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(-3);
            Thread.Sleep(2000);
            backgroundWorker1.CancelAsync();
            
            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
        }

        private void Messenger(Dictionary<string,string> dicoMessenger, DateTime datumm)
        {
           

            if (driver == null)
            {
                InitializeDriver();                
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


                //while (!isElementPresentByID(driver, "email"))
                ////{
                //// 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");
                //// 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);
                ////}


                //// 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                //// 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                //// 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                ////searchButton.Click();
                Thread.Sleep(2500);
            }

            //string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            //pour cacher fenetre DOS
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;

                       
                // 1. Maximize the browser
                //driver.Manage().Window.Maximize();
                //driverMessenger.Close();
                //driverMessenger = new ChromeDriver(driverService, chromeOptions);
                driverMessenger = driver;

            // 2. Go to the "Google" homepage
            //driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


            //    //while (!isElementPresentByID(driver, "email"))
            //    ////{
            //    //// 3. Find the username textbox (by ID) on the homepage
            //    var userNameBox = driverMessenger.FindElementById("email");
            //    //// 4. Enter the text (to search for) in the textbox
            //    userNameBox.SendKeys(textBoxUSERNAME.Text);
            //    ////}


            //    //// 3. Find the username textbox (by ID) on the homepage
            //    var userpasswordBox = driverMessenger.FindElementById("pass");

            //    //// 4. Enter the text (to search for) in the textbox
            //    userpasswordBox.SendKeys(textBoxPASSWORD.Text);
            //    Thread.Sleep(5000);

            //    //// 5. Find the search button (by Name) on the homepage
            //    driverMessenger.FindElementById("loginbutton").Click();
            //    ////searchButton.Click();
            //    Thread.Sleep(2500);

            
            
            
            


            try
            {


                    string targetName = textBoxops.Text;
                    string textes = "";
                    string messagesFromInString = "";
                    string messagesVisiblesWithScreenshots = "";
                    string pathToFolder = "";
                    List<IWebElement> earlier = new List<IWebElement>();
                    List<DateTime> sameTime = new List<DateTime>();
                    int hauteurr = 0;
                //var imageDown = "";
                    Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                    Dictionary<string, string> dicoMessagesTo = new Dictionary<string, string>();
                    Dictionary<string, string> dicoPictures = new Dictionary<string, string>();
                    Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                    Dictionary<string, string> videosVisibles = new Dictionary<string, string>();
                    Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();
                    List<string> videosVisiblesForFile = new List<string>();
                    Dictionary<string, string> audioVisiblesForFile = new Dictionary<string, string>();
                    Dictionary<string, string> audioVisibles = new Dictionary<string, string>();
                    List<string> audioVisibless = new List<string>();
                    List<string> audioVisiblessForFile = new List<string>();
                    List<string> docPartages = new List<string>();
                    Dictionary<string,string> mois = new Dictionary<string, string>();
                    Dictionary<string, string> classeTraitee = new Dictionary<string, string>();
                    int WAIT = 500;
                    mois.Add("jan", "01");
                    mois.Add("fév", "02");
                    mois.Add("fev", "02");
                    mois.Add("fèv", "02");
                    mois.Add("mar", "03");
                    mois.Add("avr", "04");
                    mois.Add("mai", "05");
                    mois.Add("jui", "06");
                    mois.Add("juil", "07");
                    mois.Add("janvier", "01");
                    mois.Add("février", "02");
                    mois.Add("mars", "03");
                    mois.Add("avril", "04");
                    mois.Add("juin", "06");
                    mois.Add("juillet", "07");
                    mois.Add("août", "08");
                    mois.Add("septembre", "09");
                    mois.Add("octobre", "10");
                    mois.Add("novembre", "11");
                    mois.Add("décembre", "12");
               


                    List<string> classesConnues = new System.Collections.Generic.List<string>();
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr _ylc']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr _ylc']");
                    classesConnues.Add("//div[@class='_52mr _2poz _ui9 _4skb']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
                    classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _q4a _8lma direction_ltr text_align_ltr _ylc']");
                    classesConnues.Add("//div[@class='_2poz _52mr _ui9 _2n8h _2n8i _5fk1']");//_2poz _52mr _ui9 _2n8h _2n8i _5fk1 Vidéo ?_ccq _4tsk _3o67 _52mr _1byr _4-od
                    classesConnues.Add("//div[@class='_1mj2 _2e-6']");
                    classesConnues.Add("//div[@class='_1mj4 _2e-7']");//_1mj4 _2e-7
                    classesConnues.Add("//div[@class='_3058 _15gf']");//_3058 _15gf
                                                                      //_1mjb _454y _3czg _2poz _ui9
                                                                      //_1mjb _454y _3czg _2poz _ui9
                                                                      //_hh7 _6ybn _s1- _52mr _1fz8 _1nqp

                //récupération des années 
                //var years = driver.FindElementByXPath("rightColWrap']").Text;
                //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //if (!Directory.Exists(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper()))
                //    Directory.CreateDirectory(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper());

                //driver.Navigate().GoToUrl("https://www.facebook.com/messages/t");//https://www.facebook.com/messages/t/MOD.orga
                //Thread.Sleep(5000);



                try
                {

                        bool STOP = false;
                        string messenger = "";
                        int idFictif = 1;
                        string idFictifString = "";
                        IWebElement fromdate = null;

                    if (FASTMESSENGER)
                        WAIT = 500;
                    else
                        WAIT = 1000;

                        if (dicoMessenger.Count > 0)
                        {
                            foreach (string link in dicoMessenger.Keys)
                            {

                                hauteurr = 0;
                                string pathToSaveScreenshot = "";

                                if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                                {
                                    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                    pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link];
                                }
                                else
                                if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                                {
                                    idFictif++;
                                    idFictifString = idFictif.ToString();
                                    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString);
                                    pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString;
                                }
                                else
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link];



                            if (!Directory.Exists(pathToSaveScreenshot + "\\Audio\\"))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSaveScreenshot + "\\Audio\\");
                            }

                            var imageDown = driverMessenger.FindElement(By.Id("facebook"));
                            int hauteur = 400;
                            int resolutionEcran = resolution.Height;
                            string[] topScrollBar = null;
                            string firstValueTopBar = "";
                            string top = "";
                            string hauteurScroll = "";
                            string newHauteurScroll = "";
                            int i = 1;
                            string idTmp = "";
                            //DEPART TRY TIMEOUT------------------------------------------------------------------------------
                            try
                            {
                                driverMessenger.Manage().Timeouts().ImplicitWait.Add(System.TimeSpan.FromSeconds(300));
                                driverMessenger.Navigate().GoToUrl(link);
                                Thread.Sleep(5000);

                                messenger = driverMessenger.PageSource;
                                

                                


                                string[] id = messenger.Split(new String[] { "class=\"uiScrollableAreaWrap scrollable\"" }, StringSplitOptions.RemoveEmptyEntries);
                                topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                //on récupère la valeur Top de la scrollBar
                                

                                try
                                {
                                    foreach (string t in topScrollBar)
                                    {
                                        string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                        if ((!t.Contains("top:") && !t.Contains("uiScrollableAreaWrap")) || t.StartsWith("<html"))
                                            continue;

                                        firstValueTopBar = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                        hauteurScroll = tmp.Substring(tmp.IndexOf("height: ") + 8).Split(';')[0];
                                    }


                                }
                                catch
                                {

                                }
                                //try
                                //{
                                //    foreach (string t in topScrollBar)
                                //    {
                                //        if (!t.Contains("top:"))
                                //            continue;

                                //        top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                //        firstValueTopBar = top;
                                //    }
                                //}
                                //catch
                                //{

                                //}

                                //on récupère l'id dynamique du composant
                                
                                foreach (string idd in id)
                                {
                                    if (!idd.StartsWith("id") && !idd.Contains("Messages"))
                                        continue;


                                    idTmp = idd.Substring(idd.IndexOf("id=\"") + 4).Split('"')[0];
                                }

                                try
                                {
                                    imageDown = driverMessenger.FindElementById(idTmp);
                                    var test = driverMessenger.FindElements(By.ClassName("uiScrollableAreaGripper"));//uiScrollableAreaTrack hidden_elem
                                    //var test2 = driverMessenger.FindElements(By.CssSelector("uiScrollableAreaTrack hidden_elem"));
                                    int lastHeight = imageDown.Size.Height;


                                    
                                    

                                    int width = driverMessenger.Manage().Window.Size.Width;
                                    int height = driverMessenger.Manage().Window.Size.Height;


                                    int hauteurtotale = resolution.Height;
                                    resolutionEcran = resolution.Height;
                                    int scroll = 0;
                                    Int32.TryParse(firstValueTopBar.Replace(".", "").Replace("px", ""), out scroll);



                                    //ON SCROLL JUSQUAU DEBUT DE LA PAGE LE PLUS LOIN POSSIBLE

                                    //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", listeTimes[0 ]);
                                    //Thread.Sleep(500);

                                    if (firstValueTopBar != "")
                                    {
                                        try
                                        {
                                            while (top.Trim() != "0px")
                                            //while (newHauteurScroll != hauteurScroll)
                                            {
                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll) + ");", imageDown);
                                                //Thread.Sleep(2500);

                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - (scroll * scroll)) + ");", imageDown);
                                                //Thread.Sleep(2500);


                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                Thread.Sleep(5000);



                                                //calcul du temps
                                                if (datumm.Year != 1900)
                                                {
                                                    IList<IWebElement> listeTimes = driver.FindElements(By.TagName("time"));
                                                    Thread.Sleep(2000);

                                                    string dernieredate = "";
                                                    string datum = "";

                                                    if (listeTimes[0].Text.ToLower().Contains("à"))
                                                    {
                                                        datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                        if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                        {
                                                            datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                                                        }

                                                    }
                                                    else
                                                        datum = listeTimes[0].Text.Split(' ')[0];

                                                    CultureInfo culture = new CultureInfo("fr-FR");
                                                    DateTime testDatumValable;
                                                    if (!DateTime.TryParse(datum, out testDatumValable))
                                                        continue;

                                                    DateTime firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste
                                                    DateTime tempss;

                                                    
                                                    tempss = Convert.ToDateTime(datum, culture);
                                                    DateTime date2 = new DateTime(datumm.Year, datumm.Month, datumm.Day);

                                                    

                                                    try
                                                    {


                                                        foreach (IWebElement el in listeTimes)
                                                        {
                                                            if (DateTime.Compare(firstDate, date2) == 0)
                                                                break;
                                                                
                                                            //DateTime firstDate;



                                                            datum = "";
                                                            if (listeTimes[0].Text.ToLower().Contains("à"))
                                                            {
                                                                datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                                if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                                {
                                                                    datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                                                                }

                                                            }
                                                            else
                                                                datum = listeTimes[0].Text.Split(' ')[0];


                                                            if (DateTime.TryParse(datum, out testDatumValable))
                                                                firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste
                                                            else
                                                                continue;

                                                            datum = "";
                                                            if (el.Text.ToLower().Contains("à"))
                                                            {
                                                                datum = el.Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                                if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                                {
                                                                    datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);

                                                                    if (Convert.ToInt32(datum.Split('/')[0]) < 10 && !datum.Split('/')[0].StartsWith("0"))
                                                                        datum = "0" + datum;
                                                                }

                                                            }
                                                            else
                                                                datum = el.Text.Split(' ')[0];

                                                            try
                                                            {
                                                                tempss = Convert.ToDateTime(datum, culture);
                                                            }
                                                            catch
                                                            {
                                                                continue;
                                                            }

                                                            int result = 0;
                                                            try
                                                            {


                                                                result = DateTime.Compare(tempss, date2);
                                                                string relationship;

                                                                if (result < 0)
                                                                {
                                                                    earlier.Add(el);
                                                                    relationship = "is earlier than";
                                                                }
                                                                else
                                                                if (result == 0)
                                                                {
                                                                    relationship = "is the same time as";
                                                                    sameTime.Add(tempss);

                                                                    if (firstDate != tempss)
                                                                    {
                                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true) + 1600;", el);
                                                                        //Thread.Sleep(500);
                                                                        STOP = true;
                                                                        top = "0px";
                                                                        fromdate = el;
                                                                        break;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    relationship = "is later than";
                                                                }



                                                            }
                                                            catch
                                                            {

                                                            }


                                                        }

                                                        if (!STOP)
                                                        {
                                                            int result = DateTime.Compare(firstDate, date2);
                                                            if (result < 0)
                                                            {
                                                                STOP = true;
                                                                top = "0px";
                                                                fromdate = earlier[earlier.Count() - 1];

                                                            }
                                                        }

                                                    }
                                                    catch
                                                    {

                                                    }

                                                    if (STOP)
                                                        break;

                                                }//FIN DU IF DATE


                                                if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                {

                                                        try
                                                        {
                                                            driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                        }
                                                        catch
                                                        {

                                                        }
                                                }


                                                    hauteur += 600;
                                                    hauteurtotale += resolution.Height + (resolution.Height / 2);

                                                    messenger = driverMessenger.PageSource;

                                                    top = "";
                                                    topScrollBar = messenger.Split(new String[] { "<div class=\"uiScrollableAreaGripper" }, StringSplitOptions.RemoveEmptyEntries);
                                                    try
                                                    {
                                                        foreach (string t in topScrollBar)
                                                        {
                                                            string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                            if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                                continue;

                                                            int heightt = 0;
                                                            top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                                            newHauteurScroll = tmp.Substring(tmp.IndexOf("height: ") + 8).Split(';')[0];
                                                            Int32.TryParse(top.Trim().Replace(".", "").Replace("px", ""), out heightt);
                                                            scroll = scroll - hauteur;
                                                            break;
                                                        }

                                                        if (top.Trim() == "0px")
                                                        {
                                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) + ");", imageDown);
                                                            //Thread.Sleep(2000);
                                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                            Thread.Sleep(5000);

                                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                            Thread.Sleep(5000);

                                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                            Thread.Sleep(5000);

                                                            hauteurScroll = newHauteurScroll;
                                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll * scroll) + ");", imageDown);
                                                            //Thread.Sleep(2500);

                                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll) + ");", imageDown);
                                                            // Thread.Sleep(2500);

                                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - 2 * hauteur) + ");", imageDown);
                                                            //Thread.Sleep(2000);

                                                            messenger = driverMessenger.PageSource;

                                                            top = "";
                                                            topScrollBar = messenger.Split(new String[] { "<div class=\"uiScrollableAreaGripper" }, StringSplitOptions.RemoveEmptyEntries);


                                                            try
                                                            {
                                                                foreach (string tt in topScrollBar)
                                                                {
                                                                    string tmpp = tt.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                                    if (!tmpp.Contains("top:") || tmpp.Contains("hidden"))
                                                                        continue;

                                                                    top = tmpp.Substring(tmpp.IndexOf("top: ") + 4).Split(';')[0];
                                                                    newHauteurScroll = tmpp.Substring(tmpp.IndexOf("height: ") + 8).Split(';')[0];
                                                                    int heightt = 0;
                                                                    Int32.TryParse(top.Trim().Replace(".", "").Replace("px", ""), out heightt);
                                                                    scroll = scroll - hauteur;
                                                                    break;
                                                                }


                                                            }
                                                            catch
                                                            {
                                                                top = "";
                                                            }

                                                            hauteur += 600;
                                                            hauteurtotale += resolution.Height + (resolution.Height / 2);
                                                        }


                                                    }
                                                    catch
                                                    {

                                                    }

                                                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                                    int innerHeightt = 0;

                                                    Int32.TryParse(innerHeight.ToString(), out innerHeightt);
                                                    scroll -= innerHeightt;


                                                    //i++;
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            top = "0px";
                                        }
                                    }
                                    else
                                        if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);



                                    }
                                    else
                                        if (!isElementMessengerEndingPresent(driverMessenger))
                                        while (!isElementMessengerEndingPresent(driverMessenger))
                                        {
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) + ");", imageDown);
                                            Thread.Sleep(2000);



                                            hauteur += 600;
                                            hauteurtotale += resolution.Height + (resolution.Height / 2);
                                            //i++;
                                        }
                                    else
                                        if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);

                                    }



                                    hauteur = 0;


                                    top = "";
                                    try
                                    {
                                        foreach (string t in topScrollBar)
                                        {
                                            string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                            if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                continue;

                                            top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                            //top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                        }


                                    }
                                    catch(Exception ex) 
                                    {
                                        top = "";
                                    }

                                    //ON DESCEND--------------------------------------------------------------------------------------
                                    if (top != "")
                                    {

                                        //Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        //imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                        //Thread.Sleep(500);


                                        ////imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                        //imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + 1 + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);

                                        //Thread.Sleep(500);

                                        string topPrec = "";
                                        textes = imageDown.Text;

                                        
                                        int j = 1;
                                        int hauteurfromdate = 0;
                                        string hauteurfromdateString = "";

                                        Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                        long innerHeightt = (long)innerHeight;
                                        long scrolll = (long)innerHeight;
                                        long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                        //on initialise

                                        IList<IWebElement> firstlisteVocauxx = null;
                                        IList<IWebElement> firstallDivElements = null;
                                        IList<IWebElement> firstallOthers = new List<IWebElement>();
                                        List<IWebElement> listeNotReadOnly = null;
                                        List<IWebElement> listeOthersNotReadOnly = new List<IWebElement>();
                                        List<IWebElement> listeVideoNotReadOnly = new List<IWebElement>();
                                        var toRemove = new HashSet<IWebElement>();
                                        var toRemoveOthers = new HashSet<IWebElement>();
                                        var toRemoveVideo = new HashSet<IWebElement>();

                                        IList<IWebElement> listeVocauxxOriginal;
                                        IList<IWebElement> allDivElementsOriginal = null;
                                        IList<IWebElement> allOthersOriginal = new List<IWebElement>();

                                        foreach (string classee in classesConnues)
                                        {
                                            if (classee.Contains("_1mj2 _2e-6") || classee.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                            {

                                                firstlisteVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));
                                                listeNotReadOnly = firstlisteVocauxx.ToList();

                                            }

                                            if (classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {

                                                firstallDivElements = driverMessenger.FindElementsByXPath(classee + "//video");//_ox1 _21y0}
                                                listeVideoNotReadOnly = firstallDivElements.ToList();
                                            }

                                            if (!classee.Contains("_1mj2 _2e-6") || !classee.Contains("_1mj4 _2e-7") || !classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                            {
                                                
                                                foreach(IWebElement el in driverMessenger.FindElementsByXPath(classee))
                                                {
                                                    firstallOthers.Add(el);
                                                    listeOthersNotReadOnly.Add(el);
                                                }
                                                
                                                //firstallOthers = driverMessenger.FindElementsByXPath(classee);//_ox1 _21y0}
                                                //listeOthersNotReadOnly = firstallOthers.ToList();

                                            }

                                                
                                        }

                                            while (true)
                                            {
                                            
                                            if (fromdate != null)
                                            {
                                                //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                                hauteurfromdate = fromdate.Location.Y;
                                                hauteurfromdateString = hauteurfromdate.ToString();

                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                                Thread.Sleep(500);

                                                //hauteurfromdate = fromdate.Location.Y;
                                                fromdate = null;
                                                STOP = false;
                                                //hauteur += 600;

                                            }
                                            else
                                            {

                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0," + hauteurr + ");", imageDown);

                                                Thread.Sleep(WAIT);
                                            }



                                            if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                            {

                                                try
                                                {
                                                    driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                }
                                                catch
                                                {

                                                }
                                            }
                                            //int hei = imageDown.Size.Height;
                                            messenger = driverMessenger.PageSource;

                                            if(!FASTMESSENGER)
                                            foreach (string classe in classesConnues)
                                            {

                                                if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                {


                                                        //pour récupérer les a contenant la longueur du message vocal
                                                        //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        IList<IWebElement> listeVocauxx = firstlisteVocauxx;
                                                        listeVocauxxOriginal = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                        if(!listeVocauxxOriginal.SequenceEqual(firstlisteVocauxx))
                                                        {
                                                            listeVocauxx = listeVocauxxOriginal;
                                                            firstlisteVocauxx = listeVocauxxOriginal;
                                                            listeNotReadOnly = listeVocauxx.ToList();

                                                        }


                                                        List<IWebElement> newListVocaux = listeNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                                    if (listeNotReadOnly.Count > 0)
                                                    foreach (IWebElement ell in newListVocaux)
                                                    {
                                                        //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                        //    break;
                                                        if (ell.Text == "")
                                                            continue;

                                                        string tentation = ell.ToString();
                                                        string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                        if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                        {
                                                            //string tentation = ell.ToString();
                                                            //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                            IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            string dateAudio = "";
                                                            string nomFichierAudio = "";

                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "From";
                                                                        toRemove.Add(ell);
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                            if (dateAudio == "")
                                                            {
                                                                els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                dateAudio = "";
                                                                foreach (IWebElement el in els)
                                                                {
                                                                    if (el.GetAttribute("data-tooltip-content") != null)
                                                                    {

                                                                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                        {
                                                                            var texte = el.GetAttribute("data-tooltip-content");
                                                                            var position = el.Location;
                                                                            dateAudio = texte.Replace(":", "-");
                                                                            nomFichierAudio = "To";
                                                                            break;
                                                                        }



                                                                    }
                                                                }
                                                            }

                                                            string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];


                                                            if (duree.Length == 4)
                                                                duree = "00:0" + duree;

                                                            if (audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibless.Add(audioVisibles[idd]);
                                                                continue;
                                                            }

                                                            try
                                                            {
                                                                TimeSpan tempss;
                                                                TimeSpan.TryParse(duree, out tempss);
                                                                //DateTime temps;
                                                                //DateTime.TryParse(duree, out temps);
                                                                ell.Click();
                                                                StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                Stopwatch sw = new Stopwatch();
                                                                sw.Start();
                                                                //Thread.Sleep(500);
                                                                while (true)
                                                                {

                                                                    if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                    {
                                                                        if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                        {

                                                                            try
                                                                            {
                                                                                driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                            }
                                                                            catch
                                                                            {

                                                                            }
                                                                        }
                                                                        break;
                                                                    }

                                                                        toRemove.Add(ell);
                                                                }

                                                                StopRecording();
                                                                Thread.Sleep(1000);

                                                                if (!audioVisibles.ContainsKey(idd))
                                                                {
                                                                    audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                    audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                        
                                                                        if (!classeTraitee.ContainsKey(idd))
                                                                        {
                                                                            classeTraitee.Add(idd, idd);
                                                                        }
                                                                }



                                                                j++;

                                                            }
                                                            catch (Exception exx)
                                                            {
                                                                //StopRecording();
                                                                //Thread.Sleep(5000);
                                                            }


                                                            //ell.Click();


                                                        }
                                                            
                                                            
                                                            
                                                        }

                                                        listeNotReadOnly.RemoveAll(toRemove.Contains);

                                                        //pour cliquer sur le message vocal et jouer le son
                                                        //driverMessenger.FindElementsByXPath(classe)[0].Click();

                                                    }


                                                if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                {

                                                        IList<IWebElement> listeVideos = firstallDivElements;
                                                        allDivElementsOriginal = driverMessenger.FindElementsByXPath(classe + "//video");

                                                        if (!allDivElementsOriginal.SequenceEqual(firstallDivElements))
                                                        {
                                                            listeVideos = allDivElementsOriginal;
                                                            firstlisteVocauxx = allDivElementsOriginal;
                                                            listeVideoNotReadOnly = listeVideos.ToList();

                                                        }


                                                        List<IWebElement> newListVideos = listeVideoNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();



                                                        //IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    if (listeVideoNotReadOnly.Count > 0)
                                                    for (int ii = newListVideos.Count() - 1; ii >=0; ii--)
                                                    {
                                                            string tentation = newListVideos[ii].ToString();
                                                            string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            if (classeTraitee.ContainsKey(idd))
                                                            {
                                                                continue;
                                                            }


                                                        if (listeVideoNotReadOnly[ii].GetAttribute("class") != null)
                                                        {
                                                            //here the print statement will print the value of each div tag element
                                                            var tmp = newListVideos[ii].GetAttribute("class");

                                                            if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                            {
                                                                //var video = driver.FindElementsByClassName(tmp);
                                                                //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                try
                                                                {
                                                                    //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                    var tmpp = newListVideos[ii].GetAttribute("src");

                                                                    //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                    //    break;


                                                                    if (newListVideos[ii].Location.Y > 15 && newListVideos[ii].Location.Y < (resolutionEcran - 250))
                                                                    {
                                                                           

                                                                        if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                        {
                                                                            videosVisibles.Add(tmpp + ii, tmpp);

                                                                                if (!classeTraitee.ContainsKey(idd))
                                                                                {
                                                                                    classeTraitee.Add(idd, tmpp);
                                                                                }
                                                                        }

                                                                    toRemoveVideo.Add(newListVideos[ii]);
                                                                    }
                                                                    //break;



                                                                    //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }

                                                            }
                                                        }

                                                     listeVideoNotReadOnly.RemoveAll(toRemoveVideo.Contains);

                                                    }




                                                }

                                                    //if (!classe.Contains("_1mj2 _2e-6") || !classe.Contains("_1mj4 _2e-7") || !classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                                    //{
                                                    //    foreach (IWebElement el in driverMessenger.FindElementsByXPath(classe))
                                                    //    {
                                                    //        //firstallOthers.Add(el);
                                                    //        //listeOthersNotReadOnly.Add(el);
                                                    //        allOthersOriginal.Add(el);
                                                    //    }




                                                    //}
                                                   


                                                       


                                            }//FIN FOREACH CLASSES CONNUES


                                            IList<IWebElement> listeOthers = firstallOthers;
                                            //allOthersOriginal = driverMessenger.FindElements(By.XPath(classe));

                                            //if (!allOthersOriginal.SequenceEqual(firstallOthers))
                                            //{
                                            //    listeOthers = allOthersOriginal;
                                            //    firstallOthers = allOthersOriginal;
                                            //    listeOthersNotReadOnly = listeOthers.ToList();

                                            //}

                                            List<IWebElement> newList = listeOthersNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                            if (listeOthersNotReadOnly.Count > 0)

                                                //var messages = driverMessenger.FindElementsByXPath(classe);
                                                //object[] messagesToExtract = messages.ToArray();


                                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in newList)
                                                {

                                                    string tentation = o.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    if (classeTraitee.ContainsKey(idd))
                                                        continue;

                                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    {
                                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                    //else
                                                    //    continue;


                                                    if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                        {
                                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                            if (!classeTraitee.ContainsKey(idd))
                                                            {
                                                                classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                            }
                                                        }

                                                        toRemoveOthers.Add(o);
                                                    }
                                                    //else
                                                    //    break;

                                                    //messagesToExtract.ToList().Remove(o);

                                                }

                                            listeOthersNotReadOnly.RemoveAll(toRemoveOthers.Contains);
                                            allOthersOriginal = new List<IWebElement>();
                                            toRemoveOthers = new HashSet<IWebElement>();



                                            //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                            //imageDown = driver.FindElementById(idTmp);
                                            //Thread.Sleep(200);

                                            //if (textes != imageDown.Text)
                                            //    textes = imageDown.Text + "\n";

                                            messenger = driverMessenger.PageSource;
                                            //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            top = "";
                                            try
                                            {
                                                foreach (string t in topScrollBar)
                                                {
                                                    string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                    if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                        continue;

                                                    top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                                    //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                                    //    continue;

                                                    //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                                }

                                                if (top == topPrec)
                                                    break;

                                                topPrec = top;
                                            }
                                            catch
                                            {

                                            }

                                            Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                            //imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                            Thread.Sleep(500);


                                            //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                            imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                            Thread.Sleep(500);

                                            //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                            string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                            //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                            pathToFolder = pathToSaveScreenshot;

                                            if(!FASTMESSENGER)
                                            foreach (string cle in messagesVisibles.Keys)
                                            {

                                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                                {

                                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                    string tmp = "";
                                                    foreach (string li in lignes)
                                                    {
                                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                    }



                                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                }

                                                //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                            }

                                            if (!FASTMESSENGER)
                                               if (videosVisibles.Count > 0)
                                                foreach (string cle in videosVisibles.Keys)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{

                                                    videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                            if (!FASTMESSENGER)
                                                if (audioVisibless.Count > 0)
                                                foreach (string cle in audioVisibless)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{
                                                    if (!audioVisiblesForFile.ContainsKey(cle))
                                                        audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                            messagesVisibles = new Dictionary<string, string>();
                                            videosVisibles = new Dictionary<string, string>();
                                            //audioVisibles = new Dictionary<string, string>();
                                            hauteur += 600;
                                            hauteurr = 450;

                                            i++;
                                        }
                                        ForGrid forGrid = new ForGrid();
                                        forGrid.PathToFolder = pathToFolder;
                                        forGrid.Url = link;

                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";
                                        

                                        backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                        //FillDataGridViewMessenger(pathToFolder, link);
                                    }
                                    else//Si pas de scrollbar alors seulement une page------------------------------------------------------------------------------------------
                                    {
                                        int j = 1;
                                        foreach (string classe in classesConnues)
                                        {


                                            if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                            {


                                                //pour récupérer les a contenant la longueur du message vocal
                                                //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                foreach (IWebElement ell in listeVocauxx)
                                                {
                                                    //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                    //    break;
                                                    if (ell.Text == "")
                                                        continue;

                                                    string tentation = ell.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                    //string tentation = ell.ToString();
                                                    //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //if (audioVisibles.ContainsKey(idd))
                                                    //{
                                                    //    continue;
                                                    //}

                                                    if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        //string tentation = ell.ToString();
                                                        //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                        IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                        string dateAudio = "";
                                                        string nomFichierAudio = "";

                                                        foreach (IWebElement el in els)
                                                        {
                                                            if (el.GetAttribute("data-tooltip-content") != null)
                                                            {

                                                                if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                {
                                                                    var texte = el.GetAttribute("data-tooltip-content");
                                                                    var position = el.Location;
                                                                    dateAudio = texte.Replace(":", "-");
                                                                    nomFichierAudio = "From";
                                                                    break;
                                                                }



                                                            }
                                                        }
                                                        if (dateAudio == "")
                                                        {
                                                            els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            dateAudio = "";
                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "To";
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                        }

                                                        string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                        //if (classe.Contains("_1mj2 _2e-6"))
                                                        //    nomFichierAudio = "From";
                                                        //else
                                                        //    nomFichierAudio = "To";

                                                        if (duree.Length == 4)
                                                            duree = "00:0" + duree;

                                                        if (audioVisibles.ContainsKey(idd))
                                                        {
                                                            audioVisibless.Add(audioVisibles[idd]);
                                                            continue;
                                                        }

                                                        try
                                                        {
                                                            TimeSpan tempss;
                                                            TimeSpan.TryParse(duree, out tempss);
                                                            //DateTime temps;
                                                            //DateTime.TryParse(duree, out temps);
                                                            ell.Click();
                                                            StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                            Stopwatch sw = new Stopwatch();
                                                            sw.Start();
                                                            //Thread.Sleep(500);
                                                            while (true)
                                                            {

                                                                if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                {
                                                                    if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                    {

                                                                        try
                                                                        {
                                                                            driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                        }
                                                                        catch
                                                                        {

                                                                        }
                                                                    }
                                                                    break;
                                                                }

                                                            }

                                                            StopRecording();
                                                            Thread.Sleep(1000);

                                                            if (!audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                            }



                                                            j++;

                                                        }
                                                        catch (Exception exx)
                                                        {
                                                            //StopRecording();
                                                            //Thread.Sleep(5000);
                                                        }


                                                        //ell.Click();


                                                    }

                                                }

                                                //pour cliquer sur le message vocal et jouer le son
                                                //driverMessenger.FindElementsByXPath(classe)[0].Click();
                                            }







                                            if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {


                                                IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                {

                                                    if (allDivElements[ii].GetAttribute("class") != null)
                                                    {
                                                        //here the print statement will print the value of each div tag element
                                                        var tmp = allDivElements[ii].GetAttribute("class");

                                                        if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                        {
                                                            //var video = driver.FindElementsByClassName(tmp);
                                                            //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                            try
                                                            {
                                                                //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                var tmpp = allDivElements[ii].GetAttribute("src");


                                                                if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                {
                                                                    if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                    {
                                                                        videosVisibles.Add(tmpp + ii, tmpp);
                                                                    }
                                                                }
                                                                //break;



                                                                //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }

                                                        }
                                                    }

                                                }




                                            }

                                            if (!classe.Contains("_1mj2 _2e-6") && !classe.Contains("_1mj4 _2e-7") && classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                            {
                                                var messages = driverMessenger.FindElementsByXPath(classe);
                                                object[] messagesToExtract = messages.ToArray();



                                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                                {

                                                    string tentation = o.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    {
                                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                    //else
                                                    //    continue;

                                                    if (o.Location.Y + o.Size.Height > 15 && o.Location.Y < (resolutionEcran - 300))
                                                    {
                                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                                        {
                                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                        }
                                                    }

                                                }
                                            }

                                            
                                        }


                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";

                                        Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        //Save the screenshot
                                        //if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                                        //{
                                        //    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                        //}
                                        //else
                                        //if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                                        //{
                                        //    idFictif++;
                                        //    idFictifString = idFictif.ToString();
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + i.ToString());
                                        //}



                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                                        Thread.Sleep(100);

                                        string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                        pathToFolder = pathToSaveScreenshot;

                                        foreach (string cle in messagesVisibles.Keys)
                                        {

                                            if (!messagesVisiblesForFile.ContainsKey(cle))
                                            {

                                                string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                string tmp = "";
                                                foreach (string li in lignes)
                                                {
                                                    tmp += li.Trim().Replace("\r", "");
                                                }



                                                messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                            }

                                            //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                        }

                                        if (videosVisibles.Count > 0)
                                            foreach (string cle in videosVisibles.Keys)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{

                                                videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                        if (audioVisibles.Count > 0)
                                            foreach (string cle in audioVisibles.Values)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{
                                                if (!audioVisibles.ContainsKey(cle))
                                                    audioVisiblesForFile.Add(cle + ";" + pathToFile + "\n", cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                        messagesVisibles = new Dictionary<string, string>();
                                        videosVisibles = new Dictionary<string, string>();
                                        //audioVisibles = new Dictionary<string, string>();

                                        ForGrid forGrid = new ForGrid();
                                        forGrid.PathToFolder = pathToFolder;
                                        forGrid.Url = link;

                                        backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                        //FillDataGridViewMessenger(pathToFolder, link);
                                        //continue;
                                    }




                                }
                                catch (Exception ex)//SI TIMEOUT
                                {
                                    MessageBox.Show("error dans Messenger : " + ex.Message);
                                    ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return window.stop");
                                    Thread.Sleep(1000);

                                    Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                    imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                    Thread.Sleep(500);


                                    //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                    imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + 1 + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);

                                    Thread.Sleep(500);

                                    string topPrec = "";
                                    textes = imageDown.Text;


                                    int j = 1;
                                    int hauteurfromdate = 0;
                                    string hauteurfromdateString = "";

                                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                    long innerHeightt = (long)innerHeight;
                                    long scrolll = (long)innerHeight;
                                    long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                    //on initialise

                                    IList<IWebElement> firstlisteVocauxx = null;
                                    IList<IWebElement> firstallDivElements = null;
                                    IList<IWebElement> firstallOthers = new List<IWebElement>();
                                    List<IWebElement> listeNotReadOnly = null;
                                    List<IWebElement> listeOthersNotReadOnly = new List<IWebElement>();
                                    List<IWebElement> listeVideoNotReadOnly = new List<IWebElement>();
                                    var toRemove = new HashSet<IWebElement>();
                                    var toRemoveOthers = new HashSet<IWebElement>();
                                    var toRemoveVideo = new HashSet<IWebElement>();

                                    IList<IWebElement> listeVocauxxOriginal;
                                    IList<IWebElement> allDivElementsOriginal = null;
                                    IList<IWebElement> allOthersOriginal = new List<IWebElement>();

                                    foreach (string classee in classesConnues)
                                    {
                                        if (classee.Contains("_1mj2 _2e-6") || classee.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                        {

                                            firstlisteVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));
                                            listeNotReadOnly = firstlisteVocauxx.ToList();

                                        }

                                        if (classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                        {

                                            firstallDivElements = driverMessenger.FindElementsByXPath(classee + "//video");//_ox1 _21y0}
                                            listeVideoNotReadOnly = firstallDivElements.ToList();
                                        }

                                        if (!classee.Contains("_1mj2 _2e-6") || !classee.Contains("_1mj4 _2e-7") || classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                        {

                                            foreach (IWebElement el in driverMessenger.FindElementsByXPath(classee))
                                            {
                                                firstallOthers.Add(el);
                                                listeOthersNotReadOnly.Add(el);
                                            }

                                            //firstallOthers = driverMessenger.FindElementsByXPath(classee);//_ox1 _21y0}
                                            //listeOthersNotReadOnly = firstallOthers.ToList();

                                        }


                                    }

                                    while (true)
                                    {

                                        if (fromdate != null)
                                        {
                                            //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                            hauteurfromdate = fromdate.Location.Y;
                                            hauteurfromdateString = hauteurfromdate.ToString();

                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                            Thread.Sleep(500);

                                            //hauteurfromdate = fromdate.Location.Y;
                                            fromdate = null;
                                            STOP = false;
                                            //hauteur += 600;

                                        }
                                        else
                                        {

                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0," + hauteurr + ");", imageDown);

                                            Thread.Sleep(WAIT);
                                        }



                                        if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                        {

                                            try
                                            {
                                                driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                            }
                                            catch
                                            {

                                            }
                                        }
                                        //int hei = imageDown.Size.Height;
                                        messenger = driverMessenger.PageSource;

                                        if (!FASTMESSENGER)
                                            foreach (string classe in classesConnues)
                                            {

                                                if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                {


                                                    //pour récupérer les a contenant la longueur du message vocal
                                                    //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    IList<IWebElement> listeVocauxx = firstlisteVocauxx;
                                                    listeVocauxxOriginal = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                    if (!listeVocauxxOriginal.SequenceEqual(firstlisteVocauxx))
                                                    {
                                                        listeVocauxx = listeVocauxxOriginal;
                                                        firstlisteVocauxx = listeVocauxxOriginal;
                                                        listeNotReadOnly = listeVocauxx.ToList();

                                                    }


                                                    List<IWebElement> newListVocaux = listeNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                                    if (listeNotReadOnly.Count > 0)
                                                        foreach (IWebElement ell in newListVocaux)
                                                        {
                                                            //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                            //    break;
                                                            if (ell.Text == "")
                                                                continue;

                                                            string tentation = ell.ToString();
                                                            string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                            {
                                                                //string tentation = ell.ToString();
                                                                //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                                IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                string dateAudio = "";
                                                                string nomFichierAudio = "";

                                                                foreach (IWebElement el in els)
                                                                {
                                                                    if (el.GetAttribute("data-tooltip-content") != null)
                                                                    {

                                                                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                        {
                                                                            var texte = el.GetAttribute("data-tooltip-content");
                                                                            var position = el.Location;
                                                                            dateAudio = texte.Replace(":", "-");
                                                                            nomFichierAudio = "From";
                                                                            toRemove.Add(ell);
                                                                            break;
                                                                        }



                                                                    }
                                                                }
                                                                if (dateAudio == "")
                                                                {
                                                                    els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                    dateAudio = "";
                                                                    foreach (IWebElement el in els)
                                                                    {
                                                                        if (el.GetAttribute("data-tooltip-content") != null)
                                                                        {

                                                                            if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                            {
                                                                                var texte = el.GetAttribute("data-tooltip-content");
                                                                                var position = el.Location;
                                                                                dateAudio = texte.Replace(":", "-");
                                                                                nomFichierAudio = "To";
                                                                                break;
                                                                            }



                                                                        }
                                                                    }
                                                                }

                                                                string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];


                                                                if (duree.Length == 4)
                                                                    duree = "00:0" + duree;

                                                                if (audioVisibles.ContainsKey(idd))
                                                                {
                                                                    audioVisibless.Add(audioVisibles[idd]);
                                                                    continue;
                                                                }

                                                                try
                                                                {
                                                                    TimeSpan tempss;
                                                                    TimeSpan.TryParse(duree, out tempss);
                                                                    //DateTime temps;
                                                                    //DateTime.TryParse(duree, out temps);
                                                                    ell.Click();
                                                                    StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                    Stopwatch sw = new Stopwatch();
                                                                    sw.Start();
                                                                    //Thread.Sleep(500);
                                                                    while (true)
                                                                    {

                                                                        if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                        {
                                                                            if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                            {

                                                                                try
                                                                                {
                                                                                    driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            }
                                                                            break;
                                                                        }

                                                                        toRemove.Add(ell);
                                                                    }

                                                                    StopRecording();
                                                                    Thread.Sleep(1000);

                                                                    if (!audioVisibles.ContainsKey(idd))
                                                                    {
                                                                        audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                        audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                                        if (!classeTraitee.ContainsKey(idd))
                                                                        {
                                                                            classeTraitee.Add(idd, idd);
                                                                        }
                                                                    }



                                                                    j++;

                                                                }
                                                                catch (Exception exx)
                                                                {
                                                                    //StopRecording();
                                                                    //Thread.Sleep(5000);
                                                                }


                                                                //ell.Click();


                                                            }



                                                        }

                                                    listeNotReadOnly.RemoveAll(toRemove.Contains);

                                                    //pour cliquer sur le message vocal et jouer le son
                                                    //driverMessenger.FindElementsByXPath(classe)[0].Click();

                                                }


                                                if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                {

                                                    IList<IWebElement> listeVideos = firstallDivElements;
                                                    allDivElementsOriginal = driverMessenger.FindElementsByXPath(classe + "//video");

                                                    if (!allDivElementsOriginal.SequenceEqual(firstallDivElements))
                                                    {
                                                        listeVideos = allDivElementsOriginal;
                                                        firstlisteVocauxx = allDivElementsOriginal;
                                                        listeVideoNotReadOnly = listeVideos.ToList();

                                                    }


                                                    List<IWebElement> newListVideos = listeVideoNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();



                                                    //IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    if (listeVideoNotReadOnly.Count > 0)
                                                        for (int ii = newListVideos.Count() - 1; ii >= 0; ii--)
                                                        {
                                                            string tentation = newListVideos[ii].ToString();
                                                            string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            if (classeTraitee.ContainsKey(idd))
                                                            {
                                                                continue;
                                                            }


                                                            if (listeVideoNotReadOnly[ii].GetAttribute("class") != null)
                                                            {
                                                                //here the print statement will print the value of each div tag element
                                                                var tmp = newListVideos[ii].GetAttribute("class");

                                                                if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                                {
                                                                    //var video = driver.FindElementsByClassName(tmp);
                                                                    //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                    try
                                                                    {
                                                                        //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                        var tmpp = newListVideos[ii].GetAttribute("src");

                                                                        //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                        //    break;


                                                                        if (newListVideos[ii].Location.Y > 15 && newListVideos[ii].Location.Y < (resolutionEcran - 250))
                                                                        {


                                                                            if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                            {
                                                                                videosVisibles.Add(tmpp + ii, tmpp);

                                                                                if (!classeTraitee.ContainsKey(idd))
                                                                                {
                                                                                    classeTraitee.Add(idd, tmpp);
                                                                                }
                                                                            }

                                                                            toRemoveVideo.Add(newListVideos[ii]);
                                                                        }
                                                                        //break;



                                                                        //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                    }
                                                                    catch (Exception exx)
                                                                    {

                                                                    }

                                                                }
                                                            }

                                                            listeVideoNotReadOnly.RemoveAll(toRemoveVideo.Contains);

                                                        }




                                                }

                                                if (!classe.Contains("_1mj2 _2e-6") || !classe.Contains("_1mj4 _2e-7") || !classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                                {
                                                    foreach (IWebElement el in driverMessenger.FindElementsByXPath(classe))
                                                    {
                                                        //firstallOthers.Add(el);
                                                        //listeOthersNotReadOnly.Add(el);
                                                        allOthersOriginal.Add(el);
                                                    }




                                                }




                                            }//FIN FOREACH CLASSES CONNUES


                                        IList<IWebElement> listeOthers = firstallOthers;
                                        //allOthersOriginal = driverMessenger.FindElements(By.XPath(classe));

                                        if (!allOthersOriginal.SequenceEqual(firstallOthers))
                                        {
                                            listeOthers = allOthersOriginal;
                                            firstallOthers = allOthersOriginal;
                                            listeOthersNotReadOnly = listeOthers.ToList();

                                        }

                                        List<IWebElement> newList = listeOthersNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                        if (listeOthersNotReadOnly.Count > 0)

                                            //var messages = driverMessenger.FindElementsByXPath(classe);
                                            //object[] messagesToExtract = messages.ToArray();


                                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in newList)
                                            {

                                                string tentation = o.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if (classeTraitee.ContainsKey(idd))
                                                    continue;

                                                if (!dicoMessagesFrom.ContainsKey(idd))
                                                {
                                                    dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                }
                                                //else
                                                //    continue;


                                                if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                {
                                                    if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                    {
                                                        messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                        if (!classeTraitee.ContainsKey(idd))
                                                        {
                                                            classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                        }
                                                    }

                                                    toRemoveOthers.Add(o);
                                                }
                                                //else
                                                //    break;

                                                //messagesToExtract.ToList().Remove(o);

                                            }

                                        listeOthersNotReadOnly.RemoveAll(toRemoveOthers.Contains);
                                        allOthersOriginal = new List<IWebElement>();
                                        toRemoveOthers = new HashSet<IWebElement>();



                                        //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                        //imageDown = driver.FindElementById(idTmp);
                                        //Thread.Sleep(200);

                                        //if (textes != imageDown.Text)
                                        //    textes = imageDown.Text + "\n";

                                        messenger = driverMessenger.PageSource;
                                        //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                        //on récupère la valeur Top de la scrollBar

                                        topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                        //on récupère la valeur Top de la scrollBar

                                        top = "";
                                        try
                                        {
                                            foreach (string t in topScrollBar)
                                            {
                                                string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                    continue;

                                                top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                                //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                                //    continue;

                                                //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                            }

                                            if (top == topPrec)
                                                break;

                                            topPrec = top;
                                        }
                                        catch
                                        {

                                        }

                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        //imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                        Thread.Sleep(500);


                                        //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                        Thread.Sleep(500);

                                        //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                        string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                        //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                        pathToFolder = pathToSaveScreenshot;

                                        if (!FASTMESSENGER)
                                            foreach (string cle in messagesVisibles.Keys)
                                            {

                                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                                {

                                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                    string tmp = "";
                                                    foreach (string li in lignes)
                                                    {
                                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                    }



                                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                }

                                                //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                            }

                                        if (!FASTMESSENGER)
                                            if (videosVisibles.Count > 0)
                                                foreach (string cle in videosVisibles.Keys)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{

                                                    videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                        if (!FASTMESSENGER)
                                            if (audioVisibless.Count > 0)
                                                foreach (string cle in audioVisibless)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{
                                                    if (!audioVisiblesForFile.ContainsKey(cle))
                                                        audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                        messagesVisibles = new Dictionary<string, string>();
                                        videosVisibles = new Dictionary<string, string>();
                                        //audioVisibles = new Dictionary<string, string>();
                                        hauteur += 600;
                                        hauteurr = 450;


                                        i++;
                                    }
                                    ForGrid forGrid = new ForGrid();
                                    forGrid.PathToFolder = pathToFolder;
                                    forGrid.Url = link;

                                    imageDown = driverMessenger.FindElementById(idTmp);
                                    textes = imageDown.Text + "\n";
                                    

                                    backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);



                                }


                            }
                            catch (Exception ex)//FIN TRY TIMEOUT-----------------------------------------------------------------------
                            {
                                //MessageBox.Show("error dans Messenger " + ex.Message);

                                       Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                        Thread.Sleep(500);


                                        //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + 1 + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);

                                        Thread.Sleep(500);

                                        string topPrec = "";
                                        textes = imageDown.Text;

                                        
                                        int j = 1;
                                        int hauteurfromdate = 0;
                                        string hauteurfromdateString = "";

                                        Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                        long innerHeightt = (long)innerHeight;
                                        long scrolll = (long)innerHeight;
                                        long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                        //on initialise

                                        IList<IWebElement> firstlisteVocauxx = null;
                                        IList<IWebElement> firstallDivElements = null;
                                        IList<IWebElement> firstallOthers = new List<IWebElement>();
                                        List<IWebElement> listeNotReadOnly = null;
                                        List<IWebElement> listeOthersNotReadOnly = new List<IWebElement>();
                                        List<IWebElement> listeVideoNotReadOnly = new List<IWebElement>();
                                        var toRemove = new HashSet<IWebElement>();
                                        var toRemoveOthers = new HashSet<IWebElement>();
                                        var toRemoveVideo = new HashSet<IWebElement>();

                                        IList<IWebElement> listeVocauxxOriginal;
                                        IList<IWebElement> allDivElementsOriginal = null;
                                        IList<IWebElement> allOthersOriginal = new List<IWebElement>();

                                        foreach (string classee in classesConnues)
                                        {
                                            if (classee.Contains("_1mj2 _2e-6") || classee.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                            {

                                                firstlisteVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));
                                                listeNotReadOnly = firstlisteVocauxx.ToList();

                                            }

                                            if (classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {

                                                firstallDivElements = driverMessenger.FindElementsByXPath(classee + "//video");//_ox1 _21y0}
                                                listeVideoNotReadOnly = firstallDivElements.ToList();
                                            }

                                            if (!classee.Contains("_1mj2 _2e-6") || !classee.Contains("_1mj4 _2e-7") || classee.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                            {
                                                
                                                foreach(IWebElement el in driverMessenger.FindElementsByXPath(classee))
                                                {
                                                    firstallOthers.Add(el);
                                                    listeOthersNotReadOnly.Add(el);
                                                }
                                                
                                                //firstallOthers = driverMessenger.FindElementsByXPath(classee);//_ox1 _21y0}
                                                //listeOthersNotReadOnly = firstallOthers.ToList();

                                            }

                                                
                                        }

                                            while (true)
                                            {
                                            
                                            if (fromdate != null)
                                            {
                                                //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                                hauteurfromdate = fromdate.Location.Y;
                                                hauteurfromdateString = hauteurfromdate.ToString();

                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                                Thread.Sleep(500);

                                                //hauteurfromdate = fromdate.Location.Y;
                                                fromdate = null;
                                                STOP = false;
                                                //hauteur += 600;

                                            }
                                            else
                                            {

                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0," + hauteurr + ");", imageDown);

                                                Thread.Sleep(WAIT);
                                            }



                                            if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                            {

                                                try
                                                {
                                                    driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                }
                                                catch
                                                {

                                                }
                                            }
                                            //int hei = imageDown.Size.Height;
                                            messenger = driverMessenger.PageSource;

                                            if(!FASTMESSENGER)
                                            foreach (string classe in classesConnues)
                                            {

                                                if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                {


                                                        //pour récupérer les a contenant la longueur du message vocal
                                                        //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        IList<IWebElement> listeVocauxx = firstlisteVocauxx;
                                                        listeVocauxxOriginal = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                        if(!listeVocauxxOriginal.SequenceEqual(firstlisteVocauxx))
                                                        {
                                                            listeVocauxx = listeVocauxxOriginal;
                                                            firstlisteVocauxx = listeVocauxxOriginal;
                                                            listeNotReadOnly = listeVocauxx.ToList();

                                                        }


                                                        List<IWebElement> newListVocaux = listeNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                                    if (listeNotReadOnly.Count > 0)
                                                    foreach (IWebElement ell in newListVocaux)
                                                    {
                                                        //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                        //    break;
                                                        if (ell.Text == "")
                                                            continue;

                                                        string tentation = ell.ToString();
                                                        string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                        if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                        {
                                                            //string tentation = ell.ToString();
                                                            //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                            IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            string dateAudio = "";
                                                            string nomFichierAudio = "";

                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "From";
                                                                        toRemove.Add(ell);
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                            if (dateAudio == "")
                                                            {
                                                                els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                dateAudio = "";
                                                                foreach (IWebElement el in els)
                                                                {
                                                                    if (el.GetAttribute("data-tooltip-content") != null)
                                                                    {

                                                                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                        {
                                                                            var texte = el.GetAttribute("data-tooltip-content");
                                                                            var position = el.Location;
                                                                            dateAudio = texte.Replace(":", "-");
                                                                            nomFichierAudio = "To";
                                                                            break;
                                                                        }



                                                                    }
                                                                }
                                                            }

                                                            string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];


                                                            if (duree.Length == 4)
                                                                duree = "00:0" + duree;

                                                            if (audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibless.Add(audioVisibles[idd]);
                                                                continue;
                                                            }

                                                            try
                                                            {
                                                                TimeSpan tempss;
                                                                TimeSpan.TryParse(duree, out tempss);
                                                                //DateTime temps;
                                                                //DateTime.TryParse(duree, out temps);
                                                                ell.Click();
                                                                StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                Stopwatch sw = new Stopwatch();
                                                                sw.Start();
                                                                //Thread.Sleep(500);
                                                                while (true)
                                                                {

                                                                    if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                    {
                                                                        if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                        {

                                                                            try
                                                                            {
                                                                                driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                            }
                                                                            catch
                                                                            {

                                                                            }
                                                                        }
                                                                        break;
                                                                    }

                                                                        toRemove.Add(ell);
                                                                }

                                                                StopRecording();
                                                                Thread.Sleep(1000);

                                                                if (!audioVisibles.ContainsKey(idd))
                                                                {
                                                                    audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                    audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                        
                                                                        if (!classeTraitee.ContainsKey(idd))
                                                                        {
                                                                            classeTraitee.Add(idd, idd);
                                                                        }
                                                                }



                                                                j++;

                                                            }
                                                            catch (Exception exx)
                                                            {
                                                                //StopRecording();
                                                                //Thread.Sleep(5000);
                                                            }


                                                            //ell.Click();


                                                        }
                                                            
                                                            
                                                            
                                                        }

                                                        listeNotReadOnly.RemoveAll(toRemove.Contains);

                                                        //pour cliquer sur le message vocal et jouer le son
                                                        //driverMessenger.FindElementsByXPath(classe)[0].Click();

                                                    }


                                                if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                {

                                                        IList<IWebElement> listeVideos = firstallDivElements;
                                                        allDivElementsOriginal = driverMessenger.FindElementsByXPath(classe + "//video");

                                                        if (!allDivElementsOriginal.SequenceEqual(firstallDivElements))
                                                        {
                                                            listeVideos = allDivElementsOriginal;
                                                            firstlisteVocauxx = allDivElementsOriginal;
                                                            listeVideoNotReadOnly = listeVideos.ToList();

                                                        }


                                                        List<IWebElement> newListVideos = listeVideoNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();



                                                        //IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    if (listeVideoNotReadOnly.Count > 0)
                                                    for (int ii = newListVideos.Count() - 1; ii >=0; ii--)
                                                    {
                                                            string tentation = newListVideos[ii].ToString();
                                                            string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            if (classeTraitee.ContainsKey(idd))
                                                            {
                                                                continue;
                                                            }


                                                        if (listeVideoNotReadOnly[ii].GetAttribute("class") != null)
                                                        {
                                                            //here the print statement will print the value of each div tag element
                                                            var tmp = newListVideos[ii].GetAttribute("class");

                                                            if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                            {
                                                                //var video = driver.FindElementsByClassName(tmp);
                                                                //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                try
                                                                {
                                                                    //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                    var tmpp = newListVideos[ii].GetAttribute("src");

                                                                    //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                    //    break;


                                                                    if (newListVideos[ii].Location.Y > 15 && newListVideos[ii].Location.Y < (resolutionEcran - 250))
                                                                    {
                                                                           

                                                                        if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                        {
                                                                            videosVisibles.Add(tmpp + ii, tmpp);

                                                                                if (!classeTraitee.ContainsKey(idd))
                                                                                {
                                                                                    classeTraitee.Add(idd, tmpp);
                                                                                }
                                                                        }

                                                                    toRemoveVideo.Add(newListVideos[ii]);
                                                                    }
                                                                    //break;



                                                                    //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                }
                                                                catch (Exception exx)
                                                                {

                                                                }

                                                            }
                                                        }

                                                     listeVideoNotReadOnly.RemoveAll(toRemoveVideo.Contains);

                                                    }




                                                }

                                                    if (!classe.Contains("_1mj2 _2e-6") || !classe.Contains("_1mj4 _2e-7") || !classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                                    {
                                                        foreach (IWebElement el in driverMessenger.FindElementsByXPath(classe))
                                                        {
                                                            //firstallOthers.Add(el);
                                                            //listeOthersNotReadOnly.Add(el);
                                                            allOthersOriginal.Add(el);
                                                        }




                                                    }

                                                       


                                            }//FIN FOREACH CLASSES CONNUES


                                            IList<IWebElement> listeOthers = firstallOthers;
                                            //allOthersOriginal = driverMessenger.FindElements(By.XPath(classe));

                                            if (!allOthersOriginal.SequenceEqual(firstallOthers))
                                            {
                                                listeOthers = allOthersOriginal;
                                                firstallOthers = allOthersOriginal;
                                                listeOthersNotReadOnly = listeOthers.ToList();

                                            }

                                            List<IWebElement> newList = listeOthersNotReadOnly.Where(o => o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250)).ToList();


                                            if (listeOthersNotReadOnly.Count > 0)

                                                //var messages = driverMessenger.FindElementsByXPath(classe);
                                                //object[] messagesToExtract = messages.ToArray();


                                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in newList)
                                                {

                                                    string tentation = o.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    if (classeTraitee.ContainsKey(idd))
                                                        continue;

                                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    {
                                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                    //else
                                                    //    continue;


                                                    if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                        {
                                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                            if (!classeTraitee.ContainsKey(idd))
                                                            {
                                                                classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                            }
                                                        }

                                                        toRemoveOthers.Add(o);
                                                    }
                                                    //else
                                                    //    break;

                                                    //messagesToExtract.ToList().Remove(o);

                                                }

                                            listeOthersNotReadOnly.RemoveAll(toRemoveOthers.Contains);
                                            allOthersOriginal = new List<IWebElement>();
                                            toRemoveOthers = new HashSet<IWebElement>();



                                            //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                            //imageDown = driver.FindElementById(idTmp);
                                            //Thread.Sleep(200);

                                            //if (textes != imageDown.Text)
                                            //    textes = imageDown.Text + "\n";

                                            messenger = driverMessenger.PageSource;
                                            //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            top = "";
                                            try
                                            {
                                                foreach (string t in topScrollBar)
                                                {
                                                    string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                    if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                        continue;

                                                    top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                                    //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                                    //    continue;

                                                    //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                                }

                                                if (top == topPrec)
                                                    break;

                                                topPrec = top;
                                            }
                                            catch
                                            {

                                            }

                                            imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                            //imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                                            Thread.Sleep(500);


                                            //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                            imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                            Thread.Sleep(500);

                                            //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                            string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                            //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                            pathToFolder = pathToSaveScreenshot;

                                            if(!FASTMESSENGER)
                                            foreach (string cle in messagesVisibles.Keys)
                                            {

                                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                                {

                                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                    string tmp = "";
                                                    foreach (string li in lignes)
                                                    {
                                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                    }



                                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                }

                                                //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                            }

                                            if (!FASTMESSENGER)
                                               if (videosVisibles.Count > 0)
                                                foreach (string cle in videosVisibles.Keys)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{

                                                    videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                            if (!FASTMESSENGER)
                                                if (audioVisibless.Count > 0)
                                                foreach (string cle in audioVisibless)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{
                                                    if (!audioVisiblesForFile.ContainsKey(cle))
                                                        audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                            messagesVisibles = new Dictionary<string, string>();
                                            videosVisibles = new Dictionary<string, string>();
                                            //audioVisibles = new Dictionary<string, string>();
                                            hauteur += 600;
                                            hauteurr = 450;


                                            i++;
                                        }
                                        ForGrid forGrid = new ForGrid();
                                        forGrid.PathToFolder = pathToFolder;
                                        forGrid.Url = link;

                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";
                                        

                                        backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);

                                

                            }

                                string codePagee = driverMessenger.PageSource;

                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages.txt", false))
                                {
                                    //if (File.Exists(saveFileDialog1.FileName))
                                    //    File.Delete(saveFileDialog1.FileName);

                                    file.Write(textes);
                                }

                                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages_Bis.txt", false))
                                //{
                                //    //if (File.Exists(saveFileDialog1.FileName))
                                //    //    File.Delete(saveFileDialog1.FileName);

                                //    textes = "";
                                //    foreach(string t in messagesVisibles.Values)
                                //    {
                                //        textes += t + "\n";
                                //    }

                                    
                                //    file.Write(textes);
                                //}

                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages_With_Screenshots.txt", false))
                                {
                                    textes = "";
                                    foreach (string t in messagesVisiblesForFile.Values)
                                    {
                                        textes += t;
                                    }


                                    file.Write(textes);
                                    messagesVisiblesForFile = new Dictionary<string, string>();
                                }

                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Videos_With_Screenshots.txt", false))
                                {
                                    if (videosVisiblesForFile.Count > 0)
                                    {
                                        textes = "";
                                        foreach (string t in videosVisiblesForFile)
                                        {
                                            textes += t;
                                        }


                                        file.Write(textes);
                                        
                                    }
                                        
                                            

                                }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Audio_With_Screenshots.txt", false))
                            {
                                if (audioVisiblesForFile.Count > 0)
                                {
                                    textes = "";
                                    foreach (string t in audioVisiblesForFile.Values)
                                    {
                                        textes += t;
                                    }


                                    file.Write(textes);

                                }



                            }

                            if (!Directory.Exists(pathToSaveScreenshot + "\\Videos\\"))
                                {
                                    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                    Directory.CreateDirectory(pathToSaveScreenshot + "\\Videos\\");
                                }

                                using (var client = new WebClient())
                                {

                                    try

                                    {
                                        Dictionary<string, string> dico = new Dictionary<string, string>();
                                        foreach (string t in videosVisiblesForFile)
                                        {
                                            //FileInfo fileinfo = new FileInfo(t);
                                            string keyy = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];

                                            if (!dico.ContainsKey(keyy))
                                            {
                                                dico.Add(keyy, t);
                                                string nomFichier = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];
                                                client.DownloadFile(t, pathToSaveScreenshot + "\\Videos\\" + nomFichier);
                                                Thread.Sleep(1000);
                                            }
                                            

                                        }

                                        videosVisiblesForFile = new List<string>();
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DES VIDEOS");
                                        //return;
                                    }
                                }


                            //on essaie de récupérer les documents partagés
                            if (!FASTMESSENGER)
                            try
                            {
                                IList<IWebElement> tmpp = driverMessenger.FindElements(By.ClassName("uiScrollableAreaContent"));
                                foreach (IWebElement div in tmpp)
                                {
                                    if (!div.Text.StartsWith("Information"))
                                        continue;

                                    IList<IWebElement> docPartagess = div.FindElements(By.TagName("a"));

                                    foreach (IWebElement elpart in docPartagess)
                                    {
                                        if (elpart.GetAttribute("href") != "")
                                        {
                                            docPartages.Add(elpart.GetAttribute("href"));
                                        }
                                    }
                                }

                            }
                            catch
                            {

                            }

                            if (!FASTMESSENGER)
                            if (docPartages.Count > 0)
                            {
                                if (!Directory.Exists(pathToSaveScreenshot + "\\Documents_Partages\\"))
                                {
                                    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                    Directory.CreateDirectory(pathToSaveScreenshot + "\\Documents_Partages\\");
                                }

                                using (var client = new WebClient())
                                {

                                    try

                                    {
                                        
                                        foreach (string t in docPartages)
                                        {
                                            try
                                            {
                                                //FileInfo fichier = new FileInfo(t);
                                                string urll = t;
                                                string nomFichier = t.Split(new string[] { "%3F" },StringSplitOptions.RemoveEmptyEntries)[0];
                                                
                                                urll = urll.Replace("%3A", ":").Replace("%3D", "=").Replace("%2F", "/").Replace("%3F", "?").Replace("%26", "&");
                                                
                                                if (urll.Contains("https://l.facebook.com/l.php?u="))
                                                {
                                                    nomFichier = nomFichier.Substring(nomFichier.LastIndexOf("%2F") + 3).Trim();
                                                    urll = urll.Substring(urll.IndexOf("https://l.facebook.com/l.php?u=") + 31);
                                                }
                                                //else
                                                //    nomFichier = nomFichier.Split('?')[0].Substring(nomFichier.LastIndexOf("/") + 1).Trim();



                                                client.DownloadFile(urll, pathToSaveScreenshot + "\\Documents_Partages\\" + nomFichier);
                                                Thread.Sleep(1000);
                                            }

                                            catch
                                            {

                                            }

                                        }

                                        docPartages = new List<string>();
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DES VIDEOS");
                                        //return;
                                    }
                                }
                            }




                                Thread.Sleep(2500);

                                textes = "";
                                messagesVisiblesForFile = new Dictionary<string, string>();
                                messagesVisibles = new Dictionary<string, string>();
                                videosVisibles = new Dictionary<string, string>();
                                videosVisiblesForFile = new List<string>();
                                audioVisiblesForFile = new Dictionary<string, string>();
                                messagesVisibles = new Dictionary<string, string>();
                                videosVisibles = new Dictionary<string, string>();
                                audioVisibles = new Dictionary<string, string>();
                                audioVisibless = new List<string>();
                            //idFictif = 1;
                        }

                            textes = "";
                            messagesVisiblesForFile = new Dictionary<string, string>();
                            audioVisiblesForFile = new Dictionary<string, string>();
                            videosVisiblesForFile = new List<string>();
                            messagesVisibles = new Dictionary<string, string>();
                            videosVisibles = new Dictionary<string, string>();
                            audioVisibles = new Dictionary<string, string>();
                            audioVisibless = new List<string>();
                            classeTraitee = new Dictionary<string, string>();
                        
                            idFictif = 1;
                        }


                }
                    catch
                    {

                    }


                    try
                    {
                        Object lastHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");

                        while (true)
                        {
                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            Thread.Sleep(2000);

                            Object newHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");
                            if (newHeight.Equals(lastHeight))
                            {
                                break;
                            }
                            lastHeight = newHeight;
                        }
                    }
                    catch
                    {
                        //e.printStackTrace();
                    }



            }
            catch 
            {

                    //driverMessenger.Quit();
            }




            //}
            //Thread.Sleep(2000);
            backgroundWorkerGetMessenger.ReportProgress(-105);
            Thread.Sleep(2000);
            //backgroundWorker1.CancelAsync();

            if (backgroundWorkerGetMessenger != null && backgroundWorkerGetMessenger.IsBusy)
                backgroundWorkerGetMessenger.CancelAsync();

            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;
        }
        private void MessengerForTesting(Dictionary<string, string> dicoMessenger, DateTime datumm)
        {


            if (driver == null)
            {
                InitializeDriver();
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


                //while (!isElementPresentByID(driver, "email"))
                ////{
                //// 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");
                //// 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);
                ////}


                //// 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                //// 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                //// 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                ////searchButton.Click();
                Thread.Sleep(2500);
            }

            //string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            //pour cacher fenetre DOS
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;


            // 1. Maximize the browser
            //driver.Manage().Window.Maximize();
            //driverMessenger.Close();
            //driverMessenger = new ChromeDriver(driverService, chromeOptions);
            driverMessenger = driver;

            // 2. Go to the "Google" homepage
            //driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


            //    //while (!isElementPresentByID(driver, "email"))
            //    ////{
            //    //// 3. Find the username textbox (by ID) on the homepage
            //    var userNameBox = driverMessenger.FindElementById("email");
            //    //// 4. Enter the text (to search for) in the textbox
            //    userNameBox.SendKeys(textBoxUSERNAME.Text);
            //    ////}


            //    //// 3. Find the username textbox (by ID) on the homepage
            //    var userpasswordBox = driverMessenger.FindElementById("pass");

            //    //// 4. Enter the text (to search for) in the textbox
            //    userpasswordBox.SendKeys(textBoxPASSWORD.Text);
            //    Thread.Sleep(5000);

            //    //// 5. Find the search button (by Name) on the homepage
            //    driverMessenger.FindElementById("loginbutton").Click();
            //    ////searchButton.Click();
            //    Thread.Sleep(2500);







            try
            {


                string targetName = textBoxops.Text;
                string textes = "";
                string messagesFromInString = "";
                string messagesVisiblesWithScreenshots = "";
                string pathToFolder = "";
                List<IWebElement> earlier = new List<IWebElement>();
                List<DateTime> sameTime = new List<DateTime>();
                int hauteurr = 450;
                //var imageDown = "";
                Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                Dictionary<string, string> dicoMessagesTo = new Dictionary<string, string>();
                Dictionary<string, string> dicoPictures = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                Dictionary<string, string> videosVisibles = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();
                List<string> videosVisiblesForFile = new List<string>();
                Dictionary<string, string> audioVisiblesForFile = new Dictionary<string, string>();
                Dictionary<string, string> audioVisibles = new Dictionary<string, string>();
                List<string> audioVisibless = new List<string>();
                List<string> audioVisiblessForFile = new List<string>();
                List<string> docPartages = new List<string>();
                Dictionary<string, string> mois = new Dictionary<string, string>();
                Dictionary<string, string> classeTraitee = new Dictionary<string, string>();
                int WAIT = 500;
                mois.Add("jan", "01");
                mois.Add("fév", "02");
                mois.Add("fev", "02");
                mois.Add("fèv", "02");
                mois.Add("mar", "03");
                mois.Add("avr", "04");
                mois.Add("mai", "05");
                mois.Add("jui", "06");
                mois.Add("juil", "07");
                mois.Add("janvier", "01");
                mois.Add("février", "02");
                mois.Add("mars", "03");
                mois.Add("avril", "04");
                mois.Add("juin", "06");
                mois.Add("juillet", "07");
                mois.Add("août", "08");
                mois.Add("septembre", "09");
                mois.Add("octobre", "10");
                mois.Add("novembre", "11");
                mois.Add("décembre", "12");



                List<string> classesConnues = new System.Collections.Generic.List<string>();
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("//div[@class='_52mr _2poz _ui9 _4skb']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
                classesConnues.Add("//div[@class='clearfix _o46 _3erg _3i_m _nd_ _q4a _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("//div[@class='_2poz _52mr _ui9 _2n8h _2n8i _5fk1']");//_2poz _52mr _ui9 _2n8h _2n8i _5fk1 Vidéo ?_ccq _4tsk _3o67 _52mr _1byr _4-od
                classesConnues.Add("//div[@class='_1mj2 _2e-6']");
                classesConnues.Add("//div[@class='_1mj4 _2e-7']");//_1mj4 _2e-7
                classesConnues.Add("//div[@class='_3058 _15gf']");//_3058 _15gf
                                                                  //_1mjb _454y _3czg _2poz _ui9
                                                                  //_1mjb _454y _3czg _2poz _ui9
                                                                  //_hh7 _6ybn _s1- _52mr _1fz8 _1nqp

                List<string> classesConnuess = new System.Collections.Generic.List<string>();
                classesConnuess.Add("clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr");
                classesConnuess.Add("clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr");
                classesConnuess.Add("clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr _ylc");
                classesConnuess.Add("clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr");
                classesConnuess.Add("clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr _ylc");
                classesConnuess.Add("_52mr _2poz _ui9 _4skb");
                classesConnuess.Add("clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr");
                classesConnuess.Add("clearfix _o46 _3erg _3i_m _nd_ _q4a _8lma direction_ltr text_align_ltr _ylc");
                classesConnuess.Add("_2poz _52mr _ui9 _2n8h _2n8i _5fk1");//_2poz _52mr _ui9 _2n8h _2n8i _5fk1 Vidéo ?_ccq _4tsk _3o67 _52mr _1byr _4-od
                classesConnuess.Add("_1mj2 _2e-6");
                classesConnuess.Add("_1mj4 _2e-7");//_1mj4 _2e-7
                classesConnuess.Add("_3058 _15gf");

                //récupération des années 
                //var years = driver.FindElementByXPath("rightColWrap']").Text;
                //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //if (!Directory.Exists(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper()))
                //    Directory.CreateDirectory(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper());

                //driver.Navigate().GoToUrl("https://www.facebook.com/messages/t");//https://www.facebook.com/messages/t/MOD.orga
                //Thread.Sleep(5000);



                try
                {

                    bool STOP = false;
                    string messenger = "";
                    int idFictif = 1;
                    string idFictifString = "";
                    IWebElement fromdate = null;

                    if (FASTMESSENGER)
                        WAIT = 500;
                    else
                        WAIT = 2000;

                    if (dicoMessenger.Count > 0)
                    {
                        foreach (string link in dicoMessenger.Keys)
                        {



                            string pathToSaveScreenshot = "";

                            if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link];
                            }
                            else
                            if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                            {
                                idFictif++;
                                idFictifString = idFictif.ToString();
                                Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString);
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString;
                            }
                            else
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link];



                            if (!Directory.Exists(pathToSaveScreenshot + "\\Audio\\"))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSaveScreenshot + "\\Audio\\");
                            }

                            var imageDown = driverMessenger.FindElement(By.Id("facebook"));
                            int hauteur = 400;
                            int resolutionEcran = resolution.Height;
                            string[] topScrollBar = null;
                            string firstValueTopBar = "";
                            string top = "";
                            string hauteurScroll = "";
                            string newHauteurScroll = "";
                            int i = 1;
                            string idTmp = "";
                            //DEPART TRY TIMEOUT------------------------------------------------------------------------------
                            try
                            {
                                driverMessenger.Manage().Timeouts().ImplicitWait.Add(System.TimeSpan.FromSeconds(300));
                                driverMessenger.Navigate().GoToUrl(link);
                                Thread.Sleep(5000);

                                messenger = driverMessenger.PageSource;





                                string[] id = messenger.Split(new String[] { "class=\"uiScrollableAreaWrap scrollable\"" }, StringSplitOptions.RemoveEmptyEntries);
                                topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                //on récupère la valeur Top de la scrollBar


                                try
                                {
                                    foreach (string t in topScrollBar)
                                    {
                                        string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                        if ((!t.Contains("top:") && !t.Contains("uiScrollableAreaWrap")) || t.StartsWith("<html"))
                                            continue;

                                        firstValueTopBar = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                        hauteurScroll = tmp.Substring(tmp.IndexOf("height: ") + 8).Split(';')[0];
                                    }


                                }
                                catch
                                {

                                }
                                //try
                                //{
                                //    foreach (string t in topScrollBar)
                                //    {
                                //        if (!t.Contains("top:"))
                                //            continue;

                                //        top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                //        firstValueTopBar = top;
                                //    }
                                //}
                                //catch
                                //{

                                //}

                                //on récupère l'id dynamique du composant

                                foreach (string idd in id)
                                {
                                    if (!idd.StartsWith("id") && !idd.Contains("Messages"))
                                        continue;


                                    idTmp = idd.Substring(idd.IndexOf("id=\"") + 4).Split('"')[0];
                                }

                                try
                                {
                                    imageDown = driverMessenger.FindElementById(idTmp);
                                    var test = driverMessenger.FindElements(By.ClassName("uiScrollableAreaGripper"));//uiScrollableAreaTrack hidden_elem
                                    //var test2 = driverMessenger.FindElements(By.CssSelector("uiScrollableAreaTrack hidden_elem"));
                                    int lastHeight = imageDown.Size.Height;





                                    int width = driverMessenger.Manage().Window.Size.Width;
                                    int height = driverMessenger.Manage().Window.Size.Height;


                                    int hauteurtotale = resolution.Height;
                                    resolutionEcran = resolution.Height;
                                    int scroll = 0;
                                    Int32.TryParse(firstValueTopBar.Replace(".", "").Replace("px", ""), out scroll);



                                    //ON SCROLL JUSQUAU DEBUT DE LA PAGE LE PLUS LOIN POSSIBLE

                                    //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", listeTimes[0 ]);
                                    //Thread.Sleep(500);

                                    if (firstValueTopBar != "")
                                    {
                                        try
                                        {
                                            
                                            //tout d'abord on s'assure d'etre en bas de page grace à l'élément time
                                            
                                            
                                            while (top.Trim() != "0px")
                                            //while (newHauteurScroll != hauteurScroll)
                                            {
                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll) + ");", imageDown);
                                                //Thread.Sleep(2500);

                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - (scroll * scroll)) + ");", imageDown);
                                                //Thread.Sleep(2500);


                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                Thread.Sleep(5000);



                                                //calcul du temps
                                                if (datumm.Year != 1900)
                                                {
                                                    IList<IWebElement> listeTimes = driver.FindElements(By.TagName("time"));
                                                    Thread.Sleep(2000);

                                                    string dernieredate = "";
                                                    string datum = "";

                                                    if (listeTimes[0].Text.ToLower().Contains("à"))
                                                    {
                                                        datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                        if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                        {
                                                            datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                                                        }

                                                    }
                                                    else
                                                        datum = listeTimes[0].Text.Split(' ')[0];

                                                    CultureInfo culture = new CultureInfo("fr-FR");
                                                    DateTime testDatumValable;
                                                    if (!DateTime.TryParse(datum, out testDatumValable))
                                                        continue;

                                                    DateTime firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste
                                                    DateTime tempss;


                                                    tempss = Convert.ToDateTime(datum, culture);
                                                    DateTime date2 = new DateTime(datumm.Year, datumm.Month, datumm.Day);



                                                    try
                                                    {


                                                        foreach (IWebElement el in listeTimes)
                                                        {
                                                            if (DateTime.Compare(firstDate, date2) == 0)
                                                                break;

                                                            //DateTime firstDate;



                                                            datum = "";
                                                            if (listeTimes[0].Text.ToLower().Contains("à"))
                                                            {
                                                                datum = listeTimes[0].Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                                if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                                {
                                                                    datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);
                                                                }

                                                            }
                                                            else
                                                                datum = listeTimes[0].Text.Split(' ')[0];


                                                            if (DateTime.TryParse(datum, out testDatumValable))
                                                                firstDate = Convert.ToDateTime(datum, culture);// premiere date dans la liste
                                                            else
                                                                continue;

                                                            datum = "";
                                                            if (el.Text.ToLower().Contains("à"))
                                                            {
                                                                datum = el.Text.ToLower().Split('à')[0].Trim().Replace(" ", "/");
                                                                if (mois.ContainsKey(datum.Split('/')[1].ToLower()))
                                                                {
                                                                    datum = datum.ToLower().Replace(datum.Split('/')[1].ToLower(), mois[datum.Split('/')[1].ToLower()]);

                                                                    if (Convert.ToInt32(datum.Split('/')[0]) < 10 && !datum.Split('/')[0].StartsWith("0"))
                                                                        datum = "0" + datum;
                                                                }

                                                            }
                                                            else
                                                                datum = el.Text.Split(' ')[0];

                                                            try
                                                            {
                                                                tempss = Convert.ToDateTime(datum, culture);
                                                            }
                                                            catch
                                                            {
                                                                continue;
                                                            }

                                                            int result = 0;
                                                            try
                                                            {


                                                                result = DateTime.Compare(tempss, date2);
                                                                string relationship;

                                                                if (result < 0)
                                                                {
                                                                    earlier.Add(el);
                                                                    relationship = "is earlier than";
                                                                }
                                                                else
                                                                if (result == 0)
                                                                {
                                                                    relationship = "is the same time as";
                                                                    sameTime.Add(tempss);

                                                                    if (firstDate != tempss)
                                                                    {
                                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true) + 1600;", el);
                                                                        //Thread.Sleep(500);
                                                                        STOP = true;
                                                                        top = "0px";
                                                                        fromdate = el;
                                                                        break;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    relationship = "is later than";
                                                                }



                                                            }
                                                            catch
                                                            {

                                                            }


                                                        }

                                                        if (!STOP)
                                                        {
                                                            int result = DateTime.Compare(firstDate, date2);
                                                            if (result < 0)
                                                            {
                                                                STOP = true;
                                                                top = "0px";
                                                                fromdate = earlier[earlier.Count() - 1];

                                                            }
                                                        }

                                                    }
                                                    catch
                                                    {

                                                    }

                                                    if (STOP)
                                                        break;

                                                }//FIN DU IF DATE


                                                if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                {

                                                    try
                                                    {
                                                        driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                    }
                                                    catch
                                                    {

                                                    }
                                                }


                                                hauteur += 600;
                                                hauteurtotale += resolution.Height + (resolution.Height / 2);

                                                messenger = driverMessenger.PageSource;

                                                top = "";
                                                topScrollBar = messenger.Split(new String[] { "<div class=\"uiScrollableAreaGripper" }, StringSplitOptions.RemoveEmptyEntries);
                                                try
                                                {
                                                    foreach (string t in topScrollBar)
                                                    {
                                                        string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                        if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                            continue;

                                                        int heightt = 0;
                                                        top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                                        newHauteurScroll = tmp.Substring(tmp.IndexOf("height: ") + 8).Split(';')[0];
                                                        Int32.TryParse(top.Trim().Replace(".", "").Replace("px", ""), out heightt);
                                                        scroll = scroll - hauteur;
                                                        break;
                                                    }

                                                    if (top.Trim() == "0px")
                                                    {
                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) + ");", imageDown);
                                                        //Thread.Sleep(2000);
                                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                        Thread.Sleep(5000);

                                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                        Thread.Sleep(5000);

                                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                                        Thread.Sleep(5000);

                                                        hauteurScroll = newHauteurScroll;
                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll * scroll) + ");", imageDown);
                                                        //Thread.Sleep(2500);

                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (0 - scroll) + ");", imageDown);
                                                        // Thread.Sleep(2500);

                                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - 2 * hauteur) + ");", imageDown);
                                                        //Thread.Sleep(2000);

                                                        messenger = driverMessenger.PageSource;

                                                        top = "";
                                                        topScrollBar = messenger.Split(new String[] { "<div class=\"uiScrollableAreaGripper" }, StringSplitOptions.RemoveEmptyEntries);


                                                        try
                                                        {
                                                            foreach (string tt in topScrollBar)
                                                            {
                                                                string tmpp = tt.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                                if (!tmpp.Contains("top:") || tmpp.Contains("hidden"))
                                                                    continue;

                                                                top = tmpp.Substring(tmpp.IndexOf("top: ") + 4).Split(';')[0];
                                                                newHauteurScroll = tmpp.Substring(tmpp.IndexOf("height: ") + 8).Split(';')[0];
                                                                int heightt = 0;
                                                                Int32.TryParse(top.Trim().Replace(".", "").Replace("px", ""), out heightt);
                                                                scroll = scroll - hauteur;
                                                                break;
                                                            }


                                                        }
                                                        catch
                                                        {
                                                            top = "";
                                                        }

                                                        hauteur += 600;
                                                        hauteurtotale += resolution.Height + (resolution.Height / 2);
                                                    }


                                                }
                                                catch
                                                {

                                                }

                                                Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                                int innerHeightt = 0;

                                                Int32.TryParse(innerHeight.ToString(), out innerHeightt);
                                                scroll -= innerHeightt;


                                                //i++;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            top = "0px";
                                        }
                                    }
                                    else
                                        if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);



                                    }
                                    else
                                        if (!isElementMessengerEndingPresent(driverMessenger))
                                        while (!isElementMessengerEndingPresent(driverMessenger))
                                        {
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) + ");", imageDown);
                                            Thread.Sleep(2000);



                                            hauteur += 600;
                                            hauteurtotale += resolution.Height + (resolution.Height / 2);
                                            //i++;
                                        }
                                    else
                                        if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);

                                    }



                                    hauteur = 0;


                                    top = "";
                                    try
                                    {
                                        foreach (string t in topScrollBar)
                                        {
                                            string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                            if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                continue;

                                            top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                            //top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        top = "";
                                    }

                                    //ON DESCEND--------------------------------------------------------------------------------------
                                    if (top != "")
                                    {
                                        string topPrec = "";
                                        textes = imageDown.Text;


                                        int j = 1;
                                        int hauteurfromdate = 0;
                                        string hauteurfromdateString = "";

                                        Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                        long innerHeightt = (long)innerHeight;
                                        long scrolll = (long)innerHeight;
                                        long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                        while (true)
                                        {

                                            if (fromdate != null)
                                            {
                                                //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                                hauteurfromdate = fromdate.Location.Y;
                                                hauteurfromdateString = hauteurfromdate.ToString();

                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                                Thread.Sleep(500);

                                                //hauteurfromdate = fromdate.Location.Y;
                                                fromdate = null;
                                                STOP = false;
                                                //hauteur += 600;

                                            }
                                            else
                                            {

                                                //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                                ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0," + hauteurr + ");", imageDown);

                                                Thread.Sleep(WAIT);
                                            }



                                            if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                            {

                                                try
                                                {
                                                    driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                }
                                                catch
                                                {

                                                }
                                            }
                                            //int hei = imageDown.Size.Height;
                                            messenger = driverMessenger.PageSource;

                                            //TEST 
                                            var messagesTesting = driverMessenger.FindElementsByTagName("div");
                                            object[] messagesToExtractTesting = messagesTesting.ToArray();

                                            //QUE LES TEXTES
                                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtractTesting)
                                            {

                                                string tentation = o.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if (o.GetAttribute("class").Contains("_1mj2 _2e-6") || o.GetAttribute("class").Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                    {


                                                        //pour récupérer les a contenant la longueur du message vocal
                                                        //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                        IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                        foreach (IWebElement ell in listeVocauxx)
                                                        {
                                                            //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                            //    break;
                                                            if (ell.Text == "")
                                                                continue;

                                                            tentation = ell.ToString();
                                                            idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                            //string tentation = ell.ToString();
                                                            //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            //if (audioVisibles.ContainsKey(idd))
                                                            //{
                                                            //    continue;
                                                            //}

                                                            if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                            {
                                                                //string tentation = ell.ToString();
                                                                //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                                IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                string dateAudio = "";
                                                                string nomFichierAudio = "";

                                                                foreach (IWebElement el in els)
                                                                {
                                                                    if (el.GetAttribute("data-tooltip-content") != null)
                                                                    {

                                                                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                        {
                                                                            var texte = el.GetAttribute("data-tooltip-content");
                                                                            var position = el.Location;
                                                                            dateAudio = texte.Replace(":", "-");
                                                                            nomFichierAudio = "From";
                                                                            break;
                                                                        }



                                                                    }
                                                                }
                                                                if (dateAudio == "")
                                                                {
                                                                    els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                    dateAudio = "";
                                                                    foreach (IWebElement el in els)
                                                                    {
                                                                        if (el.GetAttribute("data-tooltip-content") != null)
                                                                        {

                                                                            if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                            {
                                                                                var texte = el.GetAttribute("data-tooltip-content");
                                                                                var position = el.Location;
                                                                                dateAudio = texte.Replace(":", "-");
                                                                                nomFichierAudio = "To";
                                                                                break;
                                                                            }



                                                                        }
                                                                    }
                                                                }

                                                                string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                                //if (classe.Contains("_1mj2 _2e-6"))
                                                                //    nomFichierAudio = "From";
                                                                //else
                                                                //    nomFichierAudio = "To";

                                                                if (duree.Length == 4)
                                                                    duree = "00:0" + duree;

                                                                if (audioVisibles.ContainsKey(idd))
                                                                {
                                                                    audioVisibless.Add(audioVisibles[idd]);
                                                                    continue;
                                                                }

                                                                try
                                                                {
                                                                    TimeSpan tempss;
                                                                    TimeSpan.TryParse(duree, out tempss);
                                                                    //DateTime temps;
                                                                    //DateTime.TryParse(duree, out temps);
                                                                    ell.Click();
                                                                    StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                    Stopwatch sw = new Stopwatch();
                                                                    sw.Start();
                                                                    //Thread.Sleep(500);
                                                                    while (true)
                                                                    {

                                                                        if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                        {
                                                                            if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                            {

                                                                                try
                                                                                {
                                                                                    driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                                }
                                                                                catch
                                                                                {

                                                                                }
                                                                            }
                                                                            break;
                                                                        }

                                                                    }

                                                                    StopRecording();
                                                                    Thread.Sleep(1000);

                                                                    if (!audioVisibles.ContainsKey(idd))
                                                                    {
                                                                        audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                        audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                                        if (!classeTraitee.ContainsKey(idd))
                                                                        {
                                                                            classeTraitee.Add(idd, idd);
                                                                        }
                                                                    }



                                                                    j++;

                                                                }
                                                                catch (Exception exx)
                                                                {
                                                                    //StopRecording();
                                                                    //Thread.Sleep(5000);
                                                                }


                                                                //ell.Click();


                                                            }

                                                        }

                                                        //pour cliquer sur le message vocal et jouer le son
                                                        //driverMessenger.FindElementsByXPath(classe)[0].Click();

                                                    }


                                                    if (o.GetAttribute("class").Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                    {

                                                        IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath("//div[@class='_2poz _52mr _ui9 _2n8h _2n8i _5fk1']" + "//video");//_ox1 _21y0
                                                        for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                        {
                                                            tentation = allDivElements[ii].ToString();
                                                            idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                            if (classeTraitee.ContainsKey(idd))
                                                            {
                                                                continue;
                                                            }


                                                            if (allDivElements[ii].GetAttribute("class") != null)
                                                            {
                                                                //here the print statement will print the value of each div tag element
                                                                var tmp = allDivElements[ii].GetAttribute("class");

                                                                if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                                {
                                                                    //var video = driver.FindElementsByClassName(tmp);
                                                                    //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                    try
                                                                    {
                                                                        //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                        var tmpp = allDivElements[ii].GetAttribute("src");

                                                                        //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                        //    break;


                                                                        if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                        {


                                                                            if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                            {
                                                                                videosVisibles.Add(tmpp + ii, tmpp);

                                                                                if (!classeTraitee.ContainsKey(idd))
                                                                                {
                                                                                    classeTraitee.Add(idd, tmpp);
                                                                                }
                                                                            }
                                                                        }
                                                                        //break;



                                                                        //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                    }
                                                                    catch (Exception ex)
                                                                    {

                                                                    }

                                                                }
                                                            }

                                                        }




                                                    }
                                                                                              
                                                
                                                
                                                //if (!classesConnuess.Contains(o.GetAttribute("class")))
                                                //    continue;

                                                //if (o.GetAttribute("class").Contains("_1mj2 _2e-6") || o.GetAttribute("class").Contains("_1mj4 _2e-7"))
                                                //    continue;

                                                //if (o.GetAttribute("class").Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))
                                                //    continue;

                                                //string tentation = o.ToString();
                                                //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if(o.Text != "")
                                                {
                                                    int numeroScreenshot = o.LocationOnScreenOnceScrolledIntoView.Y / (resolutionEcran - 265);
                                                    messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", "") + "_Screenshot_" + numeroScreenshot);
                                                }

                                            }

                                            string texteTesting = "";
                                            foreach(string msg in messagesVisibles.Values)
                                            {
                                                texteTesting += msg + "\n";
                                            }
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Div_Screenshots.txt", false))
                                            {
                                               

                                                file.Write(texteTesting);
                                            }


                                            //FIN TEST----------------------------

                                            if (!FASTMESSENGER)
                                                foreach (string classe in classesConnues)
                                                {



                                                    //if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                    //{


                                                    //    //pour récupérer les a contenant la longueur du message vocal
                                                    //    //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    //    //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    //    IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("_1mj2 _2e-6']"));

                                                    //    foreach (IWebElement ell in listeVocauxx)
                                                    //    {
                                                    //        //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                    //        //    break;
                                                    //        if (ell.Text == "")
                                                    //            continue;

                                                    //        string tentation = ell.ToString();
                                                    //        string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //        //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                    //        //string tentation = ell.ToString();
                                                    //        //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //        //if (audioVisibles.ContainsKey(idd))
                                                    //        //{
                                                    //        //    continue;
                                                    //        //}

                                                    //        if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                    //        {
                                                    //            //string tentation = ell.ToString();
                                                    //            //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                    //            IList<IWebElement> els = driverMessenger.FindElements(By.XPath("_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                    //            string dateAudio = "";
                                                    //            string nomFichierAudio = "";

                                                    //            foreach (IWebElement el in els)
                                                    //            {
                                                    //                if (el.GetAttribute("data-tooltip-content") != null)
                                                    //                {

                                                    //                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                    //                    {
                                                    //                        var texte = el.GetAttribute("data-tooltip-content");
                                                    //                        var position = el.Location;
                                                    //                        dateAudio = texte.Replace(":", "-");
                                                    //                        nomFichierAudio = "From";
                                                    //                        break;
                                                    //                    }



                                                    //                }
                                                    //            }
                                                    //            if (dateAudio == "")
                                                    //            {
                                                    //                els = driverMessenger.FindElements(By.XPath("_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                    //                dateAudio = "";
                                                    //                foreach (IWebElement el in els)
                                                    //                {
                                                    //                    if (el.GetAttribute("data-tooltip-content") != null)
                                                    //                    {

                                                    //                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                    //                        {
                                                    //                            var texte = el.GetAttribute("data-tooltip-content");
                                                    //                            var position = el.Location;
                                                    //                            dateAudio = texte.Replace(":", "-");
                                                    //                            nomFichierAudio = "To";
                                                    //                            break;
                                                    //                        }



                                                    //                    }
                                                    //                }
                                                    //            }

                                                    //            string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                    //            //if (classe.Contains("_1mj2 _2e-6"))
                                                    //            //    nomFichierAudio = "From";
                                                    //            //else
                                                    //            //    nomFichierAudio = "To";

                                                    //            if (duree.Length == 4)
                                                    //                duree = "00:0" + duree;

                                                    //            if (audioVisibles.ContainsKey(idd))
                                                    //            {
                                                    //                audioVisibless.Add(audioVisibles[idd]);
                                                    //                continue;
                                                    //            }

                                                    //            try
                                                    //            {
                                                    //                TimeSpan tempss;
                                                    //                TimeSpan.TryParse(duree, out tempss);
                                                    //                //DateTime temps;
                                                    //                //DateTime.TryParse(duree, out temps);
                                                    //                ell.Click();
                                                    //                StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                    //                Stopwatch sw = new Stopwatch();
                                                    //                sw.Start();
                                                    //                //Thread.Sleep(500);
                                                    //                while (true)
                                                    //                {

                                                    //                    if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                    //                    {
                                                    //                        if (isElementPresent(driver, "_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                    //                        {

                                                    //                            try
                                                    //                            {
                                                    //                                driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                    //                            }
                                                    //                            catch
                                                    //                            {

                                                    //                            }
                                                    //                        }
                                                    //                        break;
                                                    //                    }

                                                    //                }

                                                    //                StopRecording();
                                                    //                Thread.Sleep(1000);

                                                    //                if (!audioVisibles.ContainsKey(idd))
                                                    //                {
                                                    //                    audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                    //                    audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                    //                    if (!classeTraitee.ContainsKey(idd))
                                                    //                    {
                                                    //                        classeTraitee.Add(idd, idd);
                                                    //                    }
                                                    //                }



                                                    //                j++;

                                                    //            }
                                                    //            catch (Exception exx)
                                                    //            {
                                                    //                //StopRecording();
                                                    //                //Thread.Sleep(5000);
                                                    //            }


                                                    //            //ell.Click();


                                                    //        }

                                                    //    }

                                                    //    //pour cliquer sur le message vocal et jouer le son
                                                    //    //driverMessenger.FindElementsByXPath(classe)[0].Click();

                                                    //}


                                                    //if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                    //{

                                                    //    IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    //    for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                    //    {
                                                    //        string tentation = allDivElements[ii].ToString();
                                                    //        string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //        if (classeTraitee.ContainsKey(idd))
                                                    //        {
                                                    //            continue;
                                                    //        }


                                                    //        if (allDivElements[ii].GetAttribute("class") != null)
                                                    //        {
                                                    //            //here the print statement will print the value of each div tag element
                                                    //            var tmp = allDivElements[ii].GetAttribute("class");

                                                    //            if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                    //            {
                                                    //                //var video = driver.FindElementsByClassName(tmp);
                                                    //                //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                    //                try
                                                    //                {
                                                    //                    //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                    //                    var tmpp = allDivElements[ii].GetAttribute("src");

                                                    //                    //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                    //                    //    break;


                                                    //                    if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                    //                    {


                                                    //                        if (!videosVisibles.ContainsKey(tmpp + ii))
                                                    //                        {
                                                    //                            videosVisibles.Add(tmpp + ii, tmpp);

                                                    //                            if (!classeTraitee.ContainsKey(idd))
                                                    //                            {
                                                    //                                classeTraitee.Add(idd, tmpp);
                                                    //                            }
                                                    //                        }
                                                    //                    }
                                                    //                    //break;



                                                    //                    //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                    //                }
                                                    //                catch (Exception ex)
                                                    //                {

                                                    //                }

                                                    //            }
                                                    //        }

                                                    //    }




                                                    //}


                                                    var messages = driverMessenger.FindElementsByXPath(classe);
                                                    object[] messagesToExtract = messages.ToArray();


                                                    //foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                                    //{

                                                    //    string tentation = o.ToString();
                                                    //    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //    if (classeTraitee.ContainsKey(idd))
                                                    //        continue;

                                                    //    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    //    {
                                                    //        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    //    }
                                                    //    //else
                                                    //    //    continue;


                                                    //    if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                    //    {
                                                    //        if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                    //        {
                                                    //            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                    //            if (!classeTraitee.ContainsKey(idd))
                                                    //            {
                                                    //                classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    //            }
                                                    //        }

                                                    //    }
                                                    //    //else
                                                    //    //    break;


                                                    //}


                                                }

                                            //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                            //imageDown = driver.FindElementById(idTmp);
                                            //Thread.Sleep(200);

                                            //if (textes != imageDown.Text)
                                            //    textes = imageDown.Text + "\n";

                                            messenger = driverMessenger.PageSource;
                                            //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            top = "";
                                            try
                                            {
                                                foreach (string t in topScrollBar)
                                                {
                                                    string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                    if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                        continue;

                                                    top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                                    //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                                    //    continue;

                                                    //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                                }

                                                if (top == topPrec)
                                                    break;

                                                topPrec = top;
                                            }
                                            catch
                                            {

                                            }

                                            Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                            imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();


                                            //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                            imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                            Thread.Sleep(100);

                                            //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                            string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                            //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                            pathToFolder = pathToSaveScreenshot;

                                            if (!FASTMESSENGER)
                                                foreach (string cle in messagesVisibles.Keys)
                                                {

                                                    if (!messagesVisiblesForFile.ContainsKey(cle))
                                                    {

                                                        string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                        string tmp = "";
                                                        foreach (string li in lignes)
                                                        {
                                                            tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                        }



                                                        messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                    }

                                                    //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                                }

                                            if (!FASTMESSENGER)
                                                if (videosVisibles.Count > 0)
                                                    foreach (string cle in videosVisibles.Keys)
                                                    {
                                                        //if (!videosVisiblesForFile.ContainsKey(cle))
                                                        //{

                                                        videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                        //}
                                                    }

                                            if (!FASTMESSENGER)
                                                if (audioVisibless.Count > 0)
                                                    foreach (string cle in audioVisibless)
                                                    {
                                                        //if (!videosVisiblesForFile.ContainsKey(cle))
                                                        //{
                                                        if (!audioVisiblesForFile.ContainsKey(cle))
                                                            audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                        //}
                                                    }

                                            messagesVisibles = new Dictionary<string, string>();
                                            videosVisibles = new Dictionary<string, string>();
                                            //audioVisibles = new Dictionary<string, string>();
                                            hauteur += 600;


                                            i++;
                                        }
                                        ForGrid forGrid = new ForGrid();
                                        forGrid.PathToFolder = pathToFolder;
                                        forGrid.Url = link;

                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";

                                        backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                        //FillDataGridViewMessenger(pathToFolder, link);
                                    }
                                    else//Si pas de scrollbar alors seulement une page------------------------------------------------------------------------------------------
                                    {
                                        int j = 1;
                                        foreach (string classe in classesConnues)
                                        {


                                            if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                            {


                                                //pour récupérer les a contenant la longueur du message vocal
                                                //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                foreach (IWebElement ell in listeVocauxx)
                                                {
                                                    //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                    //    break;
                                                    if (ell.Text == "")
                                                        continue;

                                                    string tentation = ell.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                    //string tentation = ell.ToString();
                                                    //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //if (audioVisibles.ContainsKey(idd))
                                                    //{
                                                    //    continue;
                                                    //}

                                                    if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        //string tentation = ell.ToString();
                                                        //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                        IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                        string dateAudio = "";
                                                        string nomFichierAudio = "";

                                                        foreach (IWebElement el in els)
                                                        {
                                                            if (el.GetAttribute("data-tooltip-content") != null)
                                                            {

                                                                if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                {
                                                                    var texte = el.GetAttribute("data-tooltip-content");
                                                                    var position = el.Location;
                                                                    dateAudio = texte.Replace(":", "-");
                                                                    nomFichierAudio = "From";
                                                                    break;
                                                                }



                                                            }
                                                        }
                                                        if (dateAudio == "")
                                                        {
                                                            els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            dateAudio = "";
                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "To";
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                        }

                                                        string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                        //if (classe.Contains("_1mj2 _2e-6"))
                                                        //    nomFichierAudio = "From";
                                                        //else
                                                        //    nomFichierAudio = "To";

                                                        if (duree.Length == 4)
                                                            duree = "00:0" + duree;

                                                        if (audioVisibles.ContainsKey(idd))
                                                        {
                                                            audioVisibless.Add(audioVisibles[idd]);
                                                            continue;
                                                        }

                                                        try
                                                        {
                                                            TimeSpan tempss;
                                                            TimeSpan.TryParse(duree, out tempss);
                                                            //DateTime temps;
                                                            //DateTime.TryParse(duree, out temps);
                                                            ell.Click();
                                                            StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                            Stopwatch sw = new Stopwatch();
                                                            sw.Start();
                                                            //Thread.Sleep(500);
                                                            while (true)
                                                            {

                                                                if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                {
                                                                    if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                    {

                                                                        try
                                                                        {
                                                                            driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                        }
                                                                        catch
                                                                        {

                                                                        }
                                                                    }
                                                                    break;
                                                                }

                                                            }

                                                            StopRecording();
                                                            Thread.Sleep(1000);

                                                            if (!audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                            }



                                                            j++;

                                                        }
                                                        catch (Exception exx)
                                                        {
                                                            //StopRecording();
                                                            //Thread.Sleep(5000);
                                                        }


                                                        //ell.Click();


                                                    }

                                                }

                                                //pour cliquer sur le message vocal et jouer le son
                                                //driverMessenger.FindElementsByXPath(classe)[0].Click();
                                            }







                                            if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {


                                                IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                {

                                                    if (allDivElements[ii].GetAttribute("class") != null)
                                                    {
                                                        //here the print statement will print the value of each div tag element
                                                        var tmp = allDivElements[ii].GetAttribute("class");

                                                        if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                        {
                                                            //var video = driver.FindElementsByClassName(tmp);
                                                            //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                            try
                                                            {
                                                                //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                var tmpp = allDivElements[ii].GetAttribute("src");


                                                                if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                {
                                                                    if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                    {
                                                                        videosVisibles.Add(tmpp + ii, tmpp);
                                                                    }
                                                                }
                                                                //break;



                                                                //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }

                                                        }
                                                    }

                                                }




                                            }



                                            var messages = driverMessenger.FindElementsByXPath(classe);
                                            object[] messagesToExtract = messages.ToArray();



                                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                            {

                                                string tentation = o.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if (!dicoMessagesFrom.ContainsKey(idd))
                                                {
                                                    dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                }
                                                //else
                                                //    continue;

                                                if (o.Location.Y + o.Size.Height > 15 && o.Location.Y < (resolutionEcran - 300))
                                                {
                                                    if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                                    {
                                                        messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                }

                                            }
                                        }


                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";

                                        Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        //Save the screenshot
                                        //if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                                        //{
                                        //    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                        //}
                                        //else
                                        //if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                                        //{
                                        //    idFictif++;
                                        //    idFictifString = idFictif.ToString();
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + i.ToString());
                                        //}



                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                                        Thread.Sleep(100);

                                        string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                        pathToFolder = pathToSaveScreenshot;

                                        foreach (string cle in messagesVisibles.Keys)
                                        {

                                            if (!messagesVisiblesForFile.ContainsKey(cle))
                                            {

                                                string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                string tmp = "";
                                                foreach (string li in lignes)
                                                {
                                                    tmp += li.Trim().Replace("\r", "");
                                                }



                                                messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                            }

                                            //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                        }

                                        if (videosVisibles.Count > 0)
                                            foreach (string cle in videosVisibles.Keys)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{

                                                videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                        if (audioVisibles.Count > 0)
                                            foreach (string cle in audioVisibles.Values)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{
                                                if (!audioVisibles.ContainsKey(cle))
                                                    audioVisiblesForFile.Add(cle + ";" + pathToFile + "\n", cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                        messagesVisibles = new Dictionary<string, string>();
                                        videosVisibles = new Dictionary<string, string>();
                                        //audioVisibles = new Dictionary<string, string>();

                                        ForGrid forGrid = new ForGrid();
                                        forGrid.PathToFolder = pathToFolder;
                                        forGrid.Url = link;

                                        backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                        //FillDataGridViewMessenger(pathToFolder, link);
                                        //continue;
                                    }




                                }
                                catch (Exception ex)//SI TIMEOUT
                                {
                                    MessageBox.Show("error dans Messenger : " + ex.Message);
                                    ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return window.stop");
                                    Thread.Sleep(1000);

                                    string topPrec = "";
                                    textes = imageDown.Text;

                                    //Dictionary<string, string> classeTraitee = new Dictionary<string, string>();
                                    int j = 1;
                                    int hauteurfromdate = 0;
                                    string hauteurfromdateString = "";

                                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                    long innerHeightt = (long)innerHeight;
                                    long scrolll = (long)innerHeight;
                                    long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                    while (true)
                                    {
                                        if (fromdate != null)
                                        {
                                            //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                            hauteurfromdate = fromdate.Location.Y;
                                            hauteurfromdateString = hauteurfromdate.ToString();

                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                            Thread.Sleep(500);

                                            //hauteurfromdate = fromdate.Location.Y;
                                            fromdate = null;
                                            //hauteur += 600;

                                        }
                                        else
                                        {

                                            //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + hauteur + ");", imageDown);

                                            Thread.Sleep(2000);
                                        }



                                        if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                        {

                                            try
                                            {
                                                driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                            }
                                            catch
                                            {

                                            }
                                        }
                                        //int hei = imageDown.Size.Height;
                                        messenger = driverMessenger.PageSource;

                                        if (!FASTMESSENGER)
                                            foreach (string classe in classesConnues)
                                            {

                                                if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                                {


                                                    //pour récupérer les a contenant la longueur du message vocal
                                                    //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                    IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                    foreach (IWebElement ell in listeVocauxx)
                                                    {
                                                        //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                        //    break;
                                                        if (ell.Text == "")
                                                            continue;

                                                        string tentation = ell.ToString();
                                                        string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                        //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                        //string tentation = ell.ToString();
                                                        //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                        //if (audioVisibles.ContainsKey(idd))
                                                        //{
                                                        //    continue;
                                                        //}

                                                        if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                        {
                                                            //string tentation = ell.ToString();
                                                            //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                            IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            string dateAudio = "";
                                                            string nomFichierAudio = "";

                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "From";
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                            if (dateAudio == "")
                                                            {
                                                                els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                                dateAudio = "";
                                                                foreach (IWebElement el in els)
                                                                {
                                                                    if (el.GetAttribute("data-tooltip-content") != null)
                                                                    {

                                                                        if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                        {
                                                                            var texte = el.GetAttribute("data-tooltip-content");
                                                                            var position = el.Location;
                                                                            dateAudio = texte.Replace(":", "-");
                                                                            nomFichierAudio = "To";
                                                                            break;
                                                                        }



                                                                    }
                                                                }
                                                            }

                                                            string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                            //if (classe.Contains("_1mj2 _2e-6"))
                                                            //    nomFichierAudio = "From";
                                                            //else
                                                            //    nomFichierAudio = "To";

                                                            if (duree.Length == 4)
                                                                duree = "00:0" + duree;

                                                            if (audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibless.Add(audioVisibles[idd]);
                                                                continue;
                                                            }

                                                            try
                                                            {
                                                                TimeSpan tempss;
                                                                TimeSpan.TryParse(duree, out tempss);
                                                                //DateTime temps;
                                                                //DateTime.TryParse(duree, out temps);
                                                                ell.Click();
                                                                StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                Stopwatch sw = new Stopwatch();
                                                                sw.Start();
                                                                //Thread.Sleep(500);
                                                                while (true)
                                                                {

                                                                    if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                    {
                                                                        if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                        {

                                                                            try
                                                                            {
                                                                                driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                            }
                                                                            catch
                                                                            {

                                                                            }
                                                                        }
                                                                        break;
                                                                    }

                                                                }

                                                                StopRecording();
                                                                Thread.Sleep(1000);

                                                                if (!audioVisibles.ContainsKey(idd))
                                                                {
                                                                    audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                    audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                                }



                                                                j++;

                                                            }
                                                            catch (Exception exx)
                                                            {
                                                                //StopRecording();
                                                                //Thread.Sleep(5000);
                                                            }


                                                            //ell.Click();


                                                        }

                                                    }

                                                    //pour cliquer sur le message vocal et jouer le son
                                                    //driverMessenger.FindElementsByXPath(classe)[0].Click();
                                                }


                                                if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                {


                                                    IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                    {

                                                        if (allDivElements[ii].GetAttribute("class") != null)
                                                        {
                                                            //here the print statement will print the value of each div tag element
                                                            var tmp = allDivElements[ii].GetAttribute("class");

                                                            if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                            {
                                                                //var video = driver.FindElementsByClassName(tmp);
                                                                //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                try
                                                                {
                                                                    //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                    var tmpp = allDivElements[ii].GetAttribute("src");

                                                                    //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                    //    break;


                                                                    if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                    {
                                                                        if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                        {
                                                                            videosVisibles.Add(tmpp + ii, tmpp);
                                                                        }
                                                                    }
                                                                    //break;



                                                                    //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                }
                                                                catch
                                                                {

                                                                }

                                                            }
                                                        }

                                                    }




                                                }


                                                var messages = driverMessenger.FindElementsByXPath(classe);
                                                object[] messagesToExtract = messages.ToArray();


                                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                                {

                                                    string tentation = o.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //if (classeTraitee.ContainsKey(idd))
                                                    //    continue;

                                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    {
                                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                    //else
                                                    //    continue;


                                                    if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                        {
                                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                            //if (!classeTraitee.ContainsKey(idd))
                                                            //{
                                                            //    classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                            //}
                                                        }

                                                    }
                                                    //else
                                                    //    break;


                                                }


                                            }

                                        //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                        //imageDown = driver.FindElementById(idTmp);
                                        //Thread.Sleep(200);

                                        //if (textes != imageDown.Text)
                                        //    textes = imageDown.Text + "\n";

                                        messenger = driverMessenger.PageSource;
                                        //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                        //on récupère la valeur Top de la scrollBar

                                        topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                        //on récupère la valeur Top de la scrollBar

                                        top = "";
                                        try
                                        {
                                            foreach (string t in topScrollBar)
                                            {
                                                string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                    continue;

                                                top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                                //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                                //    continue;

                                                //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                            }

                                            if (top == topPrec)
                                                break;

                                            topPrec = top;
                                        }
                                        catch
                                        {

                                        }

                                        Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();


                                        //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                        Thread.Sleep(100);

                                        //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                        string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                        //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                        pathToFolder = pathToSaveScreenshot;

                                        if (!FASTMESSENGER)
                                            foreach (string cle in messagesVisibles.Keys)
                                            {

                                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                                {

                                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                    string tmp = "";
                                                    foreach (string li in lignes)
                                                    {
                                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                    }



                                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                }

                                                //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                            }

                                        if (!FASTMESSENGER)
                                            if (videosVisibles.Count > 0)
                                                foreach (string cle in videosVisibles.Keys)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{

                                                    videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                        if (!FASTMESSENGER)
                                            if (audioVisibless.Count > 0)
                                                foreach (string cle in audioVisibless)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{
                                                    if (!audioVisiblesForFile.ContainsKey(cle))
                                                        audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                        messagesVisibles = new Dictionary<string, string>();
                                        videosVisibles = new Dictionary<string, string>();
                                        //audioVisibles = new Dictionary<string, string>();
                                        hauteur += 600;


                                        i++;
                                    }
                                    ForGrid forGrid = new ForGrid();
                                    forGrid.PathToFolder = pathToFolder;
                                    forGrid.Url = link;

                                    imageDown = driverMessenger.FindElementById(idTmp);
                                    textes = imageDown.Text + "\n";

                                    backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                    //FillDataGridViewMessenger(pathToFolder, link);



                                }


                            }
                            catch (Exception ex)//FIN TRY TIMEOUT-----------------------------------------------------------------------
                            {
                                //MessageBox.Show("error dans Messenger " + ex.Message);


                                string topPrec = "";
                                textes = imageDown.Text;

                                //Dictionary<string, string> classeTraitee = new Dictionary<string, string>();
                                int j = 1;
                                int hauteurfromdate = 0;
                                string hauteurfromdateString = "";

                                Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                                long innerHeightt = (long)innerHeight;
                                long scrolll = (long)innerHeight;
                                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                                while (true)
                                {
                                    if (fromdate != null)
                                    {
                                        //hauteurfromdate = fromdate.Location.Y + (Int32)scrolll;
                                        hauteurfromdate = fromdate.Location.Y;
                                        hauteurfromdateString = hauteurfromdate.ToString();

                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollIntoView(true);", fromdate);
                                        Thread.Sleep(500);

                                        //hauteurfromdate = fromdate.Location.Y;
                                        fromdate = null;
                                        //hauteur += 600;

                                    }
                                    else
                                    {

                                        //((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollBy(0,450);", imageDown);
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + hauteur + ");", imageDown);

                                        Thread.Sleep(2000);
                                    }



                                    if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                    {

                                        try
                                        {
                                            driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                        }
                                        catch
                                        {

                                        }
                                    }
                                    //int hei = imageDown.Size.Height;
                                    messenger = driverMessenger.PageSource;

                                    if (!FASTMESSENGER)
                                        foreach (string classe in classesConnues)
                                        {

                                            if (classe.Contains("_1mj2 _2e-6") || classe.Contains("_1mj4 _2e-7"))//|| classe.Contains("_3058 _15gf")
                                            {


                                                //pour récupérer les a contenant la longueur du message vocal
                                                //IList<IWebElement> newlisteVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                //IList<IWebElement> listeVocaux = driverMessenger.FindElementsByXPath(classe + "//a");
                                                IList<IWebElement> listeVocauxx = driverMessenger.FindElements(By.XPath("//div[@class='_1mj2 _2e-6']"));

                                                foreach (IWebElement ell in listeVocauxx)
                                                {
                                                    //if (listeVocaux[0].Location.Y > 15 && listeVocaux[0].Location.Y < (resolutionEcran - 250))
                                                    //    break;
                                                    if (ell.Text == "")
                                                        continue;

                                                    string tentation = ell.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //IList<IWebElement> testt = ell.FindElements(By.TagName("a"));

                                                    //string tentation = ell.ToString();
                                                    //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //if (audioVisibles.ContainsKey(idd))
                                                    //{
                                                    //    continue;
                                                    //}

                                                    if (ell.Location.Y > 15 && ell.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        //string tentation = ell.ToString();
                                                        //string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];


                                                        IList<IWebElement> els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs _5z-5']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                        string dateAudio = "";
                                                        string nomFichierAudio = "";

                                                        foreach (IWebElement el in els)
                                                        {
                                                            if (el.GetAttribute("data-tooltip-content") != null)
                                                            {

                                                                if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                {
                                                                    var texte = el.GetAttribute("data-tooltip-content");
                                                                    var position = el.Location;
                                                                    dateAudio = texte.Replace(":", "-");
                                                                    nomFichierAudio = "From";
                                                                    break;
                                                                }



                                                            }
                                                        }
                                                        if (dateAudio == "")
                                                        {
                                                            els = driverMessenger.FindElements(By.XPath("//div[@class='_3zvs']"));//driverMessenger.FindElements(By.XPath("_3zvs']"))[2].GetAttribute("data-tooltip-content")

                                                            dateAudio = "";
                                                            foreach (IWebElement el in els)
                                                            {
                                                                if (el.GetAttribute("data-tooltip-content") != null)
                                                                {

                                                                    if ((ell.Location.Y - el.Location.Y) > 0 && (ell.Location.Y - el.Location.Y) < 5)
                                                                    {
                                                                        var texte = el.GetAttribute("data-tooltip-content");
                                                                        var position = el.Location;
                                                                        dateAudio = texte.Replace(":", "-");
                                                                        nomFichierAudio = "To";
                                                                        break;
                                                                    }



                                                                }
                                                            }
                                                        }

                                                        string duree = ell.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                        //if (classe.Contains("_1mj2 _2e-6"))
                                                        //    nomFichierAudio = "From";
                                                        //else
                                                        //    nomFichierAudio = "To";

                                                        if (duree.Length == 4)
                                                            duree = "00:0" + duree;

                                                        if (audioVisibles.ContainsKey(idd))
                                                        {
                                                            audioVisibless.Add(audioVisibles[idd]);
                                                            continue;
                                                        }

                                                        try
                                                        {
                                                            TimeSpan tempss;
                                                            TimeSpan.TryParse(duree, out tempss);
                                                            //DateTime temps;
                                                            //DateTime.TryParse(duree, out temps);
                                                            ell.Click();
                                                            StartRecord(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                            Stopwatch sw = new Stopwatch();
                                                            sw.Start();
                                                            //Thread.Sleep(500);
                                                            while (true)
                                                            {

                                                                if (sw.ElapsedMilliseconds > tempss.TotalMilliseconds + (2000))
                                                                {
                                                                    if (isElementPresent(driver, "//div[@class='_10 uiLayer _4-hy _3qw']"))//driver.FindElement(By.XPath("_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click()
                                                                    {

                                                                        try
                                                                        {
                                                                            driver.FindElement(By.XPath("//div[@class='_10 uiLayer _4-hy _3qw']")).FindElement(By.TagName("a")).Click();


                                                                        }
                                                                        catch
                                                                        {

                                                                        }
                                                                    }
                                                                    break;
                                                                }

                                                            }

                                                            StopRecording();
                                                            Thread.Sleep(1000);

                                                            if (!audioVisibles.ContainsKey(idd))
                                                            {
                                                                audioVisibles.Add(idd, pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");
                                                                audioVisibless.Add(pathToSaveScreenshot + "\\Audio\\" + nomFichierAudio + "_" + dateAudio + "_duree_" + duree.Replace(":", "-") + "_" + j + ".wav");

                                                            }



                                                            j++;

                                                        }
                                                        catch (Exception exx)
                                                        {
                                                            //StopRecording();
                                                            //Thread.Sleep(5000);
                                                        }


                                                        //ell.Click();


                                                    }

                                                }

                                                //pour cliquer sur le message vocal et jouer le son
                                                //driverMessenger.FindElementsByXPath(classe)[0].Click();
                                            }


                                            if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {


                                                IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                {

                                                    if (allDivElements[ii].GetAttribute("class") != null)
                                                    {
                                                        //here the print statement will print the value of each div tag element
                                                        var tmp = allDivElements[ii].GetAttribute("class");

                                                        if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                        {
                                                            //var video = driver.FindElementsByClassName(tmp);
                                                            //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                            try
                                                            {
                                                                //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                var tmpp = allDivElements[ii].GetAttribute("src");

                                                                //if (allDivElements[0].Location.Y > 15 && allDivElements[0].Location.Y < (resolutionEcran - 250))
                                                                //    break;


                                                                if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                {
                                                                    if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                    {
                                                                        videosVisibles.Add(tmpp + ii, tmpp);
                                                                    }
                                                                }
                                                                //break;



                                                                //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                            }
                                                            catch
                                                            {

                                                            }

                                                        }
                                                    }

                                                }




                                            }


                                            var messages = driverMessenger.FindElementsByXPath(classe);
                                            object[] messagesToExtract = messages.ToArray();


                                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                            {

                                                string tentation = o.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                //if (classeTraitee.ContainsKey(idd))
                                                //    continue;

                                                if (!dicoMessagesFrom.ContainsKey(idd))
                                                {
                                                    dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                }
                                                //else
                                                //    continue;


                                                if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                {
                                                    if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                    {
                                                        messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                        //if (!classeTraitee.ContainsKey(idd))
                                                        //{
                                                        //    classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                        //}
                                                    }

                                                }
                                                //else
                                                //    break;


                                            }


                                        }

                                    //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                    //imageDown = driver.FindElementById(idTmp);
                                    //Thread.Sleep(200);

                                    //if (textes != imageDown.Text)
                                    //    textes = imageDown.Text + "\n";

                                    messenger = driverMessenger.PageSource;
                                    //topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                    //on récupère la valeur Top de la scrollBar

                                    topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                    //on récupère la valeur Top de la scrollBar

                                    top = "";
                                    try
                                    {
                                        foreach (string t in topScrollBar)
                                        {
                                            string tmp = t.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                            if (!tmp.Contains("top:") || tmp.Contains("hidden"))
                                                continue;

                                            top = tmp.Substring(tmp.IndexOf("top: ") + 4).Split(';')[0];

                                            //if ((!tmp.Contains("top:") && ! tmp.Contains("uiScrollableAreaWrap")) || tmp.StartsWith("<html"))
                                            //    continue;

                                            //top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                        }

                                        if (top == topPrec)
                                            break;

                                        topPrec = top;
                                    }
                                    catch
                                    {

                                    }

                                    Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                    imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();


                                    //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                    imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                                    Thread.Sleep(100);

                                    //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                    string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".png";
                                    //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                    pathToFolder = pathToSaveScreenshot;

                                    if (!FASTMESSENGER)
                                        foreach (string cle in messagesVisibles.Keys)
                                        {

                                            if (!messagesVisiblesForFile.ContainsKey(cle))
                                            {

                                                string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                string tmp = "";
                                                foreach (string li in lignes)
                                                {
                                                    tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                }



                                                messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                            }

                                            //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                        }
                                    if (!FASTMESSENGER)
                                        if (videosVisibles.Count > 0)
                                            foreach (string cle in videosVisibles.Keys)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{

                                                videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                //}
                                            }
                                    if (!FASTMESSENGER)
                                        if (audioVisibless.Count > 0)
                                            foreach (string cle in audioVisibless)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{
                                                if (!audioVisiblesForFile.ContainsKey(cle))
                                                    audioVisiblesForFile.Add(cle, cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                    messagesVisibles = new Dictionary<string, string>();
                                    videosVisibles = new Dictionary<string, string>();
                                    //audioVisibles = new Dictionary<string, string>();
                                    hauteur += 600;


                                    i++;
                                }
                                ForGrid forGrid = new ForGrid();
                                forGrid.PathToFolder = pathToFolder;
                                forGrid.Url = link;

                                imageDown = driverMessenger.FindElementById(idTmp);
                                textes = imageDown.Text + "\n";

                                backgroundWorkerGetMessenger.ReportProgress(-7, forGrid);
                                //FillDataGridViewMessenger(pathToFolder, link);


                            }

                            string codePagee = driverMessenger.PageSource;

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages.txt", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                file.Write(textes);
                            }

                            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages_Bis.txt", false))
                            //{
                            //    //if (File.Exists(saveFileDialog1.FileName))
                            //    //    File.Delete(saveFileDialog1.FileName);

                            //    textes = "";
                            //    foreach(string t in messagesVisibles.Values)
                            //    {
                            //        textes += t + "\n";
                            //    }


                            //    file.Write(textes);
                            //}

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages_With_Screenshots.txt", false))
                            {
                                textes = "";
                                foreach (string t in messagesVisiblesForFile.Values)
                                {
                                    textes += t;
                                }


                                file.Write(textes);
                                messagesVisiblesForFile = new Dictionary<string, string>();
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Videos_With_Screenshots.txt", false))
                            {
                                if (videosVisiblesForFile.Count > 0)
                                {
                                    textes = "";
                                    foreach (string t in videosVisiblesForFile)
                                    {
                                        textes += t;
                                    }


                                    file.Write(textes);

                                }



                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Audio_With_Screenshots.txt", false))
                            {
                                if (audioVisiblesForFile.Count > 0)
                                {
                                    textes = "";
                                    foreach (string t in audioVisiblesForFile.Values)
                                    {
                                        textes += t;
                                    }


                                    file.Write(textes);

                                }



                            }

                            if (!Directory.Exists(pathToSaveScreenshot + "\\Videos\\"))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSaveScreenshot + "\\Videos\\");
                            }

                            using (var client = new WebClient())
                            {

                                try

                                {
                                    Dictionary<string, string> dico = new Dictionary<string, string>();
                                    foreach (string t in videosVisiblesForFile)
                                    {
                                        //FileInfo fileinfo = new FileInfo(t);
                                        string keyy = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];

                                        if (!dico.ContainsKey(keyy))
                                        {
                                            dico.Add(keyy, t);
                                            string nomFichier = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];
                                            client.DownloadFile(t, pathToSaveScreenshot + "\\Videos\\" + nomFichier);
                                            Thread.Sleep(1000);
                                        }


                                    }

                                    videosVisiblesForFile = new List<string>();
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DES VIDEOS");
                                    //return;
                                }
                            }


                            //on essaie de récupérer les documents partagés
                            if (!FASTMESSENGER)
                                try
                                {
                                    IList<IWebElement> tmpp = driverMessenger.FindElements(By.ClassName("uiScrollableAreaContent"));
                                    foreach (IWebElement div in tmpp)
                                    {
                                        if (!div.Text.StartsWith("Information"))
                                            continue;

                                        IList<IWebElement> docPartagess = div.FindElements(By.TagName("a"));

                                        foreach (IWebElement elpart in docPartagess)
                                        {
                                            if (elpart.GetAttribute("href") != "")
                                            {
                                                docPartages.Add(elpart.GetAttribute("href"));
                                            }
                                        }
                                    }

                                }
                                catch
                                {

                                }

                            if (!FASTMESSENGER)
                                if (docPartages.Count > 0)
                                {
                                    if (!Directory.Exists(pathToSaveScreenshot + "\\Documents_Partages\\"))
                                    {
                                        //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                        Directory.CreateDirectory(pathToSaveScreenshot + "\\Documents_Partages\\");
                                    }

                                    using (var client = new WebClient())
                                    {

                                        try

                                        {

                                            foreach (string t in docPartages)
                                            {
                                                try
                                                {
                                                    //FileInfo fichier = new FileInfo(t);
                                                    string urll = t;
                                                    string nomFichier = t.Split(new string[] { "%3F" }, StringSplitOptions.RemoveEmptyEntries)[0];

                                                    urll = urll.Replace("%3A", ":").Replace("%3D", "=").Replace("%2F", "/").Replace("%3F", "?").Replace("%26", "&");

                                                    if (urll.Contains("https://l.facebook.com/l.php?u="))
                                                    {
                                                        nomFichier = nomFichier.Substring(nomFichier.LastIndexOf("%2F") + 3).Trim();
                                                        urll = urll.Substring(urll.IndexOf("https://l.facebook.com/l.php?u=") + 31);
                                                    }
                                                    //else
                                                    //    nomFichier = nomFichier.Split('?')[0].Substring(nomFichier.LastIndexOf("/") + 1).Trim();



                                                    client.DownloadFile(urll, pathToSaveScreenshot + "\\Documents_Partages\\" + nomFichier);
                                                    Thread.Sleep(1000);
                                                }

                                                catch
                                                {

                                                }

                                            }

                                            docPartages = new List<string>();
                                        }
                                        catch (Exception ex)
                                        {
                                            //MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DES VIDEOS");
                                            //return;
                                        }
                                    }
                                }




                            Thread.Sleep(2500);

                            textes = "";
                            messagesVisiblesForFile = new Dictionary<string, string>();
                            messagesVisibles = new Dictionary<string, string>();
                            videosVisibles = new Dictionary<string, string>();
                            videosVisiblesForFile = new List<string>();
                            audioVisiblesForFile = new Dictionary<string, string>();
                            messagesVisibles = new Dictionary<string, string>();
                            videosVisibles = new Dictionary<string, string>();
                            audioVisibles = new Dictionary<string, string>();
                            audioVisibless = new List<string>();
                            //idFictif = 1;
                        }

                        textes = "";
                        messagesVisiblesForFile = new Dictionary<string, string>();
                        audioVisiblesForFile = new Dictionary<string, string>();
                        videosVisiblesForFile = new List<string>();
                        messagesVisibles = new Dictionary<string, string>();
                        videosVisibles = new Dictionary<string, string>();
                        audioVisibles = new Dictionary<string, string>();
                        audioVisibless = new List<string>();
                        classeTraitee = new Dictionary<string, string>();

                        idFictif = 1;
                    }


                }
                catch
                {

                }


                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");

                    while (true)
                    {
                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        Thread.Sleep(2000);

                        Object newHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");
                        if (newHeight.Equals(lastHeight))
                        {
                            break;
                        }
                        lastHeight = newHeight;
                    }
                }
                catch
                {
                    //e.printStackTrace();
                }



            }
            catch
            {

                //driverMessenger.Quit();
            }




            //}
            //Thread.Sleep(2000);
            //backgroundWorker1.ReportProgress(-3);
            Thread.Sleep(2000);
            //backgroundWorker1.CancelAsync();

            if (backgroundWorkerGetMessenger != null && backgroundWorkerGetMessenger.IsBusy)
                backgroundWorkerGetMessenger.CancelAsync();

            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;
        }


        private void SetDefaultLanguage()
        {
            if (driver == null)
            {
                InitializeDriver();
                driverMessenger = driver;

                // 2. Go to the "Google" homepage
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            try
            {
                //on essai de remettre langue par defaut
                driverMessenger.Navigate().GoToUrl("https://www.facebook.com/settings?tab=language");
                Thread.Sleep(5000);

                IWebElement combo = driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemContent fcg']"));
                driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemEditText']")).Click();
                Thread.Sleep(500);

                SelectElement comboo = new SelectElement(driver.FindElement(By.TagName("select")));
                IWebElement item = comboo.SelectedOption;

                if (item.GetAttribute("value") == "fr_FR")
                {
                    comboo.SelectByValue(LANGUAGESELECTED);
                    Thread.Sleep(1500);

                    //submit uiButton uiButtonConfirm
                    driverMessenger.FindElement(By.XPath("//label[@class='submit uiButton uiButtonConfirm']")).Click();
                    Thread.Sleep(2500);
                }


            }
            catch (NoSuchElementException ex)
            {

            }
        }
        private void SetFrenchLanguage()
        {
            if (driver == null)
            {
                InitializeDriver();
                driverMessenger = driver;

                // 2. Go to the "Google" homepage
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            try
            {
                //on essai de remettre langue par defaut
                driverMessenger.Navigate().GoToUrl("https://www.facebook.com/settings?tab=language");
                Thread.Sleep(5000);

                IWebElement combo = driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemContent fcg']"));
                driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemEditText']")).Click();
                Thread.Sleep(1500);

                SelectElement comboo = new SelectElement(driver.FindElement(By.TagName("select")));
                IWebElement item = comboo.SelectedOption;
                LANGUAGESELECTED = item.GetAttribute("value");

                try
                {
                    Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                    imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();

                    Thread.Sleep(1000);


                    //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                    imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\DefaultLanguage.png", OpenQA.Selenium.ScreenshotImageFormat.Png);


                    Thread.Sleep(1000);
                }
                catch
                {

                }
               

                if (item.GetAttribute("value") != "fr_FR")
                {
                    comboo.SelectByValue("fr_FR");
                    Thread.Sleep(1500);

                    //submit uiButton uiButtonConfirm
                    driverMessenger.FindElement(By.XPath("//label[@class='submit uiButton uiButtonConfirm']")).Click();
                    Thread.Sleep(2500);
                }


            }
            catch (NoSuchElementException ex)
            {

            }
        }
        private void MessengerFromDate(Dictionary<string, string> dicoMessenger, string datum)
        {
            //getMessenger = true;
            
            pictureBoxlogofacebook.Visible = true;
            pictureBoxwaiting.Visible = true;
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Visible = true;
            pictureBoxlogofacebook.BringToFront();
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Refresh();


            //string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            //pour cacher fenetre DOS
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();
            int nbreAnnee = 1;

            try
            {
                if (driverMessenger.WindowHandles.Count == 0)
                {


                    // 1. Maximize the browser
                    //driver.Manage().Window.Maximize();
                    driverMessenger.Close();
                    driverMessenger = new ChromeDriver(driverService, chromeOptions);

                    // 2. Go to the "Google" homepage
                    driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


                    //while (!isElementPresentByID(driver, "email"))
                    //{
                    // 3. Find the username textbox (by ID) on the homepage
                    var userNameBox = driver.FindElementById("email");
                    // 4. Enter the text (to search for) in the textbox
                    userNameBox.SendKeys(textBoxUSERNAME.Text);
                    //}


                    // 3. Find the username textbox (by ID) on the homepage
                    var userpasswordBox = driver.FindElementById("pass");

                    // 4. Enter the text (to search for) in the textbox
                    userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                    Thread.Sleep(5000);

                    // 5. Find the search button (by Name) on the homepage
                    driver.FindElementById("loginbutton").Click();
                    //searchButton.Click();
                    Thread.Sleep(2500);
                }

            }

            catch
            {
                // 1. Maximize the browser
                //driver.Manage().Window.Maximize();
                //driverMessenger.Close();
                driverMessenger = new ChromeDriver(driverService, chromeOptions);

                // 2. Go to the "Google" homepage
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


                //while (!isElementPresentByID(driver, "email"))
                //{
                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");
                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);
                //}


                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);

            }


            //using (var driver = new ChromeDriver(driverService, chromeOptions))
            //{

            //// 1. Maximize the browser
            ////driver.Manage().Window.Maximize();



            //// 2. Go to the "Google" homepage
            //driverMessenger.Navigate().GoToUrl("https://facebook.com/login");


            ////while (!isElementPresentByID(driver, "email"))
            ////{
            //    // 3. Find the username textbox (by ID) on the homepage
            //    var userNameBox = driver.FindElementById("email");
            //    // 4. Enter the text (to search for) in the textbox
            //    userNameBox.SendKeys(textBoxUSERNAME.Text);
            ////}


            //// 3. Find the username textbox (by ID) on the homepage
            //var userpasswordBox = driver.FindElementById("pass");

            //// 4. Enter the text (to search for) in the textbox
            //userpasswordBox.SendKeys(textBoxPASSWORD.Text);
            //Thread.Sleep(5000);

            //// 5. Find the search button (by Name) on the homepage
            //driver.FindElementById("loginbutton").Click();
            ////searchButton.Click();
            //Thread.Sleep(2500);


            try
            {


                string targetName = textBoxops.Text;
                string textes = "";
                string messagesFromInString = "";
                string messagesVisiblesWithScreenshots = "";
                string pathToFolder = "";
                //var imageDown = "";
                Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                Dictionary<string, string> dicoMessagesTo = new Dictionary<string, string>();
                Dictionary<string, string> dicoPictures = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                Dictionary<string, string> videosVisibles = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();
                List<string> videosVisiblesForFile = new List<string>();

                List<string> classesConnues = new System.Collections.Generic.List<string>();
                classesConnues.Add("clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr']");
                classesConnues.Add("clearfix _o46 _3erg _29_7 direction_ltr text_align_ltr']");
                classesConnues.Add("clearfix _o46 _3erg _29_7 _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr']");
                classesConnues.Add("clearfix _o46 _3erg _3i_m _nd_ _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("_52mr _2poz _ui9 _4skb']");
                classesConnues.Add("clearfix _o46 _3erg _3i_m _nd_ direction_ltr text_align_ltr']");
                classesConnues.Add("clearfix _o46 _3erg _3i_m _nd_ _q4a _8lma direction_ltr text_align_ltr _ylc']");
                classesConnues.Add("_2poz _52mr _ui9 _2n8h _2n8i _5fk1']");//_2poz _52mr _ui9 _2n8h _2n8i _5fk1 Vidéo ?_ccq _4tsk _3o67 _52mr _1byr _4-od
                                                                                         //classesConnues.Add("_2n8h _2n8i _5fk1']");




                //récupération des années 
                //var years = driver.FindElementByXPath("rightColWrap']").Text;
                //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //if (!Directory.Exists(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper()))
                //    Directory.CreateDirectory(pathToSave + @"\Facebook_Friends\" + targetName.ToUpper());

                //driver.Navigate().GoToUrl("https://www.facebook.com/messages/t");//https://www.facebook.com/messages/t/MOD.orga
                //Thread.Sleep(5000);



                try
                {


                    string messenger = "";
                    int idFictif = 1;
                    string idFictifString = "";
                    Dictionary<string, string> dicoDATTUM = new Dictionary<string, string>();
                    if (dicoMessenger.Count > 0)
                    {
                        foreach (string link in dicoMessenger.Keys)
                        {

                            string pathToSaveScreenshot = "";

                            if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link];
                            }
                            else
                            if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                            {
                                idFictif++;
                                idFictifString = idFictif.ToString();
                                Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString);
                                pathToSaveScreenshot = pathToSave + "\\Messenger\\" + dicoMessenger[link] + idFictifString;
                            }






                            try
                            {
                                driverMessenger.Navigate().GoToUrl(link);
                                Thread.Sleep(5000);

                                messenger = driverMessenger.PageSource;



                                string[] id = messenger.Split(new String[] { "class=\"uiScrollableAreaWrap scrollable\"" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                //on récupère la valeur Top de la scrollBar

                                string top = "";
                                try
                                {
                                    foreach (string t in topScrollBar)
                                    {
                                        if (!t.Contains("top:"))
                                            continue;

                                        top = t.Substring(t.IndexOf("top: ") + 4).Split(';')[0];
                                    }
                                }
                                catch
                                {

                                }

                                //on récupère l'id dynamique du composant
                                string idTmp = "";
                                foreach (string idd in id)
                                {
                                    if (!idd.StartsWith("id") && !idd.Contains("Messages"))
                                        continue;


                                    idTmp = idd.Substring(idd.IndexOf("id=\"") + 4).Split('"')[0];
                                }

                                try
                                {
                                    var imageDown = driverMessenger.FindElementById(idTmp);
                                    int lastHeight = imageDown.Size.Height;


                                    int hauteur = 100;
                                    int i = 1;

                                    int width = driverMessenger.Manage().Window.Size.Width;
                                    int height = driverMessenger.Manage().Window.Size.Height;



                                    Rectangle resolution = Screen.FromControl(this).WorkingArea;
                                    int hauteurtotale = resolution.Height;
                                    int resolutionEcran = resolution.Height;

                                    if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);

                                        

                                    }
                                    else
                                        while (!isElementMessengerEndingPresent(driverMessenger))
                                        {
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + (lastHeight - hauteur) + ");", imageDown);
                                            Thread.Sleep(2000);

                                            IList<IWebElement> datums = driverMessenger.FindElementsById(idTmp);

                                            foreach (IWebElement el in datums)
                                            {
                                                el.FindElements(By.TagName("time"));
                                                string dattum = el.Text;
                                                string tentation = el.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if (dattum.Contains("/"))
                                                {
                                                                                                  
                                                    
                                                    if (!dicoDATTUM.ContainsKey(idd))
                                                    {
                                                        dicoDATTUM.Add(idd, dattum.Substring(0, 9));
                                                    }

                                                }
                                                else
                                                {

                                                }

                                            }


                                            hauteur += 600;
                                            hauteurtotale += resolution.Height + (resolution.Height / 2);
                                            //i++;
                                        }

                                    if (isElementMessengerEndingPresent(driverMessenger))
                                    {
                                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0,0);", imageDown);
                                        Thread.Sleep(2000);

                                    }



                                    hauteur = 0;

                                    


                                    try
                                    {
                                        messenger = driverMessenger.PageSource;
                                        topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);
                                        top = topScrollBar[1].Substring(topScrollBar[1].IndexOf("top: ") + 4).Split(';')[0];
                                    }
                                    catch
                                    {
                                        top = "";
                                    }


                                    if (top != "")
                                    {
                                        string topPrec = "";
                                        textes = imageDown.Text;

                                        Dictionary<string, string> classeTraitee = new Dictionary<string, string>();
                                        while (true)
                                        {
                                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("arguments[0].scrollTo(0," + hauteur + ");", imageDown);
                                            Thread.Sleep(2000);


                                            //int hei = imageDown.Size.Height;
                                            messenger = driverMessenger.PageSource;


                                            foreach (string classe in classesConnues)
                                            {
                                                if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                                {


                                                    IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                    for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                    {

                                                        if (allDivElements[ii].GetAttribute("class") != null)
                                                        {
                                                            //here the print statement will print the value of each div tag element
                                                            var tmp = allDivElements[ii].GetAttribute("class");

                                                            if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                            {
                                                                //var video = driver.FindElementsByClassName(tmp);
                                                                //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                                try
                                                                {
                                                                    //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                    var tmpp = allDivElements[ii].GetAttribute("src");


                                                                    if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                    {
                                                                        if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                        {
                                                                            videosVisibles.Add(tmpp + ii, tmpp);
                                                                        }
                                                                    }
                                                                    //break;



                                                                    //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                                }
                                                                catch (Exception ex)
                                                                {

                                                                }

                                                            }
                                                        }

                                                    }




                                                }


                                                var messages = driverMessenger.FindElementsByXPath(classe);
                                                object[] messagesToExtract = messages.ToArray();


                                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                                {

                                                    string tentation = o.ToString();
                                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                    //if (classeTraitee.ContainsKey(idd))
                                                    //    continue;

                                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                                    {
                                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                    //else
                                                    //    continue;


                                                    if (o.Location.Y > 15 && o.Location.Y < (resolutionEcran - 250))
                                                    {
                                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "" && !o.Text.Contains("Lire-4:01Paramètres"))
                                                        {
                                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));

                                                            //if (!classeTraitee.ContainsKey(idd))
                                                            //{
                                                            //    classeTraitee.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                            //}
                                                        }

                                                    }
                                                    //else
                                                    //    break;


                                                }


                                            }

                                            //on récupère tous les messages ppur ensuite comparer avec messageFrom et messageTo
                                            //imageDown = driver.FindElementById(idTmp);
                                            //Thread.Sleep(200);

                                            //if (textes != imageDown.Text)
                                            //    textes = imageDown.Text + "\n";

                                            messenger = driverMessenger.PageSource;
                                            topScrollBar = messenger.Split(new String[] { "class=\"uiScrollableAreaGripper\"" }, StringSplitOptions.RemoveEmptyEntries);

                                            //on récupère la valeur Top de la scrollBar

                                            top = "";
                                            try
                                            {


                                                top = topScrollBar[1].Substring(topScrollBar[1].IndexOf("top: ") + 4).Split(';')[0];

                                                if (top == topPrec)
                                                    break;

                                                topPrec = top;
                                            }
                                            catch
                                            {
                                                MessageBox.Show("erreur avec la scrollbar");
                                            }



                                            Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                            imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();


                                            //if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                                            //{
                                            //    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                            //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                            //}
                                            //else
                                            //{
                                            //    idFictif++;
                                            //    idFictifString = idFictif.ToString();
                                            //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + i.ToString());
                                            //}

                                            //imageScreenshott.SaveAsFile(pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                            imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);


                                            Thread.Sleep(100);

                                            //string pathToFile = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString) + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                            string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                            //pathToFolder = pathToSave + "\\Messenger\\" + dicoMessenger[link] + (idFictifString == "1" ? "" : idFictifString);
                                            pathToFolder = pathToSaveScreenshot;

                                            foreach (string cle in messagesVisibles.Keys)
                                            {

                                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                                {

                                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                    string tmp = "";
                                                    foreach (string li in lignes)
                                                    {
                                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                                    }



                                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                                }

                                                //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                            }

                                            if (videosVisibles.Count > 0)
                                                foreach (string cle in videosVisibles.Keys)
                                                {
                                                    //if (!videosVisiblesForFile.ContainsKey(cle))
                                                    //{

                                                    videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                    //}
                                                }

                                            messagesVisibles = new Dictionary<string, string>();
                                            videosVisibles = new Dictionary<string, string>();
                                            hauteur += 400;


                                            i++;
                                        }

                                        FillDataGridViewMessenger(pathToFolder, link);
                                    }
                                    else//Si pas de scrollbar alors seulement une page------------------------------------------------------------------------------------------
                                    {

                                        foreach (string classe in classesConnues)
                                        {
                                            if (classe.Contains("_2poz _52mr _ui9 _2n8h _2n8i _5fk1"))//si classe contenant vidéo on télécharge d'abord la vidéo
                                            {


                                                IList<IWebElement> allDivElements = driverMessenger.FindElementsByXPath(classe + "//video");//_ox1 _21y0
                                                for (int ii = 0; ii < allDivElements.Count(); ii++)
                                                {

                                                    if (allDivElements[ii].GetAttribute("class") != null)
                                                    {
                                                        //here the print statement will print the value of each div tag element
                                                        var tmp = allDivElements[ii].GetAttribute("class");

                                                        if (tmp == "_ox1 _21y0")// si div avec vidéo
                                                        {
                                                            //var video = driver.FindElementsByClassName(tmp);
                                                            //IList<IWebElement> allDivElementss = driver.FindElementsByClassName(tmp);

                                                            try
                                                            {
                                                                //var tmpp = allDivElementss[0].FindElement(By.XPath("video")).GetAttribute("src");
                                                                var tmpp = allDivElements[ii].GetAttribute("src");


                                                                if (allDivElements[ii].Location.Y > 15 && allDivElements[ii].Location.Y < (resolutionEcran - 250))
                                                                {
                                                                    if (!videosVisibles.ContainsKey(tmpp + ii))
                                                                    {
                                                                        videosVisibles.Add(tmpp + ii, tmpp);
                                                                    }
                                                                }
                                                                //break;



                                                                //IWebElement oo = allDivElementss[0].FindElement(By.XPath("video"));


                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }

                                                        }
                                                    }

                                                }




                                            }



                                            var messages = driverMessenger.FindElementsByXPath(classe);
                                            object[] messagesToExtract = messages.ToArray();



                                            foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messagesToExtract)
                                            {

                                                string tentation = o.ToString();
                                                string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                                if (!dicoMessagesFrom.ContainsKey(idd))
                                                {
                                                    dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                }
                                                //else
                                                //    continue;

                                                if (o.Location.Y + o.Size.Height > 15 && o.Location.Y < (resolutionEcran - 300))
                                                {
                                                    if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                                    {
                                                        messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                                    }
                                                }

                                            }
                                        }


                                        imageDown = driverMessenger.FindElementById(idTmp);
                                        textes = imageDown.Text + "\n";

                                        Screenshot imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        imageScreenshott = ((ITakesScreenshot)driverMessenger).GetScreenshot();
                                        //Save the screenshot
                                        //if (!Directory.Exists(pathToSave + "\\Messenger\\" + dicoMessenger[link]))
                                        //{
                                        //    //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link]);
                                        //}
                                        //else
                                        //if (dicoMessenger[link].ToLower().Contains("utilisateur de") || dicoMessenger[link].ToLower().Contains("user"))
                                        //{
                                        //    idFictif++;
                                        //    idFictifString = idFictif.ToString();
                                        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link] + i.ToString());
                                        //}



                                        imageScreenshott.SaveAsFile(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
                                        Thread.Sleep(100);

                                        string pathToFile = pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + "_" + i + ".jpg";
                                        pathToFolder = pathToSaveScreenshot;

                                        foreach (string cle in messagesVisibles.Keys)
                                        {

                                            if (!messagesVisiblesForFile.ContainsKey(cle))
                                            {

                                                string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                                string tmp = "";
                                                foreach (string li in lignes)
                                                {
                                                    tmp += li.Trim().Replace("\r", "");
                                                }



                                                messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                            }

                                            //messagesVisiblesWithScreenshots += valeur + ";" + pathToFile + "\n";
                                        }

                                        if (videosVisibles.Count > 0)
                                            foreach (string cle in videosVisibles.Keys)
                                            {
                                                //if (!videosVisiblesForFile.ContainsKey(cle))
                                                //{

                                                videosVisiblesForFile.Add(cle + ";" + pathToFile + "\n");
                                                //}
                                            }

                                        messagesVisibles = new Dictionary<string, string>();
                                        videosVisibles = new Dictionary<string, string>();

                                        FillDataGridViewMessenger(pathToFolder, link);
                                        //continue;
                                    }




                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("error dans Messenger : " + ex.Message);
                                }


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("error dans Messenger " + ex.Message);
                            }

                            string codePagee = driverMessenger.PageSource;

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_" + dicoMessenger[link] + ".txt", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                file.Write(textes);
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages.txt", false))
                            {
                                //if (File.Exists(saveFileDialog1.FileName))
                                //    File.Delete(saveFileDialog1.FileName);

                                textes = "";
                                foreach (string t in messagesVisibles.Values)
                                {
                                    textes += t + "\n";
                                }


                                file.Write(textes);
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Messages_With_Screenshots.txt", false))
                            {
                                textes = "";
                                foreach (string t in messagesVisiblesForFile.Values)
                                {
                                    textes += t;
                                }


                                file.Write(textes);
                                messagesVisiblesForFile = new Dictionary<string, string>();
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSaveScreenshot + "\\Messenger_Videos_With_Screenshots.txt", false))
                            {
                                if (videosVisiblesForFile.Count > 0)
                                {
                                    textes = "";
                                    foreach (string t in videosVisiblesForFile)
                                    {
                                        textes += t;
                                    }


                                    file.Write(textes);

                                }



                            }

                            if (!Directory.Exists(pathToSaveScreenshot + "\\Videos\\"))
                            {
                                //EraseDirectory(pathToSave + "\\Messenger\\" + dicoMessenger[link], true);  
                                Directory.CreateDirectory(pathToSaveScreenshot + "\\Videos\\");
                            }

                            using (var client = new WebClient())
                            {

                                try

                                {
                                    Dictionary<string, string> dico = new Dictionary<string, string>();
                                    foreach (string t in videosVisiblesForFile)
                                    {
                                        //FileInfo fileinfo = new FileInfo(t);
                                        string keyy = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];

                                        if (!dico.ContainsKey(keyy))
                                        {
                                            dico.Add(keyy, t);
                                            string nomFichier = t.Substring(t.LastIndexOf('/') + 1).Split('?')[0];
                                            client.DownloadFile(t, pathToSaveScreenshot + "\\Videos\\" + nomFichier);
                                            Thread.Sleep(100);
                                        }


                                    }

                                    videosVisiblesForFile = new List<string>();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DES VIDEOS");
                                    //return;
                                }
                            }


                            Thread.Sleep(2500);

                            textes = "";
                            messagesVisiblesForFile = new Dictionary<string, string>();
                            messagesVisibles = new Dictionary<string, string>();
                            videosVisibles = new Dictionary<string, string>();
                            videosVisiblesForFile = new List<string>();
                            
                            
                            //idFictif = 1;
                        }

                        textes = "";
                        messagesVisiblesForFile = new Dictionary<string, string>();
                        messagesVisibles = new Dictionary<string, string>();
                        videosVisibles = new Dictionary<string, string>();
                        idFictif = 1;
                    }


                }
                catch
                {

                }


                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");

                    while (true)
                    {
                        ((IJavaScriptExecutor)driverMessenger).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        Thread.Sleep(2000);

                        Object newHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.body.scrollHeight");
                        if (newHeight.Equals(lastHeight))
                        {
                            break;
                        }
                        lastHeight = newHeight;
                    }
                }
                catch
                {
                    //e.printStackTrace();
                }



                //string codePage = driverMessenger.PageSource;

                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\timeline_" + ".html", false))
                //{


                //    file.Write(codePage);
                //}


                Thread.Sleep(2500);
                //nbreAnnee++;






                driverMessenger.Quit();



                //progressBar1.Maximum = lignes.Count() - 1;



            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {

                driverMessenger.Quit();
            }


            //}
            //Thread.Sleep(2000);
            //backgroundWorker1.ReportProgress(-3);
            Thread.Sleep(2000);
            //backgroundWorker1.CancelAsync();

            if (backgroundWorkerMessenger != null && backgroundWorkerMessenger.IsBusy)
                backgroundWorkerMessenger.CancelAsync();

            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
        }

        private void GetContactsMessenger()
        {

            

            if (driver == null)
            {
                InitializeDriver();
                driverMessenger = driver;

                // 2. Go to the "Google" homepage
                driverMessenger.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driverMessenger.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driverMessenger.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driverMessenger.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;           
            System.Random rnd = new System.Random();
            

            if (!profilIsSet)
                GetProfileInformations(backgroundWorkerMessenger);
                        

            driverMessenger = driver;


            try
                {


                    string targetName = textBoxops.Text;

                    //récupération des années 
                    //var years = driver.FindElementByXPath("rightColWrap']").Text;
                    //string codePagee = ((OpenQA.Selenium.Remote.RemoteWebDriver)((OpenQA.Selenium.Remote.RemoteWebElement)years).WrappedDriver).PageSource;
                    //string[] liYears = years.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);


                if (checkBoxFrench.Checked)
                    SetFrenchLanguage();
                //try 
                //{
                //    //on essai d'identifier la langue utilisée pour Facebook
                //    driverMessenger.Navigate().GoToUrl("https://www.facebook.com/settings?tab=language");
                //    Thread.Sleep(5000);

                //    IWebElement combo = driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemContent fcg']"));
                //    driverMessenger.FindElement(By.XPath("//span[@class='fbSettingsListItemEditText']")).Click();
                //    Thread.Sleep(500);

                //    SelectElement comboo = new SelectElement(driver.FindElement(By.TagName("select")));
                //    IWebElement item = comboo.SelectedOption;
                //    LANGUAGESELECTED = item.GetAttribute("value");
                //    if(item.GetAttribute("value") != "fr_FR")
                //    {
                //        comboo.SelectByValue("fr_FR");
                //        Thread.Sleep(1500);

                //        //submit uiButton uiButtonConfirm
                //        driverMessenger.FindElement(By.XPath("//label[@class='submit uiButton uiButtonConfirm']")).Click();
                //        Thread.Sleep(2500);
                //    }
                    

                //}
                //catch (NoSuchElementException ex)
                //{

                //}





                driverMessenger.Navigate().GoToUrl("https://www.facebook.com/messages/t");//https://www.facebook.com/messages/t/MOD.orga
                    Thread.Sleep(5000);


                    try
                    {
                        var imageDown = driverMessenger.FindElementByXPath("//div[@class='uiScrollableAreaWrap scrollable']");
                        Object lastHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight");

                        while (true)
                        {
                            //driver.execute_script('document.getElementById("viewport").scrollTop += 100')

                            ((IJavaScriptExecutor)driverMessenger).ExecuteScript("document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollTo(0, document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight)");
                            Thread.Sleep(2000);

                            Object newHeight = ((IJavaScriptExecutor)driverMessenger).ExecuteScript("return document.getElementsByClassName('uiScrollableAreaWrap scrollable')[0].scrollHeight");
                            if (newHeight.Equals(lastHeight))
                            {
                                break;
                            }
                            lastHeight = newHeight;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                    //IWebElement liste = driver.FindElement(By.ClassName("uiScrollableAreaContent"));

                    string[] id = driverMessenger.PageSource.Split(new String[] { "aria-label=\"Conversations\"" }, StringSplitOptions.RemoveEmptyEntries);
                    string idTmp = "";

                    //on récupère l'id dynamique du composant

                    foreach (string idd in id)
                    {
                        if ( idd.StartsWith("<html"))
                            continue;


                        idTmp = idd.Substring(idd.IndexOf("id=\"") + 4).Split('"')[0];
                    }


                    IWebElement liste = driver.FindElement(By.Id(idTmp));

                    IList <IWebElement> ell = liste.FindElements(By.TagName("li"));

                    //IWebElement liste = driver.FindElement(By.ClassName("uiScrollableAreaContent"));
                    //IList<IWebElement> ell = driver.FindElements(By.XPath("//li[@class='_5l-3 _1ht1 _6zk9']"));
                    List <ForGrid> listeUtilisateurs = new List<ForGrid>();

                    //string urlContactMessenger = "";
                    //string userNameMessenger = "";
                    //string urlImageMessenger = "";

                    //listeUtilisateurs.Add(forGrid);


                    if (ell.Count > 0)
                    {
                        foreach(IWebElement el in ell)
                        {
                            ForGrid forGrid = new ForGrid();
                            string urlContactMessenger = "";
                            string userNameMessenger = "";
                            string urlImageMessenger = "";

                            string innerHTML = el.GetAttribute("innerHTML");

                            if (innerHTML.Contains("data-href=\"https://www.facebook.com/messages/t"))
                            {
                                urlContactMessenger = innerHTML.Substring(innerHTML.IndexOf("data-href=\"") + 11).Split('"')[0];
                                if (urlContactMessenger.Contains("#"))
                                    continue;
                            }

                            if (innerHTML.Contains("data-tooltip-content=\""))
                            {
                                userNameMessenger = innerHTML.Substring(innerHTML.IndexOf("data-tooltip-content=\"") + 22).Split('"')[0];
                            }

                            if (innerHTML.Contains("background-image: url(&quot;"))
                            {
                                urlImageMessenger = innerHTML.Substring(innerHTML.IndexOf("background-image: url(&quot;") + 28).Split('"')[0];
                            }
                            else
                            if (innerHTML.Contains("img class") || innerHTML.Contains("img alt"))
                            {
                                urlImageMessenger = innerHTML.Substring(innerHTML.IndexOf("src=\"") + 5).Split('"')[0];
                            }
                            else
                            {
                                urlImageMessenger = pathConfig + "\\anonymous.jpg";

                            }
                          
                            string tmp = userNameMessenger.Replace("&amp;", "&").Replace("&quot;", "'");
                            if(tmp.Length > 25)
                            {
                                tmp = tmp.Substring(0, 25);// + "...";
                            }
                            forGrid.Url = urlContactMessenger;
                            forGrid.Username = tmp.Replace(":","").Replace("'","").Replace("/","").Replace("\\","").Replace("\"","").Replace("|","").Replace("<","").Replace(">","").Replace("*","").Replace("?","");
                            forGrid.PathToPicture = urlImageMessenger.Replace("&amp;", "&"); 



                            //if (el.GetAttribute("data-href") != null  || el.GetAttribute("data-href") != "")
                            //{
                            //    string linkk = el.GetAttribute("data-href");
                            //    forGrid.Url = linkk;

                            //    string usernamee = "";
                            //    string picture = "";
                            //    if(el.FindElements(By.TagName("span"))[0].Text != "")
                            //    //if (el.FindElements(By.TagName("div"))[0].FindElement(By.TagName("div")).GetAttribute("data-tooltip-content") == null)
                            //    {
                            //        usernamee = el.FindElements(By.TagName("span"))[0].Text;
                            //        forGrid.Username = usernamee;

                            //    }
                            //    //else
                            //    //{
                            //    //    usernamee = el.FindElements(By.TagName("div"))[0].FindElement(By.TagName("div")).GetAttribute("data-tooltip-content");
                            //    //    forGrid.Username = usernamee;
                            //    //}
                                   

                            //    picture = el.FindElement(By.TagName("img")).GetAttribute("src");
                            //    forGrid.PathToPicture = picture;
                            //}

                            listeUtilisateurs.Add(forGrid);
                        }
                    }

                    if (!Directory.Exists(pathToSave + "\\Messenger\\PicturesProfiles"))
                        Directory.CreateDirectory(pathToSave + "\\Messenger\\PicturesProfiles");

                    int ii = 1;
                    foreach (ForGrid forGr in listeUtilisateurs)
                    {
                        if (!dicoMessenger.Keys.Contains(forGr.Url))
                        {
                            dicoMessenger.Add(forGr.Url, forGr.Username);
                        }

                        string linkkk = "";
                        string usernameee = "";
                        
                        backgroundWorkerMessenger.ReportProgress(-1, listeUtilisateurs.Count());

                        try

                        {
                            backgroundWorkerMessenger.ReportProgress(ii);

                            using (var client = new WebClient())
                            {
                                if (!File.Exists(pathToSave + "\\Messenger\\PicturesProfiles\\" + forGr.Username + ".jpg"))
                                {
                                    client.DownloadFile(forGr.PathToPicture, pathToSave + "\\Messenger\\PicturesProfiles\\" + forGr.Username + ".jpg");
                                }


                                Thread.Sleep(500);

                            }

                            //backgroundWorkerMessenger.ReportProgress(-5, pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");

                            //Image image = Image.FromFile(pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");
                            string pathToFolder = pathToSave + "\\Messenger\\PicturesProfiles\\" + forGr.Username + ".jpg";
                            //Bitmap pic = new Bitmap(50, 50);
                            //pic = (Bitmap)Image.FromFile(pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");
                            //image.Image = image;
                            //image.Size = new Size(50, 50);
                            ForGrid forGridd = new ForGrid();
                            forGridd.Username =forGr.Username;
                            forGridd.PathToFolder = pathToFolder;
                            forGridd.Url = forGr.Url;
                            forGridd.PathToPicture = pathToSave + "\\Messenger\\PicturesProfiles\\" + forGr.Username + ".jpg";


                            backgroundWorkerMessenger.ReportProgress(-6, forGridd);
                            ii++;


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL MESSENGER " + ex.Message);
                            //return;
                        }

                    }
                    //    string messenger = driverMessenger.PageSource;

                    //    string debut = messenger.Substring(messenger.IndexOf("aria-label=\"Conversations\""));
                    //    string[] destinataires = debut.Split(new String[] { "data-href=\"" }, StringSplitOptions.RemoveEmptyEntries);
                    //    //string[] profilePictures = debut.Split(new string[] { "<img class=\"_87v3 img\"" }, StringSplitOptions.RemoveEmptyEntries);//_6zkb _7t1l _6zkc
                    //    string[] profilePictures = debut.Split(new string[] { "<div class=\"_6zkb _7t1l _6zkc\">" }, StringSplitOptions.RemoveEmptyEntries);
                    //    // on récupère les destinataires

                    //    foreach (string dest in destinataires)
                    //    {
                    //        if (!dest.StartsWith("https://www.facebook.com/messages/t/"))
                    //            continue;

                    //        string linkk = dest.Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    //        if (linkk.Contains("#"))
                    //            continue;

                    //        string usernamee = dest.Substring(dest.IndexOf("data-tooltip-content=\"") + 21).Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    //        if (!dicoMessenger.Keys.Contains(linkk))
                    //        {
                    //            dicoMessenger.Add(linkk, usernamee);
                    //        }
                    //    }

                    //    // on récupère les photos de profils

                    //    string link = "";
                    //    string username = "";
                    //    int i = 0;
                    //backgroundWorkerMessenger.ReportProgress(-1, profilePictures.Count());

                    //if (!Directory.Exists(pathToSave + "\\Messenger\\PicturesProfiles"))
                    //    Directory.CreateDirectory(pathToSave + "\\Messenger\\PicturesProfiles");

                    //foreach (string imagee in profilePictures)
                    //{
                    //        if (!imagee.StartsWith("<div data-tooltip-root-class=") && !imagee.StartsWith("<div data-tooltip-content="))
                    //            continue;

                    //        if(imagee.StartsWith("<div data-tooltip-root-class="))
                    //        {
                    //            link = imagee.Substring(imagee.IndexOf("src=\"") + 5).Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("&amp;", "&");
                    //            username = imagee.Substring(imagee.IndexOf("data-tooltip-content=\"") + 22).Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];

                    //        }
                    //        else
                    //        if (imagee.StartsWith("<div data-tooltip-content"))
                    //        {
                    //            link = imagee.Substring(imagee.IndexOf("background-image: url") + 28).Split(new String[] { "&quot;);" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("&amp;", "&");//&quot;);
                    //            username = imagee.Substring(imagee.IndexOf("data-tooltip-content=\"") + 22).Split(new String[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //        }

                    //        //if (!Directory.Exists(pathToSave + "\\Messenger\\PicturesProfiles" ))
                    //        //    Directory.CreateDirectory(pathToSave + "\\Messenger\\PicturesProfiles");

                    //    backgroundWorkerMessenger.ReportProgress(i+1);

                    //        try

                    //        {
                    //            using (var client = new WebClient())
                    //            {
                    //                if (!File.Exists(pathToSave + "\\Messenger\\PicturesProfiles\\"+ username + ".jpg"))
                    //                {
                    //                    client.DownloadFile(link, pathToSave + "\\Messenger\\PicturesProfiles\\"+ username + ".jpg");
                    //                }
                                    

                    //                Thread.Sleep(500);

                    //            }

                    //            //backgroundWorkerMessenger.ReportProgress(-5, pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");

                    //            //Image image = Image.FromFile(pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");
                    //            string pathToFolder = pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg";
                    //            //Bitmap pic = new Bitmap(50, 50);
                    //            //pic = (Bitmap)Image.FromFile(pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg");
                    //            //image.Image = image;
                    //            //image.Size = new Size(50, 50);
                    //            ForGrid forGrid = new ForGrid();
                    //            forGrid.Username = username;
                    //            forGrid.PathToFolder = pathToFolder;
                    //            forGrid.Url = dicoMessenger.ElementAt(i).Key;
                    //            forGrid.PathToPicture = pathToSave + "\\Messenger\\PicturesProfiles\\" + username + ".jpg";

                           
                    //        backgroundWorkerMessenger.ReportProgress(-6, forGrid);
                    //            i++;
                             

                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT DE LA PHOTO DE PROFIL MESSENGER " + ex.Message);
                    //            //return;
                    //        }
                            
                    //    }

                    


                    }
                    catch
                    {

                    }
                                   




                    //driver.Close();



                    //progressBar1.Maximum = lignes.Count() - 1;



                }
                catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
                {
                ;

                }


            //}
            //Thread.Sleep(2000);
            //backgroundWorker1.ReportProgress(-3);
            Thread.Sleep(2000);
            //backgroundWorker1.CancelAsync();
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;
        }

        private void FillDataGridViewMessenger(Image image, string username, string link, string pathToPictureProfile)
        {
            try
            {
                dataGridViewMessenger.Rows.Add(image, username, link, false, "", pathToPictureProfile);
                dataGridViewMessenger.ClearSelection();
                //dataGridViewPictures.Rows[rowIndex].Selected = true;
                dataGridViewMessenger.FirstDisplayedScrollingRowIndex = dataGridViewMessenger.Rows.Count - 1;
                //dataGridViewMessenger.Refresh();
            }
            catch
            {

            }
            
        }

        private void FillDataGridViewMessenger(string pathToFolder, string link)
        {
            
            foreach(DataGridViewRow row in dataGridViewMessenger.Rows)
            {
                if(row.Cells[2].Value.ToString() == link)
                {
                    row.Cells[3].Value = false;
                    row.Cells[4].Value = pathToFolder;
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
                    row.DefaultCellStyle.ForeColor = Color.White;

                    dataGridViewMessenger.ClearSelection();
                    //dataGridViewPictures.Rows[rowIndex].Selected = true;
                    dataGridViewMessenger.FirstDisplayedScrollingRowIndex = row.Index;
                    

                    dataGridViewMessenger.Focus();

                    break;
                }
            }

            dataGridViewMessenger.Refresh();


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxwaiting_Click(object sender, EventArgs e)
        {

        }

        private void InitializeDatagridViewMessenger()
        {
            getMessenger = true;
            
            pictureBoxlogofacebook.Visible = true;
            pictureBoxwaiting.Visible = true;
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Visible = true;
            pictureBoxlogofacebook.BringToFront();
            pictureBoxwaiting.Refresh();
            pictureBoxlogofacebook.Refresh();
            dataGridViewMessenger.Rows.Clear();
            dataGridViewPictures.Rows.Clear();
            //progressBarContactMessenger.Visible = true;
            progressBarContactMessenger.Maximum = 0;
            progressBarContactMessenger.Value = 0;
            
        }

        private void dataGridViewMessenger_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                //Reference the GridView Row.
                DataGridViewRow row = dataGridViewMessenger.Rows[e.RowIndex];



                row.Cells[3].Value = !(bool)row.Cells[3].Value;



            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            selected = !selected;
            
            foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
            {
                
                if (selected)
                {
                    row.Cells[3].Value = false;
                }

                if (!selected)
                {
                    row.Cells[3].Value = true;
                }
            }

            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            if (textBoxops.Text == "")
            {
                textBoxops.BackColor = Color.Red;
                MessageBox.Show("Veuillez remplir le champ OPS");
                return;
            }
            else
                textBoxops.BackColor = Color.White;
            

            Dictionary<string, string> dicoDestinatairesFromGrid = new Dictionary<string, string>();

            if(dataGridViewMessenger.Rows.Count > 0)
            {
                dataGridViewPictures.Rows.Clear();
                dicoResultsIndexSearch = new Dictionary<int, int>();
                flowLayoutPanel2.Controls.Clear();

                foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
                {
                    bool isSelected = Convert.ToBoolean(row.Cells[3].Value);
                    if (isSelected)
                    {
                       if(!dicoDestinatairesFromGrid.ContainsKey(row.Cells[2].Value.ToString())) 
                        dicoDestinatairesFromGrid.Add(row.Cells[2].Value.ToString(), row.Cells[1].Value.ToString());                        
                        
                    }
                    
                }

            }

            if(dicoDestinatairesFromGrid.Count > 0)
            {
                //MessengerFromDate(dicoDestinatairesFromGrid,"");

                dataGridViewMessenger.ClearSelection();
                dataGridViewMessenger.FirstDisplayedScrollingRowIndex = 0;
                dataGridViewMessenger.Focus();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxwaiting.Visible = true;
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Visible = true;
                pictureBoxlogofacebook.BringToFront();
                pictureBoxwaiting.Refresh();
                pictureBoxlogofacebook.Refresh();

                if (!backgroundWorkerGetMessenger.IsBusy)
                    backgroundWorkerGetMessenger.RunWorkerAsync(dicoDestinatairesFromGrid);
            }
                
        }

        private void dataGridViewMessenger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridViewMessenger.Rows.Count > 0)//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
            {

                string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();

                if (test == "")
                    return;

                panel5.Visible = false;
                flowLayoutPanel2.Controls.Clear();
                panelNext.Visible = false;

                if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                    sortedListForSearching.Clear();


                videos = new Dictionary<string, string>();
                audios = new Dictionary<string, string>();
                
                dataGridViewPictures.Rows.Clear();
                var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
                fichiers = sorted.ToArray();

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView2.Size.Height;
                int hauteurForm = MESSENGERR.Size.Height;
                int indexPage = 0;


                foreach (FileInfo fichier in fichiers)
                {

                    if (indexPage == STEPP)
                        break;
                    Image tmp = Image.FromFile(fichier.FullName);
                    int differentiel = tmp.Width - tmp.Height;
                    Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - (120 - differentiel), hauteurForm - 120)));

                    //Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm-84, hauteurForm - 85)));
                    dataGridViewPictures.Rows.Add(imgg, fichier.Name);


                    indexPage++;
                }

                NextMessenger = STEPP;

                //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
                labelNbreMessenger.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString());

                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");


                    foreach (string li in lignes)
                    {
                        if (li == "")
                            continue;

                        string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];


                        int indexx = 0;
                        Int32.TryParse(numeroLigne, out indexx);


                        if (!videos.ContainsKey(((indexx).ToString())))
                        {
                            //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
                            videos.Add(((indexx).ToString()), ((indexx).ToString()));
                            //videos.Add(((indexx - 1).ToString(), ((indexx - 1).ToString()));
                        }



                    }
                }

                if (videos.Count > 0)
                {
                    if (videos.ContainsKey((dataGridViewPictures.FirstDisplayedScrollingRowIndex).ToString()))
                    {
                        pictureBox4.Visible = true;
                        panel5.Visible = true;
                    }

                    else
                    {
                        pictureBox4.Visible = false;
                        panel5.Visible = false;
                    }

                }

                if (File.Exists(test + "\\Messenger_Audio_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Audio_With_Screenshots.txt");


                    foreach (string li in lignes)
                    {
                        if (li == "")
                            continue;

                        string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];


                        int indexx = 0;
                        Int32.TryParse(numeroLigne, out indexx);


                        if (!audios.ContainsKey(((indexx).ToString())))
                        {
                            //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
                            audios.Add(((indexx).ToString()), ((indexx).ToString()));
                            //videos.Add(((indexx - 1).ToString(), ((indexx - 1).ToString()));
                        }



                    }
                }

                if (audios.Count > 0)
                {
                    if (audios.ContainsKey((dataGridViewPictures.FirstDisplayedScrollingRowIndex).ToString()))
                    {
                        pictureBox4.Visible = true;
                        pictureBox10.Visible = true;
                        panel5.Visible = true;
                    }

                    else
                    {
                        pictureBox4.Visible = false;
                        pictureBox10.Visible = false;
                        panel5.Visible = false;
                    }

                }

            }

        }
        //{

        //    if (dataGridViewMessenger.Rows.Count > 0 )//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
        //    {
        //        panel7.Visible = false;

        //        if (sortedListForSearching != null || sortedListForSearching.Count > 0)
        //            sortedListForSearching.Clear();


        //        //videos = new Dictionary<string, string>();
        //        string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
        //        dataGridViewPictures.Rows.Clear();
        //        var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
        //        FileInfo[] fichiers = sorted.ToArray();

        //        Rectangle rect = GetResolutionScreen();
        //        int hauteurGrid = dataGridView2.Size.Height;
        //        int hauteurForm = MESSENGERR.Size.Height;

        //        foreach (FileInfo fichier in fichiers)
        //        {

        //            Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
        //            dataGridViewPictures.Rows.Add(img);

        //        }

        //        if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");


        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                int indexx = 0;
        //                Int32.TryParse(numeroLigne, out indexx);

        //                if (!videos.ContainsKey((indexx - 1).ToString()))
        //                {
        //                    //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
        //                    videos.Add(((indexx - 1).ToString()), ((indexx - 1).ToString()));
        //                }



        //            }
        //        }


        //    }

        //}

        //private void tabPage1_Click(object sender, EventArgs e)
        //{

        //}

        //private void button4_Click(object sender, EventArgs e)
        //{

        //    //string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
        //    string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Henri Dewyse";
        //    dataGridViewPictures.Rows.Clear();
        //    var sorted = Directory.GetFiles(test, "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
        //    FileInfo[] fichiers = sorted.ToArray();

        //    Rectangle rect = GetResolutionScreen();
        //    int hauteurGrid = dataGridView2.Size.Height + 15;
        //    int hauteurForm = MESSENGERR.Size.Height;

        //    foreach (FileInfo fichier in fichiers)
        //    {
        //        //PictureBox imageViewer = new PictureBox();
        //        //imageViewer.Image = Image.FromFile(fichier.FullName);
        //        //imageViewer.SizeMode = PictureBoxSizeMode.Normal;
        //        //imageViewer.Dock = DockStyle.Bottom;
        //        //imageViewer.Height = 1250;
        //        //imageViewer.Width = 1250;
        //        Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
        //        dataGridViewPictures.Rows.Add(img);
        //        //flowLayoutPanel1.Controls.Add(imageViewer);
        //    }
        //}

        private Rectangle GetResolutionScreen()
        {
            
            Rectangle resolution = Screen.PrimaryScreen.WorkingArea;

            //pour connaitre la taille de l'écran actuel
            Screen scrn = Screen.FromControl(this);

            return scrn.WorkingArea;

            
            //foreach (Screen screen in Screen.AllScreens)
            //{
            //    DEVMODE dm = new DEVMODE();
            //    dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            //    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

            //    Console.WriteLine($"Device: {screen.DeviceName}");
            //    Console.WriteLine($"Real Resolution: {dm.dmPelsWidth}x{dm.dmPelsHeight}");
            //    Console.WriteLine($"Virtual Resolution: {screen.Bounds.Width}x{screen.Bounds.Height}");
            //    Console.WriteLine();
            //}
        }

        private void FillJournalView()
        {
            if (!Directory.Exists(pathToSave  + "\\HOMEPAGE"))
                return;

            var sorted = Directory.GetFiles(pathToSave + "\\HOMEPAGE", "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
            fichiersJournal = sorted.ToArray();

            if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                sortedListForSearching.Clear();



            dataGridViewJournal.Rows.Clear();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridViewJournal.Size.Height;
            int hauteurForm = JOURNAL.Size.Height;

            int indexPage = 0;
            Previous = 0;

            foreach (FileInfo fichier in fichiersJournal)
            {

                if (indexPage == STEPP)
                    break;
                Image tmp = Image.FromFile(fichier.FullName);
                int differentiel = tmp.Width - tmp.Height;
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - (120 - differentiel), hauteurForm - 120)));
                //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                dataGridViewJournal.Rows.Add(imgg);


                indexPage++;
            }

            Next = STEPP;
            nbreScreenshots.Text = fichiersJournal.Length + " screenshots";
            //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
            deA.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersJournal.Length - 1).ToString());





        }
        private void FillJournalViewForBusiness()
        {
            if (!Directory.Exists(pathToSave + "\\HOMEPAGE"))
                return;

            var sorted = Directory.GetFiles(pathToSave + "\\HOMEPAGE", "*.png").Select(fn => new FileInfo(fn)).Where(f => f.Name.StartsWith("Element")).OrderBy(f => f.LastWriteTime);
            fichiersJournal = sorted.ToArray();

            if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                sortedListForSearching.Clear();



            dataGridViewJournal.Rows.Clear();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridViewJournal.Size.Height;
            int hauteurForm = JOURNAL.Size.Height;

            int indexPage = 0;
            Previous = 0;

            foreach (FileInfo fichier in fichiersJournal)
            {

                if (indexPage == STEPP)
                    break;
                Image tmp = Image.FromFile(fichier.FullName);
                int differentiel = tmp.Width - tmp.Height;
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width-119, hauteurForm - 120)));
                //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                dataGridViewJournal.Rows.Add(imgg);


                indexPage++;
            }

            Next = STEPP;
            nbreScreenshots.Text = fichiersJournal.Length + " screenshots";
            //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
            deA.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersJournal.Length - 1).ToString());





        }
        private void FillJournalViewImport(string pathToSave)
        {
            if (!Directory.Exists(pathToSave +  "\\HOMEPAGE"))
                return;

            var sorted = Directory.GetFiles(pathToSave +  "\\HOMEPAGE", "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
            fichiersJournal = sorted.ToArray();

            if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                sortedListForSearching.Clear();



            dataGridViewJournal.Rows.Clear();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridViewJournal.Size.Height;
            int hauteurForm = JOURNAL.Size.Height;

            int indexPage = 0;
            Previous = 0;

            if(fichiersJournal.Count() > 0)
            {
                foreach (FileInfo fichier in fichiersJournal)
                {

                    if (indexPage == STEPP)
                        break;
                    Image tmp = Image.FromFile(fichier.FullName);
                    int differentiel = tmp.Width - tmp.Height;
                    Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - (120 - differentiel), hauteurForm - 120)));
                    //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                    dataGridViewJournal.Rows.Add(imgg);


                    indexPage++;
                }

                Next = STEPP;
                nbreScreenshots.Text = fichiersJournal.Length + " screenshots";
                //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
                deA.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersJournal.Length - 1).ToString());

                pictureBoxJournal.Visible = true;
            }
           





        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                flowLayoutPanel2.Controls.Clear();
                panelNext.Visible = false;

                return;
            }

            SearchKeywords(textBox1.Text);
        }

        private void SearchKeywords(string keyword)
        {
            if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
            {
                dicoResultsIndexSearch = new Dictionary<int, int>();
                flowLayoutPanel2.Controls.Clear();


                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_Messages_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Messages_With_Screenshots.txt");

                    if (sortedListForSearching.Count == 0)
                    {
                        foreach (string li in lignes)
                        {
                            if (li == "")
                                continue;

                            string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            sortedListForSearching.Add(li.ToLower());


                        }

                        try
                        {

                            List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

                            foreach (string r in resultatsRecherche)
                            {
                                string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                int indexx = Int32.Parse(numeroLigne) - 1;
                                string arechercher = r.Split(';')[0];

                                if (!arechercher.ToLower().Contains(keyword.ToLower()))
                                    continue;

                                if (!dicoResultsIndexSearch.ContainsKey(indexx))
                                {
                                    dicoResultsIndexSearch.Add(indexx, indexx);
                                    LinkLabel link = new LinkLabel();
                                    link.Text = (Int32.Parse(numeroLigne) - 1).ToString();
                                    link.AutoSize = true;
                                    link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    link.Name = "linkLabel" + (indexx - 1);
                                    link.Size = new System.Drawing.Size(18, 20);
                                    link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
                                    flowLayoutPanel2.Controls.Add(link);
                                    //labelResultsSearch.Text += indexx.ToString() + " ";
                                }

                                labelResultsSearch.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


                            }



                            //int index = sortedListForSearching.FindIndex(x => x.Contains(keyword));

                            //if (index == -1)
                            //    return;

                            //string numeroLigne = (sortedListForSearching[index].Split(';')[1]).Substring(sortedListForSearching[index].Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            //int indexx = Int32.Parse(numeroLigne) - 1;

                            if (dicoResultsIndexSearch.Count > 0)
                            {
                                //dataGridViewPictures.ClearSelection();
                                dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
                                dataGridViewPictures.Focus();
                                flowLayoutPanel2.Visible = true;
                                panelNext.Visible = true;

                            }

                        }
                        catch (ArgumentNullException ex)
                        {
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

                            foreach (string r in resultatsRecherche)
                            {
                                string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                int indexx = Int32.Parse(numeroLigne) - 1;

                                string arechercher = r.Split(';')[0];

                                if (!arechercher.ToLower().Contains(keyword.ToLower()))
                                    continue;

                                if (!dicoResultsIndexSearch.ContainsKey(indexx))
                                {
                                    dicoResultsIndexSearch.Add(indexx, indexx);
                                    LinkLabel link = new LinkLabel();
                                    link.Text = numeroLigne;
                                    link.AutoSize = true;
                                    link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    link.Name = "linkLabel" + indexx;
                                    link.Size = new System.Drawing.Size(18, 20);
                                    link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
                                    flowLayoutPanel2.Controls.Add(link);
                                    //labelResultsSearch.Text += indexx.ToString() + " ";
                                }

                                labelResultsSearch.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


                            }

                            if (dicoResultsIndexSearch.Count > 0)
                            {
                                //dataGridViewPictures.ClearSelection();
                                dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
                                dataGridViewPictures.Focus();

                                flowLayoutPanel2.Visible = true;
                                panelNext.Visible = true;
                            }
                        }
                        catch (ArgumentNullException ex)
                        {
                            return;
                        }
                    }


                }
            }
        }

        private void dataGridViewPictures_Scroll(object sender, ScrollEventArgs e)
        {


            foreach (Form f in Application.OpenForms)
            {

                if (f.Name == "FormVideos")
                {
                    
                    f.Close();                    
                    break;
                }

            }


            if (videos.Count > 0)
            {

                string nomPng = dataGridViewPictures.Rows[dataGridViewPictures.FirstDisplayedScrollingRowIndex].Cells[1].Value.ToString();
                string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];

                if (videos.ContainsKey(numeroVideo))
                {
                    pictureBox4.Visible = true;
                    panel5.Visible = true;
                }

                else
                {
                    pictureBox4.Visible = false;
                    panel5.Visible = false;
                }

            }

            if (audios.Count > 0)
            {

                string nomPng = dataGridViewPictures.Rows[dataGridViewPictures.FirstDisplayedScrollingRowIndex].Cells[1].Value.ToString();
                string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];

                if (audios.ContainsKey(numeroVideo))
                {
                    //pictureBox4.Visible = true;
                    pictureBox7.Visible = true;
                    panel5.Visible = true;
                }

                else
                {
                    //pictureBox4.Visible = false;
                    pictureBox7.Visible = false;
                    //panel5.Visible = false;
                }

            }

            string texteAGarder = labelNbreMessenger.Text.Split('/')[1];
            string numero = labelNbreMessenger.Text.Split('/')[0].Replace("_____", "");
            Int32.Parse(numero);
            int step = dataGridViewPictures.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridViewPictures.FirstDisplayedScrollingRowIndex;


            labelNbreMessenger.Text = "_____" + (step + (NextMessenger - STEPP)) + "/" + texteAGarder;
        }


        //private void SearchKeywords(string keyword)
        //{
        //    if(dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
        //    {
        //        dicoResultsIndexSearch = new Dictionary<int, int>();
        //        flowLayoutPanel2.Controls.Clear();


        //        string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Messages_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Messages_With_Screenshots.txt");

        //            if(sortedListForSearching.Count == 0)
        //            {
        //                foreach (string li in lignes)
        //                {
        //                    if (li == "")
        //                        continue;

        //                    string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                    sortedListForSearching.Add(li.ToLower());


        //                }

        //                try
        //                {

        //                    List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

        //                    foreach(string r in resultatsRecherche)
        //                    {
        //                        string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                        int indexx = Int32.Parse(numeroLigne) - 1;
        //                        string arechercher = r.Split(';')[0];

        //                        if (!arechercher.ToLower().Contains(keyword.ToLower()))
        //                            continue;

        //                        if (!dicoResultsIndexSearch.ContainsKey(indexx))
        //                        {
        //                            dicoResultsIndexSearch.Add(indexx, indexx);
        //                            LinkLabel link = new LinkLabel();
        //                            link.Text = numeroLigne;
        //                            link.AutoSize = true;
        //                            link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //                            link.Name = "linkLabel" + indexx;
        //                            link.Size = new System.Drawing.Size(18, 20);
        //                            link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        //                            flowLayoutPanel2.Controls.Add(link);
        //                            //labelResultsSearch.Text += indexx.ToString() + " ";
        //                        }

        //                        labelResultsSearch.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


        //                    }



        //                    //int index = sortedListForSearching.FindIndex(x => x.Contains(keyword));

        //                    //if (index == -1)
        //                    //    return;

        //                    //string numeroLigne = (sortedListForSearching[index].Split(';')[1]).Substring(sortedListForSearching[index].Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

        //                    //int indexx = Int32.Parse(numeroLigne) - 1;

        //                    if(dicoResultsIndexSearch.Count > 0)
        //                    {
        //                        dataGridViewPictures.ClearSelection();
        //                        dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
        //                        dataGridViewPictures.Focus();
        //                    }

        //                }
        //                catch (ArgumentNullException ex)
        //                {
        //                    return;
        //                }

        //            }
        //            else
        //            {
        //                try
        //                {
        //                    List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

        //                    foreach (string r in resultatsRecherche)
        //                    {
        //                        string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                        int indexx = Int32.Parse(numeroLigne) - 1;

        //                        if (!dicoResultsIndexSearch.ContainsKey(indexx))
        //                        {
        //                            dicoResultsIndexSearch.Add(indexx, indexx);
        //                            LinkLabel link = new LinkLabel();
        //                            link.Text = numeroLigne;
        //                            link.AutoSize = true;
        //                            link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //                            link.Name = "linkLabel" + indexx;
        //                            link.Size = new System.Drawing.Size(18, 20);
        //                            link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        //                            flowLayoutPanel2.Controls.Add(link);
        //                            //labelResultsSearch.Text += indexx.ToString() + " ";
        //                        }

        //                        labelResultsSearch.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


        //                    }

        //                    if (dicoResultsIndexSearch.Count > 0)
        //                    {
        //                        dataGridViewPictures.ClearSelection();
        //                        dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
        //                        dataGridViewPictures.Focus();
        //                    }
        //                }
        //                catch (ArgumentNullException ex)
        //                {
        //                    return;
        //                }
        //            }

        //            //foreach(string li in lignes)
        //            //{
        //            //    if (li == "")
        //            //        continue;

        //            //    if (li.ToLower().Contains(keyword.ToLower()))
        //            //    {

        //            //        try
        //            //        {
        //            //            string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

        //            //            int index = Int32.Parse(numeroLigne) - 1;

        //            //            dataGridViewPictures.ClearSelection();
        //            //            //dataGridViewPictures.Rows[rowIndex].Selected = true;
        //            //            dataGridViewPictures.FirstDisplayedScrollingRowIndex = index;

        //            //            dataGridViewPictures.Focus();


        //            //        }
        //            //        catch
        //            //        {

        //            //        }

        //            //    }
        //            //}
        //        }
        //    }
        //}

        public static bool EraseDirectory(string folderPath, bool recursive)
        {
            //Safety check for directory existence.
            if (!Directory.Exists(folderPath))
                return false;

            foreach (string file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);
            }

            //Iterate to sub directory only if required.
            if (recursive)
            {
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    EraseDirectory(dir, recursive);
                }
            }
            //Delete the parent directory before leaving
            Directory.Delete(folderPath);
            return true;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName,
                                       bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);

                try
                {
                    file.CopyTo(temppath, true);
                }
                catch
                {
                    Process pro = new Process();
                    pro.StartInfo.UseShellExecute = false;
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.StartInfo.CreateNoWindow = true;
                    pro.StartInfo.RedirectStandardOutput = true;
                    pro.StartInfo.FileName = "cmd.exe";
                    pro.StartInfo.Arguments = "/C copy \"" + file.FullName + "\" \"" + temppath + "\"";
                    pro.Start();
                    //Console.WriteLine(Process.StandardOutput.ReadToEnd());
                    pro.WaitForExit();
                    pro.Close();
                    
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void button4_Click_2(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0)
            {
                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                //if (File.Exists(test + "\\Messenger_" + test.Substring(test.LastIndexOf("\\") + 1) + ".txt"))
                //{
                //    Messages msg = new Messages();
                //    msg.SetRichTextBox(File.ReadAllText(test + "\\Messenger_" + test.Substring(test.LastIndexOf("\\") + 1) + ".txt"));
                //    msg.Show();

                //}

                if (File.Exists(test + "\\Messenger_Messages.txt"))
                {
                    Messages msg = new Messages();
                    msg.SetRichTextBox(File.ReadAllText(test + "\\Messenger_Messages.txt"));
                    msg.Show();

                }




            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            
            SaveCase();
            profilIsSet = false;
        }

        private void SaveCase()
        {


            //if (dataGridViewMessenger.Rows.Count > 0 || dataGridView2.Rows.Count > 0)
            //{


                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)

                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\" +  textBoxops.Text.ToUpper()))
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\" + textBoxops.Text.ToUpper());
                {
                    CopyData(pathToSave, folderBrowserDialog1.SelectedPath, textBoxops.Text);

                    //Données dans les champs
                    string ligne = "<Case>\n";
                    ligne += textBoxUSERNAMEFRIENDS.Text + ";" + textBoxUSERNAME.Text + ";" + textBoxPASSWORD.Text + ";" + textBoxops.Text.ToUpper() + ";"  + "..\\" + textBoxops.Text +";" + labelID.Text + "\n";
                    ligne += "</Case>\n";

                    //pictureProfile
                    if (labelpathPictureProfile.Text != "")
                    {
                        ligne += "<PictureProfile>\n" + labelpathPictureProfile.Text.Replace(pathToSave + textBoxops.Text.ToUpper(), ".") + "\n</PictureProfile>\n";
                    }
                    //DataGridViewMessenger
                    ligne += "<DataGridViewMessenger>\n";
                    foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
                    {
                        if (Directory.Exists(row.Cells[4].Value.ToString()) && Directory.GetFiles(row.Cells[4].Value.ToString()).Count() > 0)
                            ligne += row.Cells[5].Value.ToString().Replace(pathToSave, ".") + "; " + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + ";" + row.Cells[4].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".\\") + ";" + row.Cells[5].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".\\") + "\n";
                    }
                    ligne += "</DataGridViewMessenger>\n";


                    //DataGridViewPictures
                    ligne += "<DataGridViewFriends>\n";
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        ligne += row.Cells[4].Value.ToString().Replace(pathToSave, ".") + ";" + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + "\n";
                    }
                    ligne += "</DataGridViewFriends>\n";

                    //DataGridViewFollowers
                    ligne += "<DataGridViewFollowers>\n";
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        ligne += row.Cells[4].Value.ToString().Replace(pathToSave, ".") + ";" + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + "\n";
                    }
                    ligne += "</DataGridViewFollowers>\n";


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(folderBrowserDialog1.SelectedPath + "\\" + textBoxops.Text.ToUpper() + "\\" + textBoxops.Text.ToUpper() + "_CASE.fbv", false))
                    {
                        file.Write(ligne);
                    }

                    MessageBox.Show("Votre ECASE a été sauvegardé.");
                }



            //}
            //else
            //    CopyData(pathToSave + textBoxops.Text.ToUpper(), folderBrowserDialog1.SelectedPath, textBoxops.Text);
                              
        }
        private void SaveCaseClosingForm()
        {


            //if (dataGridViewMessenger.Rows.Count > 0 || dataGridView2.Rows.Count > 0)
            //{


           
               

                //Données dans les champs
                string ligne = "<Case>\n";
                ligne += textBoxUSERNAMEFRIENDS.Text + ";" + textBoxUSERNAME.Text + ";" + textBoxPASSWORD.Text + ";" + textBoxops.Text.ToUpper() + ";" + "..\\" + textBoxops.Text + ";" + labelID.Text + "\n";
                ligne += "</Case>\n";

                //pictureProfile
                if (labelpathPictureProfile.Text != "")
                {
                    ligne += "<PictureProfile>\n" + labelpathPictureProfile.Text.Replace(pathToSave + textBoxops.Text.ToUpper(), ".") + "\n</PictureProfile>\n";
                }
                //DataGridViewMessenger
                ligne += "<DataGridViewMessenger>\n";
                foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
                {
                    if (Directory.Exists(row.Cells[4].Value.ToString()) && Directory.GetFiles(row.Cells[4].Value.ToString()).Count() > 0)
                        ligne += row.Cells[5].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".") + "; " + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + ";" + row.Cells[4].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".\\") + ";" + row.Cells[5].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".\\") + "\n";
                }
                ligne += "</DataGridViewMessenger>\n";


                //DataGridViewPictures
                ligne += "<DataGridViewFriends>\n";
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    ligne += row.Cells[4].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".") + ";" + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + "\n";
                }
                ligne += "</DataGridViewFriends>\n";

                //DataGridViewFollowers
                ligne += "<DataGridViewFollowers>\n";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    ligne += row.Cells[4].Value.ToString().Replace(pathToSave + textBoxops.Text.ToUpper(), ".") + ";" + row.Cells[1].Value + ";" + row.Cells[2].Value + ";" + row.Cells[3].Value + "\n";
                }
                ligne += "</DataGridViewFollowers>\n";


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\" + textBoxops.Text + "_CASE.fbv", true))
                {
                    file.Write(ligne);
                }

               
            

        }

        private void CopyData(string path, string dest, string ops)
        {
            //if(MessageBox.Show("Etes-vous certain d'effacer " + "repertoire dest : " + dest + "\\" + ops) == DialogResult.OK)
            //{
                if (!Directory.Exists(dest + "\\" + ops))
                    Directory.CreateDirectory(dest + "\\" + ops);
                //else
                //    EraseDirectory(dest + "\\" + ops, true);

                foreach (string dir in Directory.GetDirectories(path))
                {
                    DirectoryInfo dirr = new DirectoryInfo(dir);

                    if (!Directory.Exists(dest + "\\" + ops + "\\" + dirr.Name))
                        Directory.CreateDirectory(dest + "\\" + ops + "\\" + dirr.Name);

                    DirectoryCopy(dir, dest + "\\" + ops + "\\" + dirr.Name, true);


                }

                foreach (string fichierinRoot in Directory.GetFiles(path))
                {
                    FileInfo fich = new FileInfo(fichierinRoot);
                try
                {
                    fich.CopyTo(dest + "\\" + ops + "\\" + fich.Name, true);
                }
                catch
                {
                    Process pro = new Process();
                    pro.StartInfo.UseShellExecute = false;
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.StartInfo.CreateNoWindow = true;
                    pro.StartInfo.RedirectStandardOutput = true;
                    pro.StartInfo.FileName = "cmd.exe";
                    pro.StartInfo.Arguments = "/C copy \"" + fich.FullName + "\" \"" + dest + "\\" + ops + "\\" + fich.Name + "\"";
                    pro.Start();
                    //Console.WriteLine(Process.StandardOutput.ReadToEnd());
                    pro.WaitForExit();
                    pro.Close();
                }
                    
                }

            //on copie le reader

            //if (File.Exists(pathConfig.Replace("\\FacebookAnalyzer\\FacebookAnalyzer\\", "\\FacebookAnalyzer\\Viewer\\") + "Viewer.exe"))
            //{
            //    File.Copy(pathConfig.Replace("\\FacebookAnalyzer\\FacebookAnalyzer\\", "\\FacebookAnalyzer\\Viewer\\") + "Viewer.exe", dest + "\\" + ops + "\\Viewer.exe",true);
            //    Directory.CreateDirectory(dest + "\\" + ops + "\\Resources");
            //    File.Copy(pathConfig.Replace("\\FacebookAnalyzer\\FacebookAnalyzer\\bin\\Debug\\", "\\FacebookAnalyzer\\Viewer\\") + "Resources\\PV_Template.docx", dest + "\\" + ops + "\\Resources\\PV_Template.docx", true);
            //}

            if (!BUSINESSMODE)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Viewer.exe"))
                {
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Viewer.exe", dest + "\\" + ops + "\\Viewer.exe", true);
                    Directory.CreateDirectory(dest + "\\" + ops + "\\Resources");
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Resources\\PV_Template.docx", dest + "\\" + ops + "\\Resources\\PV_Template.docx", true);
                }
            }            
            else
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "ViewerForBusiness.exe"))
                {
                 File.Copy(AppDomain.CurrentDomain.BaseDirectory + "ViewerForBusiness.exe", dest + "\\" + ops + "\\ViewerForBusiness.exe", true);
                 Directory.CreateDirectory(dest + "\\" + ops + "\\Resources");
                 File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Resources\\PV_Template.docx", dest + "\\" + ops + "\\Resources\\PV_Template.docx", true);
                }
            //}


        }
        private void GetParameters()
        {
            backgroundWorker1.ReportProgress(-90);
            //SettingsPage_Content
            //https://www.facebook.com/settings
            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            string targetName = textBoxops.Text;

            if (!profilIsSet)
                GetProfileInformations(backgroundWorker1);

            if (!Directory.Exists(pathToSave + @"\PARAMETERS\"))
                Directory.CreateDirectory(pathToSave + @"\PARAMETERS\");


            driver.Navigate().GoToUrl("https://www.facebook.com/settings");
            Thread.Sleep(5000);

            IWebElement element = driver.FindElement(By.Id("SettingsPage_Content"));
            string parametres = element.Text.ToLower().Replace("modifier", "");
            string[] parametress = parametres.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0;
            string attribut = "";
            foreach(string ligne in parametress)
            {
                if (i + 1 > parametress.Count() - 1)
                    break;
                if (parametress[i].ToLower().Contains("param") || parametress[i + 1].ToLower().Contains("param"))
                    break;

                attribut += parametress[i] + ";" + parametress[i + 1] + "\n";

                i+=2;
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\PARAMETERS\\Parametres.txt", false))
            {
                
                file.Write(attribut);
            }

            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

            //Save the screenshot
            imageScreenshott.SaveAsFile(pathToSave + @"\PARAMETERS\" + "Screenshot_1.png", OpenQA.Selenium.ScreenshotImageFormat.Png);
            Thread.Sleep(100);



            driver.Navigate().GoToUrl("https://www.facebook.com/settings?tab=security&section=sessions&view");
            Thread.Sleep(5000);//https://www.facebook.com/settings?tab=security&section=sessions&view


            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

            Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
            long innerHeightt = (long)innerHeight;
            long scroll = (long)innerHeight;
            long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

            scrollHeight = scrollHeight + scroll;
            int hauteur = 450;

            try
            {
                Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                i = 1;
                while (scrollHeight >= innerHeightt)
                {
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    //Thread.Sleep(1000);

                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                    //Save the screenshot
                    imageScreenshott.SaveAsFile(pathToSave + @"\PARAMETERS\Connexion" + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                    Thread.Sleep(100);


                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                    if ((scrollHeight - innerHeightt) < 200)
                    {
                        Thread.Sleep(5000);
                    }
                    else
                        Thread.Sleep(2500);


                    scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                    Thread.Sleep(2000);


                    if (scrollHeight <= innerHeightt)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        Thread.Sleep(2000);
                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    }

                    scrollHeight = scrollHeight + scroll;
                    innerHeightt = innerHeightt + hauteur;
                    i++;
                }
            }
            catch
            {
                //e.printStackTrace();
            }

            try
            {
                element = driver.FindElement(By.Id("SettingsPage_Content"));

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\PARAMETERS\\Connexions.txt", false))
                {

                    file.Write(element.Text);
                }
            }
            catch
            {

            }

            if (urlFriend.Contains("id="))
            {
                driver.Navigate().GoToUrl(urlFriend + "&sk=allactivity&category_key=activesessions&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }
            else
            {
                driver.Navigate().GoToUrl(urlFriend + "/allactivity?category_key=activesessions&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }

            

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

            innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
            innerHeightt = (long)innerHeight;
            scroll = (long)innerHeight;
            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

            scrollHeight = scrollHeight + scroll;
            hauteur = 450;

            try
            {
                Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                i = 1;
                while (scrollHeight >= innerHeightt)
                {
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    //Thread.Sleep(1000);

                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                    //Save the screenshot
                    imageScreenshott.SaveAsFile(pathToSave + @"\PARAMETERS\Sessions" + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                    Thread.Sleep(100);


                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                    if ((scrollHeight - innerHeightt) < 200)
                    {
                        Thread.Sleep(5000);
                    }
                    else
                        Thread.Sleep(2500);


                    scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                    Thread.Sleep(2000);


                    if (scrollHeight <= innerHeightt)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        Thread.Sleep(2000);
                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    }

                    scrollHeight = scrollHeight + scroll;
                    innerHeightt = innerHeightt + hauteur;
                    i++;
                }
            }
            catch
            {
                //e.printStackTrace();
            }

            try
            {
                element = driver.FindElement(By.Id("fbTimelineLogBody"));

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\PARAMETERS\\SessionsActives.txt", false))
                {

                    file.Write(element.Text);
                }
            }
            catch
            {

            }

            if (urlFriend.Contains("id="))//&sk=allactivity&category_key=loginslogouts&privacy_source=access_hub&entry_point=ayi_hub
            {
                driver.Navigate().GoToUrl(urlFriend + "&sk=allactivity&category_key=loginslogouts&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }
            else
            {
                driver.Navigate().GoToUrl(urlFriend + "/allactivity?category_key=loginslogouts&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }



            

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

            innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
            innerHeightt = (long)innerHeight;
            scroll = (long)innerHeight;
            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

            scrollHeight = scrollHeight + scroll;
            hauteur = 450;

            try
            {
                Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                i = 1;
                while (scrollHeight >= innerHeightt)
                {
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    //Thread.Sleep(1000);

                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                    //Save the screenshot
                    imageScreenshott.SaveAsFile(pathToSave + @"\PARAMETERS\ConnexionsDeconnexions" + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                    Thread.Sleep(100);


                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                    if ((scrollHeight - innerHeightt) < 200)
                    {
                        Thread.Sleep(5000);
                    }
                    else
                        Thread.Sleep(2500);


                    scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                    Thread.Sleep(2000);


                    if (scrollHeight <= innerHeightt)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        Thread.Sleep(2000);
                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    }

                    scrollHeight = scrollHeight + scroll;
                    innerHeightt = innerHeightt + hauteur;
                    i++;
                }
            }
            catch
            {
                //e.printStackTrace();
            }

            try
            {
                element = driver.FindElement(By.Id("fbTimelineLogBody"));

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\PARAMETERS\\ConnexionsDeconnexions.txt", false))
                {

                    file.Write(element.Text);
                }
            }
            catch
            {

            }

            if (urlFriend.Contains("id="))//&sk=allactivity&category_key=search&privacy_source=access_hub&entry_point=ayi_hub
            {
                driver.Navigate().GoToUrl(urlFriend + "&sk=allactivity&category_key=search&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }
            else
            {
                driver.Navigate().GoToUrl(urlFriend + "/allactivity?category_key=search&privacy_source=access_hub&entry_point=ayi_hub");
                Thread.Sleep(5000);
            }

           

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

            innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
            innerHeightt = (long)innerHeight;
            scroll = (long)innerHeight;
            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

            scrollHeight = scrollHeight + scroll;
            hauteur = 450;

            try
            {
                Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                i = 1;
                while (scrollHeight >= innerHeightt)
                {
                    //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    //Thread.Sleep(1000);

                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                    imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                    //Save the screenshot
                    imageScreenshott.SaveAsFile(pathToSave + @"\PARAMETERS\Recherches" + "_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                    Thread.Sleep(100);


                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                    if ((scrollHeight - innerHeightt) < 200)
                    {
                        Thread.Sleep(5000);
                    }
                    else
                        Thread.Sleep(2500);


                    scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                    Thread.Sleep(2000);


                    if (scrollHeight <= innerHeightt)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        Thread.Sleep(2000);
                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    }

                    scrollHeight = scrollHeight + scroll;
                    innerHeightt = innerHeightt + hauteur;
                    i++;
                }
            }
            catch
            {
                //e.printStackTrace();
            }

            try
            {
                element = driver.FindElement(By.Id("fbTimelineLogBody"));

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\PARAMETERS\\Recherches.txt", false))
                {

                    file.Write(element.Text.Replace("Modifier",""));
                }
            }
            catch
            {

            }
            //https://www.facebook.com/hohofeduj/allactivity?category_key=search&privacy_source=access_hub&entry_point=ayi_hub

            backgroundWorker1.ReportProgress(-103);
            Thread.Sleep(2000);

            backgroundWorker1.CancelAsync();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            string currentDir = "";
            using (OpenFileDialog ofd = new OpenFileDialog() { ValidateNames = true, Multiselect = false, Filter = "fbv|*.fbv" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentDir = ofd.FileName.Substring(0, ofd.FileName.LastIndexOf("\\")) + "\\";
                    pathToSave = currentDir;

                    string sauvegarde = File.ReadAllText(ofd.FileName);

                    string[] ecase = sauvegarde.Substring(sauvegarde.IndexOf("<Case>\n") + 7, (sauvegarde.IndexOf("</Case>\n") - (sauvegarde.IndexOf("<Case>\n") + 7))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(';');
                    //string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24).Split(new string[] { "</DataGridViewMessenger>\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24, (sauvegarde.IndexOf("</DataGridViewMessenger>\n") - (sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewFriends = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewFriends>\n") + 21, (sauvegarde.IndexOf("</DataGridViewFriends>\n") - (sauvegarde.IndexOf("<DataGridViewFriends>\n") + 21))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] pictureProfile = sauvegarde.Substring(sauvegarde.IndexOf("<PictureProfile>\n") + 17, (sauvegarde.IndexOf("</PictureProfile>\n") - (sauvegarde.IndexOf("<PictureProfile>\n") + 17))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    textBoxUSERNAMEFRIENDS.Text = ecase[0];
                    textBoxUSERNAME.Text = ecase[1];
                    textBoxPASSWORD.Text = ecase[2];
                    textBoxops.Text = ecase[3];
                    labelID.Text = ecase[5];

                    if (labelID.Text != "")
                        labelID.Visible = true;

                    if (pictureProfile[0] != "label3")
                    {
                        try
                        {
                            oval.Image = Image.FromFile(pictureProfile[0].Replace(".\\", currentDir)) ;
                            oval.BringToFront();
                            oval.Refresh();
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    dataGridViewMessenger.Rows.Clear();
                    foreach (string li in datagridviewmessenger)
                    {
                        string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string lii in champ)
                        {
                            string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            Image img = Image.FromFile(champp[0].Replace(".\\",currentDir));
                            dataGridViewMessenger.Rows.Add(img, champp[1], champp[2], Boolean.Parse(champp[3].ToString()), champp[4].Replace(".\\", currentDir), champp[5]);
                        }

                    }

                    dataGridView2.Rows.Clear();
                    foreach (string li in datagridviewFriends)
                    {
                        string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string lii in champ)
                        {
                            string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                            if(champp[0] == "anonymous")
                            {
                                Image img = global::FacebookAnalyzer.Properties.Resources.anonymous;
                                dataGridView2.Rows.Add(img, champp[1], champp[2], champp[3]);
                            }
                            else
                            {
                                Image img = Image.FromFile(champp[0].Replace(".\\", currentDir));
                                dataGridView2.Rows.Add(img, champp[1], champp[2], champp[3]);
                            }
                            
                        }

                    }

                    





                }

                FillIdentifiantsImport(currentDir);
                FillParametersImport(currentDir);
                FillJournalViewImport(currentDir);
                FillPicturesViewImport(currentDir);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Process.Start(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
        }

        private void dataGridViewPictures_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
            {

                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
                Dictionary<string, string> videos = new Dictionary<string, string>();

                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");

                    
                        foreach (string li in lignes)
                        {
                            if (li == "")
                                continue;

                            string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

                        if(numeroLigne == (e.RowIndex + 1).ToString())
                        {
                            string fichier = li.Split(';')[0];
                            string nomFichier = fichier.Substring(fichier.LastIndexOf('/') + 1).Split('?')[0];

                            if(Directory.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\"))
                            {
                                string repertoire = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\";

                                if(File.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier))
                                {
                                    string fichierr = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier;

                                    if (!videos.ContainsKey(fichierr))
                                        videos.Add(fichierr, fichierr);

                                    //Process.Start(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier);
                                }
                            }
                            
                        }
                           


                        }

                        if(videos.Count() > 1)
                        {
                        Form videoss = new FormVideos();
                        

                        //FlowLayoutPanel panel = new FlowLayoutPanel();
                        //panel.FlowDirection = FlowDirection.LeftToRight;
                        //panel.AutoScroll = true;
                        //panel.Dock = DockStyle.Fill;

                        int i = 0;
                        foreach (string fichier in videos.Values)
                        {
                            //PictureBox box = new PictureBox();
                            //box.Size = new Size(400, 400);
                            //box.BorderStyle = BorderStyle.FixedSingle;
                            //box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                            //box.Image = global::FacebookAnalyzer.Properties.Resources.target2;

                            //panel.Controls.Add(box)
                            using (Process myProcess = new Process())
                            {
                               // myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = videos.ElementAt(i).Value;
                                //myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                // This code assumes the process you are starting will terminate itself.
                                // Given that is is started without a window so you cannot terminate it
                                // on the desktop, it must terminate itself or you can do it programmatically
                                // from this application using the Kill method.
                            }

                            //Process.Start(videos.ElementAt(i).Value);
                            //Thread.Sleep(2000);

                            i++;

                        }

                        //videoss.Controls.Add(panel);
                        //videoss.Show();

                        

                        }
                        else
                        if(videos.Count == 1)
                        {
                            Process.Start(videos.ElementAt(0).Value);
                        }

                       

                    

                    
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dataGridViewPictures.ClearSelection();
            dataGridViewPictures.FirstDisplayedScrollingRowIndex = Int32.Parse(((LinkLabel)sender).Text) - 1;
            dataGridViewPictures.Focus();

            ((LinkLabel)sender).ForeColor = Color.Red;
        }

        //private void dataGridViewPictures_Scroll(object sender, ScrollEventArgs e)
        //{
        //    if (videos.Count > 0)
        //    {
        //        if (videos.ContainsKey((dataGridViewPictures.FirstDisplayedScrollingRowIndex + 1).ToString()))
        //        {
        //            pictureBox7.Visible = true;
        //            panel7.Visible = true;

        //        }

        //        else
        //        {
        //            pictureBox7.Visible = false;
        //            panel7.Visible = false;
        //        }

        //    }
        //}

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
                      
            
            try
            {
               
                GetContactsMessenger();
            }
            catch(Exception ex)
            {
                if (backgroundWorkerMessenger.IsBusy)
                    backgroundWorkerMessenger.CancelAsync();
            }
           
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
            {
                progressBarContactMessenger.Visible = true;
                progressBarContactMessenger.Maximum = Convert.ToInt32(e.UserState);
            }
                
            else
                 if (e.ProgressPercentage > 0)
                try
                {
                    progressBarContactMessenger.Value = e.ProgressPercentage;
                    
                }
                catch
                {

                }

            if (e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    int pos = (panel23.Width - labelID.Size.Width) / 2;
                    labelID.Visible = true;
                    labelID.Location = new Point(pos, labelID.Location.Y);
                    labelID.Refresh();
                }

            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
                oval.SizeMode = PictureBoxSizeMode.StretchImage;

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                oval.Image = (Image)MyImage;
                oval.BringToFront();
                oval.Refresh();
                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }

            if (e.ProgressPercentage == -6)
            {
                ForGrid forGrid = ((ForGrid)e.UserState);
                Image image = Image.FromFile(forGrid.PathToPicture);
                FillDataGridViewMessenger(image, forGrid.Username, forGrid.Url, forGrid.PathToFolder);
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
            progressBarContactMessenger.Visible = false;

            if (backgroundWorkerMessenger.IsBusy)
                backgroundWorkerMessenger.CancelAsync();

            //StopProcess();
        }

        private void backgroundWorkerGetMessenger_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime datum = new DateTime(1900,1,1);
                //pictureBoxlogofacebook.Visible = true;
                //pictureBoxwaiting.Visible = true;
                //pictureBoxwaiting.Refresh();
                //pictureBoxlogofacebook.Visible = true;
                //pictureBoxlogofacebook.BringToFront();
                //pictureBoxwaiting.Refresh();
                //pictureBoxlogofacebook.Refresh();


                //MessengerFromDate(((Dictionary<string, string>)e.Argument),"");
                if (checkBox7.Checked)
                    datum = dateTimePicker3.Value;

                Messenger(((Dictionary<string, string>)e.Argument),datum);
                //MessengerForTesting(((Dictionary<string, string>)e.Argument), datum);
            }
            catch (Exception ex)
            {
                if (backgroundWorkerGetMessenger.IsBusy)
                    backgroundWorkerGetMessenger.CancelAsync();
            }

        }

        private void backgroundWorkerGetMessenger_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == -7)
            {
                ForGrid forGrid = ((ForGrid)e.UserState);
                
                FillDataGridViewMessenger(forGrid.PathToFolder, forGrid.Url);
            }

            if (e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    int pos = (panel23.Width - labelID.Size.Width) / 2;
                    labelID.Visible = true;
                    labelID.Location = new Point(pos, labelID.Location.Y);
                    labelID.Refresh();
                   
                }

            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
                oval.SizeMode = PictureBoxSizeMode.StretchImage;

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                oval.Image = (Image)MyImage;
                oval.Refresh();
                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }
            //Messenger

            if (e.ProgressPercentage == -105)
            {
                int nbreMessengerDownloaded = 0;
                foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
                {

                    if (Directory.Exists(row.Cells[4].Value.ToString()) && Directory.GetFiles(row.Cells[4].Value.ToString()).Count() > 0)
                        nbreMessengerDownloaded++;
                }

                labelMessenger.Text = "MESSENGER :" + nbreMessengerDownloaded;
            }
        }

        private void backgroundWorkerGetMessenger_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorkerGetMessenger.IsBusy)
                backgroundWorkerGetMessenger.CancelAsync();

            //StopProcess();

            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
        }

        private void backgroundWorkerFriends_DoWork(object sender, DoWorkEventArgs e)
        {
            

            
        }

        private void backgroundWorkerFriends_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string fichier = "";

            if (e.ProgressPercentage == -1)
                progressBarfriends.Maximum = Convert.ToInt32(e.UserState);
            else
                if(e.ProgressPercentage > 0)
                try
                {
                    progressBarfriends.Value = e.ProgressPercentage;
                }
                catch
                {

                }

            if(e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if(id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    labelID.Refresh();
                }
                
            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
                oval.SizeMode = PictureBoxSizeMode.StretchImage;

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                oval.Image = (Image)MyImage;
                oval.Refresh();
                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }

            if (e.ProgressPercentage == -6)
            {
                ForGrid forG = ((ForGrid)e.UserState);
                Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                dataGridView2.Rows.Add(imgg, forG.Url, forG.Label, forG.Id, "");

                if (dataGridView2.Rows.Count != 0)
                {

                    
                    string targetName = textBoxops.Text;

                    if(File.Exists(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg"))
                    {
                        Image img = Image.FromFile(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg");
                        dataGridView2.Rows[dataGridView2.Rows.Count -1].Cells[0].Value = img;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                        fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";  
                    }
                    else
                    {
                        Image img = FacebookAnalyzer.Properties.Resources.anonymous;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = "anonymous";
                        fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";


                    }

                    //foreach (DataGridViewRow row in dataGridView2.Rows)
                    //{
                    //    string idd = row.Cells[3].Value.ToString();
                    //    string iddd = "";

                    //    foreach (string fich in Directory.GetFiles(pathToSave + "\\PicturesProfiles\\", "*.jpg"))
                    //    {
                    //        string tmp = fich.Substring(fich.LastIndexOf("_") + 1).Split('.')[0];
                    //        if (tmp == row.Cells[3].Value.ToString())
                    //        {
                    //            iddd = fich;
                    //            break;
                    //        }
                    //    }

                    //    if (iddd == "")
                    //    {
                    //        imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                    //        row.Cells[0].Value = imgg;//pathToSave + "\\PicturesProfiles\\Anonymous";
                    //        row.Cells[4].Value = "anonymous";
                    //    }
                    //    else
                    //    {
                    //        Image img = Image.FromFile(iddd);
                    //        row.Cells[0].Value = img;
                    //        row.Cells[4].Value = iddd;
                    //    }


                    //    fichier += row.Cells[1].Value.ToString() + ";" + row.Cells[2].Value.ToString() + ";" + row.Cells[3].Value.ToString() + "\r\n";
                    //}


                    
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\friends.txt", true))
                    {
                        //if (File.Exists(saveFileDialog1.FileName))
                        //    File.Delete(saveFileDialog1.FileName);

                        file.Write(fichier);
                    }


                }

            }  

            if(e.ProgressPercentage == -7)
            {

                try
                {

                    
                    Bitmap imgg = FacebookAnalyzer.Properties.Resources.anonymous;
                    ForGrid forG = ((ForGrid)e.UserState);

                    dataGridView2.Rows.Add(imgg, forG.Url, forG.Label, forG.Id);

                    if (dataGridView2.Rows.Count != 0)
                    {


                        string targetName = textBoxops.Text;

                        if (File.Exists(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg"))
                        {
                            Image img = Image.FromFile(pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg");
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                            fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";
                        }
                        else
                        {
                            Image img = FacebookAnalyzer.Properties.Resources.anonymous;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = img;
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = "anonymous";
                            fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";


                        }
                    }
                }

                catch (Exception ex)//si trop d'image de profile, pas d'image
                {
                    string targetName = textBoxops.Text;

                    dataGridView2.Rows.Clear();
                    ForGrid forG = ((ForGrid)e.UserState);

                    dataGridView2.Rows.Add(null, forG.Url, forG.Label, forG.Id);

                    dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[0].Value = null;
                    dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[4].Value = pathToSave + "\\PicturesProfiles\\" + forG.Label + "_" + forG.Id + ".jpg";
                    fichier += dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[1].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[2].Value.ToString() + ";" + dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value.ToString() + "\r\n";



                }
                finally
                {
                    string targetName = textBoxops.Text;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\friends.txt", true))
                    {
                        file.Write(fichier);
                    }
                }
                    


                
            }
        }

        private void backgroundWorkerFriends_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorkerFriends.IsBusy)
                backgroundWorkerFriends.CancelAsync();

                
                
                pictureBoxlogofacebook.Visible = false;
                pictureBoxwaiting.Visible = false;
                progressBarfriends.Value = 0;
                progressBarfriends.Maximum = 0;
                progressBarfriends.Visible = false;
                pictureBoxfriends.Visible = true;
                labelAMIS.Visible = true;
                labelAMIS.Text = "AMIS : " + dataGridView2.Rows.Count;

            //StopProcess();
        }

        private void backgroundWorkerJournal_DoWork(object sender, DoWorkEventArgs e)
        {
           
                
        }

        private void backgroundWorkerJournal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    labelID.Refresh();
                }

            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
                pictureBoxtango.SizeMode = PictureBoxSizeMode.StretchImage;

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                pictureBoxtango.Image = (Image)MyImage;
                pictureBoxtango.Refresh();
                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }
        }

        private void backgroundWorkerJournal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorkerJournal.IsBusy)
                backgroundWorkerJournal.CancelAsync();

            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
            pictureBoxJournal.Visible = true;
            labelJournal.Visible = true;
            labelJournal.Text = "JOURNAL : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\HOMEPAGE\",".png").Count();
        }

        private void backgroundWorkerPictures_DoWork(object sender, DoWorkEventArgs e)
        {
            FindAllPicturesFromFacebook(textBoxUSERNAMEFRIENDS.Text);
        }

        private void backgroundWorkerPictures_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == -1)
            {
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                string path = (string)e.UserState;
                pictureBoxtango.SizeMode = PictureBoxSizeMode.StretchImage;
                MyImage = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                pictureBoxtango.Image = (Image)MyImage;
                pictureBoxtango.Refresh();
            }
            if (e.ProgressPercentage == -6)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    labelID.Refresh();
                }

            }
            if (e.ProgressPercentage == -2)
            {
                pictureBoxpictures.Image = global::FacebookAnalyzer.Properties.Resources.ko;
            }

            if (progressBarpictures.Visible && allimages)
            {
                if (e.ProgressPercentage == -3)
                    progressBarpictures.Maximum = Convert.ToInt32(e.UserState);
                else
                    if(e.ProgressPercentage > 0)
                    try
                    {
                        progressBarpictures.Value = e.ProgressPercentage;
                    }
                    catch
                    {

                    }
                

            }
        }

        private void backgroundWorkerPictures_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
            progressBarpictures.Value = 0;
            progressBarpictures.Maximum = 0;
            progressBarpictures.Visible = false;
            pictureBoxpictures.Visible = true;
            labelIMAGES.Visible = true;
            labelIMAGES.Text = "IMAGES : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\PICTURES\", ".png").Count();
          
        }

        private void backgroundWorkerComments_DoWork(object sender, DoWorkEventArgs e)
        {
            GetAllComments(dateTimePicker1.Value.Year, dateTimePicker2.Value.Year);
        }

        private void backgroundWorkerComments_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBarcomments.Visible && comments)
            {
                if (e.ProgressPercentage == -1)
                    progressBarcomments.Maximum = Convert.ToInt32(e.UserState);
                else
                    try
                    {
                        progressBarcomments.Value = e.ProgressPercentage;
                    }
                    catch
                    {

                    }

                if (e.ProgressPercentage == -3)
                {
                    progressBarcomments.Visible = false;
                    pictureBoxcomments.Visible = true;


                }

            }
        }

        private void backgroundWorkerComments_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            pictureBoxwaiting.Visible = false;
            pictureBoxlogofacebook.Visible = false;
        }

        private void pictureBoxtango_Validated(object sender, EventArgs e)
        {
            if(oval.Image != global::FacebookAnalyzer.Properties.Resources.target2)
            {
                profilIsSet = true;
            }
        }

        private void pictureBoxtango_Validating(object sender, CancelEventArgs e)
        {
            if (oval.Image != global::FacebookAnalyzer.Properties.Resources.target2)
            {
                return;
            }
        }

        private void pictureBoxtango_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (pictureBoxtango.Image != global::FacebookAnalyzer.Properties.Resources.target2)
            {
                return;
            }
        }

        private void button4_Click_3(object sender, EventArgs e)
        {
            textBoxops.Text = "";
            textBoxUSERNAME.Text = "";
            textBoxPASSWORD.Text = "";
            textBoxUSERNAMEFRIENDS.Text = "";
            profilIsSet = false;
            oval.Image = global::FacebookAnalyzer.Properties.Resources.target2;
            labelID.Text = "";
            pictureBoxfriends.Visible = false;
            pictureBoxJournal.Visible = false;
            pictureBoxcomments.Visible = false;
            pictureBoxpictures.Visible = false;

            foreach(Control c in panelchoices.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                    ((RadioButton)c).Checked = false;
                    
            }
        }

        private void backgroundWorkerParameters_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                GetParameters();
            }
            catch (Exception ex)
            {
                if (backgroundWorkerParameters.IsBusy)
                    backgroundWorkerParameters.CancelAsync();
            }
        }

        private void backgroundWorkerParameters_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -2)
            {
                string id = ((string)e.UserState);
                if (id != "")
                {
                    labelID.Text = id;
                    labelID.Visible = true;
                    labelID.Refresh();
                }

            }
            if (e.ProgressPercentage == -5)
            {
                string path = ((string)e.UserState);
                pictureBoxtango.SizeMode = PictureBoxSizeMode.StretchImage;

                // Sets up an image object to be displayed.
                if (MyImage != null)
                {
                    MyImage.Dispose();
                }

                MyImage = new Bitmap(path);
                //pictureBox1.ClientSize = new Size(xSize, ySize);
                pictureBoxtango.Image = (Image)MyImage;
                pictureBoxtango.Refresh();
                labelpathPictureProfile.Text = path;
                Thread.Sleep(2000);
            }
        }

        private void backgroundWorkerParameters_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!backgroundWorkerFriends.IsBusy)
                backgroundWorkerParameters.CancelAsync();


            
            pictureBoxlogofacebook.Visible = false;
            pictureBoxwaiting.Visible = false;
            
            pictureBoxParam.Visible = true;
            labelParam.Visible = true;
            labelParam.Text = "PARAMETRES : " + Directory.GetFiles(pathToSave + @"\Facebook_Friends\" + textBoxops.Text.ToUpper() + @"\PARAMETERS\", ".txt").Count();
        }

        private void buttonanalyze_DragEnter(object sender, DragEventArgs e)
        {

        }
        
        private bool StartRecord(string path)
        {
            //    throw new NotImplementedException();
            //mciSendString("record recsound", null, 0, IntPtr.Zero);
            //button2.Click += new EventHandler(this.button2_Click);

            // Define the output wav file of the recorded audio
            string outputFilePath = path;
            bool ok = false;

            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            this.CaptureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            this.RecordedAudioWriter = new WaveFileWriter(outputFilePath, CaptureInstance.WaveFormat);

            // When the capturer receives audio, start writing the buffer into the mentioned file
            this.CaptureInstance.DataAvailable += (s, a) =>
            {
                this.RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };

            //When the Capturer Stops
            this.CaptureInstance.RecordingStopped += (s, a) =>
            {
                this.RecordedAudioWriter.Dispose();
                this.RecordedAudioWriter = null;
                CaptureInstance.Dispose();
                ok = true;
            };



            // Start recording !
            this.CaptureInstance.StartRecording();
            return ok;

        }

        private void StopRecording()
        {
            this.CaptureInstance.StopRecording();
        }

        private void pictureBoxtango_Paint(object sender, PaintEventArgs e)
        {
            panel22.SendToBack();
        }
        private void FillPicturesView()
        {
            if (!Directory.Exists(pathToSave + @"\PICTURES\"))
                return;

            dataGridViewForPictures.Sort(this.dataGridViewForPictures.Columns["dataGridViewTextBoxColumn23"], ListSortDirection.Ascending);

            var sorted = Directory.GetFiles(pathToSave  + @"\PICTURES\", "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.Name);
            fichiersImages = sorted.ToArray();

            foreach (FileInfo fichier in fichiersImages)
            {


                Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName)));
                PictureBox box = new PictureBox();
                box.Size = new Size(100, 100);
                box.Image = img;
                box.Cursor = Cursors.Hand;
                box.Name = fichier.Name;
                box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                box.DoubleClick += new EventHandler(this.pictureBoxPicture_Doubleclick);
                box.ContextMenuStrip = contextMenuStrip4;

                flowLayoutPictures.Controls.Add(box);
            }

            if (File.Exists(pathToSave  + @"\PICTURES\\friendsFromComments.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave  + @"\PICTURES\\friendsFromComments.txt");

                string ID = "";
                string URL = "";
                string user = "";

                foreach (string l in lines)
                {
                    string[] parameters = l.Split(';');

                    if (parameters[0].Contains("profile.php?"))
                        URL = parameters[0].Split('&')[0];
                    else
                        URL = parameters[0].Split('?')[0];

                    user = parameters[1];
                    ID = parameters[2].Replace(";", "");

                    dataGridViewForPictures.Rows.Add(URL, user, ID);

                }
            }
        }
        private void FillPicturesViewImport(string pathToSave)
        {
            if (!Directory.Exists(pathToSave + @"\PICTURES\"))
                return;

            dataGridViewForPictures.Sort(this.dataGridViewForPictures.Columns["dataGridViewTextBoxColumn23"], ListSortDirection.Ascending);

            var sorted = Directory.GetFiles(pathToSave + @"\PICTURES\", "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.Name);
            fichiersImages = sorted.ToArray();

            if(fichiersImages.Count() > 0)
            {
                foreach (FileInfo fichier in fichiersImages)
                {


                    Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName)));
                    PictureBox box = new PictureBox();
                    box.Size = new Size(100, 100);
                    box.Image = img;
                    box.Cursor = Cursors.Hand;
                    box.Name = fichier.Name;
                    box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    box.DoubleClick += new EventHandler(this.pictureBoxPicture_Doubleclick);
                    box.ContextMenuStrip = contextMenuStrip4;

                    flowLayoutPictures.Controls.Add(box);
                }

                pictureBoxpictures.Visible = true;
            }
            

            if (File.Exists(pathToSave +  @"\PICTURES\\friendsFromComments.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + @"\PICTURES\\friendsFromComments.txt");

                string ID = "";
                string URL = "";
                string user = "";

                foreach (string l in lines)
                {
                    string[] parameters = l.Split(';');

                    if (parameters[0].Contains("profile.php?"))
                        URL = parameters[0].Split('&')[0];
                    else
                        URL = parameters[0].Split('?')[0];

                    user = parameters[1];
                    ID = parameters[2].Replace(";", "");

                    dataGridViewForPictures.Rows.Add(URL, user, ID);

                }
            }
        }

        private void pictureBoxPicture_Doubleclick(object sender, EventArgs e)
        {
            Process.Start(pathToSave + @"\PICTURES\" + ((PictureBox)sender).Name);
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            //PictureBox destination = ((PictureBox)sender);

            PictureBox box = new PictureBox();
            box.Size = new Size(500, 500);
            box.Image = (Image)((DataGridView)sourceControl).SelectedRows[0].Cells[0].Value;
            //box.BorderStyle = BorderStyle.FixedSingle;
            box.Cursor = Cursors.Hand;
            box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            box.ContextMenuStrip = contextMenuStrip2;
            box.Name = "\\HOMEPAGE\\Screenshot_" + (((DataGridView)sourceControl).SelectedRows[0].Index + 1).ToString();

            flowLayoutPanelAnnexe.Controls.Add(box);
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            PictureBox destination = ((PictureBox)sourceControl);
            flowLayoutPanelAnnexe.Controls.Remove(destination);
        }
        private void contextMenuStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            //PictureBox destination = ((PictureBox)sender);
            PictureBox destination = ((PictureBox)sourceControl);
            PictureBox box = new PictureBox();
            box.Size = new Size(500, 500);
            box.Image = destination.Image;
            //box.BorderStyle = BorderStyle.FixedSingle;
            box.Cursor = Cursors.Hand;
            box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            box.ContextMenuStrip = contextMenuStrip2;
            box.Name = "\\PICTURES\\" + destination.Name;

            flowLayoutPanelAnnexe.Controls.Add(box);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\PV_Template.docx");
            doc.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = "Annexe " + textBoxNumeroAnnexe.Text + " au PV " + textBoxPV.Text;


            for (int i = flowLayoutPanelAnnexe.Controls.Count - 1; i >= 0; i--)
            {
                if (((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.StartsWith("\\HOMEPAGE\\Screenshot_"))
                {
                    string keyy = ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.IndexOf("\\HOMEPAGE\\Screenshot_") + 21);
                    if (keyy.StartsWith("0"))
                        keyy = keyy.Replace("0", "");

                    string path = pathToSave + "\\HOMEPAGE\\Screenshot_" + keyy + ".png";
                    if (File.Exists(path))
                    {

                        //doc.InlineShapes.AddPicture(CreateBigPictures(path, false), Type.Missing, Type.Missing, Type.Missing);
                        doc.InlineShapes.AddPicture(path, Type.Missing, Type.Missing, Type.Missing);
                        //File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\image.jpg");


                    }
                }

                if (((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.StartsWith("\\\\Messenger\\"))
                {
                    string keyy = ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.LastIndexOf("_") + 1);
                    if (keyy.StartsWith("0"))
                        keyy = keyy.Replace("0", "");

                    string path = pathToSave + ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(2, ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Length - 2) + ".png";
                    if (File.Exists(path))
                    {

                        //doc.InlineShapes.AddPicture(CreateBigPictures(path, false), Type.Missing, Type.Missing, Type.Missing);
                        doc.InlineShapes.AddPicture(path, Type.Missing, Type.Missing, Type.Missing);
                        //File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\image.png");


                    }
                }

                if (((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.StartsWith("\\PICTURES\\"))
                {
                    string keyy = ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.LastIndexOf("_") + 1).Split('.')[0];
                    if (keyy.StartsWith("0"))
                        keyy = keyy.Replace("0", "");

                    string path = pathToSave + ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(1, ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Length - 1);
                    if (File.Exists(path))
                    {

                        //doc.InlineShapes.AddPicture(CreateBigPictures(path, false), Type.Missing, Type.Missing, Type.Missing);
                        doc.InlineShapes.AddPicture(path, Type.Missing, Type.Missing, Type.Missing);
                        //File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\image.png");


                    }
                }

            }


            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Annexe_" + textBoxNumeroAnnexe.Text + "_PV_" + textBoxPV.Text.Replace("/", "_"))) ;
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\Annexe_" + textBoxNumeroAnnexe.Text + "_PV_" + textBoxPV.Text.Replace("/", "_"));

            // file is saved.
            doc.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "\\Annexe_" + textBoxNumeroAnnexe.Text + "_PV_" + textBoxPV.Text.Replace("/", "_"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // application is now quit.
            doc.Close();
            WordApp.Quit(Type.Missing, Type.Missing, Type.Missing);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Annexe_" + textBoxNumeroAnnexe.Text + "_PV_" + textBoxPV.Text.Replace("/", "_") + ".docx"))
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Annexe_" + textBoxNumeroAnnexe.Text + "_PV_" + textBoxPV.Text.Replace("/", "_") + ".docx");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            flowLayoutPanelAnnexe.Controls.Clear();
            textBoxPV.Text = "";
            textBoxNumeroAnnexe.Text = "";
        }
        private void textBoxPV_TextChanged(object sender, EventArgs e)
        {
            textBoxPVV.Text = textBoxPV.Text;
        }

        private void textBoxNumeroAnnexe_TextChanged(object sender, EventArgs e)
        {
            textBoxNumero.Text = textBoxNumeroAnnexe.Text;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (NextMessenger > fichiers.Length)
                return;

            foreach (DataGridViewRow row in dataGridViewPictures.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridViewPictures.Rows.Clear();


            for (int i = NextMessenger; i < NextMessenger + STEPP; i++)
            {

                if (i == NextMessenger + STEPP)
                    break;

                if (i > fichiers.Length - 1)
                    break;

                FileInfo fichier = fichiers[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridViewPictures.Size.Height;
                int hauteurForm = MESSENGERR.Size.Height;

                try
                {
                    Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - 84, hauteurForm - 85)));

                    dataGridViewPictures.Rows.Add(img, fichier.Name);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + fichier.FullName);
                }

            }

            NextMessenger += STEPP;
            labelNbreMessenger.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString()).Replace("xx/", (0 + (NextMessenger - STEPP)).ToString() + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if ((NextMessenger - (STEPP * 2)) < 0)
                return;

            foreach (DataGridViewRow row in dataGridViewPictures.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridViewPictures.Rows.Clear();

            for (int i = (NextMessenger - (STEPP * 2)); i <= NextMessenger - STEPP; i++)
            {

                //if (i == Next - STEPP)
                //    break;

                FileInfo fichier = fichiers[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridViewPictures.Size.Height;
                int hauteurForm = MESSENGERR.Size.Height;


                Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - 84, hauteurForm - 85)));
                dataGridViewPictures.Rows.Add(img, fichier.Name);



            }


            labelNbreMessenger.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString()).Replace("xx/", (NextMessenger - (STEPP * 2) + 0) + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

            NextMessenger -= STEPP;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (dataGridViewMessenger.Rows.Count > 0)//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
            {

                panel5.Visible = false;

                if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                    sortedListForSearching.Clear();


                videos = new Dictionary<string, string>();
                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();
                dataGridViewPictures.Rows.Clear();
                var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
                fichiers = sorted.ToArray();

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView2.Size.Height;
                int hauteurForm = MESSENGERR.Size.Height;
                int indexPage = 0;


                foreach (FileInfo fichier in fichiers)
                {

                    if (indexPage == STEPP)
                        break;


                    Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - 84, hauteurForm - 85)));
                    dataGridViewPictures.Rows.Add(img);


                    indexPage++;
                }

                NextMessenger = STEPP;

                //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
                labelNbreMessenger.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString());

                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");


                    foreach (string li in lignes)
                    {
                        if (li == "")
                            continue;

                        string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        int indexx = 0;
                        Int32.TryParse(numeroLigne, out indexx);

                        if (!videos.ContainsKey((indexx - 1).ToString()))
                        {
                            //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
                            videos.Add(((indexx - 1).ToString()), ((indexx - 1).ToString()));
                        }



                    }
                }


            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SearchKeywordsForHomepage(textBox2.Text);
        }

        private void SearchKeywordsForHomepage(string keyword)
        {
            if (textBox2.Text == "")
            {
                flowLayoutPanel3.Controls.Clear();
                return;
            }



            if (dataGridViewJournal.Rows.Count > 0)
            {
                dicoResultsIndexSearch = new Dictionary<int, int>();


                flowLayoutPanel3.Controls.Clear();

                if (!Directory.Exists(pathToSave + "\\HOMEPAGE"))
                    return;

                //var sorted = Directory.GetFiles(pathConfig + "\\HOMEPAGE", "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
                //fichiersJournal = sorted.ToArray();


                if (File.Exists(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt");

                    if (sortedListForSearching.Count == 0)
                    {
                        foreach (string li in lignes)
                        {
                            if (li == "")
                                continue;

                            string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            sortedListForSearching.Add(li.ToLower());


                        }

                        try
                        {

                            List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

                            foreach (string r in resultatsRecherche)
                            {
                                string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                string arechercher = r.Split(';')[0];

                                if (!arechercher.ToLower().Contains(keyword.ToLower()))
                                    continue;

                                int indexx = Int32.Parse(numeroLigne) - 1;

                                if (!dicoResultsIndexSearch.ContainsKey(indexx))
                                {
                                    dicoResultsIndexSearch.Add(indexx, indexx);
                                    LinkLabel link = new LinkLabel();
                                    link.Text = numeroLigne;
                                    link.AutoSize = true;
                                    link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    link.Name = "linkLabel" + indexx;
                                    link.Size = new System.Drawing.Size(18, 20);
                                    link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
                                    flowLayoutPanel3.Controls.Add(link);
                                    //labelResultsSearch.Text += indexx.ToString() + " ";
                                }

                                labelResultsHomepage.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


                            }



                            //int index = sortedListForSearching.FindIndex(x => x.Contains(keyword));

                            //if (index == -1)
                            //    return;

                            //string numeroLigne = (sortedListForSearching[index].Split(';')[1]).Substring(sortedListForSearching[index].Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            //int indexx = Int32.Parse(numeroLigne) - 1;

                            //if (dicoResultsIndexSearch.Count > 0)
                            //{
                            //    dataGridViewJournal.ClearSelection();
                            //    dataGridViewJournal.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
                            //    dataGridViewJournal.Focus();
                            //}

                        }
                        catch (ArgumentNullException ex)
                        {
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

                            foreach (string r in resultatsRecherche)
                            {
                                string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                int indexx = Int32.Parse(numeroLigne) - 1;

                                if (!dicoResultsIndexSearch.ContainsKey(indexx))
                                {
                                    dicoResultsIndexSearch.Add(indexx, indexx);
                                    LinkLabel link = new LinkLabel();
                                    link.Text = numeroLigne;
                                    link.AutoSize = true;
                                    link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                    link.Name = "linkLabel" + indexx;
                                    link.Size = new System.Drawing.Size(18, 20);
                                    link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
                                    flowLayoutPanel3.Controls.Add(link);
                                    //labelResultsSearch.Text += indexx.ToString() + " ";
                                }

                                labelResultsHomepage.Text = "Résultat : " + dicoResultsIndexSearch.Count + " trouvé(s)";


                            }

                            //if (dicoResultsIndexSearch.Count > 0)
                            //{
                            //    dataGridViewJournal.ClearSelection();
                            //    dataGridViewJournal.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
                            //    dataGridViewJournal.Focus();
                            //}
                        }
                        catch (ArgumentNullException ex)
                        {
                            return;
                        }
                    }


                }
            }

        }

        private void pictureBoxRightArrow_Click(object sender, EventArgs e)
        {
            {
                if (Next > fichiersJournal.Length)
                    return;

                foreach (DataGridViewRow row in dataGridViewJournal.Rows)
                {
                    Image img = (Image)row.Cells[0].Value;
                    img.Dispose();


                }
                GC.Collect();

                dataGridViewJournal.Rows.Clear();


                for (int i = Next; i < Next + STEPP; i++)
                {

                    if (i == Next + STEPP)
                        break;

                    if (i > fichiersJournal.Length - 1)
                        break;

                    FileInfo fichier = fichiersJournal[i];

                    Rectangle rect = GetResolutionScreen();
                    int hauteurGrid = dataGridViewJournal.Size.Height;
                    int hauteurForm = JOURNAL.Size.Height;

                    try
                    {

                        //Image img = Image.FromFile(fichier.FullName);
                        if (!BUSINESSMODE)
                        {
                            Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - 119, hauteurForm - 120)));
                            dataGridViewJournal.Rows.Add(img);
                        }
                        else
                        {
                            Image tmp = Image.FromFile(fichier.FullName);
                            int differentiel = tmp.Width - tmp.Height;
                            Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width, hauteurForm - 120)));
                            dataGridViewJournal.Rows.Add(imgg);
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error " + fichier.FullName);
                    }

                }

                Next += STEPP;
                deA.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersJournal.Length - 1).ToString()).Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
                //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
            }
        }

        private void pictureBoxLeftArrow_Click(object sender, EventArgs e)
        {
            {

                if ((Next - (STEPP * 2)) < 0)
                    return;

                foreach (DataGridViewRow row in dataGridViewJournal.Rows)
                {
                    Image img = (Image)row.Cells[0].Value;
                    img.Dispose();


                }
                GC.Collect();

                dataGridViewJournal.Rows.Clear();

                for (int i = (Next - (STEPP * 2)); i <= Next - STEPP; i++)
                {

                    //if (i == Next - STEPP)
                    //    break;

                    FileInfo fichier = fichiersJournal[i];

                    Rectangle rect = GetResolutionScreen();
                    int hauteurGrid = dataGridViewJournal.Size.Height;
                    int hauteurForm = JOURNAL.Size.Height;

                    if (!BUSINESSMODE)
                    {
                        Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - 119, hauteurForm - 120)));
                        dataGridViewJournal.Rows.Add(img);
                    }
                    else
                    {
                        Image tmp = Image.FromFile(fichier.FullName);
                        int differentiel = tmp.Width - tmp.Height;
                        Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width, hauteurForm - 120)));
                        dataGridViewJournal.Rows.Add(imgg);
                    }
                    



                }


                deA.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersJournal.Length - 1).ToString()).Replace("xx/", (Next - (STEPP * 2) + 0) + "/");
                //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

                Next -= STEPP;
            }

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (BUSINESSMODE)
                FillJournalViewForBusiness();
            else
            FillJournalViewImport(pathToSave);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                dataGridViewJournal.ClearSelection();
                dataGridViewJournal.FirstDisplayedScrollingRowIndex = Int32.Parse(((LinkLabel)sender).Text);
                dataGridViewJournal.Focus();

                ((LinkLabel)sender).ForeColor = Color.Red;
            }
            catch
            {
                int index = Int32.Parse(((LinkLabel)sender).Text);
                int indexAfficher = index % STEPP;
                int nbreStep = index / STEPP;

                Next = nbreStep * STEPP;


                foreach (DataGridViewRow row in dataGridViewJournal.Rows)
                {
                    Image img = (Image)row.Cells[0].Value;
                    img.Dispose();


                }


                dataGridViewJournal.Rows.Clear();


                for (int i = Next; i < Next + STEPP; i++)
                {

                    if (i == Next + STEPP)
                        break;

                    if (i > fichiersJournal.Length - 1)
                        break;

                    FileInfo fichier = fichiersJournal[i];

                    Rectangle rect = GetResolutionScreen();
                    int hauteurGrid = dataGridViewJournal.Size.Height;
                    int hauteurForm = JOURNAL.Size.Height;

                    try
                    {

                        Image img = Image.FromFile(fichier.FullName);
                        dataGridViewJournal.Rows.Add(img);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error " + fichier.FullName);
                    }

                }

                Next += STEPP;

                dataGridViewJournal.ClearSelection();
                dataGridViewJournal.FirstDisplayedScrollingRowIndex = indexAfficher;
                dataGridViewJournal.Focus();



            }

        }

        private void FillIdentifiants()
        {
            panelIdentifiantsVisible.Visible = true;
            Dictionary<string, string> identifiants = new Dictionary<string, string>();
            dataGridViewIdentifiants.Rows.Clear();

            //Friends
            if (File.Exists(pathToSave + "\\friends.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\friends.txt");
                foreach (string li in lines)
                {
                    if (!identifiants.ContainsKey(li.Split(';')[2]))
                    {
                        identifiants.Add(li.Split(';')[2], li);
                    }
                }
            }

            //Friends
            if (File.Exists(pathToSave + "\\followers.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\followers.txt");
                foreach (string li in lines)
                {
                    if (!identifiants.ContainsKey(li.Split(';')[2]))
                    {
                        identifiants.Add(li.Split(';')[2], li);
                    }
                }
            }

            //FriendsFromPictures
            if (File.Exists(pathToSave + "\\PICTURES\\friendsFromComments.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\PICTURES\\friendsFromComments.txt");
                string tmp = "";
                foreach (string li in lines)
                {
                    string[] ident = li.Split(';');

                    string url = ident[0];
                    string username = ident[1];
                    string id = ident[2];

                    if (ident[0].Contains("?fref"))
                    {
                        url = ident[0].Split(new string[] { "?fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&fref"))
                    {
                        url = ident[0].Split(new string[] { "&fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&href"))
                    {
                        url = ident[0].Split(new string[] { "&href" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("?comment"))
                    {
                        url = ident[0].Split(new string[] { "?comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&comment"))
                    {
                        url = ident[0].Split(new string[] { "&comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }

                    tmp = url + ";" + username + ";" + id + "\n";

                    if (!identifiants.ContainsKey(id))
                    {
                        identifiants.Add(id, tmp);
                    }
                }
            }

            //FriendsFromPictures
            if (File.Exists(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt");
                string tmp = "";
                foreach (string li in lines)
                {
                    string[] ident = li.Split(';');

                    string url = ident[0];
                    string username = ident[1];
                    string id = ident[2];

                    if (ident[0].Contains("?fref"))
                    {
                        url = ident[0].Split(new string[] { "?fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&fref"))
                    {
                        url = ident[0].Split(new string[] { "&fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&href"))
                    {
                        url = ident[0].Split(new string[] { "&href" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("?comment"))
                    {
                        url = ident[0].Split(new string[] { "?comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&comment"))
                    {
                        url = ident[0].Split(new string[] { "&comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }

                    tmp = url + ";" + username + ";" + id + "\n";


                    if (!identifiants.ContainsKey(id))
                    {
                        identifiants.Add(id, tmp.Trim());
                    }
                }
            }

            if (identifiants.Count > 0)
            {
                foreach (string li in identifiants.Values)
                {
                    dataGridViewIdentifiants.Rows.Add(li.Split(';')[0], li.Split(';')[1], li.Split(';')[2]);
                }
            

                if (!File.Exists(pathToSave +  "\\AllIdentifiants.txt"))
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\AllIdentifiants.txt", false))
                    {
                        string textes = "";
                        foreach (string li in identifiants.Values)
                        {
                            textes += li.Trim() + "\n";
                        }

                        file.Write(textes);

                    }
                else
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\AllIdentifiants.txt", true))
                    {
                        string textes = "";
                        foreach (string li in identifiants.Values)
                        {
                            textes += li.Trim() + "\n";
                        }

                        file.Write(textes);

                    }
                }

                panelIdentifiantsVisible.Visible = false;
                pictureBoxIdentifiants.Visible = true;
                labelIdentifiants.Text = "IDENTIFIANTS : " + dataGridViewIdentifiants.Rows.Count;

            }
            else
                panelIdentifiantsVisible.Visible = false;
        }
        private void FillParameters()
        {
            Dictionary<string, string> identifiants = new Dictionary<string, string>();
            
            

            //SessionsActives
            if (File.Exists(pathToSave + "\\PARAMETERS\\SessionsActives.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave  + "\\PARAMETERS\\SessionsActives.txt");
                foreach (string li in lines)
                {
                    richTextBoxSessionsActives.Text += li;
                }
            }

            //Connexionsdeconnexions
            if (File.Exists(pathToSave + "\\PARAMETERS\\ConnexionsDeconnexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\PARAMETERS\\ConnexionsDeconnexions.txt");
                foreach (string li in lines)
                {
                    richTextBoxConnexionsDeconnexions.Text += li;
                }
            }

            //Recherches
            if (File.Exists(pathToSave + "\\PARAMETERS\\Recherches.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave  + "\\PARAMETERS\\Recherches.txt");
                foreach (string li in lines)
                {
                    richTextBoxRecherches.Text += li;
                }
            }
            //Connexions
            if (File.Exists(pathToSave + "\\PARAMETERS\\Connexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\PARAMETERS\\Connexions.txt");
                foreach (string li in lines)
                {
                    richTextBox1Connexions.Text += li;
                }
            }

            //Parametres
            if (File.Exists(pathToSave + "\\PARAMETERS\\Parametres.txt"))
            {
                string[] lines = File.ReadAllLines(pathToSave + "\\PARAMETERS\\Parametres.txt");
                foreach (string li in lines)
                {
                    richTextBoxparam.Text += li;
                }
            }


        }
        private void GetPictures()
        {


            if (driver == null)
            {
                InitializeDriverForBusiness();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;

            if (!profilIsSet)
                GetProfileInformationsForBusiness(backgroundWorker1);


            try
            {
                backgroundWorker1.ReportProgress(-120);

                string targetName = textBoxops.Text;
                Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();


                int width = driver.Manage().Window.Size.Width;
                int height = driver.Manage().Window.Size.Height;



                if (!Directory.Exists(pathToSave + @"\PICTURES\"))
                    Directory.CreateDirectory(pathToSave + @"\PICTURES\");


                //driver.Navigate().GoToUrl(urlFriend + "/posts/?ref=page_internal");
                driver.Navigate().GoToUrl(urlFriend + "/photos");
                Thread.Sleep(5000);

                if (isElementFlyOutPresent(driver))
                {
                    driver.FindElementByXPath("//div[@class='fbNubFlyout fbDockChatTabFlyout uiContextualLayerParent']");

                    IList<IWebElement> el = driver.FindElementsByXPath("//div[@class='fbNubFlyout fbDockChatTabFlyout uiContextualLayerParent']");
                    foreach (IWebElement ell in el)
                    {
                        IList<IWebElement> divs = ell.FindElements(By.ClassName("close"));

                        foreach (IWebElement divv in divs)
                        {

                            try
                            {
                                divv.Click();
                                break;
                            }
                            catch
                            {

                            }


                        }
                    }

                }

                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");


                    int i = 1;
                    int hauteur = 450;
                    //int h = 0;
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                    long innerHeightt = (long)innerHeight;
                    long scroll = (long)innerHeight;
                    long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    scrollHeight = scrollHeight + scroll;
                    IList<IWebElement> elementsNew = driver.FindElements(By.TagName("div"));

                    IWebElement toResize = null;
                    if (toResize == null)
                        foreach (IWebElement el in elementsNew)
                        {
                            if (el.Size.Width >= 820 && el.Size.Width < 900)
                            {
                                toResize = el;

                                break;
                            }

                        }

                    while (scrollHeight >= innerHeightt)
                    {

                        CaptureElementScreenShot(toResize, pathToSave + @"\PICTURES\" + "ElementScreenshot_" + i + ".png");


                        Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                        imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                        //Save the screenshot
                        imageScreenshott.SaveAsFile(pathToSave + @"\PICTURES\" + "Screenshot_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                        Thread.Sleep(100);

                        string pathToFile = pathToSave + @"\PICTURES\" + "Screenshot_" + i + ".png";




                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(2500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        i++;

                    }
                }
                catch (Exception ex)
                {
                    //e.printStackTrace();
                }






            }
            catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
            {


            }

            backgroundWorker1.ReportProgress(-121);

            Thread.Sleep(2000);

            backgroundWorker1.CancelAsync();


        }
        private bool isElementFlyOutPresent(ChromeDriver driver)
        {
            try
            {
                return driver.FindElementByXPath("//div[@class='fbNubFlyout fbDockChatTabFlyout uiContextualLayerParent']").Displayed;
                //return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                return false;
            }
        }
        public Image CaptureElementScreenShot(IWebElement element, string uniqueName)
        {
            Screenshot screenshot = ((ITakesScreenshot)this.driver).GetScreenshot();
            screenshot.SaveAsFile(uniqueName, OpenQA.Selenium.ScreenshotImageFormat.Png);


            Image img = Bitmap.FromFile(uniqueName);
            Rectangle rect = new Rectangle();

            int width = 0;
            int height = 0;

            if (element != null)
            {
                // Get the Width and Height of the WebElement using
                width = element.Size.Width;
                height = element.Size.Height;

                // Get the Location of WebElement in a Point.
                // This will provide X & Y co-ordinates of the WebElement
                Point p = element.Location;

                // Create a rectangle using Width, Height and element location
                rect = new Rectangle(p.X - 6, 0, width + 10, img.Height);
                //rect.Intersect(new Rectangle(0, 0, width, height));
            }



            //rect.Height = finalHeight;
            Bitmap bmpImage = new Bitmap(img);
            img.Dispose();
            var cropedImag = bmpImage.Clone(rect, bmpImage.PixelFormat);
            cropedImag.Save(uniqueName);


            return cropedImag;
        }
        private void GetProfileInformationsForBusiness(BackgroundWorker t)
        {

            string targetName = textBoxops.Text;
            string titrePage = "";
            string urlFriend = textBoxUSERNAMEFRIENDS.Text;
            string ID = "";
            //pour cacher fenetre DOS

            if (driver == null)
            {
                InitializeDriver();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                Thread.Sleep(2500);
                //searchButton.Click();

                //u_0_8
                //"//div[@class='menuBar']//*[@class='menuItem']"
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl(urlFriend);
                titrePage = driver.Title;
                Thread.Sleep(5000);
            }
            //var driverService = ChromeDriverService.CreateDefaultService();
            //driverService.HideCommandPromptWindow = true;
            // progressBarfriends.Visible = true;

            //var driver = new ChromeDriver(driverService, new ChromeOptions());

            //System.Diagnostics.Process.Start(filepath);
            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--disable-notifications");
            System.Random rnd = new System.Random();

            driver.Navigate().GoToUrl(urlFriend);
            titrePage = driver.Title;
            Thread.Sleep(5000);

            if (profilIsSet == false)
                try
                {


                    //var image = driver.FindElementByXPath("//a[@class='_2dgj']");
                    IList<IWebElement> el = driver.FindElementsByTagName("img");
                    IList<IWebElement> svg = driver.FindElementsByTagName("svg");
                    IList<IWebElement> ids = driver.FindElementsByTagName("a");

                    //foreach(IWebElement elId in ids)
                    //{
                    //    var tmp = elId.GetAttribute("aria-label");
                    //    if(tmp != null)
                    //        if (tmp.ToLower().Contains("profil"))
                    //        {
                    //            ID = elId.GetAttribute("href");
                    //            if (ID.Contains("facebook.com/"))
                    //            {
                    //                ID = ID.Substring(ID.IndexOf("facebook.com/") + 13).Split('/')[0];
                    //                //backgroundWorkerFriends.ReportProgress(-2, ID);
                    //            }

                    //            break;
                    //        }

                    //}

                    if (svg != null)//new look
                    {
                        try
                        {
                            foreach (IWebElement ell in svg)
                            {
                                if (ell.Size.Width == ell.Size.Height && ell.Size.Width == 132 && !isElementPresent(driver, "rq0escxv lpgh02oy tkr6xdv7 rek2kq2y"))
                                {
                                    IList<IWebElement> imgs = ell.FindElements(By.TagName("g"));
                                    IWebElement link = imgs[0];

                                    if (link.Size.Width == link.Size.Height && link.Size.Width == 132)// && link.Size.Width < 200)
                                    {
                                        var linkToImage = link.FindElement(By.TagName("image")).GetAttribute("xlink:href");

                                        if (linkToImage != "")
                                        {
                                            try
                                            {
                                                using (var client = new WebClient())
                                                {
                                                    if (!Directory.Exists(pathToSave + "\\"))
                                                        Directory.CreateDirectory(pathToSave  + "\\");

                                                    if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                    {
                                                        client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        Thread.Sleep(5000);


                                                        t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        //t.ReportProgress(-2, ID);

                                                        Thread.Sleep(2000);
                                                        profilIsSet = true;

                                                    }
                                                    else
                                                    {


                                                        t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                        //t.ReportProgress(-2, ID);

                                                        profilIsSet = true;
                                                    }


                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                                return;
                                            }


                                        }
                                        break;
                                    }
                                }

                                if (!profilIsSet)//si groupe
                                {
                                    if (ell.Size.Width == ell.Size.Height && ell.Size.Width == 40 && ell.GetAttribute("class") == "pzggbiyp")
                                    {
                                        IList<IWebElement> imgs = ell.FindElements(By.TagName("g"));
                                        IWebElement link = imgs[0];

                                        if (link.Size.Width == link.Size.Height && link.Size.Width == 40)// && link.Size.Width < 200)
                                        {
                                            var linkToImage = link.FindElement(By.TagName("image")).GetAttribute("xlink:href");

                                            if (linkToImage != "")
                                            {
                                                try
                                                {
                                                    using (var client = new WebClient())
                                                    {
                                                        if (!Directory.Exists(pathToSave + "\\"))
                                                            Directory.CreateDirectory(pathToSave + "\\");

                                                        if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                        {
                                                            client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            Thread.Sleep(5000);


                                                            t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            //t.ReportProgress(-2, ID);

                                                            Thread.Sleep(2000);
                                                            profilIsSet = true;

                                                        }
                                                        else
                                                        {


                                                            t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                            //t.ReportProgress(-2, ID);

                                                            profilIsSet = true;
                                                        }


                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                                    return;
                                                }


                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        catch
                        {

                        }
                    }

                    if (el != null && !profilIsSet)
                    {
                        try
                        {
                            //ID = el[0].GetAttribute("href");
                            //if (ID.Contains("facebook.com/"))
                            //{
                            //    ID = ID.Substring(ID.IndexOf("facebook.com/") + 13).Split('/')[0];
                            //    //backgroundWorkerFriends.ReportProgress(-2, ID);
                            //}
                            IList<IWebElement> els = el[0].FindElements(By.TagName("img"));// html / body / div[1] / div / div / div[2] / div / div / div[1] / div / div[1] / div[2] / div / div / div / div / div[1] / div / div / a / div / svg / g / image

                            foreach (IWebElement ell in el)
                            {

                                if (ell.Size.Width == ell.Size.Height && ell.Size.Width >= 132)
                                {


                                    var linkToImage = ell.GetAttribute("src");

                                    if (linkToImage != "")
                                    {
                                        try
                                        {
                                            using (var client = new WebClient())
                                            {
                                                if (!Directory.Exists(pathToSave + "\\"))
                                                    Directory.CreateDirectory(pathToSave + "\\");

                                                if (!File.Exists(pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg"))
                                                {
                                                    client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    Thread.Sleep(5000);


                                                    t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    //t.ReportProgress(-2, ID);

                                                    Thread.Sleep(2000);
                                                    profilIsSet = true;

                                                }
                                                else
                                                {


                                                    t.ReportProgress(-5, pathToSave + "\\" + titrePage.Replace("\"", "") + ".jpg");
                                                    //t.ReportProgress(-2, ID);

                                                    profilIsSet = true;
                                                }


                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                            return;
                                        }


                                    }

                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                        catch
                        {

                        }
                    }
                    //clic sur image dans href
                    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                    //backgroundWorker1.ReportProgress(1);

                    Thread.Sleep(5000);
                }
                catch (OpenQA.Selenium.NoSuchElementException ex)
                {

                    try
                    {
                        var image = driver.FindElementByXPath("//a[@class='_1nv3 _1nv5 profilePicThumb']");
                        IWebElement el = driver.FindElementByXPath("//a[@class='_1nv3 _11kg _1nv5 profilePicThumb']");
                        if (el != null)
                        {
                            try
                            {
                                ID = el.GetAttribute("href");
                                if (ID.Contains("profile_id="))
                                {
                                    ID = ID.Substring(ID.IndexOf("profile_id=") + 11).Split('"')[0];
                                    //backgroundWorkerFriends.ReportProgress(-2, ID);
                                }
                            }
                            catch
                            {

                            }

                            IList<IWebElement> els = el.FindElements(By.TagName("img"));
                            foreach (IWebElement ell in els)
                            {
                                var linkToImage = ell.GetAttribute("src");

                                if (linkToImage != "")
                                {
                                    try
                                    {
                                        using (var client = new WebClient())
                                        {

                                            if (!Directory.Exists(pathToSave + "\\"))
                                                Directory.CreateDirectory(pathToSave + "\\");

                                            if (!File.Exists(pathToSave + "\\" + titrePage + ".jpg"))
                                            {
                                                client.DownloadFile(linkToImage, pathToSave + "\\" + titrePage + ".jpg");
                                                Thread.Sleep(5000);


                                                t.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                                                t.ReportProgress(-2, ID);

                                                Thread.Sleep(2000);

                                            }
                                            else
                                            {


                                                t.ReportProgress(-5, pathToSave + "\\" + titrePage + ".jpg");
                                                t.ReportProgress(-2, ID);


                                            }

                                            profilIsSet = true;
                                        }
                                    }
                                    catch (Exception exx)
                                    {
                                        MessageBox.Show("PROBLEME AVEC LE TELECHARGEMENT POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                                        return;
                                    }


                                }
                            }

                        }
                        //clic sur image dans href
                        //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", image);
                        //backgroundWorker1.ReportProgress(1);

                        //Thread.Sleep(5000);

                    }
                    catch (OpenQA.Selenium.NoSuchElementException exx)
                    {
                        MessageBox.Show("PROBLEME AVEC L'IDENTIFIEUR DE CLASSE POUR LA PHOTO DE PROFIL" + Environment.NewLine + ex.Message);
                        return;
                    }


                }
        }
        private void GetHomePageForBusiness()
        {
            string resultat = "";
            Dictionary<string, string> resultats = new Dictionary<string, string>();

            if (driver == null)
            {
                InitializeDriverForBusiness();
                // 2. Go to the "Google" homepage
                driver.Navigate().GoToUrl("https://facebook.com/login");

                // 3. Find the username textbox (by ID) on the homepage
                var userNameBox = driver.FindElementById("email");

                // 4. Enter the text (to search for) in the textbox
                userNameBox.SendKeys(textBoxUSERNAME.Text);

                // 3. Find the username textbox (by ID) on the homepage
                var userpasswordBox = driver.FindElementById("pass");

                // 4. Enter the text (to search for) in the textbox
                userpasswordBox.SendKeys(textBoxPASSWORD.Text);
                Thread.Sleep(5000);

                // 5. Find the search button (by Name) on the homepage
                driver.FindElementById("loginbutton").Click();
                //searchButton.Click();
                Thread.Sleep(2500);
            }

            string urlFriend = textBoxUSERNAMEFRIENDS.Text;

            System.Random rnd = new System.Random();
            int nbreAnnee = 1;


            if (!profilIsSet)
                GetProfileInformationsForBusiness(backgroundWorker1);


            try
            {

                //backgroundWorker1.ReportProgress(-130);
                string targetName = textBoxops.Text;
                
                Dictionary<string, string> dicoMessagesFrom = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisibles = new Dictionary<string, string>();
                Dictionary<string, string> messagesVisiblesForFile = new Dictionary<string, string>();

                int newHauteur = 0;

                int width = driver.Manage().Window.Size.Width;
                int height = driver.Manage().Window.Size.Height;



                if (!Directory.Exists(pathToSave + @"\HOMEPAGE\"))
                    Directory.CreateDirectory(pathToSave + @"\HOMEPAGE\");


                //driver.Navigate().GoToUrl(urlFriend + "/posts/?ref=page_internal");
                driver.Navigate().GoToUrl(urlFriend);
                Thread.Sleep(5000);

                if (isElementFlyOutPresent(driver))
                {
                    driver.FindElementByXPath("//div[@class='fbNubFlyout fbDockChatTabFlyout uiContextualLayerParent']");

                    IList<IWebElement> el = driver.FindElementsByXPath("//div[@class='fbNubFlyout fbDockChatTabFlyout uiContextualLayerParent']");
                    foreach (IWebElement ell in el)
                    {
                        IList<IWebElement> divs = ell.FindElements(By.ClassName("close"));

                        foreach (IWebElement divv in divs)
                        {

                            try
                            {
                                divv.Click();
                                break;
                            }
                            catch
                            {

                            }


                        }
                    }

                }

                try
                {
                    Object lastHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");


                    int i = 1;
                    int hauteur = 450;

                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);"); //Scroll To Top

                    Object innerHeight = ((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight;");
                    long innerHeightt = (long)innerHeight;
                    long scroll = (long)innerHeight;
                    long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                    scrollHeight = scrollHeight + scroll;

                    IList<IWebElement> elements = driver.FindElements(By.XPath("//div[@class='_5pcr userContentWrapper']"));
                    IList<IWebElement> elementsNew = driver.FindElements(By.TagName("div"));

                    IWebElement toResize = null;
                    if (toResize == null)
                        foreach (IWebElement el in elementsNew)
                        {
                            if (el.Size.Width == 500)
                            {
                                toResize = el;
                                break;
                            }

                        }

                    while (scrollHeight >= innerHeightt)
                    {


                        if (elementsNew.Count > 0)//newLook
                        {

                            CaptureElementScreenShot(toResize, pathToSave + @"\HOMEPAGE\" + "ElementScreenshot_" + i + ".png");

                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);

                            string pathToFile = pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png";

                            if (!FASTJOURNALFORBUSINESS)
                            {


                                var messageFromm = driver.FindElementsByXPath("//div[@class='_5pcr userContentWrapper']");



                                ////on récupère les message provenant de from
                                object[] messageFroms = messageFromm.ToArray();

                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messageFroms)
                                {

                                    string tentation = o.ToString();
                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                    {
                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                    }
                                    //else
                                    //    continue;

                                    if (o.LocationOnScreenOnceScrolledIntoView.Y > 15 && o.LocationOnScreenOnceScrolledIntoView.Y < (height - 250))
                                    {
                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                        {
                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                        }
                                    }

                                }

                                foreach (string cle in messagesVisibles.Keys)
                                {

                                    if (!messagesVisiblesForFile.ContainsKey(cle))
                                    {

                                        string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                        string tmp = "";
                                        foreach (string li in lignes)
                                        {
                                            tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                        }



                                        messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                    }

                                }

                            }

                        }
                        else
                        {
                            IWebElement findePage = null;

                            if (elements.Count > 0)
                            {
                                findePage = elements[0];
                                foreach (IWebElement el in elements)
                                {
                                    IList<IWebElement> ell = el.FindElements(By.TagName("i"));
                                    findePage = ell[0];
                                    //MakeElemScreenshot(driver, el);
                                    CaptureElementScreenShot(el, pathToSave + @"\HOMEPAGE\" + "ElementScreenshot_" + i + ".png");


                                    break;
                                }
                            }

                            if (!FASTJOURNALFORBUSINESS)
                            {


                                var messageFromm = driver.FindElementsByXPath("//div[@class='_5pcr userContentWrapper']");



                                ////on récupère les message provenant de from
                                object[] messageFroms = messageFromm.ToArray();

                                foreach (OpenQA.Selenium.Remote.RemoteWebElement o in messageFroms)
                                {

                                    string tentation = o.ToString();
                                    string idd = tentation.Substring(tentation.IndexOf("Element (id = ") + 14).Split(')')[0];

                                    if (!dicoMessagesFrom.ContainsKey(idd))
                                    {
                                        dicoMessagesFrom.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                    }
                                    //else
                                    //    continue;

                                    if (o.LocationOnScreenOnceScrolledIntoView.Y > 15 && o.LocationOnScreenOnceScrolledIntoView.Y < (height - 250))
                                    {
                                        if (!messagesVisibles.ContainsKey(idd) && o.Text != "")
                                        {
                                            messagesVisibles.Add(idd, o.Text.Trim().Replace("\n", "").Replace("\t", ""));
                                        }
                                    }

                                }

                            }


                            var messageFrom = driver.FindElementsByXPath("//div[@class='_5pcr userContentWrapper']");


                            Screenshot imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();
                            imageScreenshott = ((ITakesScreenshot)driver).GetScreenshot();

                            //Save the screenshot
                            imageScreenshott.SaveAsFile(pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png", OpenQA.Selenium.ScreenshotImageFormat.Png);
                            Thread.Sleep(100);

                            string pathToFile = pathToSave + @"\HOMEPAGE\" + "Screenshot_" + i + ".png";

                            foreach (string cle in messagesVisibles.Keys)
                            {

                                if (!messagesVisiblesForFile.ContainsKey(cle))
                                {

                                    string[] lignes = messagesVisibles[cle].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                    string tmp = "";
                                    foreach (string li in lignes)
                                    {
                                        tmp += li.Trim().Replace("\r", "").Replace(";", "");
                                    }



                                    messagesVisiblesForFile.Add(cle, tmp + ";" + pathToFile + "\n");
                                }

                            }
                        }



                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                        if ((scrollHeight - innerHeightt) < 200)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                            Thread.Sleep(500);


                        scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");
                        Thread.Sleep(2000);


                        if (scrollHeight <= innerHeightt)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, " + hauteur + ");");
                            Thread.Sleep(2000);
                            scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight;");

                        }

                        scrollHeight = scrollHeight + scroll;
                        innerHeightt = innerHeightt + hauteur;
                        i++;

                    }
                }
                catch (Exception ex)
                {
                    //e.printStackTrace();
                }
                try
                {
                    string codePage = driver.PageSource;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                    {
                        //if (File.Exists(saveFileDialog1.FileName))
                        //    File.Delete(saveFileDialog1.FileName);


                        file.Write(codePage);
                    }

                    if (!FASTJOURNAL)
                    {
                        Dictionary<string, string> newDico = new Dictionary<string, string>();
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\friendsFromHomepage.txt", false))
                        {
                            //on essaie de recuperer les identifiants
                            string[] idfs = codePage.Split(new string[] { "<a class=\"_6qw4\"" }, StringSplitOptions.RemoveEmptyEntries);
                            //string resultat = "";
                            string id = "";
                            string username = "";
                            string url = "";
                            //Dictionary<string, string> resultats = new Dictionary<string, string>();



                            foreach (string li in idfs)
                            {
                                string tmp = li.Trim();
                                if (tmp.StartsWith("data-hovercard=") && tmp.Contains("?id="))
                                {
                                    id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                    if (tmp.Contains("href=\""))
                                    {


                                        url = "https://www.facebook.com/" + tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                        username = tmp.Substring(tmp.IndexOf("\">") + 2).Split('<')[0];
                                    }

                                    resultat = url + ";" + username + ";" + id + "\n";
                                    if (!resultats.ContainsKey(resultat))
                                    {
                                        resultats.Add(resultat, resultat);
                                    }

                                }
                            }

                            idfs = codePage.Split(new string[] { "<a title=\"" }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string li in idfs)
                            {
                                string tmp = li.Trim();
                                if (tmp.Contains("href=\"") && tmp.Contains("?id="))


                                {
                                    id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];

                                    if (tmp.Contains("href=\""))
                                    {
                                        url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                        username = tmp.Split('"')[0];
                                    }

                                    resultat = url + ";" + username + ";" + id + "\n";
                                    if (!resultats.ContainsKey(resultat))
                                    {
                                        resultats.Add(resultat, resultat);
                                    }


                                }
                            }

                            idfs = codePage.Split(new string[] { "<a class=\"profileLink" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string li in idfs)
                            {
                                string tmp = li.Trim();
                                if (tmp.StartsWith("\" title=\"") && tmp.Contains("?id="))
                                {
                                    id = tmp.Substring(tmp.IndexOf("?id=") + 4).Split('&')[0];



                                    if (tmp.Contains("href=\""))
                                    {
                                        url = tmp.Substring(tmp.IndexOf("href=\"") + 6).Split('?')[0].Replace(";", "");
                                        username = tmp.Substring(tmp.IndexOf("\" title=\"") + 9).Split('"')[0];
                                    }

                                    resultat = url + ";" + username + ";" + id + "\n";
                                    if (!resultats.ContainsKey(resultat))
                                    {
                                        resultats.Add(resultat, resultat);
                                    }


                                }
                            }

                           

                            string pourFichier = "";
                            foreach (string l in resultats.Values)
                            {
                                if (l.Contains("<img class") || l.Contains("<div class"))
                                    continue;
                                pourFichier += l;
                            }

                            file.Write(pourFichier);
                        }

                        //if (newDico.Count() > 0)
                        //{
                        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\AllContactsFromHomepage.txt", false))
                        //    {
                        //        string pourFichier = "";
                        //        foreach (string ll in newDico.Values)
                        //        {
                        //            pourFichier += ll;
                        //        }
                        //        file.Write(pourFichier);
                        //    }
                        //}
                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt", false))
                        //{
                        //    string textes = "";
                        //    foreach (string t in messagesVisiblesForFile.Values)
                        //    {
                        //        textes += t;
                        //    }


                        //    file.Write(textes);
                        //    messagesVisiblesForFile = new Dictionary<string, string>();
                        //}
                    }




                }
                catch (Exception ex)
                {

                }


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt", false))
                {
                    string textes = "";
                    foreach (string t in messagesVisiblesForFile.Values)
                    {
                        textes += t;
                    }


                    file.Write(textes);
                    messagesVisiblesForFile = new Dictionary<string, string>();
                }

                Thread.Sleep(2500);

               

            }
            catch (OpenQA.Selenium.NoSuchElementException ex)//si pas d'acces à la page on essaie les annees manuellement
            {
                try
                {
                    string targetName = textBoxops.Text;


                    string[] liYears = new string[] { DateTime.Now.Year.ToString(), ((DateTime.Now.Year) - 1).ToString() };

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    backgroundWorkerJournal.ReportProgress(-1, liYears.Length);

                    //label18.Text = "Téléchargement des comments en cours ";
                    foreach (string y in liYears)
                    {
                        driver.Navigate().GoToUrl(urlFriend);
                        Thread.Sleep(5000);

                        while (!isElementPresentForComment(driver))
                        {


                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                            //((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 100)");
                            Thread.Sleep(rnd.Next(500, 1500));



                        }


                        string codePage = driver.PageSource;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathToSave + "\\HOMEPAGE\\Journal.html", false))
                        {
                            //if (File.Exists(saveFileDialog1.FileName))
                            //    File.Delete(saveFileDialog1.FileName);

                            file.Write(codePage);
                        }

                        //backgroundWorkerJournal.ReportProgress(nbreAnnee);
                        Thread.Sleep(100);
                        nbreAnnee++;
                    }






                    //driver.Close();



                }
                catch
                {

                }

            }


            //}
            //Thread.Sleep(2000);
            backgroundWorker1.ReportProgress(-122);
            Thread.Sleep(2000);
            backgroundWorker1.CancelAsync();
            //comments = false;
            //labelanalyseencours.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //pictureBoxlogofacebook.Visible = false;

        }



        private void FillIdentifiantsImport(string pathConfig)
        {
            Dictionary<string, string> identifiants = new Dictionary<string, string>();
            dataGridViewIdentifiants.Rows.Clear();

            //Friends
            if (File.Exists(pathConfig + "\\friends.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\friends.txt");
                foreach (string li in lines)
                {
                    if (!identifiants.ContainsKey(li.Split(';')[2]))
                    {
                        identifiants.Add(li.Split(';')[2], li);
                    }
                }
                pictureBoxfriends.Visible = true;
            }

            //Friends
            if (File.Exists(pathConfig + "\\followers.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\followers.txt");
                foreach (string li in lines)
                {
                    if (!identifiants.ContainsKey(li.Split(';')[2]))
                    {
                        identifiants.Add(li.Split(';')[2], li);
                    }
                }
            }

            //FriendsFromPictures
            if (File.Exists(pathConfig + "\\PICTURES\\friendsFromComments.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PICTURES\\friendsFromComments.txt");
                string tmp = "";
                foreach (string li in lines)
                {
                    string[] ident = li.Split(';');

                    string url = ident[0];
                    string username = ident[1];
                    string id = ident[2];

                    if (ident[0].Contains("?fref"))
                    {
                        url = ident[0].Split(new string[] { "?fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&fref"))
                    {
                        url = ident[0].Split(new string[] { "&fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&href"))
                    {
                        url = ident[0].Split(new string[] { "&href" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("?comment"))
                    {
                        url = ident[0].Split(new string[] { "?comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&comment"))
                    {
                        url = ident[0].Split(new string[] { "&comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }

                    tmp = url + ";" + username + ";" + id + "\n";

                    if (!identifiants.ContainsKey(id))
                    {
                        identifiants.Add(id, tmp);
                    }
                }
            }

            //FriendsFromPictures
            if (File.Exists(pathConfig + "\\HOMEPAGE\\friendsFromHomepage.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\HOMEPAGE\\friendsFromHomepage.txt");
                string tmp = "";
                foreach (string li in lines)
                {
                    string[] ident = li.Split(';');

                    string url = ident[0];
                    string username = ident[1];
                    string id = ident[2];

                    if (ident[0].Contains("?fref"))
                    {
                        url = ident[0].Split(new string[] { "?fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&fref"))
                    {
                        url = ident[0].Split(new string[] { "&fref" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&href"))
                    {
                        url = ident[0].Split(new string[] { "&href" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("?comment"))
                    {
                        url = ident[0].Split(new string[] { "?comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (ident[0].Contains("&comment"))
                    {
                        url = ident[0].Split(new string[] { "&comment" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }

                    tmp = url + ";" + username + ";" + id + "\n";


                    if (!identifiants.ContainsKey(id))
                    {
                        identifiants.Add(id, tmp.Trim());
                    }
                }
            }

            if (identifiants.Count > 0)
            {
                foreach (string li in identifiants.Values)
                {
                    dataGridViewIdentifiants.Rows.Add(li.Split(';')[0], li.Split(';')[1], li.Split(';')[2]);
                }
            }

            if (!File.Exists(pathConfig + "\\AllIdentifiants.txt"))
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathConfig + "\\AllIdentifiants.txt", false))
                {
                    string textes = "";
                    foreach (string li in identifiants.Values)
                    {
                        textes += li.Trim() + "\n";
                    }

                    file.Write(textes);

                }
        }
        private void FillParametersImport(string pathConfig)
        {
            

            //SessionsActives
            if (File.Exists(pathConfig + "\\PARAMETERS\\SessionsActives.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig +  "\\PARAMETERS\\SessionsActives.txt");
                foreach (string li in lines)
                {
                    if (li.Contains("Modifier"))
                    {
                        string lii = li.Replace("Modifier", "\n");

                        richTextBoxSessionsActives.Text += lii + "\n";
                    }
                    else
                        richTextBoxSessionsActives.Text += li + "\n";

                }

                pictureBoxParam.Visible = true;
            }

            //Connexionsdeconnexions
            if (File.Exists(pathConfig + "\\PARAMETERS\\ConnexionsDeconnexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\ConnexionsDeconnexions.txt");
                foreach (string li in lines)
                {
                    richTextBoxConnexionsDeconnexions.Text += li + "\n";
                }

                pictureBoxParam.Visible = true;
            }

            //Recherches
            if (File.Exists(pathConfig + "\\PARAMETERS\\Recherches.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Recherches.txt");
                foreach (string li in lines)
                {
                    richTextBoxRecherches.Text += li + "\n";
                }

                pictureBoxParam.Visible = true;
            }
            //Connexions
            if (File.Exists(pathConfig + "\\PARAMETERS\\Connexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Connexions.txt");
                foreach (string li in lines)
                {
                    richTextBox1Connexions.Text += li + "\n";
                }

                pictureBoxParam.Visible = true;
            }

            //Parametres
            if (File.Exists(pathConfig +  "\\PARAMETERS\\Parametres.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Parametres.txt");
                foreach (string li in lines)
                {
                    richTextBoxparam.Text += li + "\n";
                }

                pictureBoxParam.Visible = true;
            }


        }

        private void dataGridViewIdentifiants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Process.Start(dataGridViewIdentifiants.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void dataGridViewJournal_Scroll(object sender, ScrollEventArgs e)
        {

            string texteAGarder = deA.Text.Split('/')[1];
            string numero = deA.Text.Split('/')[0].Replace("_____", "");
            Int32.Parse(numero);
            int step = dataGridViewJournal.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridViewJournal.FirstDisplayedScrollingRowIndex;


            deA.Text = "_____" + (step + (Next - STEPP)) + "/" + texteAGarder;
            //deA.Text = deA.Text.Replace(texteAremplacer, dataGridViewJournal.FirstDisplayedScrollingRowIndex.ToString() + "/");



        }

        private void textBoxops_Validated(object sender, EventArgs e)
        {
           
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            DateTime tempss;
            CultureInfo culture = new CultureInfo("fr-FR");
            string datum = "15 MAI 2020 à 22:22".Split('à')[0].Replace(" ", "/");
            tempss = Convert.ToDateTime("15/MAI/2020 à 22:22".Split('à')[0], culture);
            DateTime date2 = new DateTime(2020, 5, 1, 0, 0, 0);
        }

        private void dataGridViewForPictures_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Process.Start(dataGridViewForPictures.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void contextMenuStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            //PictureBox destination = ((PictureBox)sender);

            string pathToFolder = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString().Replace(".\\", "\\");
            string folderName = dataGridViewMessenger.SelectedRows[0].Cells[1].Value.ToString();

            PictureBox box = new PictureBox();
            box.Size = new Size(500, 500);
            box.Image = (Image)((DataGridView)sourceControl).SelectedRows[0].Cells[0].Value;
            //box.BorderStyle = BorderStyle.FixedSingle;
            box.Cursor = Cursors.Hand;
            box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            box.ContextMenuStrip = contextMenuStrip2;
            box.Name = pathToFolder + "\\Messenger_" + folderName.Trim() + "_" + (((DataGridView)sourceControl).SelectedRows[0].Index + 1).ToString();

            flowLayoutPanelAnnexe.Controls.Add(box);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
                FASTMESSENGER = true;
            else
                FASTMESSENGER = false;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
                FASTJOURNAL = true;
            else
                FASTJOURNAL = false;
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {

        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox9.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox6.Checked;
        }

        private void checkBoxBusinessMode_CheckedChanged(object sender, EventArgs e)
        {
            label13.Visible = checkBoxBusinessMode.Checked;
            pictureBox36.Visible = checkBoxBusinessMode.Checked;
            panelBusiness.Visible = checkBoxBusinessMode.Checked;
            panelTopBusiness.Visible = checkBoxBusinessMode.Checked;
            panel24.Visible = !checkBoxBusinessMode.Checked;
            panel30.Visible = !checkBoxBusinessMode.Checked;
            panel13.Visible = !checkBoxBusinessMode.Checked;
            panelBusinessResults.Visible = checkBoxBusinessMode.Checked;
            panelBusinessInProgress.Visible = checkBoxBusinessMode.Checked;
            BUSINESSMODE = checkBoxBusinessMode.Checked;
            



            //TabControl tabControlBusiness = tabControl1;
            if (checkBoxBusinessMode.Checked)
            {
                
                tabControl1.TabPages.Remove(tabPageresults);
                tabControl1.TabPages.Remove(MESSENGERR);
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Insert(2,tabPage5);
                checkBoxBusinessMode.BackColor = Color.Crimson;
                checkBoxBusinessMode.ForeColor = Color.White;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox9.Checked = false;
                //tabControl1.Visible = !checkBoxBusinessMode.Checked;
                //tabControlBusiness.Visible = checkBoxBusinessMode.Checked;
                //this.Controls.Add(tabControlBusiness);
                //tabControlBusiness.BringToFront();
            }
            else
            {
                //this.Controls.Remove(tabControlBusiness);
                checkBoxBusinessMode.BackColor = Color.White;
                checkBoxBusinessMode.ForeColor = Color.Black;
                tabControl1.TabPages.Insert(1, tabPageresults);
                tabControl1.TabPages.Insert(3, tabPage2);
                tabControl1.TabPages.Insert(4, MESSENGERR);
                tabControl1.TabPages.Remove(tabPage5);
                tabControl1.Visible = true;
                tabControl1.BringToFront();
                this.Controls.Add(tabControl1);
                tabControl1.BringToFront();
            }

        }

        private void checkBoxBusinessFastPictures_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxBusinessPictures.Checked = checkBoxBusinessFastPictures.Checked;
        }

        private void checkBoxBusinessFastJournal_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxBusinessJournal.Checked = checkBoxBusinessFastJournal.Checked;
            FASTJOURNALFORBUSINESS = checkBoxBusinessFastJournal.Checked;
        }
        private void FillPicturesViewForBusiness()
        {
            if (!Directory.Exists(pathToSave + "\\PICTURES"))
                return;


            var sorted = Directory.GetFiles(pathToSave + "\\PICTURES", "*.png").Select(fn => new FileInfo(fn)).Where(f => f.Name.StartsWith("Element")).OrderBy(f => f.LastWriteTime);
            fichiersImagess = sorted.ToArray();

            dataGridView3.Rows.Clear();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridView3.Size.Height;
            int hauteurForm = JOURNAL.Size.Height;

            int indexPage = 0;
            Previous = 0;

            foreach (FileInfo fichier in fichiersImagess)
            {

                if (indexPage == STEPP)
                    break;
                Image tmp = Image.FromFile(fichier.FullName);
                int differentiel = tmp.Width - tmp.Height;
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width - 119, hauteurForm - 120)));
                //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                dataGridView3.Rows.Add(imgg);


                indexPage++;
            }

            Next = STEPP;
            label18.Text = fichiersImagess.Length + " screenshots";
            //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
            label26.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersImagess.Length - 1).ToString());



        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {
            FillPicturesViewForBusiness();
        }

        private void pictureBox53_Click(object sender, EventArgs e)
        {
            if (Next > fichiersImagess.Length)
                return;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridView3.Rows.Clear();


            for (int i = Next; i < Next + STEPP; i++)
            {

                if (i == Next + STEPP)
                    break;

                if (i > fichiersImagess.Length - 1)
                    break;

                FileInfo fichier = fichiersImagess[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView3.Size.Height;
                int hauteurForm = JOURNAL.Size.Height;

                try
                {

                    Image img = Image.FromFile(fichier.FullName);
                    Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width - 119, hauteurForm - 120)));
                    dataGridView3.Rows.Add(img);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + fichier.FullName);
                }

            }

            Next += STEPP;
            label26.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersImagess.Length - 1).ToString()).Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
        }

        private void pictureBox38_Click(object sender, EventArgs e)
        {
            if ((Next - (STEPP * 2)) < 0)
                return;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridView3.Rows.Clear();

            for (int i = (Next - (STEPP * 2)); i <= Next - STEPP; i++)
            {

                //if (i == Next - STEPP)
                //    break;

                FileInfo fichier = fichiersImagess[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView3.Size.Height;
                int hauteurForm = JOURNAL.Size.Height;

                Image img = Image.FromFile(fichier.FullName);
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width - 119, hauteurForm - 120)));
                dataGridView3.Rows.Add(imgg);



            }


            label26.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersImagess.Length - 1).ToString()).Replace("xx/", (Next - (STEPP * 2) + 0) + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

            Next -= STEPP;
        }

        private void dataGridView3_Scroll(object sender, ScrollEventArgs e)
        {
            string texteAGarder = label26.Text.Split('/')[1];
            string numero = label26.Text.Split('/')[0].Replace("_____", "");
            Int32.Parse(numero);
            int step = dataGridView3.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridView3.FirstDisplayedScrollingRowIndex;


            label26.Text = "_____" + (step + (Next - STEPP)) + "/" + texteAGarder;
        }

        private void contextMenuStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            //PictureBox destination = ((PictureBox)sender);

            PictureBox box = new PictureBox();
            box.Size = new Size(500, 500);
            box.Image = (Image)((DataGridView)sourceControl).SelectedRows[0].Cells[0].Value;
            //box.BorderStyle = BorderStyle.FixedSingle;
            box.Cursor = Cursors.Hand;
            box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            box.ContextMenuStrip = contextMenuStrip2;
            box.Name = "\\PICTURES\\ElementScreenshot_" + (((DataGridView)sourceControl).SelectedRows[0].Index + 1).ToString();
            flowLayoutPanelAnnexe.Controls.Add(box);
        }
    }
}
