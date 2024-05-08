using MongoDB.Driver;
using System.Configuration;
using System.Windows;

namespace space_flights_crud
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        IMongoCollection<Flight> flightsCollection;
        public ScheduleWindow()
        {
            InitializeComponent();
        }

        //TODO: Improve the method
        //TODO: Make the waypoint input field works
        private void InsertSampleData()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoConnectionURI"].ConnectionString;
            var dbName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase(dbName);
            flightsCollection = db.GetCollection<Flight>("flights");
            Flight flight = new Flight();
            flight.ShipModel = input_ship_model.Text;
            flight.Category = int.Parse(input_category.Text);
            flight.From = input_from.Text;
            flight.Departure = DateTime.Parse(input_departure.Text);
            List<char> charBuffer = new List<char>();
            foreach (char i in input_waypoints.Text)
            {
                if (i == ',')
                {
                    Waypoint tmpwp = new Waypoint();
                    tmpwp.Destination = new string(charBuffer.ToArray());
                    flight.Waypoints.Add(tmpwp);
                    charBuffer.Clear();
                }
                else
                {
                    charBuffer.Add(i);
                }
            }
            flightsCollection.InsertOne(flight);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            InsertSampleData();
        }
    }
}
