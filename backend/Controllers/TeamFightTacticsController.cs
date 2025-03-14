using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using backend.Models;
using backend.DTOs;

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

    [HttpGet("{id}", Name = "GetPieceById")]
    public async Task<IActionResult> GetPieceById(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT id, name, role, price FROM Pieces WHERE id=@id";

        try
        {
            var piece = await connection.QuerySingleOrDefaultAsync<Piece>(sql, id);

            return Ok(piece);
        }
        catch (Exception e)
        {
            var error = new
            {
                e.Message,
                e.StackTrace
            };

            return StatusCode(500, error);
        }

    }

    [HttpPost]
    public async Task<IActionResult> AddPiece(PieceDTO piece)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "INSERT INTO Pieces (name, role, price) VALUES (@name, @role, @price)";

        try
        {
            await connection.ExecuteAsync(sql, piece);

            return Created();
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

    [HttpDelete]
    public async Task<IActionResult> DeletePiece(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "DELETE FROM Pieces WHERE id=@id";

        try
        {
            await connection.ExecuteAsync(sql, new { id = id });

            return NoContent();
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

    [HttpPut]
    public async Task<IActionResult> UpdatePiece(PieceDTO piece, Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "UPDATE Pieces SET name=@name, role=@role, price=@price WHERE id=@id";

        try
        {
            await connection.ExecuteAsync(sql, new
            {
                name = piece.Name,
                role = piece.Role,
                price = piece.Price,
                id
            });

            return NoContent();
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