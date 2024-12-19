using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class SkillOrTalent
{
    [BsonId]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string id => Id.ToString();

    // Default initialization for non-nullable properties
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProficiencyLevel { get; set; } = 1;  // Default to level 1 (beginner)
    public DateTime DateAcquired { get; set; } = DateTime.MinValue;  // Default to no date
    public string Category { get; set; } = string.Empty;
    public bool IsCertified { get; set; } = false;  // Default to no certification

    // Add UserId to associate the skill with a user
    public string UserId { get; set; } = string.Empty;  // UserId to link to a specific user
}
