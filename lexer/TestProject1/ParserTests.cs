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
        string source = "x = this;";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;


        Parser.AssignmentStmt exprStmt = (Parser.AssignmentStmt)code.stmts[0];
        Parser.ThisExp thisExpr = (Parser.ThisExp)exprStmt.right;

        Assert.IsNotNull(thisExpr, "The expression should not be null.");
        Assert.IsTrue(thisExpr is Parser.ThisExp, "The parsed expression should be an instance of ThisExp.");

    }

    public void Parser_ShouldParseVarDecStmt()
    {
        string source = "bool x;";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.VarDecStmt stmt = (Parser.VarDecStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.varType is Parser.BooleanType);
        Assert.IsTrue(stmt.varIdentifier is Parser.Identifier);
        Assert.IsTrue(stmt.varIdentifier.value.Equals("x"));
    }

    [TestMethod]
    public void Parser_ShouldParseBreakStmt()
    {
        string source = "break;";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.BreakStmt stmt = (Parser.BreakStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt is Parser.BreakStmt);
    }

    [TestMethod]
    public void Parser_ShouldParseReturnStmt()
    {
        string source = "return x;";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.ReturnStmt stmt = (Parser.ReturnStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.left is Parser.Identifier);

        Parser.Identifier returnIdentifier = (Parser.Identifier)stmt.left;
        Assert.IsTrue(returnIdentifier.Equals(new Parser.Identifier("x")));
    }

    [TestMethod]
    public void Parser_ShouldParseBlockStmt()
    {
        string source = "{int x;return;}";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.BlockStmt block = (Parser.BlockStmt)code.stmts[0];
        Parser.Stmt[] blockStmts = block.Block;

        Assert.IsNotNull(blockStmts);
        Assert.IsTrue(blockStmts[0] is Parser.VarDecStmt);
        Assert.IsTrue(blockStmts[1] is Parser.ReturnStmt);
    }
}