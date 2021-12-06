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
    public partial class Professors : Form
    {
        public Professors()
        {
            InitializeComponent();
            ShowProf();
            //calling the get DEPARTMENT iD method
            GetDepId();
        }

        //intialize sql connection (Pass the connection String)
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-BH10LTJ\SQLEXPRESS;Initial Catalog=UniversityDb;Integrated Security=True");


        //Method to Get The department ID 

        private void GetDepId()
        {
            Con.Open();
            //Sql Query To get the depId->(DepNum) from Department table

            SqlCommand cmd = new SqlCommand("SELECT DepNum FROM Department_tbl ", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //gets collection of columns that belongs to this table 
            dt.Columns.Add("DepNum", typeof(int));
            dt.Load(Rdr);
            //set the DepId comboBox  value to DepNum
            DepIdCb.ValueMember = "DepNum";
            //add all the data of DepartmentID column to the selector(comboBox)
            DepIdCb.DataSource = dt;

            Con.Close();
        }


        //Method to Get The department Name 

        private void GetDepName()
        {
            Con.Open();
            //Sql Query To get the depName from Department table that equals selected value from DepIdComobox above
            string Query = "SELECT * FROM Department_tbl WHERE DepNum="+DepIdCb.SelectedValue.ToString()+"";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox Dep.Name Value
                DepNameTb.Text = dr["DepName"].ToString();
                Con.Close();
            }

            Con.Close();
        }


        //ShowDep Method To show Data on Departments DataGridView(DepDGV).
        private void ShowProf()
        {
            //Sql Query to display all department data in department Table

            string Query = "select * from Professor_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter datadapter = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(datadapter);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var dataset = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)

            datadapter.Fill(dataset);

            //assign the dataset to dataGridView to display it

            ProfDGV.DataSource = dataset.Tables[0];
            Con.Close();
            //call displaying method (ShowDep)


        }

        // Add button handler

        private void AddPrBtn_Click(object sender, EventArgs e)
        {
            if (ProfNameTb.Text == "" || PrAddTb.Text == "" || DepNameTb.Text == "" || PrGenCb.SelectedIndex == -1  || QualificationCb.SelectedIndex == -1 || DepIdCb.SelectedIndex == -1)
            {
                MessageBox.Show("Failed to Add Professor Please check Empty Fields");
            }
            else
            {
                try
                {

                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Professor_tbl(PrName,PrDob,PrGen,PrAdd,PrQual,PrExp,PrDepId,PrDepName,PrSalary)VALUES(@PN,@PDO,@PG,@PA,@PQ,@PE,@PD,@PDN,@PS)", Con);
                    //@PN,@PDO,@PG,@PA,@PQ,@PE,@PD,@PDN,@PS"

                    cmd.Parameters.AddWithValue("@PN", ProfNameTb.Text);

                    cmd.Parameters.AddWithValue("@PDO", ProfDobDP.Value.Date);
                    cmd.Parameters.AddWithValue("PG", PrGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PA", PrAddTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", QualificationCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PE", ExpTb.Text);
                    cmd.Parameters.AddWithValue("@PD", DepIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PDN", DepNameTb.Text);
                    cmd.Parameters.AddWithValue("@PS", SalaryTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Professor Successfully Added");
                    Con.Close();
                    ShowProf();
                    Clear();
                }
                //catch the error 
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }




        }

        //Dep Id Selector handler
        private void DepIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetDepName();
        }

        //department icon handler 
        private void label4_Click(object sender, EventArgs e)
        {
            //load the student page 
            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }

       

        public void Clear()
        {
            //assign an empty value to the textbox fields(PrName,PrDob,PrGen,PrAdd,PrQual,PrExp,PrDepId,PrDepName,PrSalary).

            IdNum.Text = "";
            ProfNameTb.Text = "";
            ProfDobDP.Text = "";
            PrGenCb.SelectedIndex = -1;
            //PrPhoneTb.Text = "";
            PrAddTb.Text = "";
            QualificationCb.SelectedIndex = -1;
            ExpTb.Text = "";
            DepIdCb.SelectedIndex = -1;
            DepNameTb.Text = "";
            SalaryTb.Text = "";
        }



        /*

          int Key = 0;
          private void ProfDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
          {
              ProfNameTb.Text= ProfDGV.SelectedRows[0].Cells[1].Value.ToString();
              ProfDobDP.Text=ProfDGV.SelectedRows[0].Cells[2].Value.ToString();
              PrGenCb.SelectedItem = ProfDGV.SelectedRows[0].Cells[3].Value.ToString();
              PrAddTb.Text = ProfDGV.SelectedRows[0].Cells[4].Value.ToString();
              QualificationCb.SelectedItem = ProfDGV.SelectedRows[0].Cells[5].Value.ToString();
              ExpTb.Text = ProfDGV.SelectedRows[0].Cells[6].Value.ToString();
              DepIdCb.SelectedItem = ProfDGV.SelectedRows[0].Cells[7].Value.ToString();
              DepNameTb.Text = ProfDGV.SelectedRows[0].Cells[8].Value.ToString();
              SalaryTb.Text = ProfDGV.SelectedRows[0].Cells[9].Value.ToString();

              if(ProfNameTb.Text == "")
              {
                  Key = 0;
              }
              else
              {
                  Key=Convert.ToInt32(ProfDGV.SelectedRows[0].Cells[0].Value.ToString());
              }


          }

          */


        //DataGridView Handler

        /*RowHeaderMouseClick event handler method for when clicking on a row in the DataGridview
    *it'll get that row data and load it in the TextFields to update it or delete it 
    **/
        private void ProfDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {




            //  PrName,PrDob,PrGen,PrAdd,PrQual,PrExp,PrDepId,PrDepName,PrSalary
            //value to identify the Row Index where mouse clicked
            int rowIndex = e.RowIndex;


            IdNum.Text = ProfDGV.Rows[rowIndex].Cells[0].Value.ToString();
            //assign the DataGridView  row to textboxFields (load the data from row in the fields).
            ProfNameTb.Text = ProfDGV.Rows[rowIndex].Cells[1].Value.ToString();
            ProfDobDP.Text = ProfDGV.Rows[rowIndex].Cells[2].Value.ToString();
            //.Selected item because we just take the user Input(No importing from DB).
            PrGenCb.SelectedItem = ProfDGV.Rows[rowIndex].Cells[3].Value.ToString();
            PrAddTb.Text = ProfDGV.Rows[rowIndex].Cells[4].Value.ToString();
            QualificationCb.SelectedItem = ProfDGV.Rows[rowIndex].Cells[5].Value.ToString();
            ExpTb.Text = ProfDGV.Rows[rowIndex].Cells[6].Value.ToString();
            //.SelectedValue because we imported the ID from Database First then let the user choose from it
            DepIdCb.SelectedValue = ProfDGV.Rows[rowIndex].Cells[7].Value.ToString();
            DepNameTb.Text = ProfDGV.Rows[rowIndex].Cells[8].Value.ToString();
            SalaryTb.Text = ProfDGV.Rows[rowIndex].Cells[9].Value.ToString();


        }

        //Edit Button Handler

        private void EditPrBtn_Click_1(object sender, EventArgs e)
        {

            //Check empty Fields first
            if (ProfNameTb.Text == "" || PrAddTb.Text == "" || DepNameTb.Text == "" || PrGenCb.SelectedIndex == -1 || QualificationCb.SelectedIndex == -1 || DepIdCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information, Please Fill all Fields");
            }
            else
            {
                try
                {
                    //open Sql connection
                    Con.Open();


                    //Update Sql query to Update data in Department Table
                    //where PrNum-> is the name of  profIdNum column in Database 
                    SqlCommand cmd = new SqlCommand("UPDATE Professor_tbl SET PrName=@PN,PrDob=@PDO,PrGen=@PG,PrAdd=@PA,PrQual=@PQ,PrExp=@PE,PrDepId=@PD,PrDepName=@PDN,PrSalary=@PS WHERE PrNum=@PI", Con);
                   
                    cmd.Parameters.AddWithValue("@PN", ProfNameTb.Text);
                    //Get data from Time Date Picker
                    cmd.Parameters.AddWithValue("@PDO", ProfDobDP.Value.Date);
                    cmd.Parameters.AddWithValue("PG", PrGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PA", PrAddTb.Text);
                    //.SelectedItem because it's from user Input
                    cmd.Parameters.AddWithValue("@PQ", QualificationCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PE", ExpTb.Text);
                    //selected value because it's imported from database
                    cmd.Parameters.AddWithValue("@PD", DepIdCb.SelectedValue.ToString());
                    //try adding .ToString();
                    cmd.Parameters.AddWithValue("@PDN", DepNameTb.Text);
                    cmd.Parameters.AddWithValue("@PS", SalaryTb.Text);
                    
                    cmd.Parameters.AddWithValue("@PI", IdNum.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Professor Updated Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowProf();
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






        //Clear button handler
        private void ClearPrBtn_Click(object sender, EventArgs e)
        {
            //calling clear method
            Clear();
        }



        //Delete Button Handler
        private void DeletePrBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (ProfNameTb.Text == "" || PrAddTb.Text == "" || DepNameTb.Text == "" || PrGenCb.SelectedIndex == -1 || QualificationCb.SelectedIndex == -1 || DepIdCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information, Please Fill all Fields");
            }
            else
            {
                try
                {
                    //open Sql connection
                    Con.Open();


                    //Delete Sql query 
                    SqlCommand Cmd = new SqlCommand("DELETE FROM Professor_tbl WHERE PrNum=@PI", Con);


                    Cmd.Parameters.AddWithValue("@PI", IdNum.Text);

                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("Professor Deleted Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowProf();
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





        //Student Icon Handler
        private void label2_Click(object sender, EventArgs e)
        {
            //load the student page 
            Students obj = new Students();
            obj.Show();
            this.Hide();
        }
        //Home Icon Handler
     
        private void label1_Click(object sender, EventArgs e)
        {
            //load the Home page 
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        //Fees Icon Handler
        private void label6_Click(object sender, EventArgs e)
        {
            //load Fees page 
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }
        //Departments Icon Handler
        private void label4_Click_1(object sender, EventArgs e)
        {
            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }
        
        //Courses Icon Handler
        private void label11_Click(object sender, EventArgs e)
        {
            //load Courses page 
            Courses obj = new Courses();
            obj.Show();
            this.Hide();
        }


        //close icon handler
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

