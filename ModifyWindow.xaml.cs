using MongoDB.Driver;
using System.Configuration;
using System.Windows;
using MongoDB.Bson;

namespace space_flights_crud
{
    /// <summary>
    /// Interaction logic for Modify.xaml
    /// </summary>
    public partial class ModifyWindow : Window
    {
        private string id;
        IMongoCollection<Flight> flightsCollection;
        public ModifyWindow(string id)
        {
            InitializeComponent();
            this.id = id;
            _ = FetchFillDataAsync();
        }
        private async Task FetchFillDataAsync()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionURI"].ConnectionString;
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(dbName);
            flightsCollection = db.GetCollection<Flight>("flights");

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("selectedID cannot be null or empty.");
            }

            var filter = Builders<Flight>.Filter.Eq("_id", new ObjectId(id));
            Flight flight = await flightsCollection.Find(filter).FirstOrDefaultAsync();

            title.Content = id.ToString();
            input_ship_model.Text = flight.ShipModel;
            input_category.Text = flight.Category.ToString();
            input_from.Text = flight.From;
            input_departure.Text = flight.Departure.ToString();
            input_waypoints.Text = string.Join(",", flight.Waypoints.Select(wp => wp.Destination));
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updatedFlight = new Flight
                {
                    FlightId = id,
                    ShipModel = input_ship_model.Text,
                    Category = int.Parse(input_category.Text),
                    From = input_from.Text,
                    Departure = DateTime.Parse(input_departure.Text),
                    Waypoints = input_waypoints.Text.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(dest => new Waypoint { Destination = dest }).ToList()
                };

                var filter = Builders<Flight>.Filter.Eq("_id", new ObjectId(id));
                var result = await flightsCollection.ReplaceOneAsync(filter, updatedFlight);

                if (result.IsAcknowledged && result.MatchedCount > 0)
                {
                    MessageBox.Show("Flight data updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update flight data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filter = Builders<Flight>.Filter.Eq("_id", new ObjectId(id));
            await flightsCollection.DeleteOneAsync(filter);
        }
    }
}