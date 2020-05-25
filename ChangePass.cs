using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Next_Generation_School_System_by_Anton {
    public partial class ChangePass : Form {
        string connection;
        public ChangePass(string conn) {
            connection = conn;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [NextGenSchoolSystem_Diary].[dbo].[ChangePass]", sql);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0).Equals(textBox1.Text) && reader.GetString(1).Equals(textBox2.Text))
                        {
                            using (SqlConnection sql1 = new SqlConnection(connection))
                            {
                                sql1.Open();
                                SqlCommand command1 = new SqlCommand("UPDATE [NextGenSchoolSystem_Diary].[dbo].Accounts " +
                                    "SET UserPassword = '" + textBox3.Text + "' " +
                                    "WHERE UserEmail = '" + textBox1.Text + "' ", sql1);
                                command1.ExecuteNonQuery();
                                using (SqlCommand command2 = new SqlCommand("DELETE FROM [NextGenSchoolSystem_Diary].[dbo].[ChangePass]" +
                                   "WHERE Id = '" + textBox2.Text + "'" , sql1))
                                {
                                    command2.ExecuteNonQuery();
                                    MessageBox.Show("We've send code to your email,please fill it in this window and write new pass",
                                "Password changing");

                                    MailAddress from = new MailAddress("bulanceaanton@gmail.com");
                                    MailAddress to = new MailAddress(textBox1.Text);

                                    MailMessage message = new MailMessage(from, to);
                                    message.Subject = "Your pass change code";
                                    var rand = new Random();
                                    string code = rand.Next().ToString();

                                    message.Body = "Your password was changed,if you didn't do it,please write to us";
                                    message.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                                    smtp.Credentials = new NetworkCredential("bulanceaanton@gmail.com", "anton2007");
                                    smtp.EnableSsl = true;
                                    smtp.Send(message);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
