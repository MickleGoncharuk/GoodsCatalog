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


namespace GoodsCatalog
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
            foreach (XElement xe in doc1.Element("root").Elements("category"))
                comboBox1.Items.Add(xe.Attribute("name").Value);
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
