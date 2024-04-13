using Lang.Lexer;
using Lang.Parser;
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
        string source = "{int x;return;}";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        Parser.BlockStmt block = (Parser.BlockStmt)code.stmts[0];
        Console.WriteLine(block.ToString());
    }
}
