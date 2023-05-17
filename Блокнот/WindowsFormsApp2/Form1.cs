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

namespace WindowsFormsApp2
{
    public partial class Блокнот : Form
    {
        public string filename;
        public bool isFileChanged;
        public Блокнот()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            filename = "";
            isFileChanged = false;
            UpdateText();
        }
        public void CreateNewDocument(object sender, EventArgs e)                                  //Файл --> Создать документ
        {
            SavedUnsavedfile();
            textBox1.Text = "";
            filename = "";
            UpdateText();
            isFileChanged = false;
        }

        private void новоеОкноToolStripMenuItem_Click(object sender, EventArgs e)                  //Файл --> Новое окно
        {
            Блокнот x = new Блокнот();
            x.Show();
        }
        public void OpenFile(object sender, EventArgs e)                                           //Файл --> Открыть документ
        {
            SavedUnsavedfile();
            openFileDialog1.FileName = "";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    textBox1.Text = sr.ReadToEnd();
                    sr.Close();
                    filename = openFileDialog1.FileName;
                    isFileChanged = false;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл");
                }
            }
            UpdateText();
        }

        public void SaveFile(string _filename )                                                     //Функция сохранения документа
        {
            if(_filename == "")
            {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    _filename = saveFileDialog1.FileName;
                }
            }
            try
            {
                StreamWriter sw = new StreamWriter(_filename + ".txt");
                sw.Write(textBox1.Text);
                filename = _filename;
                isFileChanged = false;
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить файл");
            }
            UpdateText();
        }

        private void Save(object sender, EventArgs e)                                              //Файл --> Сохранить документ
        {
            SaveFile(filename);
        }
        private void SaveAs(object sender, EventArgs e)                                            //Файл --> Сохранить как документ
        {
            SaveFile("");
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)                            // При изменении текста появляется звёздочка (несохранённый файл) и убирается (сохранённый файл)
        {

            if (!isFileChanged)
            {
                this.Text = this.Text.Replace("*", " ");
                isFileChanged = true;
                this.Text = "*" + this.Text;
            }
        }
        private void UpdateText()                                                                  //Имя документа или "Без имени" в шапке приложения
        {
            if(filename!="")
            this.Text = filename + " - Блокнот";
            else this.Text = "Без имени - Блокнот";
        }
        private void SavedUnsavedfile()                                                            //Функция сохранения несохранённого файла
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения в файле?", "Сохранение файла", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.Yes)
                {
                    SaveFile(filename);
                }
            }
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)                      //Файл --> Выйти
        {
            SavedUnsavedfile();
            Application.Exit();
        }

        private void увеличитьToolStripMenuItem_Click(object sender, EventArgs e)                  //Просмтотр --> Масштаб --> Увеличить
        {
            textBox1.Font = new Font(textBox1.Font.Name, textBox1.Font.Size + 1);
        }

        private void уменьшитьToolStripMenuItem_Click(object sender, EventArgs e)                  //Просмтотр --> Масштаб --> Увеличить
        {
            textBox1.Font = new Font(textBox1.Font.Name, textBox1.Font.Size - 1);
        }

        private void восстановитьМасштабПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)//Просмтотр --> Масштаб --> Восстановить масштаб...
        {
            textBox1.Font = new Font(textBox1.Font.Name, 8);
        }
        private void Блокнот_FormClosed(object sender, FormClosedEventArgs e)                      //При нажатии на красный крестик высвечивавется "Сохранить файл?" когда нужно
        {
            SavedUnsavedfile();
        }

        private void копироватьToolStripMenuItem_Click_1(object sender, EventArgs e)               //Изменить --> Копировать
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Copy();
            }
            else
            {
                //Выводим сообщение о том, что текст был не выделен
                MessageBox.Show(this, "Вы не выделили текст", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void вырезатьToolStripMenuItem_Click_1(object sender, EventArgs e)                 //Изменить --> Вырезать
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Copy();
                textBox1.Text = textBox1.Text.Remove(textBox1.SelectionStart, textBox1.SelectionLength);
            }
            else
            {
                //Выводим сообщение о том, что текст был не выделен
                MessageBox.Show(this, "Вы не выделили текст", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)                   //Изменить --> Отменить всё
        {
            if (textBox1.CanUndo == true)
            {
                // Undo the last operation.
                textBox1.Undo();
                // Clear the undo buffer to prevent last action from being redone.
                textBox1.ClearUndo();
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)                   //Изменить --> Вставить
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                textBox1.Paste();
            }
            else
            {
                //Выводим сообщение о том, что в буфере обмена нет текста
                MessageBox.Show(this, "В буфере обмена нет текста", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void выбратьВсёToolStripMenuItem_Click(object sender, EventArgs e)                 //Изменить --> Выбрать всё
        {
            textBox1.SelectAll();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)                    //Изменить --> Удалить
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.SelectionStart, textBox1.SelectionLength);
            }
            else
            {
                //Выводим сообщение о том, что текст был не выделен
                MessageBox.Show(this, "Вы не выделили текст", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void времяИДатаToolStripMenuItem_Click(object sender, EventArgs e)                 //Изменить --> Время и дата
        {
            textBox1.Text += DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
