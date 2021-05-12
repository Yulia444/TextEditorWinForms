using System;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
	public partial class TextEditor : Form
	{
		bool unsavedChanges;
		string fullFileName;
		public TextEditor()
		{
			InitializeComponent();
			unsavedChanges = false;
			fullFileName = null;
			tsmiUndo.Enabled = false;
		}

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			unsavedChanges = true;
			tsmiUndo.Enabled = richTextBox1.CanUndo;
		}

		private void tsmiOpen_Click(object sender, EventArgs e)
		{
			if (unsavedChanges)
			{
				DialogResult dr = 
					MessageBox.Show("The file has been changed. Do you want to save changes?", "Save Changes", MessageBoxButtons.YesNoCancel);
				if (dr == DialogResult.Cancel)
					return;
				if (dr == DialogResult.Yes)
				{
					tsmiSave_Click(sender, e);
					if (unsavedChanges)
						return;
				}	
			}

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt||";
			openFileDialog.FilterIndex = 1;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					StreamReader reader = File.OpenText(openFileDialog.FileName);
                    richTextBox1.Text = reader.ReadToEnd();
					fullFileName = openFileDialog.FileName;
					this.Text = fullFileName + " - Text Editor";
					unsavedChanges = false;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void tsmiNew_Click(object sender, EventArgs e)
		{
			if (unsavedChanges)
			{
				DialogResult dr =
					MessageBox.Show("The file has been changed. Do you want to save changes?", "Save Changes", MessageBoxButtons.YesNoCancel);
				if (dr == DialogResult.Cancel)
					return;
				if (dr == DialogResult.Yes)
				{
					tsmiSave_Click(sender, e);
					if (unsavedChanges)
						return;
				}	
			}

            richTextBox1.Clear();
			this.Text = "Text Editor";
			unsavedChanges = false;
			fullFileName = null;
		}

		private void tsmiSave_Click(object sender, EventArgs e)
		{
			if (fullFileName == null)
			{
				tsmiSaveAs_Click(sender, e);
				return;
			}
			if (!unsavedChanges)
				return;
			SaveToFile();
		}

		private void SaveToFile()
		{
			try
			{
				StreamWriter writer = new StreamWriter(fullFileName);
				writer.Write(richTextBox1.Text);
				writer.Close();
				unsavedChanges = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
		}

		private void tsmiSaveAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt||";
			saveFileDialog.FilterIndex = 1;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				fullFileName = saveFileDialog.FileName;
				SaveToFile();
				this.Text = fullFileName + " - Text Editor";
			}
		}

		private void tsmiExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (unsavedChanges)
			{
				DialogResult dr =
					MessageBox.Show("The file has been changed. Do you want to save changes?", "Save Changes", MessageBoxButtons.YesNoCancel);
				if (dr == DialogResult.Cancel)
					e.Cancel = true;
				if (dr == DialogResult.Yes)
				{
					tsmiSave_Click(sender, e);
					if (unsavedChanges)
						e.Cancel = true;
				}
			}
		}

		private void tsmiUndo_Click(object sender, EventArgs e)
		{
            richTextBox1.Undo();
		}

		private void tsmiCopy_Click(object sender, EventArgs e)
		{
            richTextBox1.Copy();
		}

		private void tsmiCut_Click(object sender, EventArgs e)
		{
            richTextBox1.Cut();
		}

		private void tsmiPaste_Click(object sender, EventArgs e)
		{
            richTextBox1.Paste();
		}

		private void tsmiSelectAll_Click(object sender, EventArgs e)
		{
            richTextBox1.SelectAll();
		}

		private void tsmiFont_Click(object sender, EventArgs e)
		{

			fontDialog1.Font = richTextBox1.Font;
			if (fontDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionFont = fontDialog1.Font;
		}

		private void tsmiFontColor_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			dialog.Color = richTextBox1.ForeColor;
			if (dialog.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionColor = dialog.Color;
		}

		private void tsmiBackColor_Click(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			dialog.Color = richTextBox1.BackColor;
			if (dialog.ShowDialog() == DialogResult.OK)
                richTextBox1.BackColor = dialog.Color;
		}


        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutEditor = new AboutProgram();
            aboutEditor.Show();
        }
    }
}
