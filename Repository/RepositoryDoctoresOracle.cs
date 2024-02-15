using Microsoft.AspNetCore.Http.HttpResults;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region
//create or replace procedure sp_delete_doctor
//(p_iddoctor DOCTOR.DOCTOR_NO%TYPE)
//as
//begin
  // delete from DOCTOR where DOCTOR_NO=p_iddoctor;
//commit;
//end;
#endregion

namespace NetCoreLinqToSqlInjection.Repository
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctores;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com=new OracleCommand();
            this.com.Connection = cn;
            string sql = "select * from DOCTOR";
            OracleDataAdapter ad = new OracleDataAdapter(sql,this.cn);
            this.tablaDoctores = new DataTable();
            ad.Fill(this.tablaDoctores);
        }
        public List<Doctor> GetDoctores()
        {
            var consulta= from datos in this.tablaDoctores.AsEnumerable()
                          select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doctor = new Doctor()
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

        public void InsertDoctor(int id, string apellido, string especialidad, int salario, int idHospital)
        {
           
            string sql = "insert into DOCTOR values (:idhospital,:iddoctor,:apellido,:especialidad,:salario)";

            OracleParameter pamIdHospital = new OracleParameter(":idhospital", idHospital);
            this.com.Parameters.Add(pamIdHospital);

            OracleParameter pamIdDoctor = new OracleParameter(":iddoctor", id);
            this.com.Parameters.Add(pamIdDoctor);

            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            this.com.Parameters.Add(pamApellido);

            OracleParameter pamespecialidad = new OracleParameter(":especialidad", especialidad);
            this.com.Parameters.Add(pamespecialidad);

            OracleParameter pamsalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamsalario);

            

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
                           where datos.Field<string>("ESPECIALIDAD").ToUpper()==especialidad.ToUpper()
                           select datos;

            if (consulta.Count()==0)
            {
                return null;
            }
            else
            {
                List<Doctor> doctores=new List<Doctor>();
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
            OracleParameter pamIdDoctor = new OracleParameter(":iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_doctor";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void ModificarDoctor(int id, string apellido, string especialidad, int salario, int idHospital)
        {
            throw new NotImplementedException();
        }

        public Doctor FindDoctor(int iddoctor)
        {
            return null;
        }
    }
}
