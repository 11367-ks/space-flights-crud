using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace space_flights_crud
{
    [Serializable]
    public class Flight
    {
        /// <summary>
        /// The unique identifier for the flight.
        /// </summary>
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string FlightId { get; set; }

        /// <summary>
        /// The model of the spaceship used for the flight.
        /// </summary>
        [BsonElement("ship_model"), BsonRepresentation(BsonType.String)]
        public string ShipModel { get; set; }

        /// <summary>
        /// The category of the flight.
        /// </summary>
        [BsonElement("category"), BsonRepresentation(BsonType.Int32)]
        public int Category { get; set; }

        /// <summary>
        /// The departure location of the flight.
        /// </summary>
        [BsonElement("from"), BsonRepresentation(BsonType.String)]
        public string From { get; set; }

        /// <summary>
        /// The departure time of the flight.
        /// </summary>
        [BsonElement("departure"), BsonRepresentation(BsonType.DateTime)]
        public DateTime Departure { get; set; }

        /// <summary>
        /// A list of waypoints with their corresponding arrival times.
        /// </summary>
        [BsonElement("waypoints"), BsonRepresentation(BsonType.Array)]
        public List<Waypoint> Waypoints { get; set; }
    }

    public class Waypoint
    {
        /// <summary>
        /// The name of the waypoint.
        /// </summary>
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        /// <summary>
        /// The arrival time at the waypoint.
        /// </summary>
        [BsonElement("arrival"), BsonRepresentation(BsonType.DateTime)]
        public DateTime Arrival { get; set; }
    }
}