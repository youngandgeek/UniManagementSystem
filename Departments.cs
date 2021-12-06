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
using System.Windows.Input;

namespace UniManagementSystem
{
    public partial class Departments : Form
    {
        public Departments()
        {
            InitializeComponent();

            //calling the ShowDep method to display Department Table
            ShowDep();
        }

        //intialize sql connection (Pass the connection String)
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-BH10LTJ\SQLEXPRESS;Initial Catalog=UniversityDb;Integrated Security=True");

        //ShowDep Method To show Data on Departments DataGridView(DepDGV).
        private void ShowDep()
        {
            //Sql Query to display all department data in department Table

            string Query = "select * from department_tbl";

            //pass the query and connection to the data adapter 

            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);

            //The SqlCommandBuilder just creates sql queries for select,update,insert and delete&puts them into the SqlDataAdapter

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);

            //DataSet consists of a collection of DataTable objects that you can relate to each other with DataRelation objects

            var ds = new DataSet();

            //pass the dataset to the sqlDataAdapter(fills the datatable)

            sda.Fill(ds);

            //assign the dataset to dataGridView to display it

            DepDGV.DataSource = ds.Tables[0];
            Con.Close();
            //call displaying method (ShowDep)


        }

        //Add Button handler
        private void AddBtn_Click_1(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (DepNameTb.Text == "" || DepIntakeTb.Text == "" || DepFeesTb.Text == "")
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
                    SqlCommand Cmd = new SqlCommand("INSERT INTO Department_tbl(DepName, DepIntake, DepFees)VALUES(@DN,@DI,@DF)", Con);

                    //create paramter to add data into database with cmd (values,class.properties)
                    Cmd.Parameters.AddWithValue("@DN", DepNameTb.Text);
                    Cmd.Parameters.AddWithValue("@DI", DepIntakeTb.Text);
                    Cmd.Parameters.AddWithValue("@DF", DepFeesTb.Text);
                    
                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("New Department Successfully Added");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowDep();
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
            DepNumTb.Text = "";
            DepNameTb.Text = "";
            DepIntakeTb.Text = "";
            DepFeesTb.Text = "";
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Close();
        }


        //DataGridView Handler

        /*Throw exception -> SystemArgumentsOutOfRange : Go to DataGridView Properties ->SelectionMode
                  * ->RowHeaderSelect ↓ -> choose ToFullRowSelect
                    */
        /*RowHeaderMouseClick event handler method for when clicking on a row in the DataGridview
         *it'll get that row data and load it in the TextFields to update it or delete it 
        */
        private void DepDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //value to identify the Row Index where mouse clicked
            int rowIndex = e.RowIndex;
            //assign the DataGridView  row to textboxFields (load the data from row in the fields).
            DepNumTb.Text = DepDGV.Rows[rowIndex].Cells[0].Value.ToString();
            DepNameTb.Text = DepDGV.Rows[rowIndex].Cells[1].Value.ToString();
            DepIntakeTb.Text = DepDGV.Rows[rowIndex].Cells[2].Value.ToString();
            DepFeesTb.Text = DepDGV.Rows[rowIndex].Cells[3].Value.ToString();

        }


        //Edit Button handler method
        private void EditBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (DepNameTb.Text == "" || DepIntakeTb.Text == "" || DepFeesTb.Text == "")
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
                    SqlCommand Cmd = new SqlCommand("UPDATE Department_tbl SET DepName=@DN, DepIntake=@DI, DepFees=@DF WHERE DepNum=@DNum", Con);

                    Cmd.Parameters.AddWithValue("@DN", DepNameTb.Text);
                    Cmd.Parameters.AddWithValue("@DI", DepIntakeTb.Text);
                    Cmd.Parameters.AddWithValue("@DF", DepFeesTb.Text);
                    Cmd.Parameters.AddWithValue("@DNum", DepNumTb.Text);

                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("Department Updated Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowDep();
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
        //Delete Button Method handler 
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            //Check empty Fields first
            if (DepNameTb.Text == "" || DepIntakeTb.Text == "" || DepFeesTb.Text == "")
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
                    SqlCommand Cmd = new SqlCommand("DELETE FROM Department_tbl WHERE DepNum=@DNum", Con);


                    Cmd.Parameters.AddWithValue("@DNum", DepNumTb.Text);

                    Cmd.ExecuteNonQuery();
                    MessageBox.Show("Department Deleted Successfully ");

                    //close connection
                    Con.Close();
                    //calling the show Department method
                    ShowDep();
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

        //Clear Button Event handler to clear the TextBoxfields 
        private void ClearDepBtn_Click(object sender, EventArgs e)
        {
            Clear();

        }




        //Home Icon Handler to load the Home page when clicked
        private void label1_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        //Student Icon handler
        private void label2_Click(object sender, EventArgs e)
        {
            Students obj = new Students();
            obj.Show();
            this.Hide();
        }

        //Courses Icon Handler
        private void label11_Click(object sender, EventArgs e)
        {
            Courses obj = new Courses();
            obj.Show();
            this.Hide();
        }

        //Professors Icon Handler
        private void label3_Click(object sender, EventArgs e)
        {
            Professors obj = new Professors();
            obj.Show();
            this.Hide();
        }


        //Fees Icon Handler
        private void label6_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

      
    }



    /* trying cellContent click with key
               int key=0;
               //DataGridView Handler
               private void DepDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
               {
           //fill the TextFields with Data from selected Row in DatagridView
                   DepNumTb.Text = DepDGV.SelectedRows[0].Cells[1].Value.ToString();
                   DepNameTb.Text = DepDGV.SelectedRows[0].Cells[2].Value.ToString();
                   DepIntakeTb.Text = DepDGV.SelectedRows[0].Cells[3].Value.ToString();
                   DepFeesTb.Text = DepDGV.SelectedRows[0].Cells[4].Value.ToString();
                   //check if no selectiond
                   if (DepNameTb.Text == "")
                   {
                       key = 0;
                       DepNumTb.Text = "";
                       DepNameTb.Text = "";
                       DepIntakeTb.Text = "";
                       DepFeesTb.Text = "";
                   }
                   else
                   {
                       //get the row
                       key = Convert.ToInt32(DepDGV.SelectedRows[0].Cells[0].Value.ToString());}}
       */

}












