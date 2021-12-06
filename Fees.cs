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

namespace UniManagementSystem
{
    public partial class Fees : Form
    {
        public Fees()
        {
            InitializeComponent();
            ShowFees();
            GetStuId();
            ShowSalary();
            GetProfID();
            
        }


        //intialize sql connection (Pass the connection String)
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-BH10LTJ\SQLEXPRESS;Initial Catalog=UniversityDb;Integrated Security=True");


        //Method to Get The Student ID 

        private void GetStuId()
        {
            Con.Open();
            //Sql Query To get the depId->(DepNum) from Department table

            SqlCommand cmd = new SqlCommand("SELECT StuNum FROM Student_tbl ", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //gets collection of columns that belongs to this table 
            dt.Columns.Add("StuNum", typeof(int));
            dt.Load(Rdr);
            //set the DepId comboBox  value to DepNum
            StuIdCB.ValueMember = "StuNum";
            //add all the data of DepartmentID column to the selector(comboBox)
            StuIdCB.DataSource = dt;

            Con.Close();
        }

        //Method to Get The Student Name 

        private void GetStuName()
        {
            Con.Open();
            //Sql Query To get the depName from Department table that equals selected value from DepIdComobox above
            string Query = "SELECT * FROM Student_tbl WHERE StuNum="+ StuIdCB.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox StuName Value
                StuNameTb.Text = dr["StuName"].ToString();
                Deptb.Text = dr["StuDepName"].ToString();
            }

            Con.Close();
        }

        //When studentId IS SELECTED get the student name(By calling it's method); 
        private void StuIdCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStuName();
        }


        //ShowFees Method To show Data of Fees on  DataGridView(DepDGV).
        private void ShowFees()
        {
            //Sql Query to display all department data in department Table

            string Query = "select * from Fees_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var ds = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)

            sda.Fill(ds);

            //assign the dataset to dataGridView to display it

            StudentFeesDGV.DataSource = ds.Tables[0];
            Con.Close();
            //call displaying method (ShowDep)


        }


        //Pay Button Hnadler
        private void StuPayBtn_Click(object sender, EventArgs e)
        {
            
       //Check empty Fields first (P,StuDob,StuGen,StuAddress,StuDepId,StuDepName,StuPhone,StSem)

    if (StuNameTb.Text == "" || AmountTb.Text == "" || Deptb.Text == "" )


                {
                    MessageBox.Show("Missing Information, Please Fill all Fields");
                }
                else
                {
                    try
                    {
                        //open Sql connection
                        Con.Open();

                        //Insert Sql query to add data(from user input/Fields) to the Student Table

                        SqlCommand cmd = new SqlCommand("INSERT INTO Fees_tbl (StuNum,StuName, StuDep,FPeriod,FAmount,PayDate)VALUES(@SI,@SN,@SD,@SP,@SA,@PD)", Con);

                    //StuNum,@SI
                    //create paramter to add data into database with cmd (values,class.properties)
                    //StuNum,StuName, StuDep,FPeriod,FAmount,PayDate)
                    //VALUES(@SI,@SN,@SD,@SP,@SA,@PD

                        cmd.Parameters.AddWithValue("@SI", StuIdCB.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SN", StuNameTb.Text);
                        //get date from date picker
                        cmd.Parameters.AddWithValue("@SD", Deptb.Text);
                        //get selected item to selector and parse it to string

                        cmd.Parameters.AddWithValue("@SP",PaidSemCB.SelectedItem.ToString());

                        cmd.Parameters.AddWithValue("@SA", AmountTb.Text);
                    //payDate
                        cmd.Parameters.AddWithValue("@PD", DateTime.Today.Date);

                    
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Fees Successfully Paid");

                        //close connection
                        Con.Close();
                        //calling the show Show student method
                        ShowFees();
                        //Call the Clear method to clear the textFields after Insertion

                        Clear();


                    }
                    //catch the error 
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }


                }

            }


        //ShowSalary Method To show Data of Fees on  DataGridView(DepDGV).
        private void ShowSalary()
        {
            //Sql Query to display all department data in department Table

            string Query = "select * from Salary_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var ds = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)

            sda.Fill(ds);

            //assign the dataset to dataGridView to display it

            SalaryDGV.DataSource = ds.Tables[0];
            Con.Close();
            //call displaying method (ShowDep)


        }


        //Method to Get The Professor ID 

        private void GetProfID()
        {
            Con.Open();
            //Sql Query To get the depId->(DepNum) from Department table

            SqlCommand cmd = new SqlCommand("SELECT PrNum FROM Professor_tbl ", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //gets collection of columns that belongs to this table 
            dt.Columns.Add("PrNum", typeof(int));
            dt.Load(Rdr);
            //set the DepId comboBox  value to DepNum
            ProfIdCB.ValueMember = "PrNum";
            //add all the data of professorID column to the selector(comboBox)
            ProfIdCB.DataSource = dt;

            Con.Close();
        }

        //Method to Get The department Name 

        private void GetProfName()
        {
            Con.Open();
            //Sql Query To get the depName from Department table that equals selected value from DepIdComobox above
            string Query = "SELECT * FROM Professor_tbl WHERE PrNum=" + ProfIdCB.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox StuName Value
                ProfNameTb.Text = dr["PrName"].ToString();
                PrAmount.Text= dr["PrSalary"].ToString();
            }

            Con.Close();
        }


        //when profId selected -> excute GetProfName method
        private void ProfIdCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetProfName();
        }
        // Professor Pay Button Hnadler


        private void PrPayBtn_Click(object sender, EventArgs e)
        {



            //Check empty Fields first (Id,Name,paidTerm)

            if (ProfNameTb.Text == "" || PrAmount.Text == "" || PrPaidTerm.Text == "")


            {
                MessageBox.Show("Missing Information, Please Fill all Fields");
            }
            else
            {
                String period = SalaryPeriod.Value.Date.Month.ToString() + "/" + SalaryPeriod.Value.Date.Year.ToString();

                try
                {
                    
                        //open Sql connection
                        Con.Open();

                        //Insert Sql query to add data(from user input/Fields) to the Student Table

                        SqlCommand cmd = new SqlCommand("INSERT INTO Salary_tbl (PrNum,PrName, PrSalary,SPeriod,SPDate)VALUES(@PN,@PNA,@PS,@SP,@SPD)", Con);

                    // PrNum,PrName, PrSalary,PrPeriod,SPeriod,SPDate)
                    // VALUES(@PN,@PNA,@PS,@SP,@SPD)";

                    //create paramter to add data into database with cmd (values,class.properties)

                        cmd.Parameters.AddWithValue("@PN", ProfIdCB.SelectedValue.ToString());
                        
                        cmd.Parameters.AddWithValue("@PNA", ProfNameTb.Text);
                        //get date from date picker
                        cmd.Parameters.AddWithValue("@PS", PrAmount.Text);
                        cmd.Parameters.AddWithValue("@SP",period );
                        cmd.Parameters.AddWithValue("@SPD", DateTime.Today.Date);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Salary Successfully Paid");

                        //close connection
                        Con.Close();
                        //calling the show Show student method
                        ShowSalary();
                        //Call the Clear method to clear the textFields after Insertion

                        Clear();


                    }
                    //catch the error 
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }


                }
                   
     
                     }
                    
                    
                  
     


        //Method To Clear the Fields 
        public void Clear()
        {

            //assign an empty value to the textbox fields

            StuIdCB.SelectedIndex = -1;
            StuNameTb.Text = "";
            Deptb.Text = "";
            PaidSemCB.SelectedIndex = -1;
            AmountTb.Text = "";
            
        }







        //Reset Button Handler
        private void StuResetBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }



        private void label11_Click(object sender, EventArgs e)
        {
            //load the Salary page 
            Courses obj = new Courses();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //load the Salary page 
            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //load the Salary page 
            Professors obj = new Professors();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //load the Salary page 
            Students obj = new Students();
            obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //load the Salary page 
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PrResetBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
