using CareerPlatform.Application.DTOs.SkillNodes;
using CareerPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/skillnodes")]
public class SkillNodesController : ControllerBase
{
    private readonly ISkillNodeService _skillNodeService;

    public SkillNodesController(ISkillNodeService skillNodeService)
    {
        _skillNodeService = skillNodeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminSkillNodeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var skillNodes = await _skillNodeService.GetAllAsync(cancellationToken);
        return Ok(skillNodes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdminSkillNodeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var skillNode = await _skillNodeService.GetByIdAsync(id, cancellationToken);

        if (skillNode is null)
        {
            return NotFound(new { message = "Skill node was not found." });
        }

        return Ok(skillNode);
    }
}
