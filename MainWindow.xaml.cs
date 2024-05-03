using MongoDB.Driver;
using System.Configuration;
using System.Windows;

namespace space_flights_crud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IMongoCollection<Flight> flightCollection;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionURI"].ConnectionString;
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(dbName);
            flightCollection = db.GetCollection<Flight>("flight");
            // TODO: Complete the connection to database
        }
    }
}