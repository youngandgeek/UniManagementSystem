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
    public partial class Courses : Form
    {
        public Courses()
        {
            InitializeComponent();
            ShowCourse();
            GetDepId();
            GetProfessorId();
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
            DePIdCb.ValueMember = "DepNum";
            //add all the data of DepartmentID column to the selector(comboBox)
            DePIdCb.DataSource = dt;

            Con.Close();
        }

        //Method to Get The department Name 

        private void GetDepName()
        {
            Con.Open();
            //Sql Query To get the depName from Department table that equals selected value from DepIdComobox above
            string Query = "SELECT * FROM Department_tbl WHERE DepNum=" + DePIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox Dep.Name Value
                DepNameTb.Text = dr["DepName"].ToString();
                //Con.Close();
            }

            Con.Close();
        }



        //Method to Get The Professor ID 

        private void GetProfessorId()
        {
            Con.Open();
            //Sql Query To get the PrId->(PrNum) from Professor table

            SqlCommand cmd = new SqlCommand("SELECT PrNum FROM Professor_tbl ", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            //gets collection of columns that belongs to this table 
            dt.Columns.Add("PrNum", typeof(int));
            dt.Load(Rdr);
            //set the DepId comboBox  value to DepNum
            ProfIdCb.ValueMember = "PrNum";
            //add all the data of ProfessorID column to the selector(comboBox) ->by passing the dataset obj
            
            ProfIdCb.DataSource = dt;

            Con.Close();
        }

        //Method to Get The Professor Name 

        private void GetProfessorName()
        {
            Con.Open();
            //Sql Query To get the ProfName from professor table that equals selected value from ProfIdComobox above
            string Query = "SELECT * FROM Professor_tbl WHERE PrNum=" + ProfIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            sd.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox Dep.Name Value
                PrNameTb.Text = dr["PrName"].ToString();
                //Con.Close();
            }

            Con.Close();
        }



        //ShowDep Method To show Data on Departments DataGridView(DepDGV).
        private void ShowCourse()
        {
            //Sql Query to display all department data in department Table

            string Query = "select * from Course_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var ds = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)

            sda.Fill(ds);

            //assign the dataset to dataGridView to display it

            CourseDGV.DataSource = ds.Tables[0];
            Con.Close();
            //call displaying method (ShowCourse)


        }



        //Add Button handler
        private void SaveCoBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (CoNameTb.Text == "" || CoDTb.Text == "" || DePIdCb.SelectedIndex == -1 || DepNameTb.Text == "" || ProfIdCb.SelectedIndex == -1 || PrNameTb.Text == "")
            {
                MessageBox.Show("Missing Information, Please Fill all Fields");


            }

            else
            {
                try
                {
                    //open Sql connection
                    Con.Open();


                    //Insert Sql query to add data(from user input/Fields) to the Department Table
                    SqlCommand Cmd = new SqlCommand("INSERT INTO Course_tbl(CName, CDuration,CDepId,CDepName,CProfId,CPrName)VALUES(@CN,@CD,@CDI,@CDN,@CPI,@CPN)", Con);

                    //create paramter to add data into database with cmd (values,class.properties)
                    //CName, CDuration,CDepId,CDepName,CProfId,CPrName
                    //@CN,@CD,@CDI,@CDN,CPI,CPN

                    Cmd.Parameters.AddWithValue("@CN", CoNameTb.Text);
                    Cmd.Parameters.AddWithValue("@CD", CoDTb.Text);
                    Cmd.Parameters.AddWithValue("@CDI", DePIdCb.SelectedValue.ToString());
                    Cmd.Parameters.AddWithValue("@CDN", DepNameTb.Text);
                    Cmd.Parameters.AddWithValue("@CPI", ProfIdCb.SelectedValue.ToString());
                    Cmd.Parameters.AddWithValue("@CPN", PrNameTb.Text);

                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("New Course Successfully Added");

                    //close connection
                    Con.Close();
                    //calling the show Course method
                    ShowCourse();
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


        /*RowHeaderMouseClick event handler method for when clicking on a row in the DataGridview
   *it'll get that row data and load it in the TextFields to update it or delete it 
   **/
        private void CourseDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
           
            //value to identify the Row Index where mouse clicked
            int rowIndex = e.RowIndex;

            //CName, CDuration,CDepId,CDepName,CProfId,CPrName

            CoIDTb.Text = CourseDGV.Rows[rowIndex].Cells[0].Value.ToString();
            //assign the DataGridView  row to textboxFields (load the data from row in the fields).
            CoNameTb.Text = CourseDGV.Rows[rowIndex].Cells[1].Value.ToString();
            CoDTb.Text = CourseDGV.Rows[rowIndex].Cells[2].Value.ToString();
            //.Selected item because we just take the user Input(No importing from DB).
            //.SelectedValue because we imported the ID from Database First then let the user choose from it
            DePIdCb.SelectedValue = CourseDGV.Rows[rowIndex].Cells[3].Value.ToString();
            DepNameTb.Text = CourseDGV.Rows[rowIndex].Cells[4].Value.ToString();
            ProfIdCb.SelectedValue = CourseDGV.Rows[rowIndex].Cells[5].Value.ToString();
            PrNameTb.Text = CourseDGV.Rows[rowIndex].Cells[6].Value.ToString();
        }

        /*Throw exception -> SystemArgumentsOutOfRange : Go to DataGridView Properties ->SelectionMode
             * ->RowHeaderSelect ↓ -> choose ToFullRowSelect
               */

        private void EditCorBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first

            //CName, CDuration,CDepId,CDepName,CProfId,CPrName

            if (CoNameTb.Text == "" || CoDTb.Text == "" || DePIdCb.SelectedIndex == -1 || DepNameTb.Text == "" || ProfIdCb.SelectedIndex == -1 || PrNameTb.Text == "")
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
                    //where CNum-> is the name of  CID Number column in Database
                    
  SqlCommand Cmd = new SqlCommand("UPDATE Course_tbl SET CName=@CN, CDuration=@CD,CDepId=@CDI,CDepName=@CDN,CProfId=@CPI,CPrName=@CPN WHERE CNum=@CID", Con);

                    Cmd.Parameters.AddWithValue("@CN", CoNameTb.Text);
                    Cmd.Parameters.AddWithValue("@CD", CoDTb.Text);
                    Cmd.Parameters.AddWithValue("@CDI", DePIdCb.SelectedValue.ToString());
                    Cmd.Parameters.AddWithValue("@CDN", DepNameTb.Text);
                    Cmd.Parameters.AddWithValue("@CPI", ProfIdCb.SelectedValue.ToString());
                    Cmd.Parameters.AddWithValue("@CPN", PrNameTb.Text);
                    Cmd.Parameters.AddWithValue("@CID",CoIDTb.Text);
                    Cmd.ExecuteNonQuery();

                    MessageBox.Show("Course Updated Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowCourse();
                    //Call the Clear method to clear the textFields after Updating
                    Clear();


                }
                //catch the error 
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);



                }
            }
        }



        //Clear Button Handler
        private void ClearCoBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }



        //Delete Button Handler
        private void DeleteCoBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (CoNameTb.Text == "" || CoDTb.Text == "" || DePIdCb.SelectedIndex == -1 || DepNameTb.Text == "" || ProfIdCb.SelectedIndex == -1 || PrNameTb.Text == "")
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
                    SqlCommand Cmd = new SqlCommand("DELETE FROM Course_tbl WHERE CNum=@CID", Con);


                    Cmd.Parameters.AddWithValue("@CID", CoIDTb.Text);

                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("Course Deleted Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Course method
                    ShowCourse();
                    //Call the Clear method to clear the textFields after Deletion
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
            CoIDTb.Text = "";
            CoNameTb.Text = "";
            CoDTb.Text = "";
            DePIdCb.SelectedIndex = -1;
            DepNameTb.Text = "";
            ProfIdCb.SelectedIndex = -1;
            PrNameTb.Text = "";


        }

        //when Department Id is selected (go to Id combox -> events-> double click on selectionChange committed)
        private void DePIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //excute this method
            GetDepName();
        }

        //when Professor Id is selected
        private void ProfIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetProfessorName();
        }

        //Home Icon Handler

        private void label1_Click(object sender, EventArgs e)
        {
            //load the Home page 
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }






        //Student Icon Handler
        private void label2_Click_1(object sender, EventArgs e)
        {

            //load the student page 
            Students obj = new Students();
            obj.Show();
            this.Hide();
        }

        //Professors Icon handler
        private void label3_Click(object sender, EventArgs e)
        {
            //load the student page 
            Professors obj = new Professors();
            obj.Show();
            this.Hide();
        }

        //Departments Icon Handler
        private void label4_Click(object sender, EventArgs e)
        {
            //load the Department page 

            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }


        //Fees Icon Handler
        private void label6_Click_1(object sender, EventArgs e)
        {

            //load Fees page 
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //load Fees page 
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

        //close icon handler

        private void pictureBox9_Click_1(object sender, EventArgs e)
        {


            Close();


        }

       
    }
    }
    

