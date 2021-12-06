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
using UniManagementSystem.Classes;

namespace UniManagementSystem
{
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            //calling the ShowStu method to display Student Table
            ShowStu();

            //calling get department ID method to display it in the Dep.Id selector
            GetDepId();

        }

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
            DepIdCB.ValueMember = "DepNum";
            //add all the data of DepartmentID column to the selector(comboBox)
            DepIdCB.DataSource = dt;

            Con.Close();
        }

        //Method to Get The department Name 

        private void GetDepName()
        {
            Con.Open();
            //Sql Query To get the depName from Department table that equals selected value from DepIdComobox above
            string Query = "SELECT * FROM Department_tbl WHERE DepNum=" + DepIdCB.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            { //set the textbox Dep.Name Value
                StuDepNameTextBox.Text = dr["DepName"].ToString();

            }

            Con.Close();
        }


        //ShowDep Method To show Data on Departments DataGridView(DepDGV).
        private void ShowStu()
        {
            //Sql Query to display all Students data in Student Table
            string Query = "select * from Student_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var ds = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)
            sda.Fill(ds);

            //assign the dataset to dataGridView to display it
            //DataGridViewName.DataSource=dataset.Tables

            StudentDGV.DataSource = ds.Tables[0];
            Con.Close();


        }

        //Method To Clear the Fields 
        public void Clear()
        {
            //assign an empty value to the textbox fields
            StuNumTb.Text = "";
            StuNameTb.Text = "";
            StuDobDp.Text = "";
            StuGenCB.SelectedIndex = -1;
            StuAddressTextBox.Text = "";
            //to clear selector
            DepIdCB.SelectedIndex = -1;
            StuDepNameTextBox.Text = "";
            StuPhoneTextBox.Text = "";
            StuSemesterCB.SelectedIndex = -1;
        }
        //Picture Button To Exit the application
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Event handler for DepId Selector when user select Dep ,it should display the Department Name in depname textfield
        private void DepIdCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Calling the GetDepName method to display the department name associated  with depId we selected

            GetDepName();

        }




        //Add button handler to add new students to student table
        private void AddStuBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first (StuName,StuDob,StuGen,StuAddress,StuDepId,StuDepName,StuPhone,StSem)

            if (StuNameTb.Text == "" || StuGenCB.SelectedIndex == -1 || StuAddressTextBox.Text == "" || DepIdCB.SelectedIndex == -1 || StuDepNameTextBox.Text == "" || StuPhoneTextBox.Text == "" || StuSemesterCB.SelectedIndex == -1)


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

                    SqlCommand cmd = new SqlCommand("INSERT INTO Student_tbl (StuName, StuDob,StuGen,StuAddress,StuDepId,StuDepName,StuPhone,StSem)VALUES(@SN,@SD,@SG,@SA,@SDI,@SDN,@SP,@ST)", Con);

                    //StuNum,@SI
                    //create paramter to add data into database with cmd (values,class.properties)
                    //StuName,StuDob,StuGen,StuAddress,StuDepId,StuDepName,StuPhone,StSem

                    // cmd.Parameters.AddWithValue("@SI", StuNumTb.Text);
                    cmd.Parameters.AddWithValue("@SN", StuNameTb.Text);
                    //get date from date picker
                    cmd.Parameters.AddWithValue("@SD", StuDobDp.Value.Date);
                    //get selected item to selector and parse it to string
                    //StuGenCB.SelectedItem.toString()
                    cmd.Parameters.AddWithValue("SG", StuGenCB.SelectedItem.ToString());

                    cmd.Parameters.AddWithValue("@SA", StuAddressTextBox.Text);
                    cmd.Parameters.AddWithValue("@SDI", DepIdCB.SelectedValue.ToString());

                    cmd.Parameters.AddWithValue("@SDN", StuDepNameTextBox.Text);

                    cmd.Parameters.AddWithValue("@SP", StuPhoneTextBox.Text);

                    cmd.Parameters.AddWithValue("@ST", StuSemesterCB.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New Student Successfully Added");

                    //close connection
                    Con.Close();
                    //calling the show Show student method
                    ShowStu();
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




        //clear Button handler
        private void ClearStuBtn_Click(object sender, EventArgs e)
        {
            //calling the clear method to clear texbox fields
            Clear();
        }


        //new DataGriwView handler with key 

        int key = 0;
        private void StudentDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*Throw exception -> SystemArgumentsOutOfRange : Go to DataGridView Properties ->SelectionMode
             * ->RowHeaderSelect ↓ -> choose ToFullRowSelect
               */

            StuNameTb.Text = StudentDGV.SelectedRows[0].Cells[1].Value.ToString();
            StuDobDp.Text = StudentDGV.SelectedRows[0].Cells[2].Value.ToString();

            //.Selected item because we just take the user Input(No importing from DB).
            StuGenCB.SelectedItem = StudentDGV.SelectedRows[0].Cells[3].Value.ToString();
            StuAddressTextBox.Text = StudentDGV.SelectedRows[0].Cells[4].Value.ToString();
            //.SelectedValue because we imported the ID from Database First then let the user choose from it
            //Input string was not in a correct format
            DepIdCB.SelectedValue = StudentDGV.SelectedRows[0].Cells[5].Value.ToString();
            StuDepNameTextBox.Text = StudentDGV.SelectedRows[0].Cells[6].Value.ToString();
            StuPhoneTextBox.Text = StudentDGV.SelectedRows[0].Cells[7].Value.ToString();
            StuSemesterCB.SelectedItem = StudentDGV.SelectedRows[0].Cells[8].Value.ToString();

            if (StuNameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(StudentDGV.SelectedRows[0].Cells[0].Value.ToString());
            }



        }
        private void EditStuBtn_Click(object sender, EventArgs e)
        {
            if ( StuNameTb.Text == "" || StuGenCB.SelectedIndex == -1 || StuAddressTextBox.Text == "" || DepIdCB.SelectedIndex == -1 || StuDepNameTextBox.Text == "" || StuPhoneTextBox.Text == "" || StuSemesterCB.SelectedIndex == -1)

            {
                MessageBox.Show("Missing Info");
           
            }
            else
            {
                try
                {
                    Con.Open();
                    //Update Sql query to Update data in Department Table
                    SqlCommand cmd = new SqlCommand("UPDATE Student_tbl SET  StuName=@SN , StuDob=@SD ,StuGen=@SG ,StuAddress=@SA ,StuDepId=@SDI ,StuDepName=@SDN ,StuPhone=@SP ,StSem=@ST WHERE StuNum=@Skey ", Con);

                    /*StuNum=@SI
                    cmd.Parameters.AddWithValue("@SI", StuNumTb.Text);
                    */

                    cmd.Parameters.AddWithValue("@SN", StuNameTb.Text);
                    //get date from date picker
                    cmd.Parameters.AddWithValue("@SD", StuDobDp.Value.Date);
                    //get selected item to selector and parse it to string
                    //StuGenCB.SelectedItem.toString()
                    cmd.Parameters.AddWithValue("SG", StuGenCB.SelectedItem.ToString());

                    cmd.Parameters.AddWithValue("@SA", StuAddressTextBox.Text);
                    cmd.Parameters.AddWithValue("@SDI", DepIdCB.SelectedValue.ToString());

                    cmd.Parameters.AddWithValue("@SDN", StuDepNameTextBox.Text);

                    cmd.Parameters.AddWithValue("@SP", StuPhoneTextBox.Text);

                    cmd.Parameters.AddWithValue("@ST", StuSemesterCB.SelectedItem.ToString());

                    cmd.Parameters.AddWithValue("@Skey", StuNumTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("student Updated Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowStu();
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


        //Delete Button Hnadler 
        private void DeleteStuBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select Student to Delete");
            }

            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Student_tbl WHERE StuNum=@Skey", Con);
                    cmd.Parameters.AddWithValue("@Skey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Successfully Deleted");

                    Con.Close();
                    ShowStu();
                    Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }




        /*
         * WORKED 
      

        /*


        //DataGridView handler method-> clock on DGV -> Events-> double click on RowHeaderMouseClick

        /*when clicking on a row in the DataGridview
         it'll get that row data and load it in the TextFields to update it or delete it */



        /*
        private void StudentDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //value to identify the Row Index where mouse clicked
            int rowIndex = e.RowIndex;
            //StuName,StuDob,StuGen,StuAddress,StuDepId,StuDepName,StuPhone,StSem

            StuNumTb.Text = StudentDGV.Rows[rowIndex].Cells[0].Value.ToString();
            //assign the DataGridView  row to textboxFields (load the data from row in the fields).
            StuNameTb.Text = StudentDGV.Rows[rowIndex].Cells[1].Value.ToString();
            StuDobDp.Text = StudentDGV.Rows[rowIndex].Cells[2].Value.ToString();

            //.Selected item because we just take the user Input(No importing from DB).
            StuGenCB.SelectedItem = StudentDGV.Rows[rowIndex].Cells[3].Value.ToString();
            StuAddressTextBox.Text = StudentDGV.Rows[rowIndex].Cells[4].Value.ToString();
            //.SelectedValue because we imported the ID from Database First then let the user choose from it
            //Input string was not in a correct format
            DepIdCB.SelectedValue = StudentDGV.Rows[rowIndex].Cells[5].Value.ToString();
            StuDepNameTextBox.Text = StudentDGV.Rows[rowIndex].Cells[6].Value.ToString();
            StuPhoneTextBox.Text = StudentDGV.Rows[rowIndex].Cells[7].Value.ToString();
            StuSemesterCB.SelectedItem = StudentDGV.Rows[rowIndex].Cells[8].Value.ToString();
        }





         * Delete Button handler
         * private void DeleteStuBtn_Click(object sender, EventArgs e)
            {

                //Check empty Fields first
                if (  StuNameTb.Text == "" || StuGenCB.SelectedIndex == -1 || StuAddressTextBox.Text == "" || DepIdCB.SelectedIndex == -1 || StuDepNameTextBox.Text == "" || StuPhoneTextBox.Text == "" || StuSemesterCB.SelectedIndex == -1)
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
                    SqlCommand Cmd = new SqlCommand("DELETE FROM Student_tbl WHERE StuNum=@SI", Con);


                        Cmd.Parameters.AddWithValue("@SI",StuNumTb.Text);

                        Cmd.ExecuteNonQuery();
                        MessageBox.Show("Student Deleted Successfully ");

                        //close connection
                        Con.Close();
                        //calling the show Department method
                        ShowStu();
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
         */




        private void label2_Click(object sender, EventArgs e)
        {

        }
        //student handler to load the Department page
        private void label4_Click(object sender, EventArgs e)
        {
            //load the student page 
            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Professors obj = new Professors();
            obj.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Courses obj = new Courses();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }






        /**
                //creating an obj of StudentClass(contain database query)
                StudentClass s = new StudentClass();

                //Save Button
                private void SaveBtn_Click(object sender, EventArgs e)
                {
                    //class properties : StuName, StuDob, StuGen,  StuAddress, StuDepId, StuDepName, StuPhone
                    //assign the textBox field(in form) to  properties in Student class
                    s.StuName = StuNameTextBox.Text;
                    s.StuDob = StuDob.Value.ToShortDateString();
                    s.StuGen = StuGenCB.Text;
                    s.StuAddress = StuAddressTextBox.Text;
                    s.StuDepName = StuDepNameTextBox.Text;
                    s.StuPhone = StuPhoneTextBox.Text;

                    //INSERTING DATA INTO DATABASE


                    //check if successfull , calling the method
                    bool success = s.Insert(s);


                    if (success == true)
                    {
                        //show this Message after data inserted successfully 

                        MessageBox.Show("NEW CONTACT SUCCEFULLY ADDED ");

                        //call the clear method to clear textbox fields after data insertion 
                        Clear();

                    }
                    else
                    {
                        MessageBox.Show("FAILED TO ADD CONTACT");
                    }
                    //LOAD DATA IN DATAGRIDVIEW & copy,paste it in load method
                    // calling the select method also call these 2 lines to update data 
                    DataTable dt = s.Select();
                    //assign the dataTable to datagridView (Name)
                    StudentDGV.DataSource = dt;

                }
                //Clear Method to clear the textbox fields after inserting the data into datagridview 
                //call it in Add Method after Data Successfully inserted message 
                public void Clear()
                {
                    //assign an empty value to the textbox fields
                    StuNameTextBox.Text="";
                    StuGenCB.Text="";
                    StuAddressTextBox.Text="";
                    StuDepNameTextBox.Text="";
                    StuPhoneTextBox.Text="";
                }
    **/

    }

}



