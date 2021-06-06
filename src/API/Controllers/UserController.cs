using API.Configurations;
using Infrastructure.Application.Core;
using Infrastructure.Identity.Core;
using Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Dtos;

namespace API.Controllers
{
    [Route("[controller]")] 
    public class UserController : WebApiBase
    {
        private readonly UserModel[] _users = new[]
        {
            new UserModel() { Id = Guid.NewGuid() ,UserName = "Maria", Role = "Administrador" },
            new UserModel() { Id = Guid.NewGuid() ,UserName = "João", Role = "Usuario" },
        };

        public UserController(IHttpContextAccessor httpContextAccessor, IAuthentication authentication) : base(httpContextAccessor, authentication)
        {
        }

        /// <summary>
        /// Autenticar um usuário.
        /// </summary>
        /// <param name="dto">Nome do usuário</param>
        /// <returns>Retorna o token</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserDto dto)
        {
            var user = _users.SingleOrDefault(x => x.UserName == dto.UserName);
            if (user is null) throw new BusinessException("Usuario não encontrado.");
             
            return Ok(await Authentication.GenerateToken(user.Id, user.Role));
        } 
    }
}
