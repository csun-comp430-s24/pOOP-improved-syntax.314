using Lang.Lexer;
using Lang.Parser;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void Parser_ShouldDifferentiateIdentifiers()
    {
        Assert.IsFalse(new Parser.Identifier("x").Equals(new Parser.Identifier("X")));
    }

    [TestMethod]
    public void Parser_ShouldParseAssignment()
    {
        string source = "x = 1 * 1;";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.AssignmentStmt stmt = (Parser.AssignmentStmt)code.stmts[0];

        Assert.IsNotNull(stmt);
        Assert.IsTrue(stmt.Equals(new Parser.AssignmentStmt(new Parser.Identifier("x"), new Parser.BinopExp(new Parser.IntegerExp(1), new Parser.MultOp(), new Parser.IntegerExp(1)))));
    }

    [TestMethod]
    public void Parser_ShouldParseBinOp()
    {
        // Setup test source code
        string source = "1 * 2 + 3 / 4;";

        // Pass source to Lexer to get a list of tokens
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        // Pass tokens to Parser
        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);

        // Extract generated AST (containing a single statement)
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.ExpStmt stmt = (Parser.ExpStmt)code.stmts[0];

        // Check its assigning a BinopExp to Identifier x
        // (Can't write expressions on their own, intentional grammar design?)
        Assert.IsNotNull(stmt);

        // Assert the Exp used in AssignmentStmt is a BinopExp
        Assert.IsTrue(stmt.expression is Parser.BinopExp);

        // Cast Exp used in AssignmentStmt to BinopExp
        // Assert that both sides of that BinopExp are also BinopExp's
        Parser.BinopExp exp = (Parser.BinopExp)stmt.expression;
        Assert.IsTrue(exp.left is Parser.BinopExp);
        Assert.IsTrue(exp.right is Parser.BinopExp);

        // Assert the subexpressions are the expected subexpressions
        Assert.IsTrue(((Parser.BinopExp)exp.left).left.Equals(new Parser.IntegerExp(1)));
        Assert.IsTrue(((Parser.BinopExp)exp.left).op.Equals(new Parser.MultOp()));
        Assert.IsTrue(((Parser.BinopExp)exp.left).right.Equals(new Parser.IntegerExp(2)));

        Assert.IsTrue(((Parser.BinopExp)exp.right).left.Equals(new Parser.IntegerExp(3)));
        Assert.IsTrue(((Parser.BinopExp)exp.right).op.Equals(new Parser.DivOp()));
        Assert.IsTrue(((Parser.BinopExp)exp.right).right.Equals(new Parser.IntegerExp(4)));
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


        Parser.ExpStmt exprStmt = (Parser.ExpStmt)code.stmts[0];
        Parser.ThisExp thisExpr = (Parser.ThisExp)exprStmt.expression;

        Assert.IsNotNull(thisExpr, "The expression should not be null.");
        Assert.IsTrue(thisExpr is Parser.ThisExp, "The parsed expression should be an instance of ThisExp.");
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
        Assert.IsTrue(stmt.Equals(new Parser.VarDecStmt(new Parser.BooleanType(), new Parser.Identifier("x"))));
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

    [TestMethod]
    public void Parser_ShouldParseMultiMethodCall()
    {
        string source = "myClass.myFunc(param1, 1 + 2 / 3, param2, false).otherFunc().field;";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.ExpStmt stmt = (Parser.ExpStmt)code.stmts[0];

        Assert.IsNotNull(stmt);

        // Build up expected Parse Result
        Parser.Exp[] fullParamList = new Parser.Exp[4];
        fullParamList[0] = new Parser.Identifier("param1");
        fullParamList[1] = new Parser.BinopExp(new Parser.IntegerExp(1), new Parser.PlusOp(), new Parser.BinopExp(new Parser.IntegerExp(2), new Parser.DivOp(), new Parser.IntegerExp(3)));
        fullParamList[2] = new Parser.Identifier("param2");
        fullParamList[3] = new Parser.FalseExp();

        Parser.MethodCall paramMethodCall = new Parser.MethodCall(new Parser.Identifier("myFunc"), fullParamList);
        Parser.MethodCall emptyMethodCall = new Parser.MethodCall(new Parser.Identifier("otherFunc"), []);

        Parser.BinopExp finalExp = new Parser.BinopExp(new Parser.BinopExp(new Parser.BinopExp(new Parser.Identifier("myClass"), new Parser.PeriodOp(), paramMethodCall), new Parser.PeriodOp(), emptyMethodCall), new Parser.PeriodOp(), new Parser.Identifier("field"));

        Parser.ExpStmt expectedStmt = new Parser.ExpStmt(finalExp);
        Assert.IsTrue(stmt.Equals(expectedStmt));
    }

    [TestMethod]
    public void Parser_ShouldParseWhileStmt()
    {
        string source = "while (true) {x = x + 1;}";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.WhileStmt stmt = (Parser.WhileStmt)code.stmts[0];

        Assert.IsNotNull(stmt);

        // Build up Expected Statement
        Parser.BinopExp binop = new Parser.BinopExp(new Parser.Identifier("x"), new Parser.PlusOp(), new Parser.IntegerExp(1));
        Parser.Stmt[] whilebody = [new Parser.AssignmentStmt(new Parser.Identifier("x"), binop)];
        Parser.BlockStmt whileblock = new Parser.BlockStmt(whilebody);
        Parser.WhileStmt expectedStmt = new Parser.WhileStmt(new Parser.TrueExp(), whileblock);

        string expectedParseResult = expectedStmt.ToString();
        Assert.IsTrue(stmt.ToString().Equals(expectedParseResult));
        Assert.IsTrue(stmt.Equals(expectedStmt));
    }

    [TestMethod]
    public void Parser_ShouldParseIfStmt()
    {
        string source = "if (true) {break;}";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.IfStmt stmt = (Parser.IfStmt)code.stmts[0];
        Assert.IsNotNull(stmt);

        // Build up expected result
        Parser.BlockStmt ifBlock = new Parser.BlockStmt([new Parser.BreakStmt()]);
        Parser.IfStmt expectedStatement = new Parser.IfStmt(new Parser.TrueExp(), ifBlock);

        string expectedParseResult = expectedStatement.ToString();
        Assert.IsTrue(stmt.ToString().Equals(expectedParseResult));
        Assert.IsTrue(stmt.Equals(expectedStatement));
    }

    [TestMethod]
    public void Parser_ShouldParseIfElseStmt()
    {
        string source = "if (true) {break;} else {return;}";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;
        Parser.IfStmt stmt = (Parser.IfStmt)code.stmts[0];
        Assert.IsNotNull(stmt);

        // Build up expected result
        Parser.BlockStmt ifBlock = new Parser.BlockStmt([new Parser.BreakStmt()]);
        Parser.BlockStmt elseBlock = new Parser.BlockStmt([new Parser.ReturnStmt(null)]);
        Parser.IfStmt expectedStatement = new Parser.IfStmt(new Parser.TrueExp(), ifBlock, elseBlock);

        string expectedParseResult = expectedStatement.ToString();
        Assert.IsTrue(stmt.ToString().Equals(expectedParseResult));
        Assert.IsTrue(stmt.Equals(expectedStatement));
    }

    [TestMethod]
    [ExpectedException(typeof(ParseException))]
    public void Parser_DontParseStmtWithoutSemicolon()
    {
        string source = "bool x";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ParseException))]
    public void Parser_DontParseInvalidClassDef()
    {
        string source = "class int { int value; init(int setValue) {value = setValue;} }";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ParseException))]
    public void Parser_DontParseInvalidMethodType()
    {
        string source = "class Test { init() {} method myFunc() break {return 0;} }";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ParseException))]
    public void Parser_DontParseMissingParenMethodDef()
    {
        string source = "class Test { init() {} method myFunc( int {return 0;} }";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
    }

    [TestMethod]
    public void Parser_ShouldParseExampleProgram()
    {
        string source = "class Animal {\n  init() {}\n  method speak() Void { return println(0); }\n}\nclass Cat extends Animal {\n  init() { super(); }\n  method speak() Void { return println(1); }\n}\nclass Dog extends Animal {\n  init() { super(); }\n  method speak() Void { return println(2); }\n}\n\nAnimal cat;\nAnimal dog;\ncat = new Cat();\ndog = new Dog();\ncat.speak();\ndog.speak();";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        // Comparing ToString() as the output is very long
        string expectedParseResult = "ClassDef(\nIdentifierExp(Animal) : \nConstructor(\n[]\nNo Super, \nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(0)))\n))\n)\nClassDef(\nIdentifierExp(Cat) : IdentifierExp(Animal)\nConstructor(\n[]\nCalls Super, []\nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(1)))\n))\n)\nClassDef(\nIdentifierExp(Dog) : IdentifierExp(Animal)\nConstructor(\n[]\nCalls Super, []\nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(2)))\n))\n)\nVarDec(ClassType(Animal), IdentifierExp(cat))\nVarDec(ClassType(Animal), IdentifierExp(dog))\nAssignmentStmt(IdentifierExp(cat), NewExp(IdentifierExp(Cat)))\nAssignmentStmt(IdentifierExp(dog), NewExp(IdentifierExp(Dog)))\nExpStmt(BinOpExp(IdentifierExp(cat) PeriodOp() MethodCall(IdentifierExp(speak), [])))\nExpStmt(BinOpExp(IdentifierExp(dog) PeriodOp() MethodCall(IdentifierExp(speak), [])))\n";
        Assert.IsTrue(code.ToString().Equals(expectedParseResult));
    }

}