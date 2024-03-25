using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc.Service
{
    public class BlobService
    {
        public BlobService() { }

        public List<Model.Blob> FindBlobs(Bitmap image, int thresholdValue)
        {

            List<Model.Blob> blobs = new List<Model.Blob>();
            int width = image.Width;
            int height = image.Height;
            bool[,] visited = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    
                        if (!visited[x, y] && image.GetPixel(x, y).R > thresholdValue)
                        {
                            Model.Blob blob = ExtractBlob(image, visited, x, y, thresholdValue);
                            blobs.Add(blob);
                        }
                    
                }
            }

            return blobs;
        }

        Model.Blob ExtractBlob(Bitmap image, bool[,] visited, int startX, int startY, int thresholdValue)
        {
            Queue<System.Drawing.Point> queue = new Queue<System.Drawing.Point>();
            queue.Enqueue(new System.Drawing.Point(startX, startY));
            visited[startX, startY] = true;

            int minX = startX, minY = startY, maxX = startX, maxY = startY;

            while (queue.Count > 0)
            {
                System.Drawing.Point point = queue.Dequeue();
                int x = point.X, y = point.Y;

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);

                // 주변 픽셀을 확인하여 연결된 영역을 찾습니다.
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int newX = x + dx;
                        int newY = y + dy;

                        if (newX >= 0 && newX < image.Width && newY >= 0 && newY < image.Height && !visited[newX, newY] && image.GetPixel(newX, newY).R > thresholdValue)
                        {
                            queue.Enqueue(new System.Drawing.Point(newX, newY));
                            visited[newX, newY] = true;
                        }
                    }
                }
            }

            int blobWidth = maxX - minX + 1;
            int blobHeight = maxY - minY + 1;

            return new Model.Blob(minX, minY, blobWidth, blobHeight);
        }
    }
}
