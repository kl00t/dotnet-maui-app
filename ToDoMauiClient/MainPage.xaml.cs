using System.Diagnostics;
using ToDoMauiClient.DataServices;

namespace ToDoMauiClient
{
    public partial class MainPage : ContentPage
    {
        private readonly IRestDataService _restDataService;

        public MainPage(IRestDataService restDataService)
        {
            InitializeComponent();

            _restDataService = restDataService;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            collectionView.ItemsSource = await _restDataService.GetAllToDosAsync();
        }

        async void OnAddToDoClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Add Button Clicked.");
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Item Changed Clicked.");
        }
    }
}