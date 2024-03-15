using Lang.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TokenizerTests
{
    //KEY WORDS
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
    public void Tokenizer_CorrectlyIdentifiesIntegerLiteral()
    {
        var tokenizer = new Tokenizer("12345");
        var token = tokenizer.Get();

        Assert.IsNotNull(token);
        Assert.AreEqual(TokenType.IntegerLiteral, token.Type);
        Assert.AreEqual("12345", token.Lexeme);
    }
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