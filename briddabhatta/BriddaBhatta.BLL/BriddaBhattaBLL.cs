using LFA.Forum.DAL.Ado;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BriddaBhatta.BLL
{
    public class BriddaBhattaBLL
    {
        private SqlHelper sh;
        private string sqlQuerry;
        private Peoples p;
        

        public BriddaBhattaBLL()
        {
            p = new Peoples("", "",DateTime.Today.Date, "", "", 0,"", 0);
            sh = new SqlHelper();
        }
        public BriddaBhattaBLL(string fname, string lname, DateTime dob, int age,string gender, string address=" ",string street=" ")
        {
            p = new Peoples(fname, lname, dob, address,street,age,gender, CalculateAmount(age));
            sh = new SqlHelper();
        }

        public bool CheckUser()
        {
            sqlQuerry = "Select ID from AdultPerson where FirstName=@FirstName AND DateOfBirth=@DateOfBirth AND LastName=@LastName";
            SqlDataReader dr1 = sh.ExecuteReader(sqlQuerry, "@FirstName", p.Firstname, "@LastName", p.Lastname,"@DateOfBirth",p.dob);
            if (dr1.Read())
            {
                dr1.Close();
                return true;
            }
            else
            {
                dr1.Close();
                return false;
            }
        }

        public SqlDataReader getColumnNames()
        {
            //sqlQuerry = "uspGetAllColumns";
            return sh.ExecuteReader("select column_name from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='AdultPerson'");
            
        }

        public string addPeople()
        {
            sqlQuerry = "uspAddPerson";
            int result = sh.ExecNonQueryProcedure(sqlQuerry, "@FirstName", p.Firstname, "@LastName", p.Lastname, "@DateOfBirth", p.dob,"@Address",p.Address,"@Street",p.Street,"@Age",p.Age,"@Amount",p.amount,"@Gender",p.gender);
            if (result == 1)
                return " Adult Person Added ";
            else
                return " Error while adding.";
        }

        public int UpdatePeople(int id)
        {
            sqlQuerry = "uspUpdatePerson";
            return sh.ExecNonQueryProcedure(sqlQuerry, "@FirstName", p.Firstname, "@LastName",p.Lastname, "@ID", id, "@DateofBirth", p.dob, "@Address", p.Address, "@Street", p.Street, "@Age", p.Age, "@Amount", p.amount, "@Gender", p.gender);
        }

        public int DeletePeople(int id)
        {
            sqlQuerry = "uspDeletePerson";
            return sh.ExecNonQueryProcedure(sqlQuerry, "@ID", id);
        }

        public List<KeyVal> getValues(String option)
        {
            sqlQuerry = "select ID, "+option+" from AdultPerson";
            List<KeyVal> lstkeyval=new List<KeyVal>();
            SqlDataReader dr=sh.ExecuteReader(sqlQuerry);
            while (dr.Read())
            {
                KeyVal k = new KeyVal();
                k.ID=int.Parse(dr[0].ToString());
                k.Value=dr[1].ToString();
                lstkeyval.Add(k);
            }
            return lstkeyval;
        }

        public SqlDataReader getDistinctValues(String option)
        {
            sqlQuerry = "select distinct " + option + " from AdultPerson";
            return sh.ExecuteReader(sqlQuerry);
        }  

        public SqlDataReader getPerson(int ID)
        {
            sqlQuerry = "select * from AdultPerson where ID = " + ID.ToString();
            return sh.ExecuteReader(sqlQuerry);
        }

        public int CalculateAge(DateTime dob)
        {
            int dt = int.Parse((DateTime.Today.Year - dob.Date.Year).ToString());
            return dt;
        }

        public int CalculateAmount(int age)
        {
            if (age >= 80)
                return 800;
            else if (age < 80 && age >= 70)
                return 600;
            else if (age < 70 && age >= 60)
                return 400;
            else
                return 0;
        }

        public class KeyVal
        {
            public int ID { get; set; }
            public string Value { get; set; }
        }

        public SqlDataReader getPersons(string field,string value)
        {
            sqlQuerry = "select * from AdultPerson where "+field+" = "+value;
            return sh.ExecuteReader(sqlQuerry);
        }
    } 
}
