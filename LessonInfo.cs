using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Next_Generation_School_System_by_Anton {
    public partial class LessonInformation : Form {
        Label lbl;
        string CurrentWeek = "";
        public string connectioning = "";
        public string CurrentSchool = "";
        public string CurrentClass = "";
        public string CurrentName = "";
        string CurrentEmail = "";
        string CurrentPass = "";

        string Clicked = "";
        string title = "";
        public int x = 3, y = 7, x1 = 1, y1 = 7, y2 = 0, plus1 = 27 - 4;
        public int plus = 24 - 7;
        public int count = 0, count1 = 0;
        int AccesLvl = 4;
        int count2 = 0;

        byte[] GlobalFile;

        List<Files> files = new List<Files>();
        List<Files> Apfiles = new List<Files>();

        public LessonInformation() {
            InitializeComponent();
        }
        private void LessonInfo_Load(object sender, EventArgs e) {

        }
        public void SetLesson(string from, string till, string title, string homeWork,
            string week, string weekNumber, string connect, string name, int AccesLevel, string email, string pass, bool ac = false) {
            CurrentEmail = email;
            CurrentPass = pass;
            CurrentSchool = week.Split(',')[4];
            AccesLvl = AccesLevel;
            label3.Text = from;
            label2.Text = till;
            label1.Text = title;
            connectioning = connect;
            CurrentName = name;
            connectioning = connect;

            lbl = new Label();
            MainWindow wnd = new MainWindow();

            if (AccesLevel == 3 || AccesLevel == 1)
            {
                button1.Visible = true;
                richTextBox1.Visible = true;
            } else
            {
                button2.Visible = false;
                button1.Visible = false;
            }

            if (AccesLevel == 1)
            {
                button3.Visible = false;
            } else
            {
                button3.Visible = true;
                button4.Visible = false;
            }
            GetFiles();
            InstLabels();

            GetFiles(1);
            InstLabels(1);

            Hometasks.Visible = true;
            lbl.Size = new Size(510, 179);
            lbl.Location = new Point(6, 21);

            CurrentWeek = week;

            string connection = connectioning;
            string AddNewElement = "SELECT * FROM " + new MainWindow().DataBaseTitle + ".dbo.DiaryTable";
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(AddNewElement, sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object homework = reader.GetValue(0);
                        object date = reader.GetValue(2);
                        object lesson = reader.GetValue(1);

                        if (lesson.ToString().Equals(label1.Text) && date.ToString().Equals(CurrentWeek))
                        {
                            if (true)
                                richTextBox1.Text = homework.ToString();
                            if (true)
                                lbl.Text = richTextBox1.Text;
                        }
                    }
                }

                AddNewElement = "SELECT * FROM " + new MainWindow().DataBaseTitle + ".dbo.PupilsMarks";
                reader.Close();
                using (SqlConnection sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    SqlCommand command2 = new SqlCommand(AddNewElement, sqlConnection);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            object Mark = reader2.GetValue(0);
                            object Index = reader2.GetValue(1);
                            string[] indentificator = week.Split(',');

                            string Index2 = "";

                            CurrentClass = indentificator[3];
                            CurrentSchool = indentificator[4];
                            if (!ac)
                            {
                                Index2 = indentificator[0] + "," + indentificator[1]
                                + "," + weekNumber + "," + new MainWindow().CurrentPupilName + "," + indentificator[3] + "," + indentificator[4];

                            } else
                                Index2 = week.Substring(0, week.Length - 1);

                            if (Index.Equals(Index2))
                            {
                                label5.Text = Mark.ToString().Split(':')[0];
                                label6.Text = Mark.ToString().Split(':')[1];
                            }
                        }
                    }
                }
                groupBox2.Controls.Add(lbl);
            }
            richTextBox2.Visible = true;
            using (SqlConnection sql = new SqlConnection(connectioning))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_ChatsAndFiles.dbo.Comments", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (CurrentWeek.Equals(reader.GetString(1)))
                        {
                            Label newlbl = new Label();
                            newlbl.Text = reader.GetString(0) + " at " +
                                reader.GetString(7) + "\n";
                            for (int i = 0; i < reader.GetString(4).Split('\n').Length - 1; i++)
                                newlbl.Text += reader.GetString(4).Split('\n')[i] + "\n";
                            newlbl.Location = new Point(50, y2);
                            newlbl.AutoSize = true;
                            panel3.Controls.Add(newlbl);

                            Label newlbl2 = new Label();
                            newlbl2.Text = reader.GetString(0) + ":\n" + reader.GetString(4);
                            newlbl2.Location = new Point(50, y2);
                            newlbl2.AutoSize = true;
                            newlbl2.MouseClick += new MouseEventHandler(this.ClickOnComment);
                            panel3.Controls.Add(newlbl2);
                            Clicked = newlbl2.Text;
                            newlbl2.Font = new Font(label3.Font, FontStyle.Underline);

                            using (SqlConnection sql1 = new SqlConnection(connectioning))
                            {
                                sql1.Open();
                                SqlCommand command1 = new SqlCommand("SELECT * FROM NextGenSchoolSystem_Diary.dbo.Accounts", sql1);
                                SqlDataReader reader1 = command1.ExecuteReader();
                                if (reader1.HasRows)
                                {
                                    while (reader1.Read())
                                    {
                                        // MessageBox.Show(reader.GetString(2),(reader1.GetString(7)).ToString());
                                        if (reader.GetString(0).Equals(reader1.GetString(0) + " " + reader1.GetString(1)) &&
                                            reader.GetString(2).Equals(reader1.GetString(7)) &&
                                            reader.GetString(2).Equals(reader1.GetString(7)))
                                        {
                                            PictureBox picture = new PictureBox();
                                            picture.Name = reader1.GetString(0) + " " + reader1.GetString(1)
                                                + "|" + reader1.GetString(7) + "|" + reader1.GetString(6) + "|"
                                                + reader1.GetString(3) + "|" + reader1.GetString(2);
                                            picture.Location = new Point(7, y2);
                                            picture.Size = new Size(40, 40);
                                            byte[] buffer = (byte[])reader1.GetValue(9);
                                            MemoryStream stream = new MemoryStream(buffer);
                                            picture.SizeMode = PictureBoxSizeMode.StretchImage;
                                            picture.Image = Image.FromStream(stream);
                                            picture.Click += new EventHandler(this.PictureClick);

                                            panel3.Controls.Add(picture);
                                            reader1.GetString(0);
                                        }
                                    }
                                }
                            }
                            y2 += plus1 + reader.GetString(0).Split('\n').Length * 2 + 20;
                        }
                    }
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e) {
            lbl.Text = richTextBox1.Text;
            string connection = connectioning;
            string ht = richTextBox1.Text;
            string lesson = label1.Text;
            string AddNewElement = @" UPDATE " + new MainWindow().DataBaseTitle + @".dbo.DiaryTable SET
              Homework = '" + ht + @"',
			  Lesson = '" + lesson + @"',
			  Diary = '" + CurrentWeek + @"'
			  WHERE Lesson = '" + lesson + @"' AND Diary = '" + CurrentWeek + @"'";
            int result;
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(AddNewElement, sql);
                result = command.ExecuteNonQuery();
            }

            if (result < 1)
            {

                MainWindow wnd = new MainWindow();

                AddNewElement = @"INSERT " + new MainWindow().DataBaseTitle + @".dbo.DiaryTable VALUES (
                '" + richTextBox1.Text + @"',
                '" + label1.Text + @"',
                '" + CurrentWeek + @"'
                )";

                using (SqlConnection sql = new SqlConnection(connection))
                {
                    sql.Open();
                    SqlCommand command = new SqlCommand(AddNewElement, sql);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e) {
            richTextBox1.Text = "";
        }
        private void button3_Click(object sender, EventArgs e) {
            string connectionString = @"Data Source=LAPTOP;Initial Catalog=NextGenSchoolSystem_ChatsAndFiles;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.ShowDialog();
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                string filename = openFile.FileName;
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);
                command.CommandText = @"INSERT NextGenSchoolSystem_ChatsAndFiles.dbo.HomeTaskSend VALUES (
                '" + shortFileName + "|" + CurrentName + "|" + label1.Text + "'," +
                "'" + CurrentSchool + "','" + CurrentClass + "',@ImageData,'" +
                DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "')";
                command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                if (filename != "")
                {
                    byte[] imageData;
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                    {
                        imageData = new byte[fs.Length];
                        fs.Read(imageData, 0, imageData.Length);
                    }
                    // передаем данные в команду через параметры
                    command.Parameters["@ImageData"].Value = imageData;

                    command.ExecuteNonQuery();
                }
            }
        }
        private void button4_Click(object sender, EventArgs e) {
            GetFiles();
            InstLabels();
        }
        private void button4_Click_1(object sender, EventArgs e) {
            {
                string connectionString = @"Data Source=LAPTOP;Initial Catalog=NextGenSchoolSystem_ChatsAndFiles;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.ShowDialog();
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    string filename = openFile.FileName;
                    string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);
                    command.CommandText = @"INSERT NextGenSchoolSystem_ChatsAndFiles.dbo.AttahedFiles VALUES (
                '" + shortFileName + "|" + CurrentName + "|" + label1.Text + "'," +
                    "'" + CurrentSchool + "','" + CurrentClass + "',@ImageData," +
                    DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ")";
                    command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                    if (filename != "")
                    {
                        byte[] imageData;
                        using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                        {
                            imageData = new byte[fs.Length];
                            fs.Read(imageData, 0, imageData.Length);
                        }
                        // передаем данные в команду через параметры
                        command.Parameters["@ImageData"].Value = imageData;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        void GetFiles(int index = 0) {
            if (index == 0)
                using (SqlConnection sql = new SqlConnection(connectioning))
                {
                    sql.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_ChatsAndFiles.dbo.HomeTaskSend", sql);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string School = reader.GetString(1);
                            string Class = reader.GetString(2);
                            this.Text = CurrentName.Equals(title.Split('|')[1]).ToString() + " "
                             + label1.Text.Equals(title.Split('|')[2]);


                            byte[] Buffer = (byte[])reader.GetValue(3);
                            if (CurrentName.Equals(title.Split('|')[1]) &&
                                label1.Text.Equals(title.Split('|')[2]) || AccesLvl == 1
                                && label1.Text.Equals(title.Split('|')[2])
                                && CurrentSchool.Equals(School)
                                && Class.Equals(Class))
                            {
                                Files file = new Files(title, School, Class, reader.GetString(4), Buffer);
                                files.Add(file);
                                /// MessageBox.Show(reader.GetString(4));
                            }
                        }
                    }
                }
            else if (index == 1)
            {

                using (SqlConnection sql = new SqlConnection(connectioning))
                {
                    sql.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_ChatsAndFiles.dbo.AttahedFiles", sql);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string School = reader.GetString(1);
                            string Class = reader.GetString(2);
                            byte[] Buffer = (byte[])reader.GetValue(3);
                            if (label1.Text.Equals(title.Split('|')[2])
                                && CurrentSchool.Equals(School)
                                && Class.Equals(Class))
                            {
                                Files file = new Files(title, School, Class, reader.GetString(4), Buffer);
                                Apfiles.Add(file);
                            }
                        }
                    }
                }
            }
        }
        void InstLabels(int index = 0) {
            if (index == 0)
            {
                y = 7;
                count = 0;
                for (int i = 0; i < files.Count; i++)
                {
                    this.Text = CurrentName;
                    Label lbl = new Label();
                    lbl.AutoSize = true;
                    lbl.Location = new Point(x, y);
                    y += plus;
                    lbl.Text = count + 1 + ")" + files[i].Title.Split('|')[0] + "-" + files[i].Title.Split('|')[1];
                    lbl.Name = files[i].Date;
                    lbl.MouseClick += new MouseEventHandler(this.MouseClickOnHt);
                    panel1.Controls.Add(lbl);
                    count++;
                }
            } else if (index == 1)
            {
                y1 = 7;
                count1 = 0;

                for (int i = 0; i < Apfiles.Count; i++)
                {

                    this.Text = CurrentName;
                    Label lbl = new Label();
                    lbl.AutoSize = true;
                    lbl.Location = new Point(x1, y1);
                    y1 += plus;
                    lbl.Text = count1 + 1 + ")" + Apfiles[i].Title.Split('|')[0] + "-" + Apfiles[i].Title.Split('|')[1];
                    lbl.Name = Apfiles[i].Date;
                    lbl.MouseClick += new MouseEventHandler(this.MouseClickOnHtAt);
                    panel2.Controls.Add(lbl);
                    count1++;
                }
            }
        }
        private void button5_Click(object sender, EventArgs e) {
            Label newlbl = new Label();
            newlbl.Text = CurrentName + " at " +
                DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "\n";
            for (int i = 0; i < richTextBox2.Text.Split('\n').Length - 1; i++)
                newlbl.Text += richTextBox2.Text.Split('\n')[i] + "\n";
            newlbl.Location = new Point(50, y2);
            newlbl.AutoSize = true;
            panel3.Controls.Add(newlbl);

            Label newlbl2 = new Label();
            newlbl2.Text = CurrentName + ":\n" + richTextBox2.Text;
            newlbl2.Location = new Point(50, y2);
            newlbl2.AutoSize = true;
            panel3.Controls.Add(newlbl2);
            newlbl2.Font = new Font(label3.Font, FontStyle.Underline);

            using (SqlConnection sql1 = new SqlConnection(connectioning))
            {
                sql1.Open();

                SqlCommand command1 = new SqlCommand("INSERT NextGenSchoolSystem_ChatsAndFiles.dbo.Comments VALUES('" +
                    "" + CurrentName + "','" +
                    "" + CurrentWeek + "','" +
                    "" + CurrentSchool + "','" +
                    "" + CurrentClass + "','" +
                    "" + richTextBox2.Text + "',NULL,NULL," +
                    "'" + DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "')", sql1);
                command1.ExecuteNonQuery();
            }

            using (SqlConnection sql1 = new SqlConnection(connectioning))
            {
                sql1.Open();
                SqlCommand command1 = new SqlCommand("SELECT * FROM NextGenSchoolSystem_Diary.dbo.Accounts", sql1);
                SqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        if (CurrentName.Equals(reader1.GetString(0) + " " + reader1.GetString(1)) &&
                          reader1.GetString(7).Equals(CurrentSchool))
                        {
                            PictureBox picture = new PictureBox();
                            picture.Name = CurrentName + "|" + CurrentSchool + "|" + CurrentClass + "|" + CurrentEmail + "|" + CurrentPass;
                            picture.Location = new Point(7, y2);
                            picture.Size = new Size(40, 40);
                            byte[] buffer = (byte[])reader1.GetValue(9);
                            MemoryStream stream = new MemoryStream(buffer);
                            picture.SizeMode = PictureBoxSizeMode.StretchImage;
                            picture.Image = Image.FromStream(stream);
                            picture.Click += new EventHandler(this.PictureClick);
                            //MessageBox.Show("");

                            panel3.Controls.Add(picture);
                            reader1.GetString(0);
                        }
                    }
                }
            }
            y2 += plus1 + richTextBox2.Text.Split('\n').Length * 2;
        }
        void ClickOnHt(Label clicked) {
            string folder = "";
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                folder = diag.SelectedPath;  //selected folder path

                int number = Int32.Parse(clicked.Text.Split(')')[0]) - 1;

                using (System.IO.FileStream fs = new System.IO.FileStream(folder + "//" + files[number].Title.Split('|')[0], FileMode.OpenOrCreate))
                {
                    fs.Write(files[number].Data, 0, files[number].Data.Length);
                }
                MessageBox.Show("File uploaded succesfully ", "IO party");
            }
        }
        void ClickOnAp(Label clicked) {
            string folder = "";
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folder = diag.SelectedPath;  //selected folder path
            }


            int number = Int32.Parse(clicked.Text.Split(')')[0]) - 1;
            using (System.IO.FileStream fs = new System.IO.FileStream(folder + "//" + Apfiles[number].Title.Split('|')[0], FileMode.OpenOrCreate))
            {
                fs.Write(Apfiles[number].Data, 0, Apfiles[number].Data.Length);
            }
            MessageBox.Show("File uploaded succesfully ", "IO party");
        }
        void MouseClickOnHt(object sender, MouseEventArgs e) {
            Label lbl = (Label)sender;

            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show("Do you realy want to delete this file?", "File deleting", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    using (SqlConnection sql = new SqlConnection(connectioning))
                    {
                        sql.Open();
                        SqlCommand command = new SqlCommand("DELETE FROM NextGenSchoolSystem_ChatsAndFiles.dbo.HomeTaskSend WHERE " +
                            "Title = '" + lbl.Text.Split(')')[1].Split('-')[0] + "|" + lbl.Text.Split(')')[1].Split('-')[1] + "|" + label1.Text + "' AND " +
                            "School = '" + CurrentSchool + "'", sql);
                        command.ExecuteNonQuery();
                    }


                    for (int i = 0; i < Apfiles.Count; i++)
                    {
                        if (files[i].Title.Equals(lbl.Text.Split(')')[1].Split('-')[0]
                            + " | " + lbl.Text.Split(')')[1].Split('-')[1] + " | " + label1.Text) && files[i].School.Equals(CurrentSchool))
                        {
                            files.RemoveAt(i);
                        }

                    }
                }
            } else if (e.Button == MouseButtons.Right)
            {
                ClickOnHt(lbl);
            } else if (e.Button == MouseButtons.Middle)
            {
                MouseHovering(lbl);
            }
        }
        void MouseClickOnHtAt(object sender, MouseEventArgs e) {
            Label lbl = (Label)sender;
            if (e.Button == MouseButtons.Right && AccesLvl == 1)
            {
                if (MessageBox.Show("Do you realy want to delete this file?", "File deleting", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection sql = new SqlConnection(connectioning))
                    {
                        sql.Open();
                        SqlCommand command = new SqlCommand("DELETE FROM NextGenSchoolSystem_ChatsAndFiles.dbo.AttahedFiles WHERE " +
                            "Title = '" + lbl.Text.Split(')')[1].Split('-')[0] + "|" + lbl.Text.Split(')')[1].Split('-')[1] + "|" + label1.Text + "' AND " +
                            "School = '" + CurrentSchool + "'", sql);
                        command.ExecuteNonQuery();
                    }


                    for (int i = 0; i < Apfiles.Count; i++)
                    {
                        if (Apfiles[i].Title.Equals(lbl.Text.Split(')')[1].Split('-')[0]
                            + " | " + lbl.Text.Split(')')[1].Split('-')[1] + " | " + label1.Text) && Apfiles[i].School.Equals(CurrentSchool))
                        {
                            Apfiles.RemoveAt(i);
                        }
                    }
                }
            } else if (e.Button == MouseButtons.Left && AccesLvl == 1)
            {
                ClickOnAp(lbl);
            } else if (e.Button == MouseButtons.Middle)
            {
                MouseHovering(lbl);
            }
        }
        void ClickOnComment(object sender, MouseEventArgs e) {

            string folder = "";
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folder = diag.SelectedPath;  //selected folder path
            }
            MessageBox.Show(folder + "\\" + Clicked);
            using (System.IO.FileStream fs = new System.IO.FileStream(folder + "\\" + Clicked.Substring(0, Clicked.Split(':')[1].Length), FileMode.OpenOrCreate))
            {
                fs.Write(GlobalFile, 0, GlobalFile.Length);
            }
            MessageBox.Show("File uploaded succesfully ", "IO party");
        }
        void PictureClick(object sender, EventArgs e) {
            PictureBox picture = (PictureBox)sender;

            Social_Network social = new Social_Network(picture.Name.Split('|')[0].Split(' ')[0], picture.Name.Split('|')[0].Split(' ')[1],
               picture.Name.Split('|')[3], picture.Name.Split('|')[4], picture.Name.Split('|')[1], picture.Name.Split('|')[2], connectioning,
               false);
            social.Show();
        }
        void MouseHovering(Label date) {
            getDate.Enabled = false;

            label8.Text = date.Name;
            label8.Visible = true;

            getDate.Enabled = true;
            count2 = 0;
            //  MessageBox.Show(lbl.Name);
        }
        private void getDate_Tick(object sender, EventArgs e) {
            count2++;
            if (count2 == 3)
            {
                getDate.Enabled = false;
                label8.Visible = false;
            }
        }
    }
}


