// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming
namespace DotNetCore.CAP.MySql
{
    internal class CapEFDbTransaction : IDbContextTransaction
    {
        private readonly ICapTransaction _transaction;

        public CapEFDbTransaction(ICapTransaction transaction)
        {
            _transaction = transaction;
            var dbContextTransaction = (IDbContextTransaction) _transaction.DbTransaction;
            TransactionId = dbContextTransaction.TransactionId;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public Guid TransactionId { get; }
    }
}