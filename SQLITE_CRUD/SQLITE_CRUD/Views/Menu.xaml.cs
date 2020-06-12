using SQLITE_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLITE_CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Menu : MasterDetailPage{
        private Usuario user;
        private string pasta;

		public Menu (Usuario user, string pasta)
		{
            this.user = user;
            this.pasta = pasta;
            InitializeComponent();
            DependencyService.Get<IBancoSQLite>().CriarDataBase_Jogo(pasta);
            NavigationPage.SetHasNavigationBar(this, false);
            Master.BackgroundColor = Color.White;
            lbl_cabecalho.Text += " Admin, \n" + user.Nome;
            Detail = new NavigationPage(new MenuDetail());
        }
        public Menu()
        {
            InitializeComponent();
            DependencyService.Get<IBancoSQLite>().CriarDataBase_Jogo(pasta);
            NavigationPage.SetHasNavigationBar(this, false);
            Master.BackgroundColor = Color.White;
            lbl_cabecalho.Text += " Admin, \n" + user.Nome;
            lbl_face.Text = "\uf09a";
            Detail = new NavigationPage(new MenuDetail());
        }
        //metodos que permitem que o usuario troque os temas
        private void btn_TemaAzul(object sender, EventArgs e)
        {
            DependencyService.Get<ITemas>().TemaAzul();
        }
        private void btn_TemaVerde(object sender, EventArgs e)
        {
            DependencyService.Get<ITemas>().TemaVerde();
        }
        private void btn_TemaVermelho(object sender, EventArgs e)
        {
            DependencyService.Get<ITemas>().TemaVermelho();
        }
        private void btn_TemaRoxo(object sender, EventArgs e)
        {
            DependencyService.Get<ITemas>().TemaRoxo();
        }
        private void btn_TemaClaro(object sender, EventArgs e)
        {
            DependencyService.Get<ITemas>().TemaClaro();
        }

        //carrega na Detail a pagina de Perfil
        private void Go_AlterarSenha(object sender, EventArgs e){
            Detail = new NavigationPage(new Perfil(user, pasta));
        }
        //carrega na Detail a pagina de Inicial
        private void Go_Home(object sender, EventArgs e){
            Detail = new NavigationPage(new MenuDetail());
        }
        //carrega na Detail a pagina de AdicionarJogo
        private void Go_AdicionarJogo(object sender, EventArgs e){
            Detail = new NavigationPage(new AdicionarJogo(pasta));
        }
        //carrega na Detail a pagina de ListaBanco
        private void Go_ListaLoja(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ListarBanco(user,pasta));
        }
        //retorna para a pagina inicial do app
        private void Go_LogOut(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
        //carrega na Detail a pagina de ConsultarJogo
        private void Go_ConsultarJogo(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Consulta_Jogo(user, pasta));
        }
    }
}