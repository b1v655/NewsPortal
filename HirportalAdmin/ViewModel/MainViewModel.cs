using HirportalAdmin.Model;
using HirportalAdmin.Presistence;
using HirportalData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HirportalAdmin.ViewModel
{
    public static class CurrentUser
    {
        public static String Name;
        public static String Password;
    }
    public class MainViewModel : ViewModelBase
    {

        private IHirportalModel model;
        private ObservableCollection<ArticlesDTO> articles;
        private ArticlesDTO _currentArticle;
        private Boolean _isLoaded;
        private Int32 _selectedIndex;

        public ObservableCollection<ArticlesDTO> Articles
        {
            get { return articles; }
            private set
            {
                if (articles != value)
                {
                    articles = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }
        public ArticlesDTO CurrentArticle
        {
            get { return _currentArticle; }
            private set
            {
                if (_currentArticle != value)
                {
                    _currentArticle = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A kiválasztott elem indexének lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                    if (_selectedIndex >= 0 && _selectedIndex < articles.Count())
                        CurrentArticle = new ArticlesDTO
                        {
                        Id = articles[_selectedIndex].Id,
                        Title = articles[_selectedIndex].Title,
                        User= articles[_selectedIndex].User,
                        Images = articles[_selectedIndex].Images,
                        Content = articles[_selectedIndex].Content,
                        Date = articles[_selectedIndex].Date,
                        Summary = articles[_selectedIndex].Summary,
                        IsMainArticle = articles[_selectedIndex].IsMainArticle,
                        };

                }
            }
        }
        public ArticlesDTO EditedArticle { get; private set; }
        public DelegateCommand CreateArticleCommand { get; private set; }
        public DelegateCommand DeleteArticleCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand UpdateArticleCommand { get; private set; }
        public DelegateCommand SaveChangesCommand { get; private set; }
        public DelegateCommand CancelChangesCommand { get; private set; }
        public DelegateCommand DeleteImageCommand { get; private set; }
        public DelegateCommand CreateImageCommand { get; private set; }

        public event EventHandler ExitApplication;
        public event EventHandler ArticleEditingStarted;
        public event EventHandler ArticleEditingFinished;
        public event EventHandler<ArticlesEventArgs> ImageEditingStarted;
        public event EventHandler<ArticlesEventArgs> SureDeleteArticle;
        //public event EventHandler OnA
        public MainViewModel(IHirportalModel model)
        {
            this.model = model;
            _isLoaded = false;
            _selectedIndex = -1;
            model.ArticlesChanged += ModelArticlesChanged;
            CreateArticleCommand = new DelegateCommand(param =>
            {
                EditedArticle = new ArticlesDTO();
                //EditedArticle.Id = -1;
                OnArticleEditingStarted();
            });

            DeleteArticleCommand = new DelegateCommand(param => DeleteArticle(param as ArticlesDTO));
            
            UpdateArticleCommand = new DelegateCommand(param => UpdateArticle(param as ArticlesDTO));
            CreateImageCommand = new DelegateCommand(param => OnImageEditingStarted((param as ArticlesDTO).Id));
            DeleteImageCommand = new DelegateCommand(param => DeleteImage(param as ImageDTO));
            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }
        private void Update()
        {
            Articles = new ObservableCollection<ArticlesDTO>(model.Articles.OrderByDescending(a => a.Date)); // az adatokat egy követett gyűjteménybe helyezzük
            SelectedIndex = 0;
            CurrentArticle = Articles[SelectedIndex];
            IsLoaded = true;
        }
        private void ModelArticlesChanged(object sender, ArticlesEventArgs e)
        {
            Update();
            /*Int32 index = Articles.IndexOf(Articles.FirstOrDefault(article => article.Id == e.articleId));
            Articles.RemoveAt(index); // módosítjuk a kollekciót
            Articles.Insert(index, model.Articles[index-1]);

            CurrentArticle = Articles[index]; // és az aktuális épületet*/
        }
        private void DeleteImage(ImageDTO image)
        {
            if (image == null)
                return;
            EditedArticle.Images.Remove(image);
        }
        private void SaveChanges()
        {
            // ellenőrzések
            if (String.IsNullOrEmpty(EditedArticle.Title))
            {
                OnMessageApplication("Az cim nincs megadva!");
                return;
            }
            if (String.IsNullOrEmpty(EditedArticle.Content))
            {
                OnMessageApplication("A cikk nincs megadva!");
                return;
            }
            if (String.IsNullOrEmpty(EditedArticle.Summary))
            {
                OnMessageApplication("Az osszegzés nincs megadva!");
                return;
            }
            if(EditedArticle.IsMainArticle && EditedArticle.Images.Count < 1)
            {
                OnMessageApplication("Fő cikk de nincsen kép!");
                return;
            }

            // mentés
            if (EditedArticle.Id == 0)
            {
                //EditedArticle.User = new UserDTO { Name = CurrentUser.Name, Password="" };
                EditedArticle.User = new UserDTO { Name = Articles[0].User.Name, Id = Articles[0].User.Id, Password = "" };
                EditedArticle.Date = DateTime.Now;
                model.CreateArticle(EditedArticle);
                Articles.Add(EditedArticle);
                CurrentArticle = EditedArticle;
                
            }
            else // ha már létezik az épület
            {
                EditedArticle.Date = DateTime.Now;
                foreach(ImageDTO image in CurrentArticle.Images.ToList())
                {
                    model.DeleteImage(image);
                }
                foreach (ImageDTO image in EditedArticle.Images.ToList())
                {
                    model.CreateImage(EditedArticle.Id,image.Image);
                }
                model.UpdateArticle(EditedArticle);
            }

            EditedArticle = null;

            OnArticleEditingFinished();
            Update();

        }
        public void CancelChanges()
            
        {           
            EditedArticle = null;
            OnArticleEditingFinished();
        }
        private void UpdateArticle(ArticlesDTO article)
        {
            if (article == null)
                return;

            EditedArticle = new ArticlesDTO
            {
                Id = article.Id,
                Summary = article.Summary,
                User = article.User,
                Title = article.Title,
                Content = article.Content,
                IsMainArticle = article.IsMainArticle,
                //Images=article.Images
            };
            OnArticleEditingStarted();
        }
        public void DeleteArticle(Int32 articleId)
        {
            Int32 i = 0;
            Boolean l = false;
            ArticlesDTO article=null;
           
            while(!l && i < Articles.Count)
            {
                article = Articles[i];
                l = article.Id == articleId;
                i++;
            }
            if (l)
            {
                model.DeleteArticle(article);
                Articles.Remove(article);
            }
        }
        private void DeleteArticle(ArticlesDTO article)
        {   if (article == null) return;
            if (SureDeleteArticle != null)
                SureDeleteArticle(this, new ArticlesEventArgs { articleId = article.Id });

        }
        private async void LoadAsync()
        {
            try
            {
                await model.LoadAsync();
                Articles = new ObservableCollection<ArticlesDTO>(model.Articles.OrderByDescending(a => a.Date)); // az adatokat egy követett gyűjteménybe helyezzük
                SelectedIndex = -1;
                IsLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        /// <summary>
        /// Mentés végreahajtása.
        /// </summary>
        private async void SaveAsync()
        {
            try
            {
                await model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
        private void OnArticleEditingStarted()
        {
            if (ArticleEditingStarted != null)
                ArticleEditingStarted(this, EventArgs.Empty);
        }
        private void OnArticleEditingFinished()
        {
            if (ArticleEditingFinished != null)
                ArticleEditingFinished(this, EventArgs.Empty);
        }
        private void OnImageEditingStarted(Int32 articleId)
        {
            if (ImageEditingStarted != null)
                ImageEditingStarted(this, new ArticlesEventArgs { articleId = articleId });
        }
    }
}
