using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RoomApi.Models
{
    public interface ISkillOrTalentRepository
    {
        // Register a new skill or talent
        Task<SkillOrTalent> RegisterAsync(SkillOrTalent skillOrTalent);
        
        // Get a skill or talent by category (for example, a category like "Programming" or "Art")
        Task<SkillOrTalent> GetByCategoryAsync(string category);
        
        // Update an existing skill or talent
        Task<SkillOrTalent> UpdateSkillAsync(SkillOrTalent skillOrTalent);
        
        // Delete a skill or talent by its ID
        Task DeleteSkillAsync(string id);
        
        // Get all skills or talents
        Task<IEnumerable<SkillOrTalent>> GetAllSkillsAsync();
        
        // Get a skill or talent by the user's ID
        Task<SkillOrTalent> GetByUserAsync(string userId);
    }
}
