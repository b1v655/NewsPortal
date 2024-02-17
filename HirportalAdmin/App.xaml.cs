using HirportalAdmin.Model;
using HirportalAdmin.Presistence;
using HirportalAdmin.View;
using HirportalAdmin.ViewModel;
using HirportalData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HirportalAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHirportalModel _model;
        private MainViewModel _viewModel;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainWindow _view;
        private MakeArticleWindow _makeView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new HirportalModel(new HirportalPresistence("http://localhost:58304/")); // megadjuk a szolgáltatás címét
            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();
        }
        public async void App_Exit(object sender, ExitEventArgs e)
        {
                await _model.LogoutAsync();
            
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
           
           // _model = new HirportalModel(new HirportalPresistence("http://localhost:58304/")); // megadjuk a szolgáltatás címét
            _viewModel = new MainViewModel(_model);
            _viewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _viewModel.ArticleEditingStarted += new EventHandler(MainViewModel_ArticleEditingStarted);
            _viewModel.ArticleEditingFinished += new EventHandler(MainViewModel_ArticleEditingFinished);
            _viewModel.ImageEditingStarted += new EventHandler<ArticlesEventArgs>(ViewModel_ImageEditingStarted);
            _viewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _viewModel.SureDeleteArticle += new EventHandler<ArticlesEventArgs>(ViewModel_DeleteArticle);
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
            _loginView.Close();
        }
        private void ViewModel_DeleteArticle(object sender,ArticlesEventArgs e)
        {
            if (MessageBox.Show("Bitos hogy kiszeretné törölni?", "Hirportál", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _viewModel.DeleteArticle(e.articleId);
            }
        }
        private void ViewModel_ImageEditingStarted(object sender, ArticlesEventArgs e)
        {
            try
            {
                // egy dialógusablakban bekérjük a fájlnevet
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    // kép létrehozása (a megfelelő méretekkel)
                    //if(_viewModel.EditedArticle==null)
                    //_model.CreateImage(e.articleId,ImageHandler.OpenAndResize(dialog.FileName, 600));
                    _viewModel.EditedArticle.Images.Add(new ImageDTO { ArticleId= _viewModel.EditedArticle.Id, Image= ImageHandler.OpenAndResize(dialog.FileName, 600) });
                    Console.WriteLine("A képek száma:"+_viewModel.EditedArticle.Images.Count);
                    _makeView.imageListBox.UpdateLayout();

                }
            }
            catch { }
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Hírportál", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Hírportál", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void MainViewModel_ArticleEditingStarted(object sender, EventArgs e)
        {
            _makeView = new MakeArticleWindow(); // külön szerkesztő dialógus az épületekre
            _makeView.DataContext = _viewModel;
            _makeView.Show();
        }

        private void MainViewModel_ArticleEditingFinished(object sender, EventArgs e)
        {
            _makeView.Close();
        }
        private async void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            // az adatok opcionáls mentése
            if (MessageBox.Show("Elmentsük a változtatásokat?", "Hirportál", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _model.SaveAsync();
            }

            Shutdown();
        }
    }
}
