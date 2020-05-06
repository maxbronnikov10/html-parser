using Parser.Core;
using Parser.Core.Habra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Parser
{
    class Window : Form
    {
        readonly TableLayoutPanel table, mainFieldTable, inputTabel;
        readonly ListBox tagsList;
        readonly MenuStrip menu;
        readonly SaveFileDialog saveFileDialog;
        readonly TextBox baseUrlTextBox, prefixTextBox, selectorTextBox, classNameTextBox;
        ParserWorker parser;

        public Window()
        {
            parser = new ParserWorker(new DefaultParser(), null);
            Button startButton = new Button() {Text = "Start", Dock = DockStyle.Fill};
            Button abortButton = new Button() { Text = "Abort", Dock = DockStyle.Fill };

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files(*.txt)|*.txt";

            menu = new MenuStrip() { Dock = DockStyle.Fill };
            ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Сохранить");
            saveItem.Click += SaveClick;
            fileItem.DropDownItems.Add(saveItem);
            menu.Items.Add(fileItem);
            
            baseUrlTextBox = new TextBox() {Dock = DockStyle.Fill };
            prefixTextBox = new TextBox() { Dock = DockStyle.Fill };
            selectorTextBox = new TextBox() { Dock = DockStyle.Fill };
            classNameTextBox = new TextBox() { Dock = DockStyle.Fill };

            Label baseUrlLabel = new Label() { Text = "Base Url:", Dock = DockStyle.Fill };
            Label prefixLabel = new Label() { Text = "Prefix", Dock = DockStyle.Fill };
            Label selectorLabel = new Label() { Text = "Selector", Dock = DockStyle.Fill };
            Label classNameLabel = new Label() { Text = "Name of class", Dock = DockStyle.Fill };

            tagsList = new ListBox() { Dock = DockStyle.Fill };

            table = new TableLayoutPanel() { Dock = DockStyle.Fill };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            table.Controls.Add(menu, 0, 0);
     

            mainFieldTable = new TableLayoutPanel() { Dock = DockStyle.Fill };
            mainFieldTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainFieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            mainFieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            mainFieldTable.Controls.Add(tagsList, 0, 0);
            table.Controls.Add(mainFieldTable, 0, 1);


            inputTabel = new TableLayoutPanel() { Dock = DockStyle.Fill };
            inputTabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for(int i = 0; i < 10; i++) inputTabel.RowStyles.Add(new RowStyle(SizeType.Percent,10));
            inputTabel.Controls.Add(baseUrlLabel,0,0);
            inputTabel.Controls.Add(baseUrlTextBox, 0, 1);
            inputTabel.Controls.Add(prefixLabel, 0, 2);
            inputTabel.Controls.Add(prefixTextBox, 0, 3);
            inputTabel.Controls.Add(selectorLabel, 0, 4);
            inputTabel.Controls.Add(selectorTextBox, 0, 5);
            inputTabel.Controls.Add(classNameLabel, 0, 6);
            inputTabel.Controls.Add(classNameTextBox, 0, 7);
            inputTabel.Controls.Add(startButton, 0, 8);
            inputTabel.Controls.Add(abortButton, 0, 9);
            mainFieldTable.Controls.Add(inputTabel, 1,0);

            Controls.Add(table);

            startButton.Click += StartButton_Click;
            abortButton.Click += (sender, args) => parser.Abort();
            parser.OnCompleted += (obj) => MessageBox.Show("All works done!");
            parser.OnNewData += Parser_OnNewData;
        }

        private void SaveClick(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            string fileName = saveFileDialog.FileName;
            StringBuilder text = new StringBuilder();
            foreach (var item in tagsList.Items) text.AppendLine(item.ToString());
            System.IO.File.WriteAllText(fileName, text.ToString());
            MessageBox.Show("Файл сохранен");
        }

        private void Parser_OnNewData(object arg1, IEnumerable arg2)
        {
            foreach (var item in arg2)
            {
                tagsList.Items.Add(item);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            parser.Settings = new DefaultSettings(baseUrlTextBox.Text, prefixTextBox.Text, selectorTextBox.Text, classNameTextBox.Text);
            parser.Start();
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new Window());
        }
    }
}
