public abstract  class MaterialBiblioteca{
    public int Id { get; init;}
    private string _titulo;
    public string Titulo { get 
    {return _titulo;}
     set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _titulo = "Título não informado";
            }
            else
            {
                _titulo = value;
            }
        }
    }
    
    private int _anoPublicacao;
    public int AnoPublicacao
    {
        get
        {
            return _anoPublicacao;
        }
        set
        {
            if (value < 0)
            {
                _anoPublicacao = 0;
            }
            else
            {
                _anoPublicacao = value;
            }
        }
    }

    public abstract decimal CalcularPrazo(); //livro/revista TEM/OBRIGA a fazer override

    public virtual string Descricao() // livro/receita PERMITE fazer override
{
    return $"Título: {Titulo}, Ano de Publicação: {AnoPublicacao}";
}
}

