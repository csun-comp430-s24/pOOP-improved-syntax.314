using Lang.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TokenizerTests
{
    //Keywords Tests
    [TestMethod]
    public void Tokenizer_ShouldIdentifyVarToken()
    {
        var source = "var";
        var tokenizer = new Tokenizer(source);
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.VarToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_ShouldIdentifyIntToken()
    {
        var source = "int";
        var tokenizer = new Tokenizer(source);
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.IntToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_ShouldIdentifyBoolToken()
    {
        var source = "bool";
        var tokenizer = new Tokenizer(source);
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.BooleanToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_ShouldIdentifyVoidToken()
    {
        var source = "void";
        var tokenizer = new Tokenizer(source);
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.VoidToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesThisKeyword()
    {
        var tokenizer = new Tokenizer("this");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.ThisToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesTrueKeyword()
    {
        var tokenizer = new Tokenizer("true");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.TrueToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesFalseKeyword()
    {
        var tokenizer = new Tokenizer("false");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.FalseToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesPrintLnKeyword()
    {
        var tokenizer = new Tokenizer("println");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.PrintlnToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesNewKeyword()
    {
        var tokenizer = new Tokenizer("new");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.NewToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesWhileKeyword()
    {
        var tokenizer = new Tokenizer("while");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.WhileToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesBreakKeyword()
    {
        var tokenizer = new Tokenizer("break");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.BreakToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesReturnKeyword()
    {
        var tokenizer = new Tokenizer("return");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.ReturnToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesIfKeyword()
    {
        var tokenizer = new Tokenizer("if");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.IfToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesElseKeyword()
    {
        var tokenizer = new Tokenizer("else");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.ElseToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesMethodKeyword()
    {
        var tokenizer = new Tokenizer("method");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.MethodToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesInitKeyword()
    {
        var tokenizer = new Tokenizer("init");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.InitToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesSuperKeyword()
    {
        var tokenizer = new Tokenizer("super");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.SuperToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    /*
     * 
     * 
     * 
     * 
     * 
     * 
     */

    //Operator Tests
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesPlusOperator()
    {
        var tokenizer = new Tokenizer("+");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.AddToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesEqualsOperator()
    {
        var tokenizer = new Tokenizer("=");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.EqualsToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesDoubleEqualsOperator()
    {
        var tokenizer = new Tokenizer("==");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.DoubleEqualsToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesSubOperator()
    {
        var tokenizer = new Tokenizer("-");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.SubToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesMultOperator()
    {
        var tokenizer = new Tokenizer("*");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.MultToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesDivOperator()
    {
        var tokenizer = new Tokenizer("/");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.DivToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesAndOperator()
    {
        var tokenizer = new Tokenizer("&&");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.AndToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesOrOperator()
    {
        var tokenizer = new Tokenizer("||");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.OrToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesSemicolonOperator()
    {
        var tokenizer = new Tokenizer(";");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.SemicolonToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesOpenParenthesisOperator()
    {
        var tokenizer = new Tokenizer("(");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.OpenParenthesisToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesNotEqualsOperator()
    {
        var tokenizer = new Tokenizer("!=");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.NotEqualsToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesOpenBracketOperator()
    {
        var tokenizer = new Tokenizer("{");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.OpenBracketToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }


    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesClosedBracketOperator()
    {
        var tokenizer = new Tokenizer("}");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.ClosedBracketToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }


    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesCloseParenthesisOperator()
    {
        var tokenizer = new Tokenizer(")");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.CloseParenthesisToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }


    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesCommaTokenOperator()
    {
        var tokenizer = new Tokenizer(",");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.CommaToken, token.Type);
        Assert.AreEqual("", token.Lexeme);
    }
    /*
     * 
     * 
     * 
     * 
     * 
     * 
     */

    [TestMethod]
    public void Tokenizer_CorrectlyIdentifiesIntegerLiteral()
    {
        var tokenizer = new Tokenizer("12345");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.IntegerLiteral, token.Type);
        Assert.AreEqual("12345", token.Lexeme);
    }
   
    [TestMethod]
    public void Tokenizer_IgnoresWhitespace()
    {
        var tokenizer = new Tokenizer(" ");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.Space, token.Type);
    }
    [TestMethod]
    public void Tokenizer_RejectsInvalidInput()
    {
        var tokenizer = new Tokenizer("@");
        var token = tokenizer.Get();

        Assert.IsNull(token);
    }
    
    [TestMethod]
    public void Tokenizer_HandlesEndOfFile()
    {
        var tokenizer = new Tokenizer("var x");
        Token token;
        while ((token = tokenizer.Get()) != null)
        {
            // process tokens
        }
        Assert.IsNull(token);
    }

    [TestMethod]
    public void Tokenizer_HandlesMultiLineInput()
    {
        var tokenizer = new Tokenizer("var x\nint y");
        var token1 = tokenizer.Get();
        var token2 = tokenizer.Get();
        // Assertions for both tokens
    }
    [TestMethod]
    public void Tokenizer_PeekDoesNotAdvance()
    {
        var tokenizer = new Tokenizer("var x");
        var peekedToken = tokenizer.Peek();
        var token = tokenizer.Get();

        Assert.AreEqual(peekedToken.Type, token.Type);
        Assert.AreEqual(peekedToken.Lexeme, token.Lexeme);
    }


}