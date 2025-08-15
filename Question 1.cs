using System;
using System.Collections.Generic;
using System.Globalization;

public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processed {FormatCurrency(transaction.Amount)} for {transaction.Category}");
    }

    private string FormatCurrency(decimal amount) => 
        amount.ToString("C", new CultureInfo("en-GH"));
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processed {FormatCurrency(transaction.Amount)} for {transaction.Category}");
    }

    private string FormatCurrency(decimal amount) => 
        amount.ToString("C", new CultureInfo("en-GH"));
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processed {FormatCurrency(transaction.Amount)} for {transaction.Category}");
    }

    private string FormatCurrency(decimal amount) => 
        amount.ToString("C", new CultureInfo("en-GH"));
}

public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance.ToString("C", new CultureInfo("en-GH"))}");
        }
    }
}

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var account = new SavingsAccount("GH123456", 1000m);

        var t1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 200m, "Entertainment");

        ITransactionProcessor mobileMoney = new MobileMoneyProcessor();
        mobileMoney.Process(t1);

        ITransactionProcessor bankTransfer = new BankTransferProcessor();
        bankTransfer.Process(t2);

        ITransactionProcessor cryptoWallet = new CryptoWalletProcessor();
        cryptoWallet.Process(t3);

        account.ApplyTransaction(t1);
        account.ApplyTransaction(t2);
        account.ApplyTransaction(t3);

        _transactions.AddRange(new[] { t1, t2, t3 });
    }
}

class Program
{
    static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}.
