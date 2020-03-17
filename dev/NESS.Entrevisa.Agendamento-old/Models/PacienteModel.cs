using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NESS.Entrevisa.Agendamento.Models
{
    /// <summary>
    /// Classe que representa um paciente
    /// </summary>
    public class PacienteModel
    {
        /// <summary>
        /// ID do paciente definido automaticamente pelo banco de dados
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nome do paciente que foi informado no momento do seu cadastro
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data do paciente definida automaticamente pelo banco de dados
        /// </summary>
        public DateTime CriadoEm { get; set; }

    }
}