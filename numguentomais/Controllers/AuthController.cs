using beyondthegame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly string chaveSecreta; // Sua chave secreta aqui

    public AuthController(DatabaseContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        chaveSecreta = configuration["JwtSettings:Key"];
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] empresa empresa)
    {
        // Substitua esta lógica pela autenticação da empresa
        if (AutenticarEmpresa(empresa.nome, empresa.senha, out int idEmpresa))
        {
            var token = GerarTokenJWT(empresa.nome, idEmpresa);
            return Ok(new { Token = token });
        }
        else
        {
            return Unauthorized();
        }
    }

    private bool AutenticarEmpresa(string nome, string senha, out int idEmpresa)
    {
        idEmpresa = 0; // Inicialize com um valor padrão
        // Use o Entity Framework para verificar as credenciais da empresa no banco de dados
        var empresaAutenticada = _dbContext.empresa.FirstOrDefault(e => e.nome == nome && e.senha == senha);
        if (empresaAutenticada != null)
        {
            idEmpresa = empresaAutenticada.id;
            return true;
        }
        return false;
    }

    private string GerarTokenJWT(string nomeUsuario, int idEmpresa)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, nomeUsuario),
            new Claim("IdEmpresa", idEmpresa.ToString()), // Inclua o ID da empresa como reivindicação
            // Adicione outras reivindicações (claims) conforme necessário
        };

        var token = new JwtSecurityToken(
            issuer: "AuthService",
            audience: "empresa",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
