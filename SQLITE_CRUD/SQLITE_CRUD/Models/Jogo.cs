using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SQLITE_CRUD.Models
{
    public class Jogo
    {
        [PrimaryKey]
        public string Nome{ get; set; }
        public string Genero { get; set; }
        public int Class_Indicativa{ get; set; }
        public string Plataforma { get; set; }
        public int Ano_Lancamento { get; set; }
        public string Dir_Imagem { get; set; }
    }
}
