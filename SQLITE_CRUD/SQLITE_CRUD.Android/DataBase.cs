using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLITE_CRUD.Models;
using Android.Util;
using Java.IO;

[assembly: Xamarin.Forms.Dependency(typeof(SQLITE_CRUD.Droid.DataBase))]
namespace SQLITE_CRUD.Droid
{
    public class DataBase:IBancoSQLite
    {
        private string pasta;
        //Android Geral
        public bool CopiarImg(string origem,string destino, string arquivo)
        {
            InputStream in_;
            OutputStream out_;
            try
            {

                File toFile = new File(origem);
                File fromFile = new File(destino + "/" + arquivo);
                in_ = new FileInputStream(toFile);
                out_ = new FileOutputStream(fromFile);

                byte[] buffer = new byte[1024];
                int length;
                while ((length = in_.Read(buffer))>0)
                {
                    out_.Write(buffer, 0, length);
                }

                in_.Close();
                out_.Close();
                return true;

            }catch(Exception ex)
            {
                return false;
            }
        }
        
        //Banco de Usuarios...
        public bool CriarDataBase(string pasta)
        {
            this.pasta = pasta;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Usuario.db")))
                {
                    conexao.CreateTable<Usuario>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InserirUsuario(Usuario user, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Usuario.db"))){
                    conexao.Insert(user);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Usuario> GetUsuarios(string pasta)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Usuario.db")))
                {
                    return conexao.Table<Usuario>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public bool AtualizarUsuario(string Email, Usuario novo_user, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Usuario.db"))){

                    var user = conexao.Table<Usuario>().First(u => u.Email == Email);

                    user.Admin = novo_user.Admin;
                    user.CPF = novo_user.CPF;
                    user.Dir_Imagem = novo_user.Dir_Imagem;
                    user.Email = novo_user.Email;
                    user.Nome = novo_user.Nome;
                    user.Nome_arquivo = novo_user.Nome_arquivo;
                    user.Senha = novo_user.Senha;
                    
                    conexao.Update(user);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool DeletarUsuario(Usuario user, string pasta)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Usuario.db")))
                {
                    if (ExisteUsuario(user.Email,pasta) == true)
                    {
                        conexao.Delete(user);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool ExisteUsuario(string Email,string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Usuario.db"))){
                    var user = conexao.Table<Usuario>().Count(u=>u.Email == Email);
                    if (user==0){
                        return false;
                    }
                    else{
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public Usuario RetornaUsuario(string Email, string pasta)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db")))
                {
                    List<Usuario> lista = GetUsuarios(pasta);
                    foreach (var i in lista)
                    {
                        if(i.Email == Email)
                        {
                            return i;
                        }
                    }
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        //Banco de Jogos
        public bool CriarDataBase_Jogo(string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    conexao.CreateTable<Jogo>();
                    return true;
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool InserirJogo(Jogo jogo, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    conexao.Insert(jogo);
                    return true;
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Jogo> GetJogos(string pasta)
        {
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    return conexao.Table<Jogo>().ToList();
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public bool AtualizarJogo(string nome_jogo, Jogo new_jogo, string pasta){
            try {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))) {
                    var game = conexao.Table<Jogo>().First(u => u.Nome == nome_jogo);

                    game.Ano_Lancamento = new_jogo.Ano_Lancamento;
                    game.Class_Indicativa = new_jogo.Class_Indicativa;
                    game.Dir_Imagem = new_jogo.Dir_Imagem;
                    game.Genero = new_jogo.Genero;
                    game.Nome = new_jogo.Nome;
                    game.Plataforma = new_jogo.Plataforma;

                    conexao.Update(game);
                    return true;
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool DeletarJogo(string nome, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    if (ExisteJogo(nome, pasta) == true){
                        List<Jogo> lista = GetJogos(pasta);
                        Jogo game = new Jogo();
                        foreach(var i in lista){
                            if (i.Nome==nome){
                                game.Ano_Lancamento = i.Ano_Lancamento;
                                game.Class_Indicativa = i.Class_Indicativa;
                                game.Dir_Imagem = i.Dir_Imagem;
                                game.Genero = i.Genero;
                                game.Nome = i.Nome;
                                game.Plataforma = i.Plataforma;
                            }
                        }
                        conexao.Delete(game);
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool ExisteJogo(string Nome, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    var game = conexao.Table<Jogo>().Count(u => u.Nome == Nome);

                    if (game == 0){
                        return false;
                    }
                    else{
                        return true;
                    }
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public Jogo RetornaJogo(string nome, string pasta){
            try{
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(this.pasta, "Jogos.db"))){
                    var game = conexao.Table<Jogo>().First(j => j.Nome == nome);
                    return game;
                }
            }
            catch (SQLiteException ex){
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }

        }
    }
}