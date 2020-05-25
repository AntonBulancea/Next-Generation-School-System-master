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
    public partial class RegistrateNewAcc : Form {
        public RegistrateNewAcc() {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) {
            MainWindow wnd = new MainWindow();
            string connection = "Data Source=" + wnd.ComputerName + ";" +
                    "Initial Catalog=" + wnd.DataBaseTitle + ";Integrated Security=True";
            string Name = name.Text;
            string Surname = surname.Text;
            string Email = email.Text;
            string Id = id.Text;
            string Password = pass.Text;

            int updated = 0;

            using (SqlConnection sql = new SqlConnection(connection))
            {
              var a = MessageBox.Show("Do you want to set your profile photo right now?","Photo question",MessageBoxButtons.YesNo);
                sql.Open();
                SqlCommand command = new SqlCommand(@"UPDATE NextGenSchoolSystem_Diary.dbo.Accounts
                    SET UserPassword = '" + Password + "'" +
                    " WHERE" +
                    " UserName = '" + Name + "'" +
                    " AND UserSurName = '" + Surname + "'" +
                    " AND UserEmail = '" + Email + "' " +
                    "AND id = '" + Id + "'", sql);
                if (a == DialogResult.Yes)
                {
                  OpenFileDialog openFile = new OpenFileDialog();
                openFile.ShowDialog();
                string way = openFile.FileName;
                SqlCommand command1 = new SqlCommand(@"UPDATE NextGenSchoolSystem_Diary.dbo.Accounts
                    SET Photo = @ImageData" +
                  " WHERE" +
                  " UserName = '" + Name + "'" +
                  " AND UserSurName = '" + Surname + "'" +
                  " AND UserEmail = '" + Email + "' " +
                  "AND id = '" + Id + "'", sql);

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
                    command1.ExecuteNonQuery();
                   
                }
                command.ExecuteNonQuery();
                updated = command.ExecuteNonQuery();
            }
            if (updated > 0)
                MessageBox.Show("Hello, " + Name + " " + Surname, "Regestration complete");
            else
                MessageBox.Show("Sorry, " + Name + " " + Surname + ",you are not in a list of courses", "Regestration failed");

            if (updated > 0 && checkBox2.Checked)
            {

                using (SqlConnection sql = new SqlConnection(connection))
                {
                    sql.Open();
                    SqlCommand command = new SqlCommand("INSERT NextGenSchoolSystem_Diary.dbo.Accounts VALUES" +
                        "('" + Email + "','" + Name + " " + Surname + "')", sql);
                    updated = command.ExecuteNonQuery();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e) {
            MainWindow wnd = new MainWindow();
            string connection = "Data Source=LAPTOP;Initial Catalog=NextGenSchoolSystem_Diary;Integrated Security=True";
            string Title = stitle.Text;
            string Email = semail.Text;
            string Tel = stel.Text;
            string DirName = dname.Text;
            string DirSurname = dsurname.Text;
            string DirPass = dpass.Text;

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(@"INSERT NextGenSchoolSystem_Diary.dbo.Accounts VALUES 
                ('" + DirName + "','"
                + DirSurname + "'," +
                "'" + DirPass + "'," +
                "'" + Email + "'," +
                "'" + Tel + "'," +
                "'0'," +
                "'No'," +
                "'" + Title + "'," +
                "'" + Title.GetHashCode() + DirSurname.GetHashCode() + "',NULL,'','',0)", sql);
                
                command.ExecuteNonQuery();
            }

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand(@"INSERT NextGenSchoolSystem_Diary.dbo.SchoolTable VALUES 
                ('" + Title + "','"
                + Email + "'," +
                "'" + Tel + "'," +
                "'false','0')",sql);
                command.ExecuteNonQuery();
            }
            MessageBox.Show("You've regestrated " + Title + " as a school!" +
                "Now you are it's director,in main window you can add new pupil (for more info come: www.ihaventsitenowsojustwritemeinvk.com)" +
                "Your school isn't regestrated official now,send us your building and command photo on email nextgenerationschoolsystem@gmail.com" +
                "Thanks for using NGSS!", "School Regestrated");

            FileStream file = File.Open("SimpleInformationFile.txt", FileMode.OpenOrCreate);
            file.Close();
            File.WriteAllText("SimpleInformationFile.txt", "1" + "|" + DirName + " " + DirSurname + "|"
                + Title + "|"
                + "No" + "|"
                + "0|" + Email + "|" + dpass.Text + "|" + Title.GetHashCode() + DirSurname.GetHashCode()
                );
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked)
            {
                groupBox2.Text = "Registrate new course";
            } else
            {
                groupBox2.Text = "Registrate new school";
            }
        }
    }
}
