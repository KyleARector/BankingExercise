using System.Collections.Generic;
using System.Linq;

namespace Banking
{
    // Enumeration of various possible account types
    public enum AcctType
    {
        PERSONAL_CHECKING,
        INDV_INVESTMENT,
        CORP_INVESTMENT
    }

    // Bank class that requires a name
    // Contains a dictionary with accounts belonging to the bank
    // Has method to tranfer balance between two Account objects
    public class Bank
    {
        // Fields
        public string Name { get; }

        // Constructor
        // Requires string bank_name 
        public Bank(string bank_name)
        {
            Name = bank_name;
            accounts = new Dictionary<int, Account>();
        }

        public void CreateAccount(string owner, AcctType acct_type)
        {
            // Instantiate a new account
            Account new_acct;
            // Find the next available account number
            int acct_number = GetUniqueAcctNum();
            // Evaluate account type
            if (acct_type == AcctType.PERSONAL_CHECKING)
                new_acct = new CheckingAccount(acct_number, owner);
            else if (acct_type == AcctType.INDV_INVESTMENT)
                new_acct = new IndvInvestmentAccount(acct_number, owner);
            else if (acct_type == AcctType.CORP_INVESTMENT)
                new_acct = new CorpInvestmentAccount(acct_number, owner);
            else
                throw new System.ArgumentException("Invalid account type requested.");
            // Add the account to the dictionary if it is valid
            AddAccount(new_acct);
        }

        // Remove an account from the bank's dictionary
        // Requires an Account object to remove
        public void RemoveAccount(int acct_num_to_remove)
        {
            // If the account exists, remove it from dictionary
            if (!accounts.ContainsKey(acct_num_to_remove))
                throw new System.ArgumentException("Invalid account number requested.");
            accounts.Remove(acct_num_to_remove);
        }

        // Retrieve an Account object
        // Requires a matching int acct_number
        public Account GetAccountByNumber(int acct_number)
        {
            if (!accounts.ContainsKey(acct_number))
                throw new System.ArgumentException("Invalid account number requested.");
            return accounts[acct_number];
        }

        // Transfer funds into another account
        // Requires a double amount and Account object
        public void Transfer(double amount, Account send_acct, Account recv_acct)
        {
            // Use regular methods and exceptions
            send_acct.Withdraw(amount);
            recv_acct.Deposit(amount);
        }

        // Accounts in dictionary for quick look up
        // Also supports changing key type
        private Dictionary<int, Account> accounts;

        // Add a new account to the bank's dictionary
        // Requires an Account object to add
        private void AddAccount(Account new_acct)
        {
            accounts.Add(new_acct.Number, new_acct);
        }

        private int GetUniqueAcctNum()
        {
            // Get latest account number
            if (accounts.Count() == 0)
                return 1;
            return accounts.Keys.Max() + 1;
        }
    }

    // Abstract bank account class
    // Requires an owner 
    // Maintains the balance of the account, and has methods to withdraw and deposit
    public abstract class Account
    {
        public int Number { get; }
        public string Owner { get; }
        public double Balance { get; private set; }

        // Constructor
        // Requires a int acct_number and string owner
        public Account(int acct_number, string owner)
        {
            Number = acct_number;
            Owner = owner;
        }

        // Withdraw funds from account
        // Requires a double amount
        public virtual void Withdraw(double amount)
        {
            // Do not allow overdraft
            if (amount > Balance)
                throw new System.InvalidOperationException("Account can not be overdrawn.");
            else
                Balance -= amount;
        }

        // Deposit funds into account
        // Requires a double amount
        public virtual void Deposit(double amount)
        {
            Balance += amount;
        }
    }

    // Child Account class to support Checking account
    public class CheckingAccount : Account
    {
        // Default constructor
        public CheckingAccount(int acct_number, string owner) : base(acct_number, owner) { }
    }

    // Child Account class to support Individual Investment account
    // Overrides base Withdraw method with additional conditional criteria
    public class IndvInvestmentAccount : Account
    {
        // Default constructor
        public IndvInvestmentAccount(int acct_number, string owner) : base(acct_number, owner) { }

        // Withdraw funds from account
        // Override from base class
        // Requires a double amount
        public override void Withdraw(double amount)
        {
            // Do not allow overdraft or over $1000.00
            if (amount > 1000.00)
                throw new System.InvalidOperationException("Can only withdraw $1000.00 per transaction.");
            base.Withdraw(amount);
        }
    }

    // Child Account class to support Corporate Investment account
    public class CorpInvestmentAccount : Account
    {
        // Default constructor
        public CorpInvestmentAccount(int acct_number, string owner) : base(acct_number, owner) { }
    }

}
