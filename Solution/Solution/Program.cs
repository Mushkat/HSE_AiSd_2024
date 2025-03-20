using FinanceModule.Domain.Services;
using FinanceModule.Application.Commands;
using FinanceModule.Application.Decorators;
using FinanceModule.Infrastructure.Exporters;
using FinanceModule.Domain.Models;
using FinanceModule.Infrastructure.Importers;
using YamlDotNet.Core.Tokens;


namespace FinanceModule.Console
{
    using System;
    public class Program
    {
        private static BankAccountFacade accountFacade = new BankAccountFacade();
        private static CategoryFacade categoryFacade = new CategoryFacade();
        private static OperationFacade operationFacade = new OperationFacade(accountFacade);

        public void CreateAccount(BankAccountFacade facade, string name, decimal balance)
        {
            var command = new CreateAccountCommand(facade, name, balance);
            var timedCommand = new TimedCommandDecorator<BankAccount>(command);
            var account = timedCommand.Execute();
            Console.WriteLine($"Счет создан! ID: {account.Id}, Баланс: {account.Balance} ₽");
        }

        public void CreateCategory(CategoryFacade facade, string name, string type)
        {
            var command = new CreateCategoryCommand(facade, name, type);
            var timedCommand = new TimedCommandDecorator<Category>(command);
            var category = timedCommand.Execute();
            Console.WriteLine($"Категория создана! ID: {category.Id}, Тип: {category.Type}");
        }

        public void CreateOperation(OperationFacade operationFacade, BankAccountFacade accountFacade, string type, int accountId, decimal amount, DateTime date, int categoryId, string description)
        {
            var command = new CreateOperationCommand(operationFacade, type, accountId, amount, date, categoryId, description);
            var timedCommand = new TimedCommandDecorator<Operation>(command);
            var operation = timedCommand.Execute();
            Console.WriteLine($"Операция создана! Сумма: {operation.Amount} ₽");
        }

        static void Main(string[] args)
        {
            bool isRunning = true;
            try
            {
                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine("=== Учет финансов ВШЭ-Банк ===");
                    Console.WriteLine("1. Создать новый счет");
                    Console.WriteLine("2. Создать категорию");
                    Console.WriteLine("3. Добавить операцию");
                    Console.WriteLine("4. Показать все счета");
                    Console.WriteLine("5. Показать все категории");
                    Console.WriteLine("6. Показать все операции");
                    Console.WriteLine("7. Экспортировать данные");
                    Console.WriteLine("8. Импортировать данные");
                    Console.WriteLine("9. Подсчет разницы доходов и расходов за период");
                    Console.WriteLine("10. Пересчитать баланс счета");
                    Console.WriteLine("11. Удалить счет");
                    Console.WriteLine("12. Удалить категорию");
                    Console.WriteLine("13. Удалить операцию");
                    Console.WriteLine("14. Выход");
                    Console.Write("Выберите действие: ");

                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            CreateAccount();
                            break;
                        case "2":
                            CreateCategory();
                            break;
                        case "3":
                            CreateOperation();
                            break;
                        case "4":
                            ShowAccounts();
                            WaitForUser();
                            break;
                        case "5":
                            ShowCategories();
                            WaitForUser();
                            break;
                        case "6":
                            ShowOperations();
                            WaitForUser();
                            break;
                        case "7":
                            ExportData();
                            break;
                        case "8":
                            ImportData();
                            break;
                        case "9":
                            CalculateBalanceDifference();
                            break;
                        case "10":
                            UpdateBalance();
                            break;
                        case "11":
                            DeleteAccount();
                            break;
                        case "12":
                            DeleteCategory();
                            break;
                        case "13":
                            DeleteOperation();
                            break;
                        case "14":
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Неверный ввод! Нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
                Console.WriteLine("Что-то пошло не так, перезапустите программу");
            }
        }

        static void CreateAccount()
        {
            Console.Write("Введите название счета: ");
            var name = Console.ReadLine();

            decimal balance = ReadDecimal("Введите начальный баланс: ");

            var command = new CreateAccountCommand(accountFacade, name, balance);
            var timedCommand = new TimedCommandDecorator<BankAccount>(command);
            var account = timedCommand.Execute();

            Console.WriteLine($"Счет создан! ID: {account.Id}, Баланс: {account.Balance} $");
            WaitForUser();
        }

        static void CreateCategory()
        {
            Console.Write("Введите название категории: ");
            var name = Console.ReadLine();

            Console.Write("Выберите тип (Income/Expense): ");
            var type = Console.ReadLine();

            var command = new CreateCategoryCommand(categoryFacade, name, type);
            var timedCommand = new TimedCommandDecorator<Category>(command);
            var category = timedCommand.Execute();

            Console.WriteLine($"Категория создана! ID: {category.Id}, Тип: {category.Type}");
            WaitForUser();
        }

        static void CreateOperation()
        {
            // Выбор счета
            ShowAccounts();
            int accountId = ReadInt("Введите ID счета: ");

            // Выбор категории
            ShowCategories();
            int categoryId = ReadInt("Введите ID категории: ");

            // Ввод данных операции
            Console.Write("Выберите тип операции (Income/Expense): ");
            var type = Console.ReadLine();

            decimal amount = ReadDecimal("Введите сумму: ");

            Console.Write("Введите описание: ");
            var description = Console.ReadLine();

            var command = new CreateOperationCommand(
                operationFacade,
                type,
                accountId,
                amount,
                DateTime.Now,
                categoryId,
                description
            );
            var timedCommand = new TimedCommandDecorator<Operation>(command);
            var operation = timedCommand.Execute();

            Console.WriteLine($"Операция создана! Сумма: {operation.Amount} $");
            Console.WriteLine($"Новый баланс счета: {accountFacade.GetAccountById(accountId).Balance} $");
            WaitForUser();
        }

        static void ShowAccounts()
        {
            Console.WriteLine("\nСписок счетов:");
            foreach (var acc in accountFacade.GetAccounts())
            {
                Console.WriteLine($"[ID: {acc.Id}] {acc.Name} | Баланс: {acc.Balance} $");
            }
        }

        static void ShowCategories()
        {
            Console.WriteLine("\nСписок категорий:");
            foreach (var cat in categoryFacade.GetCategories())
            {
                Console.WriteLine($"[ID: {cat.Id}] {cat.Name} ({cat.Type})");
            }
        }

        static void ShowOperations()
        {
            Console.WriteLine("\nСписок операций:");
            foreach (var op in operationFacade.GetOperations())
            {
                Console.WriteLine($"[ID: {op.Id}] {op.Date:dd.MM.yyyy} | {op.Description} | {op.Amount} $");
            }
        }

        static void CalculateBalanceDifference()
        {
            DateTime startDate = ReadDate("Введите начальную дату (гггг-мм-дд): ");
            DateTime endDate = ReadDate("Введите конечную дату (гггг-мм-дд): ");

            decimal difference = operationFacade.CalculateBalanceDifference(startDate, endDate);
            Console.WriteLine($"Разница между доходами и расходами за период: {difference} $");
            WaitForUser();
        }

        static void UpdateBalance()
        {
            ShowAccounts();
            int accountId = ReadInt("Введите ID счета: ");
            decimal newBalance = ReadDecimal("Введите новый баланс: ");

            accountFacade.UpdateBalance(accountId, newBalance);
            Console.WriteLine($"Баланс счета обновлен! Новый баланс: {newBalance} $");
            WaitForUser();
        }

        static void DeleteAccount()
        {
            ShowAccounts();
            int accountId = ReadInt("Введите ID счета для удаления: ");

            accountFacade.DeleteAccount(accountId);

            // Удаляем все операции, связанные с этим счетом
            var operationsToRemove = operationFacade.GetOperations()
                .Where(op => op.BankAccountId == accountId)
                .ToList();

            foreach (var operation in operationsToRemove)
            {
                operationFacade.DeleteOperation(operation.Id);
            }

            Console.WriteLine($"Счет и все связанные операции удалены!");
            WaitForUser();
        }

        static void DeleteCategory()
        {
            ShowCategories();
            int categoryId = ReadInt("Введите ID категории для удаления: ");

            categoryFacade.DeleteCategory(categoryId);

            // Удаляем все операции, связанные с этой категорией
            var operationsToRemove = operationFacade.GetOperations()
                .Where(op => op.CategoryId == categoryId)
                .ToList();

            foreach (var operation in operationsToRemove)
            {
                operationFacade.DeleteOperation(operation.Id);
            }

            Console.WriteLine($"Категория и все связанные операции удалены!");
            WaitForUser();
        }

        static void DeleteOperation()
        {
            ShowOperations();
            int operationId = ReadInt("Введите ID операции для удаления: ");

            operationFacade.DeleteOperation(operationId);
            Console.WriteLine($"Операция удалена, баланс счета обновлен!");
            WaitForUser();
        }

        static decimal ReadDecimal(string message)
        {
            decimal result;
            do
            {
                Console.Write(message);
            } while (!decimal.TryParse(Console.ReadLine(), out result));
            return result;
        }

        static int ReadInt(string message)
        {
            int result;
            do
            {
                Console.Write(message);
            } while (!int.TryParse(Console.ReadLine(), out result));
            return result;
        }

        static DateTime ReadDate(string message)
        {
            DateTime result;
            do
            {
                Console.Write(message);
            } while (!DateTime.TryParse(Console.ReadLine(), out result));
            return result;
        }

        static void WaitForUser()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ExportData()
        {
            Console.WriteLine("Выберите формат экспорта:");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. YAML");
            var format = Console.ReadLine();
            var resFormat = format switch
            {
                "1" => "csv",
                "2" => "json",
                "3" => "yaml"
            };

            IDataExporter exporter = format switch
            {
                "1" => new CsvExporter(),
                "2" => new JsonExporter(),
                "3" => new YamlExporter(),
                _ => throw new ArgumentException("Неверный формат")
            };

            foreach (var account in accountFacade.GetAccounts())
                exporter.Export(account);

            foreach (var category in categoryFacade.GetCategories())
                exporter.Export(category);

            foreach (var operation in operationFacade.GetOperations())
                exporter.Export(operation);

            string filePath = $"../../../../ExportResults/export.{resFormat}";
            File.WriteAllText(filePath, exporter.GetResult());
            Console.WriteLine($"Данные экспортированы в {filePath}");
            WaitForUser();
        }
        static void ImportData()
        {
            Console.WriteLine("Выберите формат импорта:");
            Console.WriteLine("1. CSV");
            Console.WriteLine("2. JSON");
            Console.WriteLine("3. YAML");
            var format = Console.ReadLine();

            DataImporter importer = format switch
            {
                "1" => new CsvDataImporter(accountFacade, categoryFacade, operationFacade),
                "2" => new JsonDataImporter(accountFacade, categoryFacade, operationFacade),
                "3" => new YamlDataImporter(accountFacade, categoryFacade, operationFacade),
                _ => throw new ArgumentException("Неверный формат")
            };

            Console.Write("Введите путь к файлу: ");
            var filePath = Console.ReadLine();

            importer.Import(filePath);
            Console.WriteLine("Данные успешно импортированы!");
            WaitForUser();
        }
    }
}