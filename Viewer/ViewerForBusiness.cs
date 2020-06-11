using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Viewer

{
    public partial class ViewerForBusiness : Form
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
        public bool getMessenger = false;
        public bool getContactMessenger = false;
        Dictionary<string, string> dicoDestinatairesFromGrid = new Dictionary<string, string>();
        public bool selected = false;
        //public IList<string, string> listeForSearch = new List<string, string>();
        public List<string> sortedListForSearching = new System.Collections.Generic.List<string>();
        string pathConfig = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<int, int> dicoResultsIndexSearch = new Dictionary<int, int>();
        Dictionary<string, string> videos = new Dictionary<string, string>();
        Dictionary<string, string> audios = new Dictionary<string, string>();
        static int Next;
        static int NextMessenger;
        static int Previous;
        public int STEPP = 100;
        FileInfo[] fichiersJournal;
        FileInfo[] fichiersImages;
        FileInfo[] fichiers;
        Initcontrol _init;
        OvalPictureBox oval;

        // Copyright (c) 2020 All Rights Reserved
        // </copyright>
        // <author>Frank Bastin</author>
        public ViewerForBusiness(Initcontrol init)
        {
            Visible = false;
            InitializeComponent();
            
            
            _init = init;
            _init.Refresh();
            _init.Show();
            _init.Update();

            //System.Threading.Thread.Sleep(500);
            //dataGridViewMessenger
            //textBoxops.Select();
            GetResolutionScreen();
            //textBox1.Width = this.Width;

            var sorted = Directory.GetFiles(pathConfig, "*.fbv").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
            FileInfo[] fichiers = sorted.ToArray();

            Rectangle rect = GetResolutionScreen();
            //int hauteurGrid = dataGridView2.Size.Height;
            //int hauteurForm = MESSENGER.Size.Height;
            dataGridViewJournal.ContextMenuStrip = contextMenuStrip1;
            //dataGridViewPictures.ContextMenuStrip = contextMenuStrip3;
            
            dataGridView1.ContextMenuStrip = contextMenuStrip4;
            oval = new OvalPictureBox();
            oval.Anchor = System.Windows.Forms.AnchorStyles.Top;
            oval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            //oval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //oval.Image = global::Viewer.Properties.Resources.target2;
            oval.Location = new System.Drawing.Point(76, 44);
            oval.Name = "pictureboxtango";
            oval.Size = new System.Drawing.Size(150, 150);
            oval.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            oval.TabIndex = 8;
            oval.TabStop = false;
            oval.BringToFront();
            oval.Visible = true;

            tabPage2.Controls.Add(oval);
            

            foreach (FileInfo fichier in fichiers)
            {

                Import(fichier.FullName);

            }

            _init.GetProgressBar().Value += 4;
            _init.GetProgressBar().Refresh();
            //Thread.Sleep(100);

            //List<int> tabToBeRemoved = new List<int>();

            //if (dataGridView2.Rows.Count > 0)
            //{
            //    Image img = global::Viewer.Properties.Resources.friends;
            //    dataGridViewResume.Rows.Add(img,dataGridView2.Rows.Count, "AMIS");
            //    //labelFriends.Text = "AMIS : " + dataGridView2.Rows.Count;
            //}

            //if (dataGridView1.Rows.Count > 0)
            //{

            //    dataGridViewResume.Rows.Add(dataGridView1.Rows.Count, "ABONNES");
            //    //labelFriends.Text = "AMIS : " + dataGridView2.Rows.Count;
            //}

            //if(dataGridView1.Rows.Count == 0 && dataGridView2.Rows.Count ==0)
            //    tabControl1.Controls.Remove(tabControl1.Controls["AMIS"]);

            //else
            //{

            //    tabControl1.Controls.Remove(tabControl1.Controls["AMIS"]);
            //    //tabToBeRemoved.Add(1);
            //    labelFriends.Text = "AMIS / ABONNES : " + (dataGridView2.Rows.Count + dataGridView1.Rows.Count).ToString();

            //}

            //if (dataGridViewMessenger.Rows.Count > 0)
            //{
            //    Image img = global::Viewer.Properties.Resources.Messenger_icon;
            //    dataGridViewResume.Rows.Add(img,dataGridViewMessenger.Rows.Count, "MESSENGER");
            //    //labelMessenger.Text = "MESSENGER : " + dataGridViewMessenger.Rows.Count;
            //}
            //else
            //{
            //    tabControl1.Controls.Remove(tabControl1.Controls["MESSENGER"]);
            //    //tabToBeRemoved.Add(3);
            //    //labelMessenger.Text = "MESSENGER : " + dataGridViewMessenger.Rows.Count;

            //}

            if(GetNumberPictures() != "0")
            {
                Image img = global::Viewer.Properties.Resources.pictures_1320567790130713653_64;
                
                dataGridViewResume.Rows.Add(img,GetNumberPictures(), "IMAGES");
                //labelPictures.Text = "IMAGES : " + GetNumberPictures();
            }
            else
            {
                //labelPictures.Text = "IMAGES : " + GetNumberPictures();
                //tabToBeRemoved.Add(4);
                tabControl1.Controls.Remove(tabControl1.Controls["IMAGES"]);

            }
            

            if(GetNumberHomePage() != "0")
            {
                Image img = global::Viewer.Properties.Resources.document_131964752656203567_64;
                dataGridViewResume.Rows.Add(img,GetNumberHomePage(), "JOURNAL");
                //labelJournal.Text = "JOURNAL : " + GetNumberHomePage();

            }
            else
            {
                tabControl1.Controls.Remove(tabControl1.Controls["JOURNAL"]);
                //tabToBeRemoved.Add(2);
                //labelJournal.Text = "JOURNAL : " + GetNumberHomePage();

            }

            if (File.Exists(pathConfig + "\\AllIdentifiants.txt"))              
            {
                Image img = global::Viewer.Properties.Resources._141ab281;
                dataGridViewResume.Rows.Add(img,File.ReadAllLines(pathConfig + "\\AllIdentifiants.txt").Length, "IDENTIFIANTS");
                //labelIdentifiants.Text = "IDENTIFIANTS : " + File.ReadAllLines(pathConfig + "\\AllIdentifiants.txt").Length;

            }

            
            FillParametersImport(pathConfig);
            dataGridViewResume.ClearSelection();
            dataGridViewResume.Sort(dataGridViewResume.Columns[2], ListSortDirection.Ascending);

        }

        private void FillParametersImport(string pathConfig)
        {
            Dictionary<string, string> identifiants = new Dictionary<string, string>();
            //dataGridViewIdentifiants.Rows.Clear();

            int i = 1;

            //SessionsActives
            if (File.Exists(pathConfig + "\\PARAMETERS\\SessionsActives.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\SessionsActives.txt");
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

                labelParam.Text = "PARAMETRES : " + i++;
            }

            //Connexionsdeconnexions
            if (File.Exists(pathConfig + "\\PARAMETERS\\ConnexionsDeconnexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\ConnexionsDeconnexions.txt");
                foreach (string li in lines)
                {
                    richTextBoxConnexionsDeconnexions.Text += li + "\n";
                }

                labelParam.Text = "PARAMETRES : " + i++;
            }

            //Recherches
            if (File.Exists(pathConfig + "\\PARAMETERS\\Recherches.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Recherches.txt");
                foreach (string li in lines)
                {
                    richTextBoxRecherches.Text += li + "\n";
                }

                labelParam.Text = "PARAMETRES : " + i++;
            }
            //Connexions
            if (File.Exists(pathConfig + "\\PARAMETERS\\Connexions.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Connexions.txt");
                foreach (string li in lines)
                {
                    richTextBox1Connexions.Text += li + "\n";
                }

                labelParam.Text = "PARAMETRES : " + i++;
            }

            //Parametres
            if (File.Exists(pathConfig + "\\PARAMETERS\\Parametres.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\PARAMETERS\\Parametres.txt");
                foreach (string li in lines)
                {
                    richTextBoxparam.Text += li + "\n";
                }

                labelParam.Text = "PARAMETRES : " + i++;
            }

            if (i > 1)
            {
                Image img = global::Viewer.Properties.Resources.cogwheel_configuration_options_parameters_properties_settings_1320165735121041526_64;
                dataGridViewResume.Rows.Add(img,i - 1, "PARAMETRES");
            }
                
        }

        public void Import(string fichier)
        {
            
                    string sauvegarde = File.ReadAllText(fichier);

                    string[] ecase = sauvegarde.Substring(sauvegarde.IndexOf("<Case>\n") + 7, (sauvegarde.IndexOf("</Case>\n") - (sauvegarde.IndexOf("<Case>\n") + 7))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(';');
                    //string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24).Split(new string[] { "</DataGridViewMessenger>\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24, (sauvegarde.IndexOf("</DataGridViewMessenger>\n") - (sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewFriends = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewFriends>\n") + 21, (sauvegarde.IndexOf("</DataGridViewFriends>\n") - (sauvegarde.IndexOf("<DataGridViewFriends>\n") + 21))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewFollowers= sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewFollowers>\n") + 24, (sauvegarde.IndexOf("</DataGridViewFollowers>\n") - (sauvegarde.IndexOf("<DataGridViewFollowers>\n") + 24))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] pictureProfile = sauvegarde.Substring(sauvegarde.IndexOf("<PictureProfile>\n") + 16, (sauvegarde.IndexOf("</PictureProfile>\n") - (sauvegarde.IndexOf("<PictureProfile>\n") + 16))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    textBoxUSERNAMEFRIENDS.Text = ecase[0];
                    textBoxUSERNAME.Text = ecase[1];
                    textBoxPASSWORD.Text = ecase[2];
                    textBoxops.Text = ecase[3];
                    //try
                    //{
                    //    labelID.Text = ecase[5].Trim();
                    //    if (labelID.Text != "")
                    //    {
                    //        labelID.Visible = true;
                    //        labelID.Refresh();

                    //    }
                    //    else
                    //        labelID.Visible = false;
                    //}
                    //catch
                    //{
                    //    labelID.Visible = false;

                    //}
                    
                        

                    if(pictureProfile[0] != "label3")
                    {
                        try
                        {
                            oval.Image = Image.FromFile(pictureProfile[0]);
                            oval.BringToFront();
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }

                    //dataGridViewMessenger.Rows.Clear();
                    //foreach (string li in datagridviewmessenger)
                    //{
                    //    string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //    foreach (string lii in champ)
                    //    {
                    //        string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    //        Image img = Image.FromFile(champp[0]);
                    //        dataGridViewMessenger.Rows.Add(img, champp[1], champp[2], Boolean.Parse(champp[3].ToString()), champp[4]);
                    //    }

                    //}

                    //dataGridView2.Rows.Clear();
                    //foreach (string li in datagridviewFriends)
                    //{
                    //    string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //    foreach (string lii in champ)
                    //    {
                    //        string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    //        if(champp[0] != "anonymous")
                    //        {
                    //            Image img = Image.FromFile(champp[0]);
                    //            dataGridView2.Rows.Add(img, champp[1], champp[2], champp[3]);
                    //        }
                    //        else
                    //        {
                    //            Image img = global::Viewer.Properties.Resources.anonymous;
                    //            dataGridView2.Rows.Add(img, champp[1], champp[2], champp[3]);
                    //        }
                            
                    //    }

                    //}

                    //dataGridView1.Rows.Clear();
                    //foreach (string li in datagridviewFollowers)
                    //{
                    //    string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //    foreach (string lii in champ)
                    //    {
                    //        string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    //        if (champp[0] != "anonymous")
                    //        {
                    //            Image img = Image.FromFile(champp[0]);
                    //            dataGridView1.Rows.Add(img, champp[1], champp[2], champp[3]);
                    //        }
                    //        else
                    //        {
                    //            Image img = global::Viewer.Properties.Resources.anonymous;
                    //            dataGridView1.Rows.Add(img, champp[1], champp[2], champp[3]);
                    //        }

                    //    }

                    //}


            FillIdentifiants();



        }
        private void button1_Click(object sender, EventArgs e)
        {
            //FindAllFriendsFromFacebookBis(textBoxUSERNAMEFRIENDS.Text);
            //GetAllCommentsForTest(dateTimePicker1.Value.Year, dateTimePicker2.Value.Year);
            //check datetimepicker pour les commentaires

           
        }

        public string GetNumberPictures()
        {
            if(Directory.Exists(pathConfig + "\\PICTURES"))
            {
                return Directory.GetFiles(pathConfig + "\\PICTURES","*.png").Count().ToString();
            }

            return "0";
        }

        public string GetNumberHomePage()
        {
            if (Directory.Exists(pathConfig + "\\HOMEPAGE"))
            {
                return Directory.GetFiles(pathConfig + "\\HOMEPAGE", "*.png").Count().ToString();
            }

            return "0";
        }
        private void button1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            StopProcess();
            
        }
        [System.Runtime.InteropServices.DllImport("ole32.dll")]
        static extern void CoFreeUnusedLibraries();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

           
            // while (true)
            //{
            while (!backgroundWorker1.CancellationPending)
                {

                             


                if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        Reset();

                        backgroundWorker1.Dispose();
                        GC.Collect();

                        //FB : This code kill all the ressources no more read memory error
                        System.Windows.Forms.Application.ExitThread();
                        System.Windows.Forms.Application.DoEvents();
                        CoFreeUnusedLibraries();
                        this.Refresh();
                        break;
                    }
            //}
        }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // First call, the percentage is negative to signal that UserState
            // contains the number of pages we loop on....
            
                        
            


        }

       

        private void StopProcess()
        {

            backgroundWorker1.CancelAsync();
            backgroundWorker1.Dispose();
            backgroundWorker1 = null;

            try
            {
                
            }
            catch
            {

            }


            GC.Collect();

            

            


        }
        private void Reset()
        {
                        

            friends = false;
            comments = false;
            allimages = false;
            

        }

       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");

            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }

            try
            {
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
       

        public void IsANewThread()
        {
            if (backgroundWorker1 != null && backgroundWorker1.IsBusy)
                return;

               else
                    if (backgroundWorker1 != null)
                        backgroundWorker1.RunWorkerAsync();
                    else
                    {
                        //Reset();

                        backgroundWorker1 = new BackgroundWorker();

                        backgroundWorker1.WorkerReportsProgress = true;

                        backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

                        backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

                        backgroundWorker1.WorkerSupportsCancellation = true;
                        backgroundWorker1.RunWorkerAsync();
                    }
        }

        

        private void tabPageStart_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
            //return;
            textBoxops.Focus();

            //Thread.Sleep(5000);
            try
            {
                FillJournalView();
                //Thread.Sleep(500);
                _init.GetProgressBar().Value += 1;
                _init.GetProgressBar().Update();
                //Thread.Sleep(500);

                FillPicturesView();
                //Thread.Sleep(500);
                _init.GetProgressBar().Value += 1;
                _init.GetProgressBar().Update();

                foreach (Form f in Application.OpenForms)
                {

                    if (f.Name == "Initcontrol")
                    {
                        
                        f.Close();


                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }

                }
                this.Opacity = 100;
                this.Show();
            }
            catch
            {
                foreach (Form f in Application.OpenForms)
                {

                    if (f.Name == "Initcontrol")
                    {

                        f.Close();


                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }

                }
                this.Opacity = 100;
                this.Show();
            }
            

        }

        private void FillPicturesView()
        {
            if (!Directory.Exists(pathConfig + "\\PICTURES"))
                return;

            
            var sorted = Directory.GetFiles(pathConfig + "\\PICTURES", "*.png").Select(fn => new FileInfo(fn)).Where(f => f.Name.StartsWith("Element")).OrderBy(f => f.LastWriteTime);
            fichiersImages = sorted.ToArray();

            dataGridView1.Rows.Clear();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridView1.Size.Height;
            int hauteurForm = IMAGES.Size.Height;

            int indexPage = 0;
            Previous = 0;

            foreach (FileInfo fichier in fichiersImages)
            {

                if (indexPage == STEPP)
                    break;
                Image tmp = Image.FromFile(fichier.FullName);
                int differentiel = tmp.Width - tmp.Height;
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width - 119, hauteurForm - 168)));
                //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                dataGridView1.Rows.Add(imgg);


                indexPage++;
            }

            Next = STEPP;
            label2.Text = fichiersImages.Length + " screenshots";
            //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
            label3.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersImages.Length - 1).ToString());



        }

        private void pictureBoxPicture_Doubleclick(object sender, EventArgs e)
        {
            Process.Start(pathConfig + "\\PICTURES\\" + ((PictureBox)sender).Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }


        //private void FillDataGridViewMessenger(Image image, string username, string link, string pathToPictureProfile)
        //{
        //    dataGridViewMessenger.Rows.Add(image, username, link,false,"",pathToPictureProfile);
        //}

        //private void FillDataGridViewMessenger(string pathToFolder, string link)
        //{
            
        //    foreach(DataGridViewRow row in dataGridViewMessenger.Rows)
        //    {
        //        if(row.Cells[2].Value.ToString() == link)
        //        {
        //            row.Cells[3].Value = false;
        //            row.Cells[4].Value = pathToFolder;
        //            row.DefaultCellStyle.BackColor = Color.LightSkyBlue;

        //            dataGridViewMessenger.ClearSelection();
        //            //dataGridViewPictures.Rows[rowIndex].Selected = true;
        //            //dataGridViewMessenger.FirstDisplayedScrollingRowIndex = row.Index;
                    

        //            dataGridViewMessenger.Focus();

        //            break;
        //        }
        //    }

        //    dataGridViewMessenger.Refresh();


        //}

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxwaiting_Click(object sender, EventArgs e)
        {

        }

        //private void InitializeDatagridViewMessenger()
        //{
        //    getMessenger = true;
            
        //}

        //private void dataGridViewMessenger_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex == 3)
        //    {
        //        //Reference the GridView Row.
        //        DataGridViewRow row = dataGridViewMessenger.Rows[e.RowIndex];


                
        //            row.Cells[3].Value = !(bool)row.Cells[3].Value;
                    
                                   
                
        //    }
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    selected = !selected;
            
        //    foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
        //    {
                
        //        if (selected)
        //        {
        //            row.Cells[3].Value = false;
        //        }

        //        if (!selected)
        //        {
        //            row.Cells[3].Value = true;
        //        }
        //    }

            
        //}

        //private void pictureBox4_Click(object sender, EventArgs e)
        //{

        //    if (textBoxops.Text == "")
        //    {
        //        textBoxops.BackColor = Color.Red;
        //        MessageBox.Show("Veuillez remplir le champ OPS");
        //        return;
        //    }
        //    else
        //        textBoxops.BackColor = Color.White;


        //    Dictionary<string, string> dicoDestinatairesFromGrid = new Dictionary<string, string>();

        //    if(dataGridViewMessenger.Rows.Count > 0)
        //    {
        //        dataGridViewPictures.Rows.Clear();
                
        //        foreach (DataGridViewRow row in dataGridViewMessenger.Rows)
        //        {
        //            bool isSelected = Convert.ToBoolean(row.Cells[3].Value);
        //            if (isSelected)
        //            {
        //               if(!dicoDestinatairesFromGrid.ContainsKey(row.Cells[2].Value.ToString())) 
        //                dicoDestinatairesFromGrid.Add(row.Cells[2].Value.ToString(), row.Cells[1].Value.ToString());                        
                        
        //            }
                    
        //        }

        //    }

           
                
        //}

        //private void dataGridViewMessenger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
            
        //    if (dataGridViewMessenger.Rows.Count > 0 )//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
        //    {

        //        panel5.Visible = false;
        //        flowLayoutPanel2.Controls.Clear();
        //        panelNext.Visible = false;

        //        if (sortedListForSearching != null || sortedListForSearching.Count > 0)
        //            sortedListForSearching.Clear();


        //        videos = new Dictionary<string, string>();
        //        audios = new Dictionary<string, string>();
        //        string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
        //        dataGridViewPictures.Rows.Clear();
        //        var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
        //        fichiers = sorted.ToArray();

        //        Rectangle rect = GetResolutionScreen();
        //        int hauteurGrid = dataGridView2.Size.Height;
        //        int hauteurForm = MESSENGER.Size.Height;
        //        int indexPage = 0;
                

        //        foreach (FileInfo fichier in fichiers)
        //        {

        //            if (indexPage == STEPP)
        //                break;
        //            Image tmp = Image.FromFile(fichier.FullName);
        //            int differentiel = tmp.Width - tmp.Height;
        //            Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm - (120 - differentiel), hauteurForm - 120)));

        //            //Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm-84, hauteurForm - 85)));
        //            dataGridViewPictures.Rows.Add(imgg,fichier.Name);


        //            indexPage++;
        //        }

        //        NextMessenger = STEPP;
                
        //        //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
        //        labelNbreMessenger.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString());

        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");

                  
        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];


        //                int indexx = 0;
        //                Int32.TryParse(numeroLigne, out indexx);
                        

        //                if (!videos.ContainsKey(((indexx ).ToString())))
        //                {
        //                    //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
        //                    videos.Add(((indexx ).ToString()), ((indexx ).ToString()));
        //                    //videos.Add(((indexx - 1).ToString(), ((indexx - 1).ToString()));
        //                }
                        

                        
        //            }
        //        }

        //        if (videos.Count > 0)
        //        {
        //            if (videos.ContainsKey((dataGridViewPictures.FirstDisplayedScrollingRowIndex).ToString()))
        //            {
        //                pictureBox4.Visible = true;
        //                panel5.Visible = true;
        //            }

        //            else
        //            {
        //                pictureBox4.Visible = false;
        //                panel5.Visible = false;
        //            }

        //        }

        //        if (File.Exists(test + "\\Messenger_Audio_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Audio_With_Screenshots.txt");


        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];


        //                int indexx = 0;
        //                Int32.TryParse(numeroLigne, out indexx);


        //                if (!audios.ContainsKey(((indexx).ToString())))
        //                {
        //                    //dataGridViewPictures.Rows[indexx - 1].DefaultCellStyle.BackColor = Color.LightBlue;
        //                    audios.Add(((indexx).ToString()), ((indexx).ToString()));
        //                    //videos.Add(((indexx - 1).ToString(), ((indexx - 1).ToString()));
        //                }



        //            }
        //        }

        //        if (audios.Count > 0)
        //        {
        //            if (audios.ContainsKey((dataGridViewPictures.FirstDisplayedScrollingRowIndex).ToString()))
        //            {
        //                pictureBox4.Visible = true;
        //                pictureBox10.Visible = true;
        //                panel5.Visible = true;
        //            }

        //            else
        //            {
        //                pictureBox4.Visible = false;
        //                pictureBox10.Visible = false;
        //                panel5.Visible = false;
        //            }

        //        }

        //    }
            
        //}

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        //private void button4_Click(object sender, EventArgs e)
        //{

        //    //string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
        //    string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Henri Dewyse";
        //    dataGridViewPictures.Rows.Clear();
        //    var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
        //    FileInfo[] fichiers = sorted.ToArray();

        //    Rectangle rect = GetResolutionScreen();
        //    int hauteurGrid = dataGridView2.Size.Height + 15;
        //    int hauteurForm = MESSENGER.Size.Height;

        //    foreach (FileInfo fichier in fichiers)
        //    {
        //        //PictureBox imageViewer = new PictureBox();
        //        //imageViewer.Image = Image.FromFile(fichier.FullName);
        //        //imageViewer.SizeMode = PictureBoxSizeMode.Normal;
        //        //imageViewer.Dock = DockStyle.Bottom;
        //        //imageViewer.Height = 1250;
        //        //imageViewer.Width = 1250;
        //        Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
        //        dataGridViewPictures.Rows.Add(img, fichier.Name);
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

        //private void button4_Click_1(object sender, EventArgs e)
        //{
        //    SearchKeywords(textBox1.Text);
        //}

        //private void SearchKeywords(string keyword)
        //{
        //    if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
        //    {
        //        dicoResultsIndexSearch = new Dictionary<int, int>();
        //        flowLayoutPanel2.Controls.Clear();


        //        string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Messages_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Messages_With_Screenshots.txt");

        //            if (sortedListForSearching.Count == 0)
        //            {
        //                foreach (string li in lignes)
        //                {
        //                    if (li == "")
        //                        continue;

        //                    string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                    sortedListForSearching.Add(li.ToLower());


        //                }

        //                try
        //                {

        //                    List<string> resultatsRecherche = sortedListForSearching.FindAll(x => x.Contains(keyword));

        //                    foreach (string r in resultatsRecherche)
        //                    {
        //                        string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //                        int indexx = Int32.Parse(numeroLigne) - 1;
        //                        string arechercher = r.Split(';')[0];

        //                        if (!arechercher.ToLower().Contains(keyword.ToLower()))
        //                            continue;

        //                        if (!dicoResultsIndexSearch.ContainsKey(indexx))
        //                        {
        //                            dicoResultsIndexSearch.Add(indexx, indexx);
        //                            LinkLabel link = new LinkLabel();
        //                            //link.Text = numeroLigne;
        //                            link.Text = (Int32.Parse(numeroLigne) - 1).ToString();
        //                            link.AutoSize = true;
        //                            link.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //                            //link.Name = "linkLabel" + indexx;
        //                            link.Name = "linkLabel" + (indexx - 1);
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

        //                    if (dicoResultsIndexSearch.Count > 0)
        //                    {
        //                        //dataGridViewPictures.ClearSelection();
        //                        dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
        //                        dataGridViewPictures.Focus();
        //                        flowLayoutPanel2.Visible = true;
        //                        panelNext.Visible = true;

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
        //                        string numeroLigne = (r.Split(';')[1]).Substring(r.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
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

        //                    if (dicoResultsIndexSearch.Count > 0)
        //                    {
        //                        //dataGridViewPictures.ClearSelection();
        //                        dataGridViewPictures.FirstDisplayedScrollingRowIndex = dicoResultsIndexSearch.ElementAt(0).Value;
        //                        dataGridViewPictures.Focus();

        //                        flowLayoutPanel2.Visible = true;
        //                        panelNext.Visible = true;
        //                    }
        //                }
        //                catch (ArgumentNullException ex)
        //                {
        //                    return;
        //                }
        //            }

                    
        //        }
        //    }
        //}
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

                if (!Directory.Exists(pathConfig + "\\HOMEPAGE"))
                    return;

                //var sorted = Directory.GetFiles(pathConfig + "\\HOMEPAGE", "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.LastWriteTime);
                //fichiersJournal = sorted.ToArray();

                
                if (File.Exists(pathConfig + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(pathConfig + "\\HOMEPAGE\\HomepageComments_With_Screenshots.txt");

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

        private void button4_Click_2(object sender, EventArgs e)
        {

        }

        //private void pictureBox5_Click(object sender, EventArgs e)
        //{

        //    if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0)
        //    {
        //        string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Messages.txt"))
        //        {
        //            Messages msg = new Messages();
        //            msg.SetRichTextBox(File.ReadAllText(test + "\\Messenger_Messages.txt"));
        //            msg.Show();

        //        }



                
        //    }
        //}

        ////private void button18_Click(object sender, EventArgs e)
        //{
        //    SaveCase();
        //}


        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { ValidateNames = true, Multiselect = false, Filter = "Text Document|*.txt" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string sauvegarde = File.ReadAllText(ofd.FileName);

                    string[] ecase = sauvegarde.Substring(sauvegarde.IndexOf("<Case>\n") + 7, (sauvegarde.IndexOf("</Case>\n") - (sauvegarde.IndexOf("<Case>\n") + 7))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(';');
                    //string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24).Split(new string[] { "</DataGridViewMessenger>\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] datagridviewmessenger = sauvegarde.Substring(sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24, (sauvegarde.IndexOf("</DataGridViewMessenger>\n") - (sauvegarde.IndexOf("<DataGridViewMessenger>\n") + 24))).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //textBoxUSERNAMEFRIENDS.Text = ecase[0];
                    //textBoxUSERNAME.Text = ecase[1];
                    //textBoxPASSWORD.Text = ecase[2];
                    textBoxops.Text = ecase[3];

                    //dataGridViewMessenger.Rows.Clear();
                    //foreach (string li in datagridviewmessenger)
                    //{
                    //    string[] champ = li.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //    foreach (string lii in champ)
                    //    {
                    //        string[] champp = lii.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    //        Image img = Image.FromFile(champp[0]);
                    //        dataGridViewMessenger.Rows.Add(img, champp[1], champp[2], Boolean.Parse(champp[3].ToString()), champp[4], champp[5]);
                    //    }

                    //}



                }
            }
        }

        //private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    Process.Start(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    if (textBox1.Text == "")
        //    {
        //        flowLayoutPanel2.Controls.Clear();
        //        panelNext.Visible = false;
              
        //        return;
        //    }
                
        //    SearchKeywords(textBox1.Text);
        //}

        //private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    dataGridViewPictures.ClearSelection();
        //    dataGridViewPictures.FirstDisplayedScrollingRowIndex = Int32.Parse(((LinkLabel)sender).Text) - 1;
        //    dataGridViewPictures.Focus();

        //    ((LinkLabel)sender).ForeColor = Color.Red;
        //}

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
        //private void dataGridViewPictures_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (panel5.Visible == false)
        //        return;

        //    if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
        //    {

        //        string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
        //        Dictionary<string, string> videos = new Dictionary<string, string>();
        //        Dictionary<string, string> audios = new Dictionary<string, string>();

        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");
        //            string nomPng = dataGridViewPictures.Rows[e.RowIndex].Cells[1].Value.ToString();
        //            string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];


        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        

        //                if (numeroLigne == numeroVideo)
        //                {
        //                    string fichier = li.Split(';')[0];
        //                    string nomFichier = fichier.Substring(fichier.LastIndexOf('/') + 1).Split('?')[0];

        //                    if (Directory.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\"))
        //                    {
        //                        string repertoire = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\";

        //                        if (File.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier))
        //                        {
        //                            string fichierr = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier;

        //                            if (!videos.ContainsKey(fichierr))
        //                                videos.Add(fichierr, fichierr);

        //                            //Process.Start(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier);
        //                        }
        //                    }

        //                }



        //            }

        //            if (videos.Count() > 1)
        //            {
        //                //Form videoss = new FormVideos();


        //                //FlowLayoutPanel panel = new FlowLayoutPanel();
        //                //panel.FlowDirection = FlowDirection.LeftToRight;
        //                //panel.AutoScroll = true;
        //                //panel.Dock = DockStyle.Fill;

        //                int i = 0;
        //                foreach (string fichier in videos.Values)
        //                {
        //                    //PictureBox box = new PictureBox();
        //                    //box.Size = new Size(400, 400);
        //                    //box.BorderStyle = BorderStyle.FixedSingle;
        //                    //box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        //                    //box.Image = global::FacebookAnalyzer.Properties.Resources.target2;

        //                    //panel.Controls.Add(box)
        //                    using (Process myProcess = new Process())
        //                    {
        //                        // myProcess.StartInfo.UseShellExecute = false;
        //                        // You can start any process, HelloWorld is a do-nothing example.
        //                        myProcess.StartInfo.FileName = videos.ElementAt(i).Value;
        //                        //myProcess.StartInfo.CreateNoWindow = true;
        //                        myProcess.Start();
        //                        // This code assumes the process you are starting will terminate itself.
        //                        // Given that is is started without a window so you cannot terminate it
        //                        // on the desktop, it must terminate itself or you can do it programmatically
        //                        // from this application using the Kill method.
        //                    }

        //                    //Process.Start(videos.ElementAt(i).Value);
        //                    //Thread.Sleep(2000);

        //                    i++;

        //                }

        //                //videoss.Controls.Add(panel);
        //                //videoss.Show();



        //            }
        //            else
        //            if (videos.Count == 1)
        //            {
        //                Process.Start(videos.ElementAt(0).Value);
        //            }






        //        }

        //        if (File.Exists(test + "\\Messenger_Audio_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Audio_With_Screenshots.txt");
        //            string nomPng = dataGridViewPictures.Rows[e.RowIndex].Cells[1].Value.ToString();
        //            string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];


        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];


        //                if (numeroLigne == numeroVideo)
        //                {
        //                    string fichier = li.Split(';')[0];
        //                    string nomFichier = fichier.Substring(fichier.LastIndexOf("\\") + 1).Split('?')[0];

        //                    if (Directory.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Audio\\"))
        //                    {
        //                        string repertoire = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Audio\\";

        //                        if (File.Exists(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Audio\\" + nomFichier))
        //                        {
        //                            string fichierr = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Audio\\" + nomFichier;

        //                            if (!audios.ContainsKey(fichierr))
        //                                audios.Add(fichierr, fichierr);

        //                            //Process.Start(dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString() + "\\Videos\\" + nomFichier);
        //                        }
        //                    }

        //                }



        //            }

        //            if (audios.Count() > 0)
        //            {
        //                FormVideos videoss = new FormVideos();
        //                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));


        //                int i = 0;
        //                string duree = "";
        //                string datum = "";
        //                foreach (string fichier in audios.Values)
        //                {
                            
        //                    if (fichier.Contains("_duree_"))
        //                    {
        //                        duree = fichier.Substring(fichier.IndexOf("_duree_") + 7).Split('_')[0].Replace("-", ":");
        //                        if (duree.StartsWith("00:0"))
        //                            duree = duree.Substring(duree.IndexOf("00:0") + 4);
        //                    }

        //                    Panel panelBulle = new Panel();

        //                    if (fichier.Contains("From"))
        //                    {
        //                        panelBulle.BackgroundImage = global::Viewer.Properties.Resources.bullegrise;
        //                        datum = fichier.Substring(fichier.IndexOf("From_") + 5).Split('_')[0].Replace("-", ":");
        //                    }
        //                    else
        //                    {
        //                        panelBulle.BackgroundImage = global::Viewer.Properties.Resources.bullebleue;
        //                        datum = fichier.Substring(fichier.IndexOf("To_") + 3).Split('_')[0].Replace("-", ":");
        //                    }
                                

                           
        //                    panelBulle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        //                    panelBulle.Location = new System.Drawing.Point(6, 10);
        //                    panelBulle.Name = audios.ElementAt(i).Value;
        //                    panelBulle.Size = new System.Drawing.Size(220, 120);
        //                    panelBulle.TabIndex = 2021;
        //                    panelBulle.Visible = true;
        //                    panelBulle.Click += new System.EventHandler(this.pictureBox10_Click);

        //                    Label label = new Label();
        //                    label.Location = new System.Drawing.Point(50, 54);
        //                    label.Text = datum + " " + duree;
        //                    label.AutoSize = true;
        //                    label.Size = new System.Drawing.Size(30, 15);
        //                    label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //                    | System.Windows.Forms.AnchorStyles.Right)));
        //                    panelBulle.Controls.Add(label);



        //                    videoss.GetPanell().Controls.Add(panelBulle);


                          

        //                    i++;

        //                }

        //                //videoss.Controls.Add(panel);
        //                videoss.Show();



        //            }
        //            //else
        //            //if (audios.Count == 1)
        //            //{
        //            //    Process.Start(audios.ElementAt(0).Value);
        //            //}






        //        }
        //    }
        //}

        //private void dataGridViewPictures_Scroll(object sender, ScrollEventArgs e)
        //{


        //    foreach (Form f in Application.OpenForms)
        //    {

        //        if (f.Name == "FormVideos")
        //        {

        //            f.Close();
        //            break;
        //        }

        //    }

        //    if (videos.Count > 0)
        //    {

        //        string nomPng = dataGridViewPictures.Rows[dataGridViewPictures.FirstDisplayedScrollingRowIndex].Cells[1].Value.ToString();
        //        string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];

        //        if (videos.ContainsKey(numeroVideo))
        //        {
        //            pictureBox4.Visible = true;
        //            panel5.Visible = true;
        //        }

        //        else
        //        {
        //            pictureBox4.Visible = false;
        //            panel5.Visible = false;
        //        }
                    
        //    }

        //    if (audios.Count > 0)
        //    {

        //        string nomPng = dataGridViewPictures.Rows[dataGridViewPictures.FirstDisplayedScrollingRowIndex].Cells[1].Value.ToString();
        //        string numeroVideo = nomPng.Substring(nomPng.LastIndexOf("_") + 1).Split('.')[0];

        //        if (audios.ContainsKey(numeroVideo))
        //        {
        //            //pictureBox4.Visible = true;
        //            pictureBox10.Visible = true;
        //            panel5.Visible = true;
        //        }

        //        else
        //        {
        //            //pictureBox4.Visible = false;
        //            pictureBox10.Visible = false;
        //            //panel5.Visible = false;
        //        }

        //    }

        //    string texteAGarder = labelNbreMessenger.Text.Split('/')[1];
        //    string numero = labelNbreMessenger.Text.Split('/')[0].Replace("_____", "");
        //    Int32.Parse(numero);
        //    int step = dataGridViewPictures.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridViewPictures.FirstDisplayedScrollingRowIndex;


        //    labelNbreMessenger.Text = "_____" + (step + (NextMessenger - STEPP)) + "/" + texteAGarder;
        //}

        private void FillJournalView()
        {
            if (!Directory.Exists(pathConfig + "\\HOMEPAGE"))
                return;

            var sorted = Directory.GetFiles(pathConfig + "\\HOMEPAGE", "*.png").Select(fn => new FileInfo(fn)).Where(f => f.Name.StartsWith("Element")).OrderBy(f => f.LastWriteTime);
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
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(tmp.Width - 119, hauteurForm - 120)));
                //Image img = CreateThumbnail(fichier.FullName, hauteurForm - 79, hauteurForm - 120);

                dataGridViewJournal.Rows.Add(imgg);


                indexPage++;
                }

                Next = STEPP;
            nbreScreenshots.Text = fichiersJournal.Length + " screenshots";
            //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
            deA.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiersJournal.Length-1).ToString());





        }
        private void FillIdentifiants()
        {
            Dictionary<string, string> identifiants = new Dictionary<string, string>();

            //Friends
            if(File.Exists(pathConfig + "\\friends.txt"))
            {
                string[] lines = File.ReadAllLines(pathConfig + "\\friends.txt");
                foreach(string li in lines)
                {
                    if(!identifiants.ContainsKey(li.Split(';')[2]))
                    {
                        identifiants.Add(li.Split(';')[2], li);
                    }
                }
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

            if(identifiants.Count > 0)
            {
                foreach(string li in identifiants.Values)
                {
                    dataGridViewIdentifiants.Rows.Add(li.Split(';')[0], li.Split(';')[1], li.Split(';')[2]);
                }
            }

            //if(!File.Exists(pathConfig + "\\AllIdentifiants.txt"))
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

        private void button1_Click_1(object sender, EventArgs e)
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

                    Image img = Image.FromFile(fichier.FullName);
                    Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width-119, hauteurForm - 120)));
                    dataGridViewJournal.Rows.Add(img);
                    

                }
               catch(Exception ex)
                {
                    MessageBox.Show("error " + fichier.FullName);
                }

            }

            Next += STEPP;
            deA.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersJournal.Length-1).ToString()).Replace("xx/",(0 + (Next - STEPP)).ToString() + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
        }

        private void button2_Click_1(object sender, EventArgs e)
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

            for (int i = (Next - (STEPP * 2)); i <= Next - STEPP ; i++)
            {

                //if (i == Next - STEPP)
                //    break;

                FileInfo fichier = fichiersJournal[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridViewJournal.Size.Height;
                int hauteurForm = JOURNAL.Size.Height;

                Image img = Image.FromFile(fichier.FullName);
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width-119, hauteurForm - 120)));
                dataGridViewJournal.Rows.Add(imgg);

                

            }

            
            deA.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersJournal.Length-1).ToString()).Replace("xx/", (Next - (STEPP * 2) + 0) + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

            Next -= STEPP;
        }

        private void dataGridViewJournal_Scroll(object sender, ScrollEventArgs e)
        {

            string texteAGarder = deA.Text.Split('/')[1];
            string numero = deA.Text.Split('/')[0].Replace("_____","");
            Int32.Parse(numero);
            int step = dataGridViewJournal.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridViewJournal.FirstDisplayedScrollingRowIndex;


            deA.Text = "_____" + (step + (Next - STEPP) ) + "/" + texteAGarder;
            //deA.Text = deA.Text.Replace(texteAremplacer, dataGridViewJournal.FirstDisplayedScrollingRowIndex.ToString() + "/");



            //scroll down
            //if(e.NewValue > e.OldValue && (dataGridViewJournal.FirstDisplayedScrollingRowIndex + 1) % STEPP == 0)
            //{

            //    //FileInfo fichier = fichiersJournal[e.NewValue + 2];
            //    //Rectangle rect = GetResolutionScreen();
            //    //int hauteurGrid = dataGridViewJournal.Size.Height;
            //    //int hauteurForm = tabPage3.Size.Height;

            //    //Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
            //    //dataGridViewJournal.Rows.Add(img);   

            //    //dataGridViewJournal.FirstDisplayedScrollingRowIndex = e.NewValue;
            //    button1_Click_1(sender, e);
            //}

            ////Scroll up
            //if (e.NewValue < e.OldValue && (dataGridViewJournal.FirstDisplayedScrollingRowIndex == 0))
            //{

            //    button2_Click_1(sender, e);
            //    dataGridViewJournal.ClearSelection();               
            //    dataGridViewJournal.Rows[STEPP - 1].Selected = true;
            //    dataGridViewJournal.Focus();
            //}

            //if ( (dataGridViewJournal.FirstDisplayedScrollingRowIndex + 1) % STEPP == 0)
            //{


            //}


            //if ((dataGridViewJournal.FirstDisplayedScrollingRowIndex == 0))

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SearchKeywordsForHomepage(textBox2.Text);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            FillJournalView();
        }

        //private void pictureBox8_Click(object sender, EventArgs e)
        //{
        //    if ((NextMessenger - (STEPP * 2)) < 0)
        //        return;

        //    foreach (DataGridViewRow row in dataGridViewPictures.Rows)
        //    {
        //        Image img = (Image)row.Cells[0].Value;
        //        img.Dispose();


        //    }
        //    GC.Collect();

        //    dataGridViewPictures.Rows.Clear();

        //    for (int i = (NextMessenger - (STEPP * 2)); i <= NextMessenger - STEPP; i++)
        //    {

        //        //if (i == Next - STEPP)
        //        //    break;

        //        FileInfo fichier = fichiers[i];

        //        Rectangle rect = GetResolutionScreen();
        //        int hauteurGrid = dataGridViewPictures.Size.Height;
        //        int hauteurForm = MESSENGER.Size.Height;


        //        Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm-84, hauteurForm - 85)));
        //        dataGridViewPictures.Rows.Add(img,fichier.Name);



        //    }


        //    labelNbreMessenger.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString()).Replace("xx/", (NextMessenger - (STEPP * 2) + 0) + "/");
        //    //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

        //    NextMessenger -= STEPP;
        //}

        //private void pictureBox9_Click(object sender, EventArgs e)
        //{
        //    if (NextMessenger > fichiers.Length)
        //        return;

        //    foreach (DataGridViewRow row in dataGridViewPictures.Rows)
        //    {
        //        Image img = (Image)row.Cells[0].Value;
        //        img.Dispose();


        //    }
        //    GC.Collect();

        //    dataGridViewPictures.Rows.Clear();


        //    for (int i = NextMessenger; i < NextMessenger + STEPP; i++)
        //    {

        //        if (i == NextMessenger + STEPP)
        //            break;

        //        if (i > fichiers.Length - 1)
        //            break;

        //        FileInfo fichier = fichiers[i];

        //        Rectangle rect = GetResolutionScreen();
        //        int hauteurGrid = dataGridViewPictures.Size.Height;
        //        int hauteurForm = MESSENGER.Size.Height;

        //        try
        //        {
        //            Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm-84, hauteurForm - 85)));
                   
        //            dataGridViewPictures.Rows.Add(img,fichier.Name);


        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("error " + fichier.FullName);
        //        }

        //    }

        //    NextMessenger += STEPP;
        //    labelNbreMessenger.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString()).Replace("xx/", (0 + (NextMessenger - STEPP)).ToString() + "/");
        //    //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
        //}

        //private void pictureBox7_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewMessenger.Rows.Count > 0)//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
        //    {

        //        panel5.Visible = false;

        //        if (sortedListForSearching != null || sortedListForSearching.Count > 0)
        //            sortedListForSearching.Clear();


        //        videos = new Dictionary<string, string>();
        //        string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();
        //        dataGridViewPictures.Rows.Clear();
        //        var sorted = Directory.GetFiles(test, "*.png").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
        //        fichiers = sorted.ToArray();

        //        Rectangle rect = GetResolutionScreen();
        //        int hauteurGrid = dataGridView2.Size.Height;
        //        int hauteurForm = MESSENGER.Size.Height;
        //        int indexPage = 0;


        //        foreach (FileInfo fichier in fichiers)
        //        {

        //            if (indexPage == STEPP)
        //                break;


        //            Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm-84, hauteurForm - 85)));
        //            dataGridViewPictures.Rows.Add(img);


        //            indexPage++;
        //        }

        //        NextMessenger = STEPP;

        //        //deA.Text = "_________0/xx________".Replace("/xx", "/" + (0 + STEPP - 1).ToString());
        //        labelNbreMessenger.Text = "_____0/xx_____".Replace("/xx", "/" + (fichiers.Length - 1).ToString());

        //        //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
        //        if (File.Exists(test + "\\Messenger_Videos_With_Screenshots.txt"))
        //        {
        //            string[] lignes = File.ReadAllLines(test + "\\Messenger_Videos_With_Screenshots.txt");


        //            foreach (string li in lignes)
        //            {
        //                if (li == "")
        //                    continue;

        //                string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".png" }, StringSplitOptions.RemoveEmptyEntries)[0];
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

        private void Viewer_Load(object sender, EventArgs e)
        {
            this.Hide();
            
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(Viewer_Load), new object[] { sender, e });
                return;
            }


            //foreach (Form f in Application.OpenForms)
            //{

            //    if (f.Name == "Initcontrol")
            //    {
            //        f.Refresh();
            //        Thread.Sleep(2000);
            //        f.Close();


            //        this.WindowState = FormWindowState.Maximized;
            //        break;
            //    }

            //}

            //this.Show();
            this.Activate();
        }

        private void pictureBoxtango_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewForPictures_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Process.Start(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
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
            box.Name ="\\HOMEPAGE\\Screenshot_"+(((DataGridView)sourceControl).SelectedRows[0].Index + 1).ToString();

            flowLayoutPanelAnnexe.Controls.Add(box);
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            PictureBox destination = ((PictureBox)sourceControl);
            flowLayoutPanelAnnexe.Controls.Remove(destination);
        }

        private void button4_Click_3(object sender, EventArgs e)
        {
            flowLayoutPanelAnnexe.Controls.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = WordApp.Documents.Open(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\PV_Template.docx");
            doc.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = "Annexe " + textBoxNumeroAnnexe.Text + " au PV " + textBoxPV.Text;


            for(int i = flowLayoutPanelAnnexe.Controls.Count - 1; i >= 0;i--)
            {
                if (((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.StartsWith("\\HOMEPAGE\\Screenshot_"))
                {
                    string keyy = ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.IndexOf("\\HOMEPAGE\\Screenshot_") + 21);
                    if (keyy.StartsWith("0"))
                        keyy = keyy.Replace("0", "");

                    string path = pathConfig + "\\HOMEPAGE\\Screenshot_" + keyy + ".png";
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

                    string path = pathConfig + ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(2, ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Length - 2) + ".png";
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

                    string path = pathConfig + ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Substring(1, ((PictureBox)flowLayoutPanelAnnexe.Controls[i]).Name.Length - 1);
                    if (File.Exists(path))
                    {

                        //doc.InlineShapes.AddPicture(CreateBigPictures(path, false), Type.Missing, Type.Missing, Type.Missing);
                        doc.InlineShapes.AddPicture(path, Type.Missing, Type.Missing, Type.Missing);
                        //File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\image.png");


                    }
                }

            }


            //foreach (Control c in flowLayoutPanelAnnexe.Controls)
            //{
            //    if (((PictureBox)c).Name.StartsWith("\\HOMEPAGE\\Screenshot_"))
            //    {
            //        string keyy = ((PictureBox)c).Name.Substring(((PictureBox)c).Name.IndexOf("\\HOMEPAGE\\Screenshot_") + 21);
            //        if (keyy.StartsWith("0"))
            //            keyy = keyy.Replace("0", "");

            //        string path = pathConfig + "\\HOMEPAGE\\Screenshot_" + keyy + ".jpg";
            //        if (File.Exists(path))
            //        {

            //            doc.InlineShapes.AddPicture(CreateBigPictures(path, false), Type.Missing, Type.Missing, Type.Missing);
            //            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\image.jpg");


            //        }
            //    }
                
            //}

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

        private string CreateBigPictures(string bitmap, bool nomFichier)
        {
            FileInfo fichier = new FileInfo(bitmap);

            Image img = Image.FromFile(bitmap);


            int differencePixel = img.Width - 640;

            if (img.Width > 640)
                while (true)
                {
                    img = CreateThumbnail(bitmap, 640, img.Height - differencePixel);

                    if (img.Width <= 640)
                        break;
                }
            //if (img.Width > 640)
            //    img = CreateThumbnail(bitmap, 640, img.Height - differencePixel);

            using (Bitmap bmp = new Bitmap(640, 842))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    int X = 0;
                    int Y = 5;
                    int i = 0;

                    g.DrawImage(img, X, Y, img.Width, img.Height);
                    if (nomFichier)
                    {

                        Rectangle ee = new Rectangle(X, Y, (img.Width) - 8, 15);
                        using (Pen pen = new Pen(Color.Black, 15))
                        {
                            g.DrawRectangle(pen, ee);
                            //loBMP.Dispose();
                        }

                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        Font font = new System.Drawing.Font("Century Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        g.DrawString(fichier.Name, font, Brushes.White, X, Y - 5);
                    }





                }

                bmp.Save(AppDomain.CurrentDomain.BaseDirectory + "\\image.png",ImageFormat.Png);
                return AppDomain.CurrentDomain.BaseDirectory + "\\image.png";
            }
        }

        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);


            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        //private void contextMenuStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    ContextMenuStrip menu = sender as ContextMenuStrip;
        //    Control sourceControl = menu.SourceControl;
        //    //PictureBox destination = ((PictureBox)sender);

        //    string pathToFolder = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString().Replace(".\\","\\");
        //    string folderName = dataGridViewMessenger.SelectedRows[0].Cells[1].Value.ToString();

        //    PictureBox box = new PictureBox();
        //    box.Size = new Size(500, 500);
        //    box.Image = (Image)((DataGridView)sourceControl).SelectedRows[0].Cells[0].Value;
        //    //box.BorderStyle = BorderStyle.FixedSingle;
        //    box.Cursor = Cursors.Hand;
        //    box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        //    box.ContextMenuStrip = contextMenuStrip2;
        //    box.Name = pathToFolder + "\\Messenger_" + folderName.Trim() + "_" + (((DataGridView)sourceControl).SelectedRows[0].Index + 1).ToString();

        //    flowLayoutPanelAnnexe.Controls.Add(box);
        //}

        private void contextMenuStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
;
        }

        private void textBoxPV_TextChanged(object sender, EventArgs e)
        {
            textBoxPVV.Text = textBoxPV.Text;
        }

        private void textBoxNumeroAnnexe_TextChanged(object sender, EventArgs e)
        {
            textBoxNumero.Text = textBoxNumeroAnnexe.Text;
        }

        private void dataGridViewIdentifiants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Process.Start(dataGridViewIdentifiants.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().Replace("//","/"));
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            using (Process myProcess = new Process())
            {
                // myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.
                myProcess.StartInfo.FileName = ((Panel)sender).Name;
                //myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                // This code assumes the process you are starting will terminate itself.
                // Given that is is started without a window so you cannot terminate it
                // on the desktop, it must terminate itself or you can do it programmatically
                // from this application using the Kill method.
            }
        }

        private void dataGridViewResume_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewResume.ClearSelection();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            FillPicturesView();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (Next > fichiersImages.Length)
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridView1.Rows.Clear();


            for (int i = Next; i < Next + STEPP; i++)
            {

                if (i == Next + STEPP)
                    break;

                if (i > fichiersImages.Length - 1)
                    break;

                FileInfo fichier = fichiersImages[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView1.Size.Height;
                int hauteurForm = JOURNAL.Size.Height;

                try
                {

                    Image img = Image.FromFile(fichier.FullName);
                    Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width - 119, hauteurForm - 120)));
                    dataGridView1.Rows.Add(img);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + fichier.FullName);
                }

            }

            Next += STEPP;
            label3.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersImages.Length - 1).ToString()).Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (0 + (Next - STEPP)).ToString() + "/");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if ((Next - (STEPP * 2)) < 0)
                return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Image img = (Image)row.Cells[0].Value;
                img.Dispose();


            }
            GC.Collect();

            dataGridView1.Rows.Clear();

            for (int i = (Next - (STEPP * 2)); i <= Next - STEPP; i++)
            {

                //if (i == Next - STEPP)
                //    break;

                FileInfo fichier = fichiersImages[i];

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView1.Size.Height;
                int hauteurForm = JOURNAL.Size.Height;

                Image img = Image.FromFile(fichier.FullName);
                Image imgg = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(img.Width - 119, hauteurForm - 120)));
                dataGridView1.Rows.Add(imgg);



            }


            label3.Text = "_____xx/xx_____".Replace("/xx", "/" + (fichiersImages.Length - 1).ToString()).Replace("xx/", (Next - (STEPP * 2) + 0) + "/");
            //deA.Text = "_________xx/xx________".Replace("xx/", (Next - (STEPP * 2) + 0) + "/");

            Next -= STEPP;
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            string texteAGarder = label3.Text.Split('/')[1];
            string numero = label3.Text.Split('/')[0].Replace("_____", "");
            Int32.Parse(numero);
            int step = dataGridView1.FirstDisplayedScrollingRowIndex == 0 ? 0 : dataGridView1.FirstDisplayedScrollingRowIndex;


            label3.Text = "_____" + (step + (Next - STEPP)) + "/" + texteAGarder;
        }
    }
}
