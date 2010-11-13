namespace FSharpDataAccess

open System.Data
open System.Data.SqlClient
open System.Configuration

type MyRecord =
    { Id : int
      Name : string }

module DataAccess =
    let GetSomeData id = seq { 
        // Note: A connection string named "Default" needs to be in the config file of the application
        let connectionString = ConfigurationManager.ConnectionStrings.["Default"].ConnectionString
        // Create a command to call SQL stored procedure
        use connection = new SqlConnection(connectionString) 
        // Replace "SomeStoredProcName" with the desired stored procedure name
        use command = new SqlCommand("SomeStoredProcName", connection)
        command.CommandType <- CommandType.StoredProcedure
        // Add parameters to the stored procedure call
        command.Parameters.Add(SqlParameter("@ID", id)) |> ignore  
        // Run the command and read results into an F# record
        connection.Open()
        use reader = command.ExecuteReader()
        while reader.Read() do
            yield { Id = unbox (reader.["ID"])
                    Name = unbox (reader.["Name"])} }
