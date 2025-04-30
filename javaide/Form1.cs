using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;
using System.Diagnostics;


namespace javaide
{
    public partial class Form1 : Form
    {
        private string javaFilePath = "Program.java";
        public Form1()
        {
            InitializeComponent();
            InitializeEditorTheme();
            txtCode.CharAdded += TxtCode_CharAdded;
        }

        private void TxtCode_CharAdded(object sender, CharAddedEventArgs e)
        {
            char c = (char)e.Char;

            string closingChar = null;

            switch (c)
            {
                case '(':
                    closingChar = ")";
                    break;
                case '{':
                    closingChar = "}";
                    break;
                case '[':
                    closingChar = "]";
                    break;
                case '"':
                    closingChar = "\"";
                    break;
                case '\'':
                    closingChar = "'";
                    break;
            }


            if (closingChar != null)
            {
                int currentPos = txtCode.CurrentPosition;
                txtCode.InsertText(currentPos, closingChar);
                txtCode.GotoPosition(currentPos); // Move cursor between the pair
            }
        }

        private void InitializeEditorTheme()
        {
            txtCode.StyleResetDefault();
            txtCode.Styles[Style.Default].Font = "Consolas";
            txtCode.Styles[Style.Default].Size = 15;
            txtCode.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
            txtCode.Styles[Style.Default].ForeColor = Color.FromArgb(220, 220, 220);
            txtCode.StyleClearAll();

            txtCode.Lexer = Lexer.Cpp;
            

            // Java Keywords
            txtCode.SetKeywords(0,
        "abstract assert break case catch class const continue default do else enum " +
        "extends final finally for goto if implements import instanceof interface " +
        "native new package private protected public return static strictfp super " +
        "switch synchronized this throw throws transient try volatile while");

            // Set type-related keywords (Style.Cpp.Word2 - GREENISH)
            txtCode.SetKeywords(1,
                "void int float double long short char boolean byte String true false null");

            // Style definitions (VS Code Dark+ Theme Inspired)
            txtCode.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(106, 153, 85);
            txtCode.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(106, 153, 85);
            txtCode.Styles[Style.Cpp.Number].ForeColor = Color.FromArgb(181, 206, 168);
            txtCode.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(206, 145, 120);
            txtCode.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(206, 145, 120);
            txtCode.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(86, 156, 214); // keywords
            txtCode.Styles[Style.Cpp.Word2].ForeColor = Color.FromArgb(78, 201, 176); // teal-green
            txtCode.Styles[Style.Cpp.Operator].ForeColor = Color.FromArgb(212, 212, 212);
            txtCode.Styles[Style.Cpp.Identifier].ForeColor = Color.FromArgb(220, 220, 220);

            // Line numbers
            txtCode.Margins[0].Width = 30;
            txtCode.Margins[0].BackColor = Color.FromArgb(45, 45, 45);

            // Highlight current line
            txtCode.CaretLineVisible = true;
            txtCode.CaretLineBackColor = Color.FromArgb(40, 40, 40);

            // Caret and selection
            txtCode.CaretForeColor = Color.White;
            txtCode.SetSelectionBackColor(true, Color.FromArgb(60, 60, 60));

            // Optional: Indentation guides
            txtCode.IndentationGuides = IndentView.LookBoth;

            
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText(javaFilePath, txtCode.Text);
            textBox2.Text = "Saved as " + javaFilePath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(javaFilePath, txtCode.Text);
            ProcessStartInfo psi = new ProcessStartInfo("javac", javaFilePath);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            textBox2.Text = string.IsNullOrEmpty(error) ? "Compiled successfully!" : error;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo("java", "Program");
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            textBox2.Text = string.IsNullOrEmpty(error) ? output : error;
        }

        private void txtCode_Click(object sender, EventArgs e)
        {

        }
    }
}
