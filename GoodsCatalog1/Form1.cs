using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace GoodsCatalog1
{
    public partial class Form1 : Form
    {
        string path1;
        string path2;

        string path3;
        string path4;

        XDocument doc1;
        XDocument doc2;
        
        public Form1()
        {
            InitializeComponent();
            
            path1 = @"..\..\categories.xml";
            path2 = @"..\..\goods.xml";
            
            doc1 = XDocument.Load(path1);
            doc2 = XDocument.Load(path2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            foreach (XElement c in doc1.Element("root").Elements("category"))
                comboBox1.Items.Add(c.Attribute("name").Value);
            comboBox1.SelectedIndex = 0;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text.Length > 0)
            {
               
                doc1.Element("root").Add(new XElement("category", new XAttribute("id", "0"), new XAttribute("name", textBox1.Text)));
                doc1.Save(path1);
                comboBox1.Items.Add(textBox1.Text);
                textBox1.Clear();
                MessageBox.Show("Вы добавили новую категорию", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = comboBox1.SelectedItem.ToString();
            var goodsX = from p in doc2.Element("root").Elements("product")
                         where p.Attribute("category").Value == name
                         select p;
            listBox1.Items.Clear();
            foreach (XElement x in goodsX)
                listBox1.Items.Add(String.Format("{0} - {1} uah", 
                    x.Attribute("name").Value,
                    x.Attribute("price").Value));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count == 0)
                MessageBox.Show("Вы не выбрали товары для добавления", "Ошибка действия",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                string buff = "";
                double price1 = 0;
                double price2 = 0;
                int k1 = 0;
                int k2 = 0;

                foreach (int k in listBox1.SelectedIndices)
                {
                    buff = listBox1.Items[k].ToString();

                    k1 = buff.IndexOf('-');
                    k2 = buff.IndexOf("uah");

                    price1 = Convert.ToDouble(buff.Substring(k1 + 2, k2 - k1 - 3));
                    price2 = Convert.ToDouble(textBox2.Text);

                    listBox2.Items.Add(buff);
                    textBox2.Text = String.Format("{0}", (price1 + price2));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndices.Count == 0)
                MessageBox.Show("Вы не выбрали товары для удаления", "Ошибка действия",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                string buff = "";
                double price1 = 0;
                double price2 = 0;
                int k1 = 0;
                int k2 = 0;
                int k = 0;
                
                for (int i = listBox2.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    k = listBox2.SelectedIndices[i];
                    buff = listBox2.Items[k].ToString();

                    k1 = buff.IndexOf('-');
                    k2 = buff.IndexOf("uah");
                    price1 = Convert.ToDouble(buff.Substring(k1 + 2, k2 - k1 - 3));
                    price2 = Convert.ToDouble(textBox2.Text);

                    textBox2.Text = String.Format("{0}", (price2 - price1));
                    listBox2.Items.RemoveAt(k);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string buff = listBox2.SelectedItem.ToString();
            string[] arr = buff.Split('-');
            string name = arr[0].Trim();

            var result = from p in doc2.Element("root").Elements("product")
                         where p.Attribute("name").Value == name
                         select p;
            
            string path = "";
            foreach (var r in result)
                path = r.Attribute("image").Value;
            pictureBox2.Image = new Bitmap(path);
        }

     

        private void button6_Click(object sender, EventArgs e)
        {
            string name = textBox3.Text;
            string price = textBox4.Text;
            string categ = textBox5.Text;
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                MessageBox.Show("Выберете картинку для нового товара", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    path3 = ofd.FileName;
                    path4 = @"..\..\Images\" + Path.GetFileName(path3);
                    File.Copy(path3, path4);
                    pictureBox2.Image = new Bitmap(path4);
                    doc2.Element("root").Add(new XElement("product", new XAttribute("name", name), new XAttribute("price", price), new XAttribute("category", categ), new XAttribute("image", path4)));
                    doc1.Element("root").Add(new XElement("category", new XAttribute("id", "0"), new XAttribute("name", categ)));
                    MessageBox.Show("Вы добавили новый товар", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    doc2.Save(path2);
                    doc1.Save(path1);
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                     
                }
                
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
               
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Вы не выбрали категорию для удаления", "Ошибка действия",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {

                string name = comboBox1.SelectedItem.ToString();
                var cat = from p in doc1.Element("root").Elements("category")
                          where p.Attribute("id").Value == name || p.Attribute("name").Value == name
                          select p;
                
                
                cat.Remove();
                doc1.Save(path1);

                var cat1 = from p1 in doc2.Element("root").Elements("product")
                           where p1.Attribute("name").Value == name && p1.Attribute("price").Value == name && p1.Attribute("category").Value == name && p1.Attribute("image").Value == name
                           select p1;
                

                cat1.Remove();
                doc2.Save(path2);
                comboBox1.SelectedIndex = 0;
                textBox1.Clear();
                MessageBox.Show("Вы удалили категорию", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                


            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (listBox1.SelectedIndex == 0)
                MessageBox.Show("Вы не выбрали товары для удаления", "Ошибка действия",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                string name = listBox1.SelectedItem.ToString();
                var cat = from p in doc2.Element("root").Elements("product")
                          where p.Attribute("name").Value == name && p.Attribute("price").Value == name && p.Attribute("category").Value == name && p.Attribute("image").Value == name
                          select p;

                
                cat.Remove();
                doc2.Save(path2);
                listBox1.Items.Remove(name);
                

                
            }
        }
    }
}
