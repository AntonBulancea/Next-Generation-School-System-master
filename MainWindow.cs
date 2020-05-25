/*
 * 
 -----------------------------------------------
 Next Generation School System by Anton Bulancea
 -----------------------------------------------
 *
 * Спасибо Алексею Буланча)
 */
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
using System.Net.Mail;
using System.Net;
namespace Next_Generation_School_System_by_Anton {
    public partial class MainWindow : Form {
        
        Dictionary<string, string> LessonsPosition = new Dictionary<string, string>()
        {
            {"Monday","3;9;0"},
            {"Tuesday","5;9;0"},
            {"Wednesday","5;9;0"},
            {"Thursday","5;9;0" },
            {"Friday","5;5;0"},
            {"Saturday","5;5;0"}
        };
        Dictionary<string, string> Marks = new Dictionary<string, string>();
        public List<string> LessonTitles = new List<string>();
        public List<string> Month = new List<string>() {
            "January", "February",
            "March", "April", "May",
            "June", "July", "August",
            "September", "October", "November",
            "December"};

        public List<string> LessonOnDay = new List<string>();
        public List<string> LessonTimeFrom = new List<string>();
        public List<string> LessonTimeTill = new List<string>();
        public List<string> Pupils = new List<string>();
        public List<string> AllMarks = new List<string>();
        public List<string> AllLessons = new List<string>();

        //Other Computers Sql working
        public string DataBaseTitle = "NextGenSchoolSystem_Diary";
        public string ComputerName = "LAPTOP";
        public string connection;//строка подключения  
        //вместо LAPTOP запишите название вашего сервера
        public bool TablesWereInst = true; //Eсли все таблицы уже были созданы,то запишите true,если нет - false

        //Other Computers Sql working

        Label chosen;
        Button CurrentMark;

        string CurrentLessonTitle;
        string CurrentClass = "6 C";
        string CurrentSchool = "School A. Puskin";
        string CurrentEmail = "";
        string CurrentPass = "";
        string CurrentId = "";
        public string CurrentPupilName = "Anton Bulancea";

        int CurrentLessonNum = -1;
        int currentWeekNumber = 1;
        int currentMonthNumber = 1;
        int count = 0;
        public int tick = 30; // сколько времени (в сек) будет открыто окно оценок (нажмите на название или наведите на урок чтобы активировать окно)

        bool HoveredEvent = false;
        bool SettingOpen_Click = true;
        bool Tick;
        bool Clicked;
        bool OpenedWithHovering = false;
        bool CloseWindow = false;
        bool OpenWindow = false;
        bool ClickSetting = false;
        bool Open = false;
        bool OpenMarkWndWithHovering = true;
        bool isTest = false;
        public int AccesLevel = 4;

        public MainWindow() {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e) {
            connection = "Data Source=" + ComputerName + ";Initial Catalog=" + DataBaseTitle + ";Integrated Security=True";
            if (ReadFromSIFile() != "")
            {
                currentWeekNumber = Int16.Parse(ReadFromSIFile().Split('|')[0]);
                CurrentPupilName = ReadFromSIFile().Split('|')[1];
                CurrentSchool = ReadFromSIFile().Split('|')[2];
                CurrentClass = ReadFromSIFile().Split('|')[3];
                AccesLevel = Int16.Parse(ReadFromSIFile().Split('|')[4]);
                label10.Text = currentWeekNumber.ToString() + " week";
                CurrentEmail = ReadFromSIFile().Split('|')[5];
                CurrentPass = ReadFromSIFile().Split('|')[6];
                CurrentId = ReadFromSIFile().Split('|')[7];
            }
            InitNgss();
            this.Text = "Next Generation School System - " + CurrentPupilName;
        }
        private void Lbl_MouseHover(object sender, EventArgs e) {
            Label Hovered = (Label)sender;
            string[] hovTextArray = Hovered.Text.Split('.');
            string hovText = hovTextArray[1].Substring(1);
            groupBox8.Text = Hovered.Text + " lesson time info";
            this.Text = hovText;
            CurrentLessonTitle = hovText;
            CurrentLessonNum = Int16.Parse(Hovered.Text.Split('.')[0]);
            this.Text = CurrentLessonNum.ToString();

            domainUpDown1.Text = LessonTimeTill[CurrentLessonNum - 1].Split(':')[0];
            domainUpDown2.Text = LessonTimeTill[CurrentLessonNum - 1].Split(':')[1];

            domainUpDown4.Text = LessonTimeFrom[CurrentLessonNum - 1].Split(':')[0];
            domainUpDown3.Text = LessonTimeFrom[CurrentLessonNum - 1].Split(':')[1];

            comboBox3.Text = Hovered.Text.Split('.')[1].Substring(1);

            comboBox2.Text = GetDay(Hovered.Height);

            comboBox4.Text = currentWeekNumber + " week," + GetDay(Hovered.Size.Height);

            if (OpenMarkWndWithHovering)
            {
                chosen = Hovered;

                groupBox9.Visible = true;
                HoveredEvent = true;
                timer.Enabled = true;
                OpenedWithHovering = true;
                comboBox6.Text = Hovered.Text;
            }
        }
        private void NewLesson_Click(object sender, EventArgs e) {
            if (!checkBox1.Checked)
                instNewLabel(comboBox3.Text, comboBox2.Text, "", "");
            else
                chosen.Text = chosen.Text.Split('.')[0] + ". " + comboBox3.Text;
        }
        private void Lesson_Click(object sender, EventArgs e) {
            LessonInformation lf = new LessonInformation();
            lf.Show();

            Label Clicked = (Label)sender;

            string[] titleArr = Clicked.Text.Split('.');
            string title = titleArr[1].Substring(1);
            this.Text = "A" + title;

            string[] hovTextArray = Clicked.Text.Split('.');
            string hovText = hovTextArray[0];
            string Day = GetDay(Clicked.Height);
            label10.Text = currentWeekNumber.ToString() + " week";
            lf.SetLesson(LessonTimeFrom[Int16.Parse(titleArr[0]) - 1], LessonTimeTill[Int16.Parse(titleArr[0]) - 1],
                title, "",
                Clicked.Text.Split('.')[0] + "," + Day + ","
                + currentWeekNumber.ToString() + "," + CurrentClass + "," + CurrentSchool, currentWeekNumber.ToString(),
                connection, CurrentPupilName, AccesLevel, CurrentEmail, CurrentPass);

            this.Text = Clicked.Text.Split('.')[0] + "," + Day + ","
                + currentWeekNumber.ToString() + "," + CurrentClass + "," + CurrentSchool;
        }
        private void GroupBoxMouseHover(object sender, EventArgs e) {
            GroupBox Hovered = (GroupBox)sender;
            comboBox2.Text = Hovered.Text;
        }
        private void Close_Click(object sender, EventArgs e) {
            WriteIntoSIFile(currentWeekNumber.ToString());
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                sqlConnection.Open();
                for (int j = 0; j < LessonTitles.Count; j++)
                {
                    string Command = @"INSERT " + DataBaseTitle + ".[dbo].[Lesson list]" + @" 
                                      VALUES('" + LessonTitles[j] + "', '00:00', '00:00', '"
                                      + LessonOnDay[j] + "','" + CurrentSchool + "')";
                    using (SqlCommand command = new SqlCommand(Command, sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            this.Text = LessonTitles.Count.ToString();

            //Second Table Inserting

            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                sqlConnection.Open();
                using (SqlCommand doing = new SqlCommand("DELETE FROM " +
            "" + DataBaseTitle + ".[dbo].[LessonTimeInformation]", sqlConnection))
                {
                    doing.ExecuteNonQuery();
                }

                for (int i = 0; i < LessonTimeTill.Count; i++)
                {
                    string Command = @"INSERT " + DataBaseTitle + @".[dbo].[LessonTimeInformation] 
                                       VALUES ('" + LessonTimeFrom[i] + "','" + LessonTimeTill[i] + "','" + i + "','" +
                                       CurrentClass + "|" + CurrentSchool + "')";

                    SqlCommand command = new SqlCommand(Command, sqlConnection);

                    command.ExecuteNonQuery();
                }
            }
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(@"UPDATE [NextGenSchoolSystem_Diary].[dbo].[Accounts] 
                    SET Activity = '" + "0" + @"' WHERE Classes = '" + CurrentClass + "' " +
                "AND School = '" + CurrentSchool + "' AND UserSurName = '" + CurrentPupilName.Split(' ')[1] + "' AND " +
                "UserName = '" + CurrentPupilName.Split(' ')[0] + "' AND UserEmail = '" + CurrentEmail + "' " +
                "AND UserPassword = '" + CurrentPass + "'", sql);
                command.ExecuteNonQuery();
            }
            this.Close();

        }
        private void ChangeET_Click(object sender, EventArgs e) {
            LessonTimeTill[CurrentLessonNum - 1] = domainUpDown1.Text + ":" + domainUpDown2.Text;
        }
        private void ChangeBT_Click(object sender, EventArgs e) {
            LessonTimeFrom[CurrentLessonNum - 1] = domainUpDown4.Text + ":" + domainUpDown3.Text;
        }
        private void ChangeADI_Click(object sender, EventArgs e) {
            LessonTimeTill[CurrentLessonNum - 1] = domainUpDown1.Text + ":" + domainUpDown2.Text;
            LessonTimeFrom[CurrentLessonNum - 1] = domainUpDown4.Text + ":" + domainUpDown3.Text;
        }
        private void LessonListChange_Click(object sender, EventArgs e) {
            comboBox3.Items.Add(textBox1.Text);
            textBox1.Text = "";
        }
        private void Next_Click(object sender, EventArgs e) {
            if (currentWeekNumber <= 48)
                currentWeekNumber++;

            label10.Text = currentWeekNumber.ToString() + " week";
        }
        private void Prev_Click(object sender, EventArgs e) {
            if (currentWeekNumber > 1)
                currentWeekNumber--;

            label10.Text = currentWeekNumber.ToString() + " week";
        }
        private void timer_Tick(object sender, EventArgs e) {
            if (HoveredEvent && groupBox9.Location.Y >= 315)
            {
                groupBox9.Location = new Point(groupBox9.Location.X, groupBox9.Location.Y - 5);
            } else if (HoveredEvent)
            {
                TurningOffTimer.Enabled = true;
                TurningOffTimer.Interval = 1000;
                timer.Enabled = false;
            }
        }
        private void TurningOffTimer_Tick(object sender, EventArgs e) {
            if (count == tick)
            {
                //   this.Text = groupBox9.Location.Y.ToString();
                TurningOffTimer.Interval = 1;

                Tick = true;

                if (groupBox9.Location != new Point(groupBox9.Location.X, 500))
                {

                    groupBox9.Location = new Point(groupBox9.Location.X, groupBox9.Location.Y + 5);
                }
                if (groupBox9.Location == new Point(groupBox9.Location.X, 500))
                {
                    Tick = false;
                    count = 0;
                    OpenedWithHovering = false;
                    // this.Text = count.ToString();
                    TurningOffTimer.Enabled = false;
                    TurningOffTimer.Interval = 1000;
                }
            } else
            {
                // this.Text = count.ToString();
                //this.Text = count.ToString();
                count++;
            }
        }
        private void MarkButton_Click(object sender, EventArgs e) {
            Button CLicked = (Button)sender;
            count = 0;
        }
        private void PupilName_TextChanged(object sender, EventArgs e) {
            count = 0;
        }
        private void WeekTitle_TextChanged(object sender, EventArgs e) {
            count = 0;
        }
        private void DayValue_TextChanged(object sender, EventArgs e) {
            count = 0;
        }
        private void Marks_lbl_Click(object sender, EventArgs e) {
            if (Clicked)
            {
                HoveredEvent = true;
                Clicked = false;
                timer.Enabled = true;
            } else
            {
                count = tick;
                TurningOffTimer.Interval = 1;
                Clicked = true;
            }
        }
        private void Publish_Click_Click(object sender, EventArgs e) { // INSERT [NextGenSchoolSystem_Diary].[dbo].[AllMarks] VALUES('','','')
            if (OpenedWithHovering)
            {//5
                string week = comboBox4.Text.Split(',')[0].Substring(0, comboBox4.Text.Split(',')[0].Length - 5);
                string Day = GetDay(chosen.Size.Height);
                string Index = chosen.Text.Split('.')[0] + "," + Day + ","
                + week + "," + Pupil.Text + "," + CurrentClass + "," + CurrentSchool;
                this.Text = Index;
                if (ContainsClild(Pupil.Text.Split(' ')[0], Pupil.Text.Split(' ')[1], CurrentSchool, CurrentClass, CurrentEmail, false))
                {
                    if (CurrentMark.Text != "")
                    {
                        if (!Marks.ContainsKey(Index))
                            Marks.Add(Index, CurrentMark.Text + ":" + comboBox5.Text + ":" + comboBox6.Text.Split('.')[1].Substring(1));
                        else
                            Marks[Index] = CurrentMark.Text + ":" + comboBox5.Text + ":" + comboBox6.Text.Split('.')[1].Substring(1);


                        using (SqlConnection sql = new SqlConnection(connection))
                        {
                            sql.Open();

                            using (SqlCommand doing = new SqlCommand("DELETE FROM " +
                            DataBaseTitle + ".[dbo].[PupilsMarks]", sql))
                            {
                                doing.ExecuteNonQuery();
                            }
                            using (SqlCommand add = new SqlCommand("INSERT [NextGenSchoolSystem_Diary].[dbo].[AllMarksDB] VALUES(" +
                                "'" + CurrentMark.Text + "','" + comboBox6.Text.Split('.')[1].Substring(1) + "','" + CurrentPupilName + "'" +
                                ",'" + Index + "')", sql))
                            {
                                add.ExecuteNonQuery();
                            }
                            foreach (KeyValuePair<string, string> entry in Marks)
                            {
                                string AddNew = "INSERT " + DataBaseTitle + ".[dbo].[PupilsMarks] VALUES('"
                                    + entry.Value + "','"
                                    + entry.Key + "')";
                                SqlCommand command = new SqlCommand(AddNew, sql);
                                command.ExecuteNonQuery();
                                this.Text = entry.Key + "+" + entry.Value;
                            }
                        }
                    }
                } else
                {
                    MessageBox.Show("Unknown pupil", "Unknown pupil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void button14_Click(object sender, EventArgs e) {
            if (!ContainsClild(name.Text, surname.Text, CurrentSchool, CurrentClass, email.Text))
            {
                bool RightFormat = true;
                int CurrentAccesLevel = 44;
                DialogResult name_res;
                DialogResult surname_res;
                switch (comboBox7.Text)
                {
                    case "Director":
                    CurrentAccesLevel = 0;
                    break;
                    case "Teacher":
                    CurrentAccesLevel = 1;
                    break;
                    case "Pupil":
                    CurrentAccesLevel = 2;
                    break;
                    case "TeaherHelper":
                    CurrentAccesLevel = 3;
                    break;
                    case "Parents":
                    CurrentAccesLevel = 4;
                    break;
                }
                try
                {
                    if (name.Text.Split(' ').Length > 1)
                    {
                        name_res = MessageBox.Show("There is a space in name,do you want to save only first name's part?",
                            "Format error",
                            MessageBoxButtons.YesNo);

                        if (name_res == DialogResult.Yes)
                            name.Text = name.Text.Split(' ')[0];
                        else
                            RightFormat = false;
                    }

                    if (surname.Text.Split(' ').Length > 1)
                    {
                        surname_res = MessageBox.Show("There is a space in name,do you want to save only first name's part?",
                              "Format error",
                              MessageBoxButtons.YesNo);

                        if (surname_res == DialogResult.Yes)
                            surname.Text = surname.Text.Split(' ')[0];
                        else
                            RightFormat = false;
                    }
                }
                catch (Exception) { }
                if (RightFormat)
                {
                    id.Text = name.Text.GetHashCode() + "NGSS" + email.GetHashCode() + "NGSS" + surname.GetHashCode();
                    Pupils.Add(name.Text);
                    Pupil.Items.Add(name.Text);


                    try
                    {
                        MailAddress from = new MailAddress("bulanceaanton@gmail.com");
                        MailAddress to = new MailAddress(email.Text);

                        MailMessage message = new MailMessage(from, to);
                        message.Subject = "Your NGSS Id";
                        message.Body = "Hello," + name.Text + " " + surname.Text + ".Your teacher have regestrated you in out NGSS system!\n" +
                            "\nOn this link: testlink.com you can watch video about NGSS and how to work with it!" +
                            "To regestrate in NGSS you need your id,here it's: " + id.Text + "\n Thanks and have a nice day!";
                        message.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                        smtp.Credentials = new NetworkCredential("bulanceaanton@gmail.com", "anton2007");
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unknown email format", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        RightFormat = false;
                    }
                }
                if (RightFormat)
                {
                    bool hasRows = false;
                    using (SqlConnection sql = new SqlConnection(connection))
                    {
                        sql.Open();
                        SqlCommand command = new SqlCommand(@"SELECT * FROM[NextGenSchoolSystem_Diary].[dbo].Accounts
                    WHERE
                    UserName = '" + name.Text + @"' AND
                    UserSurName = '" + surname.Text + @"' AND
                    UserEmail = '" + email.Text + @"' AND
                    Id = '" + name.Text.GetHashCode() + "NGSS" + email.GetHashCode() + "NGSS" + surname.GetHashCode() + "'", sql);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            hasRows = true;
                        }
                    }
                    if (!hasRows)
                    {

                        using (SqlConnection sql = new SqlConnection(connection))
                        {
                            sql.Open();
                            SqlCommand command = new SqlCommand("INSERT NextGenSchoolSystem_Diary.dbo.Accounts VALUES" +
                                "('" + name.Text + "'" +
                                ",'" + surname.Text + "',' '," +
                                "'" + email.Text + "'," +
                                "'" + tel.Text + "'," +
                                "'" + CurrentAccesLevel + "'," +
                                "'" + CurrentClass + "'," +
                                "'" + CurrentSchool + "','" + name.Text.GetHashCode() + "NGSS" + email.GetHashCode()
                                + "NGSS" + surname.GetHashCode() + "',NULL,'','',0)", sql);
                            command.ExecuteNonQuery();
                            MessageBox.Show(name.Text + " " + surname.Text + " joined us in " + CurrentSchool + ",his regestration id sended to his email",
                               name.Text + " " + surname.Text + " joined us!");
                        }
                    }
                }
            } else
            {
                MessageBox.Show("The same pupil is already registrated (the same email/the same name)", "The same pupil is already registrated");
            }
        }
        private void MarkButton_Clicked(object sender, MouseEventArgs e) {
            Button Mark = (Button)sender;
            CurrentMark = Mark;
        }
        private void button15_Click(object sender, EventArgs e) {
            this.Text = ClickSetting.ToString();
            if (!ClickSetting)
            {
                ClickSetting = true;
                WindowAnim_Close.Enabled = false;
                OpenWindow = true;
                WindoAnim_Open.Enabled = true;
            } else
            {
                ClickSetting = false;
                WindowAnim_Close.Enabled = true;
                WindoAnim_Open.Enabled = false;
                CloseWindow = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e) {
            if (OpenWindow && this.Size.Width < 1100)
            {
                this.Size = new Size(this.Size.Width + 10, this.Size.Height);
            }
        }
        private void timer2_Tick(object sender, EventArgs e) {
            if (CloseWindow && this.Size.Width > 790)
            {
                this.Size = new Size(this.Size.Width - 10, this.Size.Height);
            }
        }
        private void WeekLabel_Hover(object sender, EventArgs e) {
            comboBox4.Text = ((Label)sender).Text;
        }
        private void button17_Click(object sender, EventArgs e) {
            if (comboBox1.Text.Equals("-"))
                tick = 10000;
            else
                tick = Int32.Parse(comboBox1.Text);
        }
        private void SettingWnd_Open_Tick(object sender, EventArgs e) {
            if (SettingOpen_Click && groupBox11.Location.Y < 0)
            {
                groupBox11.Location = new Point(groupBox11.Location.X, groupBox11.Location.Y + 10);
            } else
            {
                SettingOpen_Click = false;
                SettingWnd_Open.Enabled = false;
            }
        }
        private void SettingWnd_Close_Tick(object sender, EventArgs e) {
            if (SettingOpen_Click && groupBox11.Location.Y > -groupBox11.Size.Height + 20)
            {
                groupBox11.Location = new Point(groupBox11.Location.X, groupBox11.Location.Y - 10);
            } else
            {
                SettingOpen_Click = false;
                SettingWnd_Close.Enabled = false;
            }
        }
        private void Settings_Click(object sender, EventArgs e) {
            if (Open)
            {
                SettingWnd_Open.Enabled = true;
                SettingOpen_Click = true;
                SettingWnd_Close.Enabled = false;
                Open = false;
            } else
            {
                Open = true;
                SettingWnd_Close.Enabled = true;
                SettingWnd_Open.Enabled = false;
                SettingOpen_Click = true;
            }
        }
        private void button16_Click(object sender, EventArgs e) {
            if (comboBox1.Text.Equals("-"))
                tick = 10000;
            else
                tick = Int32.Parse(comboBox1.Text);

            comboBox5.Items.Add(textBox5.Text);
        }
        private void button18_Click(object sender, EventArgs e) {
            comboBox5.Items.Add(textBox5.Text);
        }
        private void button19_Click(object sender, EventArgs e) {
            if (!ClickSetting)
            {
                ClickSetting = true;
                WindowAnim_Close.Enabled = false;
                OpenWindow = true;
                WindoAnim_Open.Enabled = true;
            } else
            {
                ClickSetting = false;
                WindowAnim_Close.Enabled = true;
                WindoAnim_Open.Enabled = false;
                CloseWindow = true;
            }
        }
        private void LogOut_Click(object sender, EventArgs e) {

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (OpenMarkWndWithHovering)
            {
                OpenMarkWndWithHovering = false;
            } else
            {
                OpenMarkWndWithHovering = true;
            }
        }
        private void OpenLogWnd_Click(object sender, EventArgs e) {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            groupBox7.Enabled = false;
            groupBox8.Enabled = false;
            groupBox9.Enabled = false;
            groupBox10.Enabled = false;
            groupBox11.Enabled = false;

            Prev.Enabled = false;
            Next.Enabled = false;
            CloseButton.Enabled = false;

            groupBox12.Visible = true;
        }
        private void OpenChatsWnd_Click(object sender, EventArgs e) {

        }
        private void button23_Click(object sender, EventArgs e) {
            groupBox1.Enabled = !false;
            groupBox2.Enabled = !false;
            groupBox3.Enabled = !false;
            groupBox4.Enabled = !false;
            groupBox5.Enabled = !false;
            groupBox6.Enabled = !false;
            groupBox7.Enabled = !false;
            groupBox8.Enabled = !false;
            groupBox9.Enabled = !false;
            groupBox10.Enabled = !false;
            groupBox11.Enabled = !false;

            Prev.Enabled = !false;
            Next.Enabled = !false;
            CloseButton.Enabled = !false;

            groupBox12.Visible = false;
        }
        private void RegButton_Click(object sender, EventArgs e) {
            new RegistrateNewAcc().Show();
        }
        private void LognButton_Click(object sender, EventArgs e) {

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_Diary.dbo.Accounts WHERE UserEmail " +
                    "= '" + textBox6.Text + "' " +
                    "AND UserPassword = '" + textBox7.Text + "'", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string Name = reader.GetString(0);
                        string SurName = reader.GetString(1);

                        string Class = reader.GetString(6);
                        string School = reader.GetString(7);
                        string Email = reader.GetString(3);
                        string Password = reader.GetString(2);
                        int AccesLevel_ = Int16.Parse(reader.GetString(5));
                        string Id = reader.GetString(8);

                        MessageBox.Show("Hello," + Name + "," + SurName, "Logining complete");

                        CurrentSchool = School;
                        CurrentPupilName = Name + " " + SurName;
                        currentWeekNumber = 1;
                        AccesLevel = AccesLevel_;
                        CurrentClass = Class;
                        CurrentEmail = Email;
                        CurrentPass = Password;
                        CurrentId = Id;
                        WriteIntoSIFile(currentWeekNumber.ToString());
                        //this.Close(); //Closes
                        //InitNgss(); // Reloads
                    }
                }
            }
        }
        private void ForgottenPass_Click(object sender, EventArgs e) {
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_Diary.dbo.Accounts", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(3).Equals(textBox6.Text))
                        {
                            MessageBox.Show("We've send code to your email,please fill it in this window and write new pass",
                                "Password changing");

                            MailAddress from = new MailAddress("bulanceaanton@gmail.com");
                            MailAddress to = new MailAddress(textBox6.Text);

                            MailMessage message = new MailMessage(from, to);
                            message.Subject = "Your pass change code";
                            var rand = new Random();
                            string code = rand.Next().ToString();

                            message.Body = "Change password with " + code + "";
                            message.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                            smtp.Credentials = new NetworkCredential("bulanceaanton@gmail.com", "anton2007");
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                            ChangePass change = new ChangePass(connection);
                            change.Show();

                            using (SqlConnection slq = new SqlConnection(connection))
                            {
                                slq.Open();
                                SqlCommand command1 = new SqlCommand("INSERT [NextGenSchoolSystem_Diary].[dbo].[ChangePass]" +
                                    "VALUES ('" + textBox6.Text + "','" + code + "')", slq);
                                command1.ExecuteNonQuery();

                            }
                            break;
                        }
                    }
                }
            }
        }
        private void button15_Click_2(object sender, EventArgs e) {
            new Achievements(AllLessons, connection, CurrentPupilName, AccesLevel, CurrentEmail, CurrentPass).Show();
        }
        private void About_Click(object sender, EventArgs e) {
            new AboutBox1().Show();
        }
        private void button20_Click(object sender, EventArgs e) {
        }
        private void button20_Click_1(object sender, EventArgs e) {
            int i = 0;
            using (SqlConnection sql = new SqlConnection(connection))
            {
                // MessageBox.Show(CurrentPupilName.Split(' ')[0], CurrentPupilName.Split(' ')[1]);
                sql.Open();
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.ShowDialog();
                string way = openFile.FileName;
                SqlCommand command1 = new SqlCommand(@"UPDATE NextGenSchoolSystem_Diary.dbo.Accounts
                    SET Photo = @ImageData" +
                  " WHERE" +
                  " UserName = '" + CurrentPupilName.Split(' ')[0] + "'" +
                  " AND UserSurName = '" + CurrentPupilName.Split(' ')[1] + "'" +
                  " AND UserEmail = '" + CurrentEmail + "' " +
                  "AND id = '" + CurrentId + "'", sql);
                //MessageBox.Show(CurrentPupilName.Split(' ')[0] + "|" + CurrentPupilName.Split(' ')[1],
                //  CurrentEmail + "|" + CurrentId);

                command1.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);
                if (way != "")
                {
                    byte[] imageData;
                    using (System.IO.FileStream fs = new System.IO.FileStream(way, FileMode.Open))
                    {
                        imageData = new byte[fs.Length];
                        fs.Read(imageData, 0, imageData.Length);
                    }
                    command1.Parameters["@ImageData"].Value = imageData;
                }
                i = command1.ExecuteNonQuery();
            }
            if (i >= 1)
                MessageBox.Show("Your Image was updated", "Image upd");
            else
                MessageBox.Show("Your Image was not updated", "Image upd error");
        }
        private void SocialNetwork_Click(object sender, EventArgs e) {
            Social_Network network = new Social_Network(CurrentPupilName.Split(' ')[0], CurrentPupilName.Split(' ')[1],
                CurrentEmail, CurrentPass, CurrentSchool, CurrentClass, connection, true);
            network.Show();
        }
        private void CloseForm(EventArgs e) {
        }
        private void button21_Click(object sender, EventArgs e) {
            MessageBox.Show("You are in a test mode now,please reg new acc later", "Test mode activated");
            isTest = true;

            CloseButton.Enabled = false;

            CurrentClass = "NGSS Class";
            CurrentSchool = "RoboCode";
            CurrentEmail = "bulanceaanton@gmail.com";
            CurrentPass = "new pass";
            CurrentId = "new antonId";
            CurrentPupilName = "Anton Bulancea";
            AccesLevel = 0;

            groupBox1.Enabled = !false;
            groupBox2.Enabled = !false;
            groupBox3.Enabled = !false;
            groupBox4.Enabled = !false;
            groupBox5.Enabled = !false;
            groupBox6.Enabled = !false;
            groupBox7.Enabled = !false;
            groupBox8.Enabled = !false;
            groupBox9.Enabled = !false;
            groupBox10.Enabled = !false;
            groupBox11.Enabled = !false;

            Prev.Enabled = !false;
            Next.Enabled = !false;
            CloseButton.Enabled = !false;

            groupBox12.Visible = !true;

            button23.Enabled = !false;

            button15.Enabled = !false;
            label10.Enabled = !false;
            InitNgss();
        }

        public void ShowLessonSettings() {
            groupBox6.Visible = true;
            groupBox7.Visible = true;
            groupBox8.Visible = true;
        }
        public void HideLessonSettings() {
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
        }
        public string ReadFromSIFile() {
            string text = "1";

            FileStream file = File.Open("SimpleInformationFile.txt", FileMode.OpenOrCreate);
            file.Close();

            text = File.ReadAllText("SimpleInformationFile.txt");
            return text;
        }
        public void WriteIntoSIFile(string CurrentWeek) {
            FileStream file = File.Open("SimpleInformationFile.txt", FileMode.OpenOrCreate);
            file.Close();
            File.WriteAllText("SimpleInformationFile.txt", CurrentWeek + "|" + CurrentPupilName + "|"
                + CurrentSchool + "|"
                + CurrentClass + "|"
                + AccesLevel.ToString() + "|" +
                CurrentEmail + "|" + CurrentPass + "|" + CurrentId);
        }
        private string GetDay(int size) {
            string Day = "";
            switch (size)
            {
                case 15:
                Day = "Monday";
                break;
                case 16:
                Day = "Tuesday";
                break;
                case 17:
                Day = "Wednesday";
                break;
                case 18:
                Day = "Thursday";
                break;
                case 19:
                Day = "Friday";
                break;
                case 20:
                Day = "Saturday";
                break;
            }
            return Day;
        }
        private void instNewLabel(string title, string day,
        string from, string till, bool TimeStatus = true) {
            string[] Position = new string[3];
            Position = LessonsPosition[day].Split(';');
            int xPos = Int32.Parse(Position[0]);
            int yPos = Int32.Parse(Position[1]);
            int count = Int32.Parse(Position[2]);

            LessonOnDay.Add(day);

            LessonTitles.Add(title);

            if (!comboBox6.Items.Contains(title))
                comboBox6.Items.Add(title);

            if (!comboBox3.Items.Contains(title))
            {
                comboBox3.Items.Add(title);
            }

            Label lbl = new Label();
            if (!TimeStatus)
                lbl.Text = (count + 1) + ". " + title;
            else
                lbl.Text = (count + 1) + ". " + comboBox3.Text;

            lbl.Font = new Font(Font.Font, FontStyle.Bold);

            lbl.Cursor = Cursors.Hand;
            int lblTextLenght = lbl.Text.Length;
            lbl.Location = new Point(xPos, yPos);
            lbl.Size = new Size(lblTextLenght * 10, 17);
            lbl.Click += new EventHandler(this.Lesson_Click);
            switch (day)
            {
                case "Monday":
                MondayPnl.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 15);
                break;
                case "Tuesday":
                TuesdayPnl.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 16);
                break;
                case "Wednesday":
                WednesdayPnl.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 17);
                break;
                case "Thursday":
                ThursdayPnl.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 18);
                break;
                case "Friday":
                FridayPnl.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 19);
                break;
                case "Saturday":
                SaturdayPanel.Controls.Add(lbl);
                lbl.Size = new Size(lblTextLenght * 10, 20);
                break;
            }

            count++;
            LessonsPosition[day] = xPos + ";" + (yPos + 16) + ";" + count;

            lbl.MouseHover += Lbl_MouseHover;

            if (LessonTimeTill.Count < count + 1)
            {
                LessonTimeFrom.Add("00:00:00");
                LessonTimeTill.Add("00:00:00");
            }
        }
        private bool ContainsClild(string name, string surname, string school, string class_, string email, bool a = true) {
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [NextGenSchoolSystem_Diary].[dbo].[Accounts]", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0).Equals(name) && reader.GetString(1).Equals(surname)
                            && reader.GetString(6).Equals(class_) && reader.GetString(7).Equals(school) ||
                            reader.GetString(3).Equals(email) && a)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void InitNgss() {

            MondayPnl.Controls.Clear();
            TuesdayPnl.Controls.Clear();
            WednesdayPnl.Controls.Clear();
            ThursdayPnl.Controls.Clear();
            FridayPnl.Controls.Clear();
            SaturdayPanel.Controls.Clear();

            LessonsPosition = new Dictionary<string, string>()
          {
            {"Monday","3;9;0"},
            {"Tuesday","5;9;0"},
            {"Wednesday","5;9;0"},
            {"Thursday","5;9;0" },
            {"Friday","5;5;0"},
            {"Saturday","5;5;0"}
        };

            LessonTimeFrom.Clear();
            LessonTimeTill.Clear();

            for (int i = 0; i < 30; i++)
            {
                LessonTimeFrom.Add("00:00");
                LessonTimeTill.Add("00:00");
            }

            Marks.Clear();

            comboBox2.Items.Add("Monday");
            comboBox2.Items.Add("Tuesday");
            comboBox2.Items.Add("Wednesday");
            comboBox2.Items.Add("Thursday");
            comboBox2.Items.Add("Friday");
            comboBox2.Items.Add("Saturday");

            for (int i = 25; i > 25; i--)
            {
                domainUpDown1.Items.Add(i);
                domainUpDown4.Items.Add(i);
            }
            for (int i = 61; i > 1; i--)
            {
                domainUpDown2.Items.Add(i);
                domainUpDown3.Items.Add(i);
            }
            bool Succes = true;
            using (SqlConnection sql = new SqlConnection(connection))
            {
                try
                {
                    sql.Open();
                }
                catch (SqlException) { Succes = false; }
                if (Succes)
                {
                    if (!TablesWereInst)
                    {
                        SqlCommand commanding = new SqlCommand(
                            "CREATE TABLE DiaryTable (Homework nvarchar(MAX),Lesson nvarchar(MAX),Diary nvarchar(MAX))" +
                            "CREATE TABLE [Lesson list] (Lesson_title nvarchar(MAX)," +
                            "TimeFrom nvarchar(MAX),TimeTill nvarchar(MAX),DayOfWeek nvarchar(MAX)) " +
                            "CREATE TABLE LessonTimeInformation (BeginingTime nvarchar(MAX)," +
                            "EndingTime nvarchar(MAX),LessonNumber nvarchar(MAX)) " +
                            "CREATE TABLE PupilsMarks (Mark nvarchar(MAX)," +
                            "DateIndex nvarchar(MAX)", sql);
                        TablesWereInst = true;
                        commanding.ExecuteNonQuery();

                    }

                    SqlCommand command = new SqlCommand("SELECT * FROM " + DataBaseTitle + ".[dbo].[Lesson list]", sql);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(4).Equals(CurrentSchool))
                            {
                                instNewLabel(reader.GetValue(0).ToString(), reader.GetValue(3).ToString(),
                                reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), false);

                                AllLessons.Add(reader.GetValue(0).ToString());
                            }
                        }
                    }

                    string[] array = new string[1000];
                    string[] array2 = new string[1000];
                    int count;
                    reader.Close();

                    SqlCommand command2 = new SqlCommand("SELECT * FROM " + DataBaseTitle + ".[dbo].[LessonTimeInformation]", sql);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            try
                            {
                                if (reader2.GetValue(0).ToString() != "" && reader2.GetValue(1).ToString() != "")
                                {
                                    LessonTimeFrom[Int16.Parse(reader2.GetValue(2).ToString())] = reader2.GetValue(0).ToString();
                                    LessonTimeTill[Int16.Parse(reader2.GetValue(2).ToString())] = reader2.GetValue(1).ToString();
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                    reader2.Close();

                    SqlCommand command3 = new SqlCommand("SELECT * FROM " + DataBaseTitle + ".[dbo].[PupilsMarks]", sql);
                    SqlDataReader reader3 = command3.ExecuteReader();
                    if (reader3.HasRows)
                    {
                        while (reader3.Read())
                        {
                            //5

                            Marks.Add(reader3.GetValue(1).ToString(), reader3.GetValue(0).ToString());
                            if (reader3.GetString(1).Split(',')[5].Equals(CurrentSchool) &&
                                reader3.GetString(1).Split(',')[3].Equals(CurrentPupilName)
                                )
                            {
                                AllMarks.Add(reader3.GetString(0));
                            }
                        }
                    }
                }
            }

            if (AccesLevel != 0)
            {
                groupBox10.Visible = false;
                groupBox6.Visible = false;
                groupBox7.Visible = false;
                groupBox8.Visible = false;
                button19.Enabled = false;
            }
            if (AccesLevel != 1)
            {
                checkBox2.Checked = false;
                label25.Enabled = false;
                label26.Enabled = false;
                label24.Enabled = false;
                label29.Enabled = false;
                checkBox2.Enabled = false;
                textBox5.Enabled = false;
                comboBox1.Enabled = false;
                button16.Enabled = false;
                button18.Enabled = false;
                button17.Enabled = false;
                groupBox9.Visible = false;
            }

            int execute = 0;
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM NextGenSchoolSystem_Diary.dbo.Accounts", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string Name = reader.GetString(0) + " " + reader.GetString(1);
                        string School = reader.GetString(7);
                        string AL = reader.GetString(5);
                        if (Name.Equals(CurrentPupilName) &&
                            School.Equals(CurrentSchool) &&
                            AL.Equals(AccesLevel.ToString()) &&
                            reader.GetString(3).Equals(CurrentEmail) && reader.GetString(2).Equals(CurrentPass))
                            execute++;
                    }
                }

            }

            if (execute == 0 && !isTest)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
                groupBox7.Enabled = false;
                groupBox8.Enabled = false;
                groupBox9.Enabled = false;
                groupBox10.Enabled = false;
                groupBox11.Enabled = false;
                SaturdayPanel.Enabled = false;
                SocialNetwork.Enabled = false;
                About.Enabled = false;

                Prev.Enabled = false;
                Next.Enabled = false;
                CloseButton.Enabled = false;

                groupBox12.Visible = true;

                button23.Enabled = false;

                button15.Enabled = false;
                label10.Enabled = false;
            }

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(@"UPDATE [NextGenSchoolSystem_Diary].[dbo].[Accounts] 
                    SET Activity = '" + "1" + @"' WHERE Classes = '" + CurrentClass + "' " +
                "AND School = '" + CurrentSchool + "' AND UserSurName = '" + CurrentPupilName.Split(' ')[1] + "' AND " +
                "UserName = '" + CurrentPupilName.Split(' ')[0] + "' AND UserEmail = '" + CurrentEmail + "' " +
                "AND UserPassword = '" + CurrentPass + "'", sql);
                command.ExecuteNonQuery();
            }
        }
    }
}