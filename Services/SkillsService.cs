using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RoomApi.Models
{
    public class SkillOrTalentService : ISkillOrTalentService
    {
        private readonly ISkillOrTalentRepository _skillOrTalentRepository;

        public SkillOrTalentService(ISkillOrTalentRepository skillOrTalentRepository)
        {
            _skillOrTalentRepository = skillOrTalentRepository;
        }

        public async Task<SkillOrTalent> RegisterAsync(SkillOrTalent skillOrTalent)
        {
            return await _skillOrTalentRepository.RegisterAsync(skillOrTalent);
        }

        public async Task<SkillOrTalent> GetByCategoryAsync(string category)
        {
            return await _skillOrTalentRepository.GetByCategoryAsync(category);
        }

        public async Task<SkillOrTalent> UpdateSkillAsync(SkillOrTalent skillOrTalent)
        {
            return await _skillOrTalentRepository.UpdateSkillAsync(skillOrTalent);
        }

        public async Task DeleteSkillAsync(string id)
        {
            await _skillOrTalentRepository.DeleteSkillAsync(id);
        }

        public async Task<IEnumerable<SkillOrTalent>> GetAllSkillsAsync()
        {
            return await _skillOrTalentRepository.GetAllSkillsAsync();
        }

        public async Task<SkillOrTalent> GetByUserAsync(string userId)
        {
            return await _skillOrTalentRepository.GetByUserAsync(userId);
        }
    }
}
