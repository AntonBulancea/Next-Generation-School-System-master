
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
        public bool teacherHelper = true;

        string CurrentPassword = "no";
        string CurrentClass = "6 C";
        string CurrentSchool = "School A. Puskin";
        public string CurrentPupilName = "Anton Bulancea";
        string CurrentEmail = "bulanceaanton@gmail.com";

        public MainWindow() {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e) {
            connection = "Data Source=" + ComputerName + ";Initial Catalog=" + DataBaseTitle + ";Integrated Security=True";
            if (ReadFromSIFile(1) != "")
            {
                currentWeekNumber = Int16.Parse(ReadFromSIFile(1));
                label10.Text = currentWeekNumber.ToString() + " week";
            }
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
                            instNewLabel(reader.GetValue(0).ToString(), reader.GetValue(3).ToString(),
                            reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), false);
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
                            LessonTimeFrom[Int16.Parse(reader2.GetValue(2).ToString())] = reader2.GetValue(0).ToString();
                            LessonTimeTill[Int16.Parse(reader2.GetValue(2).ToString())] = reader2.GetValue(1).ToString();

                            this.Text = LessonTimeFrom[1];
                        }
                    }

                    reader2.Close();

                    SqlCommand command3 = new SqlCommand("SELECT * FROM " + DataBaseTitle + ".[dbo].[PupilsMarks]", sql);
                    SqlDataReader reader3 = command3.ExecuteReader();
                    if (reader3.HasRows)
                    {
                        while (reader3.Read())
                        {
                            Marks.Add(reader3.GetValue(1).ToString(), reader3.GetValue(0).ToString());
                        }
                    }
                }
            }
            this.Text = CurrentPupilName;
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
            lf.SetLesson(LessonTimeFrom[Int16.Parse(titleArr[0])], LessonTimeTill[Int16.Parse(titleArr[0])],
                title, "",
                Clicked.Text.Split('.')[0] + "," + Day + ","
                + currentWeekNumber.ToString() + "," + CurrentClass + "," + CurrentSchool, currentWeekNumber.ToString(),
                connection);

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
                using (SqlCommand doing = new SqlCommand("DELETE FROM " +
                        DataBaseTitle + ".[dbo].[Lesson list]", sqlConnection))
                {
                    doing.ExecuteNonQuery();
                }
                for (int j = 0; j < LessonTitles.Count; j++)
                {
                    string Command = @"INSERT " + DataBaseTitle + ".[dbo].[Lesson list]" + @" 
                                      VALUES('" + LessonTitles[j] + "', '00:00', '00:00', '" + LessonOnDay[j] + "')";
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
                                       VALUES ('" + LessonTimeFrom[i] + "','" + LessonTimeTill[i] + "','" + i + "')";

                    SqlCommand command = new SqlCommand(Command, sqlConnection);

                    command.ExecuteNonQuery();
                }
            }
            WriteIntoSIFile(currentWeekNumber.ToString());
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
        private void Publish_Click_Click(object sender, EventArgs e) {
            if (OpenedWithHovering)
            {
                string Day = GetDay(chosen.Size.Height);
                string Index = chosen.Text.Split('.')[0] + "," + Day + ","
                + currentWeekNumber.ToString() + "," + Pupil.Text + "," + CurrentClass + "," + CurrentSchool;
                this.Text = Index;

                if (CurrentMark.Text != "")
                {
                    if (!Marks.ContainsKey(Index))
                        Marks.Add(Index, CurrentMark.Text + ":" + comboBox5.Text);
                    else
                        Marks[Index] = CurrentMark.Text + ":" + comboBox5.Text;

                    using (SqlConnection sql = new SqlConnection(connection))
                    {
                        sql.Open();

                        using (SqlCommand doing = new SqlCommand("DELETE FROM " +
                        DataBaseTitle + ".[dbo].[PupilsMarks]", sql))
                        {
                            doing.ExecuteNonQuery();
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
            }
        }
        private void button14_Click(object sender, EventArgs e) {
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("INSERT NextGenSchoolSystem_Diary.dbo.Accounts VALUES ('"
                    + textBox3.Text + "|" + CurrentClass + "|" + CurrentSchool + "','"
                    + textBox6.Text + "','"
                    + "','"
                    + textBox2.Text + "','"
                    + textBox4.Text + "')", sql);

                command.ExecuteNonQuery();
            }
        }
        private void MarkButton_Clicked(object sender, EventArgs e) {
            Button Mark = (Button)sender;
            CurrentMark = Mark;
            /*
            Mark10.Enabled = true;
            Mark9.Enabled = true;
            Mark8.Enabled = true;
            Mark7.Enabled = true;
            Mark6.Enabled = true;
            Mark5.Enabled = true;
            Mark4.Enabled = true;
            Mark3.Enabled = true;
            Mark2.Enabled = true;
            MarkA.Enabled = true;
            */
            Mark.Enabled = false;
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
        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            if (checkBox3.Checked)
            {
                textBox8.UseSystemPasswordChar = false;
            } else
            {
                textBox8.UseSystemPasswordChar = true;

            }
        }
        private void LogWndOpen_Click(object sender, EventArgs e) {
            this.Size = new Size(375, 550);
            LoginWnd.Location = new Point(12, 3);
        }

        private void ForgottenPass_Click(object sender, EventArgs e) {

        }
        private void LogIn_Click(object sender, EventArgs e) {
            string ConnectionString = connection;
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [NextGenSchoolSystem_Diary].[dbo].[Accounts]", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object UserName = reader.GetValue(0);
                        object UserSurName = reader.GetValue(1);
                        object UserPassword = reader.GetValue(2);
                        object UserEmail = reader.GetValue(3);
                        object Telephone = reader.GetValue(4);
                        this.Text = textBox8.Text + "|" + UserPassword + "," + textBox9.Text + "|" + UserEmail + ",";
                        if (textBox8.Text.Equals(UserPassword) && textBox9.Text.Equals(UserEmail))
                        {
                            CurrentSchool = UserName.ToString().Split('|')[2];
                            CurrentClass = UserName.ToString().Split('|')[1];
                            CurrentPupilName = UserName.ToString().Split('|')[0] + " " + UserSurName.ToString();
                            HelloLabel.Visible = true;
                            HelloLabel.Text = "" + UserName.ToString().Split('|')[0] + " " + UserSurName.ToString();
                            HelloLabel.Text = "Hello, " + UserName.ToString().Split('|')[0] + " " + UserSurName.ToString();
                            CurrentPassword = textBox8.Text;
                            CurrentEmail = UserEmail.ToString();

                        }
                    }
                }
            }
        }
        private void Reg_Click(object sender, EventArgs e) {
            Registrating reg = new Registrating();
            reg.Show();
        }
        private void Back_Click(object sender, EventArgs e) {
            this.Size = new Size(790, 550);
            LoginWnd.Location = new Point(12, 635);
        } //12; 623

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
        public string ReadFromSIFile(int Index) {
            string text = File.ReadAllText("SimpleInformationFile.txt");
            if (text.Length > 0)
            {
                CurrentPupilName = text.Split('|')[1].Split(',')[0];
                CurrentSchool = text.Split('|')[1].Split(',')[1];
                CurrentClass = text.Split('|')[1].Split(',')[2];

                CurrentEmail = text.Split('|')[2];
                CurrentPassword = text.Split('|')[3];

                this.Text = CurrentPupilName;

                return text.Split('|')[0];
            } else
            {
                return "1";
            }
        }
        public void WriteIntoSIFile(string CurrentWeek) {
            File.WriteAllText("SimpleInformationFile.txt", CurrentWeek + "|"
                + CurrentPupilName + "," + CurrentSchool + "," + CurrentClass + "|" +
                CurrentEmail + "|" + CurrentPassword);
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

        private void button19_Click_1(object sender, EventArgs e) {

        }

        private void CloseButton_Click(object sender, EventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {

        }

        private void button14_Click_1(object sender, EventArgs e) {

        }

        private void label27_Click(object sender, EventArgs e) {

        }
    }
}