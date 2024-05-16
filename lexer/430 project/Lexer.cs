using System.Text.RegularExpressions;

namespace Lang.Lexer
{
    public enum TokenType : ushort
    {
        VarToken = 1,
        Identifier = 2,
        MethodNameToken = 3,
        StringLiteral = 4,
        IntegerLiteral = 5,
        IntToken = 6,
        BooleanToken = 7,
        VoidToken = 8,
        ThisToken = 9,
        TrueToken = 10,
        FalseToken = 11,
        PrintlnToken = 12,
        NewToken = 13,
        WhileToken = 14,
        BreakToken = 15,
        ReturnToken = 16,
        IfToken = 17,
        ElseToken = 18,
        MethodToken = 19,
        InitToken = 20,
        SuperToken = 21,
        ClassToken = 39,
        ExtendsToken = 41,

        MultToken = 22,
        DivToken = 23,
        AddToken = 24,
        SubToken = 25,

        PeriodToken = 40,
        OpenBracketToken = 26,
        ClosedBracketToken = 27,
        AndToken = 28,
        CloseParenthesisToken = 29,
        CommaToken = 30,
        DoubleEqualsToken = 31,
        EqualsToken = 32,
        NotEqualsToken = 33,
        OrToken = 34,
        OpenParenthesisToken = 35,
        SemicolonToken = 36,

        Space = 37,

        Assignment = 38,
    }
    public sealed class Token
    {
        public TokenType Type { get; private set; }
        public string Lexeme { get; private set; }
        public int CurrentLine { get; private set; }
        public Token(TokenType type, string lexeme, int currentLine)
        {
            Type = type;
            Lexeme = lexeme;
            CurrentLine = currentLine;
        }
    }
    public sealed class Tokenizer
    {

        readonly List<string> Lines;
        int Line = 0;
        int Position = 0;
        bool EOF = false;

        int lexemeLength = 0;

        public Tokenizer(string source)
        {
            Lines = new List<string>(Regex.Split(source, Environment.NewLine));
            foreach(string line in Lines)
            {
                Console.WriteLine(line);
            }
        }
        char GetChar()
        {
            if (EOF) return (char)0;

            if (Lines[Line].Length == 0)
            {
                Line++;
            }

            char c = Lines[Line][Position];

            if (Position + 1 < Lines[Line].Length)
            {
                Position++;
            }
            else
            {
                if (Line + 1 < Lines.Count)
                {
                    Line++;
                    Position = 0;
                }
                else
                {
                    EOF = true;
                    Position++;
                }
            }

            return c;
        }

        void UngetString(int count)
        {
            for (int i = 0; i < count; i++)
            {
                UngetChar();
            }
        }
        void UngetChar()
        {
            if (Position != 0)
            {
                if (!EOF)
                {
                    Position--;
                }
                else
                {
                    Position--;
                    EOF = false;
                }
            }
            else
            {
                Line--;
                Position = Lines[Line].Length - 1;
            }
        }

        char PeekChar()
        {
            char c = GetChar();
            if (c != (char)0) UngetChar();
            return c;
        }
        public void Unget()
        {
            UngetString(lexemeLength);
        }
        public Token Peek()
        {
            Token token = Get();
            Unget();
            return token;
        }

        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();

            while (!EOF)
            {
                Token newToken = Get();

                if (newToken != null && newToken.Type != TokenType.Space)
                {
                    tokens.Add(newToken);
                }
            }

            return tokens;
        }

        public Token Get()
        {
            if (EOF) return null;

            TokenType type;
            string lexeme = string.Empty;

            if ((type = IsSpace()) != 0)
            {
                return new Token(type, lexeme, Line);
            }
            if ((type = IsOperator()) != 0)
            {
                return new Token(type, lexeme, Line);
            }
            if ((type = IsKeyword()) != 0)
            {
                return new Token(type, lexeme, Line);
            }
            Tuple<TokenType, String> identifier = IsIdentifier();
            if (identifier.Item1 != 0)
            {
                return new Token(TokenType.Identifier, identifier.Item2, Line);
            }
            Tuple<TokenType, String> integerLiteral = IsIntegerLiteral();
            if (integerLiteral.Item1 != 0)
            {
                return new Token(TokenType.IntegerLiteral, integerLiteral.Item2, Line);
            }



            //bad token
            return null;

        }

        Tuple<TokenType, String> IsIntegerLiteral()
        {
            if (!char.IsDigit(PeekChar()))
                return new Tuple<TokenType, string>(0, string.Empty);
            string lexeme = GetChar().ToString();
            int count = 1;
            int line = Line;
            while (char.IsDigit(PeekChar()))
            {
                lexeme = lexeme + GetChar();
                count++;
                if (line != Line)
                {
                    UngetString(count);
                    return new Tuple<TokenType, string>(0, string.Empty);
                }
            }

            lexemeLength = count;
            return new Tuple<TokenType, string>(TokenType.Identifier, lexeme);
        }

        
        TokenType IsKeyword()
        {
            if (!char.IsLetter(PeekChar())) return 0;
            string lexeme = GetChar().ToString();
            int count = 1;
            int line = Line;
            while (char.IsLetter(PeekChar()))
            {
                lexeme = lexeme + GetChar();
                count++;
                if (line != Line) break;
            }

            switch (lexeme.ToLower())
            {
                case "var":
                    {
                        lexemeLength = count;
                        return TokenType.VarToken;

                    }
                case "int":
                    {
                        lexemeLength = count;
                        return TokenType.IntToken;
                    }
                case "bool":
                    {
                        lexemeLength = count;
                        return TokenType.BooleanToken;
                    }
                case "void":
                    {
                        lexemeLength = count;
                        return TokenType.VoidToken;
                    }
                case "this":
                    {
                        lexemeLength = count;
                        return TokenType.ThisToken;
                    }

                case "super":
                    {
                        lexemeLength = count;
                        return TokenType.SuperToken;
                    }

                case "init":
                    {
                        lexemeLength = count;
                        return TokenType.InitToken;
                    }

                case "method":
                    {
                        lexemeLength = count;
                        return TokenType.MethodToken;
                    }

                case "else":
                    {
                        lexemeLength = count;
                        return TokenType.ElseToken;
                    }

                case "if":
                    {
                        lexemeLength = count;
                        return TokenType.IfToken;
                    }

                case "return":
                    {
                        lexemeLength = count;
                        return TokenType.ReturnToken;
                    }

                case "break":
                    {
                        lexemeLength = count;
                        return TokenType.BreakToken;
                    }

                case "new":
                    {
                        lexemeLength = count;
                        return TokenType.NewToken;
                    }

                case "println":
                    {
                        lexemeLength = count;
                        return TokenType.PrintlnToken;
                    }

                case "false":
                    {
                        lexemeLength = count;
                        return TokenType.FalseToken;
                    }

                case "true":
                    {
                        lexemeLength = count;
                        return TokenType.TrueToken;
                    }
                case "while":
                    {
                        lexemeLength = count;
                        return TokenType.WhileToken;
                    }
                case "class":
                    {
                        lexemeLength = count;
                        return TokenType.ClassToken;
                    }
                case "extends":
                    {
                        lexemeLength = count;
                        return TokenType.ExtendsToken;
                    }
            }


            UngetString(count);

            return 0;
        }

        Tuple<TokenType, String> IsIdentifier()
        {
            if (!(char.IsLetter(PeekChar()) || PeekChar() == '_'))
                return new Tuple<TokenType, string>(0, string.Empty);
            string lexeme = GetChar().ToString();
            int count = 1;
            int line = Line;
            while ((char.IsLetter(PeekChar()) || char.IsDigit(PeekChar()) || PeekChar() == '_'))
            {
                lexeme = lexeme + GetChar();
                count++;
                if (line != Line)
                {
                    UngetString(count);
                    return new Tuple<TokenType, string>(0, string.Empty);
                }
            }

            lexemeLength = count;
            return new Tuple<TokenType, string>(TokenType.Identifier, lexeme);
        }

        TokenType IsSpace()
        {
            if (char.IsWhiteSpace(PeekChar()))
            {
                GetChar();
                lexemeLength = 1;
                return TokenType.Space;
            }
            return 0;

        }


        TokenType IsOperator()
        {
            char c = PeekChar();

            switch (c)
            {
                case '=':
                    {
                        GetChar();
                        if (PeekChar() == '=')
                        {
                            GetChar();
                            lexemeLength = 2;
                            return TokenType.DoubleEqualsToken;
                        }
                        else return TokenType.EqualsToken;
                        lexemeLength = 1;
                        return TokenType.Assignment;
                    }

                case '+':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.AddToken;
                    }
                case '-':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.SubToken;
                    }

                case '*':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.MultToken;
                    }

                case '/':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.DivToken;
                    }
                case '&':
                    {
                        GetChar();
                        if (PeekChar() == '&')
                        {
                            GetChar();
                            lexemeLength = 2;
                            return TokenType.AndToken;
                        }
                        lexemeLength = 1;
                        return 0;
                    }
                case '|':
                    {
                        GetChar();
                        if (PeekChar() == '|')
                        {
                            GetChar();
                            lexemeLength = 2;
                            return TokenType.OrToken;
                        }
                        lexemeLength = 1;
                        return 0;
                    }
                case ';':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.SemicolonToken;
                    }

                case '(':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.OpenParenthesisToken;
                    }

                case '!':
                    {
                        GetChar();
                        if (PeekChar() == '=')
                        {
                            GetChar();
                            lexemeLength = 2;
                            return TokenType.NotEqualsToken;
                        }
                        lexemeLength = 1;
                        return 0;
                    }

                case ',':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.CommaToken;
                    }
                case '.':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.PeriodToken;
                    }

                case ')':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.CloseParenthesisToken;
                    }

                case '}':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.ClosedBracketToken;
                    }

                case '{':
                    {
                        GetChar();
                        lexemeLength = 1;
                        return TokenType.OpenBracketToken;
                    }

                default:
                    {
                        return 0;
                    }
            }

        }

    }
}