﻿using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Makara.Models;
using System.Runtime.ConstrainedExecution;

namespace Makara.Data
{
    public class Dikomng
    {
        string _dbPath = "";

        public string StatusMessage { get; set; }
        private SQLiteAsyncConnection _database;

        public Dikomng(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task Init()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath);

            await _database.CreateTableAsync<Beriki>();
        }

        // Create
        public async Task AddBerikiAsync(string ber, string def)
        {
            int result = 0;
            try
            {
                // TODO: Call Init()
                await Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(ber))
                    throw new Exception("Valid Ber required");

                // TODO: Insert the new person into the database
                result = await _database.InsertAsync(new Beriki { Ber = ber, Def = def });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, ber);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", ber, ex.Message);
            }

        }

        // Read
        public async Task<Beriki> GetBerikiAsync(string ber)
        {
            try
            {
                // TODO: Call Init()
                await Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(ber))
                    throw new Exception("Valid Ber required");

                return await _database.Table<Beriki>().Where(b => b.Ber == ber).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get {0}. Error: {1}", ber, ex.Message);
                return null;
            }
        }

        // Read All
        public async Task<List<Beriki>> GetAllBerikisAsync()
        {
            try
            {
                // TODO: Call Init()
                await Init();

                return await _database.Table<Beriki>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to get {0}. Error: {1}", "any beriki", ex.Message);
                return null;
            }
            
        }

        // Update
        public async Task<int> UpdateBerikiAsync(Beriki beriki)
        {
            try
            {
                // TODO: Call Init()
                await Init();

                // basic validation to ensure a name was entered
                if (beriki == null)
                    throw new Exception("Valid Beriki required");

                return await _database.UpdateAsync(beriki);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update {0}. Error: {1}", "the beriki", ex.Message);
                return -1;
            }            
        }

        // Delete
        public async Task<int> DeleteBerikiAsync(Beriki beriki)
        {
            try
            {
                // TODO: Call Init()
                await Init();

                // basic validation to ensure a name was entered
                if (beriki == null)
                    throw new Exception("Valid Beriki required");

                return await _database.DeleteAsync(beriki);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete {0}. Error: {1}", "the beriki", ex.Message);
                return -1;
            }

        }
    }
}