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

namespace SuperMarket
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
            ShowEmp();
        }
        public void Pop_Alert(String msg, Alert.Enum_type type)
        {
            Alert frm = new Alert();
            frm.showAlert(msg,type);

        }
        private void Clear()
        {
            UserName.Text = "";
            Phone.Text = "";
            Adress.Text = "";
            Password.Text = "";
            key = 0;
        }


        SqlConnection Connexion = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\fardo\Documents\SuperMarket.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowEmp()
        {
            Connexion.Open();
            string query = "select * from Employee";
            SqlDataAdapter sda = new SqlDataAdapter(query,Connexion);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var DS = new DataSet();
            sda.Fill(DS);
            EmployeeDGV.DataSource = DS.Tables[0];
            Connexion.Close();

        }

        private void Exit_Btn_Click(object sender, EventArgs e)
        {
            Exit E = new Exit();
            E.Show();
        }

            
        
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (UserName.Text == "" || Phone.Text == "" || Adress.Text == "" || Password.Text == "")
            {
                this.Pop_Alert("Missing Informations",Alert.Enum_type.info);
                //MessageBox.Show();
            }
            else
            {
                try
                {
                    Connexion.Open();
                    SqlCommand cmd = new SqlCommand("Insert into Employee values('" + UserName.Text + "','" + Phone.Text + "','" + Adress.Text + "','" + Password.Text + "')", Connexion);
                    cmd.ExecuteNonQuery();
                    Pop_Alert("Employee Added Successfully", Alert.Enum_type.success);
                    //MessageBox.Show("Employee Added Successfully");
                    Connexion.Close();
                    ShowEmp();
                    Clear();
                }
                catch (Exception exp)
                {
                    if(exp.Message.Substring(0,67) == "Violation of UNIQUE KEY constraint 'UQ__Employee__C9F28456F42E8A26'")
                    {
                        Pop_Alert("Insert a valid username ,"+UserName.Text+" already exists!", Alert.Enum_type.error);
                    }
                    else if (exp.Message.Substring(0,57) == "The INSERT statement conflicted with the CHECK constraint")
                    {
                        Pop_Alert("Insert a valid phone number (starts with 06 or 07)", Alert.Enum_type.error);
                        
                    }
                    else
                    {
                        Pop_Alert("Insert a valid phone number ," + Phone.Text + " already exists!", Alert.Enum_type.error);
                    }
                }
                finally
                {
                    Connexion.Close();
                }

            }
        }

        
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }

        int key = 0;
        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UserName.Text = EmployeeDGV.SelectedRows[0].Cells[1].Value.ToString();
            Phone.Text = EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString();
            Adress.Text = EmployeeDGV.SelectedRows[0].Cells[3].Value.ToString();
            Password.Text = EmployeeDGV.SelectedRows[0].Cells[4].Value.ToString();
            if(UserName.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                this.Pop_Alert("Select the employee to be delete!", Alert.Enum_type.info);
                //MessageBox.Show("Select the employee to be delete!");
            }
            else
            {
                try
                {
                    Connexion.Open();
                    string query = "Delete from Employee where ID = "+key+";";
                    SqlCommand cmd = new SqlCommand(query, Connexion);
                    cmd.ExecuteNonQuery();
                    Pop_Alert("Employee named : " + UserName.Text + " deleted Successfully", Alert.Enum_type.success);
                    //MessageBox.Show("Employee named : "+ UserName.Text+" deleted Successfully");
                    Connexion.Close();
                    ShowEmp();
                    Clear();
                }
                catch (Exception exp)
                {
                    Pop_Alert(exp.Message.Substring(0, 50), Alert.Enum_type.error);
                    //MessageBox.Show(exp.Message);
                }
                finally
                {
                    Connexion.Close();
                }

            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (UserName.Text == "" || Phone.Text == "" || Adress.Text == "" || Password.Text == "")
            {
                this.Pop_Alert("Select the employee to be Updated!", Alert.Enum_type.info);
                //MessageBox.Show("Select the employee to be Updated!");
            }
            else
            {
                
                try
                {

                    Connexion.Open();
                    string query = "Update Employee Set Username = '" + UserName.Text + "',Phone = '" + Phone.Text + "',Adress = '" + Adress.Text + "', Password = '" + Password.Text + "' where ID = " + key + ";";
                    SqlCommand cmd = new SqlCommand(query, Connexion);
                    
                    cmd.ExecuteNonQuery();
                    Connexion.Close();
                    Pop_Alert("Employee named : " + UserName.Text + " informations are updated Successfully", Alert.Enum_type.success);
                    //MessageBox.Show("Employee informations are updated Successfully"); 
                    ShowEmp();
                    Clear();
                    

                } catch (SqlException exp)
                {
                    if (exp.Message.Substring(0, 67) == "Violation of UNIQUE KEY constraint 'UQ__Employee__C9F28456F42E8A26'")
                    {
                        Pop_Alert("Insert a valid username ," + UserName.Text + " already exists!", Alert.Enum_type.error);
                    }
                    else if (exp.Message.Substring(0, 57) == "The INSERT statement conflicted with the CHECK constraint")
                    {
                        Pop_Alert("Insert a valid phone number (starts with 06 or 07)", Alert.Enum_type.error);

                    }
                    else
                    {
                        Pop_Alert("Insert a valid phone number ," + Phone.Text + " already exists!", Alert.Enum_type.error);
                    }
                    
                    //MessageBox.Show(exp.Message);

                }
                finally
                {
                    Connexion.Close();
                }
               
            }
        }

        private void Log_Out_Click(object sender, EventArgs e)
        {

        }

        private void Products_Click(object sender, EventArgs e)
        {
            Products P = new Products();
            P.Show();
            this.Hide();
        }
    }
}
