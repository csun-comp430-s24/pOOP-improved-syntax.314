using Lang.Lexer;
using Lang.Parser;
using Lang.CodeGenerator;

class Program
{
    static void Main()
    {
        string source = "class Animal {  init() {}  method speak() Void { return println(0); }  }  class Cat extends Animal {    init() { super(); }    method speak() Void { return println(1); }  }  class Dog extends Animal {    init() { super(); }    method speak() Void { return println(2); }  }    Animal cat;  Animal dog;  cat = new Cat();  dog = new Dog();  cat.speak();  dog.speak();  ";
        //source = "if (true) {break;} else {return;}";
        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        CodeGenerator codegenerator = new CodeGenerator(code);
        var Generated = codegenerator.GenerateCode(0);

        Console.WriteLine(Generated.ToString());
    }
}
