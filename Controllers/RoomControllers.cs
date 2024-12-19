using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace RoomApi.Controllers
{
    [ApiController]
    [Route("api/skills-or-talents")]
    public class SkillOrTalentController : ControllerBase
    {
        private readonly ISkillOrTalentService _skillOrTalentService;

        public SkillOrTalentController(ISkillOrTalentService skillOrTalentService)
        {
            _skillOrTalentService = skillOrTalentService;
        }

        // Register a new skill or talent
        [HttpPost("register")]
        public async Task<ActionResult<SkillOrTalent>> RegisterSkill(SkillOrTalent skillOrTalent)
        {
            var registeredSkill = await _skillOrTalentService.RegisterAsync(skillOrTalent);
            return CreatedAtAction(nameof(RegisterSkill), new { id = registeredSkill.Id }, registeredSkill);
        }

        // Get skill or talent by category
        [HttpGet("{category}")]
        public async Task<ActionResult<SkillOrTalent>> GetSkillByCategory(string category)
        {
            var skillOrTalent = await _skillOrTalentService.GetByCategoryAsync(category);
            if (skillOrTalent == null)
            {
                return NotFound(); // Skill or talent not found
            }
            return Ok(skillOrTalent); // Return skill or talent
        }

        // Update an existing skill or talent
        [HttpPut("update")]
        public async Task<ActionResult<SkillOrTalent>> UpdateSkill(SkillOrTalent skillOrTalent)
        {
            var updatedSkill = await _skillOrTalentService.UpdateSkillAsync(skillOrTalent);
            if (updatedSkill == null)
            {
                return NotFound(); // Skill or talent not found
            }
            return Ok(updatedSkill); // Return updated skill or talent
        }

        // Delete a skill or talent by ID
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteSkill(string id)
        {
            await _skillOrTalentService.DeleteSkillAsync(id);
            return NoContent(); // Successfully deleted
        }

        // Get all skills or talents
        [HttpGet("all")]
        public async Task<ActionResult> GetAllSkills()
        {
            var skillsOrTalents = await _skillOrTalentService.GetAllSkillsAsync();
            return Ok(skillsOrTalents); // Return all skills or talents
        }

        // Get a skill or talent by user ID
        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetSkillByUser(string userId)
        {
            var skillOrTalent = await _skillOrTalentService.GetByUserAsync(userId);
            if (skillOrTalent == null)
            {
                return NotFound(); // Skill or talent not found
            }
            return Ok(skillOrTalent); // Return skill or talent
        }
    }
}
