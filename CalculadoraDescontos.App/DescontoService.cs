namespace CalculadoraDescontos.App;

/// <summary>
/// Serviço responsável pelas regras de negócio da calculadora de descontos:
/// categoria do cliente por volume de compras, cálculo do desconto percentual
/// e validação da elegibilidade para cupom.
/// </summary>
public class DescontoService
{
    /// <summary>
    /// (Retorno string) Classifica o cliente conforme o total de compras:
    /// "BRONZE" para menos de 5 compras, "PRATA" entre 5 e 10 (inclusive)
    /// e "OURO" para mais de 10 compras.
    /// </summary>
    /// <param name="totalCompras">Quantidade total de compras do cliente.</param>
    /// <returns>
    /// Categoria do cliente em maiúsculas.
    /// Exemplos: 2 retorna "BRONZE"; 7 retorna "PRATA"; 15 retorna "OURO".
    /// </returns>
    public string ObterCategoriaCliente(int totalCompras)
    {
        if (totalCompras < 5)
        {
            return "BRONZE";
        }

        if (totalCompras <= 10)
        {
            return "PRATA";
        }

        return "OURO";
    }

    /// <summary>
    /// (Retorno int) Calcula o valor final com o desconto percentual aplicado.
    /// </summary>
    /// <param name="valorOriginal">Valor original do produto em reais.</param>
    /// <param name="percentualDesconto">Percentual de desconto (ex.: 10 = 10%).</param>
    /// <returns>
    /// Valor final após o desconto.
    /// Exemplo: (100, 10) retorna 90; (200, 20) retorna 160; (50, 0) retorna 50.
    /// </returns>
    public int CalcularDescontoPorPercentual(int valorOriginal, int percentualDesconto)
    {
        int valorDoDesconto = (valorOriginal * percentualDesconto) / 100;
        return valorOriginal - valorDoDesconto;
    }

    /// <summary>
    /// (Retorno bool) Verifica se o cliente é elegível para o cupom.
    /// Regra: retorna true se o cliente tiver 18 anos ou mais OU se for a
    /// primeira compra dele. Caso contrário, retorna false.
    /// </summary>
    /// <param name="idade">Idade do cliente em anos.</param>
    /// <param name="primeiraCompra">Indica se é a primeira compra do cliente.</param>
    /// <returns>
    /// true quando o cliente é elegível ao cupom; caso contrário, false.
    /// Exemplos: (20, false) retorna true; (16, true) retorna true; (17, false) retorna false.
    /// </returns>
    public bool EValidoParaCupom(int idade, bool primeiraCompra)
    {
        return idade >= 18 || primeiraCompra;
    }
}
