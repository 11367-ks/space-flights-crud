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

        private void InsertSampleData()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionURI"].ConnectionString;
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(dbName);
            flightCollection = db.GetCollection<Flight>("flight");
            Flight flight = new Flight();
            Waypoint waypoint = new Waypoint();
            waypoint.Destination = "Test";
            waypoint.Arrival = DateTime.Now;
            flight.Waypoints.Add(waypoint);
            flightCollection.InsertOne(flight);
            // TODO: Complete the connection to database
        }

        private void deploy_Click(object sender, RoutedEventArgs e)
        {
            InsertSampleData();
        }
    }
}