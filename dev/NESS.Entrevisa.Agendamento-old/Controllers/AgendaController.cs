'using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NESS.Entrevisa.Agendamento.Controllers
{
    /// <summary>
    /// Classe que permite controlar a pagina de agendamento
    /// Seria possivel ralizar as ações CRUD de um agendamento.
    /// A primeira pagina conter as lista de horarios (disponiveis ou não) e a opção de criar novos horarios de agendamento
    ///Referencia instalação SQL:   https://www.youtube.com/watch?v=OKqpZ6zbZwQ
    /// </summary>
    public class AgendaController : Controller
    {


        public ActionResult Index()
        {
            var agendas = new List<Models.AgendaModel>();

            // Acessar o banco de dados e listar todos os pacientes: 
            // Exemplo da pagina: https://www.mssqltips.com/sqlservertip/5677/how-to-get-started-with-sql-server-and-net/
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = "SELECT A.CD_AGENDA, A.CD_PACIENTE, A.DT_AGENDA, P.DC_PACIENTE, P.DT_CRIADOEM FROM TB_AGENDA A  LEFT JOIN TB_PACIENTE P ON A.CD_PACIENTE = P.CD_PACIENTE ORDER BY CD_AGENDA ASC";
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
                        var agenda = new Models.AgendaModel();

                        // ID da agenda                            
                        agenda.ID = dr.GetInt32(dr.GetOrdinal("CD_AGENDA"));

                        // Data da agenda
                        agenda.Data = dr.GetDateTime(dr.GetOrdinal("DT_AGENDA")).ToString("dd/MM/yyyy");

                        // A data da agenda pode estar disponivel ou não quando estiver disponivel, não existira um paciente  vinculado para esta data
                        if (!dr.IsDBNull(dr.GetOrdinal("CD_PACIENTE")))
                        {
                            // Existe um paciente vinculado a esta data, carrega-lo para posterior exibição
                            // Data de criação do paciente
                            agenda.Paciente = new Models.PacienteModel();
                            agenda.Paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));
                            agenda.Paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));
                            agenda.Paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));
                        }

                        agendas.Add(agenda);
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(agendas);
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
        /// <param name="agenda">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Models.AgendaModel agenda)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                var data = DateTime.Parse(agenda.Data);

                //access SQL Server and run your command
                string QUERY = string.Empty;
                if (agenda.Paciente != null)
                    QUERY = $"INSERT INTO TB_AGENDA (DT_AGENDA, CD_PACIENTE) VALUES ('{data.ToString("yyyy-MM-dd")}', {agenda.Paciente.ID}) ";
                else
                    QUERY = $"INSERT INTO TB_AGENDA (DT_AGENDA) VALUES ('{data.ToString("yyyy-MM-dd")}') ";

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
            var agenda = new Models.AgendaModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT A.CD_AGENDA, A.CD_PACIENTE, A.DT_AGENDA, P.DC_PACIENTE, P.DT_CRIADOEM FROM TB_AGENDA A  LEFT JOIN TB_PACIENTE P ON A.CD_PACIENTE = P.CD_PACIENTE WHERE CD_AGENDA = {id}";
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
                        // ID da agenda                            
                        agenda.ID = dr.GetInt32(dr.GetOrdinal("CD_AGENDA"));

                        // Data da agenda
                        agenda.Data = dr.GetDateTime(dr.GetOrdinal("DT_AGENDA")).ToString("dd/MM/yyyy");

                        // A data da agenda pode estar disponivel ou não quando estiver disponivel, não existira um paciente  vinculado para esta data
                        if (!dr.IsDBNull(dr.GetOrdinal("CD_PACIENTE")))
                        {
                            // Existe um paciente vinculado a esta data, carrega-lo para posterior exibição
                            // Data de criação do paciente
                            agenda.Paciente = new Models.PacienteModel();
                            agenda.Paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));
                            agenda.Paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));
                            agenda.Paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));
                        }
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(agenda);
        }

        /// <summary>
        /// Ação de criação de paciente
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.tutorialsteacher.com/mvc/create-edit-view-in-asp.net-mvc
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Models.AgendaModel agenda)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                var data = DateTime.Parse(agenda.Data);

                string QUERY = string.Empty;

                //access SQL Server and run your command
                if (agenda.Paciente == null)
                    QUERY = $"UPDATE TB_AGENDA SET DT_AGENDA = '{data.ToString("yyyy-MM-dd")}', CD_PACIENTE=NULL WHERE CD_AGENDA = {agenda.ID}";
                else
                    QUERY = $"UPDATE TB_AGENDA SET DT_AGENDA = '{data.ToString("yyyy-MM-dd")}', CD_PACIENTE={agenda.Paciente.ID} WHERE CD_AGENDA = {agenda.ID}";

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
            var agenda = new Models.AgendaModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT A.CD_AGENDA, A.CD_PACIENTE, A.DT_AGENDA, P.DC_PACIENTE, P.DT_CRIADOEM FROM TB_AGENDA A  LEFT JOIN TB_PACIENTE P ON A.CD_PACIENTE = P.CD_PACIENTE WHERE CD_AGENDA = {id}";
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
                        // ID da agenda                            
                        agenda.ID = dr.GetInt32(dr.GetOrdinal("CD_AGENDA"));

                        // Data da agenda
                        agenda.Data = dr.GetDateTime(dr.GetOrdinal("DT_AGENDA")).ToString("dd/MM/yyyy");

                        // A data da agenda pode estar disponivel ou não quando estiver disponivel, não existira um paciente  vinculado para esta data
                        if (!dr.IsDBNull(dr.GetOrdinal("CD_PACIENTE")))
                        {
                            // Existe um paciente vinculado a esta data, carrega-lo para posterior exibição
                            // Data de criação do paciente
                            agenda.Paciente = new Models.PacienteModel();
                            agenda.Paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));
                            agenda.Paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));
                            agenda.Paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));
                        }
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(agenda);
        }

        /// <summary>
        /// Ação para deletar o pacidente informado
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.tutorialsteacher.com/mvc/create-edit-view-in-asp.net-mvc
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Models.AgendaModel paciente)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"DELETE FROM TB_AGENDA WHERE CD_AGENDA = {paciente.ID}";
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
            var agenda = new Models.AgendaModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT A.CD_AGENDA, A.CD_PACIENTE, A.DT_AGENDA, P.DC_PACIENTE, P.DT_CRIADOEM FROM TB_AGENDA A  LEFT JOIN TB_PACIENTE P ON A.CD_PACIENTE = P.CD_PACIENTE WHERE CD_AGENDA = {id}";
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
                        // ID da agenda                            
                        agenda.ID = dr.GetInt32(dr.GetOrdinal("CD_AGENDA"));

                        // Data da agenda
                        agenda.Data = dr.GetDateTime(dr.GetOrdinal("DT_AGENDA")).ToString("dd/MM/yyyy");

                        // A data da agenda pode estar disponivel ou não quando estiver disponivel, não existira um paciente  vinculado para esta data
                        if (!dr.IsDBNull(dr.GetOrdinal("CD_PACIENTE")))
                        {
                            // Existe um paciente vinculado a esta data, carrega-lo para posterior exibição
                            // Data de criação do paciente
                            agenda.Paciente = new Models.PacienteModel();
                            agenda.Paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));
                            agenda.Paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));
                            agenda.Paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));
                        }
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            return View(agenda);
        }

        /// <summary>
        /// Ação de Delete, ira deletar o paciente informado
        /// </summary>
        /// <param name="id">parametro com o id do paciente, precisa se chamar ID por causa da definição de rota, ver artigo: https://stackoverflow.com/questions/45058519/an-optional-parameter-must-be-a-reference-type-a-nullable-type-or-be-declared</param>
        /// <returns></returns>
        public ActionResult Scheduling(int id)
        {
            var agenda = new Models.AgendaModel();

            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                //access SQL Server and run your command
                string QUERY = $"SELECT A.CD_AGENDA, A.CD_PACIENTE, A.DT_AGENDA, P.DC_PACIENTE, P.DT_CRIADOEM FROM TB_AGENDA A  LEFT JOIN TB_PACIENTE P ON A.CD_PACIENTE = P.CD_PACIENTE WHERE CD_AGENDA = {id}";
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
                        // ID da agenda                            
                        agenda.ID = dr.GetInt32(dr.GetOrdinal("CD_AGENDA"));

                        // Data da agenda
                        agenda.Data = dr.GetDateTime(dr.GetOrdinal("DT_AGENDA")).ToString("dd/MM/yyyy");

                        // A data da agenda pode estar disponivel ou não quando estiver disponivel, não existira um paciente  vinculado para esta data
                        if (!dr.IsDBNull(dr.GetOrdinal("CD_PACIENTE")))
                        {
                            // Existe um paciente vinculado a esta data, carrega-lo para posterior exibição
                            // Data de criação do paciente
                            agenda.Paciente = new Models.PacienteModel();
                            agenda.Paciente.ID = dr.GetInt32(dr.GetOrdinal("CD_PACIENTE"));
                            agenda.Paciente.Nome = dr.GetString(dr.GetOrdinal("DC_PACIENTE"));
                            agenda.Paciente.CriadoEm = dr.GetDateTime(dr.GetOrdinal("DT_CRIADOEM"));
                        }
                    }
                }
                else
                {
                    // TODO yet I dont knows what to do when thre isnt paciente
                }

                dr.Close();

                conn.Close();
            }

            var pacientes = new List<Models.PacienteModel>();

            // Acessar o banco de dados e listar todos os pacientes: 
            // Exemplo da pagina: https://www.mssqltips.com/sqlservertip/5677/how-to-get-started-with-sql-server-and-net/
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

            int pacienteID = 0;
            if (agenda.Paciente?.ID > 0) pacienteID = agenda.Paciente.ID;

            ViewBag.PacienteID = new SelectList(pacientes, "ID", "Nome", pacienteID);

            return View(agenda);
        }

        /// <summary>
        /// Ação de criação de paciente
        /// Os novos pacientes podem conter homonimos por isso não será realizada nenhum tipo de validação de nome, durante a gravação
        /// Referencia: https://www.eduardopires.net.br/2014/08/tecnica-simples-dropdownlist-asp-net-mvc/
        /// </summary>
        /// <param name="paciente">Instancia do novo paciente que será criado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Scheduling(Models.AgendaModel agenda)
        {
            var connString = "Server=DESKTOP-RSCA7A0\\SQLCAMPOS;Database=db_ness_entrevista;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connString))
            {
                string QUERY = $"UPDATE TB_AGENDA SET CD_PACIENTE={Request.Form["PacienteID"]} WHERE CD_AGENDA = {agenda.ID}";
                var cmd = new SqlCommand(QUERY, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Voltar para a pagina de listagem de pacientes
            return RedirectToAction("Index");
        }
    }
}