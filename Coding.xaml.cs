using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1
{
    public partial class Coding : Window
    {
        public string code;
        public double bitWidth; //длина бита
        public double amplitude; //амплитуда
        public Coding(string code)
        {
            this.code = code; // последовательность бит, полученная с предыдущего окна
            InitializeComponent();
            bitWidth = 30; //установка желательной длины бита
            amplitude = 30;//установка желательной амплитуды
            xSlider.Value = bitWidth; //присваеваем slider'у значение длины бита
            ySlider.Value = amplitude;//присваеваем slider'у значение амплитуды
            xValue.Text = bitWidth.ToString();//присваеваем textbox'у значение амплитуды
            yValue.Text = amplitude.ToString();//присваеваем textbox'у значение амплитуды
            AllDrawing();
        }
        public void DrawingLines(Canvas canvas)
        {
            //Рисуем вертикальные линии с отступом в длину бита
            double x = 0;
            while (x<bitWidth*code.Length+1) 
            {
                DrawingLine(canvas, x,  2*amplitude, x, 0, Brushes.Red, 1);
                x += bitWidth;
            }            
        }
        private void DrawingText(Canvas canvas, string text, double x)
        {
            double textOffset = -20; // Отступ текста от линии (можно отрегулировать по необходимости)
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
        public void DrawingNRZ(Canvas canvas)
        {
            double x = 0;
            double y = amplitude;// Начальный уровень
            byte state = 0;//состояние автомата
            void DrawRight() //функция для рисования линии вправо
            {
                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                x += bitWidth;//запоминаем положение графика по x
            }            
            void DrawUp() //функция для обработки перехода вверх
            {
                DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                y -= amplitude;//запоминаем положение графика по y
            }
            void DrawDown()//функция для обработки перехода вниз
            {
                DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                y += amplitude;//запоминаем положение графика по y
            }
            for (int i = 0; i < code.Length; i++)
            {
                switch (state)
                {
                    case 0: //график в начальном уровне
                        switch (code[i])
                        {
                            case '0': //когда линия находится в начальном уровне
                                DrawingText(canvas, code[i].ToString(), x + bitWidth / 2);
                                DrawRight();
                                break;
                            case '1':
                                state = 1;// переход в след состояние, т.к. "1" (не начальный уровень)
                                DrawingText(canvas, code[i].ToString(), x + bitWidth / 2);
                                DrawUp();
                                DrawRight();                                
                                break;
                        }
                        break;
                    case 1://график не в начальном уровне
                        switch (code[i])
                        {
                            case '0':// переход в след состояние, т.к. "0" (начальный уровень)
                                state = 0;
                                DrawingText(canvas, code[i].ToString(), x + bitWidth / 2);
                                DrawDown();
                                DrawRight();                                
                                break;
                            case '1':// остается в этом состоянии, т.к. "1" (не начальный уровень)
                                DrawingText(canvas, code[i].ToString(), x + bitWidth / 2);                                
                                DrawRight();                                
                                break;
                        }
                        break;
                }
            }
            DrawingLines(canvas);
        }
        public void DrawingNRZI(Canvas canvas)
        {
            double x = 0;
            double y = amplitude;//начальное положение
            byte state = 0; //состояние автомата            
            void DrawRight()//функция для рисования линии вправо
            {
                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                x += bitWidth;//запоминаем положение графика по x
            }            
            void DrawUp()//функция для рисования линии вверх
            {
                DrawingLine(canvas, x, y, x, y - amplitude, Brushes.Black, 2.5);
                y -= amplitude;//запоминаем положение графика по y
            }
            void DrawDown() //функция для рисования линии вниз
            {
                DrawingLine(canvas, x, y, x, y + amplitude, Brushes.Black, 2.5);
                y += amplitude;//запоминаем положение графика по y
            }
            for (int i = 0; i < code.Length; i++)
            {
                switch (state)
                {
                    /*комментарии аналогичны методу DrawingNRZ. Разница в рисовании линии, когда приходит 1.
                     Автомат реагирует только на приход "1". В случае прихода "1" рисуется вертикальная и горизонтальная линия. Уровень графика определяется состояниями
                    "0 состояние" - остается на начальном уровне. "1 состояние" - остается "уровень выше"*/
                    case 0:                        
                        switch (code[i])
                        {
                            case '0':
                                DrawRight();
                                break;
                            case '1':
                                state = 1;
                                DrawUp();
                                DrawRight();
                                break;
                        }
                        break;
                    case 1:
                        switch (code[i])
                        {
                            case '0':
                                DrawRight();
                                break;
                            case '1':
                                state = 0;
                                DrawDown();
                                DrawRight();
                                break;
                        }
                        break;
                }
            }
            DrawingLines(canvas);
        }
        public void DrawingMC(Canvas canvas)
        {
            double x = 0;
            double y = amplitude;// Начальный уровень
            byte state = 0;//состояние автомата            
            void DrawHalfRight()// Рисует горизонтальную линию длиной в половину bitWidth
            {
                DrawingLine(canvas, x, y, x + bitWidth / 2, y, Brushes.Black, 2.5);
                x += bitWidth / 2; //запоминаем положение графика по x
            }            
            void DrawUpOrDown(bool up)// Рисует вертикальную линию вверх или вниз в зависимости от того, где находится предыдущая точка
            {
                DrawingLine(canvas, x, y, x, up ? y - amplitude : y + amplitude, Brushes.Black, 2.5);//если true, то вверх, иначе вниз
                y = up ? y - amplitude : y + amplitude;//сохранение позиции в зависимости от up
            }            
            void DrawFullRight()// Рисует полную горизонтальную линию (полный бит)
            {
                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                x += bitWidth;//запоминаем положение графика по x
            }
            for (int i = 0; i < code.Length; i++)
            {
                switch (state)
                {
                    case 0: //задающее состояние. Начало графика зависит от 1-го бита.
                        switch (code[i])
                        {
                            case '0':
                                y -= amplitude; //начало графика выше середины графика
                                state = 1;
                                DrawHalfRight();
                                break;
                            case '1':
                                y += amplitude;//начало графика ниже середины графика
                                state = 2;
                                DrawHalfRight();
                                break;
                        }
                        break;
                    case 1: // Обработка линии, начинающейся сверху
                        /*Если предыдущий бит совпадает с текущим, то код синхронизируется, т.е. рисуется 2 половины 
                        интервала бита (в результате график остается на прежнем уровне)*/
                        switch (code[i])
                        {
                            case '0':
                                if (code[i - 1] == '0')
                                {
                                    DrawUpOrDown(false); // Вниз
                                    DrawHalfRight(); // половина вправо
                                    DrawUpOrDown(true); // Вверх
                                    DrawHalfRight();// половина вправо
                                }
                                else // code[i - 1] == '1'
                                {
                                    DrawUpOrDown(false); // Вниз
                                    DrawFullRight();//вправо
                                    state = 2;
                                }
                                break;
                            case '1':
                                if (code[i - 1] == '0')
                                {
                                    DrawUpOrDown(false); // Вниз
                                    DrawFullRight();//вправо
                                    state = 2;
                                }
                                else // code[i - 1] == '1'
                                {
                                    DrawUpOrDown(false); // Вниз
                                    DrawHalfRight();// половина вправо
                                    DrawUpOrDown(true); // Вверх
                                    DrawHalfRight();// половина вправо
                                }
                                break;
                        }
                        break;
                    case 2: // Обработка линии, начинающейся снизу (аналогично case 1, но наоборот)
                        switch (code[i])
                        {
                            case '0':
                                if (code[i - 1] == '0')
                                {
                                    DrawUpOrDown(true); // Вверх
                                    DrawHalfRight();// половина вправо
                                    DrawUpOrDown(false); // Вниз
                                    DrawHalfRight();// половина вправо
                                }
                                else // code[i - 1] == '1'
                                {
                                    DrawUpOrDown(true); // Вверх
                                    DrawFullRight();// вправо
                                    state = 1;
                                }
                                break;
                            case '1':
                                if (code[i - 1] == '0')
                                {
                                    DrawUpOrDown(true); // Вверх
                                    DrawFullRight();// вправо
                                    state = 1;
                                }
                                else // code[i - 1] == '1'
                                {
                                    DrawUpOrDown(true); // Вверх
                                    DrawHalfRight();// половина вправо
                                    DrawUpOrDown(false); // Вниз
                                    DrawHalfRight();// половина вправо
                                }
                                break;
                        }
                        break;
                }
            }
            //Вырисовка линий для окончания графика (в зависимости от положения y)
            if ((y == amplitude) || (y == 2 * amplitude))
            {
                DrawUpOrDown(true);
                DrawHalfRight();
            }
            else
            {
                DrawUpOrDown(false);
                DrawHalfRight();
            }
            DrawingLines(canvas);
        }
        public void DrawingAMI(Canvas canvas)
        {
            double x = 0;
            double y = amplitude; // Начальный уровень
            bool potential = false; //потенциал единицы (может быть отрицательным, либо положительным)
            byte state = 0;//состояние автомата            
            void DrawRight()//линия вправо
            {
                DrawingLine(canvas, x, y, x + bitWidth, y, Brushes.Black, 2.5);
                x += bitWidth;//запоминаем положение графика по x
            }            
            void DrawUpOrDown(bool _potential)//вертикальный переход на верхний или нижний уровень (в зависимости от потенциала)
            {
                DrawingLine(canvas, x, y, x, _potential ? 2 * amplitude : 0, Brushes.Black, 2.5);
                y = _potential ? 2 * amplitude : 0; //запоминаем положение графика по y (в зависимости от потенциала)
            }            
            void DrawToNeutral()// Возвращает линию в исходное положение
            {
                DrawingLine(canvas, x, y, x, amplitude, Brushes.Black, 2.5);
                y = amplitude; //запоминаем положение графика по y
            }
            for (int i = 0; i < code.Length; i++)
            {
                switch (state)
                {
                    case 0://график на начальном уровне
                        switch (code[i])
                        {
                            case '0':
                                DrawRight();
                                break;
                            case '1':
                                state = 1;
                                DrawUpOrDown(potential);
                                DrawRight();
                                potential = !potential; // Меняем потенциал
                                break;
                        }
                        break;
                    case 1://график не в начальном уровне
                        switch (code[i])
                        {
                            case '0':
                                state = 0;
                                DrawToNeutral();
                                DrawRight();
                                break;
                            case '1':
                                DrawUpOrDown(potential);
                                DrawRight();
                                potential = !potential;
                                break;
                        }
                        break;
                }
            }
            DrawToNeutral();//окончание графика
            DrawingLines(canvas);
        }
        private void AllDrawing()
        {
            //Очищаем canvas'ы
            NRZ.Children.Clear();
            NRZI.Children.Clear();
            MC.Children.Clear();
            AMI.Children.Clear();
            //рисуем сигналы NRZ-, NRZI-, MC- и AMI-кодировок
            DrawingNRZ(NRZ);
            DrawingNRZI(NRZI);
            DrawingMC(MC);
            DrawingAMI(AMI);
        }
        private void ySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //при изменении slider'а присваеваем textbox'у и амплитуде значение slider'а. Перерисовываем графики
            yValue.Text = ySlider.Value.ToString();
            amplitude = ySlider.Value;
            AllDrawing();
        }
        private void xSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //при изменении slider'а присваеваем textbox'у и длине бита значение slider'а. Перерисовываем графики
            xValue.Text = xSlider.Value.ToString();
            bitWidth = xSlider.Value;
            AllDrawing();
        }
    }
}