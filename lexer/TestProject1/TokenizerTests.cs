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

}