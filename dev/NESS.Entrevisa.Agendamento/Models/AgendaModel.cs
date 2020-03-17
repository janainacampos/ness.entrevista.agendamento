using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NESS.Entrevisa.Agendamento.Models
{
    public class AgendaModel
    {
        /// <summary>
        /// ID da agenda, definido automaticamente pelo banco de dados
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Data disponivel para agendamento, será informada no cadastro da agenda, a data ira conter apenas informacoes de ano,mes e dia
        /// e não podera ser repetida
        /// </summary>
        [Required]
        public string Data { get; set; }

        /// <summary>
        /// Paciente que esta agendado para a data disponivel
        /// </summary>
        public PacienteModel Paciente { get; set; }

    }
}