# calculadora-descontos-xunit

![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4.svg)
![xUnit](https://img.shields.io/badge/testes-xUnit%20Theory%20%2B%20InlineData-009E66.svg)

> **Garantia da Qualidade de Software — Gestão e Qualidade de Software**
> Professor: Daniel Henrique Matos de Paiva
> Aluno: **João Vitor Alves Rodrigues** — RA: **32513480**

Solução **.NET 10** criada via terminal (.NET CLI) para testar diversos cenários de uma calculadora de descontos, com entradas e retornos dos tipos **`string`**, **`int`** e **`bool`**. O objetivo central da lista é compreender na prática a diferença entre **`[Fact]`** e **`[Theory]`** — todos os testes deste repositório são **parametrizados com `[Theory]` + múltiplos `[InlineData(...)]`**, sem duplicar métodos de teste por cenário.

---

## 🧠 `[Fact]` vs `[Theory]` — qual a diferença?

| | `[Fact]` | `[Theory]` |
|---|---|---|
| **O que é** | Teste **único**, sem parâmetros | Teste **parametrizado** executado **múltiplas vezes** |
| **Dados de entrada** | Fixos dentro do próprio método | Fornecidos por atributos de dados: `[InlineData(...)]`, `[MemberData(...)]`, `[ClassData(...)]` |
| **Quantas vezes roda** | 1 vez | 1 vez **por linha de dados** (cada `[InlineData]` vira um teste individual no resultado) |
| **Assinatura do método** | `public void Teste()` | `public void Teste(int entrada, string esperado)` — recebe os parâmetros do `[InlineData]` |
| **Quando usar** | Um único cenário/regra | Vários cenários da mesma regra (ex.: valores-limite, combinações) |
| **Vantagem** | Simplicidade | **Não duplica código**: 1 método cobre N cenários |

### Exemplo com `[Fact]` (1 cenário = 1 método — precisaria de 3 métodos para 3 categorias):

```csharp
[Fact]
public void ObterCategoriaCliente_Com2Compras_DeveRetornarBronze()
{
    string resultado = _descontoService.ObterCategoriaCliente(2);
    Assert.Equal("BRONZE", resultado);
}
```

### O mesmo teste com `[Theory]` + `[InlineData]` (1 método cobre os 3 cenários):

```csharp
[Theory]
[InlineData(2, "BRONZE")]
[InlineData(7, "PRATA")]
[InlineData(15, "OURO")]
public void ObterCategoriaCliente_DeveRetornarCategoriaCorreta(int totalCompras, string categoriaEsperada)
{
    string resultado = _descontoService.ObterCategoriaCliente(totalCompras);
    Assert.Equal(categoriaEsperada, resultado);
}
```

No resultado do `dotnet test`, cada `[InlineData]` aparece como um **teste individual**, com os valores usados:

```
Passed DescontoServiceTests.ObterCategoriaCliente_DeveRetornarCategoriaCorreta(totalCompras: 2, categoriaEsperada: "BRONZE")
Passed DescontoServiceTests.ObterCategoriaCliente_DeveRetornarCategoriaCorreta(totalCompras: 7, categoriaEsperada: "PRATA")
Passed DescontoServiceTests.ObterCategoriaCliente_DeveRetornarCategoriaCorreta(totalCompras: 15, categoriaEsperada: "OURO")
```

---

## 📁 Estrutura da solução

```
CalculadoraDescontos.slnx                  -> Solução (criada com `dotnet new sln`)
├── CalculadoraDescontos.App/              -> Código de produção (console)
│   ├── CalculadoraDescontos.App.csproj
│   ├── Program.cs                         -> Demonstração de uso dos métodos
│   └── DescontoService.cs                 -> Regras de negócio (string, int e bool)
└── CalculadoraDescontos.Tests/            -> Testes unitários (xUnit)
    ├── CalculadoraDescontos.Tests.csproj  -> Referencia o projeto App
    └── DescontoServiceTests.cs            -> 6 [Theory] / 20 [InlineData]
```

---

## 🧩 Métodos criados (`DescontoService`)

| # | Método (assinatura) | Regra de negócio | Exemplos |
|---|---------------------|------------------|----------|
| 1 | `string ObterCategoriaCliente(int totalCompras)` | **"BRONZE"** para menos de 5 compras; **"PRATA"** entre 5 e 10 (inclusive); **"OURO"** para mais de 10. | `2` → `"BRONZE"` · `7` → `"PRATA"` · `15` → `"OURO"` |
| 2 | `int CalcularDescontoPorPercentual(int valorOriginal, int percentualDesconto)` | Retorna o **valor final com o desconto aplicado** (`valorOriginal - (valorOriginal * percentual) / 100`). | `(100, 10)` → `90` · `(200, 20)` → `160` · `(50, 0)` → `50` |
| 3 | `bool EValidoParaCupom(int idade, bool primeiraCompra)` | `true` se o cliente tiver **18 anos ou mais** **OU** se for a **primeira compra**; caso contrário, `false`. | `(20, false)` → `true` · `(16, true)` → `true` · `(17, false)` → `false` |

---

## ✅ Testes parametrizados (`DescontoServiceTests`)

### Exigidos pela lista — 3 `[Theory]` com 3 `[InlineData]` cada

| Teste `[Theory]` | Método testado | `[InlineData]` |
|------------------|----------------|----------------|
| `ObterCategoriaCliente_DeveRetornarCategoriaCorreta` | `ObterCategoriaCliente` (string) | `(2, "BRONZE")` · `(7, "PRATA")` · `(15, "OURO")` |
| `CalcularDescontoPorPercentual_DeveRetornarValorFinalCorreto` | `CalcularDescontoPorPercentual` (int) | `(100, 10, 90)` · `(200, 20, 160)` · `(50, 0, 50)` |
| `EValidoParaCupom_DeveValidarElegibilidadeCorretamente` | `EValidoParaCupom` (bool) | `(20, false, true)` · `(16, true, true)` · `(17, false, false)` |

### Adicionais de cobertura — valores-limite das regras (demonstram a força do `[Theory]`)

| Teste `[Theory]` | `[InlineData]` | O que garante |
|------------------|----------------|---------------|
| `ObterCategoriaCliente_ValoresLimitrofes_...` | `(0, "BRONZE")` · `(4, "BRONZE")` · `(5, "PRATA")` · `(10, "PRATA")` · `(11, "OURO")` | Fronteiras exatas entre BRONZE/PRATA/OURO (5 e 10 inclusive) |
| `CalcularDescontoPorPercentual_CenariosAdicionais_...` | `(80, 15, 68)` · `(1000, 100, 0)` · `(350, 5, 333)` | Percentual intermediário, desconto de 100% e divisão inteira |
| `EValidoParaCupom_ValoresLimitrofes_...` | `(18, false, true)` · `(18, true, true)` · `(10, true, true)` | Idade-limite (18 anos) e primeira compra infantil |

### Resultado da suíte e cobertura de código (coverlet)

```
Passed!  - Failed: 0, Passed: 20, Skipped: 0, Total: 20 - CalculadoraDescontos.Tests.dll (net10.0)
```

> São **6 métodos `[Theory]`** gerando **20 casos de teste individuais** (9 exigidos pela lista + 11 adicionais).

| Classe | Cobertura de linhas | Cobertura de branches |
|--------|--------------------|-----------------------|
| **`DescontoService`** (código de produção testado) | **100%** | **100%** |

> A cobertura é gerada pelo **coverlet.collector** (já incluído no template xUnit):
>
> ```bash
> dotnet test --collect:"XPlat Code Coverage"
> ```
>
> O relatório `coverage.cobertura.xml` é gravado em `CalculadoraDescontos.Tests/TestResults/`.

---

## 🚀 Instruções de execução

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado (verifique com `dotnet --version`).

### Clonar e testar

```bash
git clone https://github.com/Lebot007/calculadora-descontos-xunit.git
cd calculadora-descontos-xunit
dotnet test
```

Saída esperada:

```
Passed!  - Failed: 0, Passed: 20, Skipped: 0, Total: 20 - CalculadoraDescontos.Tests.dll (net10.0)
```

> 💡 Para ver **cada `[InlineData]` rodando como teste individual** (com os parâmetros usados), execute:
>
> ```bash
> dotnet test --logger "console;verbosity=normal"
> ```

### Executar a demonstração (opcional)

```bash
dotnet run --project CalculadoraDescontos.App
```

Saída esperada:

```
Categoria com 2 compras:  BRONZE
Categoria com 7 compras:  PRATA
Categoria com 15 compras: OURO
Valor final de 100 com 10% de desconto: 90
Valor final de 200 com 20% de desconto: 160
Valor final de 50 com 0% de desconto:   50
Cupom válido (20 anos, não é primeira compra): True
Cupom válido (16 anos, primeira compra):       True
Cupom válido (17 anos, não é primeira compra): False
```

---

## 🛠️ Recriando o projeto do zero (.NET CLI)

A solução foi criada inteiramente pelo terminal, com os comandos abaixo:

```bash
# 1. Cria a Solução
dotnet new sln -n CalculadoraDescontos

# 2. Cria o projeto da aplicação (Código de Produção)
dotnet new console -n CalculadoraDescontos.App -f net10.0

# 3. Cria o projeto de Testes Unitários com xUnit
dotnet new xunit -n CalculadoraDescontos.Tests -f net10.0

# 4. Adiciona ambos os projetos à Solução
dotnet sln add CalculadoraDescontos.App/CalculadoraDescontos.App.csproj
dotnet sln add CalculadoraDescontos.Tests/CalculadoraDescontos.Tests.csproj

# 5. Adiciona a referência do projeto de Produção no projeto de Testes
dotnet add CalculadoraDescontos.Tests/CalculadoraDescontos.Tests.csproj reference CalculadoraDescontos.App/CalculadoraDescontos.App.csproj
```

Em seguida, foram implementados:

- `CalculadoraDescontos.App/DescontoService.cs` — os três métodos com retornos `string`, `int` e `bool`;
- `CalculadoraDescontos.Tests/DescontoServiceTests.cs` — os testes com `[Theory]` + `[InlineData]` (o arquivo `UnitTest1.cs` gerado pelo template foi removido).

---

## 📄 Arquivos do repositório

| Arquivo | Descrição |
|---------|-----------|
| `README.md` | Explicação da diferença entre `[Fact]` e `[Theory]`, documentação dos métodos, cobertura e instruções de execução |
| `.gitignore` | Arquivo oficial do ecossistema .NET (gerado com `dotnet new gitignore`) |
| `LICENSE` | Licença pública **MIT** |

---

## 📜 Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
