using Lang.Lexer;
using Lang.Parser;

class Program
{
    static void Main()
    {
        string source = "myClass.localFunc(param1, 1 + 2 * 3, param2, false).field;";
        //source = "x = 1 / 2 * 3;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.ExpStmt block = (Parser.ExpStmt)code.stmts[0];
        Console.WriteLine(block.ToString());
    }
}
