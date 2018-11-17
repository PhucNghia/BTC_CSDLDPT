using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTC_FORM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //cutImg();
        }

        private void cutImg()
        {
            // read img
            Bitmap bmp = new Bitmap("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\\girl16x16.png");

            //load img to picturebox1
            pictureBox1.Image = Image.FromFile("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\\girl16x16.png");

            int width = bmp.Width;
            int heigh = bmp.Height;

            Color p;
            for (int y = 0; y < heigh; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // get pixel value
                    p = bmp.GetPixel(x, y);

                    //extract file component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    // find average
                    int agv = (r + g + b) / 3;
                    if (agv != 0)
                    {
                        int ad = x;
                    }
                    bmp.SetPixel(x, y, Color.FromArgb(a, agv, agv, agv));
                }
            }
            pictureBox2.Image = bmp;
            bmp.Save("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\\girl16x16_out.png");
        }

        Bitmap bmpIn;
        Bitmap bmpOut;
        Bitmap bmpExport;
        Bitmap bmpGiaiNen;
        int[][] arrOut;
        int[][] arrExport;
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.ImageLocation = openFileDialog.FileName;
                    bmpIn = new Bitmap(openFileDialog.FileName);
                }
                txtImportPath.Text = openFileDialog.FileName;
            }
            
            catch (Exception)
            {
                MessageBox.Show("lỗi chọn tệp");
                throw;
            }
        }

        private void btnNen_Click(object sender, EventArgs e)
        {
            //setDGV(dgvIn, getBitmap(bmpIn));
             getArrayList();
            a = b = null;
            arrOut = null;
            m = 0;
            index = 0;
            bmpOut = null;
            arrExport = null;
            bmpExport = null;
        }

        private void setDGV(DataGridView dgv, int[][] array)
        {
            int n = array.Length;
            dgv.ColumnCount = n;
            dgv.RowCount = n;
            int row = dgv.RowCount;
            int column = dgv.ColumnCount;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    dgv.Rows[i].Height = 50;
                    dgv.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    dgv.Rows[i].Cells[j].Value = array[i][j].ToString();
                }
            }
        }

        private int[][] getBitmap(Bitmap bmp)
        {
            int n = bmp.Width;
            int[][] data = new int[n][];
            for (int i = 0; i < n; i++)
                data[i] = new int[n];

            Color p;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    p = bmp.GetPixel(i, j);
                    int r = p.R;
                    //int g = p.G;
                    //int b = p.B;

                    //// find average
                    //int agv = (r + g + b) / 3;

                    data[i][j] = r;
                }
            }

            return data;
        }

        double m = 0;
        double[] a;
        double[] b;

        int index = 0;
        private int[][] nen(int[][] data)
        {
            double tbX = 0, tbbpX = 0, fi = 0, q = 0;
            int n = 4;
            int[][] result = new int[n][];
            for (int i = 0; i < n; i++)
                result[i] = new int[n];

            double tong = 0, tbTong = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tong += data[i][j];
                    tbTong += Math.Pow(data[i][j], 2);
                }
            }

            m = n * n;
            tbX = Math.Round(tong / m, 0, MidpointRounding.AwayFromZero);
            tbbpX = Math.Round(tbTong / m, 0, MidpointRounding.AwayFromZero);
            fi = Math.Pow(Math.Abs(tbbpX - Math.Pow(tbX, 2)), 0.5);
            fi = Math.Round(fi, 1, MidpointRounding.AwayFromZero);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (data[i][j] > tbX)
                    {
                        q++;
                        result[i][j] = 1;
                    }
                    else
                        result[i][j] = 0;
                }


            a[index] = tbX - fi * Math.Sqrt(q / (m - q));
            a[index] = Math.Round(a[index], 0);
            b[index] = tbX + fi * Math.Sqrt((m - q) / q);
            b[index] = Math.Round(b[index], 0);

            index++;

            return result;
        }
        private int[][] getArray(int[][] data, int startI, int endI, int startY, int endY)
        {
            int[][] array = new int[4][];
            int k = 0, l = 0;
            for (int i = 0; i < 4; i++)
                array[i] = new int[4];
            for (int i = startI; i <= endI; i++)
            {
                for (int j = startY; j <= endY; j++)
                    array[k][l++] = data[i][j];
                l = 0;
                k++;
            }
            array = nen(array);

            return array;
        }

        private List<int[][]> getArrayList()
        {
            List<int[][]> arrayList = new List<int[][]>();
            int[][] array = getBitmap(bmpIn);
            int startI = 0, endI = 0, startY = 0, endY = 0;

            a = new double[1000000];
            b = new double[1000000];
            for (int i = 0; i < 256; i++)
            {
                a[i] = b[i] = 0;
            }
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if((j + 1) % 4 == 0)
                    {
                        startY = j - 3;
                        endY = j;
                        startI = i;
                        endI = i + 3;
                        arrayList.Add(getArray(array, startI, endI, startY, endY));
                    }         
                }
                i = endI;
            }

            //Add vô dgvIn
            dgvIn.ColumnCount = array.Length;
            dgvIn.RowCount = array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    dgvIn.Rows[i].Height = 50;
                    dgvIn.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    dgvIn.Rows[i].Cells[j].Value = array[i][j].ToString();
                }
            }

            //Add vô dgvOut
            dgvOut.ColumnCount = array.Length;
            dgvOut.RowCount = array.Length;
            arrOut = new int[array.Length][];
            arrExport = new int[array.Length][];
            for (int i = 0; i < array[0].Length; i++)
            {
                arrOut[i] = new int[array.Length];
                arrExport[i] = new int[array.Length];
            }

            int jj = 0, kk = 0;
            int dem = 0;
            for (int i = 0; i < arrayList.Count; i++)
            {
                for (int j = 0; j < arrayList[i].Length; j++)
                {
                    for (int k = 0; k < arrayList[i][j].Length; k++)
                    {
                        if (arrayList[i][k][j] == 0)
                        {
                            arrOut[jj][kk] = int.Parse(a[i].ToString());
                        }
                        else
                        {
                            arrOut[jj][kk] = int.Parse(b[i].ToString());
                        }
                        arrExport[jj][kk] = arrayList[i][k][j];
                        jj++;
                    }
                    if (kk == array.Length - 1)
                    {
                        kk = 0;
                        dem = dem + 4;
                    }
                    else
                    {
                        kk++;
                        jj = dem;
                    }
                }
            }
            
            bmpOut = new Bitmap(array.Length, array.Length);
            bmpExport = new Bitmap(array.Length, array.Length);
            for (int i = 0; i < arrOut.Length; i++)
            {
                for (int j = 0; j < arrOut[i].Length; j++)
                {
                    try
                    {
                        arrOut[i][j].ToString();
                        dgvOut.Rows[i].Cells[j].Value = "";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("loi");
                    }

                    dgvOut.Rows[i].Height = 50;
                    dgvOut.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    dgvOut.Rows[i].Cells[j].Value = arrOut[i][j].ToString();
                    if (arrOut[i][j] > 255)
                        arrOut[i][j] = 0;
                    if (arrOut[i][j] <= 0)
                        arrOut[i][j] = 200;
                    bmpOut.SetPixel(i, j, Color.FromArgb(arrOut[i][j], arrOut[i][j], arrOut[i][j]));
                    bmpExport.SetPixel(i, j, Color.FromArgb(arrExport[i][j], arrExport[i][j], arrExport[i][j]));
                }
            }
            pictureBox2.Image = bmpOut;
            //bmpExport.Save("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\test\\compress\\compress_img.png");
            //pictureBox2.Image = bmpExport;
            //bmpExport.Save("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\\out.png");
            bmpExport.Save("D:\\NAM 4 KY 1\\CSDL_DPT\\BTC - Copy\\Ảnh\\test\\compress\\compress.png");
            MessageBox.Show("Nén thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return arrayList;
        }

        private void giaiNen()
        {
            int n = bmpGiaiNen.Width;
            int[][] data = new int[n][];
            for (int i = 0; i < n; i++)
                data[i] = new int[n];

            Color p;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    p = bmpGiaiNen.GetPixel(i, j);
                    int r = p.R;
                    //int g = p.G;
                    //int b = p.B;

                    //// find average
                    //int agv = (r + g + b) / 3;

                    data[i][j] = r;
                }
            }
        }
        
    }
}
