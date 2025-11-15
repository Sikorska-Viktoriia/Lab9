using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab9.os
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Клас CaseTransistors згідно з лекцією
        public class CaseTransistors
        {
            public string transType { get; set; } // тип транзистора
            public string transName { get; set; } // назва транзистора
            public string transModelName { get; set; } // назва математичної моделі транзистора
            transPrefixName[] Prefixs; // масив для зберігання правильних префіксів імен транзисторів
            CaseTransistors[] transistors; // масив для зберігання транзисторів
            public int Length; // кількість транзисторів у масиві
            public int ErrorKod; // код завершення операції записування або читання

            // Структура transPrefixName, яка призначена для оголошення префіксів імен транзисторів
            struct transPrefixName
            {
                public string PrefixName;  // ім'я префіксу
                public string PrefixText;  // пояснення значення префіксу
            }

            // Конструктор класу CaseTransistors
            public CaseTransistors(int size, string type, string tname, string modelName)
            {
                transistors = new CaseTransistors[size]; // створюємо масив
                Length = size;
                setPrefixName(); // заповнюємо масив структур префіксів
                transType = type;
                transName = tname;
                transModelName = modelName;
            }

            // Перевизначений метод для видачі інформації про транзистор
            public override string ToString()
            {
                return " Транзистор " + transName + " Тип- " + transType + " модель- " + transModelName;
            }

            // Заповнення масиву префіксів
            void setPrefixName()
            {
                Prefixs = new transPrefixName[14];
            
                Prefixs[0].PrefixName = "AC"; Prefixs[0].PrefixText = "Germanium small-signal AF transistor AC126";
                Prefixs[1].PrefixName = "AD"; Prefixs[1].PrefixText = "Germanium AF power transistor AD133";
                Prefixs[2].PrefixName = "AF"; Prefixs[2].PrefixText = "Germanium small-signal RF transistor AF117";
                Prefixs[3].PrefixName = "AL"; Prefixs[3].PrefixText = "Germanium RF power transistor ALZ10";
                Prefixs[4].PrefixName = "AS"; Prefixs[4].PrefixText = "Germanium switching transistor ASY28";
                Prefixs[5].PrefixName = "AU"; Prefixs[5].PrefixText = "Germanium power switching transistor AU103";
                Prefixs[6].PrefixName = "BC"; Prefixs[6].PrefixText = "Silicon, small signal transistor  BC548B";
                Prefixs[7].PrefixName = "BD"; Prefixs[7].PrefixText = "Silicon, power transistor BD139";
                Prefixs[8].PrefixName = "BF"; Prefixs[8].PrefixText = "Silicon, RF (high frequency) BJT or FET BF245";
                Prefixs[9].PrefixName = "BS"; Prefixs[9].PrefixText = "Silicon, switching transistor (BJT or MOSFET) BS170";
                Prefixs[10].PrefixName = "BL"; Prefixs[10].PrefixText = "Silicon, high frequency, high power (for transmitters) BLW34";
                Prefixs[11].PrefixName = "BU"; Prefixs[11].PrefixText = "Silicon, high voltage (for CRT horizontal deflection circuits) BU508";
                Prefixs[12].PrefixName = "CF"; Prefixs[12].PrefixText = "Gallium Arsenide small-signal Microwave transistor (MESFET) CF300";
                Prefixs[13].PrefixName = "CL"; Prefixs[13].PrefixText = "Gallium Arsenide Microwave power transistor (FET) CLY10";
            }

            // Метод для визначення правильності префіксу
            bool OkPrefixName(string prefix)
            {
                for (int i = 0; i < 14; i++)
                {
                    if (Prefixs[i].PrefixName == prefix) return true;
                }
                return false;
            }

            // Метод для визначення правильності індексу
            bool OkIndex(int i)
            {
                if (i >= 0 && i < Length) return true;
                else return false;
            }

            // Індексатор для класу CaseTransistors
            public CaseTransistors this[int index]
            {
                get      // вибрати об'єкт типу транзистор з індексом index
                {
                    if (OkIndex(index)) // якщо правильний індекс
                    {
                        ErrorKod = 0;
                        return transistors[index];
                    }
                    else
                    {
                        ErrorKod = 1;  // якщо індекс не правильний
                        return null;
                    }
                }
                set    // записати транзистор у масив
                {
                    // Перевіряємо індекс
                    if (!OkIndex(index))
                    {
                        ErrorKod = 1;
                        return;
                    }
                    // Перевіряємо префікс імені
                    if (!OkPrefixName(value.transName.Substring(0, 2)))
                    {
                        ErrorKod = 2; // Якщо префікс не правильний
                        return;
                    }
                    // Якщо індекс та ім'я правильні, записуємо
                    transistors[index] = value;
                    ErrorKod = 0;
                }
            }
        }

        // Порожній метод Load залишаємо, якщо він не використовується.
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Метод обробки натискання кнопки button1_Click
        private void button1_Click(object sender, EventArgs e)
        {
            // Створюємо екземпляри класу CaseTransistors
            // Головний масив (на 5 елементів)
            CaseTransistors MyTr = new CaseTransistors(5, "Bipolar", "AC126", "EbbersMoll");

            // Транзистори для додавання
            CaseTransistors MyTr1 = new CaseTransistors(1, "Field-effet", "AC126", "Gummel-Poon");
            CaseTransistors MyTr2 = new CaseTransistors(1, "Field-effet", "AD133", "Gummel-Poon");
            CaseTransistors MyTr3 = new CaseTransistors(1, "Schottky", "BD139", "Gummel-Poon");
            CaseTransistors MyTr4 = new CaseTransistors(1, "Avalanche", "OO117", "EbbersMoll"); // Неправильний префікс 'OO'
            CaseTransistors MyTr5 = new CaseTransistors(1, "Darlington", "BLW34", "EbbersMoll");
            CaseTransistors MyTr6 = new CaseTransistors(1, "Photo", "BU508", "EbbersMoll"); // Спроба додати за індексом 5 (поза межами [0..4])
            CaseTransistors MyTr7 = new CaseTransistors(1, "Bipolar", "CLY10", "EbbersMoll"); // Спроба додати за індексом 6 (поза межами [0..4])

            // Поміщаємо створені транзистори у масив MyTr з допомогою індексатора.
            string sMessage = " ";

            // 1
            MyTr[0] = MyTr1;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 1 Транзистор не додано {MyTr1.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 1 Транзистор додано {MyTr1.transName} ";

            // 2
            MyTr[1] = MyTr2;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 2 Транзистор не додано {MyTr2.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 2 Транзистор додано {MyTr2.transName} ";

            // 3
            MyTr[2] = MyTr3;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 3 Транзистор не додано {MyTr3.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 3 Транзистор додано {MyTr3.transName} ";

            // 4 (Перевірка на неправильний префікс: "OO117")
            MyTr[3] = MyTr4;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 4 Транзистор не додано {MyTr4.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 4 Транзистор додано {MyTr4.transName} ";

            // 5
            MyTr[4] = MyTr5;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 5 Транзистор не додано {MyTr5.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 5 Транзистор додано {MyTr5.transName} ";

            // 6 (Перевірка на неправильний індекс: 5, розмір масиву 5, індекси [0..4])
            MyTr[5] = MyTr6;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 6 Транзистор не додано {MyTr6.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 6 Транзистор додано {MyTr6.transName} ";

            // 7 (Перевірка на неправильний індекс: 6)
            MyTr[6] = MyTr7;
            if (MyTr.ErrorKod > 0) sMessage += $"\n 7 Транзистор не додано {MyTr7.transName} код помилки -{MyTr.ErrorKod}";
            else sMessage += $"\n 7 Транзистор додано {MyTr7.transName} ";

            // Виведення інформації про хід роботи (з кодами помилок)
            label1.Text = sMessage;

            // Виведення інформації про фактично додані транзистори
            sMessage = "";
            for (int i = 0; i < MyTr.Length; i++)
            {
                // Використання індексатора в get-блоці
                if (MyTr[i] != null) sMessage += "\n " + MyTr[i].ToString();
            }
            label2.Text = sMessage;
        }
    }
}