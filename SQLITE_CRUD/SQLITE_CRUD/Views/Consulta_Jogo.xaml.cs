using Plugin.Media;
using SQLITE_CRUD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLITE_CRUD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Consulta_Jogo : ContentPage
    {
        private string pasta;
        private Usuario user;

        public Consulta_Jogo(Usuario user, string pasta) {
            this.pasta = pasta;
            this.user = user;
            InitializeComponent();
        }

        private async void alterar_jogo(object sender, EventArgs e)
        {
            //verifica se o campo nome é nulo
            if (string.IsNullOrEmpty(nome.Text) || string.IsNullOrWhiteSpace(nome.Text)) {
                await DisplayAlert("Aviso", "Por favor preencha o campo Nome para atualizar", "OK");
                return;
            }

            //verifica se o jogo existe no banco
            bool existe = DependencyService.Get<IBancoSQLite>().ExisteJogo(nome.Text, pasta);
            if (existe == false){
                await DisplayAlert("Aviso", "Jogo inexistente para ser alterado", "OK");
                return;
            }
            //pega o jogo a partir do nome digitado
            var jogo_veio = DependencyService.Get<IBancoSQLite>().RetornaJogo(nome.Text, pasta);
            //verifica se os campos são nulos
            if (string.IsNullOrEmpty(plataforma.Text) || string.IsNullOrWhiteSpace(plataforma.Text)) {
                await DisplayAlert("Aviso", "Por favor preencha o campo Plataforma para atualizar", "OK");
                return;
            }
            if (string.IsNullOrEmpty(genero.Text) || string.IsNullOrWhiteSpace(genero.Text)) {
                await DisplayAlert("Aviso", "Por favor preencha o campo Gênero para atualizar", "OK");
                return;
            }
            if (string.IsNullOrEmpty(class_indicativa.Text) || string.IsNullOrWhiteSpace(class_indicativa.Text)) {
                await DisplayAlert("Aviso", "Por favor preencha o campo Gênero para atualizar", "OK");
                return;
            }
            if (string.IsNullOrEmpty(ano.Text) || string.IsNullOrWhiteSpace(ano.Text)) {
                await DisplayAlert("Aviso", "Por favor preencha o campo Ano para atualizar", "OK");
                return;
            }

            //verifica se há valores não numéricos na string
            foreach (var i in ano.Text) {
                if (!Regex.IsMatch(i.ToString(), @"[0123456789]")) {
                    await DisplayAlert("Aviso", "Por favor, digite apenas valores númericos", "OK");
                    return;
                }
            }
            //verifica se há valores não numéricos na string
            foreach (var i in class_indicativa.Text) {
                if (!Regex.IsMatch(i.ToString(), @"[0123456789]")) {
                    await DisplayAlert("Aviso", "Por favor, digite apenas valores númericos", "OK");
                    return;
                }
            }
            //verifica se o tamanho da string do campo ANO é menor do que o exigido
            if (ano.Text.Length < 4)
            {
                await DisplayAlert("Aviso", "Por favor, preencha o campo Ano corretamente", "OK");
                return;
            }
            if (Convert.ToInt32(class_indicativa.Text) > 18)
            {
                class_indicativa.Text = "18";
            }
            
            //chagado nesta parte do código, significa que todas as 
            //verificações foram feitas e os requisitos cumpridos com sucesso

            //verifica se a atualização do jogo foi feita com sucesso ou não
            if (DependencyService.Get<IBancoSQLite>().AtualizarJogo(nome.Text, new Jogo {
                    Nome = nome.Text, Plataforma = plataforma.Text, Genero = genero.Text,
                    Ano_Lancamento = Convert.ToInt32(ano.Text),
                    Class_Indicativa = Convert.ToInt32(class_indicativa.Text),
                    Dir_Imagem = jogo_veio.Dir_Imagem
                }, pasta))
            {

                await DisplayAlert("Aviso", "O jogo "+nome.Text+" foi atualizado com sucesso", "OK");
                Limpar_Campos();
                btn_Foto.Source = "gamepad.png";
                return;
            }
            else
            {
                await DisplayAlert("Aviso", "Ocorreu algum erro ao tentar atualizar o jogo. Por favor, tente novamente mais tarde", "OK");
                Limpar_Campos();
                btn_Foto.Source = "gamepad.png";
                return;
            }

        }
        
        private async void deletar_jogo(object sender, EventArgs e)
        {
            //verifica se o campo nome é nulo 
            if (string.IsNullOrEmpty(nome.Text) || string.IsNullOrWhiteSpace(nome.Text))
            {
                await DisplayAlert("Aviso", "Por favor preencha o campo Nome para deletar", "OK");
                return;
            }

            bool existe = DependencyService.Get<IBancoSQLite>().ExisteJogo(nome.Text, pasta);
            //verifica se o jogo existe no banco de dados
            if (existe == false)
            {
                await DisplayAlert("Aviso", "Jogo inexistente para ser deletado", "OK");
                return;
            }
            //pergunta ao usuario se realmente deseja excluir do banco o jogo escolhido
            string res = await DisplayActionSheet("Aviso", "Sim", "Não", "Tem certeza que deseja apagar?");
            //se a resposta for Não, ele apenas retorna nulo
            if (res == "Não"){
                return;
            }
            //se a resposta for sim, ele irá permitir que o jogo seja excluido
            else{
                try{
                    //verifica se a exclusão foi realizada com ou sem sucesso
                    if (DependencyService.Get<IBancoSQLite>().DeletarJogo(nome.Text, pasta)) {
                        Limpar_Campos();
                        await DisplayAlert("Aviso", "Jogo deletado com sucesso", "OK");
                        App.Current.MainPage = new NavigationPage(new Menu(user, pasta));
                    }
                    else{
                        await DisplayAlert("Aviso", "Erro ao deletar! Confira se o jogo exista", "OK");
                        return;
                    }

                }
                catch (Exception ex){
                    await DisplayAlert("Aviso", ex.Message, "OK");
                    return;
                }
            }
        }

        private async void consultar_jogo(object sender, EventArgs e){
            //verifica se o campo nome é nulo
            if (string.IsNullOrEmpty(nome.Text) || string.IsNullOrWhiteSpace(nome.Text)) {
                await DisplayAlert("Aviso", "Por favor, preencha o campo Nome para realizar a consulta", "OK");
                return;
            }
            //verifica se o jogo existe no banco
            if (!DependencyService.Get<IBancoSQLite>().ExisteJogo(nome.Text,pasta))
            {
                await DisplayAlert("Aviso","Jogo não encontrado", "OK");
                return;
            }
            //retorna o objeto do tipo Jogo, contendo todas as informações do jogo
            //permitindo as exibição para o usuario
            var game = DependencyService.Get<IBancoSQLite>().RetornaJogo(nome.Text,pasta);
            nome.Text = game.Nome;
            plataforma.Text = game.Plataforma;
            class_indicativa.Text = game.Class_Indicativa.ToString();
            btn_Foto.Source = game.Dir_Imagem;
            genero.Text = game.Genero;
            ano.Text = game.Ano_Lancamento.ToString();

        }
        //metodo invocado quando btnLimpar_Campo(Button) for pressionado
        private void btnLimpar_campos(object sender, EventArgs e)
        {
            Limpar_Campos();
        }
        private void Limpar_Campos()
        {
            genero.Text = null;
            class_indicativa.Text = null;
            ano.Text = null;
            plataforma.Text = null;
            nome.Text = null;
            btn_Foto.Source = "gamepad.png";
        }

        //metodo responsavel por abrir a galeria padrão do dispositivo do usuario
        //permitindo que ele escolha a imagem que deseja colocar no banco
        private async Task GetPhotoAsync()
        {
            //verifica se o campo nome é nulo
            if (string.IsNullOrEmpty(nome.Text) || string.IsNullOrWhiteSpace(nome.Text))
            {
               await  DisplayAlert("Aviso","Por favor digite o Nome do jogo para alterar a imagem da capa","OK");
                return;
            }
            bool existe = DependencyService.Get<IBancoSQLite>().ExisteJogo(nome.Text,pasta);
            //verifica se o jogo existe no banco
            if (existe==false)
            {
                await DisplayAlert("Aviso", "Jogo não encontrado", "OK");
                return;
            }

            var media = CrossMedia.Current;
            var file = await media.PickPhotoAsync();
            var jogo = new Jogo();

            try
            {
                if (file != null)
                {
                    //diretorio recebe o diretorio completo da foto escolhida pelo usuario
                    string diretorio = file.Path;
                    //arquivo_nome recebe o nome do arquivo contido na string diretorio
                    string arquivo_nome = Path.GetFileName(file.Path);
                    //copia a imagem dos arquivos temporarios aplicativo 
                    //para o diretorio onde se encontra o banco
                    DependencyService.Get<IBancoSQLite>().CopiarImg(diretorio, pasta, arquivo_nome);
                    //efetua a atualização do jogo
                    DependencyService.Get<IBancoSQLite>().AtualizarJogo(nome.Text, new Jogo {
                        Dir_Imagem = pasta + "/" + arquivo_nome,
                        Ano_Lancamento = jogo.Ano_Lancamento,
                        Class_Indicativa = jogo.Class_Indicativa,
                        Genero = jogo.Genero,
                        Nome = jogo.Nome,
                        Plataforma = jogo.Plataforma }, pasta);
                        btn_Foto.Source = pasta + "/" + arquivo_nome;
                }
                else
                {
                    return;
                }
            }
            catch (NullReferenceException e)
            {
                return;
            }
            catch (Exception e)
            {
                return;
            }

        }
        //metodo invocado a partir de btn_Foto(ImageButton)
        private async void btn_FotoClicked(object sender, EventArgs e)
        {
            await GetPhotoAsync();
        }
    }
}