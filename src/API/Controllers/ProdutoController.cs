using System;
using System.Linq;
using API.Configurations;
using Infrastructure.Identity.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Application.Core;
using Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ProdutoController : WebApiBase
    {
        private static readonly Produto[] Produtos = new[]
        {
            new Produto() { Id = 1, Nome = "Arroz 5 Kg", Valor = 16.99M},
            new Produto() { Id = 2, Nome = "Feijão 1 Kg", Valor = 9.50M},
            new Produto() { Id = 3, Nome = "Farofa 1 Kg", Valor = 8.20M},
        };

        public ProdutoController(IHttpContextAccessor httpContextAccessor, IAuthentication authentication) : base(httpContextAccessor, authentication) { }

        [HttpGet]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> ObterTodosAsync()
        {
            var resultado = new
            {
                UsuarioLogado = User.Id,
                Produtos
            };

            return Ok(await Task.FromResult(resultado));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObterPorAsync(int id)
        {
            var produto = Produtos.SingleOrDefault(x => x.Id == id);
            if (produto is null) throw new BusinessException("Produto não encontrado");

            var resultado = new
            {
                UsuarioLogado = User.Id,
                produto
            };

            return Ok(await Task.FromResult(resultado));
        }
    }
}
