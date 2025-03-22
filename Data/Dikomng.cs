using Makara.Models;

using SQLite;

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
            if(_database != null)
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
                await Init();

                // basic validation to ensure a name was entered
                if(string.IsNullOrEmpty(ber))
                    throw new Exception("Valid Ber required");

                // Create new Beriki with default string status (was previously StatusType enum)
                result = await _database.InsertAsync(new Beriki { Ber = ber, Def = def });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, ber);
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", ber, ex.Message);
            }
        }

        // Read
        public async Task<Beriki> GetBerikiAsync(string ber)
        {
            try
            {
                await Init();

                // basic validation to ensure a name was entered
                if(string.IsNullOrEmpty(ber))
                    throw new Exception("Valid Ber required");

                return await _database.Table<Beriki>().Where(b => b.Ber == ber).FirstOrDefaultAsync();
            }
            catch(Exception ex)
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
                await Init();

                return await _database.Table<Beriki>().ToListAsync();
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to get {0}. Error: {1}", "any beriki", ex.Message);
                return null;
            }
        }

        // Filter by Status
        public async Task<List<Beriki>> GetBerikisByStatusAsync(string status)
        {
            try
            {
                await Init();

                return await _database.Table<Beriki>()
                    .Where(b => b.Status == status)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to get berikis by status. Error: {0}", ex.Message);
                return null;
            }
        }

        // Update
        public async Task<int> UpdateBerikiAsync(Beriki beriki)
        {
            try
            {
                await Init();

                // basic validation to ensure a name was entered
                if(beriki == null)
                    throw new Exception("Valid Beriki required");

                return await _database.UpdateAsync(beriki);
            }
            catch(Exception ex)
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
                await Init();

                // basic validation to ensure a name was entered
                if(beriki == null)
                    throw new Exception("Valid Beriki required");

                return await _database.DeleteAsync(beriki);
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to delete {0}. Error: {1}", "the beriki", ex.Message);
                return -1;
            }
        }

        // Set Status
        public async Task<int> SetBerikiStatusAsync(string ber, string status)
        {
            try
            {
                await Init();

                var beriki = await GetBerikiAsync(ber);
                if(beriki == null)
                    throw new Exception("Beriki not found");

                beriki.Status = status;
                return await UpdateBerikiAsync(beriki);
            }
            catch(Exception ex)
            {
                StatusMessage = string.Format("Failed to update status. Error: {0}", ex.Message);
                return -1;
            }
        }
    }
}