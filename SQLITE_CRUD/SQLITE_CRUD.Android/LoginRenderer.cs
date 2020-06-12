using System;
using Android.App;
using Xamarin.Auth;
using Xamarin.Forms;
using Newtonsoft.Json;
using SQLITE_CRUD.Views;
using SQLITE_CRUD.Droid;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Platform.Android;
using SQLITE_CRUD;

[assembly:ExportRenderer(typeof(FacebookPageLogin),typeof(LoginRenderer))]
namespace SQLITE_CRUD.Droid
{
    public class LoginRenderer:PageRenderer
    {
        [Obsolete]
        public LoginRenderer()
        {
            var act = this.Context as Activity;
            var auth = new OAuth2Authenticator("537894073281743","", new Uri("https://m.facebook.com/dialog/oauth"),new Uri("http://www.facebook.com/connect/login_sucess.html"));

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var acessToken = eventArgs.Account.Properties["acess_token"].ToString();

                    var request = new OAuth2Request("GET",new Uri("https://graph.facebook.com.me"),null,eventArgs.Account);
                    var response = await request.GetResponseAsync();
                    var obj = JObject.Parse(response.GetResponseText());

                    var nome = obj["name"].ToString().Replace("\"","");
                    SQLITE_CRUD.App.Current.MainPage = new NavigationPage(new MainPage());
                }
            };
            act.StartActivity(auth.GetUI(act));
        }
    }
}