using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyCompiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    internal class Tiny_Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());

            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            Node funcStatement = new Node("Func_Statement");

            Func_Statement(funcStatement);
            if(funcStatement.Children.Count > 0)
            {
                program.Children.Add(funcStatement);
            }
            program.Children.Add(Main_Function());

            MessageBox.Show("Success");
            return program;
        }
        void Func_Statement(Node funcStatement)
        {
            if (((InputPointer + 1) < TokenStream.Count) && TokenStream[InputPointer + 1].tokenClass != Token_Class.Main)
            {
                funcStatement.Children.Add(Function_Statement());
                //program.Children.Add(funcStatement);
                Func_Statement(funcStatement);
            }
            else
            {
                return;
            }
        }
        Node Function_Statement()
        {
            Node functionStatement = new Node("Function_Statement");//int sum 

            functionStatement.Children.Add(Function_Decleration());
            functionStatement.Children.Add(Function_Body());
            
            return functionStatement;
        }
        Node Main_Function()
        {
            Node mainFunction = new Node("Main_Function");

            mainFunction.Children.Add(DataType());
            mainFunction.Children.Add(match(Token_Class.Main));
            mainFunction.Children.Add(match(Token_Class.LBracket));
            mainFunction.Children.Add(match(Token_Class.RBracket));
            mainFunction.Children.Add(Function_Body());

            return mainFunction;
        }
        Node Function_Decleration()
        {
            Node functionDecleration = new Node("Function_Decleration");

            functionDecleration.Children.Add(DataType());
            functionDecleration.Children.Add(match(Token_Class.Identifier));
            functionDecleration.Children.Add(match(Token_Class.LBracket));
            functionDecleration.Children.Add(Param());
            functionDecleration.Children.Add(match(Token_Class.RBracket));

            return functionDecleration;
        }
        Node DataType()
        {
            Node dataType = new Node("DataType");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].tokenClass == Token_Class.Integer)
                {
                    dataType.Children.Add(match(Token_Class.Integer));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Float)
                {
                    dataType.Children.Add(match(Token_Class.Float));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.String)
                {
                    dataType.Children.Add(match(Token_Class.String));
                }
            }

            return dataType;
        }
        Node Param()
        {
            Node param = new Node("Param");
            if ((InputPointer < TokenStream.Count) && TokenStream[InputPointer].tokenClass != Token_Class.RBracket)
            {
                param.Children.Add(DataType());
                param.Children.Add(match(Token_Class.Identifier));
                Parameter(param);
            }
            else
            {
                return null;
            }
            return param;
        }
        void Parameter(Node param)
        {
            Node parameter = new Node("Parameter");
            if ((InputPointer < TokenStream.Count) && TokenStream[InputPointer].tokenClass == Token_Class.Coma)
            {

                parameter.Children.Add(match(Token_Class.Coma));
                parameter.Children.Add(DataType());
                parameter.Children.Add(match(Token_Class.Identifier));
                param.Children.Add(parameter);
                Parameter(param);
            }
            else
            {
                return;
            }
        }
        /*Node Parameter()
        {
            Node paramater = new Node("Parameter");
            paramater.Children.Add(DataType());
            paramater.Children.Add(match(Token_Class.Identifier));
            return paramater;
        }*/

        Node Function_Body()
        {
            Node functionBody = new Node("Function_Body");
            functionBody.Children.Add(match(Token_Class.LCurlyBracket));
            Statements(functionBody);
            functionBody.Children.Add(Return_Statement());
            functionBody.Children.Add(match(Token_Class.RCurlyBracket));
            return functionBody;
        }
        void Statements(Node functionBody)
        {
            Node statements = new Node("Statements");
        
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].tokenClass != Token_Class.RCurlyBracket)
            {
                if (TokenStream[InputPointer].tokenClass == Token_Class.Read)
                {
                    statements.Children.Add(match(Token_Class.Read));
                    statements.Children.Add(match(Token_Class.Identifier));
                    statements.Children.Add(match(Token_Class.Semicolon));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Write)
                {
                    statements.Children.Add(match(Token_Class.Write));
                    if (TokenStream[InputPointer].tokenClass == Token_Class.Endl)
                    {
                        statements.Children.Add(match(Token_Class.Endl));
                    }
                    else
                    {
                        statements.Children.Add(Expression());
                    }
                    statements.Children.Add(match(Token_Class.Semicolon));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Return)
                {
                    int j = InputPointer + 1;
                    while (TokenStream[j].tokenClass != Token_Class.Semicolon)
                    {
                        j++;
                    }
                    if (TokenStream[j+1].tokenClass == Token_Class.RCurlyBracket)
                    {
                        return;
                    }
                    statements.Children.Add(Return_Statement());
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Identifier)
                {
                    statements.Children.Add(Assignment_Statement());
                    statements.Children.Add(match(Token_Class.Semicolon));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Integer || TokenStream[InputPointer].tokenClass == Token_Class.Float || TokenStream[InputPointer].tokenClass == Token_Class.String)
                {
                    if((InputPointer+2 < TokenStream.Count) && TokenStream[InputPointer+2].tokenClass == Token_Class.LBracket)
                    {
                        statements.Children.Add(Function_Statement());
                    }
                    else
                    {
                        statements.Children.Add(Decleration_Statement());
                        statements.Children.Add(match(Token_Class.Semicolon));
                    }
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.If)
                {
                    statements.Children.Add(If_Statement());
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Repeat)
                {
                    statements.Children.Add(Repeat_Statement());
                }
                if (statements.Children.Count > 0)
                {
                    functionBody.Children.Add(statements);
                    Statements(functionBody);
                }
            }
            else
            {
                return;
            }
        }
        Node Return_Statement()
        {
            Node returnStatement = new Node("Return_Statement");
            returnStatement.Children.Add(match(Token_Class.Return));
            returnStatement.Children.Add(Expression());
            returnStatement.Children.Add(match(Token_Class.Semicolon));
            return returnStatement;
        }
        Node Repeat_Statement()
        {
            Node repeat = new Node("Repeat_Statement");
            repeat.Children.Add(match(Token_Class.Repeat));
            Statements(repeat);
            repeat.Children.Add(match(Token_Class.Until));
            repeat.Children.Add(Condition_Statement());  
            return repeat;  
        }
        Node If_Statement()
        {
            Node ifStatement = new Node("If_Statement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(Condition_Statement());
            ifStatement.Children.Add(match(Token_Class.Then));
            Statements(ifStatement);
            Alternative(ifStatement);
            ifStatement.Children.Add(Alter());
            ifStatement.Children.Add(match(Token_Class.End));
            return ifStatement;
        }
        void Alternative(Node ifStatement)
        {
            Node alternative = new Node("Alternative");
            if (TokenStream[InputPointer].tokenClass == Token_Class.Elseif)
            {
                alternative.Children.Add(Elseif_Statement());
                ifStatement.Children.Add(alternative);
                Alternative(ifStatement);  
            }
            else
            {
                return;
            }
        }
        Node Alter()
        {
            Node alter = new Node("Alter");
            if (TokenStream[InputPointer].tokenClass == Token_Class.Else)
            {
                alter.Children.Add(Else_Statement());
            }
            else
            {
                return null;
            }
            return alter;
        }
        Node Elseif_Statement()
        {
            Node elseif = new Node("Elseif_Statement");
            elseif.Children.Add(match(Token_Class.Elseif));
            elseif.Children.Add(Condition_Statement());
            elseif.Children.Add(match(Token_Class.Then));
            Statements(elseif);
            return elseif;
        }

        Node Else_Statement()
        {
            Node elseStatement = new Node("Else_Statement");
            elseStatement.Children.Add(match(Token_Class.Else));  
            Statements(elseStatement);
            return elseStatement;
        }

        Node Condition_Statement()
        {
            Node conditionStatement = new Node("Condition_Statement");
            conditionStatement.Children.Add(Condition());
            Con(conditionStatement);
            return conditionStatement;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Identifier));
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());
            return condition;
        }

        void Con(Node conditionStatement)
        {
            Node con = new Node("Con");
            if (TokenStream[InputPointer].tokenClass == Token_Class.AndOp || TokenStream[InputPointer].tokenClass == Token_Class.OrOp)
            {
                con.Children.Add(Boolean_Operator());
                con.Children.Add(Condition());
                conditionStatement.Children.Add(con);
                Con(conditionStatement);
            }
            else 
            {
                return;
            }
        }

        Node Boolean_Operator()
        {
            Node Booleanop = new Node("Boolean_Operator");
            if(TokenStream[InputPointer].tokenClass == Token_Class.AndOp)
            {
                Booleanop.Children.Add(match(Token_Class.AndOp));
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.OrOp)
            {
                Booleanop.Children.Add(match(Token_Class.OrOp));
            }
            return Booleanop;
        }

        Node Condition_Operator()
        {
            Node conditionOp = new Node("Condition_Operator");
            if (TokenStream[InputPointer].tokenClass == Token_Class.GreaterThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.LessThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.EqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.EqualOp));
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.NotEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.NotEqualOp));
            }
            return conditionOp;
        }
        Node Decleration_Statement()
        {
            Node declerationStatement = new Node("Decleration_Statement");
            declerationStatement.Children.Add(DataType());
            if (TokenStream[InputPointer + 1].tokenClass != Token_Class.Coma && TokenStream[InputPointer + 1].tokenClass != Token_Class.Semicolon)
            {
                declerationStatement.Children.Add(Assignment_Statement());
            }
            else//(TokenStream[InputPointer + 1].tokenClass == Token_Class.Coma)//|| TokenStream[InputPointer + 1].tokenClass == Token_Class.Semicolon)
            {
                declerationStatement.Children.Add(match(Token_Class.Identifier));
            }  
            Repeat_Dec(declerationStatement);
           
            return declerationStatement;
        }
        void Repeat_Dec(Node declerationStatement)
        {
            if(TokenStream[InputPointer].tokenClass != Token_Class.Semicolon)
            {
                Dec(declerationStatement);
                Dec_Assignment(declerationStatement);
                Repeat_Dec(declerationStatement);
            }
            else
            {
                return;
            }
            
        }
        void Dec(Node declerationStatement)
        {
            Node dec = new Node("Dec");
            if (InputPointer + 2 < TokenStream.Count && TokenStream[InputPointer].tokenClass == Token_Class.Coma && TokenStream[InputPointer + 2].tokenClass != Token_Class.AssignmentOp)
            {
                dec.Children.Add(match(Token_Class.Coma));
                dec.Children.Add(match(Token_Class.Identifier));
                declerationStatement.Children.Add(dec);
                Dec(declerationStatement);
            }
            else
            {
                return;
            }
        }
        void Dec_Assignment(Node declerationStatement)
        {
            Node dec_assignment = new Node("Dec_Assignment");
            if (InputPointer+2<TokenStream.Count&&TokenStream[InputPointer].tokenClass == Token_Class.Coma && TokenStream[InputPointer+2].tokenClass == Token_Class.AssignmentOp)
            {
                dec_assignment.Children.Add(match(Token_Class.Coma));
                dec_assignment.Children.Add(Assignment_Statement());
                declerationStatement.Children.Add(dec_assignment);
                Dec_Assignment(declerationStatement);
            }
            else
            {
                return;
            }
        }
        Node Assignment_Statement()
        {
            Node assignmentStatement = new Node("Assignment_Statement");
            assignmentStatement.Children.Add(match(Token_Class.Identifier));
            assignmentStatement.Children.Add(match(Token_Class.AssignmentOp));
            assignmentStatement.Children.Add(Expression());
            return assignmentStatement;
        }
        Node Expression()
        {
            Node expression = new Node("Expression");
            if (InputPointer+1 < TokenStream.Count)
            {
                if (TokenStream[InputPointer].tokenClass == Token_Class.StringValue)
                {
                    expression.Children.Add(match(Token_Class.StringValue));
                    //return expression;
                }
                else if(isTerm()&& (TokenStream[InputPointer+1].tokenClass == Token_Class.Semicolon))
                {
                    expression.Children.Add(Term());
                }
                else if(isTerm() || (TokenStream[InputPointer].tokenClass == Token_Class.LBracket))
                {
                    expression.Children.Add(Equation(Term()));
                    //return expression;
                    /*Node x = Term();
                    
                    if ((TokenStream[InputPointer].tokenClass == Token_Class.Semicolon) && (x != null))// || (TokenStream[InputPointer].tokenClass == Token_Class.Coma))
                    {
                        expression.Children.Add(x);
                        return expression;
                    }
                    else 
                    {*/
                    //}

                }
                else
                {
                    Error.Errors.Add("Parsing Error: Expected Expression and " +
                        TokenStream[InputPointer].tokenClass.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            /**/
            return expression;
        }
        bool isTerm()
        {
            if (TokenStream[InputPointer].tokenClass == Token_Class.Constant)
            {
                return true;
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.Identifier && TokenStream[InputPointer + 1].tokenClass == Token_Class.LBracket)
            {
                return true;
            }
            else if (TokenStream[InputPointer].tokenClass == Token_Class.Identifier)
            {
                return true;
            }
            return false;
        }

        Node Term()
        {
            Node term = new Node("Term");
            if (InputPointer + 1 < TokenStream.Count)
            {
                if (TokenStream[InputPointer].tokenClass == Token_Class.Constant)
                {
                    term.Children.Add(match(Token_Class.Constant));
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Identifier && TokenStream[InputPointer + 1].tokenClass == Token_Class.LBracket)
                {
                    term.Children.Add(Function_call());
                }
                else if (TokenStream[InputPointer].tokenClass == Token_Class.Identifier)
                {
                    term.Children.Add(match(Token_Class.Identifier));
                }
                else
                {
                    return null;
                }
            }
            return term;
        }

        Node Function_call()
        {
            Node function_call = new Node("Function_call");
            function_call.Children.Add(match(Token_Class.Identifier));
            function_call.Children.Add(match(Token_Class.LBracket));
            function_call.Children.Add(Argument());
            function_call.Children.Add(match(Token_Class.RBracket));
            return function_call;
        }

        Node Argument()
        {
            Node argument = new Node("Argument");
            if ((InputPointer < TokenStream.Count) && TokenStream[InputPointer].tokenClass != Token_Class.RBracket)
            {
                argument.Children.Add(match(Token_Class.Identifier));
                Arglist(argument);
                return argument;
            }
            return null;
        }
        void Arglist(Node argument)
        {
            Node arglist = new Node("Arglist");
            if ((InputPointer < TokenStream.Count) && TokenStream[InputPointer].tokenClass == Token_Class.Coma)
            {

                arglist.Children.Add(match(Token_Class.Coma));
                arglist.Children.Add(match(Token_Class.Identifier));
                argument.Children.Add(arglist);
                Arglist(argument);
            }
            else
            {
                return;
            }
        }
        /*Node Arg()
        {
            Node arg = new Node("Arg");
            arg.Children.Add(match(Token_Class.Identifier));
            return arg;
        }*/

        Node Equation(Node x)
        {
            Node equation = new Node("Equation");
            if (x != null)
            {
                equation.Children.Add(x);
                Equa(equation);
            }
            else
            {
                equation.Children.Add(match(Token_Class.LBracket));
                equation.Children.Add(Term());
                Equa(equation);
                equation.Children.Add(match(Token_Class.RBracket));
                Equa(equation);
            }
            return equation;
        }
        void Equa(Node equation)
        {
            Node equa = new Node("Equa");
            if (TokenStream[InputPointer].tokenClass == Token_Class.PlusOp|| TokenStream[InputPointer].tokenClass == Token_Class.MinusOp || TokenStream[InputPointer].tokenClass == Token_Class.MultiplyOp || TokenStream[InputPointer].tokenClass == Token_Class.DivideOp)//(5)
            {
                equa.Children.Add(ArithmeticOp());
                equa.Children.Add(Beta(equation));
                equation.Children.Add(equa);
                Equa(equation);
            }
            else
            {
                return;
            }
        }
        Node Beta(Node equation)
        {
            Node beta = new Node("Beta");
            if (TokenStream[InputPointer].tokenClass == Token_Class.LBracket)
            {
                beta.Children.Add(match(Token_Class.LBracket));
                beta.Children.Add(Term());
                Equa(beta);
                beta.Children.Add(match(Token_Class.RBracket));

            }
            else
            {
                beta.Children.Add(Term());
            }
            return beta;
        }
        Node ArithmeticOp()
        {
            Node arithmeticOp = new Node("ArithmeticOp");
            if (TokenStream[InputPointer].tokenClass == Token_Class.PlusOp)
            {
                arithmeticOp.Children.Add(match(Token_Class.PlusOp));   
            }
            else if(TokenStream[InputPointer].tokenClass == Token_Class.MinusOp)
            {
                arithmeticOp.Children.Add(match(Token_Class.MinusOp));
            }
            else if(TokenStream[InputPointer].tokenClass == Token_Class.MultiplyOp)
            {
                arithmeticOp.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if(TokenStream[InputPointer].tokenClass == Token_Class.DivideOp)
            {
                arithmeticOp.Children.Add(match(Token_Class.DivideOp));
            }
            return arithmeticOp;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].tokenClass)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Error.Errors.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].tokenClass.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Error.Errors.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
    

}

