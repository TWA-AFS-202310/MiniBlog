using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniBlog.Model
{
    public class User
    {
        public User(string name, string email = "anonymous@unknow.com")
        {
            this.Name = name;
            this.Email = email;
        }

        public static string CollectionName { get; set; } = "Users";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [EmailAddress]
        // [JsonRequired]
        // [JsonIgnore]
        public string Email { get; set; } = "anonymous@unknow.com";
    }
}
