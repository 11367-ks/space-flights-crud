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

        }
    }
}
