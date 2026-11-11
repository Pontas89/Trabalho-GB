
using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Models;
using MinhaBiblioteca.Repositories;

namespace MinhaBiblioteca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly IMaterialRepository _repository;

    public MateriaisController(IMaterialRepository repository)
    {
        _repository = repository;
    }

    // GET: api/Materiais
    [HttpGet]
    public ActionResult<IEnumerable<MaterialBiblioteca>> ObterTodos()
    {
        return Ok(_repository.ObterTodos());
    }

    // GET: api/Materiais/5
    [HttpGet("{id:int}")]
    public ActionResult<MaterialBiblioteca> ObterPorId(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(material);
    }

    // POST: api/Materiais/livro
    [HttpPost("livro")]
    public ActionResult<Livro> CadastrarLivro(Livro livro)
    {
        var erro = ValidarMaterial(livro);

        if (erro != null)
            return BadRequest(erro);

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        var cadastrado = _repository.Adicionar(livro);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cadastrado.Id },
            cadastrado
        );
    }

    // POST: api/Materiais/revista
    [HttpPost("revista")]
    public ActionResult<Revista> CadastrarRevista(Revista revista)
    {
        var erro = ValidarMaterial(revista);

        if (erro != null)
            return BadRequest(erro);

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 || revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        var cadastrado = _repository.Adicionar(revista);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = cadastrado.Id },
            cadastrado
        );
    }

    // PUT: api/Materiais/livro/5
    [HttpPut("livro/{id:int}")]
    public IActionResult AtualizarLivro(int id, Livro livro)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Livro não encontrado.");

        if (existente is not Livro)
            return BadRequest("O material informado não é um livro.");

        var erro = ValidarMaterial(livro);

        if (erro != null)
            return BadRequest(erro);

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        livro.Id = id;

        if (!_repository.Atualizar(id, livro))
            return NotFound("Livro não encontrado.");

        return Ok(livro);
    }

    // PUT: api/Materiais/revista/5
    [HttpPut("revista/{id:int}")]
    public IActionResult AtualizarRevista(int id, Revista revista)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Revista não encontrada.");

        if (existente is not Revista)
            return BadRequest("O material informado não é uma revista.");

        var erro = ValidarMaterial(revista);

        if (erro != null)
            return BadRequest(erro);

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 || revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        revista.Id = id;

        if (!_repository.Atualizar(id, revista))
            return NotFound("Revista não encontrada.");

        return Ok(revista);
    }

    // DELETE: api/Materiais/5
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        if (!_repository.Remover(id))
            return NotFound("Material não encontrado.");

        return NoContent();
    }

    // GET: api/Materiais/5/descricao
    [HttpGet("{id:int}/descricao")]
    public IActionResult ObterDescricao(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(new
        {
            id = material.Id,
            tipo = material.GetType().Name,
            descricao = material.Descricao(),
            prazoEmprestimo = material.CalcularPrazo()
        });
    }

    // POST: api/Materiais/5/emprestar
    [HttpPost("{id:int}/emprestar")]
    public IActionResult Emprestar(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        if (material is not IEmprestimo emprestimo)
            return BadRequest("Este material não permite empréstimo.");

        emprestimo.Emprestar();

        return Ok(
            $"O livro '{material.Titulo}' pode ser emprestado por {material.CalcularPrazo()} dias."
        );
    }

    // Validação dos dados comuns a Livro e Revista
    private string? ValidarMaterial(MaterialBiblioteca material)
    {
        if (string.IsNullOrWhiteSpace(material.Titulo))
            return "O título é obrigatório.";

        if (material.AnoPublicacao < 0)
            return "O ano de publicação não pode ser negativo.";

        return null;
    }
}
