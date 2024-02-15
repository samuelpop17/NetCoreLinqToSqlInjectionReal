using Microsoft.AspNetCore.Http.HttpResults;
using NetCoreLinqToSqlInjection.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Numerics;

#region
//create procedure SP_DELETE_DOCTOR
//(@iddoctor int)
//as
  //  delete from DOCTOR where DOCTOR_NO = @iddoctor
//go
#endregion

namespace NetCoreLinqToSqlInjection.Repository
{
    public class RepositoryDoctoresSqlServer: IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryDoctoresSqlServer()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.tablaDoctores=new DataTable();
            string sql = "select * from DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql,this.cn);
            ad.Fill(this.tablaDoctores);
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach(var row in consulta)
            {
                Doctor doctor = new Doctor() {
                    IdHospital=row.Field<int>("HOSPITAL_COD"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDoctor = row.Field<int>("DOCTOR_NO"),

                };
                doctores.Add(doctor);
            }
            return doctores;
        }

        public Doctor FindDoctor(int iddoctor)
        {

            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == iddoctor select datos;
            var row = consulta.First();
            Doctor doctor = new Doctor
            {

                IdHospital = row.Field<int>("HOSPITAL_COD"),
                Apellido = row.Field<string>("APELLIDO"),
                Especialidad = row.Field<string>("ESPECIALIDAD"),
                Salario = row.Field<int>("SALARIO"),
                IdDoctor = row.Field<int>("DOCTOR_NO"),
            };
            return doctor;
        }

        public void InsertDoctor(int id,string apellido,string especialidad,int salario,int idHospital)
        {
            string sql = "insert into Doctor values (@idhospital,@iddoctor,@apellido,@especialidad,@salario)";

            this.com.Parameters.AddWithValue("@idhospital",idHospital);
            this.com.Parameters.AddWithValue("@iddoctor",id);
            this.com.Parameters.AddWithValue("@apellido",apellido);
            this.com.Parameters.AddWithValue("@especialidad",especialidad);
            this.com.Parameters.AddWithValue("@salario",salario);

            this.com.CommandType = CommandType.Text;

            this.com.CommandText = sql;

            this.cn.Open();

            int af = this.com.ExecuteNonQuery();

            this.cn.Close();

            this.com.Parameters.Clear();
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<string>("ESPECIALIDAD") == especialidad
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Doctor> doctores = new List<Doctor>();
                foreach (var row in consulta)
                {
                    Doctor doctor = new Doctor
                    {

                        IdHospital = row.Field<int>("HOSPITAL_COD"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Especialidad = row.Field<string>("ESPECIALIDAD"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDoctor = row.Field<int>("DOCTOR_NO"),
                    };
                    doctores.Add(doctor);
                }
                return doctores;
            }
        }

       

        public void DeleteDoctor(int idDoctor)
        {
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_DOCTOR";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void ModificarDoctor(int id, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "update DOCTOR set  HOSPITAL_COD=@idhospital,APELLIDO=@apellido,ESPECIALIDAD=@especialidad,SALARIO=@salario WHERE DOCTOR_NO=@iddoctor";

            this.com.Parameters.AddWithValue("@idhospital", idHospital);
            this.com.Parameters.AddWithValue("@iddoctor", id);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.OpenAsync();
            int af = this.com.ExecuteNonQuery();
            this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        }
}
