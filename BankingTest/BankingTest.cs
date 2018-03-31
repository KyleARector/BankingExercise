using System;
using Banking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingTest
{
    [TestClass]
    public class BankUnitTests
    {
        public Bank SetUpBank()
        {
            Bank bank = new Bank("My Bank");

            bank.CreateAccount("Me", AcctType.PERSONAL_CHECKING);
            bank.CreateAccount("You", AcctType.INDV_INVESTMENT);
            bank.CreateAccount("Jeff", AcctType.CORP_INVESTMENT);

            return bank;
        }

        [TestMethod]
        public void BankInstantiateTest()
        {
            Bank bank = SetUpBank();

            Assert.AreEqual("My Bank", bank.Name);
        }

        [TestMethod]
        public void BankAddAccount()
        {
            Bank bank = SetUpBank();

            Assert.AreEqual(1, bank.GetAccountByNumber(1).Number);
            Assert.AreEqual("Me", bank.GetAccountByNumber(1).Owner);
        }

        [TestMethod]
        public void BankRemoveAccount()
        {
            Bank bank = SetUpBank();

            bank.RemoveAccount(1);

            Assert.AreEqual(2, bank.GetAccountByNumber(2).Number);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid account number requested.")]
        public void BankFailRemoveAccount()
        {
            Bank bank = SetUpBank();

            bank.RemoveAccount(4);
        }

        [TestMethod]
        public void BankDepositChecking()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);

            checking.Deposit(100.00);

            Assert.AreEqual(100.00, checking.Balance);
        }

        [TestMethod]
        public void BankWithdrawChecking()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);

            checking.Deposit(150.00);
            checking.Withdraw(100.00);

            Assert.AreEqual(50.00, checking.Balance);
        }

        [TestMethod]
        public void BankDepositIndvInvestment()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);

            indv_invest.Deposit(100.00);

            Assert.AreEqual(100.00, indv_invest.Balance);
        }

        [TestMethod]
        public void BankWithdrawIndvInvestment()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);

            indv_invest.Deposit(150.00);
            indv_invest.Withdraw(100.00);

            Assert.AreEqual(50.00, indv_invest.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Can only withdraw $1000.00 per transaction.")]
        public void BankTooBigWithdrawIndvInvestment()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);

            indv_invest.Deposit(2000.00);
            indv_invest.Withdraw(1500.00);
        }

        [TestMethod]
        public void BankDespositCorpInvestment()
        {
            Bank bank = SetUpBank();

            Account corp_invest = bank.GetAccountByNumber(3);

            corp_invest.Deposit(100.00);

            Assert.AreEqual(100.00, corp_invest.Balance);
        }

        [TestMethod]
        public void BankWithdrawCorpInvestment()
        {
            Bank bank = SetUpBank();

            Account corp_invest = bank.GetAccountByNumber(3);

            corp_invest.Deposit(150.00);
            corp_invest.Withdraw(100.00);

            Assert.AreEqual(50.00, corp_invest.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Account can not be overdrawn.")]
        public void OverdrawChecking()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);

            checking.Deposit(150.00);
            checking.Withdraw(200.00);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Account can not be overdrawn.")]
        public void OverdrawIndvInvestment()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);

            indv_invest.Deposit(150.00);
            indv_invest.Withdraw(200.00);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Account can not be overdrawn.")]
        public void OverdrawCorpInvestment()
        {
            Bank bank = SetUpBank();

            Account corp_invest = bank.GetAccountByNumber(3);

            corp_invest.Deposit(150.00);
            corp_invest.Withdraw(200.00);
        }

        [TestMethod]
        public void TransferCheckingToIndvInvest()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);
            Account indv_invest = bank.GetAccountByNumber(2);

            checking.Deposit(100.00);
            bank.Transfer(50.00, checking, indv_invest);

            Assert.AreEqual(50.00, checking.Balance);
            Assert.AreEqual(50.00, indv_invest.Balance);
        }

        [TestMethod]
        public void TransferIndvInvestToChecking()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);
            Account indv_invest = bank.GetAccountByNumber(2);

            indv_invest.Deposit(100.00);
            bank.Transfer(50.00, indv_invest, checking);

            Assert.AreEqual(50.00, checking.Balance);
            Assert.AreEqual(50.00, indv_invest.Balance);
        }

        [TestMethod]
        public void TransferCheckingToCorpInvest()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);
            Account corp_invest = bank.GetAccountByNumber(3);

            checking.Deposit(100.00);
            bank.Transfer(50.00, checking, corp_invest);

            Assert.AreEqual(50.00, checking.Balance);
            Assert.AreEqual(50.00, corp_invest.Balance);
        }

        [TestMethod]
        public void TransferCorpInvestToChecking()
        {
            Bank bank = SetUpBank();

            Account checking = bank.GetAccountByNumber(1);
            Account corp_invest = bank.GetAccountByNumber(3);

            corp_invest.Deposit(100.00);
            bank.Transfer(50.00, corp_invest, checking);

            Assert.AreEqual(50.00, checking.Balance);
            Assert.AreEqual(50.00, corp_invest.Balance);
        }

        [TestMethod]
        public void TransferIndvInvestToCorpInvest()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);
            Account corp_invest = bank.GetAccountByNumber(3);

            indv_invest.Deposit(100.00);
            bank.Transfer(50.00, indv_invest, corp_invest);

            Assert.AreEqual(50.00, indv_invest.Balance);
            Assert.AreEqual(50.00, corp_invest.Balance);
        }

        [TestMethod]
        public void TransferCorpInvestToIndvInvest()
        {
            Bank bank = SetUpBank();

            Account indv_invest = bank.GetAccountByNumber(2);
            Account corp_invest = bank.GetAccountByNumber(3);

            corp_invest.Deposit(100.00);
            bank.Transfer(50.00, corp_invest, indv_invest);

            Assert.AreEqual(50.00, indv_invest.Balance);
            Assert.AreEqual(50.00, corp_invest.Balance);
        }
    }
}
