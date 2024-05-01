using Lang.Lexer;
using Lang.Parser;

class Program
{
    static void Main()
    {
        string source = "class Test { int x; init() {x = 1;} method myFunc() int { return x; } } Test test;";
        //source = "x = 1 / 2 * 3;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Console.WriteLine(code.ToString());
    }
}
