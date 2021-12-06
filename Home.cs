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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
       //Calling CountStudentds() method to show no of students registered
         
            CountStudents();
            CountDepartments();
            CountFees();
        
        }


        //intialize sql connection (Pass the connection String)
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-BH10LTJ\SQLEXPRESS;Initial Catalog=UniversityDb;Integrated Security=True");

        //Method to count students in student table

        private void CountStudents()
        {
            Con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Count(*) FROM Student_tbl", Con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //assign the text lbel to the query
            StuNoL.Text = dt.Rows[0][0].ToString();
            Con.Close();

        }


        //Method to count Departments in Departments table

        private void CountDepartments()
        {
            Con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Count(*) FROM Department_tbl", Con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //assign the label to the query
            DepNoL.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }


        //Method to count Fees has been Paid in Fees table

        private void CountFees()
        {
            Con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Sum(FAmount) FROM Fees_tbl", Con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //assign the text lbel to the query and add $ to it
            FeesCountL.Text = "$"+ dt.Rows[0][0].ToString();
            Con.Close();

        }


        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            Close();

        }

        //Student handler method
        
        private void label2_Click(object sender, EventArgs e)
        {
            //Load student from 
            Students obj = new Students();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Professors obj = new Professors();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Departments obj = new Departments();
            obj.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Courses obj = new Courses();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

        //Salary Icon handler
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }

        //Fees icon handler
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Fees obj = new Fees();
            obj.Show();
            this.Hide();
        }
    }
}
