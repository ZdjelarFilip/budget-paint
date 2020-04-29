using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

namespace PaintV3
{
    public partial class MainWindow : Window
    {

        private bool JeShranjen { get; set; }

        private Struktura Struktura { get; set; }
        public MainWindow()
        {
            
            
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; // height pa Width max screen size
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            BrushColorCombo.ItemsSource = typeof(Colors).GetProperties();
            PropertyInfo[] colors = BrushColorCombo.ItemsSource.Cast<PropertyInfo>().ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].Name == "Black") // to changi na color Settings pa dela
                {
                    BrushColorCombo.SelectedIndex = i;
                    break;
                }
            }

            JeShranjen = true;
            Struktura = new Struktura();


            BrushSlider.Value = Properties.Settings.Default.BrushSize;

            string dictionary = "";
            if (Properties.Settings.Default.Theme == "Dark")
                dictionary = "DarkTheme.xaml";
            else
                dictionary = "Default.xaml";

            var dict = new Uri(dictionary, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            { 
                Source = dict 
            });




            Color selectedColor1 = new Color();

            byte R = Properties.Settings.Default.colorR;
            byte G = Properties.Settings.Default.colorG;
            byte B = Properties.Settings.Default.colorB;
            //MessageBox.Show(R + " " + B + " " + G);
            selectedColor1 = Color.FromRgb(R, G, B);
            PaintCanvas.DefaultDrawingAttributes.Color = selectedColor1;
            Properties.Settings.Default.Save();
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PaintCanvas == null) return;

            var drawingAttributes = PaintCanvas.DefaultDrawingAttributes;
            drawingAttributes.Width = BrushSlider.Value;
            drawingAttributes.Height = BrushSlider.Value;
            PaintCanvas.EraserShape = new RectangleStylusShape(BrushSlider.Value, BrushSlider.Value); 
            var previousEditingMode = PaintCanvas.EditingMode;
            PaintCanvas.EditingMode = InkCanvasEditingMode.None;
            PaintCanvas.EditingMode = previousEditingMode;

            Properties.Settings.Default.BrushSize = (int)BrushSlider.Value;
            Properties.Settings.Default.Save();
        }



        private void BrushColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (BrushColorCombo.IsDropDownOpen)
            {
                Color selectedColor = (Color)(BrushColorCombo.SelectedItem as PropertyInfo).GetValue(null, null);
                PaintCanvas.DefaultDrawingAttributes.Color = selectedColor;
                Properties.Settings.Default.colorR = PaintCanvas.DefaultDrawingAttributes.Color.R;
                Properties.Settings.Default.colorG = PaintCanvas.DefaultDrawingAttributes.Color.G;
                Properties.Settings.Default.colorB = PaintCanvas.DefaultDrawingAttributes.Color.B;
                Properties.Settings.Default.Save();
            }
            
        }

        private void BrushStateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaintCanvas == null) return;

          switch (BrushStateCombo.SelectedIndex)
            {
                case 0:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case 1:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case 2:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case 3:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
            }
        }



        private void BrushShapesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaintCanvas == null) return;

            switch (BrushShapesCombo.SelectedIndex)
            {
                case 0:
                    PaintCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
                    break;
                case 1:
                    PaintCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Rectangle;
                    break;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e) //nevem zakaj je zakomentiran. za vsak slucaj nebom zbrisu, mogoc rabim kdaj :)
        {

            //BitmapSource visual_BitmapSource = get_BitmapSource_of_Element(imgCubeWhite);
            //CroppedBitmap cb = new CroppedBitmap(visual_BitmapSource, new Int32Rect((int)Mouse.GetPosition(imgCubeWhite).X, (int)Mouse.GetPosition(imgCubeWhite).Y, 1, 1));
            //byte[] pixels = new byte[4];
            //try
            //{
            //    cb.CopyPixels(pixels, 4, 0);
            //}
            //catch (Exception)
            //{
            //    //error
            //}
            //rectSelected.Fill = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));
           
            //PaintCanvas.EditingMode = InkCanvasEditingMode.Ink;
           

            //Brush newColor = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));
            //SolidColorBrush scb = (SolidColorBrush)newColor;
            //Color col = scb.Color;


            //PaintCanvas.DefaultDrawingAttributes.Color = col;
        }



        private void slider_Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)

        {

           

            //change Cube from White to Black

            if (rectColor_Background != null)
            {

                double imgOpacity = slider_Color.Value;

                imgCubeWhite.Opacity = imgOpacity;

            }

            

        }



        private void imgCubeWhite_MouseMove(object sender, MouseEventArgs e)
        {
            select_Color(sender, e);
        }





        private void select_Color(object sender, MouseEventArgs e)
        {
            try
            {

                BitmapSource visual_BitmapSource = get_BitmapSource_of_Element(imgCubeWhite);

                CroppedBitmap cb = new CroppedBitmap(visual_BitmapSource, new Int32Rect((int)Mouse.GetPosition(imgCubeWhite).X, (int)Mouse.GetPosition(imgCubeWhite).Y, 1, 1));

                byte[] pixels = new byte[4];

                try
                {
                    cb.CopyPixels(pixels, 4, 0);
                }
                catch (Exception)
                {
                }

                rectSelected.Fill = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));

                Properties.Settings.Default.colorR = PaintCanvas.DefaultDrawingAttributes.Color.R;
                Properties.Settings.Default.colorG = PaintCanvas.DefaultDrawingAttributes.Color.G;
                Properties.Settings.Default.colorB = PaintCanvas.DefaultDrawingAttributes.Color.B;
                Properties.Settings.Default.Save();
            }

            catch (Exception)

            {
            }

        }


        public BitmapSource get_BitmapSource_of_Element(FrameworkElement element)

        {

            if (element == null) { return null; }

            // init 

            double dpi = 96;

            Double width = element.ActualWidth;

            Double height = element.ActualHeight;

            RenderTargetBitmap bitmap_of_Element = null;

            if (bitmap_of_Element == null)

            {

                try

                {

                    // create empty Bitmap of element 

                    bitmap_of_Element = new RenderTargetBitmap((int)width, (int)height, dpi, dpi, PixelFormats.Default);

                    



                    // render area into bitmap 

                    DrawingVisual visual_area = new DrawingVisual();

                    using (DrawingContext dc = visual_area.RenderOpen())

                    {

                        VisualBrush visual_brush = new VisualBrush(element);

                        dc.DrawRectangle(visual_brush, null, new Rect(new Point(), new Size(width, height)));

                    }

                     // render 

                    bitmap_of_Element.Render(visual_area);

                }

                catch (Exception ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            return bitmap_of_Element;

        }

        private void imgCubeWhite_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BitmapSource visual_BitmapSource = get_BitmapSource_of_Element(imgCubeWhite);
            CroppedBitmap cb = new CroppedBitmap(visual_BitmapSource, new Int32Rect((int)Mouse.GetPosition(imgCubeWhite).X, (int)Mouse.GetPosition(imgCubeWhite).Y, 1, 1));
            byte[] pixels = new byte[4];
            try
            {
                cb.CopyPixels(pixels, 4, 0);
            }
            catch (Exception)
            {
                //error
            }
            rectSelected.Fill = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));

            


            Brush newColor = new SolidColorBrush(Color.FromRgb(pixels[2], pixels[1], pixels[0]));
            SolidColorBrush scb = (SolidColorBrush)newColor;
            Color col = scb.Color;


            PaintCanvas.DefaultDrawingAttributes.Color = col;
            PaintCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML-File | *.xml";
            dialog.ShowDialog();

            Struktura struktura = new Struktura();
            struktura.FileName = dialog.FileName;
            XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
            try
            {
                using (FileStream fileStream = new FileStream(struktura.FileName, FileMode.Create))
                {
                    serializer.Serialize(fileStream, struktura);
                }
                SetupSlika(struktura.FileName);
            }
            catch (Exception)
            {

               //error
            }
            

            
        }

        private void SetupSlika(string path)
        {
            //Struktura struktura = new Struktura();
            //XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
            //try
            //{
            //    using (FileStream fileStream = new FileStream(path, FileMode.Open))
            //    {
            //        struktura = (Struktura)serializer.Deserialize(fileStream);
            //    }

            //    Struktura = struktura;
            //    if (Struktura.Slika == null)
            //    {
            //        struktura.Slika = new Point[0][];
            //    }

            //    for (int i = 0; i < Struktura.Slika.Length; i++)
            //    {
            //        if (struktura.Slika[i] != null)
            //        {
            //            StylusPointCollection stylusCollection = new
            //              StylusPointCollection(struktura.Slika[i]);

            //            Stroke stroke = new Stroke(stylusCollection);
            //            StrokeCollection strokes = new StrokeCollection();
            //            strokes.Add(stroke);

            //            PaintCanvas.Strokes.Add(strokes);
            //        }
            //    }
            //}
            //catch (Exception)
            //{


            //}


            Struktura struktura = new Struktura();
            XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    struktura = (Struktura)serializer.Deserialize(fileStream);
                }

                Struktura = struktura;

                StrokeCollectionConverter strokeCollectionConverter = new StrokeCollectionConverter();
                PaintCanvas.Strokes = (StrokeCollection)strokeCollectionConverter.ConvertFromString(Struktura.Strokes);
            }
            catch (Exception)
            {


            }

        }

        private void SaveSlika()
        {
            //var strokes = PaintCanvas.Strokes;


            //if (strokes.Count > 0)
            //{
            //    Struktura.Slika = new Point[strokes.Count][];

            //    for (int i = 0; i < Struktura.Slika.Length; i++)
            //    {
            //        Struktura.Slika[i] =
            //          new Point[this.PaintCanvas.Strokes[i].StylusPoints.Count];


            //        for (int j = 0; j < strokes[i].StylusPoints.Count; j++)
            //        {
            //            Struktura.Slika[i][j] = new Point();
            //            Struktura.Slika[i][j].X =
            //                                  strokes[i].StylusPoints[j].X;
            //            Struktura.Slika[i][j].Y =
            //                                  strokes[i].StylusPoints[j].Y;
            //        }
            //    }

            //    XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
            //    //Struktura.Slika = new Point[PaintCanvas.Strokes.Count][];
            //    using (FileStream fileStream = new FileStream(Struktura.FileName, FileMode.Create))
            //    {
            //        serializer.Serialize(fileStream, Struktura);
            //    }
            //}

            //JeShranjen = true;

            var strokes = PaintCanvas.Strokes;

            StrokeCollectionConverter strokeCollectionConverter = new StrokeCollectionConverter();
            string str = strokeCollectionConverter.ConvertToString(strokes);


            if (strokes.Count > 0)
            {
                Struktura.Strokes = str;

                XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
                //Struktura.Slika = new Point[PaintCanvas.Strokes.Count][];
                using (FileStream fileStream = new FileStream(Struktura.FileName, FileMode.Create))
                {
                    serializer.Serialize(fileStream, Struktura);
                }
            }

            JeShranjen = true;

        }




        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Struktura.FileName == null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "XML-File | *.xml";
                dialog.ShowDialog();

                Struktura struktura = new Struktura();
                struktura.FileName = dialog.FileName;
                XmlSerializer serializer = new XmlSerializer(typeof(Struktura));
                using (FileStream fileStream = new FileStream(struktura.FileName, FileMode.Create))
                {
                    serializer.Serialize(fileStream, struktura);
                }// :D

                Struktura.FileName = struktura.FileName;
            }

            SaveSlika();
        }

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            PaintCanvas.Strokes.Clear();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-File | *.xml";
            dialog.ShowDialog();

            Struktura struktura = new Struktura();
            struktura.FileName = dialog.FileName;

            SetupSlika(struktura.FileName);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("You sure you want to close the application?", "Closing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (!JeShranjen)
                {

                    if (MessageBox.Show("You want to Save the Picture", "Save", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        SaveSlika();
                    }
                }
            }
            else
                e.Cancel = true;

        }

        private void PaintCanvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            JeShranjen = false;
        }

        private void PaintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            JeShranjen = false;
        }

        private void PaintCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            JeShranjen = false;
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow Settings = new SettingsWindow();
            Settings.ShowDialog();

        }
    }
}

