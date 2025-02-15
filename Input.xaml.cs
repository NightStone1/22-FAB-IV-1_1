using System.Windows;
using System.Windows.Input;

namespace Lab1
{
    public partial class Input : Window
    {
        public string bit;
        public Input()
        {
            InitializeComponent();
        }
        private void Coding_Click(object sender, RoutedEventArgs e)
        {
            //Переходим с последовательностью битов в следующую форму
            if (Bin.Text.Length == 0) 
            {
                MessageBox.Show("Последовательность бит отсутствует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            { 
                Coding coding = new Coding(bit);
                coding.ShowDialog();
            }
        }
        private void GetCode_Click(object sender, RoutedEventArgs e)
        {
            //Получение последовательности бит через ввод данных
            if ((LstName.Text.Length < 1)&&(Name.Text.Length < 1) && (NmbGrp.Text.Length < 1) && (NmbJ.Text.Length < 1)) 
            {
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            {
                LstName.Text.Replace(" ", ""); //удаление пробелов
                Name.Text.Replace(" ", "");//удаление пробелов
                string binG = Convert.ToString(int.Parse(NmbGrp.Text), 2); //Преобразование числа в двоичную строку
                string binJ = Convert.ToString(int.Parse(NmbJ.Text), 2);// Преобразование числа в двоичную строку
                string binL = Convert.ToString(LstName.Text.Length, 2); // Преобразование длины строки в двоичную строку
                string binN = Convert.ToString(Name.Text.Length, 2);   // Преобразование длины строки в двоичную строку
                //pad добавляет 0 слева до нужного кол-ва символов
                bit = binG.PadLeft(6,'0') + binJ.PadLeft(5, '0') + "000000000" + binL.PadLeft(4, '0') + binN.PadLeft(4, '0');
                BinNmbGrp.Text = binG;
                BinNmbJ.Text = binJ;
                BinName.Text = binN;
                BinLstName.Text = binL;
                Bin.Text = bit;
            }
        }
        private void Int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //разрешение ввода только цифр
            e.Handled = !IsDigit(e.Text);
        }
        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //разрешение ввода только букв
            e.Handled = !IsLetter(e.Text);
        }
        private bool IsLetter(string text)
        {
            return text.All(char.IsLetter);
        }
        private bool IsDigit(string text)
        {
            return text.All(char.IsDigit);
        }
        private void GetTestCode_Click(object sender, RoutedEventArgs e)
        {
            //тестовая последовательность бит для сверки с кодом, данным в лабораторной работе
            bit = "0100101011100100011";
            Bin.Text = bit;
        }
    }
}