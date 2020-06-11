using OpenQA.Selenium;
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

namespace FacebookAnalyzer
{
    public partial class Viewer : Form
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




        public Viewer()
        {
            InitializeComponent();
            //dataGridViewMessenger
            //textBoxops.Select();
            GetResolutionScreen();
            textBox1.Width = this.Width;


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

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FindAllFriendsFromFacebookBis(textBoxUSERNAMEFRIENDS.Text);
            //GetAllCommentsForTest(dateTimePicker1.Value.Year, dateTimePicker2.Value.Year);
            //check datetimepicker pour les commentaires

            if (textBoxops.Text == "")
            {
                textBoxops.BackColor = Color.Red;
                MessageBox.Show("Veuillez remplir le champ OPS");
                return;
            }
            else
                textBoxops.BackColor = Color.White;

            


            IsANewThread();
            
            
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

            

           

        }
        private void Reset()
        {

            

            friends = false;
            comments = false;
            allimages = false;
            

            //pictureBoxcomments.Visible = false;
            //pictureBoxfriends.Visible = false;
            //pictureBoxpictures.Visible = false;
            //pictureBoxwaiting.Visible = false;
            //labelanalyseencours.Visible = false;


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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (backgroundWorker1 != null)
            {
               
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

            
            IsANewThread();

        }

        
       

        private void FillDataGridViewMessenger(Image image, string username, string link, string pathToPictureProfile)
        {
            dataGridViewMessenger.Rows.Add(image, username, link,false,"",pathToPictureProfile);
        }

        private void FillDataGridViewMessenger(string pathToFolder, string link)
        {
            
            foreach(DataGridViewRow row in dataGridViewMessenger.Rows)
            {
                if(row.Cells[2].Value.ToString() == link)
                {
                    row.Cells[3].Value = false;
                    row.Cells[4].Value = pathToFolder;
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;

                    dataGridViewMessenger.ClearSelection();
                    //dataGridViewPictures.Rows[rowIndex].Selected = true;
                    //dataGridViewMessenger.FirstDisplayedScrollingRowIndex = row.Index;
                    

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

            
                
        }

        private void dataGridViewMessenger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridViewMessenger.Rows.Count > 0 )//&& ((bool)dataGridViewMessenger.Rows[e.RowIndex].Cells[3].Value))
            {

                if (sortedListForSearching != null || sortedListForSearching.Count > 0)
                    sortedListForSearching.Clear();



                string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
                dataGridViewPictures.Rows.Clear();
                var sorted = Directory.GetFiles(test, "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
                FileInfo[] fichiers = sorted.ToArray();

                Rectangle rect = GetResolutionScreen();
                int hauteurGrid = dataGridView2.Size.Height;
                int hauteurForm = tabPage1.Size.Height;

                foreach (FileInfo fichier in fichiers)
                {
                    
                    Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
                    dataGridViewPictures.Rows.Add(img);
                    
                }

                  
            }
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //string test = dataGridViewMessenger.Rows[e.RowIndex].Cells[4].Value.ToString();
            string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Henri Dewyse";
            dataGridViewPictures.Rows.Clear();
            var sorted = Directory.GetFiles(test, "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.CreationTime);
            FileInfo[] fichiers = sorted.ToArray();

            Rectangle rect = GetResolutionScreen();
            int hauteurGrid = dataGridView2.Size.Height + 15;
            int hauteurForm = tabPage1.Size.Height;

            foreach (FileInfo fichier in fichiers)
            {
                //PictureBox imageViewer = new PictureBox();
                //imageViewer.Image = Image.FromFile(fichier.FullName);
                //imageViewer.SizeMode = PictureBoxSizeMode.Normal;
                //imageViewer.Dock = DockStyle.Bottom;
                //imageViewer.Height = 1250;
                //imageViewer.Width = 1250;
                Image img = (Image)(new Bitmap(Image.FromFile(fichier.FullName), new Size(hauteurForm, hauteurForm - 1)));
                dataGridViewPictures.Rows.Add(img);
                //flowLayoutPanel1.Controls.Add(imageViewer);
            }
        }

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

        private void button4_Click_1(object sender, EventArgs e)
        {
            SearchKeywords(textBox1.Text);
        }

        private void SearchKeywords(string keyword)
        {
            if(dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0 && dataGridViewPictures.Rows.Count > 0)
            {
                                
                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_Messages_With_Screenshots.txt"))
                {
                    string[] lignes = File.ReadAllLines(test + "\\Messenger_Messages_With_Screenshots.txt");

                    if(sortedListForSearching.Count == 0)
                    {
                        foreach (string li in lignes)
                        {
                            if (li == "")
                                continue;

                            string numeroLigne = (li.Split(';')[1]).Substring(li.Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            sortedListForSearching.Add(li.ToLower());


                        }

                        try
                        {
                            int index = sortedListForSearching.FindIndex(x => x.Contains(keyword));

                            if (index == -1)
                                return;

                            string numeroLigne = (sortedListForSearching[index].Split(';')[1]).Substring(sortedListForSearching[index].Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            int indexx = Int32.Parse(numeroLigne) - 1;

                            dataGridViewPictures.ClearSelection();
                            //dataGridViewPictures.Rows[rowIndex].Selected = true;
                            dataGridViewPictures.FirstDisplayedScrollingRowIndex = indexx;

                            dataGridViewPictures.Focus();
                        }                       
                        catch(ArgumentNullException ex)
                        {
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            int index = sortedListForSearching.FindIndex(x => x.Contains(keyword));
                            string numeroLigne = (sortedListForSearching[index].Split(';')[1]).Substring(sortedListForSearching[index].Split(';')[1].LastIndexOf("_") + 1).Split(new string[] { ".jpg" }, StringSplitOptions.RemoveEmptyEntries)[0];

                            int indexx = Int32.Parse(numeroLigne) - 1;

                            dataGridViewPictures.ClearSelection();
                            //dataGridViewPictures.Rows[rowIndex].Selected = true;
                            dataGridViewPictures.FirstDisplayedScrollingRowIndex = indexx;

                            dataGridViewPictures.Focus();
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            if (dataGridViewMessenger.Rows.Count > 0 && dataGridViewMessenger.SelectedRows.Count > 0)
            {
                string test = dataGridViewMessenger.SelectedRows[0].Cells[4].Value.ToString();//C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks
                //string test = @"C:\Users\frank\Documents\Facebook_Friends\Messenger\Stephane Hendrycks";
                if (File.Exists(test + "\\Messenger_" + test.Substring(test.LastIndexOf("\\") + 1) + ".txt"))
                {
                    Messages msg = new Messages();
                    msg.SetRichTextBox(File.ReadAllText(test + "\\Messenger_" + test.Substring(test.LastIndexOf("\\") + 1) + ".txt"));
                    msg.Show();

                }



                
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            
        }

        

        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { ValidateNames = true, Multiselect = false, Filter = "Text Document|*.txt" })
            {
               



                
            }
        }
    }
}
