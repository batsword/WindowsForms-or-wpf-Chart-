using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HighChart1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        //WriteableBitmap wBitmap;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //mychart.Init();

            int width = (int)chart.ActualWidth;
            int height = (int)chart.ActualHeight;
            if (width > 0 && height > 0)
            {
                DisplayImage.Width = width;
                DisplayImage.Height = height;

                //wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);
                //DisplayImage.Source = wBitmap;

                graphEdit = new GraphChart(width, height, boardColor);

                graphEdit.HorizontalMargin = 50;                                   //横水平边距

                graphEdit.VerticalMargin = 80;                                     //竖垂直边距

                graphEdit.AreasColor = System.Drawing.Color.FromArgb(100, 0, 0, 0);         //画图区域颜色

                graphEdit.GraphColor = System.Drawing.Color.FromArgb(255, 110, 176);        //曲线面积颜色

                graphEdit.AxisColor = System.Drawing.Color.FromArgb(255, 255, 255);         //坐标轴颜色

                graphEdit.ScaleColor = System.Drawing.Color.FromArgb(20, 255, 255, 255);          //刻度线颜色



                graphEdit.XScaleCount = 24;          //X轴刻度线数量

                graphEdit.YScaleCount = 10;          //Y轴刻度线数量
            }



        }

        System.Windows.Point[] textPoint = new System.Windows.Point[11];    //  纵坐标的刻度值的坐标，对应每个刻度线
        public string[] yScaleText = new string[11]; //  纵坐标的刻度值（字符），对应每个刻度线

        System.Windows.Media.Pen solidWideBluePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 1);     // 画曲线的画笔
        System.Windows.Media.Pen solidSmallYellowPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Yellow, 1); // 画栅格的画笔
        System.Windows.Media.Pen solidMidYellowPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Yellow, 0.5); // 画栅格的画笔
        public System.Windows.Point[] linePoint = new System.Windows.Point[XPoints];        // 数据数组，记录了每个数据点的坐标值，是屏幕显示的曲线值。但不包括控件外框和栅格之间的距离信息，外框和栅格的偏移由画线的过程处理。
        public const int YScalePixel = 50;                    // 每个栅格的像素
        public const int XPoints = 1801;                      // 数据点的个数，也就是X轴的长度
        public int validPointNum = 0;                         // 有效数据点的个数。
        private const int TextPointOffsetX = 5;                 // 定义纵坐标刻度值的X坐标偏移值
        private const int TextPointOffsetY = 18;               // 定义纵坐标刻度值的Y坐标偏移值

        private const int GridStartX = 50;                     // 定义栅格起始X坐标。
        private const int GridStartY = 25;                     // 定义栅格起始Y坐标。
        int graticule_y;
        private readonly object _drawLocker = new object();
        private void RandomChart_Click(object sender, RoutedEventArgs e)
        {
            //lock (_drawLocker)
            //{
            //    int width = (int)OutCanvas.ActualWidth;
            //    int height = (int)OutCanvas.ActualHeight;

            //    WriteableBitmap wBitmap;
            //    wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);


            //    wBitmap.Lock();
            //    Bitmap backBitmap = new Bitmap(width, height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, wBitmap.BackBuffer);

            //    Graphics graphics = Graphics.FromImage(backBitmap);
            //    graphics.Clear(System.Drawing.Color.White);//整张画布置为白色

            //    System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
            //    System.Drawing.Point endPoint = new System.Drawing.Point(0, 0);
            //    for (int i = 0; i <= 10; i++)
            //    {
            //        startPoint.X = GridStartX - 1;
            //        startPoint.Y = graticule_y;
            //        endPoint.X = GridStartX - 1 + XPoints;
            //        endPoint.Y = graticule_y;
            //        graticule_y += YScalePixel;
            //        solidSmallYellowPen.DashStyle = DashStyles.Dot;
            //        graphics.DrawLine(Pens.Red, startPoint, endPoint);

            //    }

            //    //画一些随机线
            //    Random rand = new Random();
            //    for (int i = 0; i < 100; i++)
            //    {
            //        int x1 = rand.Next(width);
            //        int x2 = rand.Next(width);
            //        int y1 = rand.Next(height);
            //        int y2 = rand.Next(height);
            //        graphics.DrawLine(Pens.Red, x1, y1, x2, y2);
            //    }

            //    graphics.Flush();
            //    graphics.Dispose();
            //    graphics = null;

            //    backBitmap.Dispose();
            //    backBitmap = null;

            //    wBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            //    wBitmap.Unlock();
            //    DisplayImage.Source = wBitmap;
            //}
        }

        #region 实时曲线

        GraphChart graphEdit;
        System.Drawing.Color boardColor = System.Drawing.Color.FromArgb(17, 81, 138);//指定绘制图的背景色  
        Thread toUpdate;                               //刷新线程
        private void RealTimeChart_Click(object sender, RoutedEventArgs e)
        {

            

            toUpdate = new Thread(new ThreadStart(Run));

            toUpdate.Start();
        }

        float XRange = 1440;   //X轴最大范围（0-1440）

        float YRange = 500;    //Y轴最大范围（0-500）
        private void Run()

        {

            while (true)

            {
                PrintToChart();
                //Image image = graphEdit.GetCurrentGraph(this.GetBaseData(), XRange, YRange, false);  //如果是面积曲线图将最后一个参数设为true

                //Graphics g = chart.CreateGraphics();  //指定使用那个控件来接受曲线图

                //g.DrawImage(image, 0, 0);

                //g.Dispose();

                Thread.Sleep(50);                 //每2秒钟刷新一次  

            }

        }
        WriteableBitmap wBitmap;

        private void PrintToChart()
        {
            
            
            lock (_drawLocker)
            {
                Dispatcher.Invoke(new Action(()=> {

                    int width = (int)chart.ActualWidth;
                    int height = (int)chart.ActualHeight;


                    wBitmap = new WriteableBitmap(width, height, 72, 72, PixelFormats.Bgr24, null);


                    wBitmap.Lock();
                    Bitmap backBitmap = new Bitmap(width, height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, wBitmap.BackBuffer);

                    Graphics graphics = Graphics.FromImage(backBitmap);
                    graphics.Clear(System.Drawing.Color.White);//整张画布置为白色


                    //-----------------------画图内容-------------------------start


                    

                    System.Drawing.Image image = graphEdit.GetCurrentGraph(this.GetBaseData(), XRange, YRange, false);  //如果是面积曲线图将最后一个参数设为true
                    graphics.DrawImage(image,0,0);

                    //-----------------------画图内容-------------------------end


                    graphics.Flush();
                    graphics.Dispose();
                    graphics = null;

                    backBitmap.Dispose();
                    backBitmap = null;

                    wBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                    wBitmap.Unlock();


                    DisplayImage.Source = wBitmap;


                }));
            }
        }

        private List<System.Drawing.Point> GetBaseData()

        {

            Random r = new Random();

            List<System.Drawing.Point> result = new List<System.Drawing.Point>();  //数据

            for (int i = 0; i < XRange - 200; i += 30)

            {

                System.Drawing.Point p;

                if (i < 100)

                    p = new System.Drawing.Point(i, r.Next(180, 200));

                else

                    p = new System.Drawing.Point(i, r.Next(200, 220));

                result.Add(p);

            }

            return result;

        }

        #endregion

        private void RealTimeChartbyControl_Click(object sender, RoutedEventArgs e)
        {
            new Thread(()=> {
                while (true)
                {
                    Dispatcher.Invoke(new Action(()=> {
                        mychart.DrawCurve();
                    }));
                    
                    Thread.Sleep(50);
                }
            }).Start();
        }
    }
}
