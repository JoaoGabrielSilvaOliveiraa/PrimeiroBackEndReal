using beyondthegame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace beyond.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class EmpresaController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        
        private readonly string chaveSecreta; // Sua chave secreta aqui
        public EmpresaController(DatabaseContext dbContext, IConfiguration configuration) 
        {
            _dbContext = dbContext;
            chaveSecreta = configuration["JwtSettings:Key"];
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


        [HttpGet]
        public ActionResult<List<empresa>> BuscarTodasAsEmpresas()
        {
            var empresas = _dbContext.empresa.ToList();
            return empresas;
        }





        [HttpPost]

        public async Task<empresa> Adicionar(empresa empresa)
        {
            await _dbContext.empresa.AddAsync(empresa);
            await _dbContext.SaveChangesAsync();

            return empresa;
        }

        [HttpGet("jogos-da-empresa-logada")]
        public IActionResult ObterJogosDaEmpresaLogada()
        {
            // Verifique se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // O usuário não está autenticado
            }

            // Obtenha o ID da empresa do token JWT
            var idEmpresaLogada = User.FindFirstValue("IdEmpresa");

            if (string.IsNullOrEmpty(idEmpresaLogada) || !int.TryParse(idEmpresaLogada, out int idEmpresa))
            {
                return Unauthorized(); // O ID da empresa no token JWT é inválido
            }

            // Agora você tem o ID da empresa logada para usar na consulta dos jogos da empresa
            var jogosDaEmpresa = _dbContext.jogo
                .Where(jogo => jogo.empresa_id == idEmpresa)
                .ToList();

            return Ok(jogosDaEmpresa);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverEmpresa(int id)
        {
            var empresa = await _dbContext.empresa.FindAsync(id);

            if (empresa == null)
            {
                return NotFound(); // A empresa não foi encontrada
            }

            _dbContext.empresa.Remove(empresa);
            await _dbContext.SaveChangesAsync();

            return NoContent(); // A empresa foi removida com sucesso
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEmpresa(int id, [FromBody] empresa empresa)
        {
            // Obtenha o token JWT do cabeçalho da solicitação
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                Console.WriteLine("Ola");
            }

           

            // Verifique o token JWT para autenticação
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567891011121314151617181920211223242526272829303132333435"));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "AuthService",
                    ValidAudience = "empresa",
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var idUsuario = jwtToken.Claims.First(claim => claim.Type == "IdEmpresa").Value;

                if (!int.TryParse(idUsuario, out var idUsuarioInt) || idUsuarioInt != id)
                {
                    return Forbid(); // O usuário não tem permissão para editar esta empresa
                }

                // Resto da lógica de atualização da empresa

                return NoContent(); // Atualização bem-sucedida
            }
            catch (Exception)
            {
                return Unauthorized(); // Token JWT inválido
            }
        }




        public class AuthController : ControllerBase
        {
            private readonly IConfiguration _configuration;

            public AuthController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public IActionResult Login()
            {
                // Simule a autenticação. Você pode verificar as credenciais do usuário aqui.
                // Por exemplo, verificar um banco de dados ou outro local de armazenamento.
                string username = "usuario";
                string password = "senha";

                if (username == "usuario" && password == "senha")
                {
                    // O usuário está autenticado com sucesso, agora vamos gerar um token JWT.

                    // Chave secreta para assinar o token
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JwtSettings:Issuer"],
                        audience: _configuration["JwtSettings:Audience"],
                        expires: DateTime.UtcNow.AddHours(1),
                        claims: new[]
                        {
                        new Claim(ClaimTypes.Name, username),
                            // Você pode adicionar outras reivindicações (claims) conforme necessário.
                        },
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new { Token = tokenString });
                }

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

        
    }
}
    

