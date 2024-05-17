using Lang.CodeGenerator;
using Lang.Parser;
using Lang.Lexer;

[TestClass]
public class CodeGeneratorTests
    {
    [TestMethod]
    public void Parser_ShouldGenerateExampleProgram()
    {
        string source = "class Animal {\n  init() {}\n  method speak() Void { return println(0); }\n}\nclass Cat extends Animal {\n  init() { super(); }\n  method speak() Void { return println(1); }\n}\nclass Dog extends Animal {\n  init() { super(); }\n  method speak() Void { return println(2); }\n}\n\nAnimal cat;\nAnimal dog;\ncat = new Cat();\ndog = new Dog();\ncat.speak();\ndog.speak();";

        Tokenizer tokenizer = new Tokenizer(source);
        List<Token> tokens = tokenizer.GetAllTokens();

        Parser parser = new Parser(tokens);
        var ast = parser.ParseProgram(0);
        Parser.Code code = (Parser.Code)ast.parseResult;

        CodeGenerator codegenerator = new CodeGenerator(code);
        var Generated = codegenerator.GenerateCode();

        // Comparing ToString() as the output is very long
        string expectedGeneratedResult = "ClassDef(\nIdentifierExp(Animal) : \nConstructor(\n[]\nNo Super, \nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(0)))\n))\n)\nClassDef(\nIdentifierExp(Cat) : IdentifierExp(Animal)\nConstructor(\n[]\nCalls Super, []\nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(1)))\n))\n)\nClassDef(\nIdentifierExp(Dog) : IdentifierExp(Animal)\nConstructor(\n[]\nCalls Super, []\nBlockStmt(\n))\nMethodDef(\nIdentifierExp(speak), VoidType(), []\nBlockStmt(\n\tReturnStmt(PrintLn(IntegerExp(2)))\n))\n)\nVarDec(ClassType(Animal), IdentifierExp(cat))\nVarDec(ClassType(Animal), IdentifierExp(dog))\nAssignmentStmt(IdentifierExp(cat), NewExp(IdentifierExp(Cat)))\nAssignmentStmt(IdentifierExp(dog), NewExp(IdentifierExp(Dog)))\nExpStmt(BinOpExp(IdentifierExp(cat) PeriodOp() MethodCall(IdentifierExp(speak), [])))\nExpStmt(BinOpExp(IdentifierExp(dog) PeriodOp() MethodCall(IdentifierExp(speak), [])))\n";
        Assert.IsTrue(code.ToString().Equals(expectedGeneratedResult));
    }
}

