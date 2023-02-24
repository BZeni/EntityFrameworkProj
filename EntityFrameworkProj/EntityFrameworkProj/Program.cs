using EntityFrameworkProj;
using EntityFrameworkProj.Data;

CWL("PROJETO DE ESTUDOS EM BANCO DE DADOS", ConsoleColor.DarkBlue, ConsoleColor.Gray);

MarrariDbContext context= new ();
Methods methods = new();

try
{
    methods.Init();
}
catch (Exception ex)
{
    CWL($"\n{ex.Message}", ConsoleColor.Red);
    return;
}

while (true)
{
    int menuOption = ShowMenu();

    try
    {
        switch (menuOption)
        {
            case 1: DoListaLotes(methods); break;
            case 2: DoMediaLote(methods); break;
            case 3: DoMediaProduto(methods); break;
            case 4: DoGenerateLote(methods); break;
            case 5: DoPrintLote(methods); break;
            default:
                CWL("Opção inválida.", ConsoleColor.Yellow);
                break;
        }
    }
    catch (Exception ex)
    {
        CWL($"\n{ex.Message}", ConsoleColor.Red);
    }

    CWL("\nPressione 'ESC' para sair ou outra tecla para continuar...");
    if (Console.ReadKey(true).Key == ConsoleKey.Escape) { break; }
}

CWL("\nFim.");
Console.ReadLine();

static int ShowMenu()
{
    CWL("\nMenu de opções:");
    CWL("    [1] - Lista de lotes", ConsoleColor.White);
    CWL("    [2] - Média das medidas (Lote)", ConsoleColor.White);
    CWL("    [3] - Média das medidas (Produto)", ConsoleColor.White);
    CWL("    [4] - Gerar novo lote", ConsoleColor.White);
    CWL("    [5] - Mostra lote", ConsoleColor.White);

    var keyChar = Console.ReadKey(true).KeyChar;
    return (keyChar < '1' || keyChar > '5') ? 0 : keyChar - '0';
}

static void DoListaLotes(Methods methods)
{
    CWL("\nLista de lotes".ToUpper(), ConsoleColor.Cyan);
    CW("Entre com o código do produto: ");

    var input = Console.ReadLine();

    if (int.TryParse(input, out int codProd))
    {
        var lotes = methods.GetLotes(codProd);

        if (!lotes.Any()) { CWL("\nNenhum Lote encontrado!", ConsoleColor.Red); }

        foreach (var lote in lotes)
        {
            CWL($"\nLote {lote.IdLote} = {lote.CodProd} peças");
        }
    }
    else
    {
        CWL("\nParâmetro inválido", ConsoleColor.Red);
    }
}

static void DoMediaLote(Methods methods)
{
    CWL("\nMédia das medidas (Lote)".ToUpper(), ConsoleColor.Cyan);
    CW("Entre com o número do lote: ");

    var input = Console.ReadLine();

    if (int.TryParse(input, out int num))
    {
        var medias = methods.GetMediaLote(num);

        if (medias.IdLote == 0) { CWL("\nNenhum Lote encontrado!", ConsoleColor.Red); }
        else { CWL($"\nQnt Pecas: {medias.IdLote}\nAltura: {medias.Altura}\nLargura: {medias.Largura}\nComprimento:{medias.Comprimento}"); }
    }
    else
    {
        CWL("\nParâmetro inválido", ConsoleColor.Red);
    }
}

static void DoMediaProduto(Methods methods)
{
    CWL("\nMédia das medidas (Produto)", ConsoleColor.Cyan);
    CW("Entre com o código do produto: ");

    var input = Console.ReadLine();

    if (int.TryParse(input, out int codProd))
    {
        var medias = methods.GetMediaProduto(codProd);

        if (medias.IdLote == 0) { CWL("\nNenhum Lote encontrado!", ConsoleColor.Red); }
        else { CWL($"\nQnt Pecas: {medias.IdLote}\nAltura: {medias.Altura}\nLargura: {medias.Largura}\nComprimento:{medias.Comprimento}"); }
    }
    else
    {
        CWL("\nParâmetro inválido", ConsoleColor.Red);
    }
}

static void DoGenerateLote(Methods methods)
{
    CWL("\nGerar novo lote", ConsoleColor.Cyan);

    CW("Entre com o número do lote: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out int num))
    {
        CWL("\nNúmero inválido", ConsoleColor.Red);
        return;
    }

    CW("Entre com o código do produto: ");
    input = Console.ReadLine();

    if (!int.TryParse(input, out int codProd))
    {
        CWL("\nCódigo inválido", ConsoleColor.Red);
        return;
    }

    CW("Entre com a quantidade de peças: ");
    input = Console.ReadLine();

    if (!int.TryParse(input, out int count) || count < 1 || count > 100)
    {
        CWL("\nQuantidade inválida", ConsoleColor.Red);
        return;
    }

    try
    {
        var novoLote = new Lotes { IdLote = num, CodProd = codProd, Descricao = $"Lote {num}" };
        for (int i = 0; i < count; i++)
        {
            novoLote.Pecas.Add(new Pecas(
                i + 1,
                Random.Shared.Next(10, 31),
                Random.Shared.Next(100, 301),
                Random.Shared.Next(1000, 3001)
                ));
        }
        methods.AddLote(novoLote);
        CWL("\nLote gerado com sucesso.", ConsoleColor.Green);
    }
    catch (Exception ex)
    {
        CWL($"\n{ex.Message}", ConsoleColor.Red);
    }
}

static void DoPrintLote(Methods methods)
{
    CWL("\nMostra lote".ToUpper(), ConsoleColor.Cyan);
    CW("Entre com o número do lote: ");

    var input = Console.ReadLine();

    if (int.TryParse(input, out int num))
    {
        var lote = methods.GetLote(num);

        if (lote.CodProd == 0) { CWL("\nNenhum Lote encontrado!", ConsoleColor.Red); }
        else { CWL($"\nLote {lote.IdLote} = {lote.CodProd} peças"); }
    }
    else
    {
        CWL("\nParâmetro inválido", ConsoleColor.Red);
    }
}

static void CWL(string? text = null, ConsoleColor forecolor = ConsoleColor.Gray, ConsoleColor backcolor = ConsoleColor.Black)
{
    CW1((s) => Console.WriteLine(s), text, forecolor, backcolor);
}

static void CW(string? text = null, ConsoleColor forecolor = ConsoleColor.Gray, ConsoleColor backcolor = ConsoleColor.Black)
{
    CW1((s) => Console.Write(s), text, forecolor, backcolor);
}

static void CW1(Action<string?> action, string? text = null, ConsoleColor forecolor = ConsoleColor.Gray, ConsoleColor backcolor = ConsoleColor.Black)
{
    Console.ForegroundColor = forecolor;
    Console.BackgroundColor = backcolor;
    action.Invoke(text);
    Console.ResetColor();
}