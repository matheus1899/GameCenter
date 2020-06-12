using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLITE_CRUD.Models;
using System.Net.Mail;
using SQLITE_CRUD.Views;

namespace SQLITE_CRUD
{
    public partial class MainPage : ContentPage
    {
        private string pasta;
        //construtor da classe
        public MainPage()
        {
            InitializeComponent();
            email.Text = null;
            senha.Text = null;
            NavigationPage.SetHasNavigationBar(this, false);

            pasta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DependencyService.Get<IBancoSQLite>().CriarDataBase(pasta);
        }
        //metodo invocado ao pressionar a label com texto igual a "Esqueceu sua senha"
        private async void EsqueciSenha_Tapped(object sender, EventArgs e){
            //verifica se o email do usuario foi preenchido
            if (string.IsNullOrEmpty(email.Text) || string.IsNullOrWhiteSpace(email.Text)){
                await DisplayAlert("Aviso", "Preencha seu email para enviarmos sua nova senha", "OK");
                return;
            }
            //verifica se o usuario existe no banco 
            bool existe = DependencyService.Get<IBancoSQLite>().ExisteUsuario(email.Text, pasta);
            if (existe == false){
                await DisplayAlert("Aviso", "Seu email não foi encontrado em nossos registros", "OK");
                return;
            }
            //pega o objeto do tipo Usuario contendo todas as informações correspondentes ao email preenchido
            Usuario user = DependencyService.Get<IBancoSQLite>().RetornaUsuario(email.Text, pasta);

            string nova_senha = null;
            //while que rodará até que a nova senha gerada esteja validada
            while (true){
                if (validarSenha(nova_senha) == true){
                    break;
                }
                else{
                    nova_senha = GerarSenha();
                }
            }
            try{
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //remetente
                mail.From = new MailAddress("xamarin.teste.email@gmail.com");
                //destinatario
                mail.To.Add(email.Text);
                //Construção do corpo do email
                mail.IsBodyHtml = true;
                mail.Subject = "Olá novamente, " + user.Nome + "!";
                mail.Body = "Segue sua nova senha para efetuar o login: <div style='font-weight: 700;'>" + nova_senha + "</div>";
                //porta de conexão
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("xamarin.teste.email@gmail.com", "testexamarin");
                object a = new object();
                SmtpServer.SendAsync(mail, a);
                await DisplayAlert("Aviso", "Email enviado com sucesso", "OK");
            }
            catch (Exception ex){
                await DisplayAlert("Aviso", ex.Message + "\n" + ex.InnerException, "OK");
                return;
            }
            
            Criptografia cripto = new Criptografia();
            //realizando a criptografia da senha
            nova_senha = cripto.Encrypt(nova_senha);
            //verificando se a atualiz~ção foi bem sucedida
            if (!DependencyService.Get<IBancoSQLite>().AtualizarUsuario(user.Email, new Usuario{
                Admin = user.Admin,
                Dir_Imagem = user.Dir_Imagem,
                CPF = user.CPF,
                Nome = user.Nome,
                Nome_arquivo = user.Nome_arquivo,
                Senha = nova_senha,
                Email = user.Email
            }, pasta))
            {
                await DisplayAlert("Aviso", "Senha não alterada com sucesso", "OK");
                return;
            }
            await Task.Delay(15);
            //Logout
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
        //metodo que realiza a troca da propriedade IsPassword da label senha
        private void btnImageClicked(object sender, EventArgs e)
        {
            senha.IsPassword = !senha.IsPassword;
        }
        //reseta os valores nos campos
        private void btnLimparCampos(object sender, EventArgs e)
        {
            senha.Text = null;
            email.Text = null;
        }
        //metodo responsavel por efetuar o login no aplicativo
        private async void btnEnviar(object sender, EventArgs e)
        {
            Criptografia criptografia = new Criptografia();
            //verifica se o campo email foi preenchido
            if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrEmpty(email.Text))
            {
                await DisplayAlert("Aviso", "Por favor, preencha seu email para efetuar o login", "OK");
                return;
            }
            //verifica se o campo senha foi preenchido
            if (string.IsNullOrWhiteSpace(senha.Text) || string.IsNullOrEmpty(senha.Text))
            {
                await DisplayAlert("Aviso", "Por favor, preencha sua senha para efetuar o login", "OK");
                return;
            }

            string senha_encriptada = criptografia.Encrypt(senha.Text);
            //verifica se o email informado existe dentro do banco
            if (DependencyService.Get<IBancoSQLite>().ExisteUsuario(email.Text, pasta) == true)
            {
                Usuario user = new Usuario();
                user = DependencyService.Get<IBancoSQLite>().RetornaUsuario(email.Text,pasta);
                try
                {
                    if (user.Email == email.Text && user.Senha == senha_encriptada)
                    {
                        if (user.Admin == true)
                        {
                            //carregaa a pagina Menu com todas as funcionalidades disponiveis
                            await Navigation.PushAsync(new Menu(new Usuario { CPF = user.CPF, Email = user.Email, Senha = user.Senha, Admin = user.Admin, Nome = user.Nome }, pasta));
                        }
                        else
                        {
                            //carregaa a pagina MenuComum que é reservado ao usuario comum, sem privilégios especiais
                            await Navigation.PushAsync(new MenuComum(new Usuario { CPF = user.CPF, Email = user.Email, Senha = user.Senha, Admin = user.Admin, Nome = user.Nome }, pasta));
                        }
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Senha incorreta", "OK");
                        return;
                    }
                }catch(Exception ex)
                {
                    await DisplayAlert("Aviso", ex.Message + " - | - " + ex.InnerException, "OK");
                    return;
                }
                  
            }
            else {
                await DisplayAlert("Aviso", "Usuario inexistente", "OK");
                return;
            }
        }
        private async void btn_FaceLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookPageLogin());
        }
        private void senhaTextChanged(object sender, EventArgs e)
        {
            lbl_Aviso.IsVisible = !validarSenha(senha.Text);
        }
        //invocado a partir da label com valor igual a "Cadastrar"
        private void ChamarPagCadastrar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Pagina1());
        }
        //metodo responsavel por gerar novas senhas
        private string GerarSenha()
        {
            string[] valores = {"0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H","I","J","K",
                "L","M", "N","O","P","Q","R","S","T","U","V","W","Y","Z","@","!","#","$","%","-","/",":","?","_"};
            var a = new Random();
            string nova_senha = null;

            for (int i = 0; i < 12; i++)
            {
                nova_senha += valores[a.Next(0, valores.Length)];
            }

            return nova_senha;
        }
        //metedo responsavel por verificar se a senha cumpri os requisitos de segurança
        public bool validarSenha(string texto)
        {

            bool caracterMaiusculo = false;
            bool caractereEspecial = false;

            if (string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

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
        //metodo responsavel por verificar se o texto tem um @
        public bool validarEmail(string email)
        {
            if (email.Length < 1)
            {
                return false;
            }
            if (Regex.IsMatch(email, @"[@]") == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
