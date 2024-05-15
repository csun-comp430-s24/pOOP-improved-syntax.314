using Lang.Lexer;
using Lang.Parser;
using Lang.CodeGenerator;

class Program
{
    static void Main()
    {
        string source = "int x;\nx = 0; while (true) {x = x + 1;}";
        //source = "if (true) {break;} else {return;}";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        CodeGenerator codegenerator = new CodeGenerator(code);
        var Generated = codegenerator.GenerateCode(0);

        Console.WriteLine(Generated.ToString());
    }
}
