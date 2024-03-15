using JD_Proc.Model;
using System.Diagnostics;

namespace JD_Proc.DocImageConvert
{
    public static class DataConvertFormat
    {
        public static void ConvertBmpToCsv(string bmpFilePath, string csvFilePath)
        {
            using (Bitmap bmp = new Bitmap(bmpFilePath))
            using (StreamWriter sw = new StreamWriter(csvFilePath))
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color pixelColor = bmp.GetPixel(x, y);
                        //string csvLine = $"{pixelColor.R},{pixelColor.G},{pixelColor.B}";
                        string csvLine = ((pixelColor.R + pixelColor.G + pixelColor.B) / 3).ToString();
                        if (y == bmp.Height) sw.Write(csvLine);
                        else sw.Write(csvLine + ",");
                    }
                    sw.WriteLine();
                }
            }

        }

        public static List<List<double>> ConvertCSVToDLList(string csvFilePath, int Width, int Height)
        {

            StreamReader sr = new StreamReader(csvFilePath);
            var data = new List<List<double>>();

            if (File.Exists(csvFilePath))
            {
                int row = 0;

                while (!sr.EndOfStream)
                {
                    data.Add(new List<double>());

                    string line = sr.ReadLine();
                    string[] EachCSVdata = line.Split(',');

                    for (int x = 0; x < 640; x++)
                    {
                        data[row].Add(double.Parse(EachCSVdata[x]));
                    }

                    row = row + 1;
                }

                sr.Close();

                return data;
            }

            else return null;

        }



    }
}
