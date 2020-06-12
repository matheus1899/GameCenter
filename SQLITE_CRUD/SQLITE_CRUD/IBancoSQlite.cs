using System;
using System.Collections.Generic;
using System.Text;
using SQLITE_CRUD.Models;
namespace SQLITE_CRUD{
    public interface IBancoSQLite{
        bool CopiarImg(string origem, string destino, string arquivo);

        //Banco de Usuario
        bool CriarDataBase(string pasta);
        bool InserirUsuario(Usuario user, string pasta);
        List<Usuario> GetUsuarios(string pasta);
        bool AtualizarUsuario(string email, Usuario novo_user, string pasta);
        bool DeletarUsuario(Usuario user, string pasta);
        bool ExisteUsuario(string Email, string pasta);
        Usuario RetornaUsuario(string Email, string pasta);

        //Banco de Jogo
        bool CriarDataBase_Jogo(string pasta);
        bool InserirJogo(Jogo jogo, string pasta);
        List<Jogo> GetJogos(string pasta);
        bool AtualizarJogo(string nome_jogo, Jogo new_jogo, string pasta);
        bool DeletarJogo(string jogo, string pasta);
        bool ExisteJogo(string nome, string pasta);
        Jogo RetornaJogo(string nome, string pasta);
    }
}
