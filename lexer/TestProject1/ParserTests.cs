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

        Parser.VarDecStmt stmt = (Parser.VarDecStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
    }

    [TestMethod]
    public void Parser_ShouldParseBlockStmt()
    {
        string source = "{bool x;}";
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
    public void Parser_ShouldParseint()
    {
        string source = "int x;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.VarDecStmt stmt = (Parser.VarDecStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.varType is Parser.IntType);
        Assert.IsTrue(stmt.varIdentifier is Parser.Identifier);
        Assert.IsTrue(stmt.varIdentifier.value.Equals("x"));
    }

    [TestMethod]
    public void Parser_ShouldParseVoidStmt()
    {
        string source = "void x;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.VarDecStmt stmt = (Parser.VarDecStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.varType is Parser.VoidType);
        Assert.IsTrue(stmt.varIdentifier is Parser.Identifier);
        Assert.IsTrue(stmt.varIdentifier.value.Equals("x"));
    }


}