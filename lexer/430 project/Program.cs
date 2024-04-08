using Lang.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


class Program
{
    static void Main()
    {

    }
}
namespace Lang.Lexer
{
    public enum TokenType : ushort
    {
        VarToken = 1,
        Identifier = 2,
        MethodNameToken = 3,
        StringValueToken = 4,
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

        MultToken = 22,
        DivToken = 23,
        AddToken = 24,
        SubToken = 25,

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

        }
        char GetChar()
        {
            if (EOF) return (char)0;

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
    public sealed class Parser
    {
        interface Exp
        {
            public class VarExp : Exp
            {
                public string name;
                public VarExp(string name)
                {
                    this.name = name;
                }
            }
            public class IntegerExp : Exp
            {
                public int value;
                public IntegerExp(int value)
                {
                    this.value = value;
                }
            }
            public class StringExp : Exp
            {
                public string value;
                public StringExp(string value)
                {
                    this.value = value;
                }
            }
            public class TrueExp : Exp { }
            public class FalseExp : Exp { }
            public class ThisExp : Exp { }
            public class Println : Exp
            {
                public Exp exp;
                public Println(Exp exp)
                {
                    this.exp = exp;
                }
            }
            public class NewExp : Exp
            {
                public Exp left;
                public Exp center;
                public Exp right;
                public NewExp(Exp leftExp, Exp Identifier, Exp rightExp)
                {
                    this.left = leftExp;
                    this.center = Identifier;
                    this.right = rightExp;
                }
                public Boolean Equals(object other)
                {
                    if (other is NewExp)
                    {
                        NewExp e = (NewExp)other;
                        return e.left.Equals(left) && e.center.Equals(center) && e.right.Equals(right);
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            public class MethodCall : Exp
            {
                public Exp leftExp;
                public Exp Identifier;
                public Exp[] rightExp;

                public MethodCall(Exp leftExp, Exp Identifier, Exp[] rightExp)
                {
                    this.leftExp = leftExp;
                    this.Identifier = Identifier;
                    this.rightExp = rightExp;
                }
                public Boolean Equals(object other)
                {
                    if (other is MethodCall)
                    {
                        MethodCall e = (MethodCall)other;
                        return e.leftExp.Equals(leftExp) && e.Identifier.Equals(Identifier) && e.rightExp.Equals(rightExp);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public class Identifier : Exp
            {
                public string value;
                public Identifier(string value)
                {
                    this.value = value;
                }
                public Boolean Equals(object other)
                {
                    if (other is Identifier)
                    {
                        Identifier e = (Identifier)other;
                        return value.Equals(e.value);
                    }
                    else
                    {
                        return false;
                    }
                }
                public String ToString()
                {
                    return "IdentifierExp(" + value + ")";
                }
            }
            public class BinopExp : Exp
            {
                public Exp left;
                public Exp op;
                public Exp right;

                public BinopExp(Exp left, Exp op, Exp right)
                {
                    this.left = left;
                    this.op = op;
                    this.right = right;
                }
                public Boolean Equals(object other)
                {
                    if (other is BinopExp)
                    {
                        BinopExp e = (BinopExp)other;
                        return e.left.Equals(left) && e.right.Equals(right) && e.op.Equals(op);
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            public class CommaExp : Exp
            {
                public Exp left;
                public Exp right;
                public CommaExp(Exp left, Exp right)
                {
                    this.left = left;
                    this.right = right;
                }
                public Boolean Equals(object other)
                {
                    if (other is CommaExp)
                    {
                        CommaExp e = (CommaExp)other;
                        return e.left.Equals(left) && e.right.Equals(right);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        interface Op
        {
            public class PlusOp : Op
            {
                public Boolean Equals(object other)
                {
                    return other is PlusOp;
                }
                public String ToString()
                {
                    return "PlusOp()";
                }
            }
            public class MinusOp : Op
            {
                public Boolean Equals(object other)
                {
                    return other is MinusOp;
                }
                public String ToString()
                {
                    return "MinusOp()";
                }
            }
            public class MultOp : Op
            {
                public Boolean Equals(object other)
                {
                    return other is MultOp;
                }
                public String ToString()
                {
                    return "MultOp()";
                }
            }
            public class DivOp : Op
            {
                public Boolean Equals(object other)
                {
                    return other is DivOp;
                }
                public String ToString()
                {
                    return "DivOp()";
                }
            }
        }
        interface Type
    {
        public class IntType { }
        public class BooleanType { }
        public class VoidType { }
        public class CLassNameType { }
    }
        interface Vardec
    {
        public class VarType { }
    }
        interface stmt
    {
        public class Vardec { }
        public class AssignmentStmt
        {
            public stmt left;
            public stmt right;
            public AssignmentStmt(stmt left, stmt right)
            {
                this.left = left;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is AssignmentStmt)
                {
                    AssignmentStmt e = (AssignmentStmt)other;
                    return e.left.Equals(left) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }
        public class WhileStmt : stmt
        {
            public stmt left;
            public stmt right;
            public WhileStmt(stmt left, stmt right)
            {
                this.left = left;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is WhileStmt)
                {
                    WhileStmt e = (WhileStmt)other;
                    return e.left.Equals(left) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }
        public class BreakStmt : stmt{ }
        public class  ReturnStmt : stmt 
        {
            public stmt left;
            public stmt right;

            public ReturnStmt(stmt left, stmt right)
            {
                this.left = left;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is ReturnStmt)
                {
                    ReturnStmt e = (ReturnStmt)other;
                    return e.left.Equals(left) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }
        public class IfStmt : stmt
        {
            public stmt left;
            public stmt mleft;
            public stmt mid;
            public stmt mright;
            public stmt right;
            public IfStmt(stmt left, stmt mleft, stmt mid, stmt mright, stmt right)
            {
                this.left = left;
                this.mleft = mleft;
                this.mid = mid;
                this.mright = mright;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is IfStmt)
                {
                    IfStmt e = (IfStmt)other;
                    return e.left.Equals(left) && e.mleft.Equals(mleft) && e.mid.Equals(mid) && e.mright.Equals(mright) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }
        public class BlockStmt : stmt
        {
            public stmt[] Block;
            public BlockStmt(stmt[] Block)
            {
                this.Block = Block ;
            }
            public Boolean Equals(object other)
            {
                if (other is BlockStmt)
                {
                    BlockStmt e = (BlockStmt)other;
                    return e.Block.Equals(Block);
                }
                else
                {
                    return false;
                }
            }
        }
    }
        interface MethodDef
        {
            public class Method : MethodDef
            {
                public MethodDef left;
                public MethodDef mleft;
                public MethodDef mid;
                public MethodDef mright;
                public MethodDef[] right;
                public Method(MethodDef left, MethodDef mleft, MethodDef mid, MethodDef mright, MethodDef[] right)
                {
                    this.left = left;
                    this.mleft = mleft;
                    this.mid = mid;
                    this.mright = mright;
                    this.right = right;
                }
                public Boolean Equals(object other)
                {
                    if (other is Method)
                    {
                        Method e = (Method)other;
                        return e.left.Equals(left) && e.mleft.Equals(mleft) && e.mid.Equals(mid) && e.mright.Equals(mright) && e.right.Equals(right);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        interface Constructor
        {
            public class Con : Constructor
            {
                public Constructor left;
                public Constructor mleft;
                public Constructor mid;
                public Constructor mright;
                public Constructor[] right;
                public Con(Constructor left, Constructor mleft, Constructor mid, Constructor mright, Constructor[] right)
                {
                    this.left = left;
                    this.mleft = mleft;
                    this.mid = mid;
                    this.mright = mright;
                    this.right = right;
                }
                public Boolean Equals(object other)
                {
                    if (other is Con)
                    {
                        Con e = (Con)other;
                        return e.left.Equals(left) && e.mleft.Equals(mleft) && e.mid.Equals(mid) && e.mright.Equals(mright) && e.right.Equals(right);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        interface ClassDef
        {
            public class Class : ClassDef
            {
                public ClassDef left;
                public ClassDef Identifier1;
                public ClassDef Extender;
                public ClassDef Identifier2;
                public ClassDef Vardec;
                public ClassDef Constructor;
                public ClassDef[] MethodDef;
                public Class(ClassDef left, ClassDef identifier1, ClassDef extender, ClassDef identifier2, ClassDef vardec, ClassDef constructor, ClassDef[] methodDef)
                {
                    this.left = left;
                    this.Identifier1 = identifier1;
                    this.Extender = extender;
                    this.Identifier2 = identifier2;
                    this.Vardec = vardec;
                    this.Constructor = constructor;
                    this.MethodDef = methodDef;
                }
                public Boolean Equals(object other)
                {
                    if (other is Class)
                    {
                        Class e = (Class)other;
                        return e.left.Equals(left) && e.Identifier1.Equals(Identifier1) && e.Extender.Equals(Extender) && e.Identifier2.Equals(Identifier2) && e.Constructor.Equals(Constructor) && e.MethodDef.Equals(MethodDef);
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }
        interface program
        {
            public class Code : program
            {
                public program[] classdef;
                public program[] stmts;
                public Code(program[] classdef, program[] stmts)
                {
                    this.classdef = classdef;
                    this.stmts = stmts;
                }
                public Boolean Equals(object other)
                {
                    if (other is Code)
                    {
                        Code e = (Code)other;
                        return e.classdef.Equals(classdef) && e.stmts.Equals(stmts);
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }
    }



}
