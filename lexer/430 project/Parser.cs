using Lang.Lexer;

namespace Lang.Parser
{
    public sealed class Parser
    {
        List<Token> tokens;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public class ParseResult<A>
        {
            public A parseResult;
            public int nextPosition;

            public ParseResult(A parseResult, int nextPosition)
            {
                this.parseResult = parseResult;
                this.nextPosition = nextPosition;
            }
        }

        public interface Op { }

        public interface Exp { }

        public interface Type { }

        public interface Vardec { }
        public interface Stmt { }
        public interface MethodDef { }
        public interface Constructor { }
        public interface ClassDef { }
        public interface Program { }

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

        public class IntType : Type { }
        public class BooleanType : Type { }
        public class StringType : Type { }
        public class VoidType : Type { }
        public class CLassNameType : Type { }

        public class VarType : Vardec { }

        public class VarDecStmt : Stmt
        {
            public TokenType varType;
            public string varIdentifier;

            public VarDecStmt(TokenType varType, string varIdentifier)
            {
                this.varType = varType;
                this.varIdentifier = varIdentifier;
            }
        }
        public class AssignmentStmt : Stmt
        {
            public Stmt left;
            public Stmt right;
            public AssignmentStmt(Stmt left, Stmt right)
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
        public class WhileStmt : Stmt
        {
            public Stmt left;
            public Stmt right;
            public WhileStmt(Stmt left, Stmt right)
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
        public class BreakStmt : Stmt { }
        public class ReturnStmt : Stmt
        {
            public Stmt left;
            public Stmt right;

            public ReturnStmt(Stmt left, Stmt right)
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
        public class IfStmt : Stmt
        {
            public Stmt left;
            public Stmt mleft;
            public Stmt mid;
            public Stmt mright;
            public Stmt right;
            public IfStmt(Stmt left, Stmt mleft, Stmt mid, Stmt mright, Stmt right)
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
        public class BlockStmt : Stmt
        {
            public Stmt[] Block;
            public BlockStmt(Stmt[] Block)
            {
                this.Block = Block;
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

        public class Code : Program
        {
            public Program[] classdef;
            public Program[] stmts;
            public Code(Program[] classdef, Program[] stmts)
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

        public void ParseTokens()
        {
            int position = 0;

            while (position < tokens.Count)
            {
                break;
            }
        }

        public ParseResult<Op> ParseOp(int startPosition)
        {
            Token token = tokens[startPosition];

            switch (token.Type)
            {
                case TokenType.AddToken:
                    return new ParseResult<Op>(new PlusOp(), startPosition + 1);
                case TokenType.SubToken:
                    return new ParseResult<Op>(new MinusOp(), startPosition + 1);
                case TokenType.MultToken:
                    return new ParseResult<Op>(new MultOp(), startPosition + 1);
                case TokenType.DivToken:
                    return new ParseResult<Op>(new DivOp(), startPosition + 1);
                default:
                    throw new ParseException("Failed to Parse Operator.");
            }
        }

        public ParseResult<VarDecStmt> ParseVarDecStmt(int startPosition)
        {
            Token token = tokens[startPosition];
            switch (token.Type)
            {
                case TokenType.IntToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 1].Type == TokenType.SemicolonToken)
                            return new ParseResult<VarDecStmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    else
                        throw new ParseException("Identifier expected");
                case TokenType.StringToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 1].Type == TokenType.SemicolonToken)
                            return new ParseResult<VarDecStmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    throw new ParseException("Identifier expected");

                case TokenType.BooleanToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 1].Type == TokenType.SemicolonToken)
                            return new ParseResult<VarDecStmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    throw new ParseException("Identifier expected");

                case TokenType.VoidToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 1].Type == TokenType.SemicolonToken)
                            return new ParseResult<VarDecStmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    throw new ParseException("Identifier expected");
                default:
                    throw new ParseException("Defaulted in ParseVarDecStmt");
                    
            }
        }
        public ParseResult<Stmt>ParseStmt(int startPosition)
        {
            switch (Token.Type)
            {

            }
        }
    }

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
            Console.WriteLine("Failed to Parse.");
        }
    }
}