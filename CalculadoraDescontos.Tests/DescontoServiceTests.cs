using CalculadoraDescontos.App;
using Xunit;

namespace CalculadoraDescontos.Tests;

/// <summary>
/// Testes parametrizados da classe <see cref="DescontoService"/> escritos com xUnit.
/// Em vez de duplicar um método de teste para cada cenário (como seria necessário
/// com [Fact]), cada teste usa [Theory] + múltiplos [InlineData]: o xUnit executa o
/// mesmo método uma vez para cada linha de dados, e cada [InlineData] aparece no
/// resultado do "dotnet test" como um teste individual.
/// </summary>
public class DescontoServiceTests
{
    private readonly DescontoService _descontoService = new DescontoService();

    // =====================================================================
    // TESTES EXIGIDOS PELA LISTA DE EXERCÍCIOS ([Theory] + [InlineData])
    // =====================================================================

    /// <summary>
    /// Teste 1 (string): valida todas as categorias de cliente.
    /// Cada [InlineData] roda como um teste individual.
    /// </summary>
    [Theory]
    [InlineData(2, "BRONZE")]
    [InlineData(7, "PRATA")]
    [InlineData(15, "OURO")]
    public void ObterCategoriaCliente_DeveRetornarCategoriaCorreta(int totalCompras, string categoriaEsperada)
    {
        // Act
        string resultado = _descontoService.ObterCategoriaCliente(totalCompras);

        // Assert
        Assert.Equal(categoriaEsperada, resultado);
    }

    /// <summary>
    /// Teste 2 (int): valida o cálculo do valor final com desconto percentual.
    /// </summary>
    [Theory]
    [InlineData(100, 10, 90)]
    [InlineData(200, 20, 160)]
    [InlineData(50, 0, 50)]
    public void CalcularDescontoPorPercentual_DeveRetornarValorFinalCorreto(int valorOriginal, int percentualDesconto, int valorFinalEsperado)
    {
        // Act
        int resultado = _descontoService.CalcularDescontoPorPercentual(valorOriginal, percentualDesconto);

        // Assert
        Assert.Equal(valorFinalEsperado, resultado);
    }

    /// <summary>
    /// Teste 3 (bool): valida a elegibilidade ao cupom testando diferentes
    /// combinações de idade e primeira compra.
    /// </summary>
    [Theory]
    [InlineData(20, false, true)]   // Maior de idade, não é primeira compra -> true
    [InlineData(16, true, true)]    // Menor de idade, primeira compra       -> true
    [InlineData(17, false, false)]  // Menor de idade, não é primeira compra -> false
    public void EValidoParaCupom_DeveValidarElegibilidadeCorretamente(int idade, bool primeiraCompra, bool resultadoEsperado)
    {
        // Act
        bool resultado = _descontoService.EValidoParaCupom(idade, primeiraCompra);

        // Assert
        Assert.Equal(resultadoEsperado, resultado);
    }

    // =====================================================================
    // TESTES ADICIONAIS DE COBERTURA (valores-limite das regras de negócio)
    // =====================================================================

    /// <summary>
    /// Cobertura extra (string): valores exatamente nos limites das categorias
    /// (4/5 e 10/11), garantindo que as fronteiras das regras estão corretas.
    /// </summary>
    [Theory]
    [InlineData(0, "BRONZE")]   // Nenhuma compra
    [InlineData(4, "BRONZE")]   // Limite: última quantidade que ainda é BRONZE
    [InlineData(5, "PRATA")]    // Limite: primeira quantidade que já é PRATA
    [InlineData(10, "PRATA")]   // Limite: última quantidade que ainda é PRATA (inclusive)
    [InlineData(11, "OURO")]    // Limite: primeira quantidade que já é OURO
    public void ObterCategoriaCliente_ValoresLimitrofes_DeveRetornarCategoriaCorreta(int totalCompras, string categoriaEsperada)
    {
        // Act
        string resultado = _descontoService.ObterCategoriaCliente(totalCompras);

        // Assert
        Assert.Equal(categoriaEsperada, resultado);
    }

    /// <summary>
    /// Cobertura extra (int): percentual intermediário, desconto total (100%)
    /// e arredondamento da divisão inteira.
    /// </summary>
    [Theory]
    [InlineData(80, 15, 68)]      // 80 - (80*15/100 = 12) = 68
    [InlineData(1000, 100, 0)]    // Desconto de 100% zera o valor
    [InlineData(350, 5, 333)]     // 350 - (350*5/100 = 17) = 333
    public void CalcularDescontoPorPercentual_CenariosAdicionais_DeveRetornarValorFinalCorreto(int valorOriginal, int percentualDesconto, int valorFinalEsperado)
    {
        // Act
        int resultado = _descontoService.CalcularDescontoPorPercentual(valorOriginal, percentualDesconto);

        // Assert
        Assert.Equal(valorFinalEsperado, resultado);
    }

    /// <summary>
    /// Cobertura extra (bool): idade exatamente no limite (18 anos) e
    /// primeira compra para cliente infantil.
    /// </summary>
    [Theory]
    [InlineData(18, false, true)]  // Limite exato: 18 anos sem ser primeira compra -> true
    [InlineData(18, true, true)]   // 18 anos e primeira compra -> true
    [InlineData(10, true, true)]   // Criança na primeira compra -> true
    public void EValidoParaCupom_ValoresLimitrofes_DeveValidarElegibilidadeCorretamente(int idade, bool primeiraCompra, bool resultadoEsperado)
    {
        // Act
        bool resultado = _descontoService.EValidoParaCupom(idade, primeiraCompra);

        // Assert
        Assert.Equal(resultadoEsperado, resultado);
    }
}
