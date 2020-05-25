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
        public LessonInformation() {
            InitializeComponent();
        }
        private void LessonInfo_Load(object sender, EventArgs e) {

        }
        public void SetLesson(string from, string till, string title, string homeWork, string week, string weekNumber, string connect) {
            label3.Text = from;
            label2.Text = till;
            label1.Text = title;
            connectioning = connect;
            lbl = new Label();
            MainWindow wnd = new MainWindow();

            if (!wnd.teacherHelper)
            {
                button2.Visible = false;
                button1.Visible = false;
            } else
            {
                button1.Visible = true;
                richTextBox1.Visible = true;
                this.Size = new Size(950, 400);
            }
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
                        this.Text = date + " " + CurrentWeek;
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
                            string Index2 = indentificator[0] + "," + indentificator[1]
                            + "," + weekNumber + "," + File.ReadAllText("SimpleInformationFile.txt").Split('|')[1].Split(',')[0] + "," + indentificator[3] + "," + indentificator[4];
                            this.Text = Index + " && " + Index2;
                            if (Index.Equals(Index2))
                            {
                                this.Text = "got";
                                label5.Text = Mark.ToString().Split(':')[0];
                                label6.Text = Mark.ToString().Split(':')[1];
                            }
                        }
                    }
                }
                groupBox2.Controls.Add(lbl);
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
    }
}


