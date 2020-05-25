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
    public partial class Registrating : Form {
        public Registrating() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            string Name = textBox1.Text;
            string SurName = textBox2.Text;
            string Email = textBox4.Text;
            string Password = textBox3.Text;

            MainWindow wnd = new MainWindow();
            string ConnectionString = "Data Source=LAPTOP;Initial Catalog=NextGenSchoolSystem_Diary;Integrated Security=True";
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

                        if (UserEmail.ToString().Equals(Email)
                            && UserSurName.ToString().Equals(SurName)
                            && UserName.ToString().Split('|')[0].Equals(Name))
                        {
                            this.Text = "hello";
                            reader.Close();
                            using (SqlCommand command1 = new SqlCommand(@"UPDATE
                               [NextGenSchoolSystem_Diary].[dbo].[Accounts]
                                SET UserPassword = '" + Password + @"'
                                WHERE UserName = '" + UserName + @"' AND
                                UserSurName = '" + SurName + "' AND UserEmail = '" + Email + "'", sql))
                            {
                                command1.ExecuteNonQuery();
                            }
                            break;
                        } else
                        {

                        }

                    }
                }
            }
        }
    }
}
