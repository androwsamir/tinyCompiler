using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public enum Token_Class
{
    Main, Integer, If, Elseif, Else, Endl, Then, Repeat, Until, Write, Read, End, Return,
    Float, String, Identifier, Coma, Semicolon, Dot, LBracket, RBracket, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, AndOp, OrOp,
    LCurlyBracket, RCurlyBracket, AssignmentOp, Constant, StringValue, Error

}

namespace TinyCompiler
{
    public class Token
    {
        public string lex;
        public Token_Class tokenClass;
    }
    internal class Tiny_Scanner
    {
        List<Token> tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        List<char> notError = new List<char>();

        public Tiny_Scanner()
        {
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);

            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add(",", Token_Class.Coma);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(".", Token_Class.Dot);
            Operators.Add("(", Token_Class.LBracket);
            Operators.Add(")", Token_Class.RBracket);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);

            notError.Add('+');
            notError.Add('-');
            notError.Add('*');
            notError.Add('/');
            notError.Add('<');
            notError.Add('>');
            notError.Add('=');
            notError.Add(',');
            notError.Add(';');
            notError.Add('(');
            notError.Add(')');
            notError.Add('{');
            notError.Add('}');
        }

        public List<Token> startScanning(string sourceCode)
        {
            Token token = new Token();
            // static list should clear every run
            Error.Errors.Clear();

            for (int i = 0; i < sourceCode.Length; i++)
            {
                int j = i;
                token = new Token();
                string CurrentLexeme = sourceCode[i].ToString();
                Console.WriteLine(CurrentLexeme);

                if (sourceCode[i] == ' ' || sourceCode[i] == '\r' || sourceCode[i] == '\n')
                {
                    continue;
                }

                // ReservedWord, Identifier
                if (Char.IsLetter(sourceCode[i]))
                {
                    j++;
                    while ((j < sourceCode.Length) && (Char.IsLetterOrDigit(sourceCode[j])))
                    {
                        CurrentLexeme += sourceCode[j].ToString();
                        j++;
                    }
                    i = j - 1;
                }

                // Constant
                else if (Char.IsDigit(sourceCode[i]))
                {
                    j++;
                    while ((j < sourceCode.Length) && ((Char.IsLetterOrDigit(sourceCode[j])) || sourceCode[j].Equals('.')))
                    {
                        CurrentLexeme += sourceCode[j];
                        j++;
                        i++;
                    }

                }

                // Assignment (:=), Or (||), And (&&), NotEqual (<>)
                else if ((j < sourceCode.Length) && ((j + 1 < sourceCode.Length)) && ((sourceCode[j].Equals('<') && sourceCode[j + 1].Equals('>')) || (sourceCode[j].Equals('|') && sourceCode[j + 1].Equals('|')) || (sourceCode[j].Equals(':') && sourceCode[j + 1].Equals('=')) || (sourceCode[j].Equals('&') && sourceCode[j + 1].Equals('&'))))
                {
                    CurrentLexeme += (sourceCode[j + 1]);
                    i++;
                }

                // StringValue ("")
                else if ((j < sourceCode.Length) && (sourceCode[j].Equals('"')))
                {
                    bool check = false;
                    j++;
                    while ((j < sourceCode.Length))
                    {
                        if (sourceCode[j].Equals('"'))
                        {
                            CurrentLexeme += sourceCode[j];
                            //check = true;
                            break;
                        }
                        CurrentLexeme += sourceCode[j];
                        j++;
                    }
                    i = j;
                }

                // Comment (/**/)
                else if (((j < sourceCode.Length) && (j + 1 < sourceCode.Length)) && (sourceCode[j].Equals('/') && sourceCode[j + 1].Equals('*')))
                {
                    CurrentLexeme = "";
                    j += 2;
                    while (((j < sourceCode.Length) && (j + 1 < sourceCode.Length)) && !(sourceCode[j].Equals('*') && sourceCode[j + 1].Equals('/')))
                    {
                        j++;
                    }
                    j++;
                    i = j;
                }

                // Otherwise
                else
                {
                    bool noterror = false;
                    foreach (var x in notError)
                    {
                        if (sourceCode[i].Equals(x))
                        {
                            noterror = true;
                            break;
                        }
                    }
                    if (!noterror)
                    {
                        j++;
                        while ((j < sourceCode.Length) && (sourceCode[j] != ' ' && sourceCode[j] != '\r' && sourceCode[j] != '\n'))
                        {
                            CurrentLexeme+=sourceCode[j];
                            j++;
                        }
                        i = j-1;
                    }
                    else
                    {
                        j++;
                        if((j < sourceCode.Length) && ((sourceCode[i].Equals('<') || sourceCode[i].Equals('>')) && (sourceCode[j].Equals('='))))
                        {
                            CurrentLexeme += sourceCode[j];
                            i++;
                        }
                    }
                }

                token.lex = CurrentLexeme;
                token.tokenClass = FindTokenClass(token.lex);
                if (token.tokenClass != Token_Class.Error)
                {
                    tokens.Add(token);
                }
            }
            return tokens;
        }

        Token_Class FindTokenClass(string lex)
        {
            Token_Class TC = Token_Class.Error;

            //Is it reserved word
            foreach (var i in ReservedWords)
            {
                if (i.Key.Equals(lex))
                {
                    TC = i.Value;
                    return TC;
                }
            }

            //Is it Identifier  
            var IDregex = new Regex("^[a-zA-Z]([a-zA-Z0-9])*", RegexOptions.Compiled);
            if (IDregex.IsMatch(lex))
            {
                TC = Token_Class.Identifier;
                return TC;
            }

            //Is it Constant
            else if (isConstant(lex))
            {
                TC = Token_Class.Constant;
                return TC;
            }

            //Is it Operator 
            foreach (var i in Operators)
            {
                if (i.Key.Equals(lex))
                {
                    TC = i.Value;
                    return TC;
                }
            }

            //Is it stringValue
            var stringRegex = new Regex("\"(.*)\"", RegexOptions.Compiled);
            if (stringRegex.IsMatch(lex))
            {
                return Token_Class.StringValue;
            }

            Error.Errors.Add(lex);
            
            return TC;
        }

        bool isConstant(string lex)
        {
            bool check = false;
            var Constantregex = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            if (Constantregex.IsMatch(lex))
            {
                check = true;
            }
            return check;
        }
    }
}