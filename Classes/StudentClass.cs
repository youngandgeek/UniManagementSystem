using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniManagementSystem.Classes
{
    internal class StudentClass
    {
        //creating getter&setter for the form fields
        public int StuNum { get; set; }
        public string StuName { get; set; }
        public String StuDob { get; set; }
        public String StuGen { get; set; }
        public string StuAddress { get; set; }
        public int StuDepId { get; set; }
        public string StuDepName { get; set; }
        public string StuPhone { get; set; }


        //creating a connection of database
        static string myconstring = ConfigurationManager.ConnectionStrings["constring"].ConnectionString;

        //method to import data from database , DataTable is temporary table to show data
        public DataTable Select()
        { //an instance of database
            SqlConnection conn = new SqlConnection(myconstring);

            //obj for datatable

            DataTable dt = new DataTable();

            try
            {
                //sql query for selecting data
                string sql = "SELECT * FROM Student_tbl";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                //fill the adapter & pass the datatable obj
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conn.Close();
            }
            //return datatable
            return dt;

        }
        //inserting data into database

        public bool Insert(StudentClass s)
        {
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconstring);

            try
            {
                string sql = "INSERT INTO Student_tbl ( StuName, StuDob, StuGen, StuAddress, StuDepName, StuPhone) VALUES (@StuName, @StuDob, @StuGen,  @StuAddress, @StuDepId, @StuDepName, @StuPhone)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                //create paramter to add data into database with cmd (values,class.properties)

                cmd.Parameters.AddWithValue("@StuName", s.StuName);
                cmd.Parameters.AddWithValue("@StuDob", s.StuDob);
                cmd.Parameters.AddWithValue("@StuGen", s.StuGen);
                cmd.Parameters.AddWithValue("@StuAddress", s.StuAddress);
                cmd.Parameters.AddWithValue("@StuDepName", s.StuDepName);
                cmd.Parameters.AddWithValue("@StuPhone", s.StuPhone);
                //open database connection
                conn.Open();
                //check if query run successfully,returns the number of rows affected
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //close connection
                conn.Close();
            }
            return isSuccess;
        }

        //END OF INSERTING DATA INTO DATABASE METHOD
    }
}
