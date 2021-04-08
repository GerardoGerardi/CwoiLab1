using System;
using System.Collections.Generic;

using System.Drawing;
using System.Linq;

using System.Windows.Forms;

using System.IO;
using ImgApp.Containers;
using System.Diagnostics;

namespace ImgApp
{
    public partial class Form1 : Form
    {
        
      
        List<GroupBox> Containers = new List<GroupBox>();
        int l = 0;
        int realCount = 0;
        bool flag = false;
        public Form1()
        {
            InitializeComponent();
            AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            this.AutoScroll = true;
            //инициализация разрешения драгондропа
        }

       
        private PictureBox MakePictureBox(string PictureName)
        {
            PictureBox tmp = new PictureBox();
            tmp.Name = "Pb";
            tmp.Location = new Point(6, 19);
            tmp.Width = 174;
            tmp.Height = 172;
            tmp.Image = new Bitmap(PictureName);
            tmp.SizeMode = PictureBoxSizeMode.StretchImage;
            return tmp;
            //для группбокса
        }



        

        private void del(int i,GroupBox tmp,Button btn, Button btn2, Button btn3, ComboBox cmbb, ComboBox cmbb2,PictureBox pb,TrackBar tb)
        {
            PictureBox k = (PictureBox)Containers[i].Controls[4];
            Bitmap l = (Bitmap)k.Image;
            k.Image = null;
            l.Dispose();
            Containers[i].Controls.Remove(pb);
            Containers[i].Controls.Remove(btn);
            Containers[i].Controls.Remove(btn2);
            Containers[i].Controls.Remove(btn3);
           
            Containers[i].Controls.Remove(cmbb);
            Containers[i].Controls.Remove(cmbb2);
            Containers[i].Controls.Remove(tb);
            Controls.Remove(tmp);
         
            Containers.Remove(Containers[i]);
            SetBoxes();
            GC.Collect();
            //удаление одного контейнера
        }


       
        private void Swap(int i,int j)
        {
            GroupBox tmp;
            tmp=Containers[j];
            Containers[j] = Containers[i];
            Containers[i] = tmp;
        }


        private void ChangeAlphAndChanel(int Lvl,PictureBox pb,string PictureName,string Chanel)
        {
            Bitmap pict= (Bitmap)pb.Image;
            Bitmap k1 = new Bitmap(PictureName);
            Bitmap k = new Bitmap(k1, NormolizeWidth(k1.Width), k1.Height);
            k1.Dispose();
            pict.Dispose();
            Picture tmp = new Picture(k);
            k.Dispose();
            tmp.ChangeAlphAndChanel(Lvl,Chanel);
            pb.Image =new Bitmap( tmp.TakePicture(),pb.Size);
            tmp = null;
            GC.Collect();

        }



        private GroupBox MakeBox(string PictureName,int counter)
        {
            GroupBox tmp = new GroupBox();
            string Name = "Box" + Convert.ToString(counter);
            tmp.Name = "Box" + Convert.ToString(counter);
            tmp.Text = PictureName;
            tmp.Width = 295;
            tmp.Height = 246;


            

            Button btn2 = new Button();
            btn2.Text = "<-";
            btn2.Name = "Lefter" + Convert.ToString(counter);
            btn2.Location = new Point(191, 83);
            btn2.Height = 30;
            btn2.Width = 95;
            tmp.Controls.Add(btn2);

            Button btn3 = new Button();
            btn3.Text = "->";
            btn3.Name = "Righter" + Convert.ToString(counter);
            btn3.Location = new Point(191, 119);
            btn3.Height = 30;
            btn3.Width = 95;
            tmp.Controls.Add(btn3);

            

            ComboBox cmbb = new ComboBox();
            cmbb.Name = "Operation" + Convert.ToString(counter);
            cmbb.Location = new Point(191, 29);
            cmbb.Height = 21;
            cmbb.Width = 95;
            var LST=new List<string>{"нет","сумма","разность", "Умножение","Среднее-арифметическое", "Максимум","Минимум" };
            for (int i = 0; i < LST.Count(); i++)
                cmbb.Items.Add(LST[i]);
            cmbb.SelectedItem = "нет";
            tmp.Controls.Add(cmbb);

            ComboBox cmbb2 = new ComboBox();
            cmbb2.Name = "Chanel" + Convert.ToString(counter);
            cmbb2.Location = new Point(191, 56);
            cmbb2.Height = 21;
            cmbb2.Width = 95;
            var LST2 =new List<string>{"R","G","B","RG","RB","GB","RGB" };
            for(int i=0;i<LST2.Count();i++)
                cmbb2.Items.Add(LST2[i]);
            cmbb2.SelectedItem = "RGB";
            tmp.Controls.Add(cmbb2);

            

            
            PictureBox pb = MakePictureBox(PictureName);
            tmp.Controls.Add(pb);
            tmp.Location = new Point(0, 0);



            Button btn = new Button();
            btn.Text = "Удалить";
            btn.Name = "Delete" + Convert.ToString(counter);
            btn.Location = new Point(191, 155);
            btn.Height = 30;
            btn.Width = 95;
            tmp.Controls.Add(btn); //засосывваю в групбокс

            TrackBar trb = new TrackBar();
            trb.Location = new Point(0, 200);
            trb.Height = 45;
            trb.Width = 293;
            tmp.Controls.Add(trb); //засосывваю в групбокс





            //привязка функций к кнопке
            btn.Click += delegate {
                for(int z=0;z<Containers.Count();z++)
                {
                    if (Containers[z].Name == Name)
                        del(z, tmp, btn, btn2, btn3,  cmbb, cmbb2, pb,trb);
                }
            };
            btn2.Click += delegate {
                bool flag = false;
                for (int z = 0; (!flag)&&(z < Containers.Count()); z++)
                {
                    if (Containers[z].Text == PictureName)
                    {
                        if (z  != 0)
                        {
                            Swap(z, z - 1);
                            SetBoxes();
                        }
                        flag = true;
                    }
                }
            };
            btn3.Click += delegate {
                bool flag = false;
                for (int z = 0; (!flag) && (z < Containers.Count()); z++)
                {
                    if (Containers[z].Text == PictureName)
                    {
                        if (z + 1 != Containers.Count())
                        {
                            Swap(z, z + 1);
                            SetBoxes();
                        }
                        flag = true;
                    }
                }
            };

            return tmp;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            if (l == 0)
            {
                try
                {
                    int n = Containers.Count();

                    string[] objects = (string[])e.Data.GetData(DataFormats.FileDrop);
                    for (int i = n; i < objects.Count() + n; i++)
                    {
                        Containers.Add(MakeBox(objects[i - n], i));
                        Controls.Add(Containers[i]);
                        realCount++;
                    }
                    SetBoxes();


                }
                catch
                {
                    MessageBox.Show("Ошибка");

                }
                l++;
            }
            else
                l = 0;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {

        }






        private void SetBoxes() //выводит на экран боксы
        {
            int x = 829;
            int y = 100;
            int i = 0;
            while (i < Containers.Count())
            {
          
                Containers[i].Location = new Point(x, y);
                
                y += 284;
                i++;
            }
            
        }



        private void Form1_Resize(object sender, EventArgs e)
        {

            if(Containers.Count()>1)
                if(this.Width>=Containers[0].Width)
                    SetBoxes();
            
        }



        private int NormolizeWidth(int tmp) //нормилизация чтобы не было искажения(увеличиваю изображение до нужной ширины) если кривое разрешение(не стандартное)
        {
            int Width = tmp;

            while (Width % 4 != 0)
            {
                Width++;

            }
            return Width;
        }


        
        private Bitmap Calculate() //функция в которой происходят методы рассчёта функций из класса пикчер
        {
         

            
            if (Containers.Count > 1)
            {
                var A =new Bitmap(Containers[0].Text);

                int Width = NormolizeWidth(A.Width);
                
                
                var B = new Bitmap(A, Width, A.Height);
                A.Dispose();
                Picture Res = new Picture(B);
                TrackBar tb = (TrackBar)Containers[0].Controls[6];
                ComboBox cmbbRees = (ComboBox)Containers[0].Controls[3];
                Res.ChangeAlphAndChanel(tb.Value, cmbbRees.SelectedItem.ToString());
                B.Dispose();
                
                for (int i=1;i<Containers.Count();i++)
                {
                    
                    var bm = new Bitmap(Containers[i].Text);
                    Bitmap bm2 = new Bitmap(bm, new Size(Width, Res.Height));
                    bm.Dispose();


                    Picture tmp = new Picture(bm2);
                    bm2.Dispose();
                    TrackBar trb = (TrackBar)Containers[i].Controls[6];
                    ComboBox cmbb = (ComboBox)Containers[i].Controls[2];
                    ComboBox cmbb2 = (ComboBox)Containers[i].Controls[3];
                    
                    if (cmbb.SelectedItem.ToString() == "сумма")
                      Res.Summ(tmp, cmbb2.SelectedItem.ToString(),trb.Value);
                    else
                    if (cmbb.SelectedItem.ToString() == "разность")
                            Res.Difference(tmp, cmbb2.SelectedItem.ToString(),trb.Value);
                    else
                    if (cmbb.SelectedItem.ToString() == "Умножение")
                            Res.Multiplication(tmp, cmbb2.SelectedItem.ToString(),trb.Value);
                    else
                    if (cmbb.SelectedItem.ToString() == "Среднее-арифметическое")
                            Res.Avg(tmp, cmbb2.SelectedItem.ToString(),trb.Value);
                    else
                    if (cmbb.SelectedItem.ToString() == "Максимум")
                        Res.Max(tmp, cmbb2.SelectedItem.ToString(), trb.Value);
                    else
                    if (cmbb.SelectedItem.ToString() == "Минимум")
                        Res.Min(tmp, cmbb2.SelectedItem.ToString(), trb.Value);

                    tmp = null;
                    GC.Collect();
                }
                Bitmap Result = Res.TakePicture();
                
                return Result;

            }
            else
                return null;
        }


        public void SavePic(Bitmap image) //сохранить
        {
            SaveFileDialog saveFileFialog = new SaveFileDialog();
            saveFileFialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveFileFialog.RestoreDirectory = true;

            if (saveFileFialog.ShowDialog() == DialogResult.OK)
            {
                if (image != null)
                {
                    image.Save(saveFileFialog.FileName);
                   
      
                }
            }
            image.Dispose();
            saveFileFialog.Dispose();
            GC.Collect();
        }


        private void button1_Click(object sender, EventArgs e) //кнопка
        {
             
            Stopwatch timer = new Stopwatch();
            timer.Start();
            
            Bitmap Result = Calculate();
            timer.Stop();
            if (Result != null)
            {
                Bitmap halkhogan = new Bitmap(Result, new Size(pictureBox1.Width, pictureBox1.Height));
                pictureBox1.Image = halkhogan;

                if (checkBox1.Checked)
                {
                    label1.Text = timer.ElapsedMilliseconds.ToString() + " мс";
                    label1.Visible = true;
                    if (Result != null)
                    {
                        SavePic(Result);

                    }
                    else
                        MessageBox.Show("Ошибка");
                }
            }
            else
                MessageBox.Show("Ошибка");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

