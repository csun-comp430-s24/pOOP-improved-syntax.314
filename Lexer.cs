

public Tokenizer()
{
	_tokenDefinitions = new List();

	_tokenDefinitions.Add(new TokenDefinition(TokenType.VarToken, "^var"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.StringValueToken, "^d+"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.IntToken, "^int"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.BooleanToken, "^bool"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.VoidToken, "^void"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.ThisToken, "^this"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.TrueToken, "^true"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.FalseToken, "^false"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.PrintlnToken, "^println"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.NewToken, "^new"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.WhileToken, "^while"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.BreakToken, "^break"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.ReturnToken, "^return"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.IfToken, "^if"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.ElseToken, "^else"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.MethodToken, "^method"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.InitToken, "^init"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.SuperToken, "^super"));
    _tokenDefinitions.Add(new TokenDefinition(TokenType.AndToken, "^and"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.OrToken, "^or"));

	_tokenDefinitions.Add(new TokenDefinition(TokenType.MultToken, "^*"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.DivToken, "^/"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.AddToken, "^+"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.SubToken, "^-"));

	_tokenDefinitions.Add(new TokenDefinition(TokenType.OpenBracketToken, "^{"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.ClosedBracketToken, "^}"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.DoubleEqualsToken, "^=="));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.SemicolonToken, "^;"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.CloseParenthesisToken, "^\\)"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.CommaToken, "^,"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.EqualsToken, "^="));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.NotEqualsToken, "^!="));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.OpenParenthesisToken, "^\\("));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.StringValueToken, "^'[^']*'"));
	_tokenDefinitions.Add(new TokenDefinition(TokenType.NumberToken, "^\\d+"));
}

public class TokenDefinition
{
    private Regex _regex;
    private readonly TokenType _returnsToken;

    public TokenDefinition(TokenType returnsToken, string regexPattern)
    {
        _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
        _returnsToken = returnsToken;
    }

    public TokenMatch Match(string inputString)
    {
        var match = _regex.Match(inputString);
        if (match.Success)
        {
            string remainingText = string.Empty;
            if (match.Length != inputString.Length)
                remainingText = inputString.Substring(match.Length);

            return new TokenMatch()
            {
                IsMatch = true,
                RemainingText = remainingText,
                TokenType = _returnsToken,
                Value = match.Value
            };
        }
        else
        {
            return new TokenMatch() { IsMatch = false};
        }

    }
}

public class TokenMatch
{
    public bool IsMatch { get; set; }
    public TokenType TokenType { get; set; }
    public string Value { get; set; }
    public string RemainingText { get; set; }
}

public class DslToken
{
    public DslToken(TokenType tokenType)
    {
        TokenType = tokenType;
        Value = string.Empty;
    }

    public DslToken(TokenType tokenType, string value)
    {
        TokenType = tokenType;
        Value = value;
    }

    public TokenType TokenType { get; set; }
    public string Value { get; set; }

    public DslToken Clone()
    {
        return new DslToken(TokenType, Value);
    }
}



public List<DslToken> Tokenize(string Text)
{
    var tokens = new List<DslToken>();
    string remainingText = Text;

    while (!string.IsNullOrWhiteSpace(remainingText))
    {
        var match = FindMatch(remainingText);
        if (match.IsMatch)
        {
            tokens.Add(new DslToken(match.TokenType, match.Value));
            remainingText = match.RemainingText;
        }
        else
        {
            remainingText = remainingText.Substring(1);
        }
    }

    tokens.Add(new DslToken(TokenType.SequenceTerminator, string.Empty));

    return tokens;
}

private TokenMatch FindMatch(string Text)
{
    foreach (var tokenDefinition in _tokenDefinitions)
    {
        var match = tokenDefinition.Match(Text);
        if (match.IsMatch)
            return match;
    }

    return new TokenMatch() {  IsMatch = false };
}

int main()
{
println(Tokenize("class Animal {init() {}method speak() Void { return println(0); }}class Cat extends Animal {init() { super(); }method speak() Void { return println(1); }}class Dog extends Animal {init() { super(); }method speak() Void { return println(2); }}Animal cat;Animal dog;cat = new Cat();dog = new Dog();cat.speak();dog.speak();"));
}

/*class Animal {
  init() {}
  method speak() Void { return println(0); }
}
class Cat extends Animal {
  init() { super(); }
  method speak() Void { return println(1); }
}
class Dog extends Animal {
  init() { super(); }
  method speak() Void { return println(2); }
}

Animal cat;
Animal dog;
cat = new Cat();
dog = new Dog();
cat.speak();
dog.speak();
*/