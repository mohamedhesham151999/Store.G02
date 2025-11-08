using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Contracts
{
    public interface IDbInitializer
    {
        #region Summary
        // Task cuz of Asynchronous  => this function initialization of db to create database and apply any migrations that are not applied yet (Update-Database) and then seed data 
        // concept of seeding => you have data stored in somewhere. you take this data and put it inside the created database, this operation happens only once
        #endregion
        Task InitializerAsync();
        Task InitializeIdentityAsync();

    }
}
