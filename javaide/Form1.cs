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
            int currentPos = txtCode.CurrentPosition;
            int currentLine = txtCode.LineFromPosition(currentPos);
            string currentLineText = txtCode.Lines[currentLine].Text;

            // Auto-indent on Enter
            if (e.Char == '\n' && currentLine > 0)
            {
                string prevLine = txtCode.Lines[currentLine - 1].Text;
                string indent = GetIndentation(prevLine);

                // Add extra tab if previous line ends with {
                if (prevLine.TrimEnd().EndsWith("{"))
                {
                    indent += "\t";
                }

                txtCode.InsertText(currentPos, indent);
            }

            // Handle opening brace for block formatting
            if (e.Char == '{')
            {
                txtCode.DeleteRange(currentPos - 1, 1);

                string currentIndent = GetIndentation(currentLineText);
                string innerIndent = currentIndent + "\t";

                string block = "{\n" + innerIndent + "\n" + currentIndent + "}";

                txtCode.InsertText(currentPos - 1, block);

                // Move caret to the inner line (after the tab)
                int newCaretPos = currentPos - 1 + 2 + innerIndent.Length;
                txtCode.GotoPosition(newCaretPos);
            }

            // Handle method signature - When typing "()"
            if (e.Char == ')')
            {
                string prevLine = txtCode.Lines[currentLine].Text;

                // If the line ends with a method signature (e.g., `public static void main(String args[])`), insert the opening brace
                if (prevLine.Contains(")") && !prevLine.Contains("{"))
                {
                    string currentIndent = GetIndentation(prevLine);
                    string openBrace = currentIndent + "{\n";
                    string innerIndent = currentIndent + "\t";

                    // Insert the brace with a new line
                    txtCode.InsertText(currentPos, openBrace + innerIndent + "\n" + currentIndent + "}");

                    // Move the cursor inside the method body
                    int newCaretPos = currentPos + openBrace.Length + innerIndent.Length;
                    txtCode.GotoPosition(newCaretPos);
                }
            }

            // Handle semicolon for correct indentation
            if (e.Char == ';')
            {
                string prevLine = txtCode.Lines[currentLine].Text;

                // If the current line ends with a semicolon, move to the next line with correct indentation
                if (prevLine.TrimEnd().EndsWith(";"))
                {
                    string currentIndent = GetIndentation(prevLine);
                    // Move caret to the next line, keeping the same indentation, but no new line insertion
                    txtCode.InsertText(currentPos, "\n" + currentIndent);

                    // After inserting, move the caret to the new position
                    txtCode.GotoPosition(currentPos + currentIndent.Length + 1);
                }
            }
        }




        private string GetIndentation(string line)
        {
            StringBuilder indent = new StringBuilder();
            foreach (char c in line)
            {
                if (c == ' ' || c == '\t')
                    indent.Append(c);
                else
                    break;
            }
            return indent.ToString();
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
