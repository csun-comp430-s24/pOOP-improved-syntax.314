using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lang.Parser;
using Lang.Lexer;
using static Lang.CodeGenerator.CodeGenerator;
using static Lang.Parser.Parser;


namespace Lang.CodeGenerator
{
    public class CodeGenerator
    {
        Lang.Parser.Parser.ParseResult<Lang.Parser.Parser.Program> ast;
        public CodeGenerator(Lang.Parser.Parser.ParseResult<Lang.Parser.Parser.Program> ast)
        {
        this.ast = ast;
        }
        public CodeGenerator GenerateCode(int startPosition)
        {
            return null;
        }
    }
}
