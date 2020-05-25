using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Next_Generation_School_System_by_Anton {
    public partial class Achievements : Form {
        List<string> Notes = new List<string>();

        int x = 13, y = 0, x1 = 11, plusx = 20, plus = 41 - 13, lblPlus = 10;
        Dictionary<string, string> Index = new Dictionary<string, string>();
        string connectioning;
        string CurrentEmail = "";
        string CurrentPass = "";
        string CurrentName = "";
        int AccesLvl = 4;
        private void groupBox1_Enter(object sender, EventArgs e) {

        }
        public Achievements(List<string> b, string c, string p,int al,string email,string pass) {
            CurrentEmail = email;
            CurrentPass = pass;
            InitializeComponent();
            SetAchiments(b, c, p, al);
        }
        private void Achievements_Load(object sender, EventArgs e) {

        }
        public void SetAchiments(List<string> Lessons, string connection, string PupilName,int AccesLevel) {
            Dictionary<string, string> Marks = new Dictionary<string, string>();
            AccesLvl = AccesLevel;
            connectioning = connection;
            int MarksCount = 0;
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [NextGenSchoolSystem_Diary].[dbo].[AllMarksDB]", sql);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (PupilName.Equals(reader.GetString(2)))
                        {
                            if (!Marks.ContainsKey(reader.GetString(1)))
                            {
                                MarksCount++;
                                Marks.Add(reader.GetString(1), reader.GetString(0) + ":" + reader.GetString(3).Split(',')[2] + ",");
                                Index.Add(reader.GetString(1) + "|" + reader.GetString(3).Split(',')[2], reader.GetString(3) + ",");

                            } else
                            {
                                MarksCount++;
                                Marks[reader.GetString(1)] += reader.GetString(0) + ",";
                                Index[reader.GetString(1) + "|" + reader.GetString(3).Split(',')[2]] += reader.GetString(3) + ",";
                            }
                        }
                    }
                }
            }
            double AllMark = 0;
            int count = 0;
            for (int i = 0; i < Lessons.Count; i++)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Location = new Point(x, y);

                lbl.Text = Lessons[i];
                panel1.Controls.Add(lbl);
                x1 = lbl.Text.Length * 12;

                lbl.Font = new Font(test.Font, FontStyle.Bold);

                Label Mark = new Label();
                Mark.AutoSize = true;
                Mark.Location = new Point(x1, y);
                panel1.Controls.Add(Mark);

                for (int j = 0; j < MarksCount; j++)
                {
                    if (Marks.ContainsKey(lbl.Text))
                    {
                        try
                        {
                            Mark.Text += Marks[lbl.Text].Split(',')[j].Split(':')[0] + "  ";
                            // x1 += 20;
                            Mark.Font = new Font(mark_test.Font, FontStyle.Bold);
                            Mark.ForeColor = Color.Crimson;
                            if (!Marks[lbl.Text].Split(',')[j].Equals(""))
                            {
                                MarksCount++;
                                Mark.Name = lbl.Text + "|" + Marks[lbl.Text].Split(',')[j].Split(':')[1];
                                AllMark += Double.Parse(Marks[lbl.Text].Split(',')[j].Split(':')[0]);
                                Mark.Click += new EventHandler(this.Mark_Click);
                            }
                        }
                        catch (Exception) { }
                    }
                }
                y += plus;
            }
            double res = (AllMark / (MarksCount / 2));
            label1.Text = "Average: " + res.ToString();
        }
        public void Mark_Click(object sender, EventArgs e) {
            Label lbl = (Label)sender;
            LessonInformation lesson = new LessonInformation();
            lesson.Show();
            MessageBox.Show(AccesLvl.ToString());
            lesson.SetLesson("", "", lbl.Name.Split('|')[0],
                "",Index[lbl.Name], lbl.Name.Split('|')[1],connectioning,CurrentName,AccesLvl,CurrentEmail,CurrentPass,true);
        }
    }
}
