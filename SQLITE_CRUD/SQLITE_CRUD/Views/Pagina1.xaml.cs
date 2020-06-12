using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLITE_CRUD.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Mail;

namespace SQLITE_CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pagina1 : ContentPage
    {
       private string pasta;
 
        public Pagina1()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            pasta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        private void Nome_to(object sender, EventArgs e)
        {
            cpf.Focus();
        }

        private void btnImageClicked2(object sender, EventArgs e)
        {
            senhaAdmin.IsPassword = !senhaAdmin.IsPassword;
        }

        private void btnImageClicked(object sender, EventArgs e)
        {
            senha.IsPassword = !senha.IsPassword;
        }

        private void btnLimparCampos(object sender, EventArgs e)
        {
            nome.Text = null;
            cpf.Text = null;
            senha.Text = null;
            email.Text = null;
            if (senhaAdmin.IsEnabled==true)
            {
                senhaAdmin.Text = null;
            }
        }

        private void btnEnviar(object sender, EventArgs e) {
            
            if (validarCPF(cpf.Text)==false){
                DisplayAlert("Aviso", "CPF Inválido", "OK");
                cpf.Text = null;
                return;
            }
            if (string.IsNullOrEmpty(nome.Text) || string.IsNullOrWhiteSpace(nome.Text))
            {
                DisplayAlert("Aviso", "Por favor, preencha o seu nome", "Claro!");
                return;
            }
            if (switch_admin.IsToggled==true && validarAdmin(senhaAdmin.Text)==false) {
                DisplayAlert("Aviso", "Código de administrador incorreto", "OK");
                return;
            }

            Criptografia criptografia = new Criptografia();
            string senha_encriptada = criptografia.Encrypt(senha.Text);

            if (DependencyService.Get<IBancoSQLite>().InserirUsuario(new Usuario { CPF=cpf.Text, Nome=nome.Text, Email = email.Text,
                Senha = senha_encriptada, Admin=validarAdmin(senhaAdmin.Text) }, pasta) == true) {
                try
                {

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("xamarin.teste.email@gmail.com");
                    mail.To.Add(email.Text);
                    mail.Subject = "Seja Bem Vindo ao Game Center!";
                    mail.Body = "Esperamos que goste da sua experiência com o aplicativo.";

                    SmtpServer.Port = 587;
                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("xamarin.teste.email@gmail.com", "testexamarin");
                    object a = new object();
                    SmtpServer.SendAsync(mail,a);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Falha", ex.Message, "OK");
                }
                DisplayAlert("Sucesso", "Cadastro realizado com sucesso", "OK");
                Navigation.PopAsync();
            }
            else {
                DisplayAlert("Desculpe", "Cadastro não realizado. Tente novamente mais tarde", "OK");
            }
        }

        private void senhaTextChanged(object sender, EventArgs e)
        {
            lbl_Aviso.IsVisible = !validarSenha(senha.Text);
        }
        
        public bool validarSenha(string texto)
        {

            bool caracterMaiusculo = false;
            bool caractereEspecial = false;

            if(string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
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

        private void OnToggled(object sender, EventArgs e) {
            if (switch_admin.IsToggled==true) {
                stack_Admin.IsVisible = true;
            }
            else{
                stack_Admin.IsVisible = false;
            }
        }

        public bool validarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || string.IsNullOrWhiteSpace(cpf))
            {
                return false;
            }
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);

        }

        public bool validarAdmin(string senhaAdmin)
        {
            if (string.IsNullOrEmpty(senhaAdmin))
            {
                return false;
            }
            if (switch_admin.IsToggled == false)
            {
                return false;
            }
            else {
                if (senhaAdmin == "Administrador_GameCenter")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
    }
}