using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NESS.Entrevisa.Agendamento.Controllers
{
    /// <summary>
    /// Classe para controle das ações CRUD do paciente
    /// A açao Index é a ação de entrada, onde será exibido a lista de pacientes e a opção de se criar um novo paciente
    /// </summary>
    public class PacienteController : Controller
    {

        public ActionResult Index()
        {
            var pacientes = new List<Models.PacienteModel>();

            // Acessar o banco de dados e listar todos os pacientes: 
            // Exemplo da pagina: https://www.mssqltips.com/sqlservertip/5677/how-to-get-started-with-sql-server-and-net/
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = "SELECT * FROM TB_PACIENTE ORDER BY CD_PACIENTE ASC";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                //execute the SQLCommand
                var dr = cmd.ExecuteReader();

                //check if there are records
                if (dr.HasRows)
                {
                    // check if there are rows for read
                    while (dr.Read())
                    {
                        var paciente = new Models.PacienteModel();

                        // ID do paciente                            
                        paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));

                        // Nome do paciente 
                        paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));

                        // Data de criação do paciente
                        paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));

                        pacientes.Add(paciente);
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(pacientes);
        }

        /// <summary>
        /// Ação de Create, para a primeria vez que a pagina esta sendo aberto
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            return View();
        }

        /// <summary>
        /// Ação de criação de paciente
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.tutorialsteacher.com/mvc/create-edit-view-in-asp.net-mvc
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Models.PacienteModel paciente)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"INSERT INTO TB_PACIENTE (DC_PACIENTE) VALUES ('{paciente.Nome}') ";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Voltar para a pagina de listagem de pacientes
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Ação de Create, para a primeria vez que a pagina esta sendo aberto
        /// </summary>
        /// <param name="id">parametro com o id do paciente, precisa se chamar ID por causa da definição de rota, ver artigo: https://stackoverflow.com/questions/45058519/an-optional-parameter-must-be-a-reference-type-a-nullable-type-or-be-declared</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var paciente = new Models.PacienteModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT * FROM TB_PACIENTE WHERE CD_PACIENTE = {id}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                //execute the SQLCommand
                var dr = cmd.ExecuteReader();

                //check if there are records
                if (dr.HasRows)
                {
                    // check if there are rows for read
                    if (dr.Read())
                    {

                        // ID do paciente                            
                        paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));

                        // Nome do paciente 
                        paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));

                        // Data de criação do paciente
                        paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));

                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(paciente);
        }

        /// <summary>
        /// Ação de criação de paciente
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.tutorialsteacher.com/mvc/create-edit-view-in-asp.net-mvc
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Models.PacienteModel paciente)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"UPDATE TB_PACIENTE SET DC_PACIENTE = '{paciente.Nome}' WHERE CD_PACIENTE = {paciente.ID}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Voltar para a pagina de listagem de pacientes
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Ação de Delete, ira deletar o paciente informado
        /// </summary>
        /// <param name="id">parametro com o id do paciente, precisa se chamar ID por causa da definição de rota, ver artigo: https://stackoverflow.com/questions/45058519/an-optional-parameter-must-be-a-reference-type-a-nullable-type-or-be-declared</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var paciente = new Models.PacienteModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT * FROM TB_PACIENTE WHERE CD_PACIENTE = {id}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                //execute the SQLCommand
                var dr = cmd.ExecuteReader();

                //check if there are records
                if (dr.HasRows)
                {
                    // check if there are rows for read
                    if (dr.Read())
                    {

                        // ID do paciente                            
                        paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));

                        // Nome do paciente 
                        paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));

                        // Data de criação do paciente
                        paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));

                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(paciente);
        }

        /// <summary>
        /// Ação para deletar o pacidente informado
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.tutorialsteacher.com/mvc/create-edit-view-in-asp.net-mvc
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Models.PacienteModel paciente)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"DELETE FROM TB_PACIENTE WHERE CD_PACIENTE = {paciente.ID}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Voltar para a pagina de listagem de pacientes
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Ação de Delete, ira deletar o paciente informado
        /// </summary>
        /// <param name="id">parametro com o id do paciente, precisa se chamar ID por causa da definição de rota, ver artigo: https://stackoverflow.com/questions/45058519/an-optional-parameter-must-be-a-reference-type-a-nullable-type-or-be-declared</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var paciente = new Models.PacienteModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT * FROM TB_PACIENTE WHERE CD_PACIENTE = {id}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                //execute the SQLCommand
                var dr = cmd.ExecuteReader();

                //check if there are records
                if (dr.HasRows)
                {
                    // check if there are rows for read
                    if (dr.Read())
                    {

                        // ID do paciente                            
                        paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));

                        // Nome do paciente 
                        paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));

                        // Data de criação do paciente
                        paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));

                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(paciente);
        }
    }
}