using CalculadoraDescontos.App;

// ============================================================
// CalculadoraDescontos.App - Demonstração rápida do DescontoService
// (O foco da solução são os testes parametrizados: execute "dotnet test")
// ============================================================

DescontoService descontoService = new DescontoService();

// Exemplo 1 (retorno string): categoria do cliente
Console.WriteLine($"Categoria com 2 compras:  {descontoService.ObterCategoriaCliente(2)}");
Console.WriteLine($"Categoria com 7 compras:  {descontoService.ObterCategoriaCliente(7)}");
Console.WriteLine($"Categoria com 15 compras: {descontoService.ObterCategoriaCliente(15)}");

// Exemplo 2 (retorno int): desconto percentual
Console.WriteLine($"Valor final de 100 com 10% de desconto: {descontoService.CalcularDescontoPorPercentual(100, 10)}");
Console.WriteLine($"Valor final de 200 com 20% de desconto: {descontoService.CalcularDescontoPorPercentual(200, 20)}");
Console.WriteLine($"Valor final de 50 com 0% de desconto:   {descontoService.CalcularDescontoPorPercentual(50, 0)}");

// Exemplo 3 (retorno bool): elegibilidade para cupom
Console.WriteLine($"Cupom válido (20 anos, não é primeira compra): {descontoService.EValidoParaCupom(20, false)}");
Console.WriteLine($"Cupom válido (16 anos, primeira compra):       {descontoService.EValidoParaCupom(16, true)}");
Console.WriteLine($"Cupom válido (17 anos, não é primeira compra): {descontoService.EValidoParaCupom(17, false)}");
