using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Token> tokens = new List<Token>();
        private void Compile_Click(object sender, EventArgs e)
        {
            string code = codeTextBox.Text;
            Tiny_Compiler tiny_Compiler = new Tiny_Compiler();
            tokens = tiny_Compiler.startCompiling(code);

            displayTokens(tokens);

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(Tiny_Parser.PrintParseTree(Tiny_Compiler.treeroot));
            
            displayErrors();
            
        }

        void displayTokens(List<Token> tokenList)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Lexeme");
            dataTable.Columns.Add("Token Class");

            foreach (var i in tokenList)
            {
                dataTable.Rows.Add(i.lex.ToString(), i.tokenClass.ToString());
            }

            dataGridView1.DataSource = dataTable;
        }
        void displayErrors()
        {
            errorListTextBox.Clear();
            foreach (var i in Error.Errors)
            {
                errorListTextBox.AppendText(i);
                errorListTextBox.AppendText(Environment.NewLine);
            }
        }
    }
}