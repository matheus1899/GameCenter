using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Plugin.Media;
using SQLITE_CRUD.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SQLITE_CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Perfil : ContentPage
	{
        private Usuario user;
        private string pasta;
        private string diretorio_imagem;
        private Criptografia cripto = new Criptografia();

		public Perfil (Usuario user,string pasta)
		{
            this.user = user;
            this.pasta = pasta;
			InitializeComponent();

            ent_nome.Text = user.Nome;
            ent_cpf.Text = user.CPF;
            ent_email.Text = user.Email;

            if (!string.IsNullOrEmpty(user.Dir_Imagem)){
                btn_Foto.Source = user.Dir_Imagem;
            }
        }

        private async Task GetPhotoAsync(){
            try{
                var media = CrossMedia.Current;
                var file = await media.PickPhotoAsync();

                try {
                    if (file != null)
                    {
                        string diretorio = file.Path;
                        string arquivo_nome = Path.GetFileName(file.Path);

                        DependencyService.Get<IBancoSQLite>().CopiarImg(diretorio, pasta, arquivo_nome);
                        btn_Foto.Source = pasta + "/" + arquivo_nome;
                        diretorio_imagem = pasta + "/" + arquivo_nome;
                    }
                    else {
                        await DisplayAlert("Aviso", "Ocorreu algum problema, tente novamente mais tarde", "OK");
                        return;
                    }
                }
                catch (NullReferenceException e){
                    return;
                }
                catch (Exception e){
                    await DisplayAlert("Erro", e.InnerException + " - " + e.Message, "OK");
                }
            }
            catch (NullReferenceException e){
                Console.Write(e.Message);
                return;
            }
            catch(Exception e){
                await DisplayAlert("Erro", e.Message, "OK");
            }
            
        }

        private void ent_nova_senha_Changed(object sender, EventArgs e)
        {
            if (validarSenha(ent_nova_senha.Text))
            {
                lbl_Aviso.IsVisible = false;
            }
            else
            {
                lbl_Aviso.IsVisible = true;
            }
        }

        private async void btn_FotoClicked(object sender, EventArgs e)
        {
            await GetPhotoAsync();
        }

        private async void btn_DeletarClicked(object sender, EventArgs e)
        {
            string res = await DisplayActionSheet("Aviso","Sim","Não","Tem certeza que deseja apagar o seu cadastro?");
            if (res == "Não"){
                return;
            }
            if (DependencyService.Get<IBancoSQLite>().DeletarUsuario(user, pasta))
            {
                await DisplayAlert("Aviso", "Seu cadastro foi excluído.", "OK");
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Aviso", "Erro ao tentar excluir. Tente novamente mais tarde.", "OK");
                return;
            }
        }
        
        private async void btn_AlterarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ent_senha.Text) || string.IsNullOrEmpty(ent_nova_senha.Text) || string.IsNullOrWhiteSpace(ent_senha.Text) || string.IsNullOrWhiteSpace(ent_nova_senha.Text)){

                if(string.IsNullOrWhiteSpace(ent_senha.Text) || string.IsNullOrEmpty(ent_senha.Text)){
                    await DisplayAlert("Aviso", "Por favor, preencha o campo com a sua antiga senha", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(ent_nova_senha.Text) || string.IsNullOrEmpty(ent_nova_senha.Text))
                {
                    await DisplayAlert("Aviso", "Por favor, preencha o campo com sua nova senha", "OK");
                    return;
                }
                return;
            }
            if (ent_senha.Text != cripto.Decrypt(user.Senha))
            {
                await DisplayAlert("Aviso", "Senha antiga está incorreta, por favor corrija e tente novamente", "OK");
                return;
            }
            if (cripto.Decrypt(user.Senha) == ent_nova_senha.Text)
            {
                await DisplayAlert("Aviso", "Senha nova não pode ser igual a senha anterior, por favor corrija e tente novamente", "OK");
                return;
            }
            if (validarSenha(ent_nova_senha.Text) == false)
            {
                await DisplayAlert("Aviso", "Senha inválida, por favor cumpra com os requisitos para ter mais segurança.", "OK");
                return;
            }
            if (DependencyService.Get<IBancoSQLite>().AtualizarUsuario(user.Email, new Usuario
                {
                    Admin = user.Admin,
                    CPF = user.CPF,
                    Dir_Imagem = user.Dir_Imagem,
                    Email = user.Email,
                    Nome = user.Nome,
                    Nome_arquivo = user.Nome_arquivo,
                    Senha = cripto.Encrypt(ent_nova_senha.Text),
                }, pasta))
            {
                await DisplayAlert("Sucesso", "A atualização do cadastro foi concluida.\n Voltando para tela de Login...", "OK");
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Aviso", "Erro ao tentar atualizar. Tente novamente mais tarde", "OK");
                return;
            }
        }

        public bool validarSenha(string texto)
        {

            bool caracterMaiusculo = false;
            bool caractereEspecial = false;

            if (texto.Length < 8)
            {
                return false;
            }
            if (Regex.IsMatch(texto, @"[@!#$%-/:?_]") == true)
            {
                caractereEspecial = true;
            }
            if (Regex.IsMatch(texto, @"[ABCDEFGHIJKLMNOPQRSTUVWXYZ]") == true)
            {
                caracterMaiusculo = true;
            }

            if (caractereEspecial == true && caracterMaiusculo == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnImageClicked(object sender, EventArgs e)
        {
            ent_senha.IsPassword = !ent_senha.IsPassword;
        }

        private void btnImageClicked2(object sender, EventArgs e)
        {
            ent_nova_senha.IsPassword = !ent_nova_senha.IsPassword;
        }
    }
}