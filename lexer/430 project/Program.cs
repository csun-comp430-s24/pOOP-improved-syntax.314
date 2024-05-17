using Lang.Lexer;
using Lang.Parser;
using Lang.CodeGenerator;

class Program
{
    static void Main()
    {
        Console.Write("Enter Path to File to Compile: ");
        var dirPath = Console.ReadLine();

        if (dirPath == null)
        {
            Console.WriteLine("No file path specified. Exiting...\n");
            return;
        }

        if (dirPath.IndexOf(".pim") != dirPath.Length - 4)
        {
            Console.WriteLine("File is not a pOOP Improved Sytax file (.pim)\n");
            Console.WriteLine("Exiting...\n");
            return;
        }

        string source = File.ReadAllText(dirPath);

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        CodeGenerator codegenerator = new CodeGenerator(code);
        var Generated = codegenerator.GenerateCode();

        Console.WriteLine(Generated.ToString());
    }
}
