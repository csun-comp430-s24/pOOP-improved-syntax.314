using Lang.Lexer;
using Lang.Parser;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void Parser_ShouldParseAssignmentAndBinOp()
    {
        string source = "x = 1 * 1;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.AssignmentStmt stmt = (Parser.AssignmentStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.left.Equals(new Parser.Identifier("x")));
        Assert.IsTrue(stmt.right is Parser.BinopExp);
    }

    [TestMethod]
    public void Parser_ShouldParseExpressonThis()
    {
        string source = "this;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.ExpressionStmt exprStmt = (Parser.ExpressionStmt)code.stmts[0];
        Parser.ThisExp thisExpr = (Parser.ThisExp)exprStmt.expression;

        Assert.IsNotNull(thisExpr, "The expression should not be null.");
        Assert.IsTrue(thisExpr is Parser.ThisExp, "The parsed expression should be an instance of ThisExp.");
    }
}