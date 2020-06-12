using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLITE_CRUD.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLITE_CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListarBanco : ContentPage
	{
        private List<Jogo> lista_jogos;
        private string pasta;
        public ListarBanco(Usuario user, string pasta){
            this.pasta = pasta;
			InitializeComponent();
            //metodo responsavel por preencher a listview 
            list_view.ItemsSource = DependencyService.Get<IBancoSQLite>().GetJogos(pasta);
		}
	}
}