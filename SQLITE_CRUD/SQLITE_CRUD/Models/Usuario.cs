using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SQLITE_CRUD.Models
{
    public class Usuario
    {
        [PrimaryKey]
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public bool Admin { get; set; }
        public string Dir_Imagem { get; set; }
        public string Nome_arquivo { get; set; }
    }
}
