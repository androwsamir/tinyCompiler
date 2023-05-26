using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    internal class Tiny_Compiler
    {
        Tiny_Scanner scanner = new Tiny_Scanner();
        Tiny_Parser parser = new Tiny_Parser();
        List<Token> tokens_compile = new List<Token>();
        public static Node treeroot;

        public List<Token> startCompiling(string sourceCode)
        {
            //Scanner
            tokens_compile = scanner.startScanning(sourceCode);

            //Parser
            parser.StartParsing(tokens_compile);
            treeroot = parser.root;
            
            
            //foreach(var t in tokens_compile)
            //{
            //    Console.WriteLine(t.lex);
            //    Console.WriteLine(t.tokenClass);
            //}
            
            return tokens_compile;
        }
    }
}