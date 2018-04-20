using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMS.Core.Tools
{
    public class CommonTools
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string CreateRandomStr(int length, RandomType randomType = RandomType.Mix)
        {
            var digitArray = new string[] { "2", "3", "4", "5", "6", "7", "8", "9" };//去除0，1
            var letterArray = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };//小写字母，去除l、o
            var capArray = letterArray.Select(i => i.ToUpper());//大写字母
            IList<string> randomArray;
            switch (randomType)
            {
                case RandomType.Digit:
                    randomArray = digitArray;
                    break;
                case RandomType.Letter:
                    randomArray = letterArray.Concat(capArray).ToList();
                    break;
                default:
                    randomArray = digitArray.Concat(letterArray).Concat(capArray).ToList();
                    break;
            }
            var random = new Random();
            var builder = new StringBuilder();
            //生成随机字符串
            for (int i = 0; i < length; i++)
            {
                builder.Append(randomArray[random.Next(randomArray.Count)]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码内容</param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 15.0), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        

        #region excel
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="dropTitle">舍弃标头</param>
        /// <returns></returns>
        public static DataTable ReadExcel(Stream stream, bool dropTitle = true)
        {
            try
            {
                var result = new DataTable();

                var hssfWorkBook = new HSSFWorkbook(stream);

                var sheet = hssfWorkBook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();
                var rowNum = sheet.GetRow(0).LastCellNum;
                for (int j = 0; j < rowNum; j++)
                {
                    result.Columns.Add();
                }
                if (dropTitle)
                {
                    rows.MoveNext();
                }
                while (rows.MoveNext())
                {
                    HSSFRow row = rows.Current as HSSFRow;
                    DataRow dr = result.NewRow();
                    for (int i = 0; i < rowNum; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    result.Rows.Add(dr);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static void WriteExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                if (table.TableName == "" || table.TableName == null)
                {
                    table.TableName = "sheet0";
                }
                ISheet sheet = workbook.CreateSheet(table.TableName);
                //IRow headerRow = sheet.CreateRow(0);
                // handling header.
                //foreach (DataColumn column in table.Columns)
                //    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                // handling value.
                int rowIndex = 0;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return;
            }
        }
        #endregion

        #region reflector
        public static IEnumerable<string> GetFields<T>()
        {
            var type = typeof(T);
            var fields = type.GetFields();
            foreach (var i in fields)
            {
                yield return i.Name;
            }
        }
        #endregion

        #region thread
        public static void ExcuteTask(Func<bool> action)
        {
            var tryTime = 0;
            bool isSuccess = false;
            var task = Task.Factory.StartNew(() =>
            {
                while (!isSuccess)
                {
                    isSuccess = action();
                    if (tryTime > 3)
                    {
                        break;
                    }
                    tryTime++;
                    if (!isSuccess)
                    {
                        Thread.Sleep(5000);
                    }
                }
                return isSuccess;
            });
        }
        #endregion

        #region io
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepaths">要压缩文件路径</param>
        /// <returns></returns>
        public static Stream ZipFiles(params string[] filePaths)
        {
            if (filePaths == null && filePaths.Length == 0)
                return null;
            var zipStream = new MemoryStream();
            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in filePaths)
                {
                    var entry = zip.CreateEntry(Path.GetFileName(item));
                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = new FileStream(item, FileMode.Open))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }
        #endregion

        public enum RandomType
        {
            /// <summary>
            /// 纯数字
            /// </summary>
            Digit,
            /// <summary>
            /// 纯字母
            /// </summary>
            Letter,
            /// <summary>
            /// 混合
            /// </summary>
            Mix
        }
    }
}
