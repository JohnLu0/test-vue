using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CSVReader
{
    public partial class Form1 : Form
    {
        CSVCopy cvscopy;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();     //显示选择文件对话框
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = openFileDialog1.FileName;          //显示文件路径
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text;
            //正整数的正则表达
            Regex regInternal = new Regex(@"^[1-9]\d*$");
            //获取用户选择的地址并写入配置文件中
            string path1 = textBox2.Text;
            string path2 = textBox3.Text;
            if (text.Trim().ToString() == "" || string.IsNullOrEmpty(text.Trim()))
            {
                MessageBox.Show("时间间隔不能为空");
                return;
            }
           
            if (!(regInternal.IsMatch(text)))
            {
                MessageBox.Show("时间间隔必须为正整数");
                return;
            }

            if (Convert.ToInt32(text)<3600)
            {
                MessageBox.Show("时间必須大於3600秒");
                return;
            }
            else {
                //获取用户输入的时间间隔
                int internals = int.Parse(text);
                //调用定时存取任务
                CSVCopy cvscopy = new CSVCopy(path1, path2, internals);
                this.cvscopy = cvscopy;
                this.cvscopy.setInternal();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cvscopy.removeInternal();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //选择文件夹对话框
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                  //文件夹路径
                  string folderPath = folder.SelectedPath;
                  this.textBox3.Text = folderPath;
            }
 

        }
    }
}
