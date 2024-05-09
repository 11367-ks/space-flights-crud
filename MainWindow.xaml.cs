using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace space_flights_crud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string selectedID;
        private IMongoCollection<Flight> flightsCollection;

        public MainWindow()
        {
            InitializeComponent();
            FetchDataAsync();
        }

        private async Task FetchDataAsync()
        {
            flights.Items.Clear();
            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionURI"].ConnectionString;
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(dbName);
            flightsCollection = db.GetCollection<Flight>("flights");

            var filter = Builders<Flight>.Filter.Empty;
            var flightsList = await flightsCollection.Find(filter).ToListAsync();

            foreach (var flight in flightsList)
            {
                flights.Items.Add(flight.FlightId);
            }
        }

        private async void GetDetailsAsync()
        {
            if (string.IsNullOrEmpty(selectedID))
            {
                throw new ArgumentException("selectedID cannot be null or empty.");
            }

            var filter = Builders<Flight>.Filter.Eq("_id", new ObjectId(selectedID));
            Flight flight = await flightsCollection.Find(filter).FirstOrDefaultAsync();

            details.Items.Clear();
            details.Items.Add(flight.ShipModel);
            details.Items.Add(flight.Category);
            details.Items.Add(flight.From);
            details.Items.Add(flight.Departure);
            details.Items.Add(string.Join(", ", flight.Waypoints.Select(wp => wp.Destination)));
        }

        private void flights_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            details.Items.Clear();
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                selectedID = item.ToString();
            }
            GetDetailsAsync();
        }

        private async void refresh_Click(object sender, RoutedEventArgs e)
        {
            await FetchDataAsync();
        }

        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.Show();
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            ModifyWindow modifyWindow = new ModifyWindow(selectedID);
            modifyWindow.Show();
        }
    }
}