using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Room
{
    [BsonId]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string id => Id.ToString();

    // Use default initialization to ensure non-nullable properties are initialized
    public string UserId { get; set; } = string.Empty;
    public string Pictures { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
     public string Price { get; set; } = string.Empty;
      public string number { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
}