using MongoDB.Bson.Serialization.Attributes;

namespace Ecoeden.Inventory.Domain.Entities;
public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    public void UpdateCreationData(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void UpdateUpdationData(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}
