using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repository
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();

        void InsertDoctor(int id, string apellido, string especialidad, int salario, int idHospital);

        List<Doctor> GetDoctoresEspecialidad(string especialidad);

        void DeleteDoctor(int idDoctor);
        void ModificarDoctor(int id, string apellido, string especialidad, int salario, int idHospital);
        Doctor FindDoctor(int iddoctor);
    }
}
