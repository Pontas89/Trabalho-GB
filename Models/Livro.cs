public class Livro : MaterialBiblioteca, IEmprestimo
{
    private string _autor;
    private int _paginas;
    public int Paginas
    {
        get
        {
            return _paginas;
        }
        set
        {
            if(value > 0)
            {
                _paginas = value;
            }
            else
            {
                Console.WriteLine("Número de páginas inválido. O livro deve ter pelo menos uma página.");
            }
        }
    }
    public string Autor
    {
        get
        {
            return _autor;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _autor = value;
            }
            else
            {
                Console.WriteLine("Autor não informado. O Livro deve possuir autor informado.");
            }
        }

    }

    public override decimal CalcularPrazo()
    {
        return 14; // Prazo de 14 dias para devolução de livros
    }

    public override string Descricao()
    {
        return $"Título: {Titulo}, Autor: {Autor}, Ano de Publicação: {AnoPublicacao}, Páginas: {Paginas}";
    }
}