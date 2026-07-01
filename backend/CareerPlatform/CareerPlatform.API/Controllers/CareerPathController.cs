using System.Security.Claims;
using CareerPlatform.Application.DTOs.CareerPaths;
using CareerPlatform.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[ApiController]
[Route("api/careerpaths")]
public class CareerPathController : ControllerBase
{
    private readonly ICareerPathService _careerPathService;
    private readonly IValidator<CreateCareerPathDto> _createValidator;
    private readonly IValidator<UpdateCareerPathDto> _updateValidator;
    private readonly IValidator<SelectCareerPathDto> _selectValidator;

    public CareerPathController(
        ICareerPathService careerPathService,
        IValidator<CreateCareerPathDto> createValidator,
        IValidator<UpdateCareerPathDto> updateValidator,
        IValidator<SelectCareerPathDto> selectValidator)
    {
        _careerPathService = careerPathService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _selectValidator = selectValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CareerPathDto>>> GetAll(CancellationToken cancellationToken)
    {
        var careerPaths = await _careerPathService.GetAllAsync(cancellationToken);
        return Ok(careerPaths);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CareerPathDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var careerPath = await _careerPathService.GetByIdAsync(id, cancellationToken);

        if (careerPath is null)
        {
            return NotFound(new { message = "Career path was not found." });
        }

        return Ok(careerPath);
    }

    [HttpPost]
    public async Task<ActionResult<CareerPathDto>> Create(
        CreateCareerPathDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.ToDictionary() });
        }

        try
        {
            var careerPath = await _careerPathService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = careerPath.CareerPathId }, careerPath);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CareerPathDto>> Update(
        int id,
        UpdateCareerPathDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.ToDictionary() });
        }

        try
        {
            var careerPath = await _careerPathService.UpdateAsync(id, request, cancellationToken);
            return Ok(careerPath);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _careerPathService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [Authorize]
    [HttpPost("select")]
    public async Task<ActionResult<CareerPathDto>> Select(
        SelectCareerPathDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _selectValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.ToDictionary() });
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var selectedCareerPath = await _careerPathService.SelectAsync(userId.Value, request.CareerPathId, cancellationToken);
            return Ok(selectedCareerPath);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    private Guid? GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
