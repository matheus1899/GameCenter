using Plugin.Media;
using SQLITE_CRUD.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLITE_CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdicionarJogo : ContentPage
	{
        private string pasta;
        private string diretorio_imagem = null;

		public AdicionarJogo (string pasta){
            this.pasta = pasta;
			InitializeComponent ();
		}
        private async Task GetPhotoAsync(){
            var media = CrossMedia.Current;
            var file = await media.PickPhotoAsync();

            try {
                if (file != null){
                    string diretorio = file.Path;
                    string arquivo_nome = Path.GetFileName(file.Path);
                    //copia a imagem do diretorio de arquivos temporários do aplicativo
                    //para o diretorio onde se encontra o banco SQLite
                    DependencyService.Get<IBancoSQLite>().CopiarImg(diretorio, pasta, arquivo_nome);
                    btn_Foto.Source = pasta + "/" + arquivo_nome;
                    diretorio_imagem = pasta + "/" + arquivo_nome;
                }
                else{
                    //Aviso ao usuario de que ocorreu falha no sistema
                    //e não foi possível completar a operação
                    await DisplayAlert("Aviso", "Ocorreu algum problema, tente novamente mais tarde", "OK");
                    return;
                }
            }
            catch (NullReferenceException e){
                return;
            }
            catch (Exception e){
                return;
            }

        }

        //Metodo acionado quando o btnFoto(ImageButton) é pressionado
        private async void btn_FotoClicked(object sender, EventArgs e){
            await GetPhotoAsync();
        }
        //Metodo acionado quando o btn_Adicionar(Button) é pressionado
        private async void btnAdd_Clicked(object sender, EventArgs e){
            if (string.IsNullOrEmpty(ent_Nome.Text) || string.IsNullOrEmpty(ent_Genero.Text) || string.IsNullOrEmpty(ent_Classificacao.Text) || 
                string.IsNullOrEmpty(ent_Plataforma.Text) || string.IsNullOrEmpty(ent_Ano.Text)){
                await DisplayAlert("Aviso", "Por favor, preencha todos os campo para realizar o cadastro", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(ent_Nome.Text) || string.IsNullOrWhiteSpace(ent_Genero.Text) || 
                string.IsNullOrWhiteSpace(ent_Classificacao.Text) || string.IsNullOrWhiteSpace(ent_Plataforma.Text) 
                || string.IsNullOrWhiteSpace(ent_Ano.Text)){
                await DisplayAlert("Aviso", "Por favor, preencha todos os campo para realizar o cadastro", "OK");
                return;
            }

            //verifica se há valores não númericos na string
            foreach(var i in ent_Classificacao.Text){
                if (Regex.IsMatch(i.ToString(), @"[0123456789]") == false){
                    await DisplayAlert("Aviso", "Por favor, digite apenas valores númericos", "OK");
                    return;
                }
            }
            //verifica se há valores não numéricos na string
            foreach (var i in ent_Ano.Text){
                if (Regex.IsMatch(i.ToString(), @"[0123456789]") == false){
                    await DisplayAlert("Aviso", "Por favor, digite apenas valores númericos", "OK");
                    return;
                }
            }
            //verifica se há algum valor na propriedade diretrio_imagem
            if (string.IsNullOrEmpty(diretorio_imagem) || string.IsNullOrWhiteSpace(diretorio_imagem))
            {
                diretorio_imagem = "gamepad.png";
            }            

            //verifica se a inclusão no banco de dados foi ou não, bem sucedida
            if(DependencyService.Get<IBancoSQLite>().InserirJogo(new Jogo {
                Genero = ent_Genero.Text,
                Plataforma = ent_Plataforma.Text,
                Ano_Lancamento = Convert.ToInt32(ent_Ano.Text),
                Class_Indicativa = Convert.ToInt32(ent_Classificacao.Text),
                Nome = ent_Nome.Text,
                Dir_Imagem = diretorio_imagem
            }, pasta)==true)
            {
                await DisplayAlert("Sucesso", "O item "+ent_Nome.Text+" foi adicionado", "Ótimo");
                btn_Foto.Source = "gamepad.png";
                LimparCampos();
                return;
            }
            else{
                await DisplayAlert("Desculpe", "Ocorreu algum erro na tentativa de registrar o item. Por favor, tente novamente mais tarde", "OK");
                return;
            }
        }
        //Metodo que é invocado quando o botão Limpar Campos é pressionado
        private void btn_Limpar(object sender, EventArgs e)
        {
            LimparCampos();
        }
        //Metodo que realiza a "limpeza" dos campos
        private void LimparCampos()
        {
            ent_Ano.Text = null;
            ent_Classificacao.Text = null;
            ent_Genero.Text = null;
            ent_Nome.Text = null;
            ent_Plataforma.Text = null;
            btn_Foto.Source = "gamepad.png";
        }
    }
}