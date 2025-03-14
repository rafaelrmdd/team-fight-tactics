using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("/api/tft")]
public class TeamFightTacticsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public TeamFightTacticsController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration["DB:MyConnectionString"]!;
    }

    [HttpGet]
    public async Task<IActionResult> GetPieces()
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT Id, Name, Role, Price FROM Pieces";

        try
        {
            var pieces = await connection.QueryAsync<Piece>(sql);

            return Ok(pieces);
        }
        catch (Exception e)
        {
            var error = new
            {
                Message = e.Message,
                StackTrace = e.StackTrace
            };

            return StatusCode(500, error);
        }

    }
}