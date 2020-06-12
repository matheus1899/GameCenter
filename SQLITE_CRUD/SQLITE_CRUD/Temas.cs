using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SQLITE_CRUD.Temas))]
namespace SQLITE_CRUD
{
    public class Temas:ITemas
    {
        public void TemaAzul()
        {
            App.Current.Resources["Page_BackgroundColor"] = Color.FromHex("#001a2d");
            App.Current.Resources["MenuCabecalho_BackgroundColor"] = Color.FromHex("#001a2d");
            App.Current.Resources["MenuItens_BackgroundColor"] = Color.White;
            App.Current.Resources["Entry_TextColor"] = Color.White;
            App.Current.Resources["Entry_PlaceholderColor"] = Color.LightGray;
            App.Current.Resources["Label_Menu_TextColor"] = Color.FromHex("#001a2d");
            App.Current.Resources["Label_Page_TextColor"] = Color.White;
            App.Current.Resources["Label_Cadastrar"] = Color.LightBlue;
            App.Current.Resources["Label_Aviso"] = Color.FromHex("#ea4444");
            Application.Current.Properties["TEMA"] = "Azul";
        }
        public void TemaVerde()
        {
            App.Current.Resources["Page_BackgroundColor"] = Color.FromHex("#005418");
            App.Current.Resources["MenuCabecalho_BackgroundColor"] = Color.FromHex("#005418");
            App.Current.Resources["MenuItens_BackgroundColor"] = Color.White;
            App.Current.Resources["Entry_TextColor"] = Color.White;
            App.Current.Resources["Entry_PlaceholderColor"] = Color.LightGray;
            App.Current.Resources["Label_Menu_TextColor"] = Color.FromHex("#005418");
            App.Current.Resources["Label_Page_TextColor"] = Color.White;
            App.Current.Resources["Label_Cadastrar"] = Color.LightGreen;
            App.Current.Resources["Label_Aviso"] = Color.FromHex("#005418");
            Application.Current.Properties["TEMA"] = "Verde";
        }
        public void TemaVermelho()
        {
            App.Current.Resources["Page_BackgroundColor"] = Color.FromHex("#5b0000");
            App.Current.Resources["MenuCabecalho_BackgroundColor"] = Color.FromHex("#5b0000");
            App.Current.Resources["MenuItens_BackgroundColor"] = Color.White;
            App.Current.Resources["Entry_TextColor"] = Color.White;
            App.Current.Resources["Entry_PlaceholderColor"] = Color.LightGray;
            App.Current.Resources["Label_Menu_TextColor"] = Color.FromHex("#5b0000");
            App.Current.Resources["Label_Page_TextColor"] = Color.White;
            App.Current.Resources["Label_Cadastrar"] = Color.FromHex("ea4444");
            App.Current.Resources["Label_Aviso"] = Color.FromHex("#ea4444");
            Application.Current.Properties["TEMA"] = "Vermelho";
        }
        public void TemaRoxo()
        {
            App.Current.Resources["Page_BackgroundColor"] = Color.FromHex("#40007c");
            App.Current.Resources["MenuCabecalho_BackgroundColor"] = Color.FromHex("#40007c");
            App.Current.Resources["MenuItens_BackgroundColor"] = Color.White;
            App.Current.Resources["Entry_TextColor"] = Color.White;
            App.Current.Resources["Entry_PlaceholderColor"] = Color.LightGray;
            App.Current.Resources["Label_Menu_TextColor"] = Color.FromHex("#40007c");
            App.Current.Resources["Label_Page_TextColor"] = Color.White;
            App.Current.Resources["Label_Cadastrar"] = Color.FromHex("#8b43ea");
            App.Current.Resources["Label_Aviso"] = Color.FromHex("#ea4444");
            Application.Current.Properties["TEMA"] = "Roxo";
        }
        public void TemaClaro()
        {
            App.Current.Resources["Page_BackgroundColor"] = Color.FromHex("#c6c6c6");
            App.Current.Resources["MenuCabecalho_BackgroundColor"] = Color.FromHex("#c6c6c6");
            App.Current.Resources["MenuItens_BackgroundColor"] = Color.White;
            App.Current.Resources["Entry_TextColor"] = Color.Black;
            App.Current.Resources["Entry_PlaceholderColor"] = Color.FromHex("#424242");
            App.Current.Resources["Label_Menu_TextColor"] = Color.Black;
            App.Current.Resources["Label_Page_TextColor"] = Color.Black;
            App.Current.Resources["Label_Cadastrar"] = Color.FromHex("#606060");
            App.Current.Resources["Label_Aviso"] = Color.FromHex("#ea4444");
            Application.Current.Properties["TEMA"] = "Claro";

        }
    }
}
