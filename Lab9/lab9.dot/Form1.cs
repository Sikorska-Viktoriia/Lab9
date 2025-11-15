using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab9.dot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ==========================================================
        // КЛАСИ ТА ІНДЕКСАТОР ДЛЯ ЗАВДАННЯ "ЕЛЕКТРОННА БІБЛІОТЕКА"
        // ==========================================================

        // 1. Клас для зберігання інформації про книгу
        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year { get; set; }

            public Book(string title, string author, int year)
            {
                Title = title;
                Author = author;
                Year = year;
            }

            public override string ToString()
            {
                return $"'{Title}' (Автор: {Author}, Рік: {Year})";
            }
        }

        // 2. Клас-контейнер, що реалізує індексатор
        public class BookLibrary
        {
            private Book[] books;
            public int Length;
            // ОНОВЛЕНО: 0 - ОК, 1 - Некоректний індекс, 2 - Некоректна назва, 3 - Книга вже існує
            public int ErrorCode = 0;

            public BookLibrary(int size)
            {
                books = new Book[size];
                Length = size;
            }

            // Допоміжний метод для перевірки індексу
            private bool IsValidIndex(int index)
            {
                return index >= 0 && index < Length;
            }

            // НОВА ЛОГІКА: Перевірка на унікальність назви
            private bool IsTitleUnique(string title, int currentIndex)
            {
                if (string.IsNullOrEmpty(title)) return false; // Порожня назва завжди вважається не унікальною

                for (int i = 0; i < Length; i++)
                {
                    // Перевіряємо лише зайняті елементи і виключаємо елемент, який ми, можливо, перезаписуємо
                    if (i != currentIndex && books[i] != null && books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        return false; // Знайдено дублікат
                    }
                }
                return true; // Назва унікальна
            }

            // ІНДЕКСАТОР
            public Book this[int index]
            {
                // *** GET-блок (читання/отримання) ***
                get
                {
                    if (IsValidIndex(index))
                    {
                        ErrorCode = 0;
                        return books[index];
                    }
                    else
                    {
                        ErrorCode = 1; // Помилка: індекс поза межами
                        return null;
                    }
                }

                // *** SET-блок (запис/встановлення) ***
                set
                {
                    // 1. Перевірка індексу
                    if (!IsValidIndex(index))
                    {
                        ErrorCode = 1;
                        return;
                    }

                    // 2. Перевірка логіки 1: чи не пуста назва книги
                    if (string.IsNullOrEmpty(value.Title))
                    {
                        ErrorCode = 2; // Помилка 2: некоректна назва
                        return;
                    }

                    // 3. НОВА ПЕРЕВІРКА: Унікальність назви
                    if (!IsTitleUnique(value.Title, index))
                    {
                        ErrorCode = 3; // Помилка 3: книга з такою назвою вже є в бібліотеці
                        return;
                    }

                    // 4. Успішний запис
                    books[index] = value;
                    ErrorCode = 0;
                }
            }
        }


        // ==========================================================
        // ЛОГІКА ФОРМИ
        // ==========================================================
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Ініціалізація бібліотеки на 5 книг
            BookLibrary myLibrary = new BookLibrary(5);
            string sMessageLog = "--- Лог операцій додавання (ErrorCode 3 = Дублікат назви) ---\n";

            // 2. Створення об'єктів Book для тестування
            Book book1 = new Book("Собор", "Олесь Гончар", 1968);
            Book book2 = new Book("Чорний Ворон", "Василь Шкляр", 2009);
            Book book3 = new Book("1984", "Джордж Оруелл", 1949);
            Book bookInvalidName = new Book("", "Невідомий", 2000);
            Book bookDuplicate = new Book("Собор", "Олесь Гончар", 1968); // Дублікат book1
            Book book4 = new Book("Тіні забутих предків", "Михайло Коцюбинський", 1911);

            // 3. Тестування SET-блоку індексатора (запис)

            // Запис 1 (Індекс 0): Успішно
            myLibrary[0] = book1;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n1. Додано: {book1.Title}" : $"\n1. Помилка {myLibrary.ErrorCode} при додаванні {book1.Title}";

            // Запис 2 (Індекс 1): Успішно
            myLibrary[1] = book2;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n2. Додано: {book2.Title}" : $"\n2. Помилка {myLibrary.ErrorCode} при додаванні {book2.Title}";

            // Запис 3 (Індекс 2): Невдача (Помилка 2: Некоректна назва)
            myLibrary[2] = bookInvalidName;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n3. Додано: {bookInvalidName.Title}" : $"\n3. Помилка {myLibrary.ErrorCode} при додаванні (порожня назва)";

            // Запис 4 (Індекс 3): Невдача (Помилка 3: Дублікат назви "Собор")
            myLibrary[3] = bookDuplicate;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n4. Додано: {bookDuplicate.Title}" : $"\n4. Помилка {myLibrary.ErrorCode} при додаванні {bookDuplicate.Title} (дублікат)";

            // Запис 5 (Індекс 4): Успішно
            myLibrary[4] = book4;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n5. Додано: {book4.Title}" : $"\n5. Помилка {myLibrary.ErrorCode} при додаванні {book4.Title}";

            // Запис 6 (Індекс 5): Невдача (Помилка 1: Індекс поза межами [0..4])
            myLibrary[5] = book3;
            sMessageLog += (myLibrary.ErrorCode == 0) ? $"\n6. Додано: {book3.Title}" : $"\n6. Помилка {myLibrary.ErrorCode} при додаванні {book3.Title} (індекс 5)";

            // Виведення логу операцій у label1
            label1.Text = sMessageLog;

            // 4. Тестування GET-блоку індексатора (читання)
            string sMessageBooks = "\n--- Книги в Бібліотеці (читання індексатором) ---\n";
            for (int i = 0; i < myLibrary.Length + 1; i++)
            {
                // Виклик GET-блоку: myLibrary[i]
                Book currentBook = myLibrary[i];
                if (currentBook != null)
                {
                    sMessageBooks += $"\nКнига [{i}]: {currentBook.ToString()}";
                }
                else
                {
                    // Фіксуємо помилку, якщо get-блок її повернув
                    sMessageBooks += $"\nКнига [{i}]: NULL (Код помилки: {myLibrary.ErrorCode})";
                }
            }

            // Виведення вмісту бібліотеки у label2
            label2.Text = sMessageBooks;
        }
    }
}