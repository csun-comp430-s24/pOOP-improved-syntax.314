using Lang.Lexer;
using Lang.Parser;
using Lang.CodeGenerator;

class Program
{
    static void Main()
    {
        string source = "class Test { int x; init() {x = 1;} method myFunc() int { return this.x; } } Test test;";
        //source = "if (true) {break;} else {return;}";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        CodeGenerator2 codegenerator = new CodeGenerator2();
        var Generated = codegenerator.GenerateCode(code);

        Console.WriteLine(tokens.ToString());
        Console.WriteLine("------------------------");
        Console.WriteLine(code.ToString());
        Console.WriteLine("------------------------");
        Console.WriteLine(Generated.ToString());
    }
}
