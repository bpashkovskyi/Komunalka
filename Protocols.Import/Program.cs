using Komunalka.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Protocols.Import;

internal class Program
{
    private const string FolderPath = @"D:\\kodlo";

    static async Task Main(string[] args)
    {
        var connectionString = "Server=tcp:bpashkovskyi.database.windows.net,1433;Initial Catalog=bpashkovskyi-db;Persist Security Info=False;User ID=bpashkovskyi;Password=Q!W@E#r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<KomunalkaContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);
        var protocolContext = new KomunalkaContext(dbContextOptionsBuilder.Options);


        var files = Directory.GetFiles(FolderPath, "*.pdf", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            var protocol = FileParser.GetProtocolNumberAndDate(file);

            if (protocol == null)
            {
                continue;
            }

            var items = FileParser.GetProtocolItems(file);

            protocol.Items = items;

            protocolContext.Protocols.Add(protocol);
        }

        await protocolContext.SaveChangesAsync();
    }
}