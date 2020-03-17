USE db_ness_entrevista
GO

/*
	Tabela para controle de pacientes

	Autor: Janaiana
	Data de criação: 2020-03-13 17:00
	
	Data modificacao: : 2020-03-13 17:10
	Modificado: Adição do campo data de criação do paciente
*/

CREATE TABLE TB_PACIENTE
(
	CD_PACIENTE INT NOT NULL IDENTITY(1, 1), --CodigoPaciene, é gerado automaticamente pra não precisar controlar o proximo codigo disponivel
	DC_PACIENTE VARCHAR(48) NOT NULL, --NomePaciente
	DT_CRIADOEM DATETIME DEFAULT(GETDATE()), -- Data de criação do paciente, é gerado automaticamente pra não precisar controlar a date de cruação pelo aplicativo
	CONSTRAINT PK_TB_PACIENTE PRIMARY KEY(CD_PACIENTE)
)
GO

/*
	Tabela para controle de agendamento do paciente

	Autor: Janaiana
	Data de criação: 2020-03-13 17:20
*/
CREATE TABLE TB_AGENDA
(
	CD_AGENDA INT NOT NULL IDENTITY(1, 1), -- Codigo da agenda, é gerado automaticamente pra não precisar controlar o proximo codigo disponivel
	DT_AGENDA DATETIME NOT NULL, -- Data disponivel para a cunsulta de um paciente, é criada de forma manual com as datas disponivels para atender um paciente e deverá ser uma data unica e não conter informação de data e hora
	CD_PACIENTE INT NULL,	-- Código do paciente agendado na data disponivel
	CONSTRAINT PK_TB_AGENDA PRIMARY KEY(CD_AGENDA),
	CONSTRAINT UK_TB_AGENDA_DATA UNIQUE(DT_AGENDA),
	CONSTRAINT FK_TB_AGENDA_TB_PACIENTE FOREIGN KEY (CD_PACIENTE) REFERENCES TB_PACIENTE(CD_PACIENTE)
)
GO
