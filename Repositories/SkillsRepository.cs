using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;

namespace RoomApi.Models
{
    public class SkillOrTalentRepository : ISkillOrTalentRepository
    {
        private readonly IMongoCollection<SkillOrTalent> _skills;
        private readonly MongoClient _mongoClient;

        public SkillOrTalentRepository(IOptions<SkillOrTalentMongoDBSettings> settings)
        {
            if (settings == null || settings.Value == null)
            {
                throw new ArgumentNullException(nameof(settings), "MongoDB settings are missing.");
            }

            ValidateMongoDBSettings(settings.Value);

            // Initialize MongoDB client and database
            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = _mongoClient.GetDatabase(settings.Value.DatabaseName);
            _skills = database.GetCollection<SkillOrTalent>("SkillsOrTalents");
        }

        private void ValidateMongoDBSettings(SkillOrTalentMongoDBSettings settings)
        {
            // Ensure valid MongoDB connection settings
            if (string.IsNullOrEmpty(settings.ConnectionString) || string.IsNullOrEmpty(settings.DatabaseName))
            {
                throw new ArgumentException("Invalid MongoDB settings: ConnectionString and DatabaseName are required.");
            }
        }

        // Register a new skill or talent
        public async Task<SkillOrTalent> RegisterAsync(SkillOrTalent skillOrTalent)
        {
            if (skillOrTalent == null)
            {
                throw new ArgumentNullException(nameof(skillOrTalent), "Skill or talent cannot be null.");
            }

            try
            {
                await _skills.InsertOneAsync(skillOrTalent);
                return skillOrTalent;
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error registering skill or talent: {ex.Message}");
                throw;
            }
        }

        // Retrieve a skill or talent by category
        public async Task<SkillOrTalent> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null or empty.");
            }

            return await _skills.Find(s => s.Category == category).FirstOrDefaultAsync();
        }

        // Update an existing skill or talent
        public async Task<SkillOrTalent> UpdateSkillAsync(SkillOrTalent skillOrTalent)
        {
            if (skillOrTalent == null)
            {
                throw new ArgumentNullException(nameof(skillOrTalent), "Skill or talent cannot be null.");
            }

            try
            {
                var result = await _skills.ReplaceOneAsync(s => s.Id == skillOrTalent.Id, skillOrTalent);
                return result.IsAcknowledged ? skillOrTalent : null;
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error updating skill or talent: {ex.Message}");
                throw;
            }
        }

        // Delete a skill or talent by its ID
        public async Task DeleteSkillAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");
            }

            try
            {
                var result = await _skills.DeleteOneAsync(s => s.Id == ObjectId.Parse(id));
                if (!result.IsAcknowledged)
                {
                    Console.WriteLine("Delete operation not acknowledged.");
                }
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error deleting skill or talent: {ex.Message}");
                throw;
            }
        }

        // Retrieve all skills or talents
        public async Task<IEnumerable<SkillOrTalent>> GetAllSkillsAsync()
        {
            return await _skills.Find(_ => true).ToListAsync();
        }

        // Retrieve a skill or talent by user ID
        public async Task<SkillOrTalent> GetByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            return await _skills.Find(s => s.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
