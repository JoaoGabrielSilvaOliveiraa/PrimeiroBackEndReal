using beyondthegame.Models;
using Microsoft.AspNetCore.Mvc;

namespace numguentomais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public UsuarioController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public ActionResult<List<usuario>> BuscarTodasOsUsuarios()
        {
            var usuario = _dbContext.usuario.ToList();
            return usuario;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUsuario(int id)
        {
            var usuario = await _dbContext.usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(); // A empresa não foi encontrada
            }

            _dbContext.usuario.Remove(usuario);
            await _dbContext.SaveChangesAsync();

            return NoContent(); // A empresa foi removida com sucesso
        }
    }

}