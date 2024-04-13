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
        Tokenizer tokenizer = new Tokenizer("int x = 7;");
        List<Token> allTokens = tokenizer.GetAllTokens();
        foreach (Token token in allTokens)
        {
            Console.WriteLine($"{token.Type}" + ((token.Lexeme != "") ? $" {token.Lexeme}" : ""));
        }
        Parser parser = new Parser(allTokens);


    }
}
