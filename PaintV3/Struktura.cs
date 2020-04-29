using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace PaintV3
{

    [XmlRoot]
    public class Struktura
    {
        public string FileName { get; set; }

        //public Point[][] Slika { get; set; }

        public string Strokes { get; set; }

        public Struktura()
        {

        }
        
    }
}
