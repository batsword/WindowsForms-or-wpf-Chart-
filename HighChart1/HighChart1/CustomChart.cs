using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:HighChart1"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:HighChart1;assembly=HighChart1"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomChart/>
    ///
    /// </summary>
    public class CustomChart : Control
    {
        static CustomChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomChart), new FrameworkPropertyMetadata(typeof(CustomChart)));
        }

        public CustomChart()
        {
            Init();
        }


        GraphChart graphEdit;
        System.Drawing.Color boardColor = System.Drawing.Color.FromArgb(17, 81, 138);//指定绘制图的背景色  
        float XRange = 1440;   //X轴最大范围（0-1440）

        float YRange = 500;    //Y轴最大范围（0-500）
        public void Init()
        {
            int width = (int)this.ActualWidth;
            int height = (int)this.ActualHeight;
            if (width > 0 && height > 0)
            {

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
        System.Windows.Media.Pen solidWideBluePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 1);     // 画曲线的画笔

        System.Drawing.Image _Image;

        public void DrawCurve()
        {
            Init();
            _Image = graphEdit.GetCurrentGraph(this.GetBaseData(), XRange, YRange, false);  //如果是面积曲线图将最后一个参数设为true
            this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            

            //System.Drawing.Image image = graphEdit.GetCurrentGraph(this.GetBaseData(), XRange, YRange, false);  //如果是面积曲线图将最后一个参数设为true
            if(_Image!=null)
                drawingContext.DrawImage(GetImageStream(_Image), new Rect(0, 0,this.ActualWidth,this.ActualHeight) );

            //drawingContext.DrawLine(solidWideBluePen,new System.Windows.Point(0,0),new System.Windows.Point(50,50));
        }

        #region 图片转换

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);
        public static BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        public static System.Windows.Media.ImageSource ConvertDrawingImage2MediaImageSource(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                return bitmap;
            }
        }
        #endregion

        private List<System.Drawing.Point> GetBaseData()

        {

            Random r = new Random();

            List<System.Drawing.Point> result = new List<System.Drawing.Point>();  //数据

            for (int i = 0; i < 2000; i++)
            {
                System.Drawing.Point p;

                p = new System.Drawing.Point(i, r.Next(180, 200));

                result.Add(p);
            }

            //for (int i = 0; i < XRange - 200; i += 30)

            //{

            //    System.Drawing.Point p;

            //    if (i < 100)

            //        p = new System.Drawing.Point(i, r.Next(180, 200));

            //    else

            //        p = new System.Drawing.Point(i, r.Next(200, 220));

            //    result.Add(p);

            //}

            return result;

        }
    }
}
