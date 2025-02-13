using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab1
{
    public partial class Coding : Window
    {
        public string code;
        public Coding(string code)
        {
            InitializeComponent();
            this.code = code; // последовательность бит, полученная с предыдущего окна
            double bitWidth = 30; //длина бита
            double amplitude = 30; //амплитуда
            DrawingNRZ(NRZ, code, bitWidth, amplitude); //рисуем сигнал NRZ-кодирования
            DrawingNRZI(NRZI, code, bitWidth, amplitude);//рисуем сигнал NRZI-кодирования
            DrawingMC(MC, code, bitWidth, amplitude);//рисуем сигнал манчестерского кода
            DrawingAMI(AMI, code, bitWidth, amplitude);//рисуем сигнал AMI-кодирования
        }
        public void DrawingLines(Canvas canvas, double bitWidth, double amplitude)
        {
            //Рисуем вертикальные линии с отступом в длину бита по середине графика
            double x = 0;
            double y = amplitude;
            while (x<1000)
            {
                DrawingLine(canvas, x, y + amplitude, x, y - amplitude, Brushes.Red, 1);
                x += bitWidth;
            }            
        }
        private void DrawingText(Canvas canvas, string text, double x)
        {
            double textOffset = -20; // Отступ текста от линии (регулируйте по необходимости)
            TextBlock textBlock = new TextBlock(); //создание textblock'а
            textBlock.Text = text; // присваем textblock'у поступающий в метод текст
            textBlock.FontFamily = new FontFamily("Times New Roman"); //шрифт
            textBlock.FontSize = 16; //размер шрифта
            textBlock.Foreground = Brushes.Black; // Цвет текста
            textBlock.TextAlignment = TextAlignment.Center;
            // Получаем размер текста
            textBlock.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            System.Windows.Size textSize = textBlock.DesiredSize; textSize = textBlock.DesiredSize;
            // Устанавливаем позицию текста, центрируя его по горизонтали
            Canvas.SetLeft(textBlock, x - textSize.Width / 2);
            Canvas.SetTop(textBlock, textOffset);
            canvas.Children.Add(textBlock); //добавляем его на canvas
        }
        public void DrawingLine(Canvas canvas, double x1, double y1, double x2, double y2, Brush color, double thickness)
        {
            Line line = new Line(); // создаем новую линию
            line.X1 = x1; //координата первой точки
            line.Y1 = y1;//координата первой точки
            line.X2 = x2;//координата второй точки
            line.Y2 = y2;//координата второй точки
            line.Stroke = color; // цвет
            line.StrokeThickness = thickness; // Толщина линии
            canvas.Children.Add(line);  //добавляем его на canvas
        }
        public void DrawingNRZ(Canvas canvas, string Code, double bitWidth, double amplitude)
        {            
            double x = 0;
            double y = amplitude; // Начальный уровень
            byte state = 0; //состояние автомата
            for (int i = 0; i < Code.Length; i++)
            {
                switch (state)
                {
                    case 0:
                        switch (Code[i])
                        {
                            case '0': //когда линия находится в начальном уровне
                                state = 0;// остается в этом состоянии, т.к. "0" (начальный уровень)
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);//линия вправо
                                DrawingText(canvas, Code[i].ToString(), x + bitWidth / 2);
                                x += bitWidth;//запоминаем положение графика по x
                                break;
                            case '1':
                                state = 1; // переход в след состояние, т.к. "1" (не начальный уровень)
                                DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);//линия вверх
                                y -= amplitude; //запоминаем положение графика по y
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);//линия вправо
                                DrawingText(canvas, Code[i].ToString(), x + bitWidth / 2); //сверху отображается закодированный символ
                                x += bitWidth;//запоминаем положение графика по x
                                break;
                        }
                        break;
                    case 1: //когда линия находится не в начальном уровне
                        switch (Code[i])
                        {
                            case '0':
                                state = 0;// переход в след состояние, т.к. "0" (начальный уровень)
                                DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5); //линия вниз
                                y += amplitude;//запоминаем положение графика по y
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5); //линия вправо
                                DrawingText(canvas, Code[i].ToString(), x + bitWidth / 2);
                                x += bitWidth;//запоминаем положение графика по x
                                break;
                            case '1':
                                state = 1;// остается в этом состоянии, т.к. "1" (не начальный уровень)
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5); //линия вправо
                                DrawingText(canvas, Code[i].ToString(), x + bitWidth / 2);
                                x += bitWidth;//запоминаем положение графика по x
                                break;
                        }
                        break;                   
                }
            }
            DrawingLines(canvas, bitWidth, amplitude); 
        }
        public void DrawingNRZI(Canvas canvas, string Code, double bitWidth, double amplitude)
        {
            double x = 0; 
            double y = amplitude; // Начальный уровень
            byte state = 0;
            for (int i = 0; i < Code.Length; i++)
            {
                switch (state)
                {
                    /*комментарии аналогичны методу DrawingNRZ. Разница в рисовании линии, когда приходит 1
                     автомат реагирует только на приход "1". В случае прихода "1" рисуется вертикальная и горизонтальная линия. Уровень графика определяется состояниями
                    "0 состояние" - остается на начальном уровне. "1 состояние" - остается "уровень выше"*/
                    case 0:
                        switch (Code[i])
                        {
                            case '0': 
                                state = 0;                                
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                            case '1':
                                state = 1;
                                DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                y -= amplitude;
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                        }
                        break;
                    case 1: //когда линия находится не в начальном уровне
                        switch (Code[i])
                        {
                            case '0':
                                state = 1;
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                            case '1':
                                state = 0;
                                DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                y += amplitude;
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                        }
                        break;                   
                }
            }
            DrawingLines(canvas, bitWidth, amplitude);
        }
        public void DrawingMC(Canvas canvas, string Code, double bitWidth, double amplitude)
        {
            double x = 0;
            double y = amplitude; // Начальный уровень
            byte state = 0;
            for (int i = 0; i < Code.Length; i++)
            {
                switch (state)
                {
                    case 0: //задающее состояние. Начало графиков отличаются в зависимости от 1 бита.
                        switch (Code[i])
                        {
                            case '0': // начальный уровень устанавливается сверху от начального положения
                                state = 1;
                                y -= amplitude;
                                DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                x += bitWidth / 2;
                                break;
                            case '1':
                                state = 2;
                                y += amplitude; // начальный уровень устанавливается снизу от начального положения
                                DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                x += bitWidth / 2;
                                break;
                        }
                        break;
                    case 1://обработка линии начинающейся сверху (следующая линия идет вниз-вправо)
                        switch (Code[i])
                        {
                            case '0'://поступает 0
                                //если предыдущий "0", то код синхронизируется, т.е. рисуется 2 половины интервала бита (в результате график возвращается в начальный уровень)
                                if (Code[i-1] == '0') 
                                {
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                    state = 1;
                                }
                                if (Code[i - 1] == '1') //вырисовывается полный интервал бита
                                {
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                    state = 2;
                                }
                                break;
                            case '1': //поступает 0
                                if (Code[i - 1] == '0') //вырисовывается полный интервал бита
                                {
                                    state = 2;
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                }
                                //если предыдущий "1", то код синхронизируется, т.е. рисуется 2 половины интервала бита (в результате график возвращается в начальный уровень)
                                if (Code[i - 1] == '1')
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                }
                                break;
                        }
                        break;
                        // case 2 по сути аналогичен case 1
                    case 2: //обработка линии начинающейся снизу (следующая линия идет вверх-вправо)
                        switch (Code[i])
                        {
                            case '0':                                
                                if (Code[i - 1] == '0')
                                {
                                    state = 2;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                }
                                if (Code[i - 1] == '1')
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                }
                                break;
                            case '1':                                
                                if (Code[i - 1] == '0')
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                }
                                if (Code[i - 1] == '1')
                                {
                                    state = 2;
                                    DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                                    y -= amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                    DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                                    y += amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                                    x += bitWidth / 2;
                                }
                                break;
                        }
                        break;
                }
            }
            //Вырисовка линий для окончания графика (в зависимости от положения y)
            if ((y == amplitude) || (y == 2*amplitude))
            {
                DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                y -= amplitude;
                DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
            }
            else
            {
                DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                y += amplitude;
                DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
            }
            DrawingLines(canvas, bitWidth, amplitude);
        }
        public void DrawingAMI(Canvas canvas, string Code, double bitWidth, double amplitude)
        {
            double x = 0;
            double y = amplitude; // Начальный уровень
            bool potential = false; //bool переменная для определения потенциала единицы
            byte state = 0;
            for (int i = 0; i < Code.Length; i++)
            {
                switch (state)
                {
                    case 0: //график в начальном уровне
                        switch (Code[i])
                        {
                            case '0': //график остается на начальном уровне
                                state = 0;
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                            case '1':
                                if (potential) // если приходит 1 с положительным потенциалом, то график становится в верхнем положении
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, 2*amplitude, Brushes.Black, 2.5);
                                    y = 2*amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                    potential = false;
                                }
                                else  // иначе, график устанавливается в нижнем положении
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, 0, Brushes.Black, 2.5);
                                    y = 0;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                    potential = true;
                                }
                                break;
                        }
                        break;
                    case 1: //график не в начальном уровне
                        //описание действий по сути аналогично case 0
                        switch (Code[i])
                        {
                            case '0':
                                state = 0;
                                DrawingLine(canvas, x, y, x, amplitude, Brushes.Black, 2.5);
                                y = amplitude;
                                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                x += bitWidth;
                                break;
                            case '1':
                                if (potential)
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, 2 * amplitude, Brushes.Black, 2.5);
                                    y = 2 * amplitude;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                    potential = false;
                                }
                                else
                                {
                                    state = 1;
                                    DrawingLine(canvas, x, y, x, 0, Brushes.Black, 2.5);
                                    y = 0;
                                    DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                                    x += bitWidth;
                                    potential = true;
                                }
                                break;
                        }
                        break;
                }
            }
            DrawingLine(canvas, x, y, x, amplitude, Brushes.Black, 2.5); //окончание графика
            DrawingLines(canvas, bitWidth, amplitude);
        }
    }
}
