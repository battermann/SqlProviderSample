open System
open System.Linq
open FSharp.Data.Sql
open System.Data

let [<Literal>] ConnectionString = "Data Source=(localdb)\V11.0;Initial Catalog=Database1;Integrated Security=SSPI;"

type Sql = SqlDataProvider<
                ConnectionString = ConnectionString,
                DatabaseVendor = Common.DatabaseProviderTypes.MSSQLSERVER,
                IndividualsAmount  = 1000,
                UseOptionTypes = true>
let ctx = 
    let db = Sql.GetDataContext()
    db

type schema = Sql.dataContext

let getAllItems = 
    query { for x in ctx.Dbo.Item do
                for s3s in (!!) x.``dbo.ItemSubItem3 by Id`` do
                    for s4s in (!!) x.``dbo.ItemSubItem4 by Id`` do
                        join s3 in ctx.Dbo.SubItem3 on (s3s.SubItem3Id = s3.Id)
                        join s4 in ctx.Dbo.SubItem4 on (s4s.SubItem4Id = s4.Id)
                        select (x, s3, s4) }

[<EntryPoint>]
let main argv = 
    let items = getAllItems |> Seq.toList
    printf "%A" (items |> List.head)
    0 // return an integer exit code
